using ScreenSlicer.Compatibility;
using ScreenSlicer.Native.Compatibility;
using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Text;

namespace ScreenSlicer.Native.Windows
{
    public class SystemWindow : ISystemWindow, ICanUseRules
    {
        private string _cachedTitle;
        private Rule _rule;

        public IntPtr Handle { get; }

        public ISystemWindow Root => new SystemWindow(Methods.GetAncestor(Handle, GetAncestorFlag.GetRoot));

        public ISystemWindow Parent => new SystemWindow(Methods.GetAncestor(Handle, GetAncestorFlag.GetParent));

        public Rectangle Position => Methods.GetWindowRect(Handle, out var rect)
            ? new Rectangle(rect.Left, rect.Top, rect.Right - rect.Left, rect.Bottom - rect.Top)
            : Rectangle.Empty;

        public Rectangle ClientRectangle => !Methods.GetClientRect(Handle, out var rect)
            ? Rectangle.Empty
            : new Rectangle(rect.Left, rect.Top, rect.Right - rect.Left, rect.Bottom - rect.Top);

        public string Title => _cachedTitle = Methods.GetWindowTitle(Handle);

        public string CachedTitle
        {
            get
            {
                if (_cachedTitle == null)
                    return Title;
                else
                    return _cachedTitle;
            }
        }

        public string WindowClass => Methods.GetWindowClassName(Handle);

        public string ProcessName => Methods.GetProcessName(Handle);

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

        public void SetRule(Rule rule)
        {
            _rule = rule;
        }

        public virtual void Move(Rectangle region)
        {
            foreach (var action in _rule.MoveWindowSequence)
                action.Apply(this, ref region);
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
