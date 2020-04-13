using System;
using System.ComponentModel;
using System.Windows.Controls;
using System.Xml.Serialization;

namespace ScreenSlicer
{
    [Serializable]
    public class Slice : INotifyPropertyChanged, ICloneable
    {
        public event PropertyChangedEventHandler PropertyChanged;
        protected void NotifyPropertyChanged(string strPropertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(strPropertyName));
        }

        private Orientation _orientation;
        [XmlAttribute(nameof(Orientation))]
        public Orientation Orientation
        {
            get => _orientation;
            set
            {
                if (_orientation != value)
                {
                    _orientation = value;
                    NotifyPropertyChanged(nameof(Orientation));
                }
            }
        }

        private int _position;
        [XmlAttribute(nameof(Position))]
        public int Position
        {
            get => _position;
            set
            {
                if (_position != value)
                {
                    _position = value;
                    NotifyPropertyChanged(nameof(Position));
                }
            }
        }

        public Slice() { }

        public Slice(Orientation orientation, int position)
        {
            _orientation = orientation;
            _position = position;
        }

        public object Clone()
        {
            return new Slice(Orientation, Position);
        }
    }
}
