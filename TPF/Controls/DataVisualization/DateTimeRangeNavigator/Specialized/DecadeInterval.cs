using System;

namespace TPF.Controls.Specialized.DateTimeRangeNavigator
{
    public class DecadeInterval : IntervalBase
    {
        static DecadeInterval()
        {
            _stringFormatters = new Func<DateTime, string>[]
            {
                date => $"{GetDecadeStart(date).Year} - {GetDecadeEnd(date).Subtract(TimeSpan.FromTicks(1)).Year}"
            };
        }

        public static DateTime GetDecadeStart(DateTime date)
        {
            var decadeStartYear = date.Year - (date.Year % 10);

            if (decadeStartYear <= 0) decadeStartYear = 1;
            
            return new DateTime(decadeStartYear, 1, 1);
        }

        public static DateTime GetDecadeEnd(DateTime date)
        {
            return GetDecadeEnd(date, 1);
        }

        private static DateTime GetDecadeEnd(DateTime date, int intervalSpan)
        {
            var decadeStart = GetDecadeStart(date);

            var decadeSpan = 10 * intervalSpan;

            if (decadeStart == DateTime.MinValue) decadeSpan -= 1;
            
            var endYear = decadeStart.Year + decadeSpan;

            if (endYear <= DateTime.MaxValue.Year) return decadeStart.AddYears(decadeSpan);
            else return DateTime.MaxValue;
        }

        private TimeSpan _minimumIntervalLength = TimeSpan.FromDays(3650);
        public override TimeSpan MinimumIntervalLength
        {
            get { return _minimumIntervalLength; }
        }

        public override DateTime GetIntervalStart(DateTime dateTime)
        {
            return GetDecadeStart(dateTime);
        }

        public override DateTime IncreaseByInterval(DateTime dateTime, int intervalCount)
        {
            return GetDecadeEnd(dateTime, intervalCount);
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