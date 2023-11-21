using System;

namespace TPF.Controls.Specialized.DateTimeRangeNavigator
{
    public class DayInterval : IntervalBase
    {
        static DayInterval()
        {
            _stringFormatters = new Func<DateTime, string>[]
            {
                date => date.ToString("dddd, MMMM d, yyyy"),
                date => date.ToString("ddd, MMM d, yyyy"),
                date => date.ToString("dddd, MMMM d"),
                date => date.ToString("ddd, MMM d"),
                date => date.ToString("dddd, d"),
                date => date.ToString("ddd, d"),
                date => date.ToString("d"),
                date => date.Day.ToString()
            };
        }
        
        private TimeSpan _minimumIntervalLength = TimeSpan.FromDays(1);
        public override TimeSpan MinimumIntervalLength
        {
            get { return _minimumIntervalLength; }
        }

        public override DateTime GetIntervalStart(DateTime dateTime)
        {
            return dateTime.Date;
        }

        public override DateTime IncreaseByInterval(DateTime dateTime, int intervalCount)
        {
            return dateTime.AddDays(intervalCount);
        }

        private static readonly Func<DateTime, string>[] _stringFormatters;
        public override Func<DateTime, string>[] StringFormatters
        {
            get { return _stringFormatters; }
        }
    }
}