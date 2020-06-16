using Newtonsoft.Json;
using System.Windows;

namespace ScreenSlicer
{
    public class WindowStateSettings : SettingsPartWithNotifier
    {
        [JsonProperty(nameof(Width))]
        private double _width = 640;

        [JsonProperty(nameof(Height))]
        private double _height = 480;

        [JsonProperty(nameof(State))]
        private WindowState _state = WindowState.Normal;

        [JsonIgnore]
        public double Width
        {
            get => _width;
            set
            {
                if (value != _width)
                {
                    _width = value;
                    NotifyPropertyChanged(nameof(Width));
                }
            }
        }

        [JsonIgnore]
        public double Height
        {
            get => _height;
            set
            {
                if (value != _height)
                {
                    _height = value;
                    NotifyPropertyChanged(nameof(Height));
                }
            }
        }

        [JsonIgnore]
        public WindowState State
        {
            get => _state;
            set
            {
                if (value != _state)
                {
                    _state = value;
                    NotifyPropertyChanged(nameof(State));
                }
            }
        }
    }
}
