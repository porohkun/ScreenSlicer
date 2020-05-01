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
    /// <summary>
    /// Interaction logic for WinListWindow.xaml
    /// </summary>
    public partial class WinListWindow : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        protected void NotifyPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private readonly ProcessesWatcher _watcher;
        private readonly CollectionView _view;
        private ISystemWindow _selectedWindow;

        public ObservableCollection<ISystemWindow> Windows { get; private set; } = new ObservableCollection<ISystemWindow>();

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


        public WinListWindow()
        {
            InitializeComponent();
            DataContext = this;
        }

        public WinListWindow(ProcessesWatcher _watcher) : this()
        {
            this._watcher = _watcher;
            _view = (CollectionView)CollectionViewSource.GetDefaultView(Windows);
            _view.Filter = ViewFilter;
        }

        private bool ViewFilter(object item)
        {
            if (!(item is ISystemWindow window))
                return false;
            //if (string.IsNullOrEmpty(window.Title))
            //    return false;
            if (!window.Visible)
                return false;
            if (window.Style.HasFlag(Native.WindowStyle.Popup) && !window.Style.HasFlag(Native.WindowStyle.PopupWindow))
                return false;
            return true;
        }

        private void GridViewColumnHeader_Click(object sender, RoutedEventArgs e)
        {
            GridViewColumnHeader colHeader = (GridViewColumnHeader)e.OriginalSource;
            var colName = colHeader.Content.ToString();

            var prevDescription = _view.SortDescriptions.FirstOrDefault(d => d.PropertyName == colName);
            _view.SortDescriptions.Clear();
            if (prevDescription.PropertyName != null && prevDescription.Direction == ListSortDirection.Ascending)
                _view.SortDescriptions.Add(new SortDescription(colName, ListSortDirection.Descending));
            else
                _view.SortDescriptions.Add(new SortDescription(colName, ListSortDirection.Ascending));

            _view.Refresh();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var handle = SelectedWindow.Handle;
            SetForegroundWindow(handle);
            Methods.ShowWindow(handle, ShowWindowCommand.ShowNoActive);
        }

        [DllImport("user32.dll")]
        public static extern long SetForegroundWindow(IntPtr hWnd);



        private void Button_Click_1(object sender, RoutedEventArgs e)
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
}
