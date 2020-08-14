using Newtonsoft.Json;
using System.IO;

namespace ScreenSlicer
{
    public class MainSettings : SettingsPartWithNotifier
    {
        [JsonIgnore]
        private bool _isActive = true;

        [JsonIgnore]
        public bool IsActive
        {
            get => _isActive;
            set
            {
                if (value != _isActive)
                {
                    _isActive = value;
                    NotifyPropertyChanged(nameof(IsActive));
                }
            }
        }
    }
}
