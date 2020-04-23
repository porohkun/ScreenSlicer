using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace ScreenSlicer
{
    [Serializable]
    public class RegionsPreset : INotifyPropertyChanged, ICloneable
    {
        public event PropertyChangedEventHandler PropertyChanged;
        protected void NotifyPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private string _name;
        private ScreenRegion[] _screenRegions;

        [XmlAttribute]
        public string Name
        {
            get => _name;
            set
            {
                if (_name != value)
                {
                    _name = value;
                    NotifyPropertyChanged(nameof(Name));
                }
            }
        }
        public ScreenRegion[] ScreenRegions
        {
            get => _screenRegions;
            set
            {
                if (_screenRegions != value)
                {
                    _screenRegions = value;
                    NotifyPropertyChanged(nameof(ScreenRegion));
                }
            }
        }

        public RegionsPreset() { }

        public RegionsPreset(string name, ScreenRegion[] screenRegions)
        {
            Name = name;
            ScreenRegions = screenRegions;
        }

        public object Clone()
        {
            return new RegionsPreset(Name, ScreenRegions.Select(r => r.Clone() as ScreenRegion).ToArray());
        }
    }
}
