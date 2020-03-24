using Hardcodet.Wpf.TaskbarNotification;
using ScreenSlicer.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ScreenSlicer.NotifyIcon
{
    /// <summary>
    /// Interaction logic for NotifyIcon.xaml
    /// </summary>
    public partial class NotifyIcon : TaskbarIcon
    {
        public ICommand AppActivatedCommand { get; }
        public ICommand ShowSlicingWindowCommand { get; }
        public ICommand ShowSettingsWindowCommand { get; }
        public ICommand ExitCommand { get; }

        public NotifyIcon(AppActivatedCommand appActivatedCommand,
            ShowWindowCommand<Windows.SlicingWindow> showSlicingWindowCommand,
            ShowWindowCommand<Windows.SettingsWindow> showSettingsWindowCommand,
            ExitCommand exitCommand)
        {
            AppActivatedCommand = appActivatedCommand;
            ShowSlicingWindowCommand = showSlicingWindowCommand;
            ShowSettingsWindowCommand = showSettingsWindowCommand;
            ExitCommand = exitCommand;

            InitializeComponent();
        }

        private void ExitCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        private void ExitCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
    }
}
