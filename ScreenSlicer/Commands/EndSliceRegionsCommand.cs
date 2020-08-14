using Ninject;
using ScreenSlicer.Managers;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;

namespace ScreenSlicer.Commands
{
    public class EndSliceRegionsCommand : InjectableCommand<EndSliceRegionsCommand>
    {
        private RegionsManager _manager;

        public EndSliceRegionsCommand()
        {

        }

        //TODO: remove hack
        public void SetManager(RegionsManager manager)
        {
            _manager = manager;
        }

        protected override bool CanExecuteInternal(object parameter)
        {
            return !_manager?.IsActive ?? true;
        }

        protected override void ExecuteInternal(object parameter)
        {
            _manager.EndSlice(parameter?.ToString());
        }
    }
}
