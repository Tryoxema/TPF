using System.ComponentModel;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;

namespace TPF.Collections
{
    public class RangeObservableCollection<T> : ObservableCollection<T>
    {
        public RangeObservableCollection() { ResetOnChange = true; }

        public RangeObservableCollection(IEnumerable<T> items) : base(items) { ResetOnChange = true; }

        public RangeObservableCollection(List<T> items) : base(items) { ResetOnChange = true; }

        // Manche WPF-Controls unterstützen Add oder Remove nicht mit mehr als einem Item und brauchen stattdessen Reset
        // Die Property steuert das Verhalten und ist für Kompatibilität mit Elementen wie ListBox wichtig
        public bool ResetOnChange { get; set; }

        public bool AreNotificationsSuspended { get; private set; }

        public bool IsDirty { get; protected set; }

        private readonly List<T> _addedItemsCache = new List<T>();
        private readonly List<T> _removedItemsCache = new List<T>();

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

        public void SuspendNotifications()
        {
            AreNotificationsSuspended = true;
        }

        public void ResumeNotifications()
        {
            AreNotificationsSuspended = false;

            if (!IsDirty) return;

            IsDirty = false;

            if (ResetOnChange) Reset();
            else
            {
                if (_addedItemsCache.Count > 0)
                {
                    OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, new List<T>(_addedItemsCache)));
                }

                if (_removedItemsCache.Count > 0)
                {
                    OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, new List<T>(_removedItemsCache)));
                }

                // Wenn beide Caches leer sind wurde eine andere Action als Add und Remove getriggert
                // Da Move und Replace praktisch nie genutzt werden, triggern wir dann einfach Reset
                if (_addedItemsCache.Count == 0 && _removedItemsCache.Count == 0) Reset();
            }

            _addedItemsCache.Clear();
            _removedItemsCache.Clear();

            OnPropertyChanged(new PropertyChangedEventArgs("Count"));
            OnPropertyChanged(new PropertyChangedEventArgs("Item[]"));
        }

        protected override void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            if (AreNotificationsSuspended) return;
            
            base.OnPropertyChanged(e);
        }

        protected override void OnCollectionChanged(NotifyCollectionChangedEventArgs e)
        {
            if (AreNotificationsSuspended)
            {
                IsDirty = true;

                if (e.Action == NotifyCollectionChangedAction.Add) _addedItemsCache.AddRange(e.NewItems.OfType<T>());
                else if (e.Action == NotifyCollectionChangedAction.Remove) _removedItemsCache.AddRange(e.OldItems.OfType<T>());
            }
            else base.OnCollectionChanged(e);
        }
    }
}