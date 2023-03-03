using System.Linq;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;
using TPF.Controls.Specialized.Sparkline;
using TPF.Internal;

namespace TPF.Controls
{
    public abstract class LinearSparklineBase : SparklineBase
    {
        public LinearSparklineBase()
        {
            LinePoints = new PointCollection();
        }

        #region LineStyle DependencyProperty
        public static readonly DependencyProperty LineStyleProperty = DependencyProperty.Register("LineStyle",
            typeof(Style),
            typeof(LinearSparklineBase),
            new PropertyMetadata(null));

        public Style LineStyle
        {
            get { return (Style)GetValue(LineStyleProperty); }
            set { SetValue(LineStyleProperty, value); }
        }
        #endregion

        #region LineBrush DependencyProperty
        public static readonly DependencyProperty LineBrushProperty = DependencyProperty.Register("LineBrush",
            typeof(Brush),
            typeof(LinearSparklineBase),
            new PropertyMetadata(null));

        public Brush LineBrush
        {
            get { return (Brush)GetValue(LineBrushProperty); }
            set { SetValue(LineBrushProperty, value); }
        }
        #endregion

        #region LinePoints Readonly DependencyProperty
        internal static readonly DependencyPropertyKey LinePointsPropertyKey = DependencyProperty.RegisterReadOnly("LinePoints",
            typeof(PointCollection),
            typeof(LinearSparklineBase),
            new PropertyMetadata());

        public static readonly DependencyProperty LinePointsProperty = LinePointsPropertyKey.DependencyProperty;

        public PointCollection LinePoints
        {
            get { return (PointCollection)GetValue(LinePointsProperty); }
            private set { SetValue(LinePointsPropertyKey, value); }
        }
        #endregion

        #region IndicatorBrush DependencyProperty
        public static readonly DependencyProperty IndicatorBrushProperty = DependencyProperty.Register("IndicatorBrush",
            typeof(Brush),
            typeof(LinearSparklineBase),
            new PropertyMetadata(OnIndicatorPropertyChanged));

        public Brush IndicatorBrush
        {
            get { return (Brush)GetValue(IndicatorBrushProperty); }
            set { SetValue(IndicatorBrushProperty, value); }
        }
        #endregion

        #region IndicatorStyle DependencyProperty
        public static readonly DependencyProperty IndicatorStyleProperty = DependencyProperty.Register("IndicatorStyle",
            typeof(Style),
            typeof(LinearSparklineBase),
            new PropertyMetadata(OnIndicatorPropertyChanged));

        public Style IndicatorStyle
        {
            get { return (Style)GetValue(IndicatorStyleProperty); }
            set { SetValue(IndicatorStyleProperty, value); }
        }
        #endregion

        #region ShowIndicators DependencyProperty
        public static readonly DependencyProperty ShowIndicatorsProperty = DependencyProperty.Register("ShowIndicators",
            typeof(bool),
            typeof(LinearSparklineBase),
            new PropertyMetadata(BooleanBoxes.FalseBox, OnIndicatorPropertyChanged));

        public bool ShowIndicators
        {
            get { return (bool)GetValue(ShowIndicatorsProperty); }
            set { SetValue(ShowIndicatorsProperty, BooleanBoxes.Box(value)); }
        }
        #endregion

        private IndicatorPanel _indicatorPanel;

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            _indicatorPanel = GetTemplateChild("PART_IndicatorPanel") as IndicatorPanel;

            OnUpdateIndicators();
        }

        protected override void OnRenderSizeChanged(SizeChangedInfo sizeInfo)
        {
            base.OnRenderSizeChanged(sizeInfo);

            RefreshLinePoints();
            OnUpdateIndicators();
        }

        protected override void OnDataChanged()
        {
            base.OnDataChanged();

            RefreshLinePoints();
        }

        protected virtual void RefreshLinePoints()
        {
            LinePoints = CalculateLinePoints();
        }

        protected PointCollection CalculateLinePoints()
        {
            var newPoints = new PointCollection();

            var width = ActualWidth;
            var height = ActualHeight;

            if (width > 0 && height > 0)
            {
                var visualDataPoints = DataPoints;

                if (visualDataPoints == null) return newPoints;

                for (var i = 0; i < visualDataPoints.Count; i++)
                {
                    var dataPoint = visualDataPoints[i];

                    var relativeXPoint = XRange.GetRelativePoint(dataPoint.X);
                    var relativeYPoint = YRange.GetRelativePoint(dataPoint.Y);

                    var xCoordinate = width * relativeXPoint;
                    var yCoordinate = height - (height * relativeYPoint);

                    var newPoint = new Point(xCoordinate, yCoordinate);

                    newPoints.Add(newPoint);
                }
            }

            return newPoints;
        }

        protected override void OnUpdateIndicators()
        {
            if (_indicatorPanel == null) return;

            if (_indicatorPanel.Children.Count > 0)
            {
                foreach (IndicatorItem child in _indicatorPanel.Children)
                {
                    BindingOperations.ClearAllBindings(child);
                }

                _indicatorPanel.Children.Clear();
            }

            var visualDataPoints = DataPoints;

            if (visualDataPoints == null || visualDataPoints.Count == 0) return;

            var style = IndicatorStyle;
            var defaultBrush = IndicatorBrush;
            var showAllIndicators = ShowIndicators;
            var maxValue = visualDataPoints.Max(item => item.Y);
            var minValue = visualDataPoints.Min(item => item.Y);

            for (var i = 0; i < visualDataPoints.Count; i++)
            {
                var dataPoint = visualDataPoints[i];

                var brush = defaultBrush;
                var shouldAdd = showAllIndicators;
                var type = IndicatorType.Normal;

                if (ShowFirstPointIndicator && i == 0)
                {
                    brush = FirstPointBrush;
                    shouldAdd = true;
                    type = IndicatorType.First;
                }
                else if (ShowLastPointIndicator && i == visualDataPoints.Count - 1)
                {
                    brush = LastPointBrush;
                    shouldAdd = true;
                    type = IndicatorType.Last;
                }
                else if (ShowHighPointIndicators && dataPoint.Y == maxValue)
                {
                    brush = HighPointBrush;
                    shouldAdd = true;
                    type = IndicatorType.High;
                }
                else if (ShowLowPointIndicators && dataPoint.Y == minValue)
                {
                    brush = LowPointBrush;
                    shouldAdd = true;
                    type = IndicatorType.Low;
                }
                else if (ShowNegativePointIndicators && dataPoint.Y < 0)
                {
                    brush = NegativePointBrush;
                    shouldAdd = true;
                    type = IndicatorType.Negative;
                }

                if (!shouldAdd) continue;

                var relativeXPoint = XRange.GetRelativePoint(dataPoint.X);
                var relativeYPoint = YRange.GetRelativePoint(dataPoint.Y);

                var indicator = new IndicatorItem()
                {
                    DataContext = dataPoint,
                    RelativeX = relativeXPoint,
                    RelativeY = relativeYPoint,
                    Background = brush,
                    Type = type,
                    Style = style
                };

                indicator.SetBinding(IndicatorItem.ToolTipTemplateProperty, new Binding(nameof(ToolTipTemplate)) { Source = this });

                _indicatorPanel.Children.Add(indicator);
            }
        }
    }
}