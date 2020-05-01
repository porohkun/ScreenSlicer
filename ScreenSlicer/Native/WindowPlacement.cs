using System.Drawing;

namespace ScreenSlicer.Native
{
    public struct WindowPlacement
    {
        public int Length;
        public int Flags;
        public WindowShowStyle ShowCmd;
        public Point MinPosition;
        public Point MaxPosition;
        public NativeRectangle NormalPosition;

        public override string ToString()
        {
            return $"L:{Length} | F:{Flags} | Style:{ShowCmd} | Pos:{MinPosition}~{MaxPosition} | {NormalPosition}";
        }
    }
}
