using ScreenSlicer.Windows;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;

namespace ScreenSlicer.Commands
{
    public class ShowWindowCommand<T> : InjectableCommand<ShowWindowCommand<T>> where T : Window
    {
        protected T _window;

        public ShowWindowCommand(T window)
        {
            _window = window;
        }

        protected override bool CanExecuteInternal(object parameter)
        {
            return !_window.IsActive;
        }

        protected override void ExecuteInternal(object parameter)
        {
            if (_window is IParametricWindow parametricWindow)
                parametricWindow.SetParameter(parameter);
            _window.Show();
        }
    }
}
