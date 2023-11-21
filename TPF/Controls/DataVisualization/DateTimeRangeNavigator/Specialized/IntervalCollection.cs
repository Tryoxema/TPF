using System.Linq;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using TPF.Collections;

namespace TPF.Controls.Specialized.DateTimeRangeNavigator
{
    public class IntervalCollection : ObservableCollection<IntervalBase>
    {
        public IntervalCollection() { OrderedIntervals = new ReadOnlyCollection<IntervalBase>(_orderedIntervals); }

        public IntervalCollection(IEnumerable<IntervalBase> intervals) : base(intervals) { OrderedIntervals = new ReadOnlyCollection<IntervalBase>(_orderedIntervals); }

        public IntervalCollection(List<IntervalBase> intervals) : base(intervals) { OrderedIntervals = new ReadOnlyCollection<IntervalBase>(_orderedIntervals); }

        private readonly RangeObservableCollection<IntervalBase> _orderedIntervals = new RangeObservableCollection<IntervalBase>();
        public ReadOnlyCollection<IntervalBase> OrderedIntervals { get; }

        protected override void OnCollectionChanged(NotifyCollectionChangedEventArgs e)
        {
            _orderedIntervals.Clear();
            _orderedIntervals.AddRange(GetDistinctIntervals().OrderBy(x => x.SortingValue));

            base.OnCollectionChanged(e);
        }

        public IEnumerable<IntervalBase> GetDistinctIntervals()
        {
            return this.Distinct(new IntervalEqualityComparer());
        }
    }
}