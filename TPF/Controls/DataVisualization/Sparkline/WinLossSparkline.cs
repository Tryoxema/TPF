using System.Linq;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;
using TPF.Controls.Specialized.Sparkline;

namespace TPF.Controls
{
    public class WinLossSparkline : ColumnSparkline
    {
        static WinLossSparkline()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(WinLossSparkline), new FrameworkPropertyMetadata(typeof(WinLossSparkline)));
        }

        #region NeutralPointBrush DependencyProperty
        public static readonly DependencyProperty NeutralPointBrushProperty = DependencyProperty.Register("NeutralPointBrush",
            typeof(Brush),
            typeof(WinLossSparkline),
            new PropertyMetadata(null));

        public Brush NeutralPointBrush
        {
            get { return (Brush)GetValue(NeutralPointBrushProperty); }
            set { SetValue(NeutralPointBrushProperty, value); }
        }
        #endregion

        protected override void UpdateAxisPosition()
        {
            if (_axis == null) return;

            var y = ActualHeight - (ActualHeight * 0.5);

            _axis.X1 = 0;
            _axis.X2 = ActualWidth;
            _axis.Y1 = y;
            _axis.Y2 = y;
        }

        protected override void CalculateColumns()
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
                var neutralValue = AxisValue;

                for (var i = 0; i < visualDataPoints.Count; i++)
                {
                    var dataPoint = visualDataPoints[i];

                    var relativeXPoint = XRange.GetRelativePoint(dataPoint.X);

                    var relativeYTop = 0d;
                    var relativeYBottom = 0d;

                    if (dataPoint.Y > neutralValue)
                    {
                        relativeYTop = 1;
                        relativeYBottom = 0.5;
                    }
                    else if (dataPoint.Y < neutralValue)
                    {
                        relativeYTop = 0.5;
                        relativeYBottom = 0;
                    }
                    else if (dataPoint.Y == neutralValue)
                    {
                        relativeYTop = 0.55;
                        relativeYBottom = 0.45;
                    }

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
            var neutralValue = AxisValue;

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
                else if (dataPoint.Y == neutralValue)
                {
                    brushPropertyName = nameof(NeutralPointBrush);
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