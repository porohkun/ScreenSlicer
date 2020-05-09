using Hardcodet.Wpf.TaskbarNotification;
using Ninject;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace ScreenSlicer
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private IKernel _container;

        private TaskbarIcon _notifyIcon;

        private void Application_Startup(object sender, StartupEventArgs e)
        {
            ConfigureContainer();
            ComposeObjects();
            _notifyIcon.BeginInit();
            _container.Get<Managers.ProcessesWatcher>();
        }

        private void ConfigureContainer()
        {
            _container = new StandardKernel(new MainModule());
        }

        private void ComposeObjects()
        {
            _notifyIcon = _container.Get<NotifyIcon.NotifyIcon>();
            _container.Get<Updating.Updater>().CheckUpdates();
#if DEBUG
            _container.Get<Windows.WinListWindow>().Show();
#endif
        }

        protected override void OnExit(ExitEventArgs e)
        {
            _notifyIcon.Dispose(); //the icon would clean up automatically, but this is cleaner
            NLog.LogManager.Shutdown();
            base.OnExit(e);
        }
    }
}
