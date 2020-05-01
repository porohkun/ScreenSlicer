using System;

namespace ScreenSlicer.Native
{
    [Flags]
    public enum AsyncKeyStateResult : ushort
    {
        Pressed = 0x8000,
        WasPressedAfterLastCheck = 0x1
    }
}
