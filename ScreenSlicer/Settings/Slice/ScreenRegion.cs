using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace ScreenSlicer
{
    [Serializable]
    public class ScreenRegion : Region
    {
        [XmlAttribute]
        public bool IsPrimary { get; set; }

        [XmlElement(Order = 1)]
        public Rectangle PhysicalBounds { get; set; }

        public ScreenRegion() { }

        public ScreenRegion(Rectangle bounds, Rectangle physicalBounds, bool isPrimary) : base(bounds)
        {
            IsPrimary = isPrimary;
            PhysicalBounds = physicalBounds;
        }

        private ScreenRegion(Rectangle bounds, Rectangle physicalBounds, Slice slice, Region regionA, Region regionB, bool isMain) : base(bounds, slice, regionA, regionB)
        {
            IsPrimary = isMain;
            PhysicalBounds = physicalBounds;
        }

        public void Hook()
        {
            NotifyPropertyChanged(nameof(Bounds));
            NotifyPropertyChanged(nameof(PhysicalBounds));
        }

        public override object Clone()
        {
            return new ScreenRegion(
                Bounds,
                PhysicalBounds,
                Slice?.Clone() as Slice,
                RegionA?.Clone() as Region,
                RegionB?.Clone() as Region,
                IsPrimary);
        }
    }
}
