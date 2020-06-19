using Newtonsoft.Json;
using ScreenSlicer.Compatibility;
using ScreenSlicer.Native.Windows;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;

namespace ScreenSlicer
{
    public class CompatibilitySettings : SettingsPartWithNotifier
    {
        [JsonProperty]
        public ObservableCollection<Rule> Rules { get; private set; }

        public CompatibilitySettings()
        {
            Rules = new ObservableCollection<Rule>();
            Rules.CollectionChanged += Rules_CollectionChanged;
        }

        private void Rules_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            foreach (Rule item in (IEnumerable)e?.OldItems ?? Enumerable.Empty<Rule>())
                item.PropertyChanged -= Rule_PropertyChanged;
            foreach (Rule item in (IEnumerable)e?.NewItems ?? Enumerable.Empty<Rule>())
                item.PropertyChanged += Rule_PropertyChanged;
            NotifyPropertyChanged(nameof(Rules));
        }

        private void Rule_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            NotifyPropertyChanged(nameof(Rules));
        }

        public Rule GetRuleForWindow(ISystemWindow window)
        {
            var result = Rules.FirstOrDefault(rule => rule.Conditions.All(c => c.Check(window))) ?? Rules.FirstOrDefault(rule => rule.Name == "Default");
            if (result == null)
                throw new KeyNotFoundException("Compatibility rule 'Default' is missing");
            return result;
        }
    }
}
