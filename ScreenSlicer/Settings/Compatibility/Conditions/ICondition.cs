using ScreenSlicer.Native.Windows;
using System;
using System.ComponentModel;

namespace ScreenSlicer.Compatibility
{
    public interface ICondition : INotifyPropertyChanged, ICloneable
    {
        bool Check(ISystemWindow window);
    }
}
