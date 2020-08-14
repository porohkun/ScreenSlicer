using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScreenSlicer.Misc
{
    public class ObservabeCollectionOfClones<T> : ObservableCollection<T> where T : ICloneable
    {
        protected override void InsertItem(int index, T item)
        {
            base.InsertItem(index, (T)item.Clone());
        }

        protected override void MoveItem(int oldIndex, int newIndex)
        {
            base.MoveItem(oldIndex, newIndex);
        }

        protected override void OnCollectionChanged(NotifyCollectionChangedEventArgs e)
        {
            base.OnCollectionChanged(e);
        }

        protected override void RemoveItem(int index)
        {
            base.RemoveItem(index);
        }

        protected override void SetItem(int index, T item)
        {
            base.SetItem(index, (T)item.Clone());
        }
    }
}
