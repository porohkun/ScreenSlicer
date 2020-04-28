using System;

namespace ScreenSlicer.Native
{
    [Flags]
    public enum WindowStyle : uint
    {
        #region Unknown bits
        U_1 = 0x1,
        U_2 = 0x2,
        U_4 = 0x4,
        U_8 = 0x8,
        U_10 = 0x10,
        U_20 = 0x20,
        U_40 = 0x40,
        U_80 = 0x80,
        U_100 = 0x100,
        U_200 = 0x200,
        U_400 = 0x400,
        U_800 = 0x800,
        U_1000 = 0x1000,
        U_2000 = 0x2000,
        U_4000 = 0x4000,
        U_8000 = 0x8000,
        #endregion

        /// <summary>
        /// The window has a thin-line border.
        /// </summary>
        Border = 0x0080_0000,

        /// <summary>
        /// The window has a title bar (includes the 'Border' style).
        /// </summary>
        Caption = 0x00C0_0000,

        /// <summary>
        /// The window is a child window. A window with this style cannot have a menu bar. This style cannot be used with the 'Popup' style.
        /// </summary>
        Child = 0x4000_0000,

        /// <summary>
        /// Excludes the area occupied by child windows when drawing occurs within the parent window. This style is used when creating the parent window.
        /// </summary>
        ClipChildren = 0x0200_0000,

        /// <summary>
        /// Clips child windows relative to each other; that is, when a particular child window receives a WM_PAINT message,
        /// the 'ClipSiblings' style clips all other overlapping child windows out of the region of the child window to be updated.
        /// If 'ClipSiblings' is not specified and child windows overlap, it is possible, when drawing within the
        /// client area of a child window, to draw within the client area of a neighboring child window.
        /// </summary>
        ClipSiblings = 0x0400_0000,

        /// <summary>
        /// The window is initially disabled. A disabled window cannot receive input from the user. To change this after a window has been created, use the EnableWindow function.
        /// </summary>
        Disabled = 0x0800_0000,

        /// <summary>
        /// The window has a border of a style typically used with dialog boxes. A window with this style cannot have a title bar.
        /// </summary>
        DialogFrame = 0x0040_0000,

        /// <summary>
        /// The window is the first control of a group of controls. The group consists of this first control and all controls defined after it,
        /// up to the next control with the 'Group' style. The first control in each group usually has the 'TabStop' style so that the user
        /// can move from group to group. The user can subsequently change the keyboard focus from one control in the group to the next control
        /// in the group by using the direction keys.
        /// 
        /// You can turn this style on and off to change dialog box navigation. To change this style after a window has been created, use the SetWindowLong function.
        /// </summary>
        Group = 0x0002_0000,

        /// <summary>
        /// The window has a horizontal scroll bar.
        /// </summary>
        HorizontalScroll = 0x0010_0000,

        /// <summary>
        /// The window is initially minimized. Same as the 'Minimize' style.
        /// </summary>
        Iconic = 0x2000_0000,

        /// <summary>
        /// The window is initially maximized.
        /// </summary>
        Maximize = 0x0100_0000,

        /// <summary>
        /// The window has a maximize button. Cannot be combined with the WS_EX_CONTEXTHELP style. The 'SystemMenu' style must also be specified. 
        /// </summary>
        MaximizeBox = 0x0001_0000,

        /// <summary>
        /// The window is initially minimized. Same as the 'Iconic' style.
        /// </summary>
        Minimize = Iconic,

        /// <summary>
        /// The window has a minimize button. Cannot be combined with the WS_EX_CONTEXTHELP style. The 'SystemMenu' style must also be specified. 
        /// </summary>
        MinimizeBox = Group,

        /// <summary>
        /// The window is an overlapped window. An overlapped window has a title bar and a border. Same as the 'Tiled' style.
        /// </summary>
        Overlapped = 0x0000_0000,

        /// <summary>
        /// The window is an overlapped window. Same as the 'TiledWindow' style. 
        /// </summary>
        OverlappedWindow = Overlapped | Caption | SystemMenu | ThickFrame | MinimizeBox | MaximizeBox,

        /// <summary>
        /// The windows is a pop-up window. This style cannot be used with the 'Child' style.
        /// </summary>
        Popup = 0x8000_0000,

        /// <summary>
        /// The window is a pop-up window. The 'Caption' and 'PopupWindow' styles must be combined to make the window menu visible.
        /// </summary>
        PopupWindow = Popup | Border | SystemMenu,

        /// <summary>
        /// The window has a sizing border. Same as the 'ThickFrame' style.
        /// </summary>
        SizeBox = 0x0004_0000,

        /// <summary>
        /// The window has a window menu on its title bar. The 'Caption' style must also be specified.
        /// </summary>
        SystemMenu = 0x0008_0000,

        /// <summary>
        /// The window is a control that can receive the keyboard focus when the user presses the TAB key. Pressing the TAB key changes the
        /// keyboard focus to the next control with the 'TabStop' style.
        /// 
        /// You can turn this style on and off to change dialog box navigation. To change this style after a window has been created,
        /// use the SetWindowLong function. For user-created windows and modeless dialogs to work with tab stops, alter the message loop
        /// to call the IsDialogMessage function.
        /// </summary>
        TabStop = MaximizeBox,

        /// <summary>
        /// The window has a sizing border. Same as the 'SizeBox' style.
        /// </summary>
        ThickFrame = SizeBox,

        /// <summary>
        /// The window is an overlapped window. An overlapped window has a title bar and a border. Same as the 'Overlapped' style. 
        /// </summary>
        Tiled = Overlapped,

        /// <summary>
        /// The window is an overlapped window. Same as the 'OverlappedWindow' style. 
        /// </summary>
        TiledWindow = OverlappedWindow,

        /// <summary>
        /// The window is initially visible.
        /// This style can be turned on and off by using the ShowWindow or SetWindowPos function.
        /// </summary>
        Visible = 0x1000_0000,

        /// <summary>
        /// The window has a vertical scroll bar.
        /// </summary>
        VerticalScroll = 0x0020_0000
    }
}
