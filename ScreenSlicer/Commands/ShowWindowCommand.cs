using ScreenSlicer.Windows;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;

namespace ScreenSlicer.Commands
{
    /// <summary>
    /// Parameter means "show window as dialog"
    /// </summary>
    public class ShowWindowCommand<T> : InjectableCommand<ShowWindowCommand<T>, bool> where T : Window
    {
        protected T _window;

        public ShowWindowCommand(T window)
        {
            _window = window;
        }

        protected override bool CanExecuteInternal(bool parameter)
        {
            return !_window.IsActive;
        }

        protected override void ExecuteInternal(bool parameter)
        {
            if (_window is IParametricWindow parametricWindow)
                parametricWindow.SetParameter(parameter);
            if (parameter)
                _window.ShowDialog();
            else
                _window.Show();
        }
    }
}
