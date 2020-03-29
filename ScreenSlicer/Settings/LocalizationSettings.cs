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
    public class LocalizationSettings : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        protected void NotifyPropertyChanged(string strPropertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(strPropertyName));
        }

        [XmlIgnore]
        public ObservableCollection<CultureInfo> MergedAvailableCultures => LocalizeDictionary.Instance.MergedAvailableCultures;

        [XmlIgnore]
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

        [XmlAttribute(nameof(Culture))]
        public string CultureString
        {
            get => Culture.Name;
            set
            {
                if (value != CultureString)
                {
                    var culture = MergedAvailableCultures.FirstOrDefault(c => c.Name == value);
                    if (culture == null)
                        culture = CultureInfo.GetCultureInfo(value);
                    if (culture == null)
                        culture = CultureInfo.CurrentCulture;
                    Culture = culture;
                }
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
