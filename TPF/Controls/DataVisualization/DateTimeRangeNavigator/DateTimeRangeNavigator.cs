using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using TPF.Internal;
using TPF.Collections;
using TPF.Controls.Specialized.DateTimeRangeNavigator;

namespace TPF.Controls
{
    public class DateTimeRangeNavigator : ContentControl
    {
        static DateTimeRangeNavigator()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(DateTimeRangeNavigator), new FrameworkPropertyMetadata(typeof(DateTimeRangeNavigator)));
        }

        public DateTimeRangeNavigator()
        {
            SetValue(IntervalsPropertyKey, new IntervalCollection());
            GroupIntervalPeriods = new RangeObservableCollection<IntervalPeriod>();
            ItemIntervalPeriods = new RangeObservableCollection<IntervalPeriod>();
            IntervalManager = new IntervalManager(this);
        }

        #region VisibleRangeChanged RoutedEvent
        public static readonly RoutedEvent VisibleRangeChangedEvent = EventManager.RegisterRoutedEvent("VisibleRangeChanged",
            RoutingStrategy.Bubble,
            typeof(RangeChangedEventHandler<DateTime>),
            typeof(DateTimeRangeNavigator));

        public event RangeChangedEventHandler<DateTime> VisibleRangeChanged
        {
            add => AddHandler(VisibleRangeChangedEvent, value);
            remove => RemoveHandler(VisibleRangeChangedEvent, value);
        }
        #endregion

        #region SelectedRangeChanged RoutedEvent
        public static readonly RoutedEvent SelectedRangeChangedEvent = EventManager.RegisterRoutedEvent("SelectedRangeChanged",
            RoutingStrategy.Bubble,
            typeof(RangeChangedEventHandler<DateTime>),
            typeof(DateTimeRangeNavigator));

        public event RangeChangedEventHandler<DateTime> SelectedRangeChanged
        {
            add => AddHandler(SelectedRangeChangedEvent, value);
            remove => RemoveHandler(SelectedRangeChangedEvent, value);
        }
        #endregion

        #region Intervals ReadOnly DependencyProperty
        private static readonly DependencyPropertyKey IntervalsPropertyKey = DependencyProperty.RegisterReadOnly("Intervals",
            typeof(IntervalCollection),
            typeof(DateTimeRangeNavigator),
            new PropertyMetadata(null));

        public static readonly DependencyProperty IntervalsProperty = IntervalsPropertyKey.DependencyProperty;

        public IntervalCollection Intervals
        {
            get { return (IntervalCollection)GetValue(IntervalsProperty); }
        }
        #endregion

        #region CurrentGroupInterval ReadOnly DependencyProperty
        private static readonly DependencyPropertyKey CurrentGroupIntervalPropertyKey = DependencyProperty.RegisterReadOnly("CurrentGroupInterval",
            typeof(IntervalBase),
            typeof(DateTimeRangeNavigator),
            new PropertyMetadata(null, OnCurrentGroupIntervalPropertyChanged));

        public static readonly DependencyProperty CurrentGroupIntervalProperty = CurrentGroupIntervalPropertyKey.DependencyProperty;

        private static void OnCurrentGroupIntervalPropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            var instance = (DateTimeRangeNavigator)sender;

            instance.OnGroupIntervalChanged();
        }

        public IntervalBase CurrentGroupInterval
        {
            get { return (IntervalBase)GetValue(CurrentGroupIntervalProperty); }
            internal set { SetValue(CurrentGroupIntervalPropertyKey, value); }
        }
        #endregion

        #region CurrentItemInterval ReadOnly DependencyProperty
        private static readonly DependencyPropertyKey CurrentItemIntervalPropertyKey = DependencyProperty.RegisterReadOnly("CurrentItemInterval",
            typeof(IntervalBase),
            typeof(DateTimeRangeNavigator),
            new PropertyMetadata(null, OnCurrentItemIntervalPropertyChanged));

        public static readonly DependencyProperty CurrentItemIntervalProperty = CurrentItemIntervalPropertyKey.DependencyProperty;

        private static void OnCurrentItemIntervalPropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            var instance = (DateTimeRangeNavigator)sender;

            instance.OnItemIntervalChanged();
        }

        public IntervalBase CurrentItemInterval
        {
            get { return (IntervalBase)GetValue(CurrentItemIntervalProperty); }
            internal set { SetValue(CurrentItemIntervalPropertyKey, value); }
        }
        #endregion

        #region GroupIntervalPeriods ReadOnly DependencyProperty
        private static readonly DependencyPropertyKey GroupIntervalPeriodsPropertyKey = DependencyProperty.RegisterReadOnly("GroupIntervalPeriods",
            typeof(RangeObservableCollection<IntervalPeriod>),
            typeof(DateTimeRangeNavigator),
            new PropertyMetadata(null));

        public static readonly DependencyProperty GroupIntervalPeriodsProperty = GroupIntervalPeriodsPropertyKey.DependencyProperty;

        [Browsable(false)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public RangeObservableCollection<IntervalPeriod> GroupIntervalPeriods
        {
            get { return (RangeObservableCollection<IntervalPeriod>)GetValue(GroupIntervalPeriodsProperty); }
            private set { SetValue(GroupIntervalPeriodsPropertyKey, value); }
        }
        #endregion

        #region ItemIntervalPeriods ReadOnly DependencyProperty
        private static readonly DependencyPropertyKey ItemIntervalPeriodsPropertyKey = DependencyProperty.RegisterReadOnly("ItemIntervalPeriods",
            typeof(RangeObservableCollection<IntervalPeriod>),
            typeof(DateTimeRangeNavigator),
            new PropertyMetadata(null));

        public static readonly DependencyProperty ItemIntervalPeriodsProperty = ItemIntervalPeriodsPropertyKey.DependencyProperty;

        [Browsable(false)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public RangeObservableCollection<IntervalPeriod> ItemIntervalPeriods
        {
            get { return (RangeObservableCollection<IntervalPeriod>)GetValue(ItemIntervalPeriodsProperty); }
            private set { SetValue(ItemIntervalPeriodsPropertyKey, value); }
        }
        #endregion

        #region GroupIntervalLabelStyle DependencyProperty
        public static readonly DependencyProperty GroupIntervalLabelStyleProperty = DependencyProperty.Register("GroupIntervalLabelStyle",
            typeof(Style),
            typeof(DateTimeRangeNavigator),
            new PropertyMetadata(null));

        public Style GroupIntervalLabelStyle
        {
            get { return (Style)GetValue(GroupIntervalLabelStyleProperty); }
            set { SetValue(GroupIntervalLabelStyleProperty, value); }
        }
        #endregion

        #region ItemIntervalLabelStyle DependencyProperty
        public static readonly DependencyProperty ItemIntervalLabelStyleProperty = DependencyProperty.Register("ItemIntervalLabelStyle",
            typeof(Style),
            typeof(DateTimeRangeNavigator),
            new PropertyMetadata(null));

        public Style ItemIntervalLabelStyle
        {
            get { return (Style)GetValue(ItemIntervalLabelStyleProperty); }
            set { SetValue(ItemIntervalLabelStyleProperty, value); }
        }
        #endregion

        #region Start DependencyProperty
        public static readonly DependencyProperty StartProperty = DependencyProperty.Register("Start",
            typeof(DateTime),
            typeof(DateTimeRangeNavigator),
            new PropertyMetadata(DateTime.MinValue, OnStartPropertyChanged));

        private static void OnStartPropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            var instance = (DateTimeRangeNavigator)sender;

            instance.OnStartChanged((DateTime)e.OldValue, (DateTime)e.NewValue);
        }

        public DateTime Start
        {
            get { return (DateTime)GetValue(StartProperty); }
            set { SetValue(StartProperty, value); }
        }
        #endregion

        #region End DependencyProperty
        public static readonly DependencyProperty EndProperty = DependencyProperty.Register("End",
            typeof(DateTime),
            typeof(DateTimeRangeNavigator),
            new PropertyMetadata(DateTime.MinValue, OnEndPropertyChanged, CoerceEnd));

        private static object CoerceEnd(DependencyObject sender, object value)
        {
            var instance = (DateTimeRangeNavigator)sender;

            var end = (DateTime)value;

            if (end < instance.Start) end = instance.Start;

            return end;
        }

        private static void OnEndPropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            var instance = (DateTimeRangeNavigator)sender;

            instance.OnEndChanged((DateTime)e.OldValue, (DateTime)e.NewValue);
        }

        public DateTime End
        {
            get { return (DateTime)GetValue(EndProperty); }
            set { SetValue(EndProperty, value); }
        }
        #endregion

        #region VisibleStart DependencyProperty
        public static readonly DependencyProperty VisibleStartProperty = DependencyProperty.Register("VisibleStart",
            typeof(DateTime),
            typeof(DateTimeRangeNavigator),
            new PropertyMetadata(DateTime.MinValue, OnVisibleStartPropertyChanged, CoerceVisibleStart));

        private static object CoerceVisibleStart(DependencyObject sender, object value)
        {
            var instance = (DateTimeRangeNavigator)sender;

            var visibleStart = (DateTime)value;

            if (visibleStart < instance.Start) visibleStart = instance.Start;
            if (visibleStart > instance.End) visibleStart = instance.End;

            return visibleStart;
        }

        private static void OnVisibleStartPropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            var instance = (DateTimeRangeNavigator)sender;

            instance.OnVisibleStartChanged((DateTime)e.OldValue, (DateTime)e.NewValue);
        }

        public DateTime VisibleStart
        {
            get { return (DateTime)GetValue(VisibleStartProperty); }
            set { SetValue(VisibleStartProperty, value); }
        }
        #endregion

        #region VisibleEnd DependencyProperty
        public static readonly DependencyProperty VisibleEndProperty = DependencyProperty.Register("VisibleEnd",
            typeof(DateTime),
            typeof(DateTimeRangeNavigator),
            new PropertyMetadata(DateTime.MinValue, OnVisibleEndPropertyChanged, CoerceVisibleEnd));

        private static object CoerceVisibleEnd(DependencyObject sender, object value)
        {
            var instance = (DateTimeRangeNavigator)sender;

            var visibleEnd = (DateTime)value;

            if (visibleEnd < instance.Start) visibleEnd = instance.Start;
            if (visibleEnd < instance.VisibleStart) visibleEnd = instance.VisibleStart;

            var visibleRange = visibleEnd - instance.VisibleStart;

            if (visibleRange < instance.MinimumZoomRange) visibleEnd = instance.VisibleStart + instance.MinimumZoomRange;
            else if (instance.MaximumZoomRange > TimeSpan.Zero && visibleRange > instance.MaximumZoomRange) visibleEnd = instance.VisibleStart + instance.MaximumZoomRange;

            if (visibleEnd > instance.End) visibleEnd = instance.End;

            return visibleEnd;
        }

        private static void OnVisibleEndPropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            var instance = (DateTimeRangeNavigator)sender;

            instance.OnVisibleEndChanged((DateTime)e.OldValue, (DateTime)e.NewValue);
        }

        public DateTime VisibleEnd
        {
            get { return (DateTime)GetValue(VisibleEndProperty); }
            set { SetValue(VisibleEndProperty, value); }
        }
        #endregion

        #region SelectedStart DependencyProperty
        public static readonly DependencyProperty SelectedStartProperty = DependencyProperty.Register("SelectedStart",
            typeof(DateTime),
            typeof(DateTimeRangeNavigator),
            new PropertyMetadata(DateTime.MinValue, OnSelectedStartPropertyChanged, CoerceSelectedStart));

        private static object CoerceSelectedStart(DependencyObject sender, object value)
        {
            var instance = (DateTimeRangeNavigator)sender;

            var selectedStart = (DateTime)value;

            if (selectedStart < instance.Start) selectedStart = instance.Start;
            if (selectedStart > instance.End) selectedStart = instance.End;

            return selectedStart;
        }

        private static void OnSelectedStartPropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            var instance = (DateTimeRangeNavigator)sender;

            instance.OnSelectedStartChanged((DateTime)e.OldValue, (DateTime)e.NewValue);
        }

        public DateTime SelectedStart
        {
            get { return (DateTime)GetValue(SelectedStartProperty); }
            set { SetValue(SelectedStartProperty, value); }
        }
        #endregion

        #region SelectedEnd DependencyProperty
        public static readonly DependencyProperty SelectedEndProperty = DependencyProperty.Register("SelectedEnd",
            typeof(DateTime),
            typeof(DateTimeRangeNavigator),
            new PropertyMetadata(DateTime.MinValue, OnSelectedEndPropertyChanged, CoerceSelectedEnd));

        private static object CoerceSelectedEnd(DependencyObject sender, object value)
        {
            var instance = (DateTimeRangeNavigator)sender;

            var selectedEnd = (DateTime)value;

            if (selectedEnd < instance.Start) selectedEnd = instance.Start;
            if (selectedEnd < instance.SelectedStart) selectedEnd = instance.SelectedStart;

            var selectedRange = selectedEnd - instance.SelectedStart;

            if (selectedRange < instance.MinimumSelectionRange) selectedEnd = instance.SelectedStart + instance.MinimumSelectionRange;
            else if (instance.MaximumSelectionRange > TimeSpan.Zero && selectedRange > instance.MaximumSelectionRange) selectedEnd = instance.SelectedStart + instance.MaximumSelectionRange;

            if (selectedEnd > instance.End) selectedEnd = instance.End;

            return selectedEnd;
        }

        private static void OnSelectedEndPropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            var instance = (DateTimeRangeNavigator)sender;

            instance.OnSelectedEndChanged((DateTime)e.OldValue, (DateTime)e.NewValue);
        }

        public DateTime SelectedEnd
        {
            get { return (DateTime)GetValue(SelectedEndProperty); }
            set { SetValue(SelectedEndProperty, value); }
        }
        #endregion

        #region MinimumZoomRange DependencyProperty
        public static readonly DependencyProperty MinimumZoomRangeProperty = DependencyProperty.Register("MinimumZoomRange",
            typeof(TimeSpan),
            typeof(DateTimeRangeNavigator),
            new PropertyMetadata(TimeSpan.Zero, OnMinimumZoomRangePropertyChanged, CoerceMinimumZoomRange));

        private static object CoerceMinimumZoomRange(DependencyObject sender, object value)
        {
            var minimumZoomRange = (TimeSpan)value;

            if (minimumZoomRange < TimeSpan.Zero) minimumZoomRange = TimeSpan.Zero;

            return minimumZoomRange;
        }

        private static void OnMinimumZoomRangePropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            var instance = (DateTimeRangeNavigator)sender;

            instance.OnMinimumZoomRangeChanged();
        }

        public TimeSpan MinimumZoomRange
        {
            get { return (TimeSpan)GetValue(MinimumZoomRangeProperty); }
            set { SetValue(MinimumZoomRangeProperty, value); }
        }
        #endregion

        #region MaximumZoomRange DependencyProperty
        public static readonly DependencyProperty MaximumZoomRangeProperty = DependencyProperty.Register("MaximumZoomRange",
            typeof(TimeSpan),
            typeof(DateTimeRangeNavigator),
            new PropertyMetadata(TimeSpan.Zero, OnMaximumZoomRangePropertyChanged, CoerceMaximumZoomRange));

        private static object CoerceMaximumZoomRange(DependencyObject sender, object value)
        {
            var instance = (DateTimeRangeNavigator)sender;

            var maximumZoomRange = (TimeSpan)value;

            if (maximumZoomRange <= TimeSpan.Zero) maximumZoomRange = TimeSpan.Zero;
            else if (maximumZoomRange < instance.MinimumZoomRange) maximumZoomRange = instance.MinimumZoomRange;

            return maximumZoomRange;
        }

        private static void OnMaximumZoomRangePropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            var instance = (DateTimeRangeNavigator)sender;

            instance.OnMaximumZoomRangeChanged();
        }

        public TimeSpan MaximumZoomRange
        {
            get { return (TimeSpan)GetValue(MaximumZoomRangeProperty); }
            set { SetValue(MaximumZoomRangeProperty, value); }
        }
        #endregion

        #region MinimumSelectionRange DependencyProperty
        public static readonly DependencyProperty MinimumSelectionRangeProperty = DependencyProperty.Register("MinimumSelectionRange",
            typeof(TimeSpan),
            typeof(DateTimeRangeNavigator),
            new PropertyMetadata(TimeSpan.Zero, OnMinimumSelectionRangePropertyChanged, CoerceMinimumSelectionRange));

        private static object CoerceMinimumSelectionRange(DependencyObject sender, object value)
        {
            var minimumSelectionRange = (TimeSpan)value;

            if (minimumSelectionRange < TimeSpan.Zero) minimumSelectionRange = TimeSpan.Zero;

            return minimumSelectionRange;
        }

        private static void OnMinimumSelectionRangePropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            var instance = (DateTimeRangeNavigator)sender;

            instance.OnMinimumSelectionRangeChanged();
        }

        public TimeSpan MinimumSelectionRange
        {
            get { return (TimeSpan)GetValue(MinimumSelectionRangeProperty); }
            set { SetValue(MinimumSelectionRangeProperty, value); }
        }
        #endregion

        #region MaximumSelectionRange DependencyProperty
        public static readonly DependencyProperty MaximumSelectionRangeProperty = DependencyProperty.Register("MaximumSelectionRange",
            typeof(TimeSpan),
            typeof(DateTimeRangeNavigator),
            new PropertyMetadata(TimeSpan.Zero, OnMaximumSelectionRangePropertyChanged, CoerceMaximumSelectionRange));

        private static object CoerceMaximumSelectionRange(DependencyObject sender, object value)
        {
            var instance = (DateTimeRangeNavigator)sender;

            var maximumSelectionRange = (TimeSpan)value;

            if (maximumSelectionRange <= TimeSpan.Zero) maximumSelectionRange = TimeSpan.Zero;
            else if (maximumSelectionRange < instance.MinimumSelectionRange) maximumSelectionRange = instance.MinimumSelectionRange;

            return maximumSelectionRange;
        }

        private static void OnMaximumSelectionRangePropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            var instance = (DateTimeRangeNavigator)sender;

            instance.OnMaximumSelectionRangeChanged();
        }

        public TimeSpan MaximumSelectionRange
        {
            get { return (TimeSpan)GetValue(MaximumSelectionRangeProperty); }
            set { SetValue(MaximumSelectionRangeProperty, value); }
        }
        #endregion

        #region ScrollBarVisibility DependencyProperty
        public static readonly DependencyProperty ScrollBarVisibilityProperty = DependencyProperty.Register("ScrollBarVisibility",
            typeof(Visibility),
            typeof(DateTimeRangeNavigator),
            new PropertyMetadata(VisibilityBoxes.VisibleBox));

        public Visibility ScrollBarVisibility
        {
            get { return (Visibility)GetValue(ScrollBarVisibilityProperty); }
            set { SetValue(ScrollBarVisibilityProperty, VisibilityBoxes.Box(value)); }
        }
        #endregion

        #region TimelineBackground DependencyProperty
        public static readonly DependencyProperty TimelineBackgroundProperty = DependencyProperty.Register("TimelineBackground",
            typeof(Brush),
            typeof(DateTimeRangeNavigator),
            new PropertyMetadata(null));

        public Brush TimelineBackground
        {
            get { return (Brush)GetValue(TimelineBackgroundProperty); }
            set { SetValue(TimelineBackgroundProperty, value); }
        }
        #endregion

        #region ContentOverlayBrush DependencyProperty
        public static readonly DependencyProperty ContentOverlayBrushProperty = DependencyProperty.Register("ContentOverlayBrush",
            typeof(Brush),
            typeof(DateTimeRangeNavigator),
            new PropertyMetadata(null));

        public Brush ContentOverlayBrush
        {
            get { return (Brush)GetValue(ContentOverlayBrushProperty); }
            set { SetValue(ContentOverlayBrushProperty, value); }
        }
        #endregion

        private ResizeableScrollBar _scrollBar;
        private IntervalSelectionThumb _selectionThumb;

        private bool _updatingScrollBar;
        private bool _scrolling;
        private bool _settingSelection;

        internal IntervalManager IntervalManager { get; }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            if (_scrollBar != null)
            {
                _scrollBar.Scrolling -= ScrollBar_Scrolling;
            }

            _scrollBar = GetTemplateChild("PART_ScrollBar") as ResizeableScrollBar;
            _selectionThumb = GetTemplateChild("PART_SelectionThumb") as IntervalSelectionThumb;

            if (_scrollBar != null)
            {
                UpdateScrollBar();
                _scrollBar.Scrolling += ScrollBar_Scrolling;
            }

            if (_selectionThumb != null)
            {
                _selectionThumb.Owner = this;
            }
        }

        internal void UpdateSelectionThumb()
        {
            if (_selectionThumb != null) _selectionThumb.InvalidateThumbs();
        }

        internal void SetSelectedInterval(DateTime start, DateTime end)
        {
            _settingSelection = true;
            SelectedStart = start;
            SelectedEnd = end;
            _settingSelection = false;

            RaiseSelectedRangeChanged();
        }

        protected virtual void OnStartChanged(DateTime oldValue, DateTime newValue)
        {
            CoerceValue(EndProperty);
            CoerceValue(VisibleStartProperty);
            CoerceValue(VisibleEndProperty);
            CoerceValue(SelectedStartProperty);
            CoerceValue(SelectedEndProperty);

            UpdateScrollBar();
            UpdateCurrentIntervals();
            RefreshCurrentIntervals();
        }

        protected virtual void OnEndChanged(DateTime oldValue, DateTime newValue)
        {
            CoerceValue(VisibleStartProperty);
            CoerceValue(VisibleEndProperty);
            CoerceValue(SelectedStartProperty);
            CoerceValue(SelectedEndProperty);

            UpdateScrollBar();
            UpdateCurrentIntervals();
            RefreshCurrentIntervals();
        }

        protected virtual void OnVisibleStartChanged(DateTime oldValue, DateTime newValue)
        {
            CoerceValue(VisibleEndProperty);

            UpdateScrollBar();
            UpdateSelectionThumb();
            UpdateCurrentIntervals();
            RaiseVisibleRangeChanged();
        }

        protected virtual void OnVisibleEndChanged(DateTime oldValue, DateTime newValue)
        {
            UpdateScrollBar();
            UpdateSelectionThumb();
            UpdateCurrentIntervals();
            RaiseVisibleRangeChanged();
        }

        protected virtual void OnSelectedStartChanged(DateTime oldValue, DateTime newValue)
        {
            UpdateScrollBar();
            UpdateSelectionThumb();
            CoerceValue(SelectedEndProperty);
            RaiseSelectedRangeChanged();
        }

        protected virtual void OnSelectedEndChanged(DateTime oldValue, DateTime newValue)
        {
            UpdateScrollBar();
            UpdateSelectionThumb();
            RaiseSelectedRangeChanged();
        }

        protected virtual void OnMinimumZoomRangeChanged()
        {
            CoerceValue(MaximumZoomRangeProperty);
            CoerceValue(VisibleEndProperty);

            UpdateScrollBar();
        }

        protected virtual void OnMaximumZoomRangeChanged()
        {
            CoerceValue(VisibleEndProperty);

            UpdateScrollBar();
        }

        protected virtual void OnMinimumSelectionRangeChanged()
        {
            CoerceValue(MaximumSelectionRangeProperty);
            CoerceValue(SelectedEndProperty);
        }

        protected virtual void OnMaximumSelectionRangeChanged()
        {
            CoerceValue(SelectedEndProperty);
        }

        private void UpdateScrollBar()
        {
            if (_scrollBar == null || _updatingScrollBar ||_scrolling) return;

            var start = Start.Ticks;
            var end = End.Ticks;
            var visibleStart = VisibleStart.Ticks;
            var visibleEnd = VisibleEnd.Ticks;
            var minimumRange = MinimumZoomRange.Ticks;
            var maximumRange = MaximumZoomRange.Ticks;
            var highlightStart = SelectedStart.Ticks;
            var highlightEnd = SelectedEnd.Ticks;

            _updatingScrollBar = true;
            _scrollBar.SuppressSpanCoercion = true;
            _scrollBar.Minimum = start;
            _scrollBar.Maximum = end;
            _scrollBar.RangeStart = visibleStart;
            _scrollBar.RangeEnd = visibleEnd;
            _scrollBar.HighlightStart = highlightStart;
            _scrollBar.HighlightEnd = highlightEnd;
            _scrollBar.MinimumRangeSpan = minimumRange;
            if (maximumRange > 0) _scrollBar.MaximumRangeSpan = maximumRange;
            _scrollBar.SuppressSpanCoercion = false;
            _updatingScrollBar = false;
        }

        private void ScrollBar_Scrolling(object sender, RangeScrollingEventArgs e)
        {
            if (_updatingScrollBar) return;

            var start = new DateTime((long)e.NewRange.Start);
            var end = new DateTime((long)e.NewRange.End);

            _scrolling = true;
            VisibleStart = start;
            VisibleEnd = end;
            _scrolling = false;

            RaiseVisibleRangeChanged();
        }

        private void RaiseVisibleRangeChanged()
        {
            if (_scrolling) return;
            
            var e = new RangeChangedEventArgs<DateTime>(VisibleRangeChangedEvent, VisibleStart, VisibleEnd);

            RaiseEvent(e);
        }

        private void RaiseSelectedRangeChanged()
        {
            if (_settingSelection) return;

            var e = new RangeChangedEventArgs<DateTime>(SelectedRangeChangedEvent, SelectedStart, SelectedEnd);

            RaiseEvent(e);
        }

        private void UpdateCurrentIntervals()
        {
            IntervalManager.DetermineCurrentIntervals();
        }

        private void RefreshCurrentIntervals()
        {
            OnGroupIntervalChanged();
            OnItemIntervalChanged();
        }

        private void OnGroupIntervalChanged()
        {
            GroupIntervalPeriods.Clear();

            if (CurrentGroupInterval == null) return;

            GroupIntervalPeriods.AddRange(IntervalManager.GetIntervalPeriods(CurrentGroupInterval));
        }

        private void OnItemIntervalChanged()
        {
            ItemIntervalPeriods.Clear();

            if (CurrentItemInterval == null) return;

            ItemIntervalPeriods.AddRange(IntervalManager.GetIntervalPeriods(CurrentItemInterval));
        }
    }
}