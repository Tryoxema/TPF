using System;

namespace TPF.Controls.Specialized.DateTimeRangeNavigator
{
    public class MinuteInterval : IntervalBase
    {
        static MinuteInterval()
        {
            _stringFormatters = new Func<DateTime, string>[]
            {
                date => date.ToString("HH:mm"),
                date => date.ToString("mm")
            };
        }

        private TimeSpan _minimumIntervalLength = TimeSpan.FromMinutes(1);
        public override TimeSpan MinimumIntervalLength
        {
            get { return _minimumIntervalLength; }
        }

        public override DateTime GetIntervalStart(DateTime dateTime)
        {
            return new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, dateTime.Hour, dateTime.Minute, 0);
        }

        public override DateTime IncreaseByInterval(DateTime dateTime, int intervalCount)
        {
            return dateTime.AddMinutes(intervalCount);
        }

        private static readonly Func<DateTime, string>[] _stringFormatters;
        public override Func<DateTime, string>[] StringFormatters
        {
            get { return _stringFormatters; }
        }
    }
}