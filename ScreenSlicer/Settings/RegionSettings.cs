using Newtonsoft.Json;
using System.Collections;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Drawing;
using System.Linq;

namespace ScreenSlicer
{
    public class RegionSettings : SettingsPartWithNotifier
    {
        private RegionsPreset _currentPreset;

        [JsonProperty(nameof(MinRegionSize), Order = 0)]
        private Size _minRegionSize = new Size(200, 60);

        [JsonProperty(Order = 1)]
        public ObservableCollection<RegionsPreset> Presets { get; private set; }

        [JsonProperty(Order = 2)]
        [JsonConverter(typeof(RegionsPreset.Converter))]
        public RegionsPreset CurrentPreset
        {
            get
            {
                if (_currentPreset == null)
                    _currentPreset = GetPresetByName(string.Empty);
                return _currentPreset;
            }
            set
            {
                var selected = GetPresetByName(value?.Name ?? string.Empty);
                if (_currentPreset != selected)
                {
                    _currentPreset = selected;
                    NotifyPropertyChanged(nameof(CurrentPreset));
                }
            }
        }

        [JsonIgnore]
        public Size MinRegionSize
        {
            get => _minRegionSize;
            set
            {
                if (_minRegionSize != value)
                {
                    _minRegionSize = value;
                    NotifyPropertyChanged(nameof(MinRegionSize));
                }
            }
        }

        public RegionSettings()
        {
            Presets = new ObservableCollection<RegionsPreset>();
            Presets.CollectionChanged += Presets_CollectionChanged;
        }

        public RegionsPreset GetPresetByName(string name)
        {
            return Presets.FirstOrDefault(p => p.Name == name);
        }

        private void Presets_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            foreach (RegionsPreset item in (IEnumerable)e?.OldItems ?? Enumerable.Empty<RegionsPreset>())
                item.PropertyChanged -= Preset_PropertyChanged;
            foreach (RegionsPreset item in (IEnumerable)e?.NewItems ?? Enumerable.Empty<RegionsPreset>())
                item.PropertyChanged += Preset_PropertyChanged;
            NotifyPropertyChanged(nameof(Presets));
        }

        private void Preset_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            NotifyPropertyChanged(nameof(Presets));
        }
    }
}
