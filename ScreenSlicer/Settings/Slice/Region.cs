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
        protected void NotifyPropertyChanged(string strPropertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(strPropertyName));
        }

        private Rectangle _bounds;
        private Slice _slice;
        private Region[] _regions;

        //[XmlElement]
        public Rectangle Bounds
        {
            get => _bounds;
            set
            {
                if (_bounds != value)
                {
                    _bounds = value;
                    NotifyPropertyChanged(nameof(Bounds));
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
                    foreach (var region in _regions)
                        if (region != null)
                            region.PropertyChanged += Region_PropertyChanged;
                    NotifyPropertyChanged(nameof(Regions));
                }
            }
        }

        private void Region_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            NotifyPropertyChanged(nameof(Regions));
        }

        public Region() { }

        public Region(Rectangle bounds)
        {
            Bounds = bounds;
            Regions = new Region[0];
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

        private void Slice_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(Slice.Position) && sender is Slice slice)
            {
                UpdateSubRegions(slice);
            }
            NotifyPropertyChanged($"{nameof(Slice)}.{e.PropertyName}");
        }

        private void UpdateSubRegions(Slice slice)
        {
            if (slice == null)
            {
                Regions = new Region[0];
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
                foreach (var region in Regions)
                    region.GlueRegionCommand();
            }
        }

        public virtual object Clone()
        {
            return new Region(
                Bounds,
                Slice != null ? Slice.Clone() as Slice : null,
                Regions.Select(r => r.Clone() as Region).ToArray());
        }
    }
}
