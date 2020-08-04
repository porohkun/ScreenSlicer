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
        public static IKernel Container { get; private set; }

        private TaskbarIcon _notifyIcon;

        private void Application_Startup(object sender, StartupEventArgs e)
        {
            ConfigureContainer();
            ComposeObjects();
            _notifyIcon.BeginInit();
            Container.Get<Managers.ProcessesWatcher>();
        }

        private void ConfigureContainer()
        {
            Container = new StandardKernel(new MainModule());
        }

        private void ComposeObjects()
        {
            _notifyIcon = Container.Get<NotifyIcon.NotifyIcon>();
#if DEBUG
            Container.Get<Updating.Updater>().CheckUpdates();
            Container.Get<Windows.SettingsWindow>().Show();
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
