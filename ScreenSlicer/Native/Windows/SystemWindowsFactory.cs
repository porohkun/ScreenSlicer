using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScreenSlicer.Native.Windows
{
    public class SystemWindowsFactory
    {


        public ISystemWindow Create(IntPtr handle, bool setRule = false)
        {
            var window = new SystemWindow(handle);

            //if (setRule)
            //    SetRule(window);

            return window;
        }

        //public void SetRule(ISystemWindow window)
        //{
        //    if (window is ICanUseRules ruledWindow)
        //        ruledWindow.SetRule(Settings.Instance.Compatibility.GetRuleForWindow(window));
        //}
    }
}
