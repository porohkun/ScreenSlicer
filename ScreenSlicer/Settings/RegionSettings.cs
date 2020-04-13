﻿using System;
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
    public class RegionSettings : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        protected void NotifyPropertyChanged(string strPropertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(strPropertyName));
        }

        private RegionsPreset _currentPreset;

        [XmlArray]
        public ObservableCollection<RegionsPreset> Presets { get; private set; } = new ObservableCollection<RegionsPreset>() { };

        [XmlAttribute]
        public string CurrentPresetName
        {
            get => CurrentPreset?.Name;
            set
            {
                if (CurrentPresetName != value)
                    CurrentPreset = GetPresetByName(value);
            }
        }

        [XmlIgnore]
        public RegionsPreset CurrentPreset
        {
            get
            {
                if (_currentPreset == null)
                    CurrentPreset = GetPresetByName(string.Empty);
                return _currentPreset;
            }
            set
            {
                if (_currentPreset != value)
                {
                    if (value != null)
                        _currentPreset = value;
                    else
                        _currentPreset = GetPresetByName(string.Empty);

                    NotifyPropertyChanged(nameof(CurrentPreset));
                    NotifyPropertyChanged(nameof(CurrentPresetName));
                }
            }
        }


        public RegionSettings()
        {
            Presets.CollectionChanged += Presets_CollectionChanged;
        }

        public RegionsPreset GetPresetByName(string name)
        {
            return Presets.FirstOrDefault(p => p.Name == name);
        }

        private void Presets_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            NotifyPropertyChanged(nameof(Presets));
        }

    }
}
