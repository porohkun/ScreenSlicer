using ScreenSlicer.Native.Windows;

namespace ScreenSlicer.Compatibility
{
    public class TitleCondition : StringCondition
    {
        public override object Clone()
        {
            return new TitleCondition()
            {
                RegularExpression = RegularExpression,
                TargetValue = TargetValue
            };
        }

        protected override string GetValue(ISystemWindow window)
        {
            return window.Title;
        }
    }
}
