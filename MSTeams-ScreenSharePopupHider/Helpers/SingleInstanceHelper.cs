using System;
using System.Diagnostics;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Threading;

namespace MSTeams.ScreenSharePopupHider.Helpers
{
    public enum SingleInstanceAction
    {
        None = 0,
        FocusApp = 1,
        ShowMessage = 2
    }

    public static class SingleInstanceHelper
    {
        [DllImport("user32.dll")] private static extern bool SetForegroundWindow(IntPtr hwnd);
        [DllImport("user32.dll")] private static extern bool ShowWindowAsync(IntPtr hwnd, int nCmdShow);
        [DllImport("user32.dll")] private static extern int MessageBox(IntPtr hwnd, string text, string caption, uint type);

        const int SW_RESTORE = 9;
        const uint MB_ICONINFORMATION = 0x40;

        public static void CheckAndRun(Action startupFunc, SingleInstanceAction duplicateAction)
        {
            var appAssembly = Assembly.GetEntryAssembly()!;
            var projectGuid = GetAssemblyGuid(appAssembly);
            if (projectGuid == null)
            {
                throw new InvalidOperationException("Project GUID not found.");
            }

            var mutexName = $"{{{projectGuid}}}";
            var appMutex = new Mutex(true, mutexName); //NOTE: System wide mutex!!!
            if (appMutex.WaitOne(TimeSpan.Zero, true))
            {
                //Ok, we got the lock
                startupFunc();
                appMutex.ReleaseMutex();
            }
            else if (duplicateAction == SingleInstanceAction.FocusApp)
            {
                AttemptAppFocus(projectGuid);
            }
            else if (duplicateAction == SingleInstanceAction.ShowMessage)
            {
                //Utilize the native method to avoid having a dependency with "System.Windows.Forms" (in case of a console app)
                MessageBox(IntPtr.Zero, "Only one instance may be running", "Only one instance allowed", MB_ICONINFORMATION);
            }
        }

        private static void AttemptAppFocus(string? projectGUID)
        {
            var myProcess = Process.GetCurrentProcess();
            foreach (var process in Process.GetProcesses())
            {
                if (IsSameApp(process, projectGUID) && process.Id != myProcess.Id)
                {
                    var mainWindowHandle = process.MainWindowHandle;
                    ShowWindowAsync(mainWindowHandle, SW_RESTORE);
                    SetForegroundWindow(mainWindowHandle);
                    break;
                }
            }
        }

        private static bool IsSameApp(Process process, string? projectGUID)
        {
            var processAssembly = GetProcessAssembly(process);
            if (processAssembly != null)
            {
                try
                {
                    return GetAssemblyGuid(processAssembly) == projectGUID;
                }
                catch
                {
                }
            }
            return false;
        }

        private static Assembly? GetProcessAssembly(Process process)
        {
            try
            {
                return Assembly.LoadFile(process.MainModule!.FileName!);
            }
            catch
            {
                return null;
            }
        }

        private static string? GetAssemblyGuid(Assembly assembly)
        {
            var attributeList = assembly.GetCustomAttributes(typeof(GuidAttribute), false);
            if (attributeList.Length == 0)
            {
                return null;
            }

            var guidAttribute = (GuidAttribute)attributeList[0];
            return guidAttribute.Value;
        }
    }
}
