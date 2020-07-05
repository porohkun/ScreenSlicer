using ScreenSlicer.Native.Windows;

namespace ScreenSlicer.Compatibility
{
    public abstract class IntCondition : ConditionBase<int>
    {
        public sealed override bool Check(ISystemWindow window)
        {
            return TargetValue == GetValue(window);
        }
    }
}
