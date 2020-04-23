using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Xml.Serialization;

namespace ScreenSlicer
{
    [Serializable]
    public class Region : INotifyPropertyChanged, ICloneable
    {
        public event PropertyChangedEventHandler PropertyChanged;
        protected void NotifyPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private Rectangle _bounds;
        private Slice _slice;
        private Region[] _regions;

        public Rectangle Bounds
        {
            get => _bounds;
            set
            {
                if (_bounds != value)
                {
                    _bounds = value;
                    NotifyPropertyChanged(nameof(Bounds));
                    if (Slice != null)
                    {
                        Slice.Bound(this);
                        UpdateSubRegions(Slice);
                    }
                    NotifyPropertyChanged(nameof(MaxVerticalSlice));
                    NotifyPropertyChanged(nameof(MaxHorizontalSlice));
                }
            }
        }

        public Slice Slice
        {
            get => _slice;
            set
            {
                if (_slice != null)
                    _slice.PropertyChanged -= Slice_PropertyChanged;
                if (_slice != value)
                {
                    _slice = value;
                    if (_slice != null)
                        _slice.PropertyChanged += Slice_PropertyChanged;
                    NotifyPropertyChanged(nameof(Slice));
                    NotifyPropertyChanged(nameof(MinWidth));
                    NotifyPropertyChanged(nameof(MinHeight));
                    NotifyPropertyChanged(nameof(MinVerticalSlice));
                    NotifyPropertyChanged(nameof(MaxVerticalSlice));
                    NotifyPropertyChanged(nameof(MinHorizontalSlice));
                    NotifyPropertyChanged(nameof(MaxHorizontalSlice));
                }
            }
        }

        public Region[] Regions
        {
            get => _regions;
            set
            {
                if (_regions != value)
                {
                    if (_regions != null)
                        foreach (var region in _regions)
                            if (region != null)
                                region.PropertyChanged -= Region_PropertyChanged;
                    _regions = value;
                    if (_regions != null)
                        foreach (var region in _regions)
                            if (region != null)
                                region.PropertyChanged += Region_PropertyChanged;
                    NotifyPropertyChanged(nameof(Regions));
                }
            }
        }

        [XmlIgnore]
        public int MinWidth
        {
            get
            {
                if (Slice == null || Slice.Orientation == Orientation.Horizontal)
                    return Settings.Instance.Regions.MinRegionSize.Width;
                return Regions[0].MinWidth + Regions[1].MinWidth;
            }
        }

        [XmlIgnore]
        public int MinHeight
        {
            get
            {
                if (Slice == null || Slice.Orientation == Orientation.Vertical)
                    return Settings.Instance.Regions.MinRegionSize.Height;
                return Regions[0].MinHeight + Regions[1].MinHeight;
            }
        }

        [XmlIgnore]
        public int MinVerticalSlice
        {
            get
            {
                if (Slice == null)
                    return MinWidth;
                if (Slice.Orientation == Orientation.Horizontal)
                    return 0;
                return Regions[0].MinWidth;
            }
        }

        [XmlIgnore]
        public int MaxVerticalSlice
        {
            get
            {
                if (Slice == null)
                    return Bounds.Width - MinWidth;
                if (Slice.Orientation == Orientation.Horizontal)
                    return Bounds.Width;
                return Bounds.Width - Regions[1].MinWidth;
            }
        }

        [XmlIgnore]
        public int MinHorizontalSlice
        {
            get
            {
                if (Slice == null)
                    return MinHeight;
                if (Slice.Orientation == Orientation.Vertical)
                    return 0;
                return Regions[0].MinHeight;
            }
        }

        [XmlIgnore]
        public int MaxHorizontalSlice
        {
            get
            {
                if (Slice == null)
                    return Bounds.Height - MinHeight;
                if (Slice.Orientation == Orientation.Vertical)
                    return Bounds.Height;
                return Bounds.Height - Regions[1].MinHeight;
            }
        }

        private void Slice_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(Slice.Position) && sender is Slice slice)
            {
                UpdateSubRegions(slice);
            }
            NotifyPropertyChanged($"{nameof(Slice)}.{e.PropertyName}");
        }

        private void Region_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case nameof(Region.MinWidth):
                    NotifyPropertyChanged(nameof(MinWidth));
                    NotifyPropertyChanged(nameof(MinVerticalSlice));
                    break;
                case nameof(Region.MinHeight):
                    NotifyPropertyChanged(nameof(MinHeight));
                    NotifyPropertyChanged(nameof(MinHorizontalSlice));
                    break;
                    //case nameof(Region.Bounds):
                    //    if (Slice != null)
                    //    {
                    //        Slice.Bound(this);
                    //        UpdateSubRegions(Slice);
                    //    }
                    //    break;
            }

            NotifyPropertyChanged($"{nameof(Regions)}.{e.PropertyName}");
        }

        public Region() { }

        public Region(Rectangle bounds)
        {
            Bounds = bounds;
        }

        protected Region(Rectangle bounds, Slice slice, Region[] regions)
        {
            Bounds = bounds;
            Slice = slice;
            Regions = regions;
        }

        public void SliceRegion(Orientation orientation, int position)
        {
            if (Slice == null)
            {
                var slice = new Slice(orientation, position);
                if (!slice.Bound(this))
                    return;
                UpdateSubRegions(slice);
                Slice = slice;
            }
            else
                throw new ArgumentException("_slice != null. Region need to be glued before slice");
        }

        public void GlueRegionCommand()
        {
            UpdateSubRegions(null);
            Slice = null;
        }

        private void UpdateSubRegions(Slice slice)
        {
            if (slice == null)
            {
                Regions = null;
            }
            else
            {
                if ((Regions?.Length ?? 0) != 2 || Regions[0] == null || Regions[1] == null)
                    Regions = new Region[2] { new Region(), new Region() };

                switch (slice.Orientation)
                {
                    case Orientation.Vertical:
                        Regions[0].Bounds = new Rectangle(Bounds.X, Bounds.Y, slice.Position, Bounds.Height);
                        Regions[1].Bounds = new Rectangle(Bounds.X + slice.Position, Bounds.Y, Bounds.Width - slice.Position, Bounds.Height);
                        break;
                    case Orientation.Horizontal:
                        Regions[0].Bounds = new Rectangle(Bounds.X, Bounds.Y, Bounds.Width, slice.Position);
                        Regions[1].Bounds = new Rectangle(Bounds.X, Bounds.Y + slice.Position, Bounds.Width, Bounds.Height - slice.Position);
                        break;
                }
                //foreach (var region in Regions)
                //    region.GlueRegionCommand();
            }
        }

        public virtual object Clone()
        {
            return new Region(
                Bounds,
                Slice?.Clone() as Slice,
                Regions?.Select(r => r.Clone() as Region).ToArray());
        }
    }
}
