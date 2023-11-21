using System;

namespace TPF.Controls.Specialized.DateTimeRangeNavigator
{
    public class HourInterval : IntervalBase
    {
        static HourInterval()
        {
            _stringFormatters = new Func<DateTime, string>[]
            {
                date => date.ToString("HH")
            };
        }

        private TimeSpan _minimumIntervalLength = TimeSpan.FromHours(1);
        public override TimeSpan MinimumIntervalLength
        {
            get { return _minimumIntervalLength; }
        }

        public override DateTime GetIntervalStart(DateTime dateTime)
        {
            return new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, dateTime.Hour, 0, 0);
        }

        public override DateTime IncreaseByInterval(DateTime dateTime, int intervalCount)
        {
            return dateTime.AddHours(intervalCount);
        }

        private static readonly Func<DateTime, string>[] _stringFormatters;
        public override Func<DateTime, string>[] StringFormatters
        {
            get { return _stringFormatters; }
        }
    }
}