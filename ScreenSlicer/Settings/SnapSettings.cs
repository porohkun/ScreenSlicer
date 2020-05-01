using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using WPFLocalizeExtension.Engine;

namespace ScreenSlicer
{
    [Serializable]
    public class SnapSettings : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        protected void NotifyPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private bool _autoFill;
        private bool _snapToMonitors;
        private bool _snapToWindows;
        private bool _snapToRegions;
        private int _snapDistance;

        [XmlAttribute]
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

        [XmlAttribute]
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

        [XmlAttribute]
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

        [XmlAttribute]
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

        [XmlAttribute]
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


        public SnapSettings()
        {

        }

        private void Instance_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            NotifyPropertyChanged(e.PropertyName);
        }
    }
}
