using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using TPF.Data;
using TPF.Internal;

namespace TPF.Controls
{
    [TemplatePart(Name = "PART_Track", Type = typeof(ResizeableScrollBarTrack))]
    public class ResizeableScrollBar : Control
    {
        static ResizeableScrollBar()
        {
            InitializeCommands();

            DefaultStyleKeyProperty.OverrideMetadata(typeof(ResizeableScrollBar), new FrameworkPropertyMetadata(typeof(ResizeableScrollBar)));
        }

        #region Scrolling RoutedEvent
        public static readonly RoutedEvent ScrollingEvent = EventManager.RegisterRoutedEvent("Scrolling",
            RoutingStrategy.Bubble,
            typeof(RangeScrollingEventHandler),
            typeof(ResizeableScrollBar));

        public event RangeScrollingEventHandler Scrolling
        {
            add => AddHandler(ScrollingEvent, value);
            remove => RemoveHandler(ScrollingEvent, value);
        }
        #endregion

        #region Orientation DependencyProperty
        public static readonly DependencyProperty OrientationProperty = DependencyProperty.Register("Orientation",
            typeof(Orientation),
            typeof(ResizeableScrollBar),
            new PropertyMetadata(Orientation.Horizontal));

        public Orientation Orientation
        {
            get { return (Orientation)GetValue(OrientationProperty); }
            set { SetValue(OrientationProperty, value); }
        }
        #endregion

        #region ButtonVisibility DependencyProperty
        public static readonly DependencyProperty ButtonVisibilityProperty = DependencyProperty.Register("ButtonVisibility",
            typeof(Visibility),
            typeof(ResizeableScrollBar),
            new PropertyMetadata(VisibilityBoxes.CollapsedBox));

        public Visibility ButtonVisibility
        {
            get { return (Visibility)GetValue(ButtonVisibilityProperty); }
            set { SetValue(ButtonVisibilityProperty, VisibilityBoxes.Box(value)); }
        }
        #endregion

        #region Minimum DependencyProperty
        public static readonly DependencyProperty MinimumProperty = DependencyProperty.Register("Minimum",
            typeof(double),
            typeof(ResizeableScrollBar),
            new PropertyMetadata(0d, OnMinimumPropertyChanged));

        private static void OnMinimumPropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            var instance = (ResizeableScrollBar)sender;

            instance.OnMinimumChanged((double)e.OldValue, (double)e.NewValue);
        }

        public double Minimum
        {
            get { return (double)GetValue(MinimumProperty); }
            set { SetValue(MinimumProperty, value); }
        }
        #endregion

        #region Maximum DependencyProperty
        public static readonly DependencyProperty MaximumProperty = DependencyProperty.Register("Maximum",
            typeof(double),
            typeof(ResizeableScrollBar),
            new PropertyMetadata(1d, OnMaximumPropertyChanged));

        private static void OnMaximumPropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            var instance = (ResizeableScrollBar)sender;

            instance.OnMaximumChanged((double)e.OldValue, (double)e.NewValue);
        }

        public double Maximum
        {
            get { return (double)GetValue(MaximumProperty); }
            set { SetValue(MaximumProperty, value); }
        }
        #endregion

        #region RangeStart DependencyProperty
        public static readonly DependencyProperty RangeStartProperty = DependencyProperty.Register("RangeStart",
            typeof(double),
            typeof(ResizeableScrollBar),
            new PropertyMetadata(0d, OnRangeStartPropertyChanged, CoerceRangeStart));

        private static object CoerceRangeStart(DependencyObject sender, object value)
        {
            var instance = (ResizeableScrollBar)sender;
            var rangeStart = (double)value;

            if (!instance.SuppressSpanCoercion && instance.RangeEnd - rangeStart < instance.MinimumRangeSpan)
            {
                rangeStart = Math.Max(instance.RangeEnd - instance.MinimumRangeSpan, instance.Minimum);
            }
            else if (rangeStart + instance.MinimumRangeSpan > instance.Maximum)
            {
                rangeStart = Math.Max(instance.Minimum, instance.Maximum - instance.MinimumRangeSpan);
            }
            else if (!instance.SuppressSpanCoercion && instance.RangeEnd - rangeStart > instance.MaximumRangeSpan)
            {
                rangeStart = Math.Max(instance.RangeEnd - instance.MaximumRangeSpan, instance.Minimum);
            }
            else if (rangeStart <= instance.Minimum)
            {
                rangeStart = instance.Minimum;
            }
            else if (rangeStart >= instance.Maximum)
            {
                rangeStart = instance.Maximum;
            }

            return rangeStart;
        }

        private static void OnRangeStartPropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            var instance = (ResizeableScrollBar)sender;

            instance.OnRangeStartChanged((double)e.OldValue, (double)e.NewValue);
        }

        public double RangeStart
        {
            get { return (double)GetValue(RangeStartProperty); }
            set { SetValue(RangeStartProperty, value); }
        }
        #endregion

        #region RangeEnd DependencyProperty
        public static readonly DependencyProperty RangeEndProperty = DependencyProperty.Register("RangeEnd",
            typeof(double),
            typeof(ResizeableScrollBar),
            new PropertyMetadata(0d, OnRangeEndPropertyChanged, CoerceRangeEnd));

        private static object CoerceRangeEnd(DependencyObject sender, object value)
        {
            var instance = (ResizeableScrollBar)sender;
            var rangeEnd = (double)value;

            if (instance.RangeStart + instance.MinimumRangeSpan > rangeEnd)
            {
                rangeEnd = instance.RangeStart + instance.MinimumRangeSpan;
            }
            else if (!instance.SuppressSpanCoercion && rangeEnd - instance.RangeStart > instance.MaximumRangeSpan)
            {
                rangeEnd = Math.Min(instance.RangeStart + instance.MaximumRangeSpan, instance.Maximum);
            }
            else if (rangeEnd >= instance.Maximum)
            {
                rangeEnd = instance.Maximum;
            }

            return rangeEnd;
        }

        private static void OnRangeEndPropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            var instance = (ResizeableScrollBar)sender;

            instance.OnRangeEndChanged((double)e.OldValue, (double)e.NewValue);
        }

        public double RangeEnd
        {
            get { return (double)GetValue(RangeEndProperty); }
            set { SetValue(RangeEndProperty, value); }
        }
        #endregion

        #region HighlightStart DependencyProperty
        public static readonly DependencyProperty HighlightStartProperty = DependencyProperty.Register("HighlightStart",
            typeof(double),
            typeof(ResizeableScrollBar),
            new PropertyMetadata(0d, OnHighlightStartPropertyChanged, CoerceHighlightStart));

        private static object CoerceHighlightStart(DependencyObject sender, object value)
        {
            var instance = (ResizeableScrollBar)sender;
            var highlightStart = (double)value;

            if (highlightStart <= instance.Minimum)
            {
                highlightStart = instance.Minimum;
            }
            else if (highlightStart >= instance.Maximum)
            {
                highlightStart = instance.Maximum;
            }

            return highlightStart;
        }

        private static void OnHighlightStartPropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            var instance = (ResizeableScrollBar)sender;

            instance.CoerceValue(HighlightEndProperty);
        }

        public double HighlightStart
        {
            get { return (double)GetValue(HighlightStartProperty); }
            set { SetValue(HighlightStartProperty, value); }
        }
        #endregion

        #region HighlightEnd DependencyProperty
        public static readonly DependencyProperty HighlightEndProperty = DependencyProperty.Register("HighlightEnd",
            typeof(double),
            typeof(ResizeableScrollBar),
            new PropertyMetadata(0d, null, CoerceHighlightEnd));

        private static object CoerceHighlightEnd(DependencyObject sender, object value)
        {
            var instance = (ResizeableScrollBar)sender;
            var highlightEnd = (double)value;

            if (highlightEnd < instance.HighlightStart)
            {
                highlightEnd = instance.HighlightStart;
            }

            if (highlightEnd <= instance.Minimum)
            {
                highlightEnd = instance.Minimum;
            }
            else if (highlightEnd >= instance.Maximum)
            {
                highlightEnd = instance.Maximum;
            }

            return highlightEnd;
        }

        public double HighlightEnd
        {
            get { return (double)GetValue(HighlightEndProperty); }
            set { SetValue(HighlightEndProperty, value); }
        }
        #endregion

        #region HighlightBrush DependencyProperty
        public static readonly DependencyProperty HighlightBrushProperty = DependencyProperty.Register("HighlightBrush",
            typeof(Brush),
            typeof(ResizeableScrollBar),
            new PropertyMetadata(null));

        public Brush HighlightBrush
        {
            get { return (Brush)GetValue(HighlightBrushProperty); }
            set { SetValue(HighlightBrushProperty, value); }
        }
        #endregion

        #region MinimumRangeSpan DependencyProperty
        public static readonly DependencyProperty MinimumRangeSpanProperty = DependencyProperty.Register("MinimumRangeSpan",
            typeof(double),
            typeof(ResizeableScrollBar),
            new PropertyMetadata(0d, OnMinimumRangeSpanPropertyChanged, CoerceMinimumRangeSpan));

        private static object CoerceMinimumRangeSpan(DependencyObject sender, object value)
        {
            var instance = (ResizeableScrollBar)sender;

            var minimumSpan = (double)value;

            if (minimumSpan > instance.Maximum - instance.Minimum)
            {
                minimumSpan = instance.Maximum - instance.Minimum;
            }

            return minimumSpan;
        }

        private static void OnMinimumRangeSpanPropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            var instance = (ResizeableScrollBar)sender;

            instance.CoerceValue(RangeStartProperty);
            instance.CoerceValue(RangeEndProperty);
        }

        public double MinimumRangeSpan
        {
            get { return (double)GetValue(MinimumRangeSpanProperty); }
            set { SetValue(MinimumRangeSpanProperty, value); }
        }
        #endregion

        #region MaximumRangeSpan DependencyProperty
        public static readonly DependencyProperty MaximumRangeSpanProperty = DependencyProperty.Register("MaximumRangeSpan",
            typeof(double),
            typeof(ResizeableScrollBar),
            new PropertyMetadata(double.PositiveInfinity, OnMaximumRangeSpanPropertyChanged, CoerceMaximumRangeSpan));

        private static object CoerceMaximumRangeSpan(DependencyObject sender, object value)
        {
            var instance = (ResizeableScrollBar)sender;

            var maximumSpan = (double)value;

            if (maximumSpan < instance.MinimumRangeSpan)
            {
                maximumSpan = instance.MinimumRangeSpan;
            }
            else if (maximumSpan > instance.Maximum - instance.Minimum)
            {
                maximumSpan = instance.Maximum - instance.Minimum;
            }

            return maximumSpan;
        }

        private static void OnMaximumRangeSpanPropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            var instance = (ResizeableScrollBar)sender;

            instance.CoerceValue(RangeStartProperty);
            instance.CoerceValue(RangeEndProperty);
        }

        public double MaximumRangeSpan
        {
            get { return (double)GetValue(MaximumRangeSpanProperty); }
            set { SetValue(MaximumRangeSpanProperty, value); }
        }
        #endregion

        #region SmallChange DependencyProperty
        public static readonly DependencyProperty SmallChangeProperty = DependencyProperty.Register("SmallChange",
            typeof(double),
            typeof(ResizeableScrollBar),
            new PropertyMetadata(0.1));

        public double SmallChange
        {
            get { return (double)GetValue(SmallChangeProperty); }
            set { SetValue(SmallChangeProperty, value); }
        }
        #endregion

        #region LargeChange DependencyProperty
        public static readonly DependencyProperty LargeChangeProperty = DependencyProperty.Register("LargeChange",
            typeof(double),
            typeof(ResizeableScrollBar),
            new PropertyMetadata(1d));

        public double LargeChange
        {
            get { return (double)GetValue(LargeChangeProperty); }
            set { SetValue(LargeChangeProperty, value); }
        }
        #endregion

        #region StartThumbStyle DependencyProperty
        public static readonly DependencyProperty StartThumbStyleProperty = DependencyProperty.Register("StartThumbStyle",
            typeof(Style),
            typeof(ResizeableScrollBar),
            new PropertyMetadata(null));

        public Style StartThumbStyle
        {
            get { return (Style)GetValue(StartThumbStyleProperty); }
            set { SetValue(StartThumbStyleProperty, value); }
        }
        #endregion

        #region MiddleThumbStyle DependencyProperty
        public static readonly DependencyProperty MiddleThumbStyleProperty = DependencyProperty.Register("MiddleThumbStyle",
            typeof(Style),
            typeof(ResizeableScrollBar),
            new PropertyMetadata(null));

        public Style MiddleThumbStyle
        {
            get { return (Style)GetValue(MiddleThumbStyleProperty); }
            set { SetValue(MiddleThumbStyleProperty, value); }
        }
        #endregion

        #region EndThumbStyle DependencyProperty
        public static readonly DependencyProperty EndThumbStyleProperty = DependencyProperty.Register("EndThumbStyle",
            typeof(Style),
            typeof(ResizeableScrollBar),
            new PropertyMetadata(null));

        public Style EndThumbStyle
        {
            get { return (Style)GetValue(EndThumbStyleProperty); }
            set { SetValue(EndThumbStyleProperty, value); }
        }
        #endregion

        #region Commands
        public static RoutedCommand LineUp { get; private set; }
        public static RoutedCommand LineDown { get; private set; }
        public static RoutedCommand LineLeft { get; private set; }
        public static RoutedCommand LineRight { get; private set; }
        public static RoutedCommand PageUp { get; private set; }
        public static RoutedCommand PageDown { get; private set; }
        public static RoutedCommand PageLeft { get; private set; }
        public static RoutedCommand PageRight { get; private set; }
        public static RoutedCommand ExpandFull { get; private set; }
        public static RoutedCommand ScrollToStart { get; private set; }
        public static RoutedCommand ScrollToEnd { get; private set; }

        private static void InitializeCommands()
        {
            var type = typeof(ResizeableScrollBar);

            LineUp = new RoutedCommand("LineUp", type);
            LineDown = new RoutedCommand("LineDown", type);
            LineLeft = new RoutedCommand("LineLeft", type);
            LineRight = new RoutedCommand("LineRight", type);
            PageUp = new RoutedCommand("PageUp", type);
            PageDown = new RoutedCommand("PageDown", type);
            PageLeft = new RoutedCommand("PageLeft", type);
            PageRight = new RoutedCommand("PageRight", type);
            ExpandFull = new RoutedCommand("ExpandFull", type);
            ScrollToStart = new RoutedCommand("ScrollToStart", type);
            ScrollToEnd = new RoutedCommand("ScrollToEnd", type);

            CommandManager.RegisterClassCommandBinding(type, new CommandBinding(LineUp, OnScrollCommand));
            CommandManager.RegisterClassCommandBinding(type, new CommandBinding(LineDown, OnScrollCommand));
            CommandManager.RegisterClassCommandBinding(type, new CommandBinding(LineLeft, OnScrollCommand));
            CommandManager.RegisterClassCommandBinding(type, new CommandBinding(LineRight, OnScrollCommand));
            CommandManager.RegisterClassCommandBinding(type, new CommandBinding(PageUp, OnScrollCommand));
            CommandManager.RegisterClassCommandBinding(type, new CommandBinding(PageDown, OnScrollCommand));
            CommandManager.RegisterClassCommandBinding(type, new CommandBinding(PageLeft, OnScrollCommand));
            CommandManager.RegisterClassCommandBinding(type, new CommandBinding(PageRight, OnScrollCommand));
            CommandManager.RegisterClassCommandBinding(type, new CommandBinding(ExpandFull, OnScrollCommand));
            CommandManager.RegisterClassCommandBinding(type, new CommandBinding(ScrollToStart, OnScrollCommand));
            CommandManager.RegisterClassCommandBinding(type, new CommandBinding(ScrollToEnd, OnScrollCommand));

            CommandManager.RegisterClassInputBinding(type, new InputBinding(LineUp, new KeyGesture(Key.Up)));
            CommandManager.RegisterClassInputBinding(type, new InputBinding(LineDown, new KeyGesture(Key.Down)));
            CommandManager.RegisterClassInputBinding(type, new InputBinding(LineLeft, new KeyGesture(Key.Left)));
            CommandManager.RegisterClassInputBinding(type, new InputBinding(LineRight, new KeyGesture(Key.Right)));
            CommandManager.RegisterClassInputBinding(type, new InputBinding(PageUp, new KeyGesture(Key.PageUp)));
            CommandManager.RegisterClassInputBinding(type, new InputBinding(PageDown, new KeyGesture(Key.PageDown)));
            CommandManager.RegisterClassInputBinding(type, new InputBinding(ScrollToStart, new KeyGesture(Key.Home)));
            CommandManager.RegisterClassInputBinding(type, new InputBinding(ScrollToEnd, new KeyGesture(Key.End)));
        }

        private static void OnScrollCommand(object sender, ExecutedRoutedEventArgs e)
        {
            var instance = (ResizeableScrollBar)sender;

            if (e.Command == ExpandFull)
            {
                instance.OnExpandFull();
            }
            else if (e.Command == ScrollToStart)
            {
                instance.OnScrollToStart();
            }
            else if (e.Command == ScrollToEnd)
            {
                instance.OnScrollToEnd();
            }

            if (instance.Orientation == Orientation.Horizontal)
            {
                if (e.Command == LineLeft)
                {
                    instance.OnDecreaseSmall();
                }
                else if (e.Command == LineRight)
                {
                    instance.OnIncreaseSmall();
                }
                else if (e.Command == PageLeft)
                {
                    instance.OnDecreaseLarge();
                }
                else if (e.Command == PageRight)
                {
                    instance.OnIncreaseLarge();
                }
            }
            else
            {
                if (e.Command == LineUp)
                {
                    instance.OnDecreaseSmall();
                }
                else if (e.Command == LineDown)
                {
                    instance.OnIncreaseSmall();
                }
                else if (e.Command == PageUp)
                {
                    instance.OnDecreaseLarge();
                }
                else if (e.Command == PageDown)
                {
                    instance.OnIncreaseLarge();
                }
            }
        }

        protected virtual void OnExpandFull()
        {
            RangeStart = Minimum;
            RangeEnd = Maximum;
        }

        protected virtual void OnScrollToStart()
        {
            var range = RangeEnd - RangeStart;

            RangeStart = Minimum;
            RangeEnd = RangeStart + range;
        }

        protected virtual void OnScrollToEnd()
        {
            var range = RangeEnd - RangeStart;

            RangeStart = Maximum - range;
            RangeEnd = Maximum;
        }

        protected virtual void OnDecreaseSmall()
        {
            DecreaseValue(SmallChange);
        }

        protected virtual void OnIncreaseSmall()
        {
            IncreaseValue(SmallChange);
        }

        protected virtual void OnDecreaseLarge()
        {
            DecreaseValue(LargeChange);
        }

        protected virtual void OnIncreaseLarge()
        {
            IncreaseValue(LargeChange);
        }

        private void DecreaseValue(double value)
        {
            var newValue = Math.Max(Minimum, RangeStart - value);

            if (newValue == RangeStart) return;

            var range = RangeEnd - RangeStart;

            RangeStart = newValue;
            RangeEnd = RangeStart + range;
        }

        private void IncreaseValue(double value)
        {
            var newValue = Math.Min(Maximum, RangeEnd + value);

            if (newValue == RangeEnd) return;

            var range = RangeEnd - RangeStart;

            SuppressSpanCoercion = true;
            RangeEnd = newValue;
            RangeStart = RangeEnd - range;
            SuppressSpanCoercion = false;
        }
        #endregion

        internal bool SuppressSpanCoercion;
        private bool _suppressScrollingEvent;

        protected virtual void OnMinimumChanged(double oldMinimum, double newMinimum)
        {
            CoerceValue(RangeStartProperty);
            CoerceValue(RangeEndProperty);
            CoerceValue(HighlightStartProperty);
            CoerceValue(HighlightEndProperty);
        }

        protected virtual void OnMaximumChanged(double oldMaximum, double newMaximum)
        {
            CoerceValue(RangeStartProperty);
            CoerceValue(RangeEndProperty);
            CoerceValue(HighlightStartProperty);
            CoerceValue(HighlightEndProperty);
        }

        protected virtual void OnRangeStartChanged(double oldValue, double newValue)
        {
            RaiseScrollingEvent();
        }

        protected virtual void OnRangeEndChanged(double oldValue, double newValue)
        {
            RaiseScrollingEvent();
        }

        private void RaiseScrollingEvent()
        {
            if (_suppressScrollingEvent) return;

            var range = new DoubleRange(RangeStart, RangeEnd);

            var e = new RangeScrollingEventArgs(ScrollingEvent, range);

            RaiseEvent(e);
        }

        internal void MoveRangeStart(double delta)
        {
            var newValue = RangeStart + delta;

            newValue = Math.Max(Minimum, Math.Min(Maximum, newValue));

            RangeStart = newValue;
        }

        internal void MoveRangeEnd(double delta)
        {
            var newValue = RangeEnd + delta;

            newValue = Math.Max(Minimum, Math.Min(Maximum, newValue));

            RangeEnd = newValue;
        }

        internal void MoveRange(double delta)
        {
            var valueDelta = delta;
            var rangeStart = RangeStart;
            var rangeEnd = RangeEnd;

            if (rangeStart + valueDelta < Minimum) valueDelta = Minimum - rangeStart;
            else if (rangeEnd + valueDelta > Maximum) valueDelta = Maximum - rangeEnd;

            rangeStart += valueDelta;
            rangeEnd += valueDelta;

            SuppressSpanCoercion = true;
            _suppressScrollingEvent = true;
            RangeStart = rangeStart;
            RangeEnd = rangeEnd;
            _suppressScrollingEvent = false;
            SuppressSpanCoercion = false;

            RaiseScrollingEvent();
        }
    }
}