using ScreenSlicer.Native.Windows;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScreenSlicer.Compatibility
{
    public abstract class StringCondition : ICondition
    {
        public string TargetValue { get; set; }

        public bool Check(ISystemWindow window)
        {
            if (string.IsNullOrWhiteSpace(TargetValue))
                return true;
            return TargetValue == GetValue(window);
        }

        protected abstract string GetValue(ISystemWindow window);
    }
}
