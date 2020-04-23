using System;
using System.ComponentModel;
using System.Windows;
using System.Xml.Serialization;

namespace ScreenSlicer
{
    [Serializable]
    public class WindowStateSettings : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        protected void NotifyPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private double _width = 640;
        private double _height = 480;
        private WindowState _state;

        [XmlAttribute]
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

        [XmlAttribute]
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

        [XmlAttribute]
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
