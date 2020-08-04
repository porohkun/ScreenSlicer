using Newtonsoft.Json;
using System.IO;

namespace ScreenSlicer
{
    public class UpdatesSettings : SettingsPartWithNotifier
    {
        public readonly string Path = @"https://github.com/porohkun/ScreenSlicer/releases/latest/download/";

        [JsonProperty(nameof(AutoUpdate))]
        private bool _autoUpdate = true;

        [JsonProperty(nameof(RunOnStartup))]
        private bool _runOnStartup = true;

        [JsonIgnore]
        public bool AutoUpdate
        {
            get => _autoUpdate;
            set
            {
                if (value != _autoUpdate)
                {
                    _autoUpdate = value;
                    NotifyPropertyChanged(nameof(AutoUpdate));
                }
            }
        }

        [JsonIgnore]
        public bool RunOnStartup
        {
            get => _runOnStartup;
            set
            {
                if (value != _runOnStartup)
                {
                    _runOnStartup = value;
                    NotifyPropertyChanged(nameof(RunOnStartup));
                }
            }
        }
    }
}
