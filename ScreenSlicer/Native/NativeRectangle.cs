using System.Drawing;

namespace ScreenSlicer.Native
{
    public struct NativeRectangle
    {
        public readonly int Left;
        public readonly int Top;
        public readonly int Right;
        public readonly int Bottom;

        public int Height => Bottom - Top;
        public int Width => Right - Left;
        public Size Size => new Size(Width, Height);
        public Point Location => new Point(Left, Top);

        public NativeRectangle(int left, int top, int right, int bottom)
        {
            Left = left;
            Top = top;
            Right = right;
            Bottom = bottom;
        }

        public static NativeRectangle FromRectangle(Rectangle rectangle) => new NativeRectangle(rectangle.Left, rectangle.Top, rectangle.Right, rectangle.Bottom);

        public Rectangle ToRectangle() => Rectangle.FromLTRB(Left, Top, Right, Bottom);

        public static implicit operator NativeRectangle(Rectangle rect) => FromRectangle(rect);

        public static implicit operator Rectangle(NativeRectangle rect) => rect.ToRectangle();
    }
}
