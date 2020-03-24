using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;

namespace ScreenSlicer.Commands
{
    public abstract class InjectableCommand : ICommand
    {
        public void Execute(object parameter)
        {
            ExecuteInternal(parameter);
        }
        protected abstract void ExecuteInternal(object parameter);

        public bool CanExecute(object parameter)
        {
            return CanExecuteInternal(parameter);
        }
        protected abstract bool CanExecuteInternal(object parameter);

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }
    }
}
