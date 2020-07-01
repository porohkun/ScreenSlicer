using ScreenSlicer.Compatibility;
using ScreenSlicer.Compatibility.Actions;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Controls;

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
                Conditions = new ICondition[0]
            };
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

        public CompatibilityPageViewModel()
        {
            Rules = Settings.Instance.Compatibility.Rules;
        }
    }

    /// <summary>
    /// Interaction logic for SystemPage.xaml
    /// </summary>
    public partial class CompatibilityPage : Page
    {
        public CompatibilityPage()
        {
            DataContext = new CompatibilityPageViewModel();
            InitializeComponent();
        }
    }
}
