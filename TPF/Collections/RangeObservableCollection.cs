using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;

namespace TPF.Collections
{
    public class RangeObservableCollection<T> : ObservableCollection<T>
    {
        public RangeObservableCollection() { ResetOnChange = true; }

        public RangeObservableCollection(IEnumerable<T> items) : base(items) { ResetOnChange = true; }

        public RangeObservableCollection(List<T> items) : base(items) { ResetOnChange = true; }

        public bool ResetOnChange { get; set; }

        public void Reset()
        {
            OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
        }

        public void AddRange(IEnumerable<T> items)
        {
            InsertRange(Count, items);
        }

        public void InsertRange(int index, IEnumerable<T> items)
        {
            CheckReentrancy();

            var startIndex = index;

            var changed = false;

            foreach (var item in items)
            {
                changed = true;

                Items.Insert(index++, item);
            }

            if (!changed) return;

            if (ResetOnChange) Reset();
            else OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, new List<T>(items), startIndex));

            OnPropertyChanged(new PropertyChangedEventArgs("Count"));
            OnPropertyChanged(new PropertyChangedEventArgs("Item[]"));
        }

        public void RemoveRange(IEnumerable<T> items)
        {
            CheckReentrancy();

            var changed = false;

            foreach (var item in items)
            {
                if (Items.Remove(item)) changed = true;
            }

            if (!changed) return;

            if (ResetOnChange) Reset();
            else OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, new List<T>(items)));

            OnPropertyChanged(new PropertyChangedEventArgs("Count"));
            OnPropertyChanged(new PropertyChangedEventArgs("Item[]"));
        }
    }
}