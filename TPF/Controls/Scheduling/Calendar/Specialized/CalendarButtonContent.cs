using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows;

namespace TPF.Controls.Specialized.Calendar
{
    public class CalendarButtonContent : System.ComponentModel.INotifyPropertyChanged
    {
        public CalendarButtonContent()
        {
            IsEnabled = true;
        }

        internal CalendarButtonContent(Controls.Calendar calendar) : this()
        {
            ParentCalendar = calendar;
        }
        
        #region SetProperty
        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;

        bool SetProperty<T>(ref T field, T value, [System.Runtime.CompilerServices.CallerMemberName] string propertyName = null)
        {
            if (EqualityComparer<T>.Default.Equals(field, value)) return false;
            field = value;
            PropertyChanged?.Invoke(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            return true;
        }
        #endregion

        internal Controls.Calendar ParentCalendar;

        DateTime _date;
        public DateTime Date
        {
            get { return _date; }
            set { SetProperty(ref _date, value); }
        }

        CalendarButtonType _displayMode;
        public CalendarButtonType DisplayMode
        {
            get { return _displayMode; }
            set { SetProperty(ref _displayMode, value); }
        }

        bool _isEnabled;
        public bool IsEnabled
        {
            get { return _isEnabled; }
            set { SetProperty(ref _isEnabled, value); }
        }

        bool _isSelected;
        public bool IsSelected
        {
            get { return _isSelected; }
            set { SetProperty(ref _isSelected, value); }
        }

        bool _isFromCurrentView;
        public bool IsFromCurrentView
        {
            get { return _isFromCurrentView; }
            set { SetProperty(ref _isFromCurrentView, value); }
        }

        bool _hide;
        public bool Hide
        {
            get { return _hide; }
            set { SetProperty(ref _hide, value); }
        }

        object _toolTip;
        public object ToolTip
        {
            get { return _toolTip; }
            set { SetProperty(ref _toolTip, value); }
        }

        DataTemplate _contentTemplate;
        public DataTemplate ContentTemplate
        {
            get { return _contentTemplate; }
            set { SetProperty(ref _contentTemplate, value); }
        }

        public override string ToString()
        {
            switch (DisplayMode)
            {
                case CalendarButtonType.Day: return Date.Day.ToString();
                case CalendarButtonType.Month: return Date.ToString("MMM");
                case CalendarButtonType.Year: return Date.Year.ToString();
                case CalendarButtonType.Decade:
                {
                    return $"{Date.Year}-{Environment.NewLine}{Date.AddYears(10).Year}";
                }
                case CalendarButtonType.DayOfWeek: return (ParentCalendar?.Culture?.DateTimeFormat ?? CultureInfo.CurrentCulture.DateTimeFormat).GetShortestDayName(Date.DayOfWeek);
                case CalendarButtonType.WeekNumber:
                {
                    var calendar = ParentCalendar?.Culture?.Calendar ?? CultureInfo.CurrentCulture.Calendar;
                    var rule = ParentCalendar?.CalendarWeekRule ?? ParentCalendar?.Culture?.DateTimeFormat.CalendarWeekRule ?? CultureInfo.CurrentCulture.DateTimeFormat.CalendarWeekRule;
                    var firstDayOfWeek = ParentCalendar?.FirstDayOfWeek ?? ParentCalendar?.Culture?.DateTimeFormat.FirstDayOfWeek ?? CultureInfo.CurrentCulture.DateTimeFormat.FirstDayOfWeek;

                    return calendar.GetWeekOfYear(Date, rule, firstDayOfWeek).ToString();
                }
                case CalendarButtonType.Today: return Date.Day.ToString();
                default: return base.ToString();
            }
        }
    }
}