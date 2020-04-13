using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

        [XmlElement()]
        public Rectangle Bounds { get; set; }
        public Slice Slice { get; set; }
        public Region[] Regions { get; set; }

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

        public virtual object Clone()
        {
            return new Region(
                Bounds,
                Slice != null ? Slice.Clone() as Slice : null,
                Regions.Select(r => r.Clone() as Region).ToArray());
        }
    }
}
