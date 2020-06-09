using ScreenSlicer.Native;
using ScreenSlicer.Native.Windows;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScreenSlicer.Compatibility
{
    public class FilenameCondition : StringCondition
    {
        protected override string GetValue(ISystemWindow window)
        {
            return window.ProcessName;
        }
    }
}
