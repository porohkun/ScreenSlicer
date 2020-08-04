using System;

namespace ScreenSlicer.Commands
{
    public class DelegateCommand : InjectableCommand<DelegateCommand>
    {
        private Action<object> _executeAction;
        private Func<object, bool> _canExecuteAction;

        public DelegateCommand(Action<object> executeAction, Func<object, bool> canExecuteAction = null)
        {
            _executeAction = executeAction;
            _canExecuteAction = canExecuteAction;
        }

        protected override bool CanExecuteInternal(object parameter)
        {
            return _canExecuteAction?.Invoke(parameter) ?? true;
        }

        protected override void ExecuteInternal(object parameter)
        {
            _executeAction?.Invoke(parameter);
        }
    }
}
