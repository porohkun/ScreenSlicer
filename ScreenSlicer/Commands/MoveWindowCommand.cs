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
            var rect = CorrectRegion(window, region);
            Logger.Info($"Moving window '{window.Title}'({window.Handle}) to {rect}");
            Logger.Info($"'{window.Title}'. Position: {window.Position}, ClientRectangle: {window.ClientRectangle}");
            //    window.Move(region);
            window.PostMessage(WindowMessage.EnterSizeMove, IntPtr.Zero, IntPtr.Zero);
            Methods.ShowWindow(window.Handle, ShowWindowCommand.ShowNormal);
            window.SetPosition(rect, ShowWindowPosition.NoSendChanging | ShowWindowPosition.NoZOrder, (ISystemWindow)null);
            window.PostMessage(WindowMessage.ExitSizeMove, IntPtr.Zero, IntPtr.Zero);
            Logger.Info($"'{window.Title}'. Position: {window.Position}, ClientRectangle: {window.ClientRectangle}");
        }

        private Rectangle CorrectRegion(ISystemWindow window, Rectangle region)
        {
            var rect = window.ClientRectangle;
            var pos = window.Position;
            var expand = Size.Subtract(pos.Size, rect.Size);
            var offset = new Point(expand.Width / 2, 0);
            region.Offset(offset);
            region.Size = Size.Add(region.Size, expand);
            return region;
        }
    }
}
