using ScreenSlicer.Native.Windows;

namespace ScreenSlicer.Compatibility
{
    public class WindowClassCondition : StringCondition
    {
        public override object Clone()
        {
            return new WindowClassCondition()
            {
                RegularExpression = RegularExpression,
                TargetValue = TargetValue
            };
        }

        protected override string GetValue(ISystemWindow window)
        {
            return window.WindowClass;
        }
    }
}
