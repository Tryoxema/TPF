using System;
using System.Windows;

namespace TPF.Controls.Specialized.DateTimeRangeNavigator
{
    public class QuarterInterval : IntervalBase
    {
        public QuarterInterval()
        {
            _stringFormatters = new Func<DateTime, string>[]
            {
                date => string.Format("{0} {1}, {2}", QuarterString, GetQuarterOfYear(date), date.ToString("yyyy")),
                date => string.Format("{0} {1}", QuarterString, GetQuarterOfYear(date)),
                date => string.Format("{0}{1} {2}", ShortQuarterString, GetQuarterOfYear(date), date.ToString("yyyy")),
                date => string.Format("{0}{1}", ShortQuarterString, GetQuarterOfYear(date))
            };
        }

        #region QuarterString DependencyProperty
        public static readonly DependencyProperty QuarterStringProperty = DependencyProperty.Register("QuarterString",
            typeof(string),
            typeof(QuarterInterval),
            new PropertyMetadata("Quarter", OnMeasurementRelevantPropertyChanged));

        public string QuarterString
        {
            get { return (string)GetValue(QuarterStringProperty); }
            set { SetValue(QuarterStringProperty, value); }
        }
        #endregion

        #region ShortQuarterString DependencyProperty
        public static readonly DependencyProperty ShortQuarterStringProperty = DependencyProperty.Register("ShortQuarterString",
            typeof(string),
            typeof(QuarterInterval),
            new PropertyMetadata("Q", OnMeasurementRelevantPropertyChanged));

        public string ShortQuarterString
        {
            get { return (string)GetValue(ShortQuarterStringProperty); }
            set { SetValue(ShortQuarterStringProperty, value); }
        }
        #endregion

        public static int GetQuarterOfYear(DateTime date)
        {
            var quarter = ((date.Month - 1) / 3) + 1;

            return quarter;
        }

        public static int GetFirstMonthOfQuarter(DateTime date)
        {
            var quarter = GetQuarterOfYear(date);
            var firstMonth = ((quarter - 1) * 3) + 1;

            return firstMonth;
        }

        private TimeSpan _minimumIntervalLength = TimeSpan.FromDays(90);
        public override TimeSpan MinimumIntervalLength
        {
            get { return _minimumIntervalLength; }
        }

        public override DateTime GetIntervalStart(DateTime dateTime)
        {
            var firstMonth = GetFirstMonthOfQuarter(dateTime);

            return new DateTime(dateTime.Year, firstMonth, 1);
        }

        public override DateTime IncreaseByInterval(DateTime dateTime, int intervalCount)
        {
            return dateTime.AddMonths(intervalCount * 3);
        }

        private readonly Func<DateTime, string>[] _stringFormatters;
        public override Func<DateTime, string>[] StringFormatters
        {
            get { return _stringFormatters; }
        }
    }
}