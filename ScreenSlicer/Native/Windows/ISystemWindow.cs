using System;
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
        string CachedTitle { get; }
        string WindowClass { get; }
        string ProcessName { get; }
        bool Visible { get; }
        WindowStyle Style { get; }
        WindowPlacement Placement { get; }

        void Move(Rectangle region);
    }
}
