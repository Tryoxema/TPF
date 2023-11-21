using System;

namespace TPF.Controls.Specialized.DateTimeRangeNavigator
{
    public class YearInterval : IntervalBase
    {
        static YearInterval()
        {
            _stringFormatters = new Func<DateTime, string>[]
            {
                date => date.ToString("yyyy"),
                date => date.ToString("yy")
            };
        }

        private TimeSpan _minimumIntervalLength = TimeSpan.FromDays(365);
        public override TimeSpan MinimumIntervalLength
        {
            get { return _minimumIntervalLength; }
        }

        public override DateTime GetIntervalStart(DateTime dateTime)
        {
            return new DateTime(dateTime.Year, 1, 1);
        }

        public override DateTime IncreaseByInterval(DateTime dateTime, int intervalCount)
        {
            return dateTime.AddYears(intervalCount);
        }

        private static readonly Func<DateTime, string>[] _stringFormatters;
        public override Func<DateTime, string>[] StringFormatters
        {
            get { return _stringFormatters; }
        }

        protected override int LabelMeasurementExampleCount
        {
            get { return 1; }
        }
    }
}