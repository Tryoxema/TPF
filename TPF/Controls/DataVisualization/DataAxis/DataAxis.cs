using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using TPF.Controls.Specialized.DataAxis;
using TPF.Internal;

namespace TPF.Controls
{
    public class DataAxis : Control
    {
        static DataAxis()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(DataAxis), new FrameworkPropertyMetadata(typeof(DataAxis)));
        }

        #region Orientation DependencyProperty
        public static readonly DependencyProperty OrientationProperty = DependencyProperty.Register("Orientation",
            typeof(Orientation),
            typeof(DataAxis),
            new PropertyMetadata(Orientation.Horizontal));

        public Orientation Orientation
        {
            get { return (Orientation)GetValue(OrientationProperty); }
            set { SetValue(OrientationProperty, value); }
        }
        #endregion

        #region OriginValue DependencyProperty
        public static readonly DependencyProperty OriginValueProperty = DependencyProperty.Register("OriginValue",
            typeof(double),
            typeof(DataAxis),
            new PropertyMetadata(0.0, TickLayoutPropertyChanged));

        public double OriginValue
        {
            get { return (double)GetValue(OriginValueProperty); }
            set { SetValue(OriginValueProperty, value); }
        }
        #endregion

        #region Minimum DependencyProperty
        public static readonly DependencyProperty MinimumProperty = DependencyProperty.Register("Minimum",
            typeof(double),
            typeof(DataAxis),
            new PropertyMetadata(0.0, TickLayoutPropertyChanged));

        public double Minimum
        {
            get { return (double)GetValue(MinimumProperty); }
            set { SetValue(MinimumProperty, value); }
        }
        #endregion

        #region Maximum DependencyProperty
        public static readonly DependencyProperty MaximumProperty = DependencyProperty.Register("Maximum",
            typeof(double),
            typeof(DataAxis),
            new PropertyMetadata(100.0, TickLayoutPropertyChanged));

        public double Maximum
        {
            get { return (double)GetValue(MaximumProperty); }
            set { SetValue(MaximumProperty, value); }
        }
        #endregion

        #region MajorTickLength DependencyProperty
        public static readonly DependencyProperty MajorTickLengthProperty = DependencyProperty.Register("MajorTickLength",
            typeof(double),
            typeof(DataAxis),
            new PropertyMetadata(6.0));

        public double MajorTickLength
        {
            get { return (double)GetValue(MajorTickLengthProperty); }
            set { SetValue(MajorTickLengthProperty, value); }
        }
        #endregion

        #region MinorTickLength DependencyProperty
        public static readonly DependencyProperty MinorTickLengthProperty = DependencyProperty.Register("MinorTickLength",
            typeof(double),
            typeof(DataAxis),
            new PropertyMetadata(4.0));

        public double MinorTickLength
        {
            get { return (double)GetValue(MinorTickLengthProperty); }
            set { SetValue(MinorTickLengthProperty, value); }
        }
        #endregion

        #region TickInterval DependencyProperty
        public static readonly DependencyProperty TickIntervalProperty = DependencyProperty.Register("TickInterval",
            typeof(double),
            typeof(DataAxis),
            new PropertyMetadata(0.0, TickLayoutPropertyChanged));

        public double TickInterval
        {
            get { return (double)GetValue(TickIntervalProperty); }
            set { SetValue(TickIntervalProperty, value); }
        }
        #endregion

        #region MajorTickFrequency DependencyProperty
        public static readonly DependencyProperty MajorTickFrequencyProperty = DependencyProperty.Register("MajorTickFrequency",
            typeof(int),
            typeof(DataAxis),
            new PropertyMetadata(2, TickLayoutPropertyChanged));

        public int MajorTickFrequency
        {
            get { return (int)GetValue(MajorTickFrequencyProperty); }
            set { SetValue(MajorTickFrequencyProperty, value); }
        }
        #endregion

        #region LabelMode DependencyProperty
        public static readonly DependencyProperty LabelModeProperty = DependencyProperty.Register("LabelMode",
            typeof(DataAxisLabelMode),
            typeof(DataAxis),
            new PropertyMetadata(DataAxisLabelMode.MajorTick, TickLayoutPropertyChanged));

        public DataAxisLabelMode LabelMode
        {
            get { return (DataAxisLabelMode)GetValue(LabelModeProperty); }
            set { SetValue(LabelModeProperty, value); }
        }
        #endregion

        #region TickBrush DependencyProperty
        public static readonly DependencyProperty TickBrushProperty = DependencyProperty.Register("TickBrush",
            typeof(Brush),
            typeof(DataAxis),
            new PropertyMetadata(null));

        public Brush TickBrush
        {
            get { return (Brush)GetValue(TickBrushProperty); }
            set { SetValue(TickBrushProperty, value); }
        }
        #endregion

        #region Ticks Readonly DependencyProperty
        internal static readonly DependencyPropertyKey TicksPropertyKey = DependencyProperty.RegisterReadOnly("Ticks",
            typeof(List<DataAxisTick>),
            typeof(DataAxis),
            new PropertyMetadata(null));

        public static readonly DependencyProperty TicksProperty = TicksPropertyKey.DependencyProperty;

        public List<DataAxisTick> Ticks
        {
            get { return (List<DataAxisTick>)GetValue(TicksProperty); }
            private set { SetValue(TicksPropertyKey, value); }
        }
        #endregion

        private static void TickLayoutPropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            var instance = (DataAxis)sender;

            instance.UpdateTicks();
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            UpdateTicks();
        }

        private void UpdateTicks()
        {
            var ticks = new List<DataAxisTick>();

            var minimum = Minimum;
            var maximum = Maximum;
            var range = Math.Abs(maximum - minimum);
            var labelMode = LabelMode;

            ticks.Add(new DataAxisTick() { IsMajorTick = labelMode != DataAxisLabelMode.None, NormalizedValue = 0, Value = minimum });
            ticks.Add(new DataAxisTick() { IsMajorTick = labelMode != DataAxisLabelMode.None, NormalizedValue = 1, Value = maximum });

            if (labelMode == DataAxisLabelMode.MajorTick || labelMode == DataAxisLabelMode.StartEndOrigin)
            {
                ticks.Add(new DataAxisTick() { IsMajorTick = true, NormalizedValue = Utility.NormalizeValue(OriginValue, minimum, maximum), Value = OriginValue });
            }

            if (range > 0)
            {
                var tickInterval = TickInterval;
                var majorTickFrequency = labelMode == DataAxisLabelMode.MajorTick ? MajorTickFrequency : 0;

                if (!Utility.IsANumber(tickInterval) || tickInterval <= 0) tickInterval = range / 10;
                else if (tickInterval > range) tickInterval = range;

                var generatedTickCount = 0;

                for (var i = tickInterval; i < range; i += tickInterval)
                {
                    generatedTickCount++;

                    var value = minimum + i;
                    var normalizedValue = Utility.NormalizeValue(value, minimum, maximum);
                    var isMajorTick = majorTickFrequency > 0 && generatedTickCount % majorTickFrequency == 0;

                    ticks.Add(new DataAxisTick() { IsMajorTick = isMajorTick, NormalizedValue = normalizedValue, Value = value });
                }
            }

            Ticks = ticks;
        }
    }
}