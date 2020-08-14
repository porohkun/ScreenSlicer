using Newtonsoft.Json;
using System;

namespace ScreenSlicer
{
    public class SettingsWindowSettings : WindowStateSettings
    {
        [JsonProperty(nameof(CompatibilityPage_RuleTab_Width))]
        private double _compatibilityPage_RuleTab_Width = 180;

        [JsonIgnore]
        public double CompatibilityPage_RuleTab_Width
        {
            get => _compatibilityPage_RuleTab_Width;
            set
            {
                if (value != _compatibilityPage_RuleTab_Width)
                {
                    _compatibilityPage_RuleTab_Width = value;
                    NotifyPropertyChanged(nameof(CompatibilityPage_RuleTab_Width));
                }
            }
        }
    }
}
