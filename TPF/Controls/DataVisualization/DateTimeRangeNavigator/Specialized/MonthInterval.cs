using System;

namespace TPF.Controls.Specialized.DateTimeRangeNavigator
{
    public class MonthInterval : IntervalBase
    {
        static MonthInterval()
        {
            _stringFormatters = new Func<DateTime, string>[]
            {
                date => date.ToString("MMMM, yyyy"),
                date => date.ToString("MMM, yyy"),
                date => date.ToString("MMMM"),
                date => date.ToString("MMM"),
                date => date.ToString("MM.yyyy")
            };
        }

        private TimeSpan _minimumIntervalLength = TimeSpan.FromDays(28);
        public override TimeSpan MinimumIntervalLength
        {
            get { return _minimumIntervalLength; }
        }

        public override DateTime GetIntervalStart(DateTime dateTime)
        {
            return new DateTime(dateTime.Year, dateTime.Month, 1);
        }

        public override DateTime IncreaseByInterval(DateTime dateTime, int intervalCount)
        {
            return dateTime.AddMonths(intervalCount);
        }

        private static readonly Func<DateTime, string>[] _stringFormatters;
        public override Func<DateTime, string>[] StringFormatters
        {
            get { return _stringFormatters; }
        }
    }
}