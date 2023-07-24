using System.Windows;
using System.Windows.Controls.Primitives;

namespace TPF.Controls
{
    public class SliderThumb : SliderThumbBase
    {
        static SliderThumb()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(SliderThumb), new FrameworkPropertyMetadata(typeof(SliderThumb)));
        }

        public SliderThumb()
        {
            DragDelta += SliderThumb_DragDelta;
        }

        #region Value DependencyProperty
        public static readonly DependencyProperty ValueProperty = DependencyProperty.Register("Value",
            typeof(double),
            typeof(SliderThumb),
            new PropertyMetadata(0d, OnValueChanged, CoerceValue));

        private static object CoerceValue(DependencyObject sender, object value)
        {
            var instance = (SliderThumb)sender;

            var doubleValue = (double)value;

            var minimum = instance.ParentSlider?.Minimum ?? double.MinValue;
            var maximum = instance.ParentSlider?.Maximum ?? double.MaxValue;

            if (doubleValue < minimum) doubleValue = minimum;
            else if (doubleValue > maximum) doubleValue = maximum;

            return doubleValue;
        }

        private static void OnValueChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            var instance = (SliderThumb)sender;

            instance.OnThumbValueChanged();
        }

        public double Value
        {
            get { return (double)GetValue(ValueProperty); }
            set { SetValue(ValueProperty, value); }
        }
        #endregion

        private void SliderThumb_DragDelta(object sender, DragDeltaEventArgs e)
        {
            if (ParentSlider == null) return;

            var newValue = Value;

            if (ParentTrack != null)
            {
                newValue += ParentTrack.ValueFromDistance(e.HorizontalChange, e.VerticalChange);
            }

            newValue = ParentSlider.SnapToTick(newValue);

            Value = newValue;
        }
    }
}