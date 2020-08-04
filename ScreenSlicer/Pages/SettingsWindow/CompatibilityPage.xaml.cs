using Ninject;
using ScreenSlicer.Commands;
using ScreenSlicer.Compatibility;
using ScreenSlicer.Compatibility.Actions;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Controls;
using System.Windows.Input;

namespace ScreenSlicer.Pages.SettingsWindow
{
    public class CompatibilityPageDummy : CompatibilityPageViewModel
    {
        public CompatibilityPageDummy() : base()
        {
            Rules = new ObservableCollection<Rule>();
            var rule = new Rule()
            {
                Name = "Default",
            };
            rule.Conditions.Add(new FileInfoPropertyCondition() { Property = FileVersionInfoProperties.CompanyName, TargetValue = "aaa" });
            rule.Conditions.Add(new TitleCondition());
            rule.Conditions.Add(new WindowClassCondition());
            rule.Conditions.Add(new FilenameCondition() { TargetValue = "app.exe" });
            rule.MoveWindowSequence.Add(new CorrectTargetRegionData());
            rule.MoveWindowSequence.Add(new ModifyTargetRegionData());
            rule.MoveWindowSequence.Add(new User32PostMessageData()
            {
                Message = Native.WindowMessage.EnterSizeMove
            });
            rule.MoveWindowSequence.Add(new User32ShowWindowData()
            {
                Command = Native.ShowWindowCommand.ShowNormal
            });
            rule.MoveWindowSequence.Add(new User32SetWindowPosData()
            {
                Flags = Native.ShowWindowPosition.NoZOrder | Native.ShowWindowPosition.NoSendChanging
            });
            rule.MoveWindowSequence.Add(new User32PostMessageData()
            {
                Message = Native.WindowMessage.ExitSizeMove
            });
            Rules.Add(rule);
            SelectedRule = rule;
            Rules.Add(new Rule() { Name = "MyRule super crazy" });
        }
    }

    public class CompatibilityPageViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        protected void NotifyPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private Rule _selectedRule;

        public ObservableCollection<Rule> Rules { get; protected set; }
        public Rule SelectedRule
        {
            get => _selectedRule;
            set
            {
                if (_selectedRule != value)
                {
                    _selectedRule = value;
                    NotifyPropertyChanged(nameof(SelectedRule));
                }
            }
        }

        public ICommand ShowListWindowCommand { get; private set; }

        public CompatibilityPageViewModel(ICommand showListWindowCommand) : this()
        {
            ShowListWindowCommand = showListWindowCommand;
        }

        public CompatibilityPageViewModel()
        {
            Rules = Settings.Instance.Compatibility.Rules;
            Rules.CollectionChanged += Rules_CollectionChanged;
        }

        private void Rules_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Add && e.NewItems.Count > 0)
                SelectedRule = e.NewItems[0] as Rule;
        }
    }

    /// <summary>
    /// Interaction logic for SystemPage.xaml
    /// </summary>
    public partial class CompatibilityPage : Page
    {

        public CompatibilityPage()
        {
            DataContext = new CompatibilityPageViewModel(App.Container.Get<ShowWindowCommand<Windows.WinListWindow>>());
            InitializeComponent();
        }

        private void TextBox_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Return && sender is TextBox tb)
            {
                Keyboard.ClearFocus();
                tb.GetBindingExpression(TextBox.TextProperty).UpdateSource();
            }
        }
    }
}
