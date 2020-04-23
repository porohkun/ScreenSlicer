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
        private Region _regionA;
        private Region _regionB;

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

        public Region RegionA
        {
            get => _regionA;
            set
            {
                if (_regionA != value)
                {
                    if (_regionA != null)
                        _regionA.PropertyChanged -= Region_PropertyChanged;
                    _regionA = value;
                    if (_regionA != null)
                        _regionA.PropertyChanged += Region_PropertyChanged;
                    NotifyPropertyChanged(nameof(RegionA));
                }
            }
        }

        public Region RegionB
        {
            get => _regionB;
            set
            {
                if (_regionB != value)
                {
                    if (_regionB != null)
                        _regionB.PropertyChanged -= Region_PropertyChanged;
                    _regionB = value;
                    if (_regionB != null)
                        _regionB.PropertyChanged += Region_PropertyChanged;
                    NotifyPropertyChanged(nameof(RegionB));
                }
            }
        }

        [XmlIgnore]
        public int MinWidth
        {
            get
            {
                if (Slice == null)
                    return Settings.Instance.Regions.MinRegionSize.Width;
                if (Slice.Orientation == Orientation.Horizontal)
                    return Math.Max(RegionA.MinWidth, RegionB.MinWidth);
                return RegionA.MinWidth + RegionB.MinWidth;
            }
        }

        [XmlIgnore]
        public int MinHeight
        {
            get
            {
                if (Slice == null)
                    return Settings.Instance.Regions.MinRegionSize.Height;
                if (Slice.Orientation == Orientation.Vertical)
                    return Math.Max(RegionA.MinHeight, RegionB.MinHeight);
                return RegionA.MinHeight + RegionB.MinHeight;
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
                return RegionA.MinWidth;
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
                return Bounds.Width - RegionB.MinWidth;
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
                return RegionA.MinHeight;
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
                return Bounds.Height - RegionB.MinHeight;
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
            }

            if (sender == RegionA)
                NotifyPropertyChanged($"{nameof(RegionA)}.{e.PropertyName}");
            if (sender == RegionB)
                NotifyPropertyChanged($"{nameof(RegionB)}.{e.PropertyName}");
        }

        public Region() { }

        public Region(Rectangle bounds)
        {
            Bounds = bounds;
        }

        protected Region(Rectangle bounds, Slice slice, Region regionA, Region regionB)
        {
            Bounds = bounds;
            Slice = slice;
            RegionA = regionA;
            RegionB = regionB;
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
                RegionA = null;
                RegionB = null;
            }
            else
            {
                RegionA = RegionA ?? new Region();
                RegionB = RegionB ?? new Region();

                switch (slice.Orientation)
                {
                    case Orientation.Vertical:
                        RegionA.Bounds = new Rectangle(Bounds.X, Bounds.Y, slice.Position, Bounds.Height);
                        RegionB.Bounds = new Rectangle(Bounds.X + slice.Position, Bounds.Y, Bounds.Width - slice.Position, Bounds.Height);
                        break;
                    case Orientation.Horizontal:
                        RegionA.Bounds = new Rectangle(Bounds.X, Bounds.Y, Bounds.Width, slice.Position);
                        RegionB.Bounds = new Rectangle(Bounds.X, Bounds.Y + slice.Position, Bounds.Width, Bounds.Height - slice.Position);
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
                RegionA?.Clone() as Region,
                RegionB?.Clone() as Region);
        }
    }
}
