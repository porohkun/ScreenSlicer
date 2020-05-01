using ScreenSlicer.Managers;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;

namespace ScreenSlicer.Commands
{
    public class GlueRegionCommand : InjectableCommand
    {
        protected override bool CanExecuteInternal(object parameter)
        {
            if (parameter is Region region)
                return region.Slice != null;
            else return false;
        }

        protected override void ExecuteInternal(object parameter)
        {
            if (parameter is Region region)
            {
                region.GlueRegionCommand();
            }
        }
    }
}
