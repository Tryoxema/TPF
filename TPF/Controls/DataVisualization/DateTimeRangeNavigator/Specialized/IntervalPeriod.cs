using System;

namespace TPF.Controls.Specialized.DateTimeRangeNavigator
{
    public class IntervalPeriod : NotifyObject, IComparable, IComparable<IntervalPeriod>, IComparable<DateTime>
    {
        public IntervalPeriod(IntervalBase interval, DateTime start, DateTime end)
        {
            Interval = interval;
            Start = start;
            End = end;
        }

        public IntervalBase Interval { get; private set; }

        public DateTime Start { get; private set; }

        public DateTime End { get; private set; }

        public TimeSpan Duration
        {
            get { return End - Start; }
        }

        string _label;
        public string Label
        {
            get { return _label; }
            set { SetProperty(ref _label, value); }
        }

        public static bool operator ==(IntervalPeriod periodSpan1, IntervalPeriod periodSpan2)
        {
            if (Equals(periodSpan1, null)) return Equals(periodSpan2, null);

            return periodSpan1.Equals(periodSpan2);
        }

        public static bool operator !=(IntervalPeriod periodSpan1, IntervalPeriod periodSpan2)
        {
            return !(periodSpan1 == periodSpan2);
        }

        public static bool operator <(IntervalPeriod periodSpan1, IntervalPeriod periodSpan2)
        {
            var compareResult = periodSpan1.CompareTo(periodSpan2);

            return compareResult < 0;
        }

        public static bool operator >(IntervalPeriod periodSpan1, IntervalPeriod periodSpan2)
        {
            var compareResult = periodSpan1.CompareTo(periodSpan2);

            return compareResult > 0;
        }

        public override bool Equals(object obj)
        {
            return obj is IntervalPeriod period && GetHashCode() == period.GetHashCode();
        }

        public override int GetHashCode()
        {
            return Start.GetHashCode() ^ End.GetHashCode();
        }

        public int CompareTo(object obj)
        {
            if (obj is DateTime dateTime) return CompareTo(dateTime);
            else if (obj is IntervalPeriod period) return CompareTo(period);

            throw new ArgumentException("Invalid type {0}.", obj.GetType().FullName);
        }

        public int CompareTo(IntervalPeriod other)
        {
            return Start.CompareTo(other.Start);
        }

        public int CompareTo(DateTime other)
        {
            if ((other >= Start) && (other < End)) return 0;
            else if (other < Start) return 1;
            else return -1;
        }
    }
}