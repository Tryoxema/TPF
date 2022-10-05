using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Controls;
using TPF.Controls.Specialized.DataBar;
using TPF.Internal;

namespace TPF.Controls
{
    public abstract class DataBarBase : Control
    {
        #region BarHeightFactor DependencyProperty
        public static readonly DependencyProperty BarHeightFactorProperty = DependencyProperty.Register("BarHeightFactor",
            typeof(double),
            typeof(DataBarBase),
            new PropertyMetadata(0.6, null, ConstrainBarHeightFactor));

        private static object ConstrainBarHeightFactor(DependencyObject d, object baseValue)
        {
            var doubleValue = (double)baseValue;

            if (doubleValue < 0) doubleValue = 0;
            else if (doubleValue > 1) doubleValue = 1;

            return doubleValue;
        }

        public double BarHeightFactor
        {
            get { return (double)GetValue(BarHeightFactorProperty); }
            set { SetValue(BarHeightFactorProperty, value); }
        }
        #endregion

        #region BarStrokeThickness DependencyProperty
        public static readonly DependencyProperty BarStrokeThicknessProperty = DependencyProperty.Register("BarStrokeThickness",
            typeof(double),
            typeof(DataBarBase),
            new PropertyMetadata(0.0));

        public double BarStrokeThickness
        {
            get { return (double)GetValue(BarStrokeThicknessProperty); }
            set { SetValue(BarStrokeThicknessProperty, value); }
        }
        #endregion

        #region BarStyle DependencyProperty
        public static readonly DependencyProperty BarStyleProperty = DependencyProperty.Register("BarStyle",
            typeof(Style),
            typeof(DataBarBase),
            new PropertyMetadata(null));

        public Style BarStyle
        {
            get { return (Style)GetValue(BarStyleProperty); }
            set { SetValue(BarStyleProperty, value); }
        }
        #endregion

        #region Minimum DependencyProperty
        public static readonly DependencyProperty MinimumProperty = DependencyProperty.Register("Minimum",
            typeof(double),
            typeof(DataBarBase),
            new PropertyMetadata(0.0, MinimumPropertyChanged));

        private static void MinimumPropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            var instance = (DataBarBase)sender;

            instance.OnMinimumChanged((double)e.OldValue, (double)e.NewValue);
            instance.UpdateOriginAxisMargin();
            instance.UpdateOutOfRangeTemplates();
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
            typeof(DataBarBase),
            new PropertyMetadata(100.0, MaximumPropertyChanged));

        private static void MaximumPropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            var instance = (DataBarBase)sender;

            instance.OnMaximumChanged((double)e.OldValue, (double)e.NewValue);
            instance.UpdateOriginAxisMargin();
            instance.UpdateOutOfRangeTemplates();
        }

        public double Maximum
        {
            get { return (double)GetValue(MaximumProperty); }
            set { SetValue(MaximumProperty, value); }
        }
        #endregion

        #region OriginAxisVisibility DependencyProperty
        public static readonly DependencyProperty OriginAxisVisibilityProperty = DependencyProperty.Register("OriginAxisVisibility",
            typeof(Visibility),
            typeof(DataBarBase),
            new PropertyMetadata(Visibility.Collapsed));

        public Visibility OriginAxisVisibility
        {
            get { return (Visibility)GetValue(OriginAxisVisibilityProperty); }
            set { SetValue(OriginAxisVisibilityProperty, value); }
        }
        #endregion

        #region OriginAxisBrush DependencyProperty
        public static readonly DependencyProperty OriginAxisBrushProperty = DependencyProperty.Register("OriginAxisBrush",
            typeof(Brush),
            typeof(DataBarBase),
            new PropertyMetadata(null));

        public Brush OriginAxisBrush
        {
            get { return (Brush)GetValue(OriginAxisBrushProperty); }
            set { SetValue(OriginAxisBrushProperty, value); }
        }
        #endregion

        #region OriginAxisStyle DependencyProperty
        public static readonly DependencyProperty OriginAxisStyleProperty = DependencyProperty.Register("OriginAxisStyle",
            typeof(Style),
            typeof(DataBarBase),
            new PropertyMetadata(null));

        public Style OriginAxisStyle
        {
            get { return (Style)GetValue(OriginAxisStyleProperty); }
            set { SetValue(OriginAxisStyleProperty, value); }
        }
        #endregion

        #region OriginAxisMargin Readonly DependencyProperty
        internal static readonly DependencyPropertyKey OriginAxisMarginPropertyKey = DependencyProperty.RegisterReadOnly("OriginAxisMargin",
            typeof(Thickness),
            typeof(DataBarBase),
            new PropertyMetadata(default(Thickness)));

        public static readonly DependencyProperty OriginAxisMarginProperty = OriginAxisMarginPropertyKey.DependencyProperty;

        public Thickness OriginAxisMargin
        {
            get { return (Thickness)GetValue(OriginAxisMarginProperty); }
            protected set { SetValue(OriginAxisMarginPropertyKey, value); }
        }
        #endregion

        #region OverflowTemplate DependencyProperty
        public static readonly DependencyProperty OverflowTemplateProperty = DependencyProperty.Register("OverflowTemplate",
            typeof(DataTemplate),
            typeof(DataBarBase),
            new PropertyMetadata(null, OutOfRangeTemplatePropertiesChanged));

        public DataTemplate OverflowTemplate
        {
            get { return (DataTemplate)GetValue(OverflowTemplateProperty); }
            set { SetValue(OverflowTemplateProperty, value); }
        }
        #endregion

        #region UnderflowTemplate DependencyProperty
        public static readonly DependencyProperty UnderflowTemplateProperty = DependencyProperty.Register("UnderflowTemplate",
            typeof(DataTemplate),
            typeof(DataBarBase),
            new PropertyMetadata(null, OutOfRangeTemplatePropertiesChanged));

        public DataTemplate UnderflowTemplate
        {
            get { return (DataTemplate)GetValue(UnderflowTemplateProperty); }
            set { SetValue(UnderflowTemplateProperty, value); }
        }
        #endregion

        #region OutOfRangeMarkerType DependencyProperty
        public static readonly DependencyProperty OutOfRangeMarkerTypeProperty = DependencyProperty.Register("OutOfRangeMarkerType",
            typeof(OutOfRangeMarkerType),
            typeof(DataBarBase),
            new PropertyMetadata(OutOfRangeMarkerType.Arrow));

        public OutOfRangeMarkerType OutOfRangeMarkerType
        {
            get { return (OutOfRangeMarkerType)GetValue(OutOfRangeMarkerTypeProperty); }
            set { SetValue(OutOfRangeMarkerTypeProperty, value); }
        }
        #endregion

        private static void OutOfRangeTemplatePropertiesChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            var instance = (DataBarBase)sender;

            instance.UpdateOutOfRangeTemplates();
        }

        protected virtual void OnMinimumChanged(double oldValue, double newValue) { }

        protected virtual void OnMaximumChanged(double oldValue, double newValue) { }

        protected virtual void UpdateOutOfRangeTemplates() { }

        protected virtual void UpdateOriginAxisMargin()
        {
            // Da die Basisklasse keine Property für OriginValue hat wird hier einfach 0 übergeben
            OriginAxisMargin = CalculateOriginAxisMargin(0);
        }

        protected Thickness CalculateOriginAxisMargin(double originValue)
        {
            if (Minimum >= Maximum || !Utility.IsANumber(Minimum) || !Utility.IsANumber(Maximum))
            {
                return new Thickness(0);
            }

            var barWidth = ActualWidth - BorderThickness.Left - BorderThickness.Right;

            var left = Math.Round(Utility.CoerceValue(Utility.NormalizeValue(originValue, Minimum, Maximum), 0, 1) * barWidth, 2);

            // Wenn der errechnete Punkt bis auf 2 Nachkommastellen mit der Breite übereinstimmt, verschieben wir den Punkt um 1 nach Links
            if (left == Math.Round(barWidth, 2)) left -= 1;

            return new Thickness(left, 0, 0, 0);
        }
    }
}