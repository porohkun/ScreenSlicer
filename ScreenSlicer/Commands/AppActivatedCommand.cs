using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;

namespace ScreenSlicer.Commands
{
    public class AppActivatedCommand : InjectableCommand
    {
        protected override bool CanExecuteInternal(object parameter)
        {
            return true;
        }

        protected override void ExecuteInternal(object parameter)
        {

        }
    }
}
