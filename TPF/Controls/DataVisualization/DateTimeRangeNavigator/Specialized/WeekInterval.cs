using System;
using System.Windows;
using System.Globalization;

namespace TPF.Controls.Specialized.DateTimeRangeNavigator
{
    public class WeekInterval : IntervalBase
    {
        public WeekInterval()
        {
            _stringFormatters = new Func<DateTime, string>[]
            {
                date => string.Format("{0} {1}, {2}", WeekString, GetWeekOfYear(date), date.ToString("MMMM, yyyy")),
                date => string.Format("{0} {1}, {2}", WeekString, GetWeekOfYear(date), date.ToString("MMM, yyyy")),
                date => string.Format("{0} {1}", WeekString, GetWeekOfYear(date)),
                date => string.Format("{0}{1}", ShortWeekString, GetWeekOfYear(date))
            };
        }
        
        #region WeekString DependencyProperty
        public static readonly DependencyProperty WeekStringProperty = DependencyProperty.Register("WeekString",
            typeof(string),
            typeof(WeekInterval),
            new PropertyMetadata("Week", OnMeasurementRelevantPropertyChanged));

        public string WeekString
        {
            get { return (string)GetValue(WeekStringProperty); }
            set { SetValue(WeekStringProperty, value); }
        }
        #endregion

        #region ShortWeekString DependencyProperty
        public static readonly DependencyProperty ShortWeekStringProperty = DependencyProperty.Register("ShortWeekString",
            typeof(string),
            typeof(WeekInterval),
            new PropertyMetadata("W", OnMeasurementRelevantPropertyChanged));

        public string ShortWeekString
        {
            get { return (string)GetValue(ShortWeekStringProperty); }
            set { SetValue(ShortWeekStringProperty, value); }
        }
        #endregion

        public static DayOfWeek FirstDayOfWeek
        {
            get { return CultureInfo.CurrentCulture.DateTimeFormat.FirstDayOfWeek; }
        }

        public static int GetWeekOfYear(DateTime date)
        {
            var week = CultureInfo.CurrentCulture.Calendar.GetWeekOfYear(date, CalendarWeekRule.FirstFourDayWeek, FirstDayOfWeek);

            return week;
        }

        private TimeSpan _minimumIntervalLength = TimeSpan.FromDays(7);
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
            return dateTime.AddDays(intervalCount * 7);
        }

        private readonly Func<DateTime, string>[] _stringFormatters;
        public override Func<DateTime, string>[] StringFormatters
        {
            get { return _stringFormatters; }
        }
    }
}