using ScreenSlicer.Managers;
using ScreenSlicer.Native;
using ScreenSlicer.Native.Windows;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace ScreenSlicer.Commands
{
    public class MoveWindowCommand : InjectableCommand<MoveWindowCommand, ISystemWindow, Rectangle>
    {
        protected override bool CanExecuteInternal(ISystemWindow window, Rectangle region)
        {
            return window.Visible && region != default;
        }

        protected override void ExecuteInternal(ISystemWindow window, Rectangle region)
        {
            Logger.Info($"Moving window '{window.Title}'({window.Handle}) to {region}");
            Logger.Info($"'{window.Title}' as '{window.GetType().Name}'. Position: {window.Position}, ClientRectangle: {window.ClientRectangle}");

            window.Move(region);

            Logger.Info($"'{window.Title}'. Position: {window.Position}, ClientRectangle: {window.ClientRectangle}");
        }


    }
}
