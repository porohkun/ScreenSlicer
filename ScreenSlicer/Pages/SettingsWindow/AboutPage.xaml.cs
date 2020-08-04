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
using System.Diagnostics;
using Ninject;
using ScreenSlicer.Commands;

namespace ScreenSlicer.Pages.SettingsWindow
{
    public class LibData
    {
        public string Name { get; set; }
        public string Authors { get; set; }
        public string Site { get; set; }
        public string License { get; set; }
        public string Icon { get; set; }
        public Visibility ShowIcon => string.IsNullOrWhiteSpace(Icon) ? Visibility.Collapsed : Visibility.Visible;
    }

    /// <summary>
    /// Interaction logic for AboutPage.xaml
    /// </summary>
    public partial class AboutPage : Page
    {
        public ICommand HyperlinkCommand { get; }

        public AboutPage()
        {
            HyperlinkCommand = App.Container.Get<HyperlinkCommand>();
            DataContext = this;
            InitializeComponent();
        }

        private void Hyperlink_RequestNavigate(object sender, RequestNavigateEventArgs e)
        {
            if (HyperlinkCommand.CanExecute(e.Uri.AbsoluteUri))
            {
                HyperlinkCommand.Execute(e.Uri.AbsoluteUri);
                e.Handled = true;
            }
        }
    }
}
