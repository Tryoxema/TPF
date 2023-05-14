using System;
using System.Windows;
using System.Windows.Controls;
using System.Globalization;
using TPF.Controls.Specialized.Calendar;
using TPF.Controls.Specialized.DateTimePicker;
using TPF.Internal;

namespace TPF.Controls
{
    public class DateTimePicker : Control
    {
        static DateTimePicker()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(DateTimePicker), new FrameworkPropertyMetadata(typeof(DateTimePicker)));
        }

        public DateTimePicker()
        {
            SpecialDays = new SpecialDaysCollection();
        }

        #region ValueChanged RoutedEvent
        public static readonly RoutedEvent ValueChangedEvent = EventManager.RegisterRoutedEvent("ValueChanged",
            RoutingStrategy.Bubble,
            typeof(RoutedPropertyChangedEventHandler<DateTime?>),
            typeof(DateTimePicker));

        public event RoutedPropertyChangedEventHandler<DateTime?> ValueChanged
        {
            add => AddHandler(ValueChangedEvent, value);
            remove => RemoveHandler(ValueChangedEvent, value);
        }
        #endregion

        #region ParseValue RoutedEvent
        public static readonly RoutedEvent ParseValueEvent = EventManager.RegisterRoutedEvent("ParseValue",
            RoutingStrategy.Bubble,
            typeof(ParsingEventHandler<DateTime?>),
            typeof(DateTimePicker));

        public event ParsingEventHandler<DateTime?> ParseValue
        {
            add => AddHandler(ParseValueEvent, value);
            remove => RemoveHandler(ParseValueEvent, value);
        }
        #endregion

        #region IsReadOnly DependencyProperty
        public static readonly DependencyProperty IsReadOnlyProperty = DependencyProperty.Register("IsReadOnly",
            typeof(bool),
            typeof(DateTimePicker),
            new PropertyMetadata(BooleanBoxes.FalseBox));

        public bool IsReadOnly
        {
            get { return (bool)GetValue(IsReadOnlyProperty); }
            set { SetValue(IsReadOnlyProperty, BooleanBoxes.Box(value)); }
        }
        #endregion

        #region IsDropDownOpen DependencyProperty
        public static readonly DependencyProperty IsDropDownOpenProperty = DependencyProperty.Register("IsDropDownOpen",
            typeof(bool),
            typeof(DateTimePicker),
            new FrameworkPropertyMetadata(BooleanBoxes.FalseBox, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        public bool IsDropDownOpen
        {
            get { return (bool)GetValue(IsDropDownOpenProperty); }
            set { SetValue(IsDropDownOpenProperty, BooleanBoxes.Box(value)); }
        }
        #endregion

        #region DropDownButtonVisibility DependencyProperty
        public static readonly DependencyProperty DropDownButtonVisibilityProperty = DependencyProperty.Register("DropDownButtonVisibility",
            typeof(Visibility),
            typeof(DateTimePicker),
            new PropertyMetadata(VisibilityBoxes.VisibleBox));

        public Visibility DropDownButtonVisibility
        {
            get { return (Visibility)GetValue(DropDownButtonVisibilityProperty); }
            set { SetValue(DropDownButtonVisibilityProperty, VisibilityBoxes.Box(value)); }
        }
        #endregion

        #region SelectionOnFocus DependencyProperty
        public static readonly DependencyProperty SelectionOnFocusProperty = DependencyProperty.Register("SelectionOnFocus",
            typeof(SelectionOnFocus),
            typeof(DateTimePicker),
            new PropertyMetadata(SelectionOnFocus.SelectAll));

        public SelectionOnFocus SelectionOnFocus
        {
            get { return (SelectionOnFocus)GetValue(SelectionOnFocusProperty); }
            set { SetValue(SelectionOnFocusProperty, value); }
        }
        #endregion

        #region Watermark DependencyProperty
        public static readonly DependencyProperty WatermarkProperty = DependencyProperty.Register("Watermark",
            typeof(object),
            typeof(DateTimePicker),
            new PropertyMetadata(null));

        public object Watermark
        {
            get { return GetValue(WatermarkProperty); }
            set { SetValue(WatermarkProperty, value); }
        }
        #endregion

        #region WatermarkTemplate DependencyProperty
        public static readonly DependencyProperty WatermarkTemplateProperty = DependencyProperty.Register("WatermarkTemplate",
            typeof(DataTemplate),
            typeof(DateTimePicker),
            new PropertyMetadata(null));

        public DataTemplate WatermarkTemplate
        {
            get { return (DataTemplate)GetValue(WatermarkTemplateProperty); }
            set { SetValue(WatermarkTemplateProperty, value); }
        }
        #endregion

        #region CalendarStyle DependencyProperty
        public static readonly DependencyProperty CalendarStyleProperty = DependencyProperty.Register("CalendarStyle",
            typeof(Style),
            typeof(DateTimePicker),
            new PropertyMetadata(null));

        public Style CalendarStyle
        {
            get { return (Style)GetValue(CalendarStyleProperty); }
            set { SetValue(CalendarStyleProperty, value); }
        }
        #endregion

        #region CalendarDisplayMode DependencyProperty
        public static readonly DependencyProperty CalendarDisplayModeProperty = DependencyProperty.Register("CalendarDisplayMode",
            typeof(DisplayMode),
            typeof(DateTimePicker),
            new PropertyMetadata(DisplayMode.MonthView));

        public DisplayMode CalendarDisplayMode
        {
            get { return (DisplayMode)GetValue(CalendarDisplayModeProperty); }
            set { SetValue(CalendarDisplayModeProperty, value); }
        }
        #endregion

        #region DateSelectionMode DependencyProperty
        public static readonly DependencyProperty DateSelectionModeProperty = DependencyProperty.Register("DateSelectionMode",
            typeof(DateSelectionMode),
            typeof(DateTimePicker),
            new PropertyMetadata(DateSelectionMode.Day));

        public DateSelectionMode DateSelectionMode
        {
            get { return (DateSelectionMode)GetValue(DateSelectionModeProperty); }
            set { SetValue(DateSelectionModeProperty, value); }
        }
        #endregion

        #region ClockStyle DependencyProperty
        public static readonly DependencyProperty ClockStyleProperty = DependencyProperty.Register("ClockStyle",
            typeof(Style),
            typeof(DateTimePicker),
            new PropertyMetadata(null));

        public Style ClockStyle
        {
            get { return (Style)GetValue(ClockStyleProperty); }
            set { SetValue(ClockStyleProperty, value); }
        }
        #endregion

        #region ClockDisplayMode DependencyProperty
        public static readonly DependencyProperty ClockDisplayModeProperty = DependencyProperty.Register("ClockDisplayMode",
            typeof(ClockDisplayMode),
            typeof(DateTimePicker),
            new PropertyMetadata(ClockDisplayMode.Clock));

        public ClockDisplayMode ClockDisplayMode
        {
            get { return (ClockDisplayMode)GetValue(ClockDisplayModeProperty); }
            set { SetValue(ClockDisplayModeProperty, value); }
        }
        #endregion

        #region IsClock24Hours DependencyProperty
        public static readonly DependencyProperty IsClock24HoursProperty = DependencyProperty.Register("IsClock24Hours",
            typeof(bool),
            typeof(DateTimePicker),
            new PropertyMetadata(BooleanBoxes.TrueBox));

        public bool IsClock24Hours
        {
            get { return (bool)GetValue(IsClock24HoursProperty); }
            set { SetValue(IsClock24HoursProperty, BooleanBoxes.Box(value)); }
        }
        #endregion

        #region InputMode DependencyProperty
        public static readonly DependencyProperty InputModeProperty = DependencyProperty.Register("InputMode",
            typeof(InputMode),
            typeof(DateTimePicker),
            new PropertyMetadata(InputMode.DateTime, OnInputModeChanged));

        private static void OnInputModeChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            var instance = (DateTimePicker)sender;

            instance.UpdateDateTimeText();
        }

        public InputMode InputMode
        {
            get { return (InputMode)GetValue(InputModeProperty); }
            set { SetValue(InputModeProperty, value); }
        }
        #endregion

        #region Culture DependencyProperty
        public static readonly DependencyProperty CultureProperty = DependencyProperty.Register("Culture",
            typeof(CultureInfo),
            typeof(DateTimePicker),
            new PropertyMetadata(OnCultureChanged));

        private static void OnCultureChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            var instance = (DateTimePicker)sender;

            instance.UpdateDateTimeText();
        }

        public CultureInfo Culture
        {
            get { return (CultureInfo)GetValue(CultureProperty); }
            set { SetValue(CultureProperty, value); }
        }
        #endregion

        #region Value DependencyProperty
        public static readonly DependencyProperty ValueProperty = DependencyProperty.Register("Value",
            typeof(DateTime?),
            typeof(DateTimePicker),
            new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnValueChanged));

        private static void OnValueChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            var instance = (DateTimePicker)sender;

            instance.OnValueChanged();
            instance.RaiseValueChanged((DateTime?)e.OldValue, (DateTime?)e.NewValue);
        }

        public DateTime? Value
        {
            get { return (DateTime?)GetValue(ValueProperty); }
            set { SetValue(ValueProperty, value); }
        }
        #endregion

        #region Date DependencyProperty
        public static readonly DependencyProperty DateProperty = DependencyProperty.Register("Date",
            typeof(DateTime?),
            typeof(DateTimePicker),
            new PropertyMetadata(null, OnDateChanged));

        private static void OnDateChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            var instance = (DateTimePicker)sender;

            instance.OnValuePartChanged();
        }

        public DateTime? Date
        {
            get { return (DateTime?)GetValue(DateProperty); }
            set { SetValue(DateProperty, value); }
        }
        #endregion

        #region Time DependencyProperty
        public static readonly DependencyProperty TimeProperty = DependencyProperty.Register("Time",
            typeof(DateTime?),
            typeof(DateTimePicker),
            new PropertyMetadata(null, OnTimeChanged));

        private static void OnTimeChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            var instance = (DateTimePicker)sender;

            instance.OnValuePartChanged();
        }

        public DateTime? Time
        {
            get { return (DateTime?)GetValue(TimeProperty); }
            set { SetValue(TimeProperty, value); }
        }
        #endregion

        #region DateTimeText DependencyProperty
        public static readonly DependencyProperty DateTimeTextProperty = DependencyProperty.Register("DateTimeText",
            typeof(string),
            typeof(DateTimePicker),
            new PropertyMetadata("", OnDateTimeTextChanged));

        private static void OnDateTimeTextChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            var instance = (DateTimePicker)sender;

            instance.OnDateTimeTextChanged();
        }

        public string DateTimeText
        {
            get { return (string)GetValue(DateTimeTextProperty); }
            set { SetValue(DateTimeTextProperty, value); }
        }
        #endregion

        #region ShowParsingToolTip DependencyProperty
        public static readonly DependencyProperty ShowParsingToolTipProperty = DependencyProperty.Register("ShowParsingToolTip",
            typeof(bool),
            typeof(DateTimePicker),
            new PropertyMetadata(BooleanBoxes.TrueBox));

        public bool ShowParsingToolTip
        {
            get { return (bool)GetValue(ShowParsingToolTipProperty); }
            set { SetValue(ShowParsingToolTipProperty, BooleanBoxes.Box(value)); }
        }
        #endregion

        #region IsParsingToolTipOpen DependencyProperty
        public static readonly DependencyProperty IsParsingToolTipOpenProperty = DependencyProperty.Register("IsParsingToolTipOpen",
            typeof(bool),
            typeof(DateTimePicker),
            new PropertyMetadata(BooleanBoxes.FalseBox));

        public bool IsParsingToolTipOpen
        {
            get { return (bool)GetValue(IsParsingToolTipOpenProperty); }
            set { SetValue(IsParsingToolTipOpenProperty, BooleanBoxes.Box(value)); }
        }
        #endregion

        #region ParsingToolTip DependencyProperty
        public static readonly DependencyProperty ParsingToolTipProperty = DependencyProperty.Register("ParsingToolTip",
            typeof(object),
            typeof(DateTimePicker),
            new PropertyMetadata(null));

        public object ParsingToolTip
        {
            get { return GetValue(ParsingToolTipProperty); }
            set { SetValue(ParsingToolTipProperty, value); }
        }
        #endregion

        #region ParsingErrorToolTip DependencyProperty
        public static readonly DependencyProperty ParsingErrorToolTipProperty = DependencyProperty.Register("ParsingErrorToolTip",
            typeof(object),
            typeof(DateTimePicker),
            new PropertyMetadata("---"));

        public object ParsingErrorToolTip
        {
            get { return GetValue(ParsingErrorToolTipProperty); }
            set { SetValue(ParsingErrorToolTipProperty, value); }
        }
        #endregion

        #region ParsingToolTipTemplate DependencyProperty
        public static readonly DependencyProperty ParsingToolTipTemplateProperty = DependencyProperty.Register("ParsingToolTipTemplate",
            typeof(DataTemplate),
            typeof(DateTimePicker),
            new PropertyMetadata(null));

        public DataTemplate ParsingToolTipTemplate
        {
            get { return (DataTemplate)GetValue(ParsingToolTipTemplateProperty); }
            set { SetValue(ParsingToolTipTemplateProperty, value); }
        }
        #endregion

        #region DayOfWeekBehavior DependencyProperty
        public static readonly DependencyProperty DayOfWeekBehaviorProperty = DependencyProperty.Register("DayOfWeekBehavior",
            typeof(DayOfWeekBehavior),
            typeof(DateTimePicker),
            new PropertyMetadata(DayOfWeekBehavior.SameWeek, OnDayOfWeekBehaviorChanged));

        private static void OnDayOfWeekBehaviorChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            var instance = (DateTimePicker)sender;

            instance.OnDateTimeTextChanged();
        }

        public DayOfWeekBehavior DayOfWeekBehavior
        {
            get { return (DayOfWeekBehavior)GetValue(DayOfWeekBehaviorProperty); }
            set { SetValue(DayOfWeekBehaviorProperty, value); }
        }
        #endregion

        #region SpecialDays DependencyProperty
        public static readonly DependencyProperty SpecialDaysProperty = DependencyProperty.Register("SpecialDays",
            typeof(SpecialDaysCollection),
            typeof(Calendar),
            new PropertyMetadata(null, null, ConstrainSpecialDaysValue));

        private static object ConstrainSpecialDaysValue(DependencyObject sender, object value)
        {
            return value ?? new SpecialDaysCollection();
        }

        public SpecialDaysCollection SpecialDays
        {
            get { return (SpecialDaysCollection)GetValue(SpecialDaysProperty); }
            set { SetValue(SpecialDaysProperty, value); }
        }
        #endregion

        #region DisplayDate DependencyProperty
        public static readonly DependencyProperty DisplayDateProperty = DependencyProperty.Register("DisplayDate",
            typeof(DateTime),
            typeof(DateTimePicker),
            new PropertyMetadata(DateTime.Today));

        public DateTime DisplayDate
        {
            get { return (DateTime)GetValue(DisplayDateProperty); }
            set { SetValue(DisplayDateProperty, value); }
        }
        #endregion

        #region DisplayDateStart DependencyProperty
        public static readonly DependencyProperty DisplayDateStartProperty = DependencyProperty.Register("DisplayDateStart",
            typeof(DateTime?),
            typeof(DateTimePicker),
            new PropertyMetadata(null));

        public DateTime? DisplayDateStart
        {
            get { return (DateTime?)GetValue(DisplayDateStartProperty); }
            set { SetValue(DisplayDateStartProperty, value); }
        }
        #endregion

        #region DisplayDateEnd DependencyProperty
        public static readonly DependencyProperty DisplayDateEndProperty = DependencyProperty.Register("DisplayDateEnd",
            typeof(DateTime?),
            typeof(DateTimePicker),
            new PropertyMetadata(null));

        public DateTime? DisplayDateEnd
        {
            get { return (DateTime?)GetValue(DisplayDateEndProperty); }
            set { SetValue(DisplayDateEndProperty, value); }
        }
        #endregion

        #region SelectableDateStart DependencyProperty
        public static readonly DependencyProperty SelectableDateStartProperty = DependencyProperty.Register("SelectableDateStart",
            typeof(DateTime?),
            typeof(DateTimePicker),
            new PropertyMetadata(null));

        public DateTime? SelectableDateStart
        {
            get { return (DateTime?)GetValue(SelectableDateStartProperty); }
            set { SetValue(SelectableDateStartProperty, value); }
        }
        #endregion

        #region SelectableDateEnd DependencyProperty
        public static readonly DependencyProperty SelectableDateEndProperty = DependencyProperty.Register("SelectableDateEnd",
            typeof(DateTime?),
            typeof(DateTimePicker),
            new PropertyMetadata(null));

        public DateTime? SelectableDateEnd
        {
            get { return (DateTime?)GetValue(SelectableDateEndProperty); }
            set { SetValue(SelectableDateEndProperty, value); }
        }
        #endregion

        private TextBox _textBox;

        private bool _supressUpdateValue;
        private bool _supressDateTimeTextChanged;

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            if (_textBox != null)
            {
                _textBox.TextChanged -= TextBox_TextChanged;
            }

            _textBox = GetTemplateChild("PART_TextBox") as TextBox;

            if (_textBox != null)
            {
                _textBox.TextChanged += TextBox_TextChanged;
            }
        }

        protected override void OnKeyDown(System.Windows.Input.KeyEventArgs e)
        {
            base.OnKeyDown(e);

            if (e.Key == System.Windows.Input.Key.Enter && !IsDropDownOpen)
            {
                DateTimeText = _textBox?.Text;
            }
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            var text = _textBox.Text;

            if (DesignerHelper.IsInDesignMode) return;

            if (string.IsNullOrWhiteSpace(text))
            {
                ParsingToolTip = null;
                IsParsingToolTipOpen = false;
            }

            if (!ShowParsingToolTip)
            {
                IsParsingToolTipOpen = false;
                return;
            }

            if (TryParseDateTime(text, out var value))
            {
                var dateTimeFormatInfo = GetDateTimeFormatInfo();

                var pattern = GetDateTimePattern(dateTimeFormatInfo);
                ParsingToolTip = value.Value.ToString(pattern, dateTimeFormatInfo);
            }
            else
            {
                ParsingToolTip = ParsingErrorToolTip;
            }

            IsParsingToolTipOpen = text != DateTimeText;
        }

        protected void UpdateDateTimeText()
        {
            var dateTimeFormatInfo = GetDateTimeFormatInfo();

            var pattern = GetDateTimePattern(dateTimeFormatInfo);

            _supressDateTimeTextChanged = true;
            DateTimeText = Value != null ? Value.Value.ToString(pattern, dateTimeFormatInfo) : "";
            _supressDateTimeTextChanged = false;
        }

        private void OnValueChanged()
        {
            UpdateDateTimeText();

            _supressUpdateValue = true;
            Date = Value?.Date;
            Time = Value;
            DisplayDate = Value ?? DateTime.Today;
            _supressUpdateValue = false;
        }

        private void RaiseValueChanged(DateTime? oldValue, DateTime? newValue)
        {
            var e = new RoutedPropertyChangedEventArgs<DateTime?>(oldValue, newValue, ValueChangedEvent);

            RaiseEvent(e);
        }

        private void OnValuePartChanged()
        {
            if (_supressUpdateValue) return;

            if ((InputMode == InputMode.Date && Date == null) || (InputMode == InputMode.Time && Time == null))
            {
                Value = null;
                return;
            }

            var referenceDate = Value ?? DateTime.Today;

            var date = Date ?? referenceDate;
            var time = Time?.TimeOfDay ?? referenceDate.TimeOfDay;

            var calendar = GetDateTimeFormatInfo().Calendar;

            var year = calendar.GetYear(date);
            var month = calendar.GetMonth(date);
            var day = calendar.GetDayOfMonth(date);

            var value = new DateTime(year, month, day, time.Hours, time.Minutes, time.Seconds, calendar);

            Value = value;
        }

        private void OnDateTimeTextChanged()
        {
            if (_supressDateTimeTextChanged) return;

            var text = DateTimeText;

            if (string.IsNullOrWhiteSpace(text))
            {
                Value = null;
                return;
            }

            if (TryParseDateTime(text, out var result))
            {
                Value = result;
            }
            else
            {
                Value = null;
            }

            IsParsingToolTipOpen = false;
        }

        private bool TryParseDateTime(string input, out DateTime? value)
        {
            var isSuccessfull = false;
            value = null;

            var dateTimeFormatInfo = GetDateTimeFormatInfo();
            var referenceDate = Value ?? DateTime.Today;
            var parsingResult = DateTime.Now;

            switch (InputMode)
            {
                case InputMode.Date:
                {
                    isSuccessfull = DateParser.TryParse(input, referenceDate, dateTimeFormatInfo, out parsingResult);
                    if (!isSuccessfull) isSuccessfull = DateParser.TryParseDayOfWeek(input, referenceDate, DayOfWeekBehavior, dateTimeFormatInfo, out parsingResult);
                    if (!isSuccessfull && SpecialDays.Count > 0) isSuccessfull = DateParser.TryParseSpecialDay(input, referenceDate, SpecialDays, dateTimeFormatInfo, out parsingResult);
                    break;
                }
                case InputMode.Time:
                {
                    isSuccessfull = TimeParser.TryParse(input, referenceDate, dateTimeFormatInfo, out parsingResult);
                    break;
                }
                case InputMode.DateTime:
                {
                    isSuccessfull = DateTimeParser.TryParse(input, referenceDate, dateTimeFormatInfo, out parsingResult);
                    if (!isSuccessfull) isSuccessfull = DateTimeParser.TryParseDayOfWeek(input, referenceDate, DayOfWeekBehavior, dateTimeFormatInfo, out parsingResult);
                    if (!isSuccessfull && SpecialDays.Count > 0) isSuccessfull = DateTimeParser.TryParseSpecialDay(input, referenceDate, SpecialDays, dateTimeFormatInfo, out parsingResult);
                    break;
                }
            }

            var e = new ParsingEventArgs<DateTime?>(ParseValueEvent, input, parsingResult, isSuccessfull);

            RaiseEvent(e);

            if (e.IsSuccessful) value = e.Result;

            return e.IsSuccessful;
        }

        private string GetDateTimePattern(DateTimeFormatInfo dateTimeFormatInfo)
        {
            switch (InputMode)
            {
                case InputMode.Date: return dateTimeFormatInfo.ShortDatePattern;
                case InputMode.Time: return dateTimeFormatInfo.ShortTimePattern;
                case InputMode.DateTime:
                default: return $"{dateTimeFormatInfo.ShortDatePattern} {dateTimeFormatInfo.ShortTimePattern}";
            }
        }

        private DateTimeFormatInfo GetDateTimeFormatInfo()
        {
            return Culture?.DateTimeFormat ?? DateTimeFormatInfo.CurrentInfo;
        }
    }
}