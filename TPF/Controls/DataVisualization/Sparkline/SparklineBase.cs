using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using TPF.Collections;
using TPF.Controls.Specialized.Sparkline;
using TPF.Internal;
using TPF.Data;

namespace TPF.Controls
{
    public abstract class SparklineBase : Control
    {
        #region ItemsSource DependencyProperty
        public static readonly DependencyProperty ItemsSourceProperty = DependencyProperty.Register("ItemsSource",
            typeof(IEnumerable),
            typeof(SparklineBase),
            new PropertyMetadata(null, ItemsSourcePropertyChanged));

        private static void ItemsSourcePropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            var instance = (SparklineBase)sender;

            instance.OnItemsSourceChanged(e);
        }

        public IEnumerable ItemsSource
        {
            get { return (IEnumerable)GetValue(ItemsSourceProperty); }
            set { SetValue(ItemsSourceProperty, value); }
        }
        #endregion

        #region XValuePath DependencyProperty
        public static readonly DependencyProperty XValuePathProperty = DependencyProperty.Register("XValuePath",
            typeof(string),
            typeof(SparklineBase),
            new PropertyMetadata(null, XValuePathPropertyChanged));

        private static void XValuePathPropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            var instance = (SparklineBase)sender;

            instance.OnXValuePathChanged();
        }

        public string XValuePath
        {
            get { return (string)GetValue(XValuePathProperty); }
            set { SetValue(XValuePathProperty, value); }
        }
        #endregion

        #region YValuePath DependencyProperty
        public static readonly DependencyProperty YValuePathProperty = DependencyProperty.Register("YValuePath",
            typeof(string),
            typeof(SparklineBase),
            new PropertyMetadata(null, YValuePathPropertyChanged));

        private static void YValuePathPropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            var instance = (SparklineBase)sender;

            instance.OnYValuePathChanged();
        }

        public string YValuePath
        {
            get { return (string)GetValue(YValuePathProperty); }
            set { SetValue(YValuePathProperty, value); }
        }
        #endregion

        #region MinXValue DependencyProperty
        public static readonly DependencyProperty MinXValueProperty = DependencyProperty.Register("MinXValue",
            typeof(double),
            typeof(SparklineBase),
            new PropertyMetadata(double.NaN, MinMaxPropertyChanged));

        public double MinXValue
        {
            get { return (double)GetValue(MinXValueProperty); }
            set { SetValue(MinXValueProperty, value); }
        }
        #endregion

        #region MaxXValue DependencyProperty
        public static readonly DependencyProperty MaxXValueProperty = DependencyProperty.Register("MaxXValue",
            typeof(double),
            typeof(SparklineBase),
            new PropertyMetadata(double.NaN, MinMaxPropertyChanged));

        public double MaxXValue
        {
            get { return (double)GetValue(MaxXValueProperty); }
            set { SetValue(MaxXValueProperty, value); }
        }
        #endregion

        #region MinYValue DependencyProperty
        public static readonly DependencyProperty MinYValueProperty = DependencyProperty.Register("MinYValue",
            typeof(double),
            typeof(SparklineBase),
            new PropertyMetadata(double.NaN, MinMaxPropertyChanged));

        public double MinYValue
        {
            get { return (double)GetValue(MinYValueProperty); }
            set { SetValue(MinYValueProperty, value); }
        }
        #endregion

        #region MaxYValue DependencyProperty
        public static readonly DependencyProperty MaxYValueProperty = DependencyProperty.Register("MaxYValue",
            typeof(double),
            typeof(SparklineBase),
            new PropertyMetadata(double.NaN, MinMaxPropertyChanged));

        public double MaxYValue
        {
            get { return (double)GetValue(MaxYValueProperty); }
            set { SetValue(MaxYValueProperty, value); }
        }
        #endregion

        #region EmptyPointBehavior DependencyProperty
        public static readonly DependencyProperty EmptyPointBehaviorProperty = DependencyProperty.Register("EmptyPointBehavior",
            typeof(EmptyPointBehavior),
            typeof(SparklineBase),
            new PropertyMetadata(EmptyPointBehavior.Ignore, EmptyPointBehaviorPropertyChanged));

        private static void EmptyPointBehaviorPropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            var instance = (SparklineBase)sender;

            instance.OnEmptyPointBehaviorChanged();
        }

        public EmptyPointBehavior EmptyPointBehavior
        {
            get { return (EmptyPointBehavior)GetValue(EmptyPointBehaviorProperty); }
            set { SetValue(EmptyPointBehaviorProperty, value); }
        }
        #endregion

        #region AxisValue DependencyProperty
        public static readonly DependencyProperty AxisValueProperty = DependencyProperty.Register("AxisValue",
            typeof(double),
            typeof(SparklineBase),
            new PropertyMetadata(0.0, AxisValuePropertyChanged));

        private static void AxisValuePropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            var instance = (SparklineBase)sender;

            instance.UpdateAxisPosition();
        }

        public double AxisValue
        {
            get { return (double)GetValue(AxisValueProperty); }
            set { SetValue(AxisValueProperty, value); }
        }
        #endregion

        #region AxisBrush DependencyProperty
        public static readonly DependencyProperty AxisBrushProperty = DependencyProperty.Register("AxisBrush",
            typeof(Brush),
            typeof(SparklineBase),
            new PropertyMetadata(null));

        public Brush AxisBrush
        {
            get { return (Brush)GetValue(AxisBrushProperty); }
            set { SetValue(AxisBrushProperty, value); }
        }
        #endregion

        #region ShowAxis DependencyProperty
        public static readonly DependencyProperty ShowAxisProperty = DependencyProperty.Register("ShowAxis",
            typeof(bool),
            typeof(SparklineBase),
            new PropertyMetadata(BooleanBoxes.FalseBox, ShowAxisPropertyChanged));

        private static void ShowAxisPropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            var instance = (SparklineBase)sender;

            instance.UpdateAxisVisibility();
        }

        public bool ShowAxis
        {
            get { return (bool)GetValue(ShowAxisProperty); }
            set { SetValue(ShowAxisProperty, BooleanBoxes.Box(value)); }
        }
        #endregion

        #region FirstPointBrush DependencyProperty
        public static readonly DependencyProperty FirstPointBrushProperty = DependencyProperty.Register("FirstPointBrush",
            typeof(Brush),
            typeof(SparklineBase),
            new PropertyMetadata(OnIndicatorPropertyChanged));

        public Brush FirstPointBrush
        {
            get { return (Brush)GetValue(FirstPointBrushProperty); }
            set { SetValue(FirstPointBrushProperty, value); }
        }
        #endregion

        #region LastPointBrush DependencyProperty
        public static readonly DependencyProperty LastPointBrushProperty = DependencyProperty.Register("LastPointBrush",
            typeof(Brush),
            typeof(SparklineBase),
            new PropertyMetadata(OnIndicatorPropertyChanged));

        public Brush LastPointBrush
        {
            get { return (Brush)GetValue(LastPointBrushProperty); }
            set { SetValue(LastPointBrushProperty, value); }
        }
        #endregion

        #region HighPointBrush DependencyProperty
        public static readonly DependencyProperty HighPointBrushProperty = DependencyProperty.Register("HighPointBrush",
            typeof(Brush),
            typeof(SparklineBase),
            new PropertyMetadata(OnIndicatorPropertyChanged));

        public Brush HighPointBrush
        {
            get { return (Brush)GetValue(HighPointBrushProperty); }
            set { SetValue(HighPointBrushProperty, value); }
        }
        #endregion

        #region LowPointBrush DependencyProperty
        public static readonly DependencyProperty LowPointBrushProperty = DependencyProperty.Register("LowPointBrush",
            typeof(Brush),
            typeof(SparklineBase),
            new PropertyMetadata(OnIndicatorPropertyChanged));

        public Brush LowPointBrush
        {
            get { return (Brush)GetValue(LowPointBrushProperty); }
            set { SetValue(LowPointBrushProperty, value); }
        }
        #endregion

        #region NegativePointBrush DependencyProperty
        public static readonly DependencyProperty NegativePointBrushProperty = DependencyProperty.Register("NegativePointBrush",
            typeof(Brush),
            typeof(SparklineBase),
            new PropertyMetadata(OnIndicatorPropertyChanged));

        public Brush NegativePointBrush
        {
            get { return (Brush)GetValue(NegativePointBrushProperty); }
            set { SetValue(NegativePointBrushProperty, value); }
        }
        #endregion

        #region ShowFirstPointIndicator DependencyProperty
        public static readonly DependencyProperty ShowFirstPointIndicatorProperty = DependencyProperty.Register("ShowFirstPointIndicator",
            typeof(bool),
            typeof(SparklineBase),
            new PropertyMetadata(BooleanBoxes.FalseBox, OnIndicatorPropertyChanged));

        public bool ShowFirstPointIndicator
        {
            get { return (bool)GetValue(ShowFirstPointIndicatorProperty); }
            set { SetValue(ShowFirstPointIndicatorProperty, BooleanBoxes.Box(value)); }
        }
        #endregion

        #region ShowLastPointIndicator DependencyProperty
        public static readonly DependencyProperty ShowLastPointIndicatorProperty = DependencyProperty.Register("ShowLastPointIndicator",
            typeof(bool),
            typeof(SparklineBase),
            new PropertyMetadata(BooleanBoxes.FalseBox, OnIndicatorPropertyChanged));

        public bool ShowLastPointIndicator
        {
            get { return (bool)GetValue(ShowLastPointIndicatorProperty); }
            set { SetValue(ShowLastPointIndicatorProperty, BooleanBoxes.Box(value)); }
        }
        #endregion

        #region ShowHighPointIndicators DependencyProperty
        public static readonly DependencyProperty ShowHighPointIndicatorsProperty = DependencyProperty.Register("ShowHighPointIndicators",
            typeof(bool),
            typeof(SparklineBase),
            new PropertyMetadata(BooleanBoxes.FalseBox, OnIndicatorPropertyChanged));

        public bool ShowHighPointIndicators
        {
            get { return (bool)GetValue(ShowHighPointIndicatorsProperty); }
            set { SetValue(ShowHighPointIndicatorsProperty, BooleanBoxes.Box(value)); }
        }
        #endregion

        #region ShowLowPointIndicators DependencyProperty
        public static readonly DependencyProperty ShowLowPointIndicatorsProperty = DependencyProperty.Register("ShowLowPointIndicators",
            typeof(bool),
            typeof(SparklineBase),
            new PropertyMetadata(BooleanBoxes.FalseBox, OnIndicatorPropertyChanged));

        public bool ShowLowPointIndicators
        {
            get { return (bool)GetValue(ShowLowPointIndicatorsProperty); }
            set { SetValue(ShowLowPointIndicatorsProperty, BooleanBoxes.Box(value)); }
        }
        #endregion

        #region ShowNegativePointIndicators DependencyProperty
        public static readonly DependencyProperty ShowNegativePointIndicatorsProperty = DependencyProperty.Register("ShowNegativePointIndicators",
            typeof(bool),
            typeof(SparklineBase),
            new PropertyMetadata(BooleanBoxes.FalseBox, OnIndicatorPropertyChanged));

        public bool ShowNegativePointIndicators
        {
            get { return (bool)GetValue(ShowNegativePointIndicatorsProperty); }
            set { SetValue(ShowNegativePointIndicatorsProperty, BooleanBoxes.Box(value)); }
        }
        #endregion

        #region ToolTipTemplate DependencyProperty
        public static readonly DependencyProperty ToolTipTemplateProperty = DependencyProperty.Register("ToolTipTemplate",
            typeof(DataTemplate),
            typeof(SparklineBase),
            new PropertyMetadata(null));

        public DataTemplate ToolTipTemplate
        {
            get { return (DataTemplate)GetValue(ToolTipTemplateProperty); }
            set { SetValue(ToolTipTemplateProperty, value); }
        }
        #endregion

        private static void MinMaxPropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            var instance = (SparklineBase)sender;

            instance.UpdateData();
        }

        protected static void OnIndicatorPropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            var instance = (SparklineBase)sender;

            instance.UpdateIndicators();
        }

        private Line _axis;

        private DoubleRange _xRange;
        protected DoubleRange XRange
        {
            get { return _xRange; }
        }

        private DoubleRange _yRange;
        protected DoubleRange YRange
        {
            get { return _yRange; }
        }

        private RangeObservableCollection<SparklineDataItem> _sparklineDataItems;
        internal RangeObservableCollection<SparklineDataItem> SparklineDataItems
        {
            get { return _sparklineDataItems ?? (_sparklineDataItems = new RangeObservableCollection<SparklineDataItem>() { ResetOnChange = false }); }
        }

        protected ReadOnlyCollection<SparklineDataPoint> DataPoints { get; private set; }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            _axis = GetTemplateChild("PART_Axis") as Line;

            UpdateAxisVisibility();
            UpdateAxisPosition();
        }

        private void OnItemsSourceChanged(DependencyPropertyChangedEventArgs e)
        {
            if (e.OldValue is INotifyCollectionChanged oldItems)
            {
                oldItems.CollectionChanged -= ItemsSource_CollectionChanged;
            }

            if (e.NewValue is INotifyCollectionChanged newItems)
            {
                newItems.CollectionChanged += ItemsSource_CollectionChanged;
            }

            SparklineDataItems.Clear();
            AddSparklineDataItems(ItemsSource);
            UpdateData();
        }

        private void ItemsSource_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                {
                    AddSparklineDataItems(e.NewItems);
                    break;
                }
                case NotifyCollectionChangedAction.Remove:
                {
                    RemoveSparklineDataItems(e.OldItems);
                    break;
                }
                case NotifyCollectionChangedAction.Replace:
                {
                    RemoveSparklineDataItems(e.OldItems);
                    AddSparklineDataItems(e.NewItems);
                    break;
                }
                case NotifyCollectionChangedAction.Reset:
                {
                    SparklineDataItems.Clear();
                    AddSparklineDataItems(e.NewItems);
                    break;
                }
            }

            UpdateData();
        }

        private void AddSparklineDataItems(IEnumerable items)
        {
            if (items == null) return;

            foreach (var item in items)
            {
                var dataItem = new SparklineDataItem()
                {
                    DataItem = item,
                    XValuePath = XValuePath,
                    YValuePath = YValuePath
                };

                dataItem.PropertyChanged += DataItem_PropertyChanged;

                SparklineDataItems.Add(dataItem);
            }
        }

        private void RemoveSparklineDataItems(IEnumerable items)
        {
            if (items == null) return;

            foreach (var item in items)
            {
                var dataItem = SparklineDataItems.FirstOrDefault(x => x.DataItem == item);

                if (dataItem != null)
                {
                    dataItem.PropertyChanged -= DataItem_PropertyChanged;
                    SparklineDataItems.Remove(dataItem);
                }
            }
        }

        private void DataItem_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "XValue" || e.PropertyName == "YValue")
            {
                OnValuePathChanged();
            }
        }

        private void OnXValuePathChanged()
        {
            for (int i = 0; i < SparklineDataItems.Count; i++)
            {
                var item = SparklineDataItems[i];

                item.XValuePath = XValuePath;
            }

            OnValuePathChanged();
        }

        private void OnYValuePathChanged()
        {
            for (int i = 0; i < SparklineDataItems.Count; i++)
            {
                var item = SparklineDataItems[i];

                item.YValuePath = YValuePath;
            }

            OnValuePathChanged();
        }

        private void OnValuePathChanged()
        {
            UpdateData();
        }

        private void OnEmptyPointBehaviorChanged()
        {
            UpdateData();
        }

        protected virtual void UpdateAxisPosition()
        {
            if (_axis == null) return;

            var y = ActualHeight - (_yRange.GetRelativePoint(AxisValue) * ActualHeight);

            _axis.X1 = 0;
            _axis.X2 = ActualWidth;
            _axis.Y1 = y;
            _axis.Y2 = y;
        }

        private void UpdateAxisVisibility()
        {
            if (_axis == null) return;

            _axis.Visibility = ShowAxis ? Visibility.Visible : Visibility.Collapsed;
        }

        private void UpdateData()
        {
            var emptyPointBehavoir = EmptyPointBehavior;
            var items = SparklineDataItems;
            var points = new List<SparklineDataPoint>(items.Count);

            var useIndexing = string.IsNullOrWhiteSpace(XValuePath);
            var lastXValue = double.NaN;
            var requiresReordering = false;
            var yValueNeedsCalculation = false;
            double minY = 0, maxY = 0, minX = 0, maxX = 0;

            for (int i = 0; i < items.Count; i++)
            {
                var item = items[i];

                var x = item.XValue;
                var y = item.YValue;

                var hasYValue = Utility.IsANumber(y);

                // Wenn ein Y-Wert keine Zahl ist, dann berücksichtigen wir das Item nicht wenn EmptyPointBehavior das vorgibt
                if (!hasYValue && emptyPointBehavoir == EmptyPointBehavior.Ignore) continue;

                if (useIndexing) x = i;
                // Wenn kein X-Wert vorhanden ist, überspringen wir dieses Item
                if (!Utility.IsANumber(x)) continue;

                if (!hasYValue && emptyPointBehavoir == EmptyPointBehavior.Zero) y = 0;

                // Wenn der letzte X-Wert größer als der aktuelle ist, dann muss am Ende neu sortiert werden
                if (i > 0 && lastXValue > x) requiresReordering = true;

                if (emptyPointBehavoir == EmptyPointBehavior.Average && !hasYValue) yValueNeedsCalculation = true;

                if (y < minY) minY = y;
                if (y > maxY) maxY = y;

                var point = new SparklineDataPoint()
                {
                    DataItem = item.DataItem,
                    X = x,
                    Y = y
                };

                points.Add(point);
            }

            // Wenn die Reihenfolge der X-Werte nicht passt, einmal neu sortieren
            if (requiresReordering) points = points.OrderBy(x => x.X).ToList();

            if (yValueNeedsCalculation)
            {
                for (int i = 0; i < points.Count; i++)
                {
                    var point = points[i];

                    if (Utility.IsANumber(point.Y)) continue;

                    double previous = 0, next = 0;

                    if (i > 0) previous = points[i - 1].Y;
                    if (i < points.Count - 1) next = points[i + 1].Y;

                    // Wenn der nächste auch keine Zahl ist, dann als 0 bewerten
                    if (!Utility.IsANumber(next)) next = 0;

                    point.Y = (previous + next) / 2;
                }
            }

            if (points.Count > 0)
            {
                minX = points.First().X;
                maxX = points.Last().X;
            }

            DataPoints = points.AsReadOnly();

            // DataRanges festlegen
            if (Utility.IsANumber(MinXValue)) minX = MinXValue;
            if (Utility.IsANumber(MaxXValue)) maxX = MaxXValue;
            if (Utility.IsANumber(MinYValue)) minY = MinYValue;
            if (Utility.IsANumber(MaxYValue)) maxY = MaxYValue;

            _xRange = new DoubleRange(minX, maxX);
            _yRange = new DoubleRange(minY, maxY);

            OnDataChanged();
        }

        protected virtual void OnDataChanged()
        {
            UpdateAxisPosition();
            UpdateIndicators();
        }

        private void UpdateIndicators()
        {
            OnUpdateIndicators();
        }

        protected abstract void OnUpdateIndicators();

        protected override void OnRenderSizeChanged(SizeChangedInfo sizeInfo)
        {
            base.OnRenderSizeChanged(sizeInfo);

            UpdateAxisPosition();
        }
    }
}