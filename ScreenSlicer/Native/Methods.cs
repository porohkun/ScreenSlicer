using ScreenSlicer.Native.Screens;
using System;
using System.Runtime.InteropServices;
using System.Text;

namespace ScreenSlicer.Native
{
    public static class Methods
    {
        public delegate bool EnumMonitorsDelegate(IntPtr hMonitor, IntPtr hdcMonitor, NativeRectangle rect, IntPtr dwData);
        public delegate bool EnumWindowsDelegate(IntPtr hWnd, int lParam);

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool EnumDisplayMonitors(IntPtr hdc, IntPtr lprcClip, EnumMonitorsDelegate lpfnEnum, IntPtr dwData);

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool GetMonitorInfo(IntPtr hmonitor, [In, Out] MonitorInfo lpmi);

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool EnumDesktopWindows(IntPtr hDesktop, EnumWindowsDelegate lpfn, IntPtr lParam);

        [DllImport("user32.dll")]
        public static extern IntPtr GetAncestor(IntPtr hwnd, GetAncestorFlag gaFlags);

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool GetWindowRect(IntPtr hWnd, out NativeRectangle lpRect);

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool GetClientRect(IntPtr hWnd, out NativeRectangle lpRect);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern int GetWindowText(IntPtr hWnd, StringBuilder lpString, int nMaxCount);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern int GetWindowTextLength(IntPtr hWnd);

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool IsWindowVisible(IntPtr hWnd);

        [DllImport("user32.dll", SetLastError = true)]
        private static extern long GetWindowLong(IntPtr hWnd, GetWindowLongOffset nIndex);
        public static WindowStyle GetWindowStyle(IntPtr hWnd) => (WindowStyle)GetWindowLong(hWnd, GetWindowLongOffset.Style);

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool GetWindowPlacement(IntPtr hWnd, ref WindowPlacement lpwndpl);

        [DllImport("user32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool PostMessage(IntPtr windowHandle, uint message, IntPtr parameterW, IntPtr parameterL);

        [DllImport("user32.dll", SetLastError = true)]
        public static extern IntPtr SendMessage(IntPtr windowHandle, uint message, IntPtr parameterW, IntPtr parameterL);

        [DllImport("user32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool SetWindowPos(
            IntPtr windowHandle,
            IntPtr insertAfterWindowHandle,
            int x,
            int y,
            int width,
            int height,
            uint flags);

        [DllImport("user32.dll")]
        public static extern AsyncKeyStateResult GetAsyncKeyState(Keys vKey);

        [DllImport("user32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool GetCursorPos(out NativePoint lpPoint);
    }
}
