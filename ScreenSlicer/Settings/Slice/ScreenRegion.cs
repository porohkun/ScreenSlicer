using Newtonsoft.Json;
using System.Drawing;

namespace ScreenSlicer
{
    public class ScreenRegion : Region
    {
        public bool IsPrimary { get; set; }

        [JsonProperty(nameof(PhysicalBounds), Order = 1)]
        public Rectangle PhysicalBounds { get; private set; }

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
