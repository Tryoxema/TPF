using System;

namespace TPF.Controls.Specialized.DateTimeRangeNavigator
{
    public class SecondInterval : IntervalBase
    {
        static SecondInterval()
        {
            _stringFormatters = new Func<DateTime, string>[]
            {
                date => date.ToString("HH:mm:ss"),
                date => date.ToString("ss")
            };
        }

        private TimeSpan _minimumIntervalLength = TimeSpan.FromSeconds(1);
        public override TimeSpan MinimumIntervalLength
        {
            get { return _minimumIntervalLength; }
        }

        public override DateTime GetIntervalStart(DateTime dateTime)
        {
            return new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, dateTime.Hour, dateTime.Minute, dateTime.Second);
        }

        public override DateTime IncreaseByInterval(DateTime dateTime, int intervalCount)
        {
            return dateTime.AddSeconds(intervalCount);
        }

        private static readonly Func<DateTime, string>[] _stringFormatters;
        public override Func<DateTime, string>[] StringFormatters
        {
            get { return _stringFormatters; }
        }
    }
}