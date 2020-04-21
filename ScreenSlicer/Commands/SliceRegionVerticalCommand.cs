using ScreenSlicer.Managers;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;

namespace ScreenSlicer.Commands
{
    public class SliceRegionVerticalCommand : InjectableCommand
    {
        protected override bool CanExecuteInternal(object parameter)
        {
            if (parameter is Region region)
                return region.Slice == null;
            else return false;
        }

        protected override void ExecuteInternal(object parameter)
        {
            if (parameter is Region region)
            {
                region.SliceRegion(System.Windows.Controls.Orientation.Vertical, region.Bounds.Width / 2);
            }
        }
    }
}
