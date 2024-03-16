using System;
using System.Runtime.InteropServices;
using System.Text;

namespace MSTeams.ScreenSharePopupHider.Helpers
{
    public static class Win32
    {
        [DllImport("user32.dll")] public static extern bool IsWindowVisible(IntPtr hwnd);
        [DllImport("user32.dll")] public static extern IntPtr FindWindowEx(IntPtr parentHandle, IntPtr childAfter, string className, string? windowTitle);

        [DllImport("user32.dll")] private static extern int GetWindowTextLength(IntPtr hwnd);
        [DllImport("user32.dll")] private static extern int GetWindowText(IntPtr hwnd, StringBuilder lpString, int nMaxCount);
        [DllImport("user32.dll")] private static extern int GetWindowLong(IntPtr hWnd, int nIndex);
        [DllImport("user32.dll")] private static extern int SetWindowLong(IntPtr hWnd, int nIndex, int dwNewLong);
        [DllImport("user32.dll")] private static extern bool ShowWindow(IntPtr hwnd, int nCmdShow);

        public static string GetWindowText(IntPtr hwnd)
        {
            int length = GetWindowTextLength(hwnd);
            if (length <= 0)
            {
                return string.Empty;
            }

            var buffer = new StringBuilder(length + 1);
            GetWindowText(hwnd, buffer, buffer.Capacity);
            return buffer.ToString();
        }

        public static void HideFromTaskbar(IntPtr hwnd)
        {
            const int GWL_EXSTYLE = -20;
            const int WS_EX_TOOLWINDOW = 0x80;

            var extendedStyleFlags = GetWindowLong(hwnd, GWL_EXSTYLE);
            extendedStyleFlags |= WS_EX_TOOLWINDOW;

            SetWindowLong(hwnd, GWL_EXSTYLE, extendedStyleFlags);
        }

        public static void ShowWindow(IntPtr hwnd)
        {
            const int SW_SHOW = 0x05;
            ShowWindow(hwnd, SW_SHOW);
        }

        public static void HideWindow(IntPtr hwnd)
        {
            const int SW_HIDE = 0x00;
            ShowWindow(hwnd, SW_HIDE);
        }
    }
}
