using ScreenSlicer.Native;
using ScreenSlicer.Native.Windows;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Timers;
using System.Windows.Automation;
using System.Windows.Input;
using SendOrPostCallback = System.Threading.SendOrPostCallback;

namespace ScreenSlicer.Managers
{
    public class ProcessesWatcher
    {
        private readonly HashSet<ISystemWindow> _windows = new HashSet<ISystemWindow>();

        private readonly RegionsManager _regionsManager;
        private readonly Timer _syncTimer;
        private readonly AsyncOperation _asyncOperation;
        private readonly SendOrPostCallback _tickReporter;

        public ProcessesWatcher(RegionsManager regionsManager)
        {
            _regionsManager = regionsManager;

            _asyncOperation = AsyncOperationManager.CreateOperation((object)null);
            _tickReporter = new SendOrPostCallback(SyncedTick);

            SyncWindowsSet();

            _syncTimer = new Timer { Interval = 2000 };
            _syncTimer.Elapsed += SyncTick;
            _syncTimer.AutoReset = true;
            _syncTimer.Enabled = true;
        }

        private void SyncTick(object source, ElapsedEventArgs e)
        {
            _asyncOperation.Post(_tickReporter, null);
        }

        private void SyncedTick(object arg)
        {
            SyncWindowsSet();
        }

        private void SyncWindowsSet()
        {
            var oldWindowsSet = new HashSet<ISystemWindow>(_windows);
            var newWindowsSet = new HashSet<ISystemWindow>(GetDesktopWindows().Where(w => FilterWindows(w)));

            foreach (var win in oldWindowsSet)
                if (!newWindowsSet.Remove(win))
                {
                    _windows.Remove(win);
                    OnWindowClosed(win);
                }

            foreach (var win in newWindowsSet)
                if (_windows.Add(win))
                    OnWindowOpened(win);
        }

        private void OnWindowClosed(ISystemWindow window)
        {
            try
            {
                var windowElement = AutomationElement.FromHandle(window.Handle);
                if (windowElement != null)
                {
                    Automation.RemoveAutomationPropertyChangedEventHandler(windowElement, Window_VisualStateChanged);
                }
            }
            catch { }
        }

        private void OnWindowOpened(ISystemWindow window)
        {
            try
            {
                var windowElement = AutomationElement.FromHandle(window.Handle);
                if (windowElement != null)
                {
                    //Automation.AddAutomationPropertyChangedEventHandler(
                    //    windowElement,
                    //    TreeScope.Element, this.handlePropertyChange,
                    //    AutomationElement.ItemStatusProperty);
                    Automation.AddAutomationPropertyChangedEventHandler(windowElement, TreeScope.Element, Window_VisualStateChanged, WindowPattern.WindowVisualStateProperty);
                }
            }
            catch { }
        }

        private bool FilterWindows(ISystemWindow window)
        {
            if (!window.Visible)
                return false;
            if (!window.Style.HasFlag(WindowStyle.PopupWindow)
                && window.Style.HasFlag(WindowStyle.Popup))
                return false;
            return true;
        }

        public IEnumerable<ISystemWindow> GetDesktopWindows()
        {
            var hWnds = new List<IntPtr>();
            Methods.EnumDesktopWindows(IntPtr.Zero, (hWnd, lParam) =>
            {
                hWnds.Add(hWnd);
                return true;
            }, IntPtr.Zero);
            return hWnds.Select(h => new SystemWindow(h));
        }

        private void Window_VisualStateChanged(object src, AutomationPropertyChangedEventArgs e)
        {
            if (src is AutomationElement automation)
            {
                if (e.NewValue.Equals(WindowVisualState.Maximized) && !Methods.GetAsyncKeyState(Keys.ShiftKey).HasFlag(AsyncKeyStateResult.Pressed))
                {
                    var handle = (IntPtr)(int)automation.GetCurrentPropertyValue(AutomationElement.NativeWindowHandleProperty);
                    var window = _windows.FirstOrDefault(w => w.Handle.Equals(handle));

                    MoveToRectangle(window, SelectSuitableRegion(window));

                }
            }
        }

        private Rectangle SelectSuitableRegion(ISystemWindow window)
        {
            var rect = window.ClientRectangle;
            //var point = new Point(rect.Top, rect.Right);
            if (Methods.GetCursorPos(out NativePoint point))
            {
                var region = _regionsManager.Regions.FirstOrDefault(r => r.Contains((int)point.X, (int)point.Y));
                return region;
            }
            return new Rectangle(200, 200, 800, 600);
        }

        private void MoveToRectangle(ISystemWindow window, Rectangle region)
        {
            //    window.Move(region);
            window.PostMessage(WindowMessage.EnterSizeMove, IntPtr.Zero, IntPtr.Zero);
            Methods.ShowWindow(window.Handle, ShowWindowCommand.ShowNormal);
            window.SetPosition(region, ShowWindowPosition.NoSendChanging | ShowWindowPosition.NoZOrder, (ISystemWindow)null);
            window.PostMessage(WindowMessage.ExitSizeMove, IntPtr.Zero, IntPtr.Zero);
        }

    }
}
