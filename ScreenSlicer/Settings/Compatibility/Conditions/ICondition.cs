using ScreenSlicer.Native.Windows;
using System.ComponentModel;

namespace ScreenSlicer.Compatibility
{
    public interface ICondition : INotifyPropertyChanged
    {
        bool Check(ISystemWindow window);
    }
}
