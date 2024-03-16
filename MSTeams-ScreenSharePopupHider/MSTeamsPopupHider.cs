using System;
using System.Threading.Tasks;
using System.Windows.Forms;
using MSTeams.ScreenSharePopupHider.Helpers;

namespace MSTeams.ScreenSharePopupHider
{
    public class MSTeamsPopupHider
    {
        const string WINDOW_CLASSNAME = "TeamsWebView";
        const int CHECK_DELAY = 3000;

        bool running;

        public HideBehaviour ParticipantsHideBehaviour { get; set; }

        public void StartMonitoring(NotifyIcon trayIcon)
        {
            _ = CheckForPopups(trayIcon);
        }

        private async Task CheckForPopups(NotifyIcon trayIcon)
        {
            running = true;
            while (running)
            {
                await Task.Delay(CHECK_DELAY);

                var currWindowHandle = Win32.FindWindowEx(IntPtr.Zero, IntPtr.Zero, WINDOW_CLASSNAME, null);
                while (currWindowHandle != IntPtr.Zero)
                {
                    HandleMSTeamsWindow(currWindowHandle, trayIcon);
                    currWindowHandle = Win32.FindWindowEx(IntPtr.Zero, currWindowHandle, WINDOW_CLASSNAME, null);
                }
            }
        }

        private void HandleMSTeamsWindow(IntPtr hwnd, NotifyIcon trayIcon)
        {
            IntPtr titleBarHandle = Win32.FindWindowEx(hwnd, IntPtr.Zero, "TeamsTitleBarOverlay", null);
            if (titleBarHandle != IntPtr.Zero)
            {
                //It is the MAIN MSTeams window. Do nothing
                return;
            }

            var windowText = Win32.GetWindowText(hwnd);
            if (windowText == "Sharing control bar | Microsoft Teams")
            {
                if (Win32.IsWindowVisible(hwnd))
                {
                    trayIcon.ShowBalloonTip(5000, "MSTeams-ScreenSharePopupHider", "Dealt with that pesky popup for you! :)", ToolTipIcon.Info);
                    Win32.HideWindow(hwnd);
                }
                return;
            }

            HandleParticipantsWindow(hwnd);
        }

        private void HandleParticipantsWindow(IntPtr hwnd)
        {
            var hideBehaviour = this.ParticipantsHideBehaviour;
            if (hideBehaviour == HideBehaviour.HideCompletely)
            {
                Win32.HideWindow(hwnd);
                return;
            }
            if (hideBehaviour == HideBehaviour.HideFromTaskbar)
            {
                Win32.HideFromTaskbar(hwnd);
            }
        }

        public void StopMonitoring()
        {
            running = false;
        }
    }
}
