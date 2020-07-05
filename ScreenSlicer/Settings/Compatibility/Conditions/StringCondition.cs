using ScreenSlicer.Native.Windows;
using System.Text.RegularExpressions;

namespace ScreenSlicer.Compatibility
{
    public abstract class StringCondition : ConditionBase<string>
    {
        private bool _regularExpression;

        public bool RegularExpression
        {
            get => _regularExpression;
            set
            {
                if (_regularExpression != value)
                {
                    _regularExpression = value;
                    NotifyPropertyChanged(nameof(RegularExpression));
                }
            }
        }

        public override bool Check(ISystemWindow window)
        {
            if (string.IsNullOrWhiteSpace(TargetValue))
                return true;
            return RegularExpression ? Regex.IsMatch(GetValue(window), TargetValue) : GetValue(window) == TargetValue;
        }
    }
}
