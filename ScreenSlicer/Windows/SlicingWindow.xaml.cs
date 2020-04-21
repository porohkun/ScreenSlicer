using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
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

namespace ScreenSlicer.Windows
{
    public class SlicingWindowDummy : SlicingWindowViewModel
    {
        public SlicingWindowDummy() : base(new ScreenRegion(
            bounds: new Rectangle(40, 0, 600, 440),
            physicalBounds: new Rectangle(0, 0, 640, 480),
            isPrimary: true), new Commands.EndSliceRegionsCommand())
        {
            Region.SliceRegion(Orientation.Horizontal, 200);
        }
    }

    public class SlicingWindowViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        protected void NotifyPropertyChanged(string strPropertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(strPropertyName));
        }

        public ICommand EndSliceRegionsCommand { get; }

        public ScreenRegion Region { get; private set; }
        public Rectangle PhysicalBounds => Region.PhysicalBounds;
        public Rectangle Bounds => Region.Bounds;
        public double OffsetLeft => Bounds.Left - PhysicalBounds.Left;
        public double OffsetTop => Bounds.Top - PhysicalBounds.Top;
        public double OffsetRight => PhysicalBounds.Right - Bounds.Right;
        public double OffsetBottom => PhysicalBounds.Bottom - Bounds.Bottom;


        public SlicingWindowViewModel(ScreenRegion region, ICommand endSliceRegionsCommand)
        {
            Region = region;
            EndSliceRegionsCommand = endSliceRegionsCommand;
        }
    }

    /// <summary>
    /// Interaction logic for SlicingWindow.xaml
    /// </summary>
    public partial class SlicingWindow : Window
    {
        private SlicingWindowViewModel _viewModel;

        public SlicingWindow()
        {
            InitializeComponent();
        }

        public SlicingWindow(ScreenRegion region, ICommand endSliceRegionsCommand)
        {
            _viewModel = new SlicingWindowViewModel(region, endSliceRegionsCommand);
            DataContext = _viewModel;
            InitializeComponent();

            Grid newRoot = null;
            if (_viewModel.OffsetTop >= 20)
                newRoot = TopPanel;
            else if (_viewModel.OffsetBottom >= 20)
                newRoot = BottomPanel;
            else if (_viewModel.OffsetLeft >= 20)
                newRoot = LeftPanel;
            else if (_viewModel.OffsetRight >= 20)
                newRoot = RightPanel;
            var oldRoot = ControlsPanel.Parent as Grid;
            //oldRoot.Children.Remove(ControlsPanel);
            //newRoot.Children.Add(ControlsPanel);
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            Hide();
            e.Cancel = true;
            base.OnClosing(e);
        }
    }
}
