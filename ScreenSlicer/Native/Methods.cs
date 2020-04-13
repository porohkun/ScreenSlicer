using ScreenSlicer.Native.Screens;
using System;
using System.Runtime.InteropServices;

namespace ScreenSlicer.Native
{
    public static class Methods
    {
        public delegate bool EnumMonitorsDelegate(IntPtr hMonitor, IntPtr hdcMonitor, NativeRectangle rect, IntPtr dwData);

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool EnumDisplayMonitors(IntPtr hdc, IntPtr lprcClip, EnumMonitorsDelegate lpfnEnum, IntPtr dwData);

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool GetMonitorInfo(IntPtr hmonitor, [In, Out] MonitorInfo info);

    }
}
