using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScreenSlicer.Native.Windows
{
    public class ExplorerWindow : SystemWindow
    {
        public ExplorerWindow(IntPtr hWnd) : base(hWnd)
        {

        }

        //public override void Move(Rectangle region)
        //{

        //    PostMessage(WindowMessage.EnterSizeMove, IntPtr.Zero, IntPtr.Zero);
        //    Methods.ShowWindow(Handle, ShowWindowCommand.ShowNormal);
        //    SetPosition(region, ShowWindowPosition.NoSendChanging | ShowWindowPosition.NoZOrder, null);
        //    var rect = CorrectRegion(region);
        //    SetPosition(rect, ShowWindowPosition.NoSendChanging | ShowWindowPosition.NoZOrder, null);
        //    PostMessage(WindowMessage.ExitSizeMove, IntPtr.Zero, IntPtr.Zero);
        //}

        //private Rectangle CorrectRegion(Rectangle region)
        //{
        //    var expand = Size.Subtract(Position.Size, ClientRectangle.Size);
        //    var offset = new Point(-expand.Width / 2, 0);
        //    region.Offset(offset);
        //    region.Size = Size.Add(region.Size, expand);
        //    return region;
        //}
    }
}
