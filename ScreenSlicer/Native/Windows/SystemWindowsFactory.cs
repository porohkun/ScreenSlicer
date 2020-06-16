using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScreenSlicer.Native.Windows
{
    public class SystemWindowsFactory
    {


        public ISystemWindow Create(IntPtr handle)
        {
            var window = new SystemWindow(handle);
            var rule = Settings.Instance.Compatibility.GetRuleForWindow(window);

            window.SetRule(rule);

            return new ExplorerWindow(handle);
        }


    }
}
