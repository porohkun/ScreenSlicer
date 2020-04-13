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
        protected void NotifyPropertyChanged(string strPropertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(strPropertyName));
        }

        [XmlAttribute]
        public string Name { get; set; }
        public ScreenRegion[] ScreenRegions { get; set; }

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
