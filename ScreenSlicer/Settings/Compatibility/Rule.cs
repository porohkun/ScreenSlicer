using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScreenSlicer.Compatibility
{
    [Serializable]
    public class Rule : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected void NotifyPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        [JsonIgnore]
        public bool IsDefault => Name == "Default";

        [JsonProperty]
        public string Name { get; set; }

        public ICondition[] Conditions { get; set; }

        [JsonProperty]
        public ObservableCollection<IActionData> MoveWindowSequence { get; private set; }

        public Rule()
        {
            MoveWindowSequence = new ObservableCollection<IActionData>();
            MoveWindowSequence.CollectionChanged += MoveWindowSequences_CollectionChanged;
        }

        private void MoveWindowSequences_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            foreach (IActionData item in (IEnumerable)e?.OldItems ?? Enumerable.Empty<IActionData>())
                item.PropertyChanged -= MoveWindowSequence_PropertyChanged;
            foreach (IActionData item in (IEnumerable)e?.NewItems ?? Enumerable.Empty<IActionData>())
                item.PropertyChanged += MoveWindowSequence_PropertyChanged;
            NotifyPropertyChanged(nameof(MoveWindowSequence));
        }

        private void MoveWindowSequence_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            NotifyPropertyChanged(nameof(MoveWindowSequence));
        }
    }
}
