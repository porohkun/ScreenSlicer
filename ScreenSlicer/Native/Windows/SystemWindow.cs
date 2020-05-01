using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace ScreenSlicer.Native.Windows
{
    public class SystemWindow : ISystemWindow
    {
        public IntPtr Handle { get; }

        public ISystemWindow Root => new SystemWindow(Methods.GetAncestor(Handle, GetAncestorFlag.GetRoot));

        public ISystemWindow Parent => new SystemWindow(Methods.GetAncestor(Handle, GetAncestorFlag.GetParent));

        public Rectangle Position => Methods.GetWindowRect(Handle, out var rect)
            ? new Rectangle(rect.Left, rect.Top, rect.Right - rect.Left, rect.Bottom - rect.Top)
            : Rectangle.Empty;

        public Rectangle ClientRectangle => !Methods.GetClientRect(Handle, out var rect)
            ? Rectangle.Empty
            : new Rectangle(rect.Left, rect.Top, rect.Right - rect.Left, rect.Bottom - rect.Top);

        public string Title
        {
            get
            {
                var length = Methods.GetWindowTextLength(Handle) + 1;
                var builder = new StringBuilder(length);
                Methods.GetWindowText(Handle, builder, length);
                return builder.ToString();
            }
        }

        public bool Visible => Methods.IsWindowVisible(Handle);

        public WindowStyle Style => Methods.GetWindowStyle(Handle);

        public WindowPlacement Placement
        {
            get
            {
                var lpwndpl = new WindowPlacement();
                lpwndpl.Length = Marshal.SizeOf(lpwndpl);
                if (Methods.GetWindowPlacement(Handle, ref lpwndpl))
                    return lpwndpl;
                Marshal.ThrowExceptionForHR(Marshal.GetLastWin32Error());
                return lpwndpl;
            }
        }

        public SystemWindow(IntPtr hWnd)
        {
            Handle = hWnd;
        }

        public void PostMessage(WindowMessage message, IntPtr wParam = default, IntPtr lParam = default)
        {
            PostMessage((uint)message, wParam, lParam);
        }

        public void PostMessage(uint message, IntPtr wParam = default, IntPtr lParam = default)
        {
            Methods.PostMessage(Handle, message, wParam, lParam);
        }

        public void SetPosition(Rectangle rectangle, ShowWindowPosition flags, ISystemWindow behindWindow = null)
        {
            Methods.SetWindowPos(Handle, behindWindow != null ? behindWindow.Handle : IntPtr.Zero, rectangle.Left, rectangle.Top, rectangle.Width, rectangle.Height, (uint)flags);
        }

        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;
            if (this == obj)
                return true;
            if (obj.GetType() != GetType())
                return false;
            return Handle == ((ISystemWindow)obj).Handle;
        }

        public bool Equals(ISystemWindow other)
        {
            return other != null && Handle == other.Handle;
        }

        public override int GetHashCode()
        {
            return Handle.GetHashCode();
        }
    }
}
