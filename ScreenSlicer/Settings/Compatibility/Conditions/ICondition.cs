using ScreenSlicer.Native.Windows;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScreenSlicer.Compatibility
{
    public interface ICondition
    {
        bool Check(ISystemWindow window);
    }
}
