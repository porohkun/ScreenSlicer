using Newtonsoft.Json;
using System;
using System.ComponentModel;
using System.Linq;

namespace ScreenSlicer
{
    public class RegionsPreset : INotifyPropertyChanged, ICloneable
    {
        public event PropertyChangedEventHandler PropertyChanged;
        protected void NotifyPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        [JsonProperty(nameof(Name))]
        private string _name;

        [JsonProperty(nameof(ScreenRegions))]
        private ScreenRegion[] _screenRegions;

        [JsonIgnore]
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

        [JsonIgnore]
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

        public class Converter : JsonConverter<RegionsPreset>
        {
            public Converter()
            { }

            public override RegionsPreset ReadJson(JsonReader reader, Type objectType, RegionsPreset existingValue, bool hasExistingValue, JsonSerializer serializer)
            {
                var data = (string)reader.Value;
                return new RegionsPreset(data, null);
            }

            public override void WriteJson(JsonWriter writer, RegionsPreset value, JsonSerializer serializer)
            {
                writer.WriteValue(value.Name);
            }
        }
    }
}
