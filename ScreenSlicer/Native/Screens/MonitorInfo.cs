using System.Runtime.InteropServices;

namespace ScreenSlicer.Native.Screens
{
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto, Pack = 4)]
    public class MonitorInfo
    {
        public int cbSize = Marshal.SizeOf(typeof(MonitorInfo));
        public NativeRectangle rcMonitor = new NativeRectangle();
        public NativeRectangle rcWork = new NativeRectangle();
        public int dwFlags;
    }
}
