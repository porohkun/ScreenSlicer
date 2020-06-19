using Newtonsoft.Json;

namespace ScreenSlicer.Compatibility.Actions
{
    public class ModifyTargetRegionData : ActionDataBase
    {
        private int _left;
        private int _top;
        private int _right;
        private int _bottom;

        [JsonProperty(nameof(Left))]
        public int Left
        {
            get => _left;
            set
            {
                if (_left != value)
                {
                    _left = value;
                    NotifyPropertyChanged(nameof(Left));
                }
            }
        }

        [JsonProperty(nameof(Top))]
        public int Top
        {
            get => _top;
            set
            {
                if (_top != value)
                {
                    _top = value;
                    NotifyPropertyChanged(nameof(Top));
                }
            }
        }

        [JsonProperty(nameof(Right))]
        public int Right
        {
            get => _right;
            set
            {
                if (_right != value)
                {
                    _right = value;
                    NotifyPropertyChanged(nameof(Right));
                }
            }
        }

        [JsonProperty(nameof(Bottom))]
        public int Bottom
        {
            get => _bottom;
            set
            {
                if (_bottom != value)
                {
                    _bottom = value;
                    NotifyPropertyChanged(nameof(Bottom));
                }
            }
        }
    }
}
