using NuGet;
using ScreenSlicer.Native.Windows;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Automation;

namespace ScreenSlicer.Managers
{
    public class ProcessesWatcher
    {
        public ObservableCollection<ISystemWindow> Windows { get; private set; }

        public ProcessesWatcher()
        {
            Windows = new ObservableCollection<ISystemWindow>();
            Windows.AddRange(GetDesktopWindows());


            //var processes = Process.GetProcesses()
            //    .Where(p => (long)p.MainWindowHandle != 0);
            //foreach (var process in processes)
            //{
            //    AutomationElement windowElement = AutomationElement.FromHandle(process.MainWindowHandle);
            //    if (windowElement != null)
            //    {
            //        Automation.AddAutomationPropertyChangedEventHandler(
            //                windowElement,
            //                System.Windows.Automation.TreeScope.Element, this.handlePropertyChange,
            //                System.Windows.Automation.AutomationElement.BoundingRectangleProperty);
            //    }
            //}
        }

        public IEnumerable<ISystemWindow> GetDesktopWindows()
        {
            var hWnds = new List<IntPtr>();
            Native.Methods.EnumDesktopWindows(IntPtr.Zero,
                (hWnd, lParam) =>
                {
                    hWnds.Add(hWnd);
                    return true;
                },
                IntPtr.Zero);
            return hWnds.Select(h => new SystemWindow(h));
        }

        //private void handlePropertyChange(object src, AutomationPropertyChangedEventArgs e)
        //{
        //    if (e.Property == System.Windows.Automation.AutomationElement.BoundingRectangleProperty)
        //    {
        //        //System.Windows.Rect rectangle = e.NewValue as System.Windows.Rect;
        //        //Do other stuff here
        //    }
        //}

    }
}
