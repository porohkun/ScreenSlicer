using Newtonsoft.Json;
using System.ComponentModel;
using System.Globalization;
using WPFLocalizeExtension.Engine;

namespace ScreenSlicer
{
    public class LocalizationSettings : SettingsPartWithNotifier
    {
        [JsonProperty(nameof(Culture))]
        public CultureInfo Culture
        {
            get => LocalizeDictionary.Instance.Culture;
            set
            {
                if (value != Culture)
                    //if (MergedAvailableCultures.Contains(value))
                    LocalizeDictionary.Instance.Culture = value;
            }
        }

        public LocalizationSettings()
        {
            LocalizeDictionary.Instance.PropertyChanged += Instance_PropertyChanged;
        }

        private void Instance_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            NotifyPropertyChanged(e.PropertyName);
        }
    }
}
