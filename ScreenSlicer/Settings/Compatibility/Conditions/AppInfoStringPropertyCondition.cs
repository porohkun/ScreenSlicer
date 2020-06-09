using ScreenSlicer.Native.Windows;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScreenSlicer.Compatibility
{
    public class AppInfoStringPropertyCondition : StringCondition
    {
        protected override string GetValue(ISystemWindow window)
        {
            throw new NotImplementedException();
        }
    }
}
