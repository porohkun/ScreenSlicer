﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;

namespace ScreenSlicer.Commands
{
    public class ShowWindowCommand<T> : InjectableCommand where T : Window
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
            _window.Show();
        }
    }
}
