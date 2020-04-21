using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;

namespace ScreenSlicer.Commands
{
    public class ExitCommand : InjectableCommand
    {
        protected override bool CanExecuteInternal(object parameter)
        {
            return true;
        }

        protected override void ExecuteInternal(object parameter)
        {
            var code = 0;
            if (parameter is int)
                code = (int)parameter;
            else if (parameter is int?)
                code = ((int?)parameter).GetValueOrDefault(0);

            Application.Current.Shutdown(code);
        }
    }
}
