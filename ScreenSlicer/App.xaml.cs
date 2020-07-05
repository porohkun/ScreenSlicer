using Hardcodet.Wpf.TaskbarNotification;
using Ninject;
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
#if DEBUG
            _container.Get<Updating.Updater>().CheckUpdates();
            _container.Get<Windows.SettingsWindow>().Show();
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
