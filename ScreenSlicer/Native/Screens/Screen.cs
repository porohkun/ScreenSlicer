using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScreenSlicer.Native.Screens
{
    public class Screen
    {
        private static List<Screen> Screens = null;

        public bool IsPrimary { get; private set; }

        public Rectangle Rect { get; private set; }
        public Rectangle WorkspaceRect { get; private set; }

        public Screen(bool primary, Rectangle rect, Rectangle workspaceRect)
        {
            IsPrimary = primary;
            Rect = rect;
            WorkspaceRect = workspaceRect;
        }

        public static Screen[] GetScreens()
        {
            Screens = new List<Screen>();

            Methods.EnumDisplayMonitors(IntPtr.Zero, IntPtr.Zero, MonitorEnumProc, IntPtr.Zero);

            return Screens.ToArray();
        }

        private static bool MonitorEnumProc(IntPtr hMonitor, IntPtr hdcMonitor, NativeRectangle rect, IntPtr dwData)
        {
            var info = new MonitorInfo();

            if (Methods.GetMonitorInfo(hMonitor, info))
            {
                Screens.Add(new Screen(
                    (info.dwFlags & 1) == 1, // 1 = primary monitor
                    info.rcMonitor,
                    info.rcWork));
            }

            return true;
        }
    }
}
