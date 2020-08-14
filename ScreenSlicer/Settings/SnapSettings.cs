using Newtonsoft.Json;

namespace ScreenSlicer
{
    public class SnapSettings : SettingsPartWithNotifier
    {
        [JsonProperty(nameof(AutoFill))]
        private bool _autoFill;

        [JsonProperty(nameof(SnapToMonitors))]
        private bool _snapToMonitors = true;

        [JsonProperty(nameof(SnapToWindows))]
        private bool _snapToWindows;

        [JsonProperty(nameof(SnapToRegions))]
        private bool _snapToRegions = true;

        [JsonProperty(nameof(SnapDistance))]
        private int _snapDistance = 10;

        [JsonIgnore]
        public bool AutoFill
        {
            get => _autoFill;
            set
            {
                if (value != _autoFill)
                {
                    _autoFill = value;
                    NotifyPropertyChanged(nameof(AutoFill));
                }
            }
        }

        [JsonIgnore]
        public bool SnapToMonitors
        {
            get => _snapToMonitors;
            set
            {
                if (value != _snapToMonitors)
                {
                    _snapToMonitors = value;
                    NotifyPropertyChanged(nameof(SnapToMonitors));
                }
            }
        }

        [JsonIgnore]
        public bool SnapToWindows
        {
            get => _snapToWindows;
            set
            {
                if (value != _snapToWindows)
                {
                    _snapToWindows = value;
                    NotifyPropertyChanged(nameof(SnapToWindows));
                }
            }
        }

        [JsonIgnore]
        public bool SnapToRegions
        {
            get => _snapToRegions;
            set
            {
                if (value != _snapToRegions)
                {
                    _snapToRegions = value;
                    NotifyPropertyChanged(nameof(SnapToRegions));
                }
            }
        }

        [JsonIgnore]
        public int SnapDistance
        {
            get => _snapDistance;
            set
            {
                if (value != _snapDistance)
                {
                    _snapDistance = value;
                    NotifyPropertyChanged(nameof(SnapDistance));
                }
            }
        }
    }
}
