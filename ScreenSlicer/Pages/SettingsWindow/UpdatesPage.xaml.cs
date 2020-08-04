using Ninject;
using System.Windows;
using System.Windows.Controls;

namespace ScreenSlicer.Pages.SettingsWindow
{
    /// <summary>
    /// Interaction logic for UpdatesPage.xaml
    /// </summary>
    public partial class UpdatesPage : Page
    {
        private Updating.Updater _updater;

        public UpdatesPage()
        {
            _updater = App.Container.Get<Updating.Updater>();
            InitializeComponent();
        }

        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            _updater.SetStartup(Settings.Instance.Updates.RunOnStartup);
        }
    }
}
