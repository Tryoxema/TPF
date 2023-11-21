using System.Collections.Generic;

namespace TPF.Controls.Specialized.DateTimeRangeNavigator
{
    public class IntervalEqualityComparer : IEqualityComparer<IntervalBase>
    {
        public bool Equals(IntervalBase x, IntervalBase y)
        {
            if (ReferenceEquals(x, y)) return true;

            if (ReferenceEquals(x, null) || ReferenceEquals(y, null)) return false;

            return x.SortingValue.Equals(y.SortingValue);
        }

        public int GetHashCode(IntervalBase obj)
        {
            if (ReferenceEquals(obj, null)) return 0;

            return obj.GetType().GetHashCode();
        }
    }
}