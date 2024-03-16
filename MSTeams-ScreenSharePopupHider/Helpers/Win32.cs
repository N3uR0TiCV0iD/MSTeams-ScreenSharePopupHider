using System;
using System.Runtime.InteropServices;

namespace MSTeams.ScreenSharePopupHider.Helpers
{
    public static class Win32
    {
        [DllImport("user32.dll")] public static extern bool IsWindowVisible(IntPtr hwnd);
        [DllImport("user32.dll")] public static extern IntPtr FindWindow(string? lpClassName, string? lpWindowName);

        [DllImport("user32.dll")] private static extern bool ShowWindow(IntPtr hwnd, int nCmdShow);

        public static void HideWindow(IntPtr hwnd)
        {
            const int SW_HIDE = 0;
            ShowWindow(hwnd, SW_HIDE);
        }
    }
}
