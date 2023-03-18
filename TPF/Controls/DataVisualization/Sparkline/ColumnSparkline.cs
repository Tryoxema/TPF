using System;
using System.Linq;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;
using TPF.Controls.Specialized.Sparkline;

namespace TPF.Controls
{
    public class ColumnSparkline : SparklineBase
    {
        static ColumnSparkline()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ColumnSparkline), new FrameworkPropertyMetadata(typeof(ColumnSparkline)));
        }

        #region ColumnStyle DependencyProperty
        public static readonly DependencyProperty ColumnStyleProperty = DependencyProperty.Register("ColumnStyle",
            typeof(Style),
            typeof(ColumnSparkline),
            new PropertyMetadata(null));

        public Style ColumnStyle
        {
            get { return (Style)GetValue(ColumnStyleProperty); }
            set { SetValue(ColumnStyleProperty, value); }
        }
        #endregion

        #region ColumnBrush DependencyProperty
        public static readonly DependencyProperty ColumnBrushProperty = DependencyProperty.Register("ColumnBrush",
            typeof(Brush),
            typeof(ColumnSparkline),
            new PropertyMetadata(null));

        public Brush ColumnBrush
        {
            get { return (Brush)GetValue(ColumnBrushProperty); }
            set { SetValue(ColumnBrushProperty, value); }
        }
        #endregion

        #region ColumnWidthFactor DependencyProperty
        public static readonly DependencyProperty ColumnWidthFactorProperty = DependencyProperty.Register("ColumnWidthFactor",
            typeof(double),
            typeof(ColumnSparkline),
            new PropertyMetadata(0.9, null, ConstrainColumnWidthFactor));

        private static object ConstrainColumnWidthFactor(DependencyObject d, object baseValue)
        {
            var doubleValue = (double)baseValue;

            if (doubleValue < 0) doubleValue = 0;
            else if (doubleValue > 1) doubleValue = 1;

            return doubleValue;
        }

        public double ColumnWidthFactor
        {
            get { return (double)GetValue(ColumnWidthFactorProperty); }
            set { SetValue(ColumnWidthFactorProperty, value); }
        }
        #endregion

        protected ColumnsPanel _columnsPanel;

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            _columnsPanel = GetTemplateChild("PART_ColumnsPanel") as ColumnsPanel;

            RefreshColumns();
        }

        protected override void OnRenderSizeChanged(SizeChangedInfo sizeInfo)
        {
            base.OnRenderSizeChanged(sizeInfo);

            RefreshColumns();
        }

        protected override void OnDataChanged()
        {
            base.OnDataChanged();

            RefreshColumns();
        }

        protected virtual void RefreshColumns()
        {
            CalculateColumns();
            OnUpdateIndicators();
        }

        protected virtual void CalculateColumns()
        {
            if (_columnsPanel == null) return;

            if (_columnsPanel.Children.Count > 0)
            {
                foreach (ColumnItem child in _columnsPanel.Children)
                {
                    BindingOperations.ClearAllBindings(child);
                }

                _columnsPanel.Children.Clear();
            }

            var visualDataPoints = DataPoints;

            if (visualDataPoints == null || visualDataPoints.Count == 0) return;

            var width = ActualWidth;
            var height = ActualHeight;

            if (width > 0 && height > 0)
            {
                for (var i = 0; i < visualDataPoints.Count; i++)
                {
                    var dataPoint = visualDataPoints[i];

                    var relativeXPoint = XRange.GetRelativePoint(dataPoint.X);

                    var relativeYPoint = 0d;
                    var relativeYBase = 0d;

                    if (YRange.Delta != 0d)
                    {
                        relativeYPoint = YRange.GetRelativePoint(dataPoint.Y);

                        if (YRange.Contains(0d)) relativeYBase = YRange.GetRelativePoint(0d);
                        else if (YRange.Start > 0d) relativeYBase = YRange.GetRelativePoint(YRange.Start);
                        else relativeYBase = YRange.GetRelativePoint(YRange.End);
                    }

                    var relativeYTop = Math.Max(relativeYPoint, relativeYBase);
                    var relativeYBottom = Math.Min(relativeYPoint, relativeYBase);

                    var column = new ColumnItem()
                    {
                        DataContext = dataPoint,
                        RelativeX = relativeXPoint,
                        RelativeYTop = relativeYTop,
                        RelativeYBottom = relativeYBottom,
                        Type = IndicatorType.Normal
                    };

                    column.SetBinding(StyleProperty, new Binding(nameof(ColumnStyle)) { Source = this });
                    column.SetBinding(BackgroundProperty, new Binding(nameof(ColumnBrush)) { Source = this });
                    column.SetBinding(ColumnItem.ToolTipTemplateProperty, new Binding(nameof(ToolTipTemplate)) { Source = this });

                    _columnsPanel.Children.Add(column);
                }
            }
        }

        protected override void OnUpdateIndicators()
        {
            if (_columnsPanel == null || _columnsPanel.Children.Count == 0 || DataPoints.Count == 0) return;

            var counter = 0;

            var maxValue = DataPoints.Max(item => item.Y);
            var minValue = DataPoints.Min(item => item.Y);

            foreach (ColumnItem child in _columnsPanel.Children)
            {
                var dataPoint = child.DataContext as SparklineDataPoint;

                string brushPropertyName;
                var type = IndicatorType.Normal;

                if (ShowFirstPointIndicator && counter == 0)
                {
                    brushPropertyName = nameof(FirstPointBrush);
                    type = IndicatorType.First;
                }
                else if (ShowLastPointIndicator && counter == DataPoints.Count - 1)
                {
                    brushPropertyName = nameof(LastPointBrush);
                    type = IndicatorType.Last;
                }
                else if (ShowHighPointIndicators && dataPoint.Y == maxValue)
                {
                    brushPropertyName = nameof(HighPointBrush);
                    type = IndicatorType.High;
                }
                else if (ShowLowPointIndicators && dataPoint.Y == minValue)
                {
                    brushPropertyName = nameof(LowPointBrush);
                    type = IndicatorType.Low;
                }
                else if (ShowNegativePointIndicators && dataPoint.Y < 0)
                {
                    brushPropertyName = nameof(NegativePointBrush);
                    type = IndicatorType.Negative;
                }
                else
                {
                    brushPropertyName = nameof(ColumnBrush);
                }

                child.SetBinding(BackgroundProperty, new Binding(brushPropertyName) { Source = this });
                child.Type = type;

                counter++;
            }
        }
    }
}