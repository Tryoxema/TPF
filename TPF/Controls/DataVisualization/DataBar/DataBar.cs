using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using TPF.Controls.Specialized.DataBar;
using TPF.Internal;

namespace TPF.Controls
{
    public class DataBar : DataBarBase
    {
        static DataBar()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(DataBar), new FrameworkPropertyMetadata(typeof(DataBar)));
        }

        #region OriginValue DependencyProperty
        public static readonly DependencyProperty OriginValueProperty = DependencyProperty.Register("OriginValue",
            typeof(double),
            typeof(DataBar),
            new PropertyMetadata(0.0, OriginValuePropertyChanged));

        private static void OriginValuePropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            var instance = (DataBar)sender;

            instance.UpdateBarSize();
            instance.UpdateValueBrush();
            instance.UpdateBarBorderBrush();
            instance.UpdateLabelMargin();
            instance.UpdateOriginAxisMargin();
        }

        public double OriginValue
        {
            get { return (double)GetValue(OriginValueProperty); }
            set { SetValue(OriginValueProperty, value); }
        }
        #endregion

        #region Value DependencyProperty
        public static readonly DependencyProperty ValueProperty = DependencyProperty.Register("Value",
            typeof(double),
            typeof(DataBar),
            new PropertyMetadata(0.0, ValuePropertyChanged));

        private static void ValuePropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            var instance = (DataBar)sender;

            instance.UpdateBarSize();
            instance.UpdateValueBrush();
            instance.UpdateBarBorderBrush();
            instance.UpdateLabelText();
            instance.UpdateLabelMargin();
            instance.UpdateOriginAxisMargin();
            instance.UpdateOutOfRangeTemplates();
        }

        public double Value
        {
            get { return (double)GetValue(ValueProperty); }
            set { SetValue(ValueProperty, value); }
        }
        #endregion

        #region PositiveValueBrush DependencyProperty
        public static readonly DependencyProperty PositiveValueBrushProperty = DependencyProperty.Register("PositiveValueBrush",
            typeof(Brush),
            typeof(DataBar),
            new PropertyMetadata(null, ValueBrushPropertyChanged));

        public Brush PositiveValueBrush
        {
            get { return (Brush)GetValue(PositiveValueBrushProperty); }
            set { SetValue(PositiveValueBrushProperty, value); }
        }
        #endregion

        #region NegativeValueBrush DependencyProperty
        public static readonly DependencyProperty NegativeValueBrushProperty = DependencyProperty.Register("NegativeValueBrush",
            typeof(Brush),
            typeof(DataBar),
            new PropertyMetadata(null, ValueBrushPropertyChanged));

        public Brush NegativeValueBrush
        {
            get { return (Brush)GetValue(NegativeValueBrushProperty); }
            set { SetValue(NegativeValueBrushProperty, value); }
        }
        #endregion

        #region ActualValueBrush Readonly DependencyProperty
        internal static readonly DependencyPropertyKey ActualValueBrushPropertyKey = DependencyProperty.RegisterReadOnly("ActualValueBrush",
            typeof(Brush),
            typeof(DataBar),
            new PropertyMetadata(null));

        public static readonly DependencyProperty ActualValueBrushProperty = ActualValueBrushPropertyKey.DependencyProperty;

        public Brush ActualValueBrush
        {
            get { return (Brush)GetValue(ActualValueBrushProperty); }
            private set { SetValue(ActualValueBrushPropertyKey, value); }
        }
        #endregion

        #region PositiveValueBarBorderBrush DependencyProperty
        public static readonly DependencyProperty PositiveValueBarBorderBrushProperty = DependencyProperty.Register("PositiveValueBarBorderBrush",
            typeof(Brush),
            typeof(DataBar),
            new PropertyMetadata(null, BarBorderBrushPropertyChanged));

        public Brush PositiveValueBarBorderBrush
        {
            get { return (Brush)GetValue(PositiveValueBarBorderBrushProperty); }
            set { SetValue(PositiveValueBarBorderBrushProperty, value); }
        }
        #endregion

        #region NegativeValueBarBorderBrush DependencyProperty
        public static readonly DependencyProperty NegativeValueBarBorderBrushProperty = DependencyProperty.Register("NegativeValueBarBorderBrush",
            typeof(Brush),
            typeof(DataBar),
            new PropertyMetadata(null, BarBorderBrushPropertyChanged));

        public Brush NegativeValueBarBorderBrush
        {
            get { return (Brush)GetValue(NegativeValueBarBorderBrushProperty); }
            set { SetValue(NegativeValueBarBorderBrushProperty, value); }
        }
        #endregion

        #region ActualBarBorderBrush Readonly DependencyProperty
        internal static readonly DependencyPropertyKey ActualBarBorderBrushPropertyKey = DependencyProperty.RegisterReadOnly("ActualBarBorderBrush",
            typeof(Brush),
            typeof(DataBar),
            new PropertyMetadata(null));

        public static readonly DependencyProperty ActualBarBorderBrushProperty = ActualBarBorderBrushPropertyKey.DependencyProperty;

        public Brush ActualBarBorderBrush
        {
            get { return (Brush)GetValue(ActualBarBorderBrushProperty); }
            private set { SetValue(ActualBarBorderBrushPropertyKey, value); }
        }
        #endregion

        #region LabelPosition DependencyProperty
        public static readonly DependencyProperty LabelPositionProperty = DependencyProperty.Register("LabelPosition",
            typeof(LabelPosition),
            typeof(DataBar),
            new PropertyMetadata(LabelPosition.EndOfBarOutside, LabelPositionPropertyChanged));

        private static void LabelPositionPropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            var instance = (DataBar)sender;

            instance.UpdateLabelMargin();
        }

        public LabelPosition LabelPosition
        {
            get { return (LabelPosition)GetValue(LabelPositionProperty); }
            set { SetValue(LabelPositionProperty, value); }
        }
        #endregion

        #region LabelOffset DependencyProperty
        public static readonly DependencyProperty LabelOffsetProperty = DependencyProperty.Register("LabelOffset",
            typeof(double),
            typeof(DataBar),
            new PropertyMetadata(4.0, LabelOffsetPropertyChanged));

        private static void LabelOffsetPropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            var instance = (DataBar)sender;

            instance.UpdateLabelMargin();
        }

        public double LabelOffset
        {
            get { return (double)GetValue(LabelOffsetProperty); }
            set { SetValue(LabelOffsetProperty, value); }
        }
        #endregion

        #region LabelVisibility DependencyProperty
        public static readonly DependencyProperty LabelVisibilityProperty = DependencyProperty.Register("LabelVisibility",
            typeof(Visibility),
            typeof(DataBar),
            new PropertyMetadata(VisibilityBoxes.VisibleBox));

        public Visibility LabelVisibility
        {
            get { return (Visibility)GetValue(LabelVisibilityProperty); }
            set { SetValue(LabelVisibilityProperty, VisibilityBoxes.Box(value)); }
        }
        #endregion

        #region LabelStyle DependencyProperty
        public static readonly DependencyProperty LabelStyleProperty = DependencyProperty.Register("LabelStyle",
            typeof(Style),
            typeof(DataBar),
            new PropertyMetadata(null));

        public Style LabelStyle
        {
            get { return (Style)GetValue(LabelStyleProperty); }
            set { SetValue(LabelStyleProperty, value); }
        }
        #endregion

        #region LabelFormatString DependencyProperty
        public static readonly DependencyProperty LabelFormatStringProperty = DependencyProperty.Register("LabelFormatString",
            typeof(string),
            typeof(DataBar),
            new PropertyMetadata(null, LabelFormatStringPropertyChanged));

        private static void LabelFormatStringPropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            var instance = (DataBar)sender;

            instance.UpdateLabelText();
            instance.UpdateLabelMargin();
        }

        public string LabelFormatString
        {
            get { return (string)GetValue(LabelFormatStringProperty); }
            set { SetValue(LabelFormatStringProperty, value); }
        }
        #endregion

        #region LabelText Readonly DependencyProperty
        internal static readonly DependencyPropertyKey LabelTextPropertyKey = DependencyProperty.RegisterReadOnly("LabelText",
            typeof(string),
            typeof(DataBar),
            new PropertyMetadata(null));

        public static readonly DependencyProperty LabelTextProperty = LabelTextPropertyKey.DependencyProperty;

        public string LabelText
        {
            get { return (string)GetValue(LabelTextProperty); }
            private set { SetValue(LabelTextPropertyKey, value); }
        }
        #endregion

        #region LabelMargin Readonly DependencyProperty
        internal static readonly DependencyPropertyKey LabelMarginPropertyKey = DependencyProperty.RegisterReadOnly("LabelMargin",
            typeof(Thickness),
            typeof(DataBar),
            new PropertyMetadata(default(Thickness)));

        public static readonly DependencyProperty LabelMarginProperty = LabelMarginPropertyKey.DependencyProperty;

        public Thickness LabelMargin
        {
            get { return (Thickness)GetValue(LabelMarginProperty); }
            private set { SetValue(LabelMarginPropertyKey, value); }
        }
        #endregion

        #region OutOfRangeTemplate Readonly DependencyProperty
        internal static readonly DependencyPropertyKey OutOfRangeTemplatePropertyKey = DependencyProperty.RegisterReadOnly("OutOfRangeTemplate",
            typeof(DataTemplate),
            typeof(DataBar),
            new PropertyMetadata(null));

        public static readonly DependencyProperty OutOfRangeTemplateProperty = OutOfRangeTemplatePropertyKey.DependencyProperty;

        public DataTemplate OutOfRangeTemplate
        {
            get { return (DataTemplate)GetValue(OutOfRangeTemplateProperty); }
            private set { SetValue(OutOfRangeTemplatePropertyKey, value); }
        }
        #endregion

        #region UseBarShapeOutOfRangeMarker DependencyProperty
        public static readonly DependencyProperty UseBarShapeOutOfRangeMarkerProperty = DependencyProperty.Register("UseBarShapeOutOfRangeMarker",
            typeof(bool),
            typeof(DataBar),
            new PropertyMetadata(BooleanBoxes.TrueBox, UseBarShapeOutOfRangeMarkerPropertyChanged));

        private static void UseBarShapeOutOfRangeMarkerPropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            var instance = (DataBar)sender;

            instance.UpdateBarSize();
        }

        public bool UseBarShapeOutOfRangeMarker
        {
            get { return (bool)GetValue(UseBarShapeOutOfRangeMarkerProperty); }
            set { SetValue(UseBarShapeOutOfRangeMarkerProperty, BooleanBoxes.Box(value)); }
        }
        #endregion

        private static void ValueBrushPropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            var instance = (DataBar)sender;

            instance.UpdateValueBrush();
        }

        private static void BarBorderBrushPropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            var instance = (DataBar)sender;

            instance.UpdateBarBorderBrush();
        }

        private DataBarShape _barShape;

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            _barShape = GetTemplateChild("PART_BarShape") as DataBarShape;

            UpdateBarSize();
            UpdateLabelText();
            UpdateLabelMargin();
        }

        protected override void OnRenderSizeChanged(SizeChangedInfo sizeInfo)
        {
            base.OnRenderSizeChanged(sizeInfo);

            UpdateLabelMargin();
            UpdateOriginAxisMargin();
        }

        protected override void OnMinimumChanged(double oldValue, double newValue)
        {
            UpdateBarSize();
            UpdateLabelMargin();
        }

        protected override void OnMaximumChanged(double oldValue, double newValue)
        {
            UpdateBarSize();
            UpdateLabelMargin();
        }

        protected override void UpdateOutOfRangeTemplates()
        {
            if (Value < Minimum) OutOfRangeTemplate = UnderflowTemplate;
            else if (Value > Maximum) OutOfRangeTemplate = OverflowTemplate;
            else OutOfRangeTemplate = null;
        }

        protected override void UpdateOriginAxisMargin()
        {
            OriginAxisMargin = CalculateOriginAxisMargin(OriginValue);
        }

        private void UpdateBarSize()
        {
            if (_barShape == null) return;

            if (Minimum >= Maximum || !Utility.IsANumber(Minimum) || !Utility.IsANumber(Maximum))
            {
                _barShape.Start = 0;
                _barShape.End = 0;
                _barShape.OutOfRangeState = OutOfRangeState.None;
                return;
            }

            var value = Value;
            var origin = OriginValue;

            if (!Utility.IsANumber(value)) value = 0;
            if (!Utility.IsANumber(origin)) origin = 0;

            var normalizedValue = Utility.NormalizeValue(value, Minimum, Maximum);
            var normalizedOrigin = Utility.NormalizeValue(origin, Minimum, Maximum);

            var state = OutOfRangeState.None;

            if (UseBarShapeOutOfRangeMarker)
            {
                if (value < Minimum) state = OutOfRangeState.Underflow;
                else if (value > Maximum) state = OutOfRangeState.Overflow;
            }

            _barShape.Start = Math.Min(normalizedValue, normalizedOrigin);
            _barShape.End = Math.Max(normalizedValue, normalizedOrigin);
            _barShape.OutOfRangeState = state;
        }

        private void UpdateValueBrush()
        {
            if (Value >= OriginValue) ActualValueBrush = PositiveValueBrush;
            else ActualValueBrush = NegativeValueBrush;
        }

        private void UpdateBarBorderBrush()
        {
            if (Value >= OriginValue) ActualBarBorderBrush = PositiveValueBarBorderBrush;
            else ActualBarBorderBrush = NegativeValueBarBorderBrush;
        }

        private void UpdateLabelText()
        {
            LabelText = Value.ToString(LabelFormatString);
        }

        private void UpdateLabelMargin()
        {
            if (Minimum >= Maximum)
            {
                LabelMargin = new Thickness(0);
                return;
            }

            double left = 0;

            var offset = LabelOffset;

            if (LabelPosition == LabelPosition.Left) left = offset;
            else
            {
                var textWidth = GetTextWidth();

                if (LabelPosition == LabelPosition.Right) left = ActualWidth - textWidth - offset - BorderThickness.Left - BorderThickness.Right;
                else if (LabelPosition == LabelPosition.Center) left = ActualWidth / 2 - textWidth / 2 + offset;
                else
                {
                    var value = Utility.CoerceValue(Utility.NormalizeValue(Value, Minimum, Maximum), 0, 1);

                    var endOfBar = value * (ActualWidth - BorderThickness.Left - BorderThickness.Right);

                    if ((Value >= OriginValue && LabelPosition == LabelPosition.EndOfBarInside) || (Value < OriginValue && LabelPosition == LabelPosition.EndOfBarOutside))
                    {
                        var calculated = endOfBar - textWidth - offset;

                        left = Math.Max(calculated, offset);
                    }
                    else if ((Value < OriginValue && LabelPosition == LabelPosition.EndOfBarInside) || (Value >= OriginValue && LabelPosition == LabelPosition.EndOfBarOutside))
                    {
                        var calculated = endOfBar + offset;
                        var maximum = ActualWidth - textWidth - offset - BorderThickness.Left - BorderThickness.Right;

                        left = Math.Min(calculated, maximum);
                    }
                }
            }

            LabelMargin = new Thickness(left, 0, 0, 0);
        }

        private double GetTextWidth()
        {
            var textBlock = new TextBlock()
            {
                FontSize = FontSize,
                Text = LabelText,
                Style = LabelStyle
            };

            if (FontFamily != null) textBlock.FontFamily = FontFamily;

            textBlock.Measure(new Size(double.PositiveInfinity, double.PositiveInfinity));

            return textBlock.DesiredSize.Width;
        }
    }
}