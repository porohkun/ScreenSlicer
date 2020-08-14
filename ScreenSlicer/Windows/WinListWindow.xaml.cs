using ScreenSlicer.Commands;
using ScreenSlicer.Compatibility;
using ScreenSlicer.Managers;
using ScreenSlicer.Native;
using ScreenSlicer.Native.Windows;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace ScreenSlicer.Windows
{
    public class WinListWindowViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        protected void NotifyPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private readonly ProcessesWatcher _watcher;
        private readonly CollectionView _view;
        private Rule _selectedRule;
        private ISystemWindow _selectedWindow;
        private bool _useRule = true;

        public DelegateCommand ColumnReorderCommand { get; }
        public DelegateCommand ShowSelectedWindowCommand { get; }
        public DelegateCommand RefreshCommand { get; }

        public Rule SelectedRule
        {
            get => _selectedRule;
            set
            {
                if (_selectedRule != value)
                {
                    _selectedRule = value;
                    NotifyPropertyChanged(nameof(SelectedRule));
                    Refresh();
                    _view.Refresh();
                }
            }
        }
        public ISystemWindow SelectedWindow
        {
            get => _selectedWindow;
            set
            {
                if (_selectedWindow != value)
                {
                    _selectedWindow = value;
                    NotifyPropertyChanged(nameof(SelectedWindow));
                }
            }
        }
        public bool UseRule
        {
            get => _useRule;
            set
            {
                if (_useRule != value)
                {
                    _useRule = value;
                    NotifyPropertyChanged(nameof(UseRule));
                    _view.Refresh();
                }
            }
        }


        public ObservableCollection<ISystemWindow> Windows { get; private set; } = new ObservableCollection<ISystemWindow>();

        public WinListWindowViewModel()
        {
            _view = (CollectionView)CollectionViewSource.GetDefaultView(Windows);
            _view.Filter = ViewFilter;
        }

        public WinListWindowViewModel(ProcessesWatcher watcher) : this()
        {
            _watcher = watcher;
            ColumnReorderCommand = new DelegateCommand(p => ColumnReorder((GridViewColumnHeader)p));
            ShowSelectedWindowCommand = new DelegateCommand(p => ShowSelectedWindow());
            RefreshCommand = new DelegateCommand(p => Refresh());

            Refresh();
            _view.Refresh();
        }


        private void ColumnReorder(GridViewColumnHeader colHeader)
        {
            var colName = colHeader.Content.ToString();

            var prevDescription = _view.SortDescriptions.FirstOrDefault(d => d.PropertyName == colName);
            _view.SortDescriptions.Clear();
            if (prevDescription.PropertyName != null && prevDescription.Direction == ListSortDirection.Ascending)
                _view.SortDescriptions.Add(new SortDescription(colName, ListSortDirection.Descending));
            else
                _view.SortDescriptions.Add(new SortDescription(colName, ListSortDirection.Ascending));

            _view.Refresh();
        }

        private void ShowSelectedWindow()
        {
            if (SelectedWindow == null)
                return;
            var handle = SelectedWindow.Handle;
            Methods.SetForegroundWindow(handle);
            Methods.ShowWindow(handle, ShowWindowCommand.ShowNoActive);
        }

        private bool ViewFilter(object item)
        {
            if (!(item is ISystemWindow window))
                return false;
            if (window.Position.Size == default)
                return false;
            //if (string.IsNullOrEmpty(window.Title))
            //    return false;
            if (!window.Visible)
                return false;
            if (window.Style.HasFlag(Native.WindowStyle.Popup) && !window.Style.HasFlag(Native.WindowStyle.PopupWindow))
                return false;
            return UseRule ? (SelectedRule != null ? (SelectedRule.Conditions.All(c => c.Check(window))) : true) : true;
        }

        private void Refresh()
        {
            var newWindows = _watcher.GetDesktopWindows().ToList();
            var closedWindows = new List<ISystemWindow>();
            foreach (var win in Windows)
                if (newWindows.Contains(win))
                    newWindows.Remove(win);
                else
                    closedWindows.Add(win);
            foreach (var win in closedWindows)
                Windows.Remove(win);
            foreach (var win in newWindows)
                Windows.Add(win);
        }
    }

    /// <summary>
    /// Interaction logic for WinListWindow.xaml
    /// </summary>
    public partial class WinListWindow : IParametricWindow
    {
        public WinListWindow()
        {
            DataContext = new WinListWindowViewModel();
            InitializeComponent();
        }

        public WinListWindow(ProcessesWatcher watcher)
        {
            DataContext = new WinListWindowViewModel(watcher);
            InitializeComponent();
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            Hide();
            e.Cancel = true;
            base.OnClosing(e);
        }

        void IParametricWindow.SetParameter(object parameter)
        {
            var rule = (Rule)parameter;
            (DataContext as WinListWindowViewModel).SelectedRule = rule;
        }
    }
}
