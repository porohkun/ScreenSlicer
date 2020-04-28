﻿using ScreenSlicer.Managers;
using ScreenSlicer.Native;
using ScreenSlicer.Native.Windows;
using System;
using System.Collections.Generic;
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

        private CollectionView _view;
        private ISystemWindow _selectedWindow;

        public ProcessesWatcher Watcher { get; private set; }

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
            Watcher = _watcher;
            _view = (CollectionView)CollectionViewSource.GetDefaultView(Watcher.Windows);
            _view.Filter = ViewFilter;
        }

        private bool ViewFilter(object item)
        {
            if (!(item is ISystemWindow window))
                return false;
            //if (string.IsNullOrEmpty(window.Title))
            //    return false;
            //if (!window.Visible)
            //    return false;
            if (window.Style.HasFlag(Native.WindowStyle.Popup))
                return false;
            return true;
        }

        private void GridViewColumnHeader_Click(object sender, RoutedEventArgs e)
        {
            GridViewColumnHeader colHeader = (GridViewColumnHeader)e.OriginalSource;
            string colName = colHeader.Content.ToString();

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
            ShowWindow(handle, SW.SHOWNA);
        }

        [DllImport("user32.dll")]
        public static extern long SetForegroundWindow(IntPtr hWnd);
        [DllImport("user32.dll")]
        public static extern bool ShowWindow(IntPtr hWnd, SW cmd);

        public enum SW : int
        {
            HIDE = 0,
            SHOWNORMAL = 1,
            SHOWMINIMIZED = 2,
            SHOWMAXIMIZED = 3,
            SHOWNOACTIVATE = 4,
            SHOW = 5,
            MINIMIZE = 6,
            SHOWMINNOACTIVE = 7,
            SHOWNA = 8,
            RESTORE = 9,
            SHOWDEFAULT = 10
        }
    }
}