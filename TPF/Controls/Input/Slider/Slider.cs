using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using TPF.Internal;

namespace TPF.Controls
{
    [TemplatePart(Name = "PART_DecreaseButton", Type = typeof(RepeatButton))]
    [TemplatePart(Name = "PART_Thumb", Type = typeof(Thumb))]
    [TemplatePart(Name = "PART_IncreaseButton", Type = typeof(RepeatButton))]
    public class Slider : RangeBase
    {
        static Slider()
        {
            InitializeCommands();

            MinimumProperty.OverrideMetadata(typeof(Slider), new FrameworkPropertyMetadata(0.0, FrameworkPropertyMetadataOptions.AffectsMeasure));
            MaximumProperty.OverrideMetadata(typeof(Slider), new FrameworkPropertyMetadata(10.0, FrameworkPropertyMetadataOptions.AffectsMeasure));
            ValueProperty.OverrideMetadata(typeof(Slider), new FrameworkPropertyMetadata(0.0, FrameworkPropertyMetadataOptions.AffectsMeasure));

            DefaultStyleKeyProperty.OverrideMetadata(typeof(Slider), new FrameworkPropertyMetadata(typeof(Slider)));

            EventManager.RegisterClassHandler(typeof(Slider), Thumb.DragDeltaEvent, new DragDeltaEventHandler(OnThumbDragDelta));
        }

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
            new PropertyMetadata(1.0));

        public double TickFrequency
        {
            get { return (double)GetValue(TickFrequencyProperty); }
            set { SetValue(TickFrequencyProperty, value); }
        }
        #endregion

        #region UseNumbersAsTicks DependencyProperty
        public static readonly DependencyProperty UseNumbersAsTicksProperty = DependencyProperty.Register("UseNumbersAsTicks",
            typeof(bool),
            typeof(Slider),
            new PropertyMetadata(BooleanBoxes.FalseBox));

        public bool UseNumbersAsTicks
        {
            get { return (bool)GetValue(UseNumbersAsTicksProperty); }
            set { SetValue(UseNumbersAsTicksProperty, BooleanBoxes.Box(value)); }
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

        internal SliderTrack Track;

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            Track = (SliderTrack)GetTemplateChild("PART_Track");
        }

        protected override void OnPreviewMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            if (IsMoveToPointEnabled && Track != null && Track.Thumb != null && !Track.Thumb.IsMouseOver)
            {
                // Bewege Thumb zur Mausposition
                var point = e.MouseDevice.GetPosition(Track);

                var newValue = Track.ValueFromPoint(point);

                if (!double.IsInfinity(newValue))
                {
                    UpdateValue(newValue);
                }
                e.Handled = true;
            }

            base.OnPreviewMouseLeftButtonDown(e);
        }

        private void UpdateValue(double value)
        {
            var snappedValue = SnapToTick(value);

            Value = Math.Max(Minimum, Math.Min(Maximum, snappedValue));
        }

        private double SnapToTick(double value)
        {
            if (IsSnapToTickEnabled)
            {
                var previous = Minimum;
                var next = Maximum;

                if (TickFrequency > 0.0)
                {
                    previous = Minimum + (Math.Round((value - Minimum) / TickFrequency) * TickFrequency);
                    next = Math.Min(Maximum, previous + TickFrequency);
                }

                value = (value > (previous + next) * 0.5) ? next : previous;
            }

            return value;
        }

        private void MoveToNextTick(double change)
        {
            if (change == 0.0) return;

            var value = Value;

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
                    var tickNumber = Math.Round((value - Minimum) / TickFrequency);

                    if (greaterThan) tickNumber += 1.0;
                    else tickNumber -= 1.0;

                    next = Minimum + (tickNumber * TickFrequency);
                }
            }

            // Update wenn sich der Punkt geändert hat
            if (next != value) Value = next;
        }

        private static void OnThumbDragDelta(object sender, DragDeltaEventArgs e)
        {
            if (sender is Slider slider) slider.OnThumbDragDelta(e);
        }

        protected virtual void OnThumbDragDelta(DragDeltaEventArgs e)
        {
            var thumb = e.OriginalSource as Thumb;

            if (Track != null && thumb == Track.Thumb)
            {
                var newValue = Value + Track.ValueFromDistance(e.HorizontalChange, e.VerticalChange);

                if (!double.IsInfinity(newValue) && !double.IsNaN(newValue))
                {
                    UpdateValue(newValue);
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