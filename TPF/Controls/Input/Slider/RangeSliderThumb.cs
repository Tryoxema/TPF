using System;
using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using TPF.Internal;

namespace TPF.Controls
{
    public class RangeSliderThumb : SliderThumbBase
    {
        static RangeSliderThumb()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(RangeSliderThumb), new FrameworkPropertyMetadata(typeof(RangeSliderThumb)));
        }

        #region RangeStart DependencyProperty
        public static readonly DependencyProperty RangeStartProperty = DependencyProperty.Register("RangeStart",
            typeof(double),
            typeof(RangeSliderThumb),
            new PropertyMetadata(0d, OnRangeStartPropertyChanged, CoerceRangeStart));

        private static object CoerceRangeStart(DependencyObject sender, object value)
        {
            var instance = (RangeSliderThumb)sender;
            var rangeStart = (double)value;

            var minimum = instance.ParentSlider?.Minimum ?? double.MinValue;
            var maximum = instance.ParentSlider?.Maximum ?? double.MaxValue;

            if (!instance.SuppressSpanCoercion && instance.RangeEnd - rangeStart < instance.MinimumRangeSpan)
            {
                rangeStart = Math.Max(instance.RangeEnd - instance.MinimumRangeSpan, minimum);
            }
            else if (rangeStart + instance.MinimumRangeSpan > maximum)
            {
                rangeStart = Math.Max(minimum, maximum - instance.MinimumRangeSpan);
            }
            else if (!instance.SuppressSpanCoercion && instance.RangeEnd - rangeStart > instance.MaximumRangeSpan)
            {
                rangeStart = Math.Max(instance.RangeEnd - instance.MaximumRangeSpan, minimum);
            }
            else if (rangeStart <= minimum)
            {
                rangeStart = minimum;
            }
            else if (rangeStart >= maximum)
            {
                rangeStart = maximum;
            }

            return rangeStart;
        }

        private static void OnRangeStartPropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            var instance = (RangeSliderThumb)sender;

            instance.OnThumbValueChanged();
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
            typeof(RangeSliderThumb),
            new PropertyMetadata(0d, OnRangeEndPropertyChanged, CoerceRangeEnd));

        private static object CoerceRangeEnd(DependencyObject sender, object value)
        {
            var instance = (RangeSliderThumb)sender;
            var rangeEnd = (double)value;

            var maximum = instance.ParentSlider?.Maximum ?? double.MaxValue;

            if (instance.RangeStart + instance.MinimumRangeSpan > rangeEnd)
            {
                rangeEnd = instance.RangeStart + instance.MinimumRangeSpan;
            }
            else if (!instance.SuppressSpanCoercion && rangeEnd - instance.RangeStart > instance.MaximumRangeSpan)
            {
                rangeEnd = Math.Min(instance.RangeStart + instance.MaximumRangeSpan, maximum);
            }
            else if (rangeEnd >= maximum)
            {
                rangeEnd = maximum;
            }

            return rangeEnd;
        }

        private static void OnRangeEndPropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            var instance = (RangeSliderThumb)sender;

            instance.OnThumbValueChanged();
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
            typeof(RangeSliderThumb),
            new PropertyMetadata(0d, OnMinimumRangeSpanPropertyChanged, CoerceMinimumRangeSpan));

        private static object CoerceMinimumRangeSpan(DependencyObject sender, object value)
        {
            var instance = (RangeSliderThumb)sender;

            var minimumSpan = (double)value;

            var minimum = instance.ParentSlider?.Minimum ?? double.MinValue;
            var maximum = instance.ParentSlider?.Maximum ?? double.MaxValue;

            if (minimumSpan > maximum - minimum)
            {
                minimumSpan = maximum - minimum;
            }

            return minimumSpan;
        }

        private static void OnMinimumRangeSpanPropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            var instance = (RangeSliderThumb)sender;

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
            typeof(RangeSliderThumb),
            new PropertyMetadata(double.PositiveInfinity, OnMaximumRangeSpanPropertyChanged, CoerceMaximumRangeSpan));

        private static object CoerceMaximumRangeSpan(DependencyObject sender, object value)
        {
            var instance = (RangeSliderThumb)sender;

            var maximumSpan = (double)value;

            var minimum = instance.ParentSlider?.Minimum ?? double.MinValue;
            var maximum = instance.ParentSlider?.Maximum ?? double.MaxValue;

            if (maximumSpan < instance.MinimumRangeSpan)
            {
                maximumSpan = instance.MinimumRangeSpan;
            }
            else if (maximumSpan > maximum - minimum)
            {
                maximumSpan = maximum - minimum;
            }

            return maximumSpan;
        }

        private static void OnMaximumRangeSpanPropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            var instance = (RangeSliderThumb)sender;

            instance.CoerceValue(RangeStartProperty);
            instance.CoerceValue(RangeEndProperty);
        }

        public double MaximumRangeSpan
        {
            get { return (double)GetValue(MaximumRangeSpanProperty); }
            set { SetValue(MaximumRangeSpanProperty, value); }
        }
        #endregion

        #region StartThumbStyle DependencyProperty
        public static readonly DependencyProperty StartThumbStyleProperty = DependencyProperty.Register("StartThumbStyle",
            typeof(Style),
            typeof(RangeSliderThumb),
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
            typeof(RangeSliderThumb),
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
            typeof(RangeSliderThumb),
            new PropertyMetadata(null));

        public Style EndThumbStyle
        {
            get { return (Style)GetValue(EndThumbStyleProperty); }
            set { SetValue(EndThumbStyleProperty, value); }
        }
        #endregion

        #region IsDirectionReversed DependencyProperty
        public static readonly DependencyProperty IsDirectionReversedProperty = Slider.IsDirectionReversedProperty.AddOwner(typeof(RangeSliderThumb),
            new PropertyMetadata(BooleanBoxes.FalseBox));

        public bool IsDirectionReversed
        {
            get { return (bool)GetValue(IsDirectionReversedProperty); }
            set { SetValue(IsDirectionReversedProperty, BooleanBoxes.Box(value)); }
        }
        #endregion

        internal SliderThumb StartThumb { get; private set; }
        internal Thumb MiddleThumb { get; private set; }
        internal SliderThumb EndThumb { get; private set; }

        internal bool SuppressSpanCoercion { get; set; }

        public override void OnApplyTemplate()
        {
            BindingOperations.ClearBinding(this, IsDirectionReversedProperty);

            base.OnApplyTemplate();

            if (MiddleThumb != null)
            {
                MiddleThumb.DragDelta -= MiddleThumb_DragDelta;
            }

            StartThumb = GetTemplateChild("PART_StartThumb") as SliderThumb;
            MiddleThumb = GetTemplateChild("PART_MiddleThumb") as Thumb;
            EndThumb = GetTemplateChild("PART_EndThumb") as SliderThumb;

            if (ParentSlider != null)
            {
                SetBinding(IsDirectionReversedProperty, new Binding("IsDirectionReversed") { Source = ParentSlider });
            }

            if (MiddleThumb != null)
            {
                MiddleThumb.DragDelta += MiddleThumb_DragDelta;
            }
        }

        private void MiddleThumb_DragDelta(object sender, DragDeltaEventArgs e)
        {
            if (ParentSlider == null) return;

            var valueDelta = 0d;
            var rangeStart = RangeStart;
            var rangeEnd = RangeEnd;
            var minimum = ParentSlider.Minimum;
            var maximum = ParentSlider.Maximum;

            if (ParentTrack != null)
            {
                valueDelta += ParentTrack.ValueFromDistance(e.HorizontalChange, e.VerticalChange);

                if (rangeStart + valueDelta < minimum) valueDelta = minimum - rangeStart;
                else if (rangeEnd + valueDelta > maximum) valueDelta = maximum - rangeEnd;
            }

            rangeStart += valueDelta;
            rangeEnd += valueDelta;

            rangeStart = ParentSlider.SnapToTick(rangeStart);
            rangeEnd = ParentSlider.SnapToTick(rangeEnd);

            SuppressSpanCoercion = true;
            ParentSlider.SuppressSpanCoercion = true;
            RangeStart = rangeStart;
            RangeEnd = rangeEnd;
            SuppressSpanCoercion = false;
            ParentSlider.SuppressSpanCoercion = false;
        }
    }
}