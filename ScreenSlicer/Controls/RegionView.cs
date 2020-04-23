using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;

namespace ScreenSlicer
{
    [TemplatePart(Name = PART_GlueButton, Type = typeof(Button))]
    [TemplatePart(Name = PART_GridHorizontalSplit, Type = typeof(Grid))]
    [TemplatePart(Name = PART_GridVerticalSplit, Type = typeof(Grid))]
    public class RegionView : Control, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        protected void NotifyPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #region Template parts

        internal const string PART_GlueButton = "PART_GlueButton";
        internal const string PART_GridHorizontalSplit = "PART_GridHorizontalSplit";
        internal const string PART_GridVerticalSplit = "PART_GridVerticalSplit";

        protected Button GlueButton { get; private set; }
        protected Grid GridHorizontalSplit { get; private set; }
        protected Grid GridVerticalSplit { get; private set; }

        #endregion //Template parts

        #region Properties

        #region Region

        public static readonly DependencyProperty RegionProperty
            = DependencyProperty.Register(nameof(Region), typeof(Region), typeof(RegionView), new UIPropertyMetadata(null, OnRegionChanged));

        public Region Region
        {
            get => (Region)GetValue(RegionProperty);
            set => SetValue(RegionProperty, value);
        }

        private static void OnRegionChanged(DependencyObject source, DependencyPropertyChangedEventArgs args)
        {
            if (source is RegionView view)
                view.OnRegionChanged((Region)args.OldValue, (Region)args.NewValue);
        }

        protected virtual void OnRegionChanged(Region oldValue, Region newValue)
        {
            if (oldValue != null)
                oldValue.PropertyChanged -= Region_PropertyChanged;
            if (newValue != null)
                newValue.PropertyChanged += Region_PropertyChanged;
        }

        private void Region_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(Region.Slice))
                SetBindingsToSlice();
        }

        #endregion //Region

        #endregion //Properties

        public RegionView()
        {

        }

        #region Base Class Overrides

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();


            GlueButton = GetTemplateChild(PART_GlueButton) as Button;
            GridHorizontalSplit = GetTemplateChild(PART_GridHorizontalSplit) as Grid;
            GridVerticalSplit = GetTemplateChild(PART_GridVerticalSplit) as Grid;

            SetBindingsToSlice();
        }

        #endregion //Base Class Overrides

        private void SetBindingsToSlice()
        {
            if (Region?.Slice == null)
                foreach (var grid in new[] { GridVerticalSplit, GridHorizontalSplit })
                {
                    var remove = grid.Children.Where(e => e is RegionView).Cast<UIElement>().ToArray();
                    foreach (var entry in remove)
                        grid.Children.Remove(entry);
                }
            else
            {
                var binding = new Binding(nameof(Slice.Position))
                {
                    Source = Region.Slice,
                    Mode = BindingMode.TwoWay,
                    Converter = new Converters.IntToGridLengthConverter()
                };


                Grid grid = null;
                Action<UIElement, int> setter = null;
                switch (Region.Slice.Orientation)
                {
                    case Orientation.Vertical:
                        GridVerticalSplit.ColumnDefinitions[0].SetBinding(ColumnDefinition.WidthProperty, binding);
                        grid = GridVerticalSplit;
                        setter = Grid.SetColumn;
                        break;
                    case Orientation.Horizontal:
                        GridHorizontalSplit.RowDefinitions[0].SetBinding(RowDefinition.HeightProperty, binding);
                        grid = GridHorizontalSplit;
                        setter = Grid.SetRow;
                        break;
                }

                var viewA = new RegionView() { Region = Region.RegionA };
                var viewB = new RegionView() { Region = Region.RegionB };
                grid.Children.Add(viewA);
                grid.Children.Add(viewB);
                setter(viewA, 0);
                setter(viewB, 2);
            }

        }


    }
}
