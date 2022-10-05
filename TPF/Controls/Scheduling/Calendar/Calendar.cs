using System;
using System.Linq;
using System.Globalization;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using TPF.Internal;
using TPF.Collections;
using TPF.Controls.Specialized.Calendar;

namespace TPF.Controls
{
    public class Calendar : Control
    {
        static Calendar()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(Calendar), new FrameworkPropertyMetadata(typeof(Calendar)));
        }

        public Calendar()
        {
            // Collections erzeugen
            BlackoutDates = new ObservableCollection<DateTime>();
            SpecialDates = new SpecialDatesCollection();

            SelectedDates.CollectionChanged += OnSelectedDatesCollectionChanged;
        }

        #region DisplayDateChanged RoutedEvent
        public static readonly RoutedEvent DisplayDateChangedEvent = EventManager.RegisterRoutedEvent("DisplayDateChanged",
            RoutingStrategy.Bubble,
            typeof(CalendarDateChangedEventHandler),
            typeof(Calendar));

        public event CalendarDateChangedEventHandler DisplayDateChanged
        {
            add => AddHandler(DisplayDateChangedEvent, value);
            remove => RemoveHandler(DisplayDateChangedEvent, value);
        }
        #endregion

        #region SelectedDateChanged RoutedEvent
        public static readonly RoutedEvent SelectedDateChangedEvent = EventManager.RegisterRoutedEvent("SelectedDateChanged",
            RoutingStrategy.Bubble,
            typeof(CalendarDateChangedEventHandler),
            typeof(Calendar));

        public event CalendarDateChangedEventHandler SelectedDateChanged
        {
            add => AddHandler(SelectedDateChangedEvent, value);
            remove => RemoveHandler(SelectedDateChangedEvent, value);
        }
        #endregion

        #region CalendarModeChanged RoutedEvent
        public static readonly RoutedEvent CalendarModeChangedEvent = EventManager.RegisterRoutedEvent("CalendarModeChanged",
            RoutingStrategy.Bubble,
            typeof(CalendarModeChangedEventHandler),
            typeof(Calendar));

        public event CalendarModeChangedEventHandler CalendarModeChanged
        {
            add => AddHandler(CalendarModeChangedEvent, value);
            remove => RemoveHandler(CalendarModeChangedEvent, value);
        }
        #endregion

        #region HeaderForeground DependencyProperty
        public static readonly DependencyProperty HeaderForegroundProperty = DependencyProperty.Register("HeaderForeground",
            typeof(Brush),
            typeof(Calendar),
            new PropertyMetadata(null));

        public Brush HeaderForeground
        {
            get { return (Brush)GetValue(HeaderForegroundProperty); }
            set { SetValue(HeaderForegroundProperty, value); }
        }
        #endregion

        #region HeaderBackground DependencyProperty
        public static readonly DependencyProperty HeaderBackgroundProperty = DependencyProperty.Register("HeaderBackground",
            typeof(Brush),
            typeof(Calendar),
            new PropertyMetadata(null));

        public Brush HeaderBackground
        {
            get { return (Brush)GetValue(HeaderBackgroundProperty); }
            set { SetValue(HeaderBackgroundProperty, value); }
        }
        #endregion

        #region HeaderVisibility DependencyProperty
        public static readonly DependencyProperty HeaderVisibilityProperty = DependencyProperty.Register("HeaderVisibility",
            typeof(Visibility),
            typeof(Calendar),
            new PropertyMetadata(VisibilityBoxes.VisibleBox));

        public Visibility HeaderVisibility
        {
            get { return (Visibility)GetValue(HeaderVisibilityProperty); }
            set { SetValue(HeaderVisibilityProperty, VisibilityBoxes.Box(value)); }
        }
        #endregion

        #region DisplayMode DependencyProperty
        public static readonly DependencyProperty DisplayModeProperty = DependencyProperty.Register("DisplayMode",
            typeof(DisplayMode),
            typeof(Calendar),
            new PropertyMetadata(DisplayMode.MonthView, OnDisplayModeChanged));

        static void OnDisplayModeChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            var instance = (Calendar)sender;

            var eventArgs = new Specialized.Calendar.CalendarModeChangedEventArgs((DisplayMode)e.OldValue, (DisplayMode)e.NewValue);

            instance.OnDisplayModeChanged(eventArgs);
        }

        public DisplayMode DisplayMode
        {
            get { return (DisplayMode)GetValue(DisplayModeProperty); }
            set { SetValue(DisplayModeProperty, value); }
        }
        #endregion

        #region ViewsHeaderBackground DependencyProperty
        public static readonly DependencyProperty ViewsHeaderBackgroundProperty = DependencyProperty.Register("ViewsHeaderBackground",
            typeof(Brush),
            typeof(Calendar),
            new PropertyMetadata(null));

        public Brush ViewsHeaderBackground
        {
            get { return (Brush)GetValue(ViewsHeaderBackgroundProperty); }
            set { SetValue(ViewsHeaderBackgroundProperty, value); }
        }
        #endregion

        #region ViewsHeaderVisibility DependencyProperty
        public static readonly DependencyProperty ViewsHeaderVisibilityProperty = DependencyProperty.Register("ViewsHeaderVisibility",
            typeof(Visibility),
            typeof(Calendar),
            new PropertyMetadata(VisibilityBoxes.CollapsedBox));

        public Visibility ViewsHeaderVisibility
        {
            get { return (Visibility)GetValue(ViewsHeaderVisibilityProperty); }
            set { SetValue(ViewsHeaderVisibilityProperty, VisibilityBoxes.Box(value)); }
        }
        #endregion

        #region MonthViewStyle DependencyProperty
        public static readonly DependencyProperty MonthViewStyleProperty = DependencyProperty.Register("MonthViewStyle",
            typeof(Style),
            typeof(Calendar),
            new PropertyMetadata(null));

        public Style MonthViewStyle
        {
            get { return (Style)GetValue(MonthViewStyleProperty); }
            set { SetValue(MonthViewStyleProperty, value); }
        }
        #endregion

        #region MonthViewPanel DependencyProperty
        public static readonly DependencyProperty MonthViewPanelProperty = DependencyProperty.Register("MonthViewPanel",
            typeof(ItemsPanelTemplate),
            typeof(Calendar),
            new PropertyMetadata(null));

        public ItemsPanelTemplate MonthViewPanel
        {
            get { return (ItemsPanelTemplate)GetValue(MonthViewPanelProperty); }
            set { SetValue(MonthViewPanelProperty, value); }
        }
        #endregion

        #region MonthViewHeaderFormat DependencyProperty
        public static readonly DependencyProperty MonthViewHeaderFormatProperty = DependencyProperty.Register("MonthViewHeaderFormat",
            typeof(string),
            typeof(Calendar),
            new PropertyMetadata(null));

        public string MonthViewHeaderFormat
        {
            get { return (string)GetValue(MonthViewHeaderFormatProperty); }
            set { SetValue(MonthViewHeaderFormatProperty, value); }
        }
        #endregion

        #region DayButtonStyle DependencyProperty
        public static readonly DependencyProperty DayButtonStyleProperty = DependencyProperty.Register("DayButtonStyle",
            typeof(Style),
            typeof(Calendar),
            new PropertyMetadata(null));

        public Style DayButtonStyle
        {
            get { return (Style)GetValue(DayButtonStyleProperty); }
            set { SetValue(DayButtonStyleProperty, value); }
        }
        #endregion

        #region DayButtonStyleSelector DependencyProperty
        public static readonly DependencyProperty DayButtonStyleSelectorProperty = DependencyProperty.Register("DayButtonStyleSelector",
            typeof(StyleSelector),
            typeof(Calendar),
            new PropertyMetadata(null));

        public StyleSelector DayButtonStyleSelector
        {
            get { return (StyleSelector)GetValue(DayButtonStyleSelectorProperty); }
            set { SetValue(DayButtonStyleSelectorProperty, value); }
        }
        #endregion

        #region DayTemplate DependencyProperty
        public static readonly DependencyProperty DayTemplateProperty = DependencyProperty.Register("DayTemplate",
            typeof(DataTemplate),
            typeof(Calendar),
            new PropertyMetadata(null));

        public DataTemplate DayTemplate
        {
            get { return (DataTemplate)GetValue(DayTemplateProperty); }
            set { SetValue(DayTemplateProperty, value); }
        }
        #endregion

        #region DayTemplateSelector DependencyProperty
        public static readonly DependencyProperty DayTemplateSelectorProperty = DependencyProperty.Register("DayTemplateSelector",
            typeof(DataTemplateSelector),
            typeof(Calendar),
            new PropertyMetadata(null));

        public DataTemplateSelector DayTemplateSelector
        {
            get { return (DataTemplateSelector)GetValue(DayTemplateSelectorProperty); }
            set { SetValue(DayTemplateSelectorProperty, value); }
        }
        #endregion

        #region YearViewStyle DependencyProperty
        public static readonly DependencyProperty YearViewStyleProperty = DependencyProperty.Register("YearViewStyle",
            typeof(Style),
            typeof(Calendar),
            new PropertyMetadata(null));

        public Style YearViewStyle
        {
            get { return (Style)GetValue(YearViewStyleProperty); }
            set { SetValue(YearViewStyleProperty, value); }
        }
        #endregion

        #region YearViewPanel DependencyProperty
        public static readonly DependencyProperty YearViewPanelProperty = DependencyProperty.Register("YearViewPanel",
            typeof(ItemsPanelTemplate),
            typeof(Calendar),
            new PropertyMetadata(null));

        public ItemsPanelTemplate YearViewPanel
        {
            get { return (ItemsPanelTemplate)GetValue(YearViewPanelProperty); }
            set { SetValue(YearViewPanelProperty, value); }
        }
        #endregion

        #region YearViewHeaderFormat DependencyProperty
        public static readonly DependencyProperty YearViewHeaderFormatProperty = DependencyProperty.Register("YearViewHeaderFormat",
            typeof(string),
            typeof(Calendar),
            new PropertyMetadata(null));

        public string YearViewHeaderFormat
        {
            get { return (string)GetValue(YearViewHeaderFormatProperty); }
            set { SetValue(YearViewHeaderFormatProperty, value); }
        }
        #endregion

        #region MonthButtonStyle DependencyProperty
        public static readonly DependencyProperty MonthButtonStyleProperty = DependencyProperty.Register("MonthButtonStyle",
            typeof(Style),
            typeof(Calendar),
            new PropertyMetadata(null));

        public Style MonthButtonStyle
        {
            get { return (Style)GetValue(MonthButtonStyleProperty); }
            set { SetValue(MonthButtonStyleProperty, value); }
        }
        #endregion

        #region MonthButtonStyleSelector DependencyProperty
        public static readonly DependencyProperty MonthButtonStyleSelectorProperty = DependencyProperty.Register("MonthButtonStyleSelector",
            typeof(StyleSelector),
            typeof(Calendar),
            new PropertyMetadata(null));

        public StyleSelector MonthButtonStyleSelector
        {
            get { return (StyleSelector)GetValue(MonthButtonStyleSelectorProperty); }
            set { SetValue(MonthButtonStyleSelectorProperty, value); }
        }
        #endregion

        #region MonthTemplate DependencyProperty
        public static readonly DependencyProperty MonthTemplateProperty = DependencyProperty.Register("MonthTemplate",
            typeof(DataTemplate),
            typeof(Calendar),
            new PropertyMetadata(null));

        public DataTemplate MonthTemplate
        {
            get { return (DataTemplate)GetValue(MonthTemplateProperty); }
            set { SetValue(MonthTemplateProperty, value); }
        }
        #endregion

        #region MonthTemplateSelector DependencyProperty
        public static readonly DependencyProperty MonthTemplateSelectorProperty = DependencyProperty.Register("MonthTemplateSelector",
            typeof(DataTemplateSelector),
            typeof(Calendar),
            new PropertyMetadata(null));

        public DataTemplateSelector MonthTemplateSelector
        {
            get { return (DataTemplateSelector)GetValue(MonthTemplateSelectorProperty); }
            set { SetValue(MonthTemplateSelectorProperty, value); }
        }
        #endregion

        #region DecadeViewStyle DependencyProperty
        public static readonly DependencyProperty DecadeViewStyleProperty = DependencyProperty.Register("DecadeViewStyle",
            typeof(Style),
            typeof(Calendar),
            new PropertyMetadata(null));

        public Style DecadeViewStyle
        {
            get { return (Style)GetValue(DecadeViewStyleProperty); }
            set { SetValue(DecadeViewStyleProperty, value); }
        }
        #endregion

        #region DecadeViewPanel DependencyProperty
        public static readonly DependencyProperty DecadeViewPanelProperty = DependencyProperty.Register("DecadeViewPanel",
            typeof(ItemsPanelTemplate),
            typeof(Calendar),
            new PropertyMetadata(null));

        public ItemsPanelTemplate DecadeViewPanel
        {
            get { return (ItemsPanelTemplate)GetValue(DecadeViewPanelProperty); }
            set { SetValue(DecadeViewPanelProperty, value); }
        }
        #endregion

        #region DecadeViewHeaderFormat DependencyProperty
        public static readonly DependencyProperty DecadeViewHeaderFormatProperty = DependencyProperty.Register("DecadeViewHeaderFormat",
            typeof(string),
            typeof(Calendar),
            new PropertyMetadata(null));

        public string DecadeViewHeaderFormat
        {
            get { return (string)GetValue(DecadeViewHeaderFormatProperty); }
            set { SetValue(DecadeViewHeaderFormatProperty, value); }
        }
        #endregion

        #region YearButtonStyle DependencyProperty
        public static readonly DependencyProperty YearButtonStyleProperty = DependencyProperty.Register("YearButtonStyle",
            typeof(Style),
            typeof(Calendar),
            new PropertyMetadata(null));

        public Style YearButtonStyle
        {
            get { return (Style)GetValue(YearButtonStyleProperty); }
            set { SetValue(YearButtonStyleProperty, value); }
        }
        #endregion

        #region YearButtonStyleSelector DependencyProperty
        public static readonly DependencyProperty YearButtonStyleSelectorProperty = DependencyProperty.Register("YearButtonStyleSelector",
            typeof(StyleSelector),
            typeof(Calendar),
            new PropertyMetadata(null));

        public StyleSelector YearButtonStyleSelector
        {
            get { return (StyleSelector)GetValue(YearButtonStyleSelectorProperty); }
            set { SetValue(YearButtonStyleSelectorProperty, value); }
        }
        #endregion

        #region YearTemplate DependencyProperty
        public static readonly DependencyProperty YearTemplateProperty = DependencyProperty.Register("YearTemplate",
            typeof(DataTemplate),
            typeof(Calendar),
            new PropertyMetadata(null));

        public DataTemplate YearTemplate
        {
            get { return (DataTemplate)GetValue(YearTemplateProperty); }
            set { SetValue(YearTemplateProperty, value); }
        }
        #endregion

        #region YearTemplateSelector DependencyProperty
        public static readonly DependencyProperty YearTemplateSelectorProperty = DependencyProperty.Register("YearTemplateSelector",
            typeof(DataTemplateSelector),
            typeof(Calendar),
            new PropertyMetadata(null));

        public DataTemplateSelector YearTemplateSelector
        {
            get { return (DataTemplateSelector)GetValue(YearTemplateSelectorProperty); }
            set { SetValue(YearTemplateSelectorProperty, value); }
        }
        #endregion

        #region CenturyViewStyle DependencyProperty
        public static readonly DependencyProperty CenturyViewStyleProperty = DependencyProperty.Register("CenturyViewStyle",
            typeof(Style),
            typeof(Calendar),
            new PropertyMetadata(null));

        public Style CenturyViewStyle
        {
            get { return (Style)GetValue(CenturyViewStyleProperty); }
            set { SetValue(CenturyViewStyleProperty, value); }
        }
        #endregion

        #region CenturyViewPanel DependencyProperty
        public static readonly DependencyProperty CenturyViewPanelProperty = DependencyProperty.Register("CenturyViewPanel",
            typeof(ItemsPanelTemplate),
            typeof(Calendar),
            new PropertyMetadata(null));

        public ItemsPanelTemplate CenturyViewPanel
        {
            get { return (ItemsPanelTemplate)GetValue(CenturyViewPanelProperty); }
            set { SetValue(CenturyViewPanelProperty, value); }
        }
        #endregion

        #region CenturyViewHeaderFormat DependencyProperty
        public static readonly DependencyProperty CenturyViewHeaderFormatProperty = DependencyProperty.Register("CenturyViewHeaderFormat",
            typeof(string),
            typeof(Calendar),
            new PropertyMetadata(null));

        public string CenturyViewHeaderFormat
        {
            get { return (string)GetValue(CenturyViewHeaderFormatProperty); }
            set { SetValue(CenturyViewHeaderFormatProperty, value); }
        }
        #endregion

        #region DecadeButtonStyle DependencyProperty
        public static readonly DependencyProperty DecadeButtonStyleProperty = DependencyProperty.Register("DecadeButtonStyle",
            typeof(Style),
            typeof(Calendar),
            new PropertyMetadata(null));

        public Style DecadeButtonStyle
        {
            get { return (Style)GetValue(DecadeButtonStyleProperty); }
            set { SetValue(DecadeButtonStyleProperty, value); }
        }
        #endregion

        #region DecadeButtonStyleSelector DependencyProperty
        public static readonly DependencyProperty DecadeButtonStyleSelectorProperty = DependencyProperty.Register("DecadeButtonStyleSelector",
            typeof(StyleSelector),
            typeof(Calendar),
            new PropertyMetadata(null));

        public StyleSelector DecadeButtonStyleSelector
        {
            get { return (StyleSelector)GetValue(DecadeButtonStyleSelectorProperty); }
            set { SetValue(DecadeButtonStyleSelectorProperty, value); }
        }
        #endregion

        #region DecadeTemplate DependencyProperty
        public static readonly DependencyProperty DecadeTemplateProperty = DependencyProperty.Register("DecadeTemplate",
            typeof(DataTemplate),
            typeof(Calendar),
            new PropertyMetadata(null));

        public DataTemplate DecadeTemplate
        {
            get { return (DataTemplate)GetValue(DecadeTemplateProperty); }
            set { SetValue(DecadeTemplateProperty, value); }
        }
        #endregion

        #region DecadeTemplateSelector DependencyProperty
        public static readonly DependencyProperty DecadeTemplateSelectorProperty = DependencyProperty.Register("DecadeTemplateSelector",
            typeof(DataTemplateSelector),
            typeof(Calendar),
            new PropertyMetadata(null));

        public DataTemplateSelector DecadeTemplateSelector
        {
            get { return (DataTemplateSelector)GetValue(DecadeTemplateSelectorProperty); }
            set { SetValue(DecadeTemplateSelectorProperty, value); }
        }
        #endregion

        #region Columns DependencyProperty
        public static readonly DependencyProperty ColumnsProperty = DependencyProperty.Register("Columns",
            typeof(int),
            typeof(Calendar),
            new PropertyMetadata(1, OnColumnsChanged, ConstrainColumns));

        private static void OnColumnsChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            var instance = (Calendar)sender;

            instance.UpdateColumnsAndRows();
        }

        internal static object ConstrainColumns(DependencyObject sender, object value)
        {
            var intValue = (int)value;

            if (intValue < 1) intValue = 1;
            else if (intValue > 4) intValue = 4;

            return intValue;
        }

        public int Columns
        {
            get { return (int)GetValue(ColumnsProperty); }
            set { SetValue(ColumnsProperty, value); }
        }
        #endregion

        #region Rows DependencyProperty
        public static readonly DependencyProperty RowsProperty = DependencyProperty.Register("Rows",
            typeof(int),
            typeof(Calendar),
            new PropertyMetadata(1, OnRowsChanged, ConstrainRows));

        private static void OnRowsChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            var instance = (Calendar)sender;

            instance.UpdateColumnsAndRows();
        }

        internal static object ConstrainRows(DependencyObject sender, object value)
        {
            var intValue = (int)value;

            if (intValue < 1) intValue = 1;
            else if (intValue > 4) intValue = 4;

            return intValue;
        }

        public int Rows
        {
            get { return (int)GetValue(RowsProperty); }
            set { SetValue(RowsProperty, value); }
        }
        #endregion

        #region DisplayDate DependencyProperty
        public static readonly DependencyProperty DisplayDateProperty = DependencyProperty.Register("DisplayDate",
            typeof(DateTime),
            typeof(Calendar),
            new PropertyMetadata(DateTime.Today, OnDisplayDateChanged));

        private static void OnDisplayDateChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            var instance = (Calendar)sender;

            var eventArgs = new Specialized.Calendar.CalendarDateChangedEventArgs(DisplayDateChangedEvent, (DateTime)e.NewValue, (DateTime)e.OldValue);

            instance.OnDisplayDateChanged(eventArgs);
        }

        public DateTime DisplayDate
        {
            get { return (DateTime)GetValue(DisplayDateProperty); }
            set { SetValue(DisplayDateProperty, value); }
        }
        #endregion

        #region DisplayDateStart DependencyProperty
        public static readonly DependencyProperty DisplayDateStartProperty = DependencyProperty.Register("DisplayDateStart",
            typeof(DateTime?),
            typeof(Calendar),
            new PropertyMetadata(null, OnDisplayDateStartChanged));

        static void OnDisplayDateStartChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            var instance = (Calendar)sender;

            instance.PopulateActiveViews();
        }

        public DateTime? DisplayDateStart
        {
            get { return (DateTime?)GetValue(DisplayDateStartProperty); }
            set { SetValue(DisplayDateStartProperty, value); }
        }
        #endregion

        #region DisplayDateEnd DependencyProperty
        public static readonly DependencyProperty DisplayDateEndProperty = DependencyProperty.Register("DisplayDateEnd",
            typeof(DateTime?),
            typeof(Calendar),
            new PropertyMetadata(null, OnDisplayDateEndChanged));

        private static void OnDisplayDateEndChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            var instance = (Calendar)sender;

            instance.PopulateActiveViews();
        }

        public DateTime? DisplayDateEnd
        {
            get { return (DateTime?)GetValue(DisplayDateEndProperty); }
            set { SetValue(DisplayDateEndProperty, value); }
        }
        #endregion

        #region SelectedDate DependencyProperty
        public static readonly DependencyProperty SelectedDateProperty = DependencyProperty.Register("SelectedDate",
            typeof(DateTime?),
            typeof(Calendar),
            new PropertyMetadata(null, OnSelectedDateChanged));

        private static void OnSelectedDateChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            var instance = (Calendar)sender;

            var eventArgs = new Specialized.Calendar.CalendarDateChangedEventArgs(SelectedDateChangedEvent, (DateTime?)e.NewValue, (DateTime?)e.OldValue);

            instance.OnSelectedDateChanged(eventArgs);
        }

        public DateTime? SelectedDate
        {
            get { return (DateTime?)GetValue(SelectedDateProperty); }
            set { SetValue(SelectedDateProperty, value); }
        }
        #endregion

        #region SelectableDateStart DependencyProperty
        public static readonly DependencyProperty SelectableDateStartProperty = DependencyProperty.Register("SelectableDateStart",
            typeof(DateTime?),
            typeof(Calendar),
            new PropertyMetadata(null, OnSelectableDateStartChanged));

        private static void OnSelectableDateStartChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            var instance = (Calendar)sender;

            instance.PopulateActiveViews();
        }

        public DateTime? SelectableDateStart
        {
            get { return (DateTime?)GetValue(SelectableDateStartProperty); }
            set { SetValue(SelectableDateStartProperty, value); }
        }
        #endregion

        #region SelectableDateEnd DependencyProperty
        public static readonly DependencyProperty SelectableDateEndProperty = DependencyProperty.Register("SelectableDateEnd",
            typeof(DateTime?),
            typeof(Calendar),
            new PropertyMetadata(null, OnSelectableDateEndChanged));

        private static void OnSelectableDateEndChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            var instance = (Calendar)sender;

            instance.PopulateActiveViews();
        }

        public DateTime? SelectableDateEnd
        {
            get { return (DateTime?)GetValue(SelectableDateEndProperty); }
            set { SetValue(SelectableDateEndProperty, value); }
        }
        #endregion

        #region BlackoutDates DependencyProperty
        public static readonly DependencyProperty BlackoutDatesProperty = DependencyProperty.Register("BlackoutDates",
            typeof(IEnumerable<DateTime>),
            typeof(Calendar),
            new PropertyMetadata(null, OnBlackoutDatesChanged));

        private static void OnBlackoutDatesChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            var instance = (Calendar)sender;

            if (e.OldValue != null && e.OldValue is INotifyCollectionChanged oldCollection)
            {
                instance.UnHookBlackoutDatesCollection(oldCollection);
            }

            if (e.NewValue != null)
            {
                if (e.NewValue is INotifyCollectionChanged newCollection)
                {
                    instance.HookupBlackoutDatesCollection(newCollection);
                }

                if (instance.IsInitialized) instance.PopulateActiveViews();
            }
            else
            {
                if (instance.IsInitialized) instance.PopulateActiveViews();
            }
        }

        public IEnumerable<DateTime> BlackoutDates
        {
            get { return (IEnumerable<DateTime>)GetValue(BlackoutDatesProperty); }
            set { SetValue(BlackoutDatesProperty, value); }
        }
        #endregion

        #region SpecialDates DependencyProperty
        public static readonly DependencyProperty SpecialDatesProperty = DependencyProperty.Register("SpecialDates",
            typeof(SpecialDatesCollection),
            typeof(Calendar),
            new PropertyMetadata(null, OnSpecialDatesChanged, ConstrainSpecialDatesValue));

        internal static object ConstrainSpecialDatesValue(DependencyObject sender, object value)
        {
            if (value == null) return new SpecialDatesCollection();
            else return value;
        }

        private static void OnSpecialDatesChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            var instance = (Calendar)sender;

            if (e.OldValue != null && e.OldValue is INotifyCollectionChanged oldCollection)
            {
                instance.UnHookSpecialDatesCollection(oldCollection);
            }

            if (e.NewValue != null)
            {
                if (e.NewValue is INotifyCollectionChanged newCollection)
                {
                    instance.HookupSpecialDatesCollection(newCollection);
                }

                if (instance.IsInitialized) instance.PopulateActiveViews();
            }
            else
            {
                if (instance.IsInitialized) instance.PopulateActiveViews();
            }
        }

        public SpecialDatesCollection SpecialDates
        {
            get { return (SpecialDatesCollection)GetValue(SpecialDatesProperty); }
            set { SetValue(SpecialDatesProperty, value); }
        }
        #endregion

        #region IsReadOnly DependencyProperty
        public static readonly DependencyProperty IsReadOnlyProperty = DependencyProperty.Register("IsReadOnly",
            typeof(bool),
            typeof(Calendar),
            new PropertyMetadata(BooleanBoxes.FalseBox));

        public bool IsReadOnly
        {
            get { return (bool)GetValue(IsReadOnlyProperty); }
            set { SetValue(IsReadOnlyProperty, BooleanBoxes.Box(value)); }
        }
        #endregion

        #region AreWeekNamesVisible DependencyProperty
        public static readonly DependencyProperty AreWeekNamesVisibleProperty = DependencyProperty.Register("AreWeekNamesVisible",
            typeof(bool),
            typeof(Calendar),
            new PropertyMetadata(BooleanBoxes.TrueBox));

        public bool AreWeekNamesVisible
        {
            get { return (bool)GetValue(AreWeekNamesVisibleProperty); }
            set { SetValue(AreWeekNamesVisibleProperty, BooleanBoxes.Box(value)); }
        }
        #endregion

        #region AreWeekNumbersVisible DependencyProperty
        public static readonly DependencyProperty AreWeekNumbersVisibleProperty = DependencyProperty.Register("AreWeekNumbersVisible",
            typeof(bool),
            typeof(Calendar),
            new PropertyMetadata(BooleanBoxes.TrueBox));

        public bool AreWeekNumbersVisible
        {
            get { return (bool)GetValue(AreWeekNumbersVisibleProperty); }
            set { SetValue(AreWeekNumbersVisibleProperty, BooleanBoxes.Box(value)); }
        }
        #endregion

        #region IsTodayHighlighted DependencyProperty
        public static readonly DependencyProperty IsTodayHighlightedProperty = DependencyProperty.Register("IsTodayHighlighted",
            typeof(bool),
            typeof(Calendar),
            new PropertyMetadata(BooleanBoxes.TrueBox, OnIsTodayHighlightedChanged));

        static void OnIsTodayHighlightedChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            var instance = (Calendar)sender;

            instance.OnIsTodayHighlightedChanged();
        }

        public bool IsTodayHighlighted
        {
            get { return (bool)GetValue(IsTodayHighlightedProperty); }
            set { SetValue(IsTodayHighlightedProperty, BooleanBoxes.Box(value)); }
        }
        #endregion

        #region SelectionMode DependencyProperty
        public static readonly DependencyProperty SelectionModeProperty = DependencyProperty.Register("SelectionMode",
            typeof(SelectionMode),
            typeof(Calendar),
            new PropertyMetadata(SelectionMode.Single, OnSelectionModeChanged));

        private static void OnSelectionModeChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            var instance = (Calendar)sender;

            instance.OnSelectionModeChanged();
        }

        public SelectionMode SelectionMode
        {
            get { return (SelectionMode)GetValue(SelectionModeProperty); }
            set { SetValue(SelectionModeProperty, value); }
        }
        #endregion

        #region DateSelectionMode DependencyProperty
        public static readonly DependencyProperty DateSelectionModeProperty = DependencyProperty.Register("DateSelectionMode",
            typeof(DateSelectionMode),
            typeof(Calendar),
            new PropertyMetadata(DateSelectionMode.Day));

        public DateSelectionMode DateSelectionMode
        {
            get { return (DateSelectionMode)GetValue(DateSelectionModeProperty); }
            set { SetValue(DateSelectionModeProperty, value); }
        }
        #endregion

        #region FirstDayOfWeek DependencyProperty
        public static readonly DependencyProperty FirstDayOfWeekProperty = DependencyProperty.Register("FirstDayOfWeek",
            typeof(DayOfWeek?),
            typeof(Calendar),
            new PropertyMetadata(null));

        public DayOfWeek? FirstDayOfWeek
        {
            get { return (DayOfWeek?)GetValue(FirstDayOfWeekProperty); }
            set { SetValue(FirstDayOfWeekProperty, value); }
        }
        #endregion

        #region CalendarWeekRule DependencyProperty
        public static readonly DependencyProperty CalendarWeekRuleProperty = DependencyProperty.Register("CalendarWeekRule",
            typeof(CalendarWeekRule?),
            typeof(Calendar),
            new PropertyMetadata(null));

        public CalendarWeekRule? CalendarWeekRule
        {
            get { return (CalendarWeekRule?)GetValue(CalendarWeekRuleProperty); }
            set { SetValue(CalendarWeekRuleProperty, value); }
        }
        #endregion

        #region Culture DependencyProperty
        public static readonly DependencyProperty CultureProperty = DependencyProperty.Register("Culture",
            typeof(CultureInfo),
            typeof(Calendar),
            new PropertyMetadata(null));

        public CultureInfo Culture
        {
            get { return (CultureInfo)GetValue(CultureProperty); }
            set { SetValue(CultureProperty, value); }
        }
        #endregion

        List<DateTime> _internalSelectedDates;
        private List<DateTime> InternalSelectedDates
        {
            get { return _internalSelectedDates ?? (_internalSelectedDates = new List<DateTime>()); }
        }

        RangeObservableCollection<DateTime> _selectedDates;
        public RangeObservableCollection<DateTime> SelectedDates
        {
            get { return _selectedDates ?? (_selectedDates = new RangeObservableCollection<DateTime>()); }
        }

        Dictionary<DateTime, object> _toolTips;
        private Dictionary<DateTime, object> ToolTips
        {
            get { return _toolTips ?? (_toolTips = new Dictionary<DateTime, object>()); }
        }

        private bool _suppressActiveViewsChanging;
        private bool _selectionInProgress;
        private DateTime? _firstDisplayedDate;
        private DateTime? _lastDisplayedDate;
        private DateTime? _lastArrowKeyDate;

        internal ButtonBase HeaderButton;
        internal ButtonBase MoveLeftButton;
        internal ButtonBase MoveRightButton;
        internal Panel MonthViewsPanel;
        internal Panel YearViewsPanel;
        internal Panel DecadeViewsPanel;
        internal Panel CenturyViewsPanel;

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            if (HeaderButton != null) HeaderButton.Click -= HeaderButton_Click;
            if (MoveLeftButton != null) MoveLeftButton.Click -= MoveLeftButton_Click;
            if (MoveRightButton != null) MoveRightButton.Click -= MoveRightButton_Click;

            HeaderButton = GetTemplateChild("PART_HeaderButton") as ButtonBase;
            MoveLeftButton = GetTemplateChild("PART_MoveLeftButton") as ButtonBase;
            MoveRightButton = GetTemplateChild("PART_MoveRightButton") as ButtonBase;
            MonthViewsPanel = GetTemplateChild("PART_MonthViewsPanel") as Panel;
            YearViewsPanel = GetTemplateChild("PART_YearViewsPanel") as Panel;
            DecadeViewsPanel = GetTemplateChild("PART_DecadeViewsPanel") as Panel;
            CenturyViewsPanel = GetTemplateChild("PART_CenturyViewsPanel") as Panel;

            if (HeaderButton != null) HeaderButton.Click += HeaderButton_Click;
            if (MoveLeftButton != null) MoveLeftButton.Click += MoveLeftButton_Click;
            if (MoveRightButton != null) MoveRightButton.Click += MoveRightButton_Click;

            _suppressActiveViewsChanging = true;
            if (DateSelectionMode == DateSelectionMode.Month && DisplayMode == DisplayMode.MonthView) DisplayMode = DisplayMode.YearView;
            else if (DateSelectionMode == DateSelectionMode.Year && (DisplayMode == DisplayMode.MonthView || DisplayMode == DisplayMode.YearView)) DisplayMode = DisplayMode.DecadeView;
            _suppressActiveViewsChanging = false;

            UpdateColumnsAndRows();
            ChangeActiveViews();

            // Wenn ein SelectedDate gesetzt wurde und DisplayDate auf dem Standard ist, dann DisplayDate auf SelectedDate setzen
            if (SelectedDate != null && DisplayDate.Date == DateTime.Today) DisplayDate = SelectedDate.Value;
        }

        public void SetToolTip(DateTime dateTime, object toolTip)
        {
            // Mögliche Uhrzeit aus Datum ignorieren
            var date = dateTime.Date;

            // ToolTip ins Dictionary schreiben
            ToolTips[date] = toolTip;

            var content = GetContentForDate(date);
            // Wenn der Button aktuell sichtbar ist, ToolTip setzen
            if (content != null) content.ToolTip = toolTip;
        }

        private void HeaderButton_Click(object sender, RoutedEventArgs e)
        {
            switch (DisplayMode)
            {
                case DisplayMode.MonthView:
                {
                    _suppressActiveViewsChanging = true;
                    DisplayMode = DisplayMode.YearView;
                    _suppressActiveViewsChanging = false;
                    ChangeActiveViews(new DateTime(DisplayDate.Year, DisplayDate.Month, 1));
                    break;
                }
                case DisplayMode.YearView:
                {
                    _suppressActiveViewsChanging = true;
                    DisplayMode = DisplayMode.DecadeView;
                    _suppressActiveViewsChanging = false;

                    var yearDifference = DisplayDate.Year % 10;
                    var firstYear = DisplayDate.Year - yearDifference;

                    ChangeActiveViews(new DateTime(firstYear, 1, 1));
                    break;
                }
                case DisplayMode.DecadeView:
                {
                    _suppressActiveViewsChanging = true;
                    DisplayMode = DisplayMode.CenturyView;
                    _suppressActiveViewsChanging = false;

                    var yearDifference = DisplayDate.Year % 100;
                    var firstYear = DisplayDate.Year - yearDifference;

                    ChangeActiveViews(new DateTime(firstYear, 1, 1));
                    break;
                }
                case DisplayMode.CenturyView: break;
            }
        }

        private void MoveLeftButton_Click(object sender, RoutedEventArgs e)
        {
            MoveLeft();
        }

        private void MoveRightButton_Click(object sender, RoutedEventArgs e)
        {
            MoveRight();
        }

        private void MoveLeft()
        {
            var referenceDate = DisplayDate;

            switch (DisplayMode)
            {
                case DisplayMode.MonthView: referenceDate = referenceDate.AddMonths(-1); break;
                case DisplayMode.YearView: referenceDate = referenceDate.AddYears(-1); break;
                case DisplayMode.DecadeView: referenceDate = referenceDate.AddYears(-10); break;
                case DisplayMode.CenturyView: referenceDate = referenceDate.AddYears(-100); break;
            }

            DisplayDate = new DateTime(referenceDate.Year, referenceDate.Month, 1);
        }

        private void MoveRight()
        {
            var referenceDate = DisplayDate;

            switch (DisplayMode)
            {
                case DisplayMode.MonthView: referenceDate = referenceDate.AddMonths(1); break;
                case DisplayMode.YearView: referenceDate = referenceDate.AddYears(1); break;
                case DisplayMode.DecadeView: referenceDate = referenceDate.AddYears(10); break;
                case DisplayMode.CenturyView: referenceDate = referenceDate.AddYears(100); break;
            }

            DisplayDate = new DateTime(referenceDate.Year, referenceDate.Month, 1);
        }

        private void PopulateActiveViews()
        {
            switch (DisplayMode)
            {
                case DisplayMode.MonthView:
                {
                    if (MonthViewsPanel != null)
                    {
                        PopulateViews(MonthViewsPanel);
                    }
                    break;
                }
                case DisplayMode.YearView:
                {
                    if (YearViewsPanel != null)
                    {
                        PopulateViews(YearViewsPanel);
                    }
                    break;
                }
                case DisplayMode.DecadeView:
                {
                    if (DecadeViewsPanel != null)
                    {
                        PopulateViews(DecadeViewsPanel);
                    }
                    break;
                }
                case DisplayMode.CenturyView:
                {
                    if (CenturyViewsPanel != null)
                    {
                        PopulateViews(CenturyViewsPanel);
                    }
                    break;
                }
            }
        }

        private void PopulateViews(Panel panel)
        {
            var referenceDate = DisplayDate.Date;
            // Start und Ende aus dem Zwischenspeicher löschen
            _firstDisplayedDate = null;
            _lastDisplayedDate = null;

            for (int i = 0, count = panel.Children.Count; i < count; i++)
            {
                var child = panel.Children[i];

                if (child is CalendarView view)
                {
                    view.Header = referenceDate;

                    if (view.CalendarPanel == null)
                    {
                        view.CalendarPanel = view.ChildOfType<UniformGrid>();

                        if (view.CalendarPanel != null)
                        {
                            view.CalendarPanel.HideFirstColumn = view.HideFirstColumn;
                            view.CalendarPanel.HideFirstRow = view.HideFirstRow;
                        }
                    }

                    switch (DisplayMode)
                    {
                        case DisplayMode.MonthView:
                        {
                            PopulateMonthView(view, referenceDate, i, count);
                            referenceDate = referenceDate.AddMonths(1);
                            view.HeaderStringFormat = MonthViewHeaderFormat ?? "MMMM - yyyy";
                            break;
                        }
                        case DisplayMode.YearView:
                        {
                            PopulateYearView(view, referenceDate, i, count);
                            referenceDate = referenceDate.AddYears(1);
                            view.HeaderStringFormat = YearViewHeaderFormat ?? "yyyy";
                            break;
                        }
                        case DisplayMode.DecadeView:
                        {
                            PopulateDecadeView(view, referenceDate, i, count);
                            referenceDate = referenceDate.AddYears(10);
                            view.HeaderStringFormat = DecadeViewHeaderFormat ?? "yyyy";
                            break;
                        }
                        case DisplayMode.CenturyView:
                        {
                            PopulateCenturyView(view, referenceDate, i, count);
                            referenceDate = referenceDate.AddYears(100);
                            view.HeaderStringFormat = CenturyViewHeaderFormat ?? "yyyy";
                            break;
                        }
                    }
                }
            }
        }

        private void PopulateMonthView(CalendarView view, DateTime referenceDate, int index, int count)
        {
            var firstDayOfWeek = FirstDayOfWeek ?? Culture?.DateTimeFormat.FirstDayOfWeek ?? CultureInfo.CurrentCulture.DateTimeFormat.FirstDayOfWeek;

            var firstDayOfMonth = new DateTime(referenceDate.Year, referenceDate.Month, 1);
            var lastDayOfMonth = firstDayOfMonth.AddMonths(1).AddDays(-1);

            var firstDisplayedDate = firstDayOfMonth;

            while (firstDisplayedDate.DayOfWeek != firstDayOfWeek)
            {
                firstDisplayedDate = firstDisplayedDate.AddDays(-1);
            }

            // Wenn wir im ersten angezeigten Monat sind, das erste Datum merken
            if (index == 0) _firstDisplayedDate = firstDisplayedDate;

            var itemsSource = (ObservableCollection<CalendarButtonContent>)view.ItemsSource;

            itemsSource.Clear();

            // Wochentage-Header hinzufügen
            for (int i = 0; i < 7; i++)
            {
                var content = new CalendarButtonContent(this)
                {
                    Date = firstDisplayedDate.AddDays(i),
                    DisplayMode = CalendarButtonType.DayOfWeek,
                    IsFromCurrentView = true
                };

                itemsSource.Add(content);
            }

            var hidePrevious = index != 0;
            var hideFollowing = index < count - 1;

            var displayDateStart = DisplayDateStart.GetValueOrDefault(DateTime.MinValue);
            var displayDateEnd = DisplayDateEnd.GetValueOrDefault(DateTime.MaxValue);

            // Um keine 42 zugriffe auf GetValue auszulösen wird die Property einmal vorher ausgelesen
            var isTodayHighlighted = IsTodayHighlighted;

            // Kalendarwochen und Wochentage hinzufügen
            for (int i = 0; i < 42; i++)
            {
                var currentDate = firstDisplayedDate.AddDays(i);

                var isPreviousMonth = currentDate < firstDayOfMonth;
                var isFollowingMonth = currentDate > lastDayOfMonth;

                var hide = isPreviousMonth && hidePrevious || isFollowingMonth && hideFollowing;

                if (displayDateStart > currentDate) hide = true;
                if (displayDateEnd < currentDate) hide = true;

                if (currentDate.DayOfWeek == firstDayOfWeek)
                {
                    var weekNumber = new CalendarButtonContent(this)
                    {
                        Date = currentDate,
                        DisplayMode = CalendarButtonType.WeekNumber,
                        IsFromCurrentView = true
                    };

                    itemsSource.Add(weekNumber);
                }

                var isEnabled = true;

                if (BlackoutDates != null && BlackoutDates.Contains(currentDate)) isEnabled = false;
                if (SelectableDateStart != null && SelectableDateStart > currentDate) isEnabled = false;
                if (SelectableDateEnd != null && SelectableDateEnd < currentDate) isEnabled = false;

                if (index == count - 1 && i == 41) _lastDisplayedDate = currentDate;

                var content = new CalendarButtonContent(this)
                {
                    Date = currentDate,
                    DisplayMode = currentDate == DateTime.Today && isTodayHighlighted ? CalendarButtonType.Today : CalendarButtonType.Day,
                    IsFromCurrentView = isPreviousMonth || isFollowingMonth ? false : true,
                    Hide = hide,
                    IsEnabled = isEnabled,
                    IsSelected = DateSelectionMode == DateSelectionMode.Day && SelectedDates.Contains(currentDate),
                    ContentTemplate = SpecialDates.FirstOrDefault(x => x.Date == currentDate)?.Template
                };

                if (ToolTips.TryGetValue(currentDate, out var toolTip))
                {
                    content.ToolTip = toolTip;
                }

                itemsSource.Add(content);
            }
        }

        private void PopulateYearView(CalendarView view, DateTime referenceDate, int index, int count)
        {
            var itemsSource = (ObservableCollection<CalendarButtonContent>)view.ItemsSource;

            itemsSource.Clear();

            var displayDateStart = DisplayDateStart.GetValueOrDefault(DateTime.MinValue);
            var displayDateEnd = DisplayDateEnd.GetValueOrDefault(DateTime.MaxValue);

            for (int i = 1; i <= 12; i++)
            {
                var currentMonth = new DateTime(referenceDate.Year, i, 1);

                if (index == 0 && i == 1) _firstDisplayedDate = currentMonth;

                var hide = false;
                var isEnabled = true;

                if (displayDateStart.Month > currentMonth.Month) hide = true;
                if (displayDateEnd.Month < currentMonth.Month) hide = true;

                if (SelectableDateStart != null && SelectableDateStart.Value.Month > currentMonth.Month) isEnabled = false;
                if (SelectableDateEnd != null && SelectableDateEnd.Value.Month < currentMonth.Month) isEnabled = false;

                if (index == count - 1 && i == 12) _lastDisplayedDate = currentMonth;

                var content = new CalendarButtonContent(this)
                {
                    Date = currentMonth,
                    DisplayMode = CalendarButtonType.Month,
                    IsFromCurrentView = true,
                    Hide = hide,
                    IsEnabled = isEnabled,
                    IsSelected = DateSelectionMode == DateSelectionMode.Month && SelectedDates.Contains(currentMonth)
                };

                if (ToolTips.TryGetValue(currentMonth, out var toolTip))
                {
                    content.ToolTip = toolTip;
                }

                itemsSource.Add(content);
            }
        }

        private void PopulateDecadeView(CalendarView view, DateTime referenceDate, int index, int count)
        {
            var itemsSource = (ObservableCollection<CalendarButtonContent>)view.ItemsSource;

            itemsSource.Clear();

            var displayDateStart = DisplayDateStart.GetValueOrDefault(DateTime.MinValue);
            var displayDateEnd = DisplayDateEnd.GetValueOrDefault(DateTime.MaxValue);

            var yearDifference = referenceDate.Year % 10;

            var firstYear = referenceDate.Year - yearDifference - 1;

            if (firstYear <= 0) firstYear = 1;

            var firstDate = new DateTime(firstYear, 1, 1);

            if (index == 0) _firstDisplayedDate = firstDate;

            var hidePrevious = index != 0;
            var hideFollowing = index < count - 1;

            for (int i = 0; i < 12; i++)
            {
                var currentYear = firstDate.AddYears(i);

                var isPreviousDecade = i == 0;
                var isFollowingDecade = i == 11;

                var hide = isPreviousDecade && hidePrevious || isFollowingDecade && hideFollowing;
                var isEnabled = true;

                if (displayDateStart.Year > currentYear.Year) hide = true;
                if (displayDateEnd.Year < currentYear.Year) hide = true;

                if (SelectableDateStart != null && SelectableDateStart.Value.Year > currentYear.Year) isEnabled = false;
                if (SelectableDateEnd != null && SelectableDateEnd.Value.Year < currentYear.Year) isEnabled = false;

                if (index == count - 1 && i == 11) _lastDisplayedDate = currentYear;

                var content = new CalendarButtonContent(this)
                {
                    Date = currentYear,
                    DisplayMode = CalendarButtonType.Year,
                    IsFromCurrentView = !(isPreviousDecade || isFollowingDecade),
                    Hide = hide,
                    IsEnabled = isEnabled,
                    IsSelected = DateSelectionMode == DateSelectionMode.Year && SelectedDates.Contains(currentYear)
                };

                if (ToolTips.TryGetValue(currentYear, out var toolTip))
                {
                    content.ToolTip = toolTip;
                }

                itemsSource.Add(content);
            }
        }

        private void PopulateCenturyView(CalendarView view, DateTime referenceDate, int index, int count)
        {
            var itemsSource = (ObservableCollection<CalendarButtonContent>)view.ItemsSource;

            itemsSource.Clear();

            var displayDateStart = DisplayDateStart.GetValueOrDefault(DateTime.MinValue);
            var displayDateEnd = DisplayDateEnd.GetValueOrDefault(DateTime.MaxValue);

            var yearDifference = referenceDate.Year % 100;

            var firstYear = referenceDate.Year - yearDifference - 10;

            if (firstYear <= 0) firstYear = 1;

            var firstDate = new DateTime(firstYear, 1, 1);

            if (index == 0) _firstDisplayedDate = firstDate;

            var hidePrevious = index != 0;
            var hideFollowing = index < count - 1;

            for (int i = 0; i < 12; i++)
            {
                var currentYear = firstDate.AddYears(i * 10);

                var isPreviousCentury = i == 0;
                var isFollowingCentury = i == 11;

                var hide = isPreviousCentury && hidePrevious || isFollowingCentury && hideFollowing;

                var isEnabled = true;

                if (displayDateStart.Year > currentYear.Year) hide = true;
                if (displayDateEnd.Year < currentYear.Year) hide = true;

                if (SelectableDateStart != null && SelectableDateStart.Value.Year > currentYear.Year) isEnabled = false;
                if (SelectableDateEnd != null && SelectableDateEnd.Value.Year < currentYear.Year) isEnabled = false;

                if (index == count - 1 && i == 11) _lastDisplayedDate = currentYear;

                var content = new CalendarButtonContent(this)
                {
                    Date = currentYear,
                    DisplayMode = CalendarButtonType.Decade,
                    IsFromCurrentView = !(isPreviousCentury || isFollowingCentury),
                    Hide = hide,
                    IsEnabled = isEnabled
                };

                if (ToolTips.TryGetValue(currentYear, out var toolTip))
                {
                    content.ToolTip = toolTip;
                }

                itemsSource.Add(content);
            }
        }

        private void UpdateColumnsAndRows()
        {
            var neededViewsCount = Columns * Rows;

            if (MonthViewsPanel != null)
            {
                CreateOrRemoveViews(DisplayMode.MonthView, MonthViewsPanel, neededViewsCount);
            }

            if (YearViewsPanel != null)
            {
                CreateOrRemoveViews(DisplayMode.YearView, YearViewsPanel, neededViewsCount);
            }

            if (DecadeViewsPanel != null)
            {
                CreateOrRemoveViews(DisplayMode.DecadeView, DecadeViewsPanel, neededViewsCount);
            }

            if (CenturyViewsPanel != null)
            {
                CreateOrRemoveViews(DisplayMode.CenturyView, CenturyViewsPanel, neededViewsCount);
            }
        }

        private void CreateOrRemoveViews(DisplayMode displayMode, Panel panel, int neededViewsCount)
        {
            var childrenCount = panel.Children.Count;

            if (childrenCount > neededViewsCount)
            {
                var viewsCountToRemove = childrenCount - neededViewsCount;

                for (int i = 0; i < viewsCountToRemove; i++)
                {
                    var view = panel.Children[neededViewsCount + i];

                    BindingOperations.ClearAllBindings(view);
                }

                panel.Children.RemoveRange(neededViewsCount, viewsCountToRemove);
            }
            else if (childrenCount < neededViewsCount)
            {
                var viewsCountToCreate = neededViewsCount - childrenCount;

                for (int i = 0; i < viewsCountToCreate; i++)
                {
                    var view = new CalendarView(this);

                    switch (displayMode)
                    {
                        case DisplayMode.MonthView:
                        {
                            view.Style = MonthViewStyle;
                            view.ItemsPanel = MonthViewPanel;
                            view.HeaderStringFormat = MonthViewHeaderFormat ?? "MMMM - yyyy";
                            view.SetBinding(CalendarView.HideFirstColumnProperty, new Binding("AreWeekNumbersVisible") { Source = this, Converter = new Converter.InvertedBooleanConverter() });
                            view.SetBinding(CalendarView.HideFirstRowProperty, new Binding("AreWeekNamesVisible") { Source = this, Converter = new Converter.InvertedBooleanConverter() });
                            break;
                        }
                        case DisplayMode.YearView:
                        {
                            view.Style = YearViewStyle;
                            view.ItemsPanel = YearViewPanel;
                            view.HeaderStringFormat = YearViewHeaderFormat ?? "yyyy";
                            break;
                        }
                        case DisplayMode.DecadeView:
                        {
                            view.Style = DecadeViewStyle;
                            view.ItemsPanel = DecadeViewPanel;
                            view.HeaderStringFormat = DecadeViewHeaderFormat ?? "yyyy";
                            break;
                        }
                        case DisplayMode.CenturyView:
                        {
                            view.Style = CenturyViewStyle;
                            view.ItemsPanel = CenturyViewPanel;
                            view.HeaderStringFormat = CenturyViewHeaderFormat ?? "yyyy";
                            break;
                        }
                    }

                    view.SetBinding(CalendarView.HeaderVisibilityProperty, new Binding("ViewsHeaderVisibility") { Source = this });
                    view.SetBinding(BackgroundProperty, new Binding("ViewsHeaderBackground") { Source = this });

                    view.ItemsSource = new ObservableCollection<CalendarButtonContent>();

                    panel.Children.Add(view);
                }

                PopulateViews(panel);
            }
        }

        protected virtual void OnDisplayModeChanged(Specialized.Calendar.CalendarModeChangedEventArgs e)
        {
            RaiseEvent(e);

            if (_suppressActiveViewsChanging) return;

            ChangeActiveViews();
        }

        private void ChangeActiveViews(DateTime? displayDate = null)
        {
            switch (DisplayMode)
            {
                case DisplayMode.MonthView:
                {
                    if (HeaderButton != null)
                    {
                        // Header-Button wieder klickbar machen
                        HeaderButton.IsEnabled = true;
                        HeaderButton.Content = string.Format("{0:" + (MonthViewHeaderFormat ?? "MMMM - yyyy") + "}", DisplayDate);
                    }

                    if (MonthViewsPanel != null) MonthViewsPanel.Visibility = Visibility.Visible;
                    if (YearViewsPanel != null) YearViewsPanel.Visibility = Visibility.Collapsed;
                    if (DecadeViewsPanel != null) DecadeViewsPanel.Visibility = Visibility.Collapsed;
                    if (CenturyViewsPanel != null) CenturyViewsPanel.Visibility = Visibility.Collapsed;

                    break;
                }
                case DisplayMode.YearView:
                {
                    if (HeaderButton != null)
                    {
                        // Header-Button wieder klickbar machen
                        HeaderButton.IsEnabled = true;
                        HeaderButton.Content = string.Format("{0:" + (YearViewHeaderFormat ?? "yyyy") + "}", DisplayDate);
                    }

                    if (MonthViewsPanel != null) MonthViewsPanel.Visibility = Visibility.Collapsed;
                    if (YearViewsPanel != null) YearViewsPanel.Visibility = Visibility.Visible;
                    if (DecadeViewsPanel != null) DecadeViewsPanel.Visibility = Visibility.Collapsed;
                    if (CenturyViewsPanel != null) CenturyViewsPanel.Visibility = Visibility.Collapsed;

                    break;
                }
                case DisplayMode.DecadeView:
                {
                    if (HeaderButton != null)
                    {
                        // Header-Button wieder klickbar machen
                        HeaderButton.IsEnabled = true;
                        HeaderButton.Content = string.Format("{0:" + (DecadeViewHeaderFormat ?? "yyyy") + "}", DisplayDate);
                    }

                    if (MonthViewsPanel != null) MonthViewsPanel.Visibility = Visibility.Collapsed;
                    if (YearViewsPanel != null) YearViewsPanel.Visibility = Visibility.Collapsed;
                    if (DecadeViewsPanel != null) DecadeViewsPanel.Visibility = Visibility.Visible;
                    if (CenturyViewsPanel != null) CenturyViewsPanel.Visibility = Visibility.Collapsed;

                    break;
                }
                case DisplayMode.CenturyView:
                {
                    if (HeaderButton != null)
                    {
                        // Um Verwirrung zu verhindern wird der Header-Button deaktiviert
                        HeaderButton.IsEnabled = false;
                        HeaderButton.Content = string.Format("{0:" + (CenturyViewHeaderFormat ?? "yyyy") + "}", DisplayDate);
                    }

                    if (MonthViewsPanel != null) MonthViewsPanel.Visibility = Visibility.Collapsed;
                    if (YearViewsPanel != null) YearViewsPanel.Visibility = Visibility.Collapsed;
                    if (DecadeViewsPanel != null) DecadeViewsPanel.Visibility = Visibility.Collapsed;
                    if (CenturyViewsPanel != null) CenturyViewsPanel.Visibility = Visibility.Visible;

                    break;
                }
            }

            if (displayDate != null && displayDate != DisplayDate) DisplayDate = displayDate.Value;
            else PopulateActiveViews();
        }

        internal void ChangeViewAndDate(DisplayMode displayMode, DateTime displayDate)
        {
            _suppressActiveViewsChanging = true;
            DisplayMode = displayMode;
            _suppressActiveViewsChanging = false;

            ChangeActiveViews(displayDate);
        }

        private CalendarButtonContent GetContentForDate(DateTime dateTime)
        {
            // Falls doch eine Zeit mit enthalten ist, diese nicht beachten
            var date = dateTime.Date;

            Panel panel;
            CalendarButtonType buttonType;

            switch (DisplayMode)
            {
                case DisplayMode.MonthView:
                {
                    panel = MonthViewsPanel;
                    buttonType = CalendarButtonType.Day;
                    break;
                }
                case DisplayMode.YearView:
                {
                    panel = YearViewsPanel;
                    buttonType = CalendarButtonType.Month;
                    break;
                }
                case DisplayMode.DecadeView:
                {
                    panel = DecadeViewsPanel;
                    buttonType = CalendarButtonType.Year;
                    break;
                }
                case DisplayMode.CenturyView:
                {
                    panel = CenturyViewsPanel;
                    buttonType = CalendarButtonType.Decade;
                    break;
                }
                default: panel = null; buttonType = CalendarButtonType.Day; break;
            }

            // Diese Funktion ist nur dafür gedacht, wenn wir eine Ansicht haben
            if (panel == null || _firstDisplayedDate == null || _lastDisplayedDate == null) return null;

            // Wenn das Datum vor unserem kleinsten oder nach unserem letzten Datum liegt, abbrechen
            if (date < _firstDisplayedDate || date > _lastDisplayedDate) return null;

            var views = panel.Children.OfType<CalendarView>().ToList();

            for (int i = 0; i < views.Count; i++)
            {
                var view = views[i];

                var firstDate = view.Items.OfType<CalendarButtonContent>().FirstOrDefault(x => !x.Hide && (x.DisplayMode == buttonType || x.DisplayMode == CalendarButtonType.Today));
                var lastDate = view.Items.OfType<CalendarButtonContent>().LastOrDefault(x => !x.Hide && (x.DisplayMode == buttonType || x.DisplayMode == CalendarButtonType.Today));

                // Nur zur sicherheit eingebaut. Sollte eigentlich nicht passieren können
                if (firstDate == null || lastDate == null) continue;

                if (firstDate.Date > date || lastDate.Date < date) continue;

                return view.Items.OfType<CalendarButtonContent>().FirstOrDefault(x => !x.Hide && (x.DisplayMode == buttonType || x.DisplayMode == CalendarButtonType.Today) && x.Date == date);
            }

            // Es sollte ein Ergebnis in der Schleife gefunden werden
            return null;
        }

        protected virtual void OnDisplayDateChanged(Specialized.Calendar.CalendarDateChangedEventArgs e)
        {
            RaiseEvent(e);

            switch (DisplayMode)
            {
                case DisplayMode.MonthView:
                {
                    if (HeaderButton != null) HeaderButton.Content = string.Format("{0:" + (MonthViewHeaderFormat ?? "MMMM - yyyy") + "}", DisplayDate);

                    break;
                }
                case DisplayMode.YearView:
                {
                    if (HeaderButton != null) HeaderButton.Content = string.Format("{0:" + (YearViewHeaderFormat ?? "yyyy") + "}", DisplayDate);

                    break;
                }
                case DisplayMode.DecadeView:
                {
                    if (HeaderButton != null) HeaderButton.Content = string.Format("{0:" + (DecadeViewHeaderFormat ?? "yyyy") + "}", DisplayDate);

                    break;
                }
                case DisplayMode.CenturyView:
                {
                    if (HeaderButton != null) HeaderButton.Content = string.Format("{0:" + (CenturyViewHeaderFormat ?? "yyyy") + "}", DisplayDate);

                    break;
                }
            }

            PopulateActiveViews();
        }

        protected virtual void OnSelectedDateChanged(Specialized.Calendar.CalendarDateChangedEventArgs e)
        {
            RaiseEvent(e);

            if (e.AddedDate != null)
            {
                if (SelectionMode == SelectionMode.Single) UnSelectAllExcept(e.AddedDate.Value);
                else if (!SelectedDates.Contains(e.AddedDate.Value)) SelectedDates.Add(e.AddedDate.Value);
            }
        }

        private void OnSelectedDatesCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                {
                    for (int i = 0; i < e.NewItems.Count; i++)
                    {
                        var item = (DateTime)e.NewItems[i];

                        if (SelectableDateStart != null && SelectableDateStart > item) continue;
                        if (SelectableDateEnd != null && SelectableDateEnd < item) continue;

                        InternalSelectedDates.Add(item);

                        var content = GetContentForDate(item);

                        if (content != null && content.IsEnabled) content.IsSelected = true;

                        if (SelectedDate == null) SelectedDate = item;
                    }
                    break;
                }
                case NotifyCollectionChangedAction.Remove:
                {
                    var changeSelectedDate = false;

                    for (int i = 0; i < e.OldItems.Count; i++)
                    {
                        var item = (DateTime)e.OldItems[i];

                        InternalSelectedDates.Remove(item);

                        var content = GetContentForDate(item);

                        if (content != null) content.IsSelected = false;
                    }

                    if (changeSelectedDate) SelectedDate = SelectedDates.Count > 0 ? SelectedDates.Last() : (DateTime?)null;
                    break;
                }
                case NotifyCollectionChangedAction.Reset:
                {
                    for (int i = 0; i < InternalSelectedDates.Count; i++)
                    {
                        var item = InternalSelectedDates[i];

                        var content = GetContentForDate(item);

                        if (content != null) content.IsSelected = false;
                    }
                    InternalSelectedDates.Clear();
                    break;
                }
            }
        }

        private void UnSelectAllExcept(DateTime date)
        {
            SelectedDates.Clear();
            SelectedDate = date;
            if (!SelectedDates.Contains(date)) SelectedDates.Add(date);
            _lastArrowKeyDate = null;
        }

        private void UnSelectAllExcept(DateTime date, bool changeDisplayDate)
        {
            UnSelectAllExcept(date);
            if (changeDisplayDate) DisplayDate = date;
        }

        internal void StartSelection()
        {
            _selectionInProgress = true;
        }

        internal bool IsSelectionInProgress()
        {
            return _selectionInProgress;
        }

        internal void SelectDate(DateTime date, bool changeDisplayDate)
        {
            SelectedDate = date;

            if (changeDisplayDate) DisplayDate = date;
        }

        internal void SelectDateRange(DateTime referenceDate, DateTime selectionDate, bool unselectOthers)
        {
            var firstDate = referenceDate < selectionDate ? referenceDate : selectionDate;
            var lastDate = referenceDate < selectionDate ? selectionDate : referenceDate;

            if (unselectOthers) SelectedDates.Clear();

            var daysToSelect = new List<DateTime>();

            switch (DateSelectionMode)
            {
                case DateSelectionMode.Day:
                {
                    var days = (int)(lastDate - firstDate).TotalDays;

                    for (int i = 0; i <= days; i++)
                    {
                        var day = firstDate.AddDays(i);

                        if (BlackoutDates != null && BlackoutDates.Contains(day)) continue;
                        if (SelectedDates.Contains(day)) continue;

                        daysToSelect.Add(day);
                    }
                    break;
                }
                case DateSelectionMode.Month:
                {
                    var difference = ((lastDate.Year - firstDate.Year) * 12) + lastDate.Month - firstDate.Month;

                    var startDate = new DateTime(firstDate.Year, firstDate.Month, 1);

                    for (int i = 0; i <= difference; i++)
                    {
                        var day = startDate.AddMonths(i);

                        if (BlackoutDates != null && BlackoutDates.Contains(day)) continue;
                        if (SelectedDates.Contains(day)) continue;

                        daysToSelect.Add(day);
                    }
                    break;
                }
                case DateSelectionMode.Year:
                {
                    var years = lastDate.Year - firstDate.Year;

                    var startDate = new DateTime(firstDate.Year, 1, 1);

                    for (int i = 0; i <= years; i++)
                    {
                        var day = startDate.AddYears(i);

                        if (BlackoutDates != null && BlackoutDates.Contains(day)) continue;
                        if (SelectedDates.Contains(day)) continue;

                        daysToSelect.Add(day);
                    }
                    break;
                }
            }

            SelectedDates.AddRange(daysToSelect);
        }

        internal void UnSelectDate(DateTime date, bool changeDisplayDate)
        {
            SelectedDates.Remove(date);

            SelectedDate = SelectedDates.Count > 0 ? (DateTime?)SelectedDates.First() : null;

            if (changeDisplayDate) DisplayDate = date;
        }

        internal void FinishSelection()
        {
            _selectionInProgress = false;
            _lastArrowKeyDate = null;
        }

        protected override void OnMouseLeave(MouseEventArgs e)
        {
            base.OnMouseLeave(e);

            _selectionInProgress = false;
        }

        private void HookupBlackoutDatesCollection(INotifyCollectionChanged collection)
        {
            collection.CollectionChanged += OnBlackoutDatesCollectionChanged;
        }

        private void UnHookBlackoutDatesCollection(INotifyCollectionChanged collection)
        {
            collection.CollectionChanged -= OnBlackoutDatesCollectionChanged;
        }

        private void OnBlackoutDatesCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            // Da es nur darum geht einen einzelnen Tag auszugrauen müssen wir auch nur was machen, wenn wir gerade in der Monatsanzeige sind und auch ein Panel dafür haben
            if (DisplayMode != DisplayMode.MonthView || MonthViewsPanel == null) return;

            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                {
                    for (int i = 0; i < e.NewItems.Count; i++)
                    {
                        var item = (DateTime)e.NewItems[i];

                        var content = GetContentForDate(item);

                        if (content != null) content.IsEnabled = false;
                    }
                    break;
                }
                case NotifyCollectionChangedAction.Remove:
                {
                    for (int i = 0; i < e.OldItems.Count; i++)
                    {
                        var item = (DateTime)e.OldItems[i];

                        if (SelectableDateStart != null && SelectableDateStart > item) continue;
                        if (SelectableDateEnd != null && SelectableDateEnd < item) continue;

                        var content = GetContentForDate(item);

                        if (content != null) content.IsEnabled = true;
                    }
                    break;
                }
                case NotifyCollectionChangedAction.Reset:
                {
                    PopulateActiveViews();
                    break;
                }
            }
        }

        private void HookupSpecialDatesCollection(INotifyCollectionChanged collection)
        {
            collection.CollectionChanged += OnSpecialDatesCollectionChanged;
        }

        private void UnHookSpecialDatesCollection(INotifyCollectionChanged collection)
        {
            collection.CollectionChanged -= OnSpecialDatesCollectionChanged;
        }

        private void OnSpecialDatesCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            // Da es nur darum geht, das Template von einen einzelnen Tag zu ändern, müssen wir auch nur was machen, wenn wir gerade in der Monatsanzeige sind und auch ein Panel dafür haben
            if (DisplayMode != DisplayMode.MonthView || MonthViewsPanel == null) return;

            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                {
                    for (int i = 0; i < e.NewItems.Count; i++)
                    {
                        var item = (SpecialDate)e.NewItems[i];

                        var content = GetContentForDate(item.Date);

                        if (content != null) content.ContentTemplate = item.Template;
                    }
                    break;
                }
                case NotifyCollectionChangedAction.Remove:
                {
                    for (int i = 0; i < e.OldItems.Count; i++)
                    {
                        var item = (SpecialDate)e.OldItems[i];

                        var content = GetContentForDate(item.Date);

                        if (content != null) content.ContentTemplate = null;
                    }
                    break;
                }
                case NotifyCollectionChangedAction.Reset:
                {
                    PopulateActiveViews();
                    break;
                }
            }
        }

        protected virtual void OnSelectionModeChanged()
        {
            // Nur wenn sich SelectionMode auf Single geändert hat, kann es auch wirklich einen unterschied machen
            if (SelectionMode != SelectionMode.Single) return;

            if (SelectedDate != null) UnSelectAllExcept(SelectedDate.Value);
            else SelectedDates.Clear();
        }

        private void OnIsTodayHighlightedChanged()
        {
            if (DisplayMode != DisplayMode.MonthView) return;

            var content = GetContentForDate(DateTime.Today);

            if (content != null) content.DisplayMode = IsTodayHighlighted ? CalendarButtonType.Today : CalendarButtonType.Day;
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.PageUp:
                {
                    MoveLeft();
                    e.Handled = true;
                    break;
                }
                case Key.PageDown:
                {
                    MoveRight();
                    e.Handled = true;
                    break;
                }
                case Key.Home:
                {
                    HandleHomeKey();
                    e.Handled = true;
                    break;
                }
                case Key.End:
                {
                    HandleEndKey();
                    e.Handled = true;
                    break;
                }
                case Key.Left:
                {
                    HandleArrowLeftKey();
                    e.Handled = true;
                    break;
                }
                case Key.Up:
                {
                    HandleArrowUpKey();
                    e.Handled = true;
                    break;
                }
                case Key.Right:
                {
                    HandleArrowRightKey();
                    e.Handled = true;
                    break;
                }
                case Key.Down:
                {
                    HandleArrowDownKey();
                    e.Handled = true;
                    break;
                }
            }

            base.OnKeyDown(e);
        }

        private void HandleHomeKey()
        {
            if (IsReadOnly) return;

            // Das Datum bestimmen, was als referenz genutzt werden soll
            var referenceDate = SelectedDate != null && SelectedDate >= _firstDisplayedDate && SelectedDate <= _lastDisplayedDate ? SelectedDate.Value : DisplayDate;

            DateTime? targetDate = null;

            if (DisplayMode == DisplayMode.MonthView && DateSelectionMode == DateSelectionMode.Day)
            {
                // Letztes Datum im aktuellen Monat ermitteln
                targetDate = new DateTime(referenceDate.Year, referenceDate.Month, 1);
                // Prüfen ob dieses Datum selected werden kann
                if (SelectableDateStart != null && SelectableDateStart > targetDate)
                {
                    if (SelectableDateStart < _firstDisplayedDate || SelectableDateStart > _lastDisplayedDate) targetDate = null;
                    else targetDate = SelectableDateStart;
                }
                else if (SelectableDateEnd != null && SelectableDateEnd < targetDate)
                {
                    if (SelectableDateEnd < _firstDisplayedDate || SelectableDateEnd > _lastDisplayedDate) targetDate = null;
                    else targetDate = SelectableDateEnd;
                }
            }
            else if (DisplayMode == DisplayMode.YearView && DateSelectionMode == DateSelectionMode.Month)
            {
                // Letzten Monat im aktuellen Jahr anvisieren
                targetDate = new DateTime(referenceDate.Year, 1, 1);
                // Prüfen ob dieses Datum selected werden kann
                if (SelectableDateStart != null && SelectableDateStart > targetDate)
                {
                    if (SelectableDateStart < _firstDisplayedDate || SelectableDateStart > _lastDisplayedDate) targetDate = null;
                    else targetDate = new DateTime(SelectableDateStart.Value.Year, SelectableDateStart.Value.Month, 1);
                }
                else if (SelectableDateEnd != null && SelectableDateEnd < targetDate)
                {
                    if (SelectableDateEnd < _firstDisplayedDate || SelectableDateEnd > _lastDisplayedDate) targetDate = null;
                    else targetDate = new DateTime(SelectableDateEnd.Value.Year, SelectableDateEnd.Value.Month, 1); ;
                }
            }
            else if (DisplayMode == DisplayMode.DecadeView && DateSelectionMode == DateSelectionMode.Year)
            {
                // Das letzte Jahr des aktuellen Jahrzehnts ermitteln
                var yearDifference = referenceDate.Year % 10;

                var firstYear = referenceDate.Year - yearDifference;

                targetDate = new DateTime(firstYear, 1, 1);
                // Prüfen ob dieses Datum selected werden kann
                if (SelectableDateStart != null && SelectableDateStart > targetDate)
                {
                    if (SelectableDateStart < _firstDisplayedDate || SelectableDateStart > _lastDisplayedDate) targetDate = null;
                    else targetDate = new DateTime(SelectableDateStart.Value.Year, 1, 1);
                }
                else if (SelectableDateEnd != null && SelectableDateEnd < targetDate)
                {
                    if (SelectableDateEnd < _firstDisplayedDate || SelectableDateEnd > _lastDisplayedDate) targetDate = null;
                    else targetDate = new DateTime(SelectableDateEnd.Value.Year, 1, 1); ;
                }
            }

            if (targetDate != null) UnSelectAllExcept(targetDate.Value);
        }

        private void HandleEndKey()
        {
            if (IsReadOnly) return;

            // Das Datum bestimmen, was als referenz genutzt werden soll
            var referenceDate = SelectedDate != null && SelectedDate >= _firstDisplayedDate && SelectedDate <= _lastDisplayedDate ? SelectedDate.Value : DisplayDate;

            DateTime? targetDate = null;

            if (DisplayMode == DisplayMode.MonthView && DateSelectionMode == DateSelectionMode.Day)
            {
                // Letztes Datum im aktuellen Monat ermitteln
                targetDate = new DateTime(referenceDate.Year, referenceDate.Month, DateTime.DaysInMonth(referenceDate.Year, referenceDate.Month));
                // Prüfen ob dieses Datum selected werden kann
                if (SelectableDateStart != null && SelectableDateStart > targetDate)
                {
                    if (SelectableDateStart < _firstDisplayedDate || SelectableDateStart > _lastDisplayedDate) targetDate = null;
                    else targetDate = SelectableDateStart;
                }
                else if (SelectableDateEnd != null && SelectableDateEnd < targetDate)
                {
                    if (SelectableDateEnd < _firstDisplayedDate || SelectableDateEnd > _lastDisplayedDate) targetDate = null;
                    else targetDate = SelectableDateEnd;
                }
            }
            else if (DisplayMode == DisplayMode.YearView && DateSelectionMode == DateSelectionMode.Month)
            {
                // Letzten Monat im aktuellen Jahr anvisieren
                targetDate = new DateTime(referenceDate.Year, 12, 1);
                // Prüfen ob dieses Datum selected werden kann
                if (SelectableDateStart != null && SelectableDateStart > targetDate)
                {
                    if (SelectableDateStart < _firstDisplayedDate || SelectableDateStart > _lastDisplayedDate) targetDate = null;
                    else targetDate = new DateTime(SelectableDateStart.Value.Year, SelectableDateStart.Value.Month, 1);
                }
                else if (SelectableDateEnd != null && SelectableDateEnd < targetDate)
                {
                    if (SelectableDateEnd < _firstDisplayedDate || SelectableDateEnd > _lastDisplayedDate) targetDate = null;
                    else targetDate = new DateTime(SelectableDateEnd.Value.Year, SelectableDateEnd.Value.Month, 1); ;
                }
            }
            else if (DisplayMode == DisplayMode.DecadeView && DateSelectionMode == DateSelectionMode.Year)
            {
                // Das letzte Jahr des aktuellen Jahrzehnts ermitteln
                var yearDifference = referenceDate.Year % 10;

                var lastYear = referenceDate.Year + 10 - yearDifference;

                targetDate = new DateTime(lastYear, 1, 1);
                // Prüfen ob dieses Datum selected werden kann
                if (SelectableDateStart != null && SelectableDateStart > targetDate)
                {
                    if (SelectableDateStart < _firstDisplayedDate || SelectableDateStart > _lastDisplayedDate) targetDate = null;
                    else targetDate = new DateTime(SelectableDateStart.Value.Year, 1, 1);
                }
                else if (SelectableDateEnd != null && SelectableDateEnd < targetDate)
                {
                    if (SelectableDateEnd < _firstDisplayedDate || SelectableDateEnd > _lastDisplayedDate) targetDate = null;
                    else targetDate = new DateTime(SelectableDateEnd.Value.Year, 1, 1); ;
                }
            }

            if (targetDate != null) UnSelectAllExcept(targetDate.Value);
        }

        private void HandleArrowLeftKey()
        {
            if (IsReadOnly) return;

            switch (SelectionMode)
            {
                case SelectionMode.Single:
                {
                    DateTime? targetDate = null;

                    switch (DateSelectionMode)
                    {
                        case DateSelectionMode.Day:
                        {
                            if (DisplayMode == DisplayMode.MonthView) targetDate = (SelectedDate ?? DisplayDate).AddDays(-1);

                            break;
                        }
                        case DateSelectionMode.Month:
                        {
                            if (DisplayMode == DisplayMode.YearView) targetDate = (SelectedDate ?? DisplayDate).AddMonths(-1);

                            break;
                        }
                        case DateSelectionMode.Year:
                        {
                            if (DisplayMode == DisplayMode.DecadeView) targetDate = (SelectedDate ?? DisplayDate).AddYears(-1);

                            break;
                        }
                    }

                    if (targetDate != null)
                    {
                        if (SelectableDateStart != null && SelectableDateStart > targetDate) targetDate = SelectableDateStart;
                        if (SelectableDateEnd != null && SelectableDateEnd < targetDate) targetDate = SelectableDateEnd;

                        SelectDate(targetDate.Value, targetDate < _firstDisplayedDate || targetDate > _lastDisplayedDate);
                        _lastArrowKeyDate = targetDate;
                    }

                    break;
                }
                // Wahrscheinlich sollten die beiden Modi leicht anders funktionieren aber für den Moment ist bei beiden die Logik gleich
                case SelectionMode.Multiple:
                case SelectionMode.Extended:
                {
                    DateTime? targetDate = null;

                    switch (DateSelectionMode)
                    {
                        case DateSelectionMode.Day:
                        {
                            if (DisplayMode == DisplayMode.MonthView) targetDate = (_lastArrowKeyDate ?? SelectedDate ?? DisplayDate).AddDays(-1);

                            break;
                        }
                        case DateSelectionMode.Month:
                        {
                            if (DisplayMode == DisplayMode.YearView) targetDate = (_lastArrowKeyDate ?? SelectedDate ?? DisplayDate).AddMonths(-1);

                            break;
                        }
                        case DateSelectionMode.Year:
                        {
                            if (DisplayMode == DisplayMode.DecadeView) targetDate = (_lastArrowKeyDate ?? SelectedDate ?? DisplayDate).AddYears(-1);

                            break;
                        }
                    }

                    if (targetDate != null)
                    {
                        if (SelectableDateStart != null && SelectableDateStart > targetDate) targetDate = SelectableDateStart;
                        if (SelectableDateEnd != null && SelectableDateEnd < targetDate) targetDate = SelectableDateEnd;

                        var isControlDown = Keyboard.IsKeyDown(Key.LeftCtrl) || Keyboard.IsKeyDown(Key.RightCtrl);
                        var isShiftDown = Keyboard.IsKeyDown(Key.LeftShift) || Keyboard.IsKeyDown(Key.RightShift);

                        if (isShiftDown)
                        {
                            SelectDateRange(SelectedDate ?? DisplayDate, targetDate.Value, !isControlDown);
                            if (isControlDown) SelectDate(targetDate.Value, targetDate < _firstDisplayedDate || targetDate > _lastDisplayedDate);
                        }
                        else
                        {
                            if (isControlDown) SelectDate(targetDate.Value, targetDate < _firstDisplayedDate || targetDate > _lastDisplayedDate);
                            else UnSelectAllExcept(targetDate.Value, targetDate < _firstDisplayedDate || targetDate > _lastDisplayedDate);
                        }
                        _lastArrowKeyDate = targetDate;
                    }

                    break;
                }
            }
        }

        private void HandleArrowUpKey()
        {
            if (IsReadOnly) return;

            switch (SelectionMode)
            {
                case SelectionMode.Single:
                {
                    DateTime? targetDate = null;

                    switch (DateSelectionMode)
                    {
                        case DateSelectionMode.Day:
                        {
                            if (DisplayMode == DisplayMode.MonthView) targetDate = (SelectedDate ?? DisplayDate).AddDays(-7);

                            break;
                        }
                        case DateSelectionMode.Month:
                        {
                            if (DisplayMode == DisplayMode.YearView) targetDate = (SelectedDate ?? DisplayDate).AddMonths(-4);

                            break;
                        }
                        case DateSelectionMode.Year:
                        {
                            if (DisplayMode == DisplayMode.DecadeView) targetDate = (SelectedDate ?? DisplayDate).AddYears(-4);

                            break;
                        }
                    }

                    if (targetDate != null)
                    {
                        if (SelectableDateStart != null && SelectableDateStart > targetDate) targetDate = SelectableDateStart;
                        if (SelectableDateEnd != null && SelectableDateEnd < targetDate) targetDate = SelectableDateEnd;

                        SelectDate(targetDate.Value, targetDate < _firstDisplayedDate || targetDate > _lastDisplayedDate);
                    }

                    break;
                }
                // Wahrscheinlich sollten die beiden Modi leicht anders funktionieren aber für den Moment ist bei beiden die Logik gleich
                case SelectionMode.Multiple:
                case SelectionMode.Extended:
                {
                    DateTime? targetDate = null;

                    switch (DateSelectionMode)
                    {
                        case DateSelectionMode.Day:
                        {
                            if (DisplayMode == DisplayMode.MonthView) targetDate = (_lastArrowKeyDate ?? SelectedDate ?? DisplayDate).AddDays(-7);

                            break;
                        }
                        case DateSelectionMode.Month:
                        {
                            if (DisplayMode == DisplayMode.YearView) targetDate = (_lastArrowKeyDate ?? SelectedDate ?? DisplayDate).AddMonths(-4);

                            break;
                        }
                        case DateSelectionMode.Year:
                        {
                            if (DisplayMode == DisplayMode.DecadeView) targetDate = (_lastArrowKeyDate ?? SelectedDate ?? DisplayDate).AddYears(-4);

                            break;
                        }
                    }

                    if (targetDate != null)
                    {
                        if (SelectableDateStart != null && SelectableDateStart > targetDate) targetDate = SelectableDateStart;
                        if (SelectableDateEnd != null && SelectableDateEnd < targetDate) targetDate = SelectableDateEnd;

                        var isControlDown = Keyboard.IsKeyDown(Key.LeftCtrl) || Keyboard.IsKeyDown(Key.RightCtrl);
                        var isShiftDown = Keyboard.IsKeyDown(Key.LeftShift) || Keyboard.IsKeyDown(Key.RightShift);

                        if (isShiftDown)
                        {
                            SelectDateRange(SelectedDate ?? DisplayDate, targetDate.Value, !isControlDown);
                            if (isControlDown) SelectDate(targetDate.Value, targetDate < _firstDisplayedDate || targetDate > _lastDisplayedDate);
                        }
                        else
                        {
                            if (isControlDown) SelectDate(targetDate.Value, targetDate < _firstDisplayedDate || targetDate > _lastDisplayedDate);
                            else UnSelectAllExcept(targetDate.Value, targetDate < _firstDisplayedDate || targetDate > _lastDisplayedDate);
                        }
                        _lastArrowKeyDate = targetDate;
                    }

                    break;
                }
            }
        }

        private void HandleArrowRightKey()
        {
            if (IsReadOnly) return;

            switch (SelectionMode)
            {
                case SelectionMode.Single:
                {
                    DateTime? targetDate = null;

                    switch (DateSelectionMode)
                    {
                        case DateSelectionMode.Day:
                        {
                            if (DisplayMode == DisplayMode.MonthView) targetDate = (SelectedDate ?? DisplayDate).AddDays(1);

                            break;
                        }
                        case DateSelectionMode.Month:
                        {
                            if (DisplayMode == DisplayMode.YearView) targetDate = (SelectedDate ?? DisplayDate).AddMonths(1);

                            break;
                        }
                        case DateSelectionMode.Year:
                        {
                            if (DisplayMode == DisplayMode.DecadeView) targetDate = (SelectedDate ?? DisplayDate).AddYears(1);

                            break;
                        }
                    }

                    if (targetDate != null)
                    {
                        if (SelectableDateStart != null && SelectableDateStart > targetDate) targetDate = SelectableDateStart;
                        if (SelectableDateEnd != null && SelectableDateEnd < targetDate) targetDate = SelectableDateEnd;

                        SelectDate(targetDate.Value, targetDate < _firstDisplayedDate || targetDate > _lastDisplayedDate);
                    }

                    break;
                }
                // Wahrscheinlich sollten die beiden Modi leicht anders funktionieren aber für den Moment ist bei beiden die Logik gleich
                case SelectionMode.Multiple:
                case SelectionMode.Extended:
                {
                    DateTime? targetDate = null;

                    switch (DateSelectionMode)
                    {
                        case DateSelectionMode.Day:
                        {
                            if (DisplayMode == DisplayMode.MonthView) targetDate = (_lastArrowKeyDate ?? SelectedDate ?? DisplayDate).AddDays(1);

                            break;
                        }
                        case DateSelectionMode.Month:
                        {
                            if (DisplayMode == DisplayMode.YearView) targetDate = (_lastArrowKeyDate ?? SelectedDate ?? DisplayDate).AddMonths(1);

                            break;
                        }
                        case DateSelectionMode.Year:
                        {
                            if (DisplayMode == DisplayMode.DecadeView) targetDate = (_lastArrowKeyDate ?? SelectedDate ?? DisplayDate).AddYears(1);

                            break;
                        }
                    }

                    if (targetDate != null)
                    {
                        if (SelectableDateStart != null && SelectableDateStart > targetDate) targetDate = SelectableDateStart;
                        if (SelectableDateEnd != null && SelectableDateEnd < targetDate) targetDate = SelectableDateEnd;

                        var isControlDown = Keyboard.IsKeyDown(Key.LeftCtrl) || Keyboard.IsKeyDown(Key.RightCtrl);
                        var isShiftDown = Keyboard.IsKeyDown(Key.LeftShift) || Keyboard.IsKeyDown(Key.RightShift);

                        if (isShiftDown)
                        {
                            SelectDateRange(SelectedDate ?? DisplayDate, targetDate.Value, !isControlDown);
                            if (isControlDown) SelectDate(targetDate.Value, targetDate < _firstDisplayedDate || targetDate > _lastDisplayedDate);
                        }
                        else
                        {
                            if (isControlDown) SelectDate(targetDate.Value, targetDate < _firstDisplayedDate || targetDate > _lastDisplayedDate);
                            else UnSelectAllExcept(targetDate.Value, targetDate < _firstDisplayedDate || targetDate > _lastDisplayedDate);
                        }
                        _lastArrowKeyDate = targetDate;
                    }

                    break;
                }
            }
        }

        private void HandleArrowDownKey()
        {
            if (IsReadOnly) return;

            switch (SelectionMode)
            {
                case SelectionMode.Single:
                {
                    DateTime? targetDate = null;

                    switch (DateSelectionMode)
                    {
                        case DateSelectionMode.Day:
                        {
                            if (DisplayMode == DisplayMode.MonthView) targetDate = (SelectedDate ?? DisplayDate).AddDays(7);

                            break;
                        }
                        case DateSelectionMode.Month:
                        {
                            if (DisplayMode == DisplayMode.YearView) targetDate = (SelectedDate ?? DisplayDate).AddMonths(4);

                            break;
                        }
                        case DateSelectionMode.Year:
                        {
                            if (DisplayMode == DisplayMode.DecadeView) targetDate = (SelectedDate ?? DisplayDate).AddYears(4);

                            break;
                        }
                    }

                    if (targetDate != null)
                    {
                        if (SelectableDateStart != null && SelectableDateStart > targetDate) targetDate = SelectableDateStart;
                        if (SelectableDateEnd != null && SelectableDateEnd < targetDate) targetDate = SelectableDateEnd;

                        SelectDate(targetDate.Value, targetDate < _firstDisplayedDate || targetDate > _lastDisplayedDate);
                    }

                    break;
                }
                // Wahrscheinlich sollten die beiden Modi leicht anders funktionieren aber für den Moment ist bei beiden die Logik gleich
                case SelectionMode.Multiple:
                case SelectionMode.Extended:
                {
                    DateTime? targetDate = null;

                    switch (DateSelectionMode)
                    {
                        case DateSelectionMode.Day:
                        {
                            if (DisplayMode == DisplayMode.MonthView) targetDate = (_lastArrowKeyDate ?? SelectedDate ?? DisplayDate).AddDays(7);

                            break;
                        }
                        case DateSelectionMode.Month:
                        {
                            if (DisplayMode == DisplayMode.YearView) targetDate = (_lastArrowKeyDate ?? SelectedDate ?? DisplayDate).AddMonths(4);

                            break;
                        }
                        case DateSelectionMode.Year:
                        {
                            if (DisplayMode == DisplayMode.DecadeView) targetDate = (_lastArrowKeyDate ?? SelectedDate ?? DisplayDate).AddYears(4);

                            break;
                        }
                    }

                    if (targetDate != null)
                    {
                        if (SelectableDateStart != null && SelectableDateStart > targetDate) targetDate = SelectableDateStart;
                        if (SelectableDateEnd != null && SelectableDateEnd < targetDate) targetDate = SelectableDateEnd;

                        var isControlDown = Keyboard.IsKeyDown(Key.LeftCtrl) || Keyboard.IsKeyDown(Key.RightCtrl);
                        var isShiftDown = Keyboard.IsKeyDown(Key.LeftShift) || Keyboard.IsKeyDown(Key.RightShift);

                        if (isShiftDown)
                        {
                            SelectDateRange(SelectedDate ?? DisplayDate, targetDate.Value, !isControlDown);
                            if (isControlDown) SelectDate(targetDate.Value, targetDate < _firstDisplayedDate || targetDate > _lastDisplayedDate);
                        }
                        else
                        {
                            if (isControlDown) SelectDate(targetDate.Value, targetDate < _firstDisplayedDate || targetDate > _lastDisplayedDate);
                            else UnSelectAllExcept(targetDate.Value, targetDate < _firstDisplayedDate || targetDate > _lastDisplayedDate);
                        }
                        _lastArrowKeyDate = targetDate;
                    }

                    break;
                }
            }
        }
    }
}