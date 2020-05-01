using System;
using System.Diagnostics;
using System.Drawing;

namespace ScreenSlicer.Native.Windows
{
    public interface ISystemWindow : IEquatable<ISystemWindow>
    {
        IntPtr Handle { get; }
        ISystemWindow Root { get; }
        ISystemWindow Parent { get; }
        Rectangle Position { get; }
        Rectangle ClientRectangle { get; }
        string Title { get; }
        bool Visible { get; }
        WindowStyle Style { get; }
        WindowPlacement Placement { get; }

        void PostMessage(WindowMessage message, IntPtr wParam = default, IntPtr lParam = default);
        void PostMessage(uint message, IntPtr wParam = default, IntPtr lParam = default);
        void SetPosition(Rectangle rectangle, ShowWindowPosition flags, ISystemWindow behindWindow = null);
    }
}
