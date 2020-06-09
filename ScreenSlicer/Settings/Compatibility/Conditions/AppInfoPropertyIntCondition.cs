using ScreenSlicer.Native.Windows;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScreenSlicer.Compatibility
{
    public class AppInfoPropertyIntCondition : IntCondition
    {
        protected override int GetValue(ISystemWindow window)
        {
            throw new NotImplementedException();
        }
    }
}
