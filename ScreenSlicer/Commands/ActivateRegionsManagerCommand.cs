using ScreenSlicer.Managers;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;

namespace ScreenSlicer.Commands
{
    public class ActivateRegionsManagerCommand : InjectableCommand
    {
        private readonly RegionsManager _manager;

        public ActivateRegionsManagerCommand(RegionsManager manager)
        {
            _manager = manager;
        }

        protected override bool CanExecuteInternal(object parameter)
        {
            return !_manager.IsActive;
        }

        protected override void ExecuteInternal(object parameter)
        {
            _manager.Activate();
        }
    }
}
