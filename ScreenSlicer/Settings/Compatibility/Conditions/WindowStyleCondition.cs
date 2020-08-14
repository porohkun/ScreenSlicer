using ScreenSlicer.Native;
using ScreenSlicer.Native.Windows;

namespace ScreenSlicer.Compatibility
{
    public class WindowStyleCondition : ConditionBase<WindowStyle>
    {
        private bool _mustInclude;

        public bool MustInclude
        {
            get => _mustInclude;
            set
            {
                if (_mustInclude != value)
                {
                    _mustInclude = value;
                    NotifyPropertyChanged(nameof(MustInclude));
                }
            }
        }

        public override object Clone()
        {
            return new WindowStyleCondition()
            {
                MustInclude = MustInclude,
                TargetValue = TargetValue
            };
        }

        protected override WindowStyle GetValue(ISystemWindow window)
        {
            return window.Style;
        }

        public override bool Check(ISystemWindow window)
        {
            return MustInclude == GetValue(window).HasFlag(TargetValue);
        }
    }
}
