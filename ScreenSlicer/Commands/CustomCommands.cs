using System;
using System.Windows.Input;

namespace ScreenSlicer.Commands
{
    public static class CustomCommands
    {
        public static readonly RoutedCommand Enable = new RoutedUICommand("Enable", "Enable", typeof(CustomCommands));
        public static readonly RoutedUICommand Regions = new RoutedUICommand("Regions", "Regions", typeof(CustomCommands));
        public static readonly RoutedUICommand Settings = new RoutedUICommand("Settings", "Settings", typeof(CustomCommands));
        public static readonly RoutedUICommand Exit = new RoutedUICommand("Exit", "Exit", typeof(CustomCommands));
    }
}
