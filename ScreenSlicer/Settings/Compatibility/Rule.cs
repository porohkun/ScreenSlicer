using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;

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

        [JsonProperty(nameof(Name))]
        private string _name;

        [JsonIgnore]
        public bool IsDefault => Name == "Default";

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

        public ObservableCollection<ICondition> Conditions { get; private set; }

        [JsonProperty]
        public ObservableCollection<IActionData> MoveWindowSequence { get; private set; }

        public Rule()
        {
            Conditions = new ObservableCollection<ICondition>();
            Conditions.CollectionChanged += Conditions_CollectionChanged;

            MoveWindowSequence = new ObservableCollection<IActionData>();
            MoveWindowSequence.CollectionChanged += MoveWindowSequences_CollectionChanged;
        }

        private void Conditions_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            foreach (ICondition item in (IEnumerable)e?.OldItems ?? Enumerable.Empty<ICondition>())
                item.PropertyChanged -= Condition_PropertyChanged;
            foreach (ICondition item in (IEnumerable)e?.NewItems ?? Enumerable.Empty<ICondition>())
                item.PropertyChanged += Condition_PropertyChanged;
            NotifyPropertyChanged(nameof(Conditions));
        }

        private void Condition_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            NotifyPropertyChanged(nameof(Conditions));
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
