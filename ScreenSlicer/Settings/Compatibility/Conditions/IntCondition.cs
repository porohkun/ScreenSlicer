using ScreenSlicer.Native.Windows;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScreenSlicer.Compatibility
{
    public abstract class IntCondition : ICondition
    {
        public int TargetValue { get; set; }

        public bool Check(ISystemWindow window)
        {
            return TargetValue == GetValue(window);
        }

        protected abstract int GetValue(ISystemWindow window);
    }
}
