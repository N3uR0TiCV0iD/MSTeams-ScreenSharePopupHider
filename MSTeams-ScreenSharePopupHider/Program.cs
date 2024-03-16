using System;
using System.Drawing;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Win32;
using MSTeams.ScreenSharePopupHider.Helpers;

namespace MSTeams.ScreenSharePopupHider
{
    internal static class Program
    {
        static volatile bool applicationExit;

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        private static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            SingleInstanceHelper.CheckAndRun(Startup, SingleInstanceAction.ShowMessage);
        }

        private static void Startup()
        {
            CheckAutoStartup();
            StartApplication();
        }

        private static void CheckAutoStartup()
        {
            using (var appRegistryKey = Registry.CurrentUser.CreateSubKey("Software\\MSTeamsSSPH", true))
            {
                AutoStartupHelper.CheckAutoStartup(appRegistryKey, "MSTeamsSSPH");
            }
        }

        private static void StartApplication()
        {
            var appAssembly = Assembly.GetEntryAssembly()!;
            var startupForm = new StartupForm($"{appAssembly.GetName().Name} is now running!", Color.FromArgb(25, 30, 40), 1500, 500);
            startupForm.Show();

            var notifyIcon = new AppNotifyIcon();
            _ = CheckForSharePopup(notifyIcon.TrayIcon);

            Application.Run();

            notifyIcon.Dispose();

            applicationExit = true;
        }

        private static async Task CheckForSharePopup(NotifyIcon trayIcon)
        {
            const int nextCheckDelay = 3000;
            while (!applicationExit)
            {
                await Task.Delay(nextCheckDelay);

                var controlBarWindowHandle = Win32.FindWindow("TeamsWebView", "Sharing control bar | Microsoft Teams");
                if (controlBarWindowHandle != IntPtr.Zero && Win32.IsWindowVisible(controlBarWindowHandle))
                {
                    trayIcon.ShowBalloonTip(5000, "MSTeams-ScreenSharePopupHider", "Dealt with that pesky popup for you! :)", ToolTipIcon.Info);
                    Win32.HideWindow(controlBarWindowHandle);
                }
            }
        }
    }
}
