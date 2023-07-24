using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;
using TPF.Internal;

namespace TPF.Controls
{
    [TemplatePart(Name = "PART_TopLeftTickBar", Type = typeof(TickBar))]
    [TemplatePart(Name = "PART_BottomRightTickBar", Type = typeof(TickBar))]
    [TemplatePart(Name = "PART_TopLeftLabelsControl", Type = typeof(SliderLabelsControl))]
    [TemplatePart(Name = "PART_BottomRightLabelsControl", Type = typeof(SliderLabelsControl))]
    [TemplatePart(Name = "PART_Track", Type = typeof(SliderTrack))]
    public class Slider : RangeBase
    {
        static Slider()
        {
            InitializeCommands();

            MinimumProperty.OverrideMetadata(typeof(Slider), new FrameworkPropertyMetadata(0.0, FrameworkPropertyMetadataOptions.AffectsMeasure));
            MaximumProperty.OverrideMetadata(typeof(Slider), new FrameworkPropertyMetadata(10.0, FrameworkPropertyMetadataOptions.AffectsMeasure));
            ValueProperty.OverrideMetadata(typeof(Slider), new FrameworkPropertyMetadata(0.0, FrameworkPropertyMetadataOptions.AffectsMeasure));

            DefaultStyleKeyProperty.OverrideMetadata(typeof(Slider), new FrameworkPropertyMetadata(typeof(Slider)));
        }

        #region RangeStartChanged RoutedEvent
        public static readonly RoutedEvent RangeStartChangedEvent = EventManager.RegisterRoutedEvent("RangeStartChanged",
            RoutingStrategy.Bubble,
            typeof(RoutedPropertyChangedEventHandler<double?>),
            typeof(Slider));

        public event RoutedPropertyChangedEventHandler<double?> RangeStartChanged
        {
            add => AddHandler(RangeStartChangedEvent, value);
            remove => RemoveHandler(RangeStartChangedEvent, value);
        }
        #endregion

        #region RangeEndChanged RoutedEvent
        public static readonly RoutedEvent RangeEndChangedEvent = EventManager.RegisterRoutedEvent("RangeEndChanged",
            RoutingStrategy.Bubble,
            typeof(RoutedPropertyChangedEventHandler<double?>),
            typeof(Slider));

        public event RoutedPropertyChangedEventHandler<double?> RangeEndChanged
        {
            add => AddHandler(RangeEndChangedEvent, value);
            remove => RemoveHandler(RangeEndChangedEvent, value);
        }
        #endregion

        #region RangeStart DependencyProperty
        public static readonly DependencyProperty RangeStartProperty = DependencyProperty.Register("RangeStart",
            typeof(double),
            typeof(Slider),
            new PropertyMetadata(0d, OnRangeStartPropertyChanged, CoerceRangeStart));

        private static object CoerceRangeStart(DependencyObject sender, object value)
        {
            var instance = (Slider)sender;
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
            var instance = (Slider)sender;

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
            typeof(Slider),
            new PropertyMetadata(0d, OnRangeEndPropertyChanged, CoerceRangeEnd));

        private static object CoerceRangeEnd(DependencyObject sender, object value)
        {
            var instance = (Slider)sender;
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
            var instance = (Slider)sender;

            instance.OnRangeEndChanged((double)e.OldValue, (double)e.NewValue);
        }

        public double RangeEnd
        {
            get { return (double)GetValue(RangeEndProperty); }
            set { SetValue(RangeEndProperty, value); }
        }
        #endregion

        #region MinimumRangeSpan DependencyProperty
        public static readonly DependencyProperty MinimumRangeSpanProperty = DependencyProperty.Register("MinimumRangeSpan",
            typeof(double),
            typeof(Slider),
            new PropertyMetadata(0d, OnMinimumRangeSpanPropertyChanged, CoerceMinimumRangeSpan));

        private static object CoerceMinimumRangeSpan(DependencyObject sender, object value)
        {
            var instance = (Slider)sender;

            var minimumSpan = (double)value;

            if (minimumSpan > instance.Maximum - instance.Minimum)
            {
                minimumSpan = instance.Maximum - instance.Minimum;
            }

            return minimumSpan;
        }

        private static void OnMinimumRangeSpanPropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            var instance = (Slider)sender;

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
            typeof(Slider),
            new PropertyMetadata(double.PositiveInfinity, OnMaximumRangeSpanPropertyChanged, CoerceMaximumRangeSpan));

        private static object CoerceMaximumRangeSpan(DependencyObject sender, object value)
        {
            var instance = (Slider)sender;

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
            var instance = (Slider)sender;

            instance.CoerceValue(RangeStartProperty);
            instance.CoerceValue(RangeEndProperty);
        }

        public double MaximumRangeSpan
        {
            get { return (double)GetValue(MaximumRangeSpanProperty); }
            set { SetValue(MaximumRangeSpanProperty, value); }
        }
        #endregion

        #region TrackBrush DependencyProperty
        public static readonly DependencyProperty TrackBrushProperty = DependencyProperty.Register("TrackBrush",
            typeof(Brush),
            typeof(Slider),
            new PropertyMetadata(null));

        public Brush TrackBrush
        {
            get { return (Brush)GetValue(TrackBrushProperty); }
            set { SetValue(TrackBrushProperty, value); }
        }
        #endregion

        #region ActiveTrackBrush DependencyProperty
        public static readonly DependencyProperty ActiveTrackBrushProperty = DependencyProperty.Register("ActiveTrackBrush",
            typeof(Brush),
            typeof(Slider),
            new PropertyMetadata(null));

        public Brush ActiveTrackBrush
        {
            get { return (Brush)GetValue(ActiveTrackBrushProperty); }
            set { SetValue(ActiveTrackBrushProperty, value); }
        }
        #endregion

        #region TrackCornerRadius DependencyProperty
        public static readonly DependencyProperty TrackCornerRadiusProperty = DependencyProperty.Register("TrackCornerRadius",
            typeof(CornerRadius),
            typeof(Slider),
            new PropertyMetadata(default(CornerRadius)));

        public CornerRadius TrackCornerRadius
        {
            get { return (CornerRadius)GetValue(TrackCornerRadiusProperty); }
            set { SetValue(TrackCornerRadiusProperty, value); }
        }
        #endregion

        #region Orientation DependencyProperty
        public static readonly DependencyProperty OrientationProperty = DependencyProperty.Register("Orientation",
            typeof(Orientation),
            typeof(Slider),
            new PropertyMetadata(Orientation.Horizontal));

        public Orientation Orientation
        {
            get { return (Orientation)GetValue(OrientationProperty); }
            set { SetValue(OrientationProperty, value); }
        }
        #endregion

        #region IsDirectionReversed DependencyProperty
        public static readonly DependencyProperty IsDirectionReversedProperty = DependencyProperty.Register("IsDirectionReversed",
            typeof(bool),
            typeof(Slider),
            new PropertyMetadata(BooleanBoxes.FalseBox));

        public bool IsDirectionReversed
        {
            get { return (bool)GetValue(IsDirectionReversedProperty); }
            set { SetValue(IsDirectionReversedProperty, BooleanBoxes.Box(value)); }
        }
        #endregion

        #region IsMoveToPointEnabled DependencyProperty
        public static readonly DependencyProperty IsMoveToPointEnabledProperty = DependencyProperty.Register("IsMoveToPointEnabled",
            typeof(bool),
            typeof(Slider),
            new PropertyMetadata(BooleanBoxes.FalseBox));

        public bool IsMoveToPointEnabled
        {
            get { return (bool)GetValue(IsMoveToPointEnabledProperty); }
            set { SetValue(IsMoveToPointEnabledProperty, BooleanBoxes.Box(value)); }
        }
        #endregion

        #region HandlesMouseWheel DependencyProperty
        public static readonly DependencyProperty HandlesMouseWheelProperty = DependencyProperty.Register("HandlesMouseWheel",
            typeof(bool),
            typeof(Slider),
            new PropertyMetadata(BooleanBoxes.FalseBox));

        public bool HandlesMouseWheel
        {
            get { return (bool)GetValue(HandlesMouseWheelProperty); }
            set { SetValue(HandlesMouseWheelProperty, BooleanBoxes.Box(value)); }
        }
        #endregion

        #region ThumbMode DependencyProperty
        public static readonly DependencyProperty ThumbModeProperty = DependencyProperty.Register("ThumbMode",
            typeof(SliderThumbMode),
            typeof(Slider),
            new PropertyMetadata(SliderThumbMode.Single, TickPropertyChanged));

        public SliderThumbMode ThumbMode
        {
            get { return (SliderThumbMode)GetValue(ThumbModeProperty); }
            set { SetValue(ThumbModeProperty, value); }
        }
        #endregion

        #region ThumbStyle DependencyProperty
        public static readonly DependencyProperty ThumbStyleProperty = DependencyProperty.Register("ThumbStyle",
            typeof(Style),
            typeof(Slider),
            new PropertyMetadata(null));

        public Style ThumbStyle
        {
            get { return (Style)GetValue(ThumbStyleProperty); }
            set { SetValue(ThumbStyleProperty, value); }
        }
        #endregion

        #region RangeThumbStyle DependencyProperty
        public static readonly DependencyProperty RangeThumbStyleProperty = DependencyProperty.Register("RangeThumbStyle",
            typeof(Style),
            typeof(Slider),
            new PropertyMetadata(null));

        public Style RangeThumbStyle
        {
            get { return (Style)GetValue(RangeThumbStyleProperty); }
            set { SetValue(RangeThumbStyleProperty, value); }
        }
        #endregion

        #region IsSnapToTickEnabled DependencyProperty
        public static readonly DependencyProperty IsSnapToTickEnabledProperty = DependencyProperty.Register("IsSnapToTickEnabled",
            typeof(bool),
            typeof(Slider),
            new PropertyMetadata(BooleanBoxes.FalseBox));

        public bool IsSnapToTickEnabled
        {
            get { return (bool)GetValue(IsSnapToTickEnabledProperty); }
            set { SetValue(IsSnapToTickEnabledProperty, BooleanBoxes.Box(value)); }
        }
        #endregion

        #region Ticks Readonly DependencyProperty
        internal static readonly DependencyPropertyKey TicksPropertyKey = DependencyProperty.RegisterReadOnly("Ticks",
            typeof(List<SliderTick>),
            typeof(Slider),
            new PropertyMetadata(null));

        public static readonly DependencyProperty TicksProperty = TicksPropertyKey.DependencyProperty;

        public List<SliderTick> Ticks
        {
            get { return (List<SliderTick>)GetValue(TicksProperty); }
            private set { SetValue(TicksPropertyKey, value); }
        }
        #endregion

        #region TickPlacement DependencyProperty
        public static readonly DependencyProperty TickPlacementProperty = DependencyProperty.Register("TickPlacement",
            typeof(TickPlacement),
            typeof(Slider),
            new PropertyMetadata(TickPlacement.None));

        public TickPlacement TickPlacement
        {
            get { return (TickPlacement)GetValue(TickPlacementProperty); }
            set { SetValue(TickPlacementProperty, value); }
        }
        #endregion

        #region TickFrequency DependencyProperty
        public static readonly DependencyProperty TickFrequencyProperty = DependencyProperty.Register("TickFrequency",
            typeof(double),
            typeof(Slider),
            new PropertyMetadata(1.0, TickPropertyChanged));

        public double TickFrequency
        {
            get { return (double)GetValue(TickFrequencyProperty); }
            set { SetValue(TickFrequencyProperty, value); }
        }
        #endregion

        #region MinorTickFrequency DependencyProperty
        public static readonly DependencyProperty MinorTickFrequencyProperty = DependencyProperty.Register("MinorTickFrequency",
            typeof(int),
            typeof(Slider),
            new PropertyMetadata(0, TickPropertyChanged));

        public int MinorTickFrequency
        {
            get { return (int)GetValue(MinorTickFrequencyProperty); }
            set { SetValue(MinorTickFrequencyProperty, value); }
        }
        #endregion

        #region TickBrush DependencyProperty
        public static readonly DependencyProperty TickBrushProperty = DependencyProperty.Register("TickBrush",
            typeof(Brush),
            typeof(Slider),
            new PropertyMetadata(null));

        public Brush TickBrush
        {
            get { return (Brush)GetValue(TickBrushProperty); }
            set { SetValue(TickBrushProperty, value); }
        }
        #endregion

        #region MinorTickBrush DependencyProperty
        public static readonly DependencyProperty MinorTickBrushProperty = DependencyProperty.Register("MinorTickBrush",
            typeof(Brush),
            typeof(Slider),
            new PropertyMetadata(null));

        public Brush MinorTickBrush
        {
            get { return (Brush)GetValue(MinorTickBrushProperty); }
            set { SetValue(MinorTickBrushProperty, value); }
        }
        #endregion

        #region ActiveTickBrush DependencyProperty
        public static readonly DependencyProperty ActiveTickBrushProperty = DependencyProperty.Register("ActiveTickBrush",
            typeof(Brush),
            typeof(Slider),
            new PropertyMetadata(null));

        public Brush ActiveTickBrush
        {
            get { return (Brush)GetValue(ActiveTickBrushProperty); }
            set { SetValue(ActiveTickBrushProperty, value); }
        }
        #endregion

        #region ActiveMinorTickBrush DependencyProperty
        public static readonly DependencyProperty ActiveMinorTickBrushProperty = DependencyProperty.Register("ActiveMinorTickBrush",
            typeof(Brush),
            typeof(Slider),
            new PropertyMetadata(null));

        public Brush ActiveMinorTickBrush
        {
            get { return (Brush)GetValue(ActiveMinorTickBrushProperty); }
            set { SetValue(ActiveMinorTickBrushProperty, value); }
        }
        #endregion

        #region LabelPlacement DependencyProperty
        public static readonly DependencyProperty LabelPlacementProperty = DependencyProperty.Register("LabelPlacement",
            typeof(TickPlacement),
            typeof(Slider),
            new PropertyMetadata(TickPlacement.None));

        public TickPlacement LabelPlacement
        {
            get { return (TickPlacement)GetValue(LabelPlacementProperty); }
            set { SetValue(LabelPlacementProperty, value); }
        }
        #endregion

        #region LabelTextSelector DependencyProperty
        public static readonly DependencyProperty LabelTextSelectorProperty = DependencyProperty.Register("LabelTextSelector",
            typeof(SliderLabelTextSelector),
            typeof(Slider),
            new PropertyMetadata(null, TickPropertyChanged));

        public SliderLabelTextSelector LabelTextSelector
        {
            get { return (SliderLabelTextSelector)GetValue(LabelTextSelectorProperty); }
            set { SetValue(LabelTextSelectorProperty, value); }
        }
        #endregion

        #region Delay DependencyProperty
        public static readonly DependencyProperty DelayProperty = RepeatButton.DelayProperty.AddOwner(typeof(Slider),
            new FrameworkPropertyMetadata(Helper.GetKeyboardDelay()));

        public int Delay
        {
            get { return (int)GetValue(DelayProperty); }
            set { SetValue(DelayProperty, value); }
        }
        #endregion

        #region Interval DependencyProperty
        public static readonly DependencyProperty IntervalProperty = RepeatButton.IntervalProperty.AddOwner(typeof(Slider),
            new FrameworkPropertyMetadata(Helper.GetKeyboardSpeed()));

        public int Interval
        {
            get { return (int)GetValue(IntervalProperty); }
            set { SetValue(IntervalProperty, value); }
        }
        #endregion

        private ObservableCollection<SliderThumbBase> _thumbs;
        public ObservableCollection<SliderThumbBase> Thumbs
        {
            get
            {
                if (_thumbs == null) _thumbs = new ObservableCollection<SliderThumbBase>();

                return _thumbs;
            }
        }

        private static void TickPropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            var instance = (Slider)sender;

            instance.UpdateTicks();
        }

        public static RoutedCommand IncreaseSmall { get; private set; }
        public static RoutedCommand DecreaseSmall { get; private set; }
        public static RoutedCommand IncreaseLarge { get; private set; }
        public static RoutedCommand DecreaseLarge { get; private set; }

        private static void InitializeCommands()
        {
            var type = typeof(Slider);

            IncreaseSmall = new RoutedCommand("IncreaseSmall", type);
            DecreaseSmall = new RoutedCommand("DecreaseSmall", type);
            IncreaseLarge = new RoutedCommand("IncreaseLarge", type);
            DecreaseLarge = new RoutedCommand("DecreaseLarge", type);

            CommandManager.RegisterClassCommandBinding(type, new CommandBinding(IncreaseSmall, OnIncreaseSmallCommand));
            CommandManager.RegisterClassCommandBinding(type, new CommandBinding(DecreaseSmall, OnDecreaseSmallCommand));
            CommandManager.RegisterClassCommandBinding(type, new CommandBinding(IncreaseLarge, OnIncreaseLargeCommand));
            CommandManager.RegisterClassCommandBinding(type, new CommandBinding(DecreaseLarge, OnDecreaseLargeCommand));

            CommandManager.RegisterClassInputBinding(type, new InputBinding(IncreaseSmall, new SliderGesture(Key.Up, Key.Down, false)));
            CommandManager.RegisterClassInputBinding(type, new InputBinding(IncreaseSmall, new SliderGesture(Key.Right, Key.Left, true)));
            CommandManager.RegisterClassInputBinding(type, new InputBinding(DecreaseSmall, new SliderGesture(Key.Down, Key.Up, false)));
            CommandManager.RegisterClassInputBinding(type, new InputBinding(DecreaseSmall, new SliderGesture(Key.Left, Key.Right, true)));
        }

        private SliderTrack _track;
        private TickBar _topLeftTickBar;
        private TickBar _bottomRightTickBar;
        private SliderLabelsControl _topLeftLabelsControl;
        private SliderLabelsControl _bottomRightLabelsControl;

        internal bool SuppressSpanCoercion { get; set; }

        private static readonly SliderLabelTextSelector _defaultLabelTextSelector = new SliderLabelTextSelector();

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            _track = (SliderTrack)GetTemplateChild("PART_Track");
            _topLeftTickBar = (TickBar)GetTemplateChild("PART_TopLeftTickBar");
            _bottomRightTickBar = (TickBar)GetTemplateChild("PART_BottomRightTickBar");
            _topLeftLabelsControl = (SliderLabelsControl)GetTemplateChild("PART_TopLeftLabelsControl");
            _bottomRightLabelsControl = (SliderLabelsControl)GetTemplateChild("PART_BottomRightLabelsControl");

            UpdateTicks();
        }

        protected override void OnPreviewMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            if (IsMoveToPointEnabled && _track != null)
            {
                // Bewege Thumb zur Mausposition
                var point = e.MouseDevice.GetPosition(_track);

                var newValue = _track.ValueFromPoint(point);

                switch (ThumbMode)
                {
                    case SliderThumbMode.Single:
                    {
                        if (_track.Thumb != null && !_track.Thumb.IsMouseOver)
                        {
                            if (!double.IsInfinity(newValue))
                            {
                                UpdateValue(newValue);
                            }
                            e.Handled = true;
                        }
                        break;
                    }
                    case SliderThumbMode.Range:
                    {
                        if (_track.RangeThumb != null && !_track.RangeThumb.IsMouseOver)
                        {
                            if (!double.IsInfinity(newValue))
                            {
                                var snappedValue = SnapToTick(newValue);

                                if (newValue > RangeEnd)
                                {
                                    RangeEnd = Math.Max(Minimum, Math.Min(Maximum, snappedValue));
                                }
                                else if (newValue < RangeStart)
                                {
                                    RangeStart = Math.Max(Minimum, Math.Min(Maximum, snappedValue));
                                }
                            }
                            e.Handled = true;
                        }
                        break;
                    }
                }
            }

            base.OnPreviewMouseLeftButtonDown(e);
        }

        protected override void OnMouseWheel(MouseWheelEventArgs e)
        {
            if (HandlesMouseWheel && ThumbMode == SliderThumbMode.Single)
            {
                if (e.Delta > 0) OnIncreaseSmall();
                else if (e.Delta < 0) OnDecreaseSmall();
            }

            base.OnMouseWheel(e);
        }

        protected override void OnMinimumChanged(double oldMinimum, double newMinimum)
        {
            base.OnMinimumChanged(oldMinimum, newMinimum);

            CoerceValue(RangeStartProperty);
            CoerceValue(RangeEndProperty);
            UpdateTicks();
        }

        protected override void OnMaximumChanged(double oldMaximum, double newMaximum)
        {
            base.OnMaximumChanged(oldMaximum, newMaximum);

            CoerceValue(RangeStartProperty);
            CoerceValue(RangeEndProperty);
            UpdateTicks();
        }

        protected override void OnValueChanged(double oldValue, double newValue)
        {
            base.OnValueChanged(oldValue, newValue);

            UpdateTicks();
        }

        protected virtual void OnRangeStartChanged(double oldValue, double newValue)
        {
            var e = new RoutedPropertyChangedEventArgs<double>(oldValue, newValue, RangeStartChangedEvent);

            RaiseEvent(e);

            UpdateTicks();
        }

        protected virtual void OnRangeEndChanged(double oldValue, double newValue)
        {
            var e = new RoutedPropertyChangedEventArgs<double>(oldValue, newValue, RangeEndChangedEvent);

            RaiseEvent(e);
            UpdateTicks();
        }

        private void UpdateValue(double value)
        {
            var snappedValue = SnapToTick(value);

            Value = Math.Max(Minimum, Math.Min(Maximum, snappedValue));
        }

        internal void UpdateMargins(double topLeft, double bottomRight)
        {
            if (Orientation == Orientation.Horizontal)
            {
                var margin = new Thickness(topLeft, 0, bottomRight, 0);

                if (_topLeftTickBar != null) _topLeftTickBar.Margin = margin;
                if (_bottomRightTickBar != null) _bottomRightTickBar.Margin = margin;
                if (_topLeftLabelsControl != null) _topLeftLabelsControl.Margin = margin;
                if (_bottomRightLabelsControl != null) _bottomRightLabelsControl.Margin = margin;
            }
            else
            {
                var tickBarMargin = new Thickness(0, topLeft, 0, bottomRight);
                var leftLabelsMargin = new Thickness(0, topLeft, 2, bottomRight);
                var rightLabelsMargin = new Thickness(2, topLeft, 0, bottomRight);

                if (_topLeftTickBar != null) _topLeftTickBar.Margin = tickBarMargin;
                if (_bottomRightTickBar != null) _bottomRightTickBar.Margin = tickBarMargin;
                if (_topLeftLabelsControl != null) _topLeftLabelsControl.Margin = leftLabelsMargin;
                if (_bottomRightLabelsControl != null) _bottomRightLabelsControl.Margin = rightLabelsMargin;
            }
        }

        internal void ThumbValueChanged()
        {
            UpdateTicks();
        }

        private void UpdateTicks()
        {
            var ticks = new List<SliderTick>();

            var tickFrequency = TickFrequency;
            var minorTickFrequency = MinorTickFrequency;
            var minimum = Minimum;
            var maximum = Maximum;
            var range = Math.Abs(maximum - minimum);

            var firstTick = new SliderTick() { IsMajorTick = true, NormalizedValue = 0, Value = minimum, LabelText = GetTickLabelText(minimum), IsActive = CheckIfTickIsActive(minimum) };
            var lastTick = new SliderTick() { IsMajorTick = true, NormalizedValue = 1, Value = maximum, LabelText = GetTickLabelText(maximum), IsActive = CheckIfTickIsActive(maximum) };

            ticks.Add(firstTick);

            if (range > 0 && tickFrequency > 0)
            {
                var previousMajorTick = firstTick;

                for (var i = tickFrequency; i < range; i += tickFrequency)
                {
                    var value = minimum + i;
                    var normalizedValue = Utility.NormalizeValue(value, minimum, maximum);

                    var tick = new SliderTick() { IsMajorTick = true, NormalizedValue = normalizedValue, Value = value, LabelText = GetTickLabelText(value), IsActive = CheckIfTickIsActive(value) };

                    GenerateMinorTicks(ticks, previousMajorTick, tick, minorTickFrequency, minimum, maximum);

                    ticks.Add(tick);

                    previousMajorTick = tick;
                }

                GenerateMinorTicks(ticks, previousMajorTick, lastTick, minorTickFrequency, minimum, maximum);
            }

            ticks.Add(lastTick);

            Ticks = ticks;
        }

        private void GenerateMinorTicks(List<SliderTick> ticks, SliderTick previousTick, SliderTick tick, int frequency, double minimum, double maximum)
        {
            if (frequency <= 0) return;

            var minorTickValueIncrease = Math.Abs(tick.Value - previousTick.Value) / (frequency + 1);

            for (int j = 1; j <= frequency; j++)
            {
                var minorValue = previousTick.Value + (minorTickValueIncrease * j);
                var normalizedMinorValue = Utility.NormalizeValue(minorValue, minimum, maximum);

                var minorTick = new SliderTick() { NormalizedValue = normalizedMinorValue, Value = minorValue, IsActive = CheckIfTickIsActive(minorValue) };

                ticks.Add(minorTick);
            }
        }

        private bool CheckIfTickIsActive(double value)
        {
            switch (ThumbMode)
            {
                case SliderThumbMode.Single:
                {
                    return value <= Value;
                }
                case SliderThumbMode.Range:
                {
                    return value >= RangeStart && value <= RangeEnd;
                }
                case SliderThumbMode.Custom:
                {
                    for (int i = 0; i < Thumbs.Count; i++)
                    {
                        var thumb = Thumbs[i];

                        if (thumb is SliderThumb sliderThumb)
                        {
                            if (value == sliderThumb.Value) return true;
                        }
                        else if (thumb is RangeSliderThumb rangeThumb)
                        {
                            if (value >= rangeThumb.RangeStart && value <= rangeThumb.RangeEnd) return true;
                        }
                    }
                    break;
                }
            }

            return false;
        }

        private string GetTickLabelText(double value)
        {
            var selector = LabelTextSelector ?? _defaultLabelTextSelector;

            return selector.SelectLabelText(this, value);
        }

        internal double SnapToTick(double value)
        {
            if (IsSnapToTickEnabled)
            {
                var previous = Minimum;
                var next = Maximum;

                var ticks = Ticks;

                if (ticks != null && ticks.Count > 0)
                {
                    for (int i = 0; i < ticks.Count; i++)
                    {
                        var tick = ticks[i];

                        if (value.IsCloseOrEqual(tick.Value)) return value;
                        else if (tick.Value < value && tick.Value > previous) previous = tick.Value;
                        else if (tick.Value > value && tick.Value < next) next = tick.Value;
                    }
                }

                value = (value > (previous + next) * 0.5) ? next : previous;
            }

            return value;
        }

        private void MoveToNextTick(double change)
        {
            if (change == 0.0) return;

            double value;

            switch (ThumbMode)
            {
                case SliderThumbMode.Single:
                {
                    value = Value;
                    break;
                }
                case SliderThumbMode.Range:
                {
                    if (change > 0) value = RangeEnd;
                    else value = RangeStart;
                    break;
                }
                default: return;
            }

            // Nächsten Wert über SnapToTick finden
            var next = SnapToTick(Math.Max(Minimum, Math.Min(Maximum, value + change)));

            // Muss die Suche zu einem größeren Wert als value führen oder kleiner
            var greaterThan = change > 0;

            // Wenn wir durch das snapping wieder bei value landen, den nächsten Punkt raussuchen
            if (next == value && !(greaterThan && value == Maximum) && !(!greaterThan && value == Minimum))
            {
                if (TickFrequency > 0.0)
                {
                    // Ausrechnen an welchem Tick wir sind
                    var tickNumber = Math.Round((value - Minimum) / TickFrequency, 2);

                    if (greaterThan) tickNumber += 1.0 / Math.Max(1, MinorTickFrequency + 1);
                    else tickNumber -= 1.0 / Math.Max(1, MinorTickFrequency + 1);

                    next = Minimum + (tickNumber * TickFrequency);
                }
            }

            // Update wenn sich der Punkt geändert hat
            if (next != value)
            {
                switch (ThumbMode)
                {
                    case SliderThumbMode.Single:
                    {
                        Value = next;
                        break;
                    }
                    case SliderThumbMode.Range:
                    {
                        if (change > 0) RangeEnd = next;
                        else RangeStart = next;
                        break;
                    }
                }
            }
        }

        private static void OnIncreaseSmallCommand(object sender, ExecutedRoutedEventArgs e)
        {
            if (sender is Slider slider) slider.OnIncreaseSmall();
        }

        private static void OnDecreaseSmallCommand(object sender, ExecutedRoutedEventArgs e)
        {
            if (sender is Slider slider) slider.OnDecreaseSmall();
        }

        private static void OnIncreaseLargeCommand(object sender, ExecutedRoutedEventArgs e)
        {
            if (sender is Slider slider) slider.OnIncreaseLarge();
        }

        private static void OnDecreaseLargeCommand(object sender, ExecutedRoutedEventArgs e)
        {
            if (sender is Slider slider) slider.OnDecreaseLarge();
        }

        protected virtual void OnIncreaseSmall()
        {
            MoveToNextTick(SmallChange);
        }

        protected virtual void OnDecreaseSmall()
        {
            MoveToNextTick(-SmallChange);
        }

        protected virtual void OnIncreaseLarge()
        {
            MoveToNextTick(LargeChange);
        }

        protected virtual void OnDecreaseLarge()
        {
            MoveToNextTick(-LargeChange);
        }

        private class SliderGesture : InputGesture
        {
            public SliderGesture(Key normal, Key inverted, bool isHorizontal)
            {
                _normal = normal;
                _inverted = inverted;
                _isHorizontal = isHorizontal;
            }

            private readonly Key _normal;
            private readonly Key _inverted;
            private readonly bool _isHorizontal;

            public override bool Matches(object targetElement, InputEventArgs inputEventArgs)
            {
                if (inputEventArgs is KeyEventArgs keyEventArgs && targetElement is Slider slider && Keyboard.Modifiers == ModifierKeys.None)
                {
                    if ((int)_normal == (int)keyEventArgs.Key) return !IsInverted(slider);
                    if ((int)_inverted == (int)keyEventArgs.Key) return IsInverted(slider);
                }
                return false;
            }

            private bool IsInverted(Slider slider)
            {
                if (_isHorizontal) return slider.IsDirectionReversed != (slider.FlowDirection == FlowDirection.RightToLeft);
                else return slider.IsDirectionReversed;
            }
        }
    }
}