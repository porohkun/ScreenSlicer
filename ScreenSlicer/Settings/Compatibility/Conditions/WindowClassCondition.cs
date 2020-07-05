using ScreenSlicer.Native.Windows;

namespace ScreenSlicer.Compatibility
{
    public class WindowClassCondition : StringCondition
    {
        protected override string GetValue(ISystemWindow window)
        {
            return window.WindowClass;
        }
    }
}
