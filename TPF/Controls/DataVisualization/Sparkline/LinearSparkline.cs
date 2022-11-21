using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;
using TPF.Internal;

namespace TPF.Controls
{
    public class LinearSparkline : SparklineBase
    {
        static LinearSparkline()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(LinearSparkline), new FrameworkPropertyMetadata(typeof(LinearSparkline)));
        }

        public LinearSparkline()
        {
            LinePoints = new PointCollection();
        }

        #region LineStyle DependencyProperty
        public static readonly DependencyProperty LineStyleProperty = DependencyProperty.Register("LineStyle",
            typeof(Style),
            typeof(LinearSparkline),
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
            typeof(LinearSparkline),
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
            typeof(LinearSparkline),
            new PropertyMetadata());

        public static readonly DependencyProperty LinePointsProperty = LinePointsPropertyKey.DependencyProperty;

        public PointCollection LinePoints
        {
            get { return (PointCollection)GetValue(LinePointsProperty); }
            private set { SetValue(LinePointsPropertyKey, value); }
        }
        #endregion

        #region NormalRangeBrush DependencyProperty
        public static readonly DependencyProperty NormalRangeBrushProperty = DependencyProperty.Register("NormalRangeBrush",
            typeof(Brush),
            typeof(LinearSparkline),
            new PropertyMetadata(null));

        public Brush NormalRangeBrush
        {
            get { return (Brush)GetValue(NormalRangeBrushProperty); }
            set { SetValue(NormalRangeBrushProperty, value); }
        }
        #endregion

        #region ShowNormalRange DependencyProperty
        public static readonly DependencyProperty ShowNormalRangeProperty = DependencyProperty.Register("ShowNormalRange",
            typeof(bool),
            typeof(LinearSparkline),
            new PropertyMetadata(BooleanBoxes.FalseBox, ShowNormalRangePropertyChanged));

        private static void ShowNormalRangePropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            var instance = (LinearSparkline)sender;

            instance.UpdateNormalRangeVisibility();
        }

        public bool ShowNormalRange
        {
            get { return (bool)GetValue(ShowNormalRangeProperty); }
            set { SetValue(ShowNormalRangeProperty, BooleanBoxes.Box(value)); }
        }
        #endregion

        #region NormalRangeTop DependencyProperty
        public static readonly DependencyProperty NormalRangeTopProperty = DependencyProperty.Register("NormalRangeTop",
            typeof(double),
            typeof(LinearSparkline),
            new PropertyMetadata(double.NaN, NormalRangePropertyChanged));

        public double NormalRangeTop
        {
            get { return (double)GetValue(NormalRangeTopProperty); }
            set { SetValue(NormalRangeTopProperty, value); }
        }
        #endregion

        #region NormalRangeBottom DependencyProperty
        public static readonly DependencyProperty NormalRangeBottomProperty = DependencyProperty.Register("NormalRangeBottom",
            typeof(double),
            typeof(LinearSparkline),
            new PropertyMetadata(double.NaN, NormalRangePropertyChanged));

        public double NormalRangeBottom
        {
            get { return (double)GetValue(NormalRangeBottomProperty); }
            set { SetValue(NormalRangeBottomProperty, value); }
        }
        #endregion

        private static void NormalRangePropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            var instance = (LinearSparkline)sender;

            instance.UpdateNormalRange();
        }

        private Path _normalRange;

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            _normalRange = GetTemplateChild("PART_NormalRange") as Path;

            UpdateNormalRangeVisibility();
        }

        protected override void OnRenderSizeChanged(SizeChangedInfo sizeInfo)
        {
            base.OnRenderSizeChanged(sizeInfo);

            RefreshLinePoints();
            UpdateNormalRange();
        }

        protected override void OnDataChanged()
        {
            base.OnDataChanged();

            RefreshLinePoints();
            UpdateNormalRange();
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

                foreach (var dataPoint in visualDataPoints)
                {
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

        private void UpdateNormalRangeVisibility()
        {
            if (_normalRange == null) return;

            _normalRange.Visibility = ShowNormalRange ? Visibility.Visible : Visibility.Collapsed;
        }

        private void UpdateNormalRange()
        {
            if (_normalRange == null) return;

            _normalRange.Data = null;

            var rangeTop = NormalRangeTop;
            var rangeBottom = NormalRangeBottom;

            if (double.IsNaN(rangeTop) && double.IsNaN(rangeBottom)) return;

            if (!YRange.Contains(rangeTop) && !YRange.Contains(rangeBottom)) return;

            var size = RenderSize;
            double top, bottom;

            top = Math.Max(rangeTop, rangeBottom);
            bottom = Math.Min(rangeTop, rangeBottom);

            top = Math.Min(YRange.End, top);
            bottom = Math.Max(YRange.Start, bottom);

            var topY = size.Height - (YRange.GetRelativePoint(top) * size.Height);
            var bottomY = size.Height - (YRange.GetRelativePoint(bottom) * size.Height);

            var rect = new Rect(0, topY, size.Width, bottomY - topY);

            _normalRange.Data = new RectangleGeometry(rect);
        }
    }
}