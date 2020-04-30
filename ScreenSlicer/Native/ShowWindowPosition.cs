using System;

namespace ScreenSlicer.Native
{
    [Flags]
    public enum ShowWindowPosition : uint
    {
        NoSize = 1,
        NoMove = 2,
        NoZOrder = 4,
        NoRedraw = 8,
        NoActivate = 16,
        DrawFrame = 32,
        FrameChanged = DrawFrame,
        ShowWindow = 64,
        HideWindow = 128,
        NoCopyBits = 256,
        NoOwnerZOrder = 512,
        NoReposition = NoOwnerZOrder,
        NoSendChanging = 1024,
        DeferErase = 8192,
        AsyncWindowPos = 16384
    }
}
