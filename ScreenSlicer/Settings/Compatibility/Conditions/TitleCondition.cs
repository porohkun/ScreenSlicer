using ScreenSlicer.Native.Windows;

namespace ScreenSlicer.Compatibility
{
    public class TitleCondition : StringCondition
    {
        protected override string GetValue(ISystemWindow window)
        {
            return window.Title;
        }
    }
}
