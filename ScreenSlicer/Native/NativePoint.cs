using System.Drawing;

namespace ScreenSlicer.Native
{
    public struct NativePoint
    {
        public readonly int X;
        public readonly int Y;

        public NativePoint(int x, int y)
        {
            X = x;
            Y = y;
        }

        public static NativePoint FromPoint(Point point) => new NativePoint(point.X, point.Y);

        public Point ToPoint() => new Point(X, Y);

        public static implicit operator NativePoint(Point point) => FromPoint(point);

        public static implicit operator Point(NativePoint point) => point.ToPoint();

        public override string ToString()
        {
            return $"{{{X}:{Y}}}";
        }
    }
}
