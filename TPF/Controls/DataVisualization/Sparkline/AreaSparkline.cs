using System.Linq;
using System.Windows;
using System.Windows.Media;

namespace TPF.Controls
{
    public class AreaSparkline : LinearSparklineBase
    {
        static AreaSparkline()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(AreaSparkline), new FrameworkPropertyMetadata(typeof(AreaSparkline)));
        }

        public AreaSparkline()
        {
            AreaPoints = new PointCollection();
        }

        #region AreaPoints Readonly DependencyProperty
        internal static readonly DependencyPropertyKey AreaPointsPropertyKey = DependencyProperty.RegisterReadOnly("AreaPoints",
            typeof(PointCollection),
            typeof(AreaSparkline),
            new PropertyMetadata());

        public static readonly DependencyProperty AreaPointsProperty = AreaPointsPropertyKey.DependencyProperty;

        public PointCollection AreaPoints
        {
            get { return (PointCollection)GetValue(AreaPointsProperty); }
            private set { SetValue(AreaPointsPropertyKey, value); }
        }
        #endregion

        #region PositiveAreaStyle DependencyProperty
        public static readonly DependencyProperty PositiveAreaStyleProperty = DependencyProperty.Register("PositiveAreaStyle",
            typeof(Style),
            typeof(AreaSparkline),
            new PropertyMetadata(null));

        public Style PositiveAreaStyle
        {
            get { return (Style)GetValue(PositiveAreaStyleProperty); }
            set { SetValue(PositiveAreaStyleProperty, value); }
        }
        #endregion

        #region PositiveAreaBrush DependencyProperty
        public static readonly DependencyProperty PositiveAreaBrushProperty = DependencyProperty.Register("PositiveAreaBrush",
            typeof(Brush),
            typeof(AreaSparkline),
            new PropertyMetadata(null));

        public Brush PositiveAreaBrush
        {
            get { return (Brush)GetValue(PositiveAreaBrushProperty); }
            set { SetValue(PositiveAreaBrushProperty, value); }
        }
        #endregion

        #region PositiveAreaClip Readonly DependencyProperty
        internal static readonly DependencyPropertyKey PositiveAreaClipPropertyKey = DependencyProperty.RegisterReadOnly("PositiveAreaClip",
            typeof(RectangleGeometry),
            typeof(AreaSparkline),
            new PropertyMetadata());

        public static readonly DependencyProperty PositiveAreaClipProperty = PositiveAreaClipPropertyKey.DependencyProperty;

        public RectangleGeometry PositiveAreaClip
        {
            get { return (RectangleGeometry)GetValue(PositiveAreaClipProperty); }
            private set { SetValue(PositiveAreaClipPropertyKey, value); }
        }
        #endregion

        #region NegativeAreaStyle DependencyProperty
        public static readonly DependencyProperty NegativeAreaStyleProperty = DependencyProperty.Register("NegativeAreaStyle",
            typeof(Style),
            typeof(AreaSparkline),
            new PropertyMetadata(null));

        public Style NegativeAreaStyle
        {
            get { return (Style)GetValue(NegativeAreaStyleProperty); }
            set { SetValue(NegativeAreaStyleProperty, value); }
        }
        #endregion

        #region NegativeAreaBrush DependencyProperty
        public static readonly DependencyProperty NegativeAreaBrushProperty = DependencyProperty.Register("NegativeAreaBrush",
            typeof(Brush),
            typeof(AreaSparkline),
            new PropertyMetadata(null));

        public Brush NegativeAreaBrush
        {
            get { return (Brush)GetValue(NegativeAreaBrushProperty); }
            set { SetValue(NegativeAreaBrushProperty, value); }
        }
        #endregion

        #region NegativeAreaClip Readonly DependencyProperty
        internal static readonly DependencyPropertyKey NegativeAreaClipPropertyKey = DependencyProperty.RegisterReadOnly("NegativeAreaClip",
            typeof(RectangleGeometry),
            typeof(AreaSparkline),
            new PropertyMetadata());

        public static readonly DependencyProperty NegativeAreaClipProperty = NegativeAreaClipPropertyKey.DependencyProperty;

        public RectangleGeometry NegativeAreaClip
        {
            get { return (RectangleGeometry)GetValue(NegativeAreaClipProperty); }
            private set { SetValue(NegativeAreaClipPropertyKey, value); }
        }
        #endregion

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            UpdateAreaClip();
        }

        protected override void OnRenderSizeChanged(SizeChangedInfo sizeInfo)
        {
            base.OnRenderSizeChanged(sizeInfo);

            UpdateAreaClip();
        }

        protected override void OnDataChanged()
        {
            base.OnDataChanged();

            UpdateAreaClip();
        }

        protected override void RefreshLinePoints()
        {
            base.RefreshLinePoints();

            AreaPoints = CalculateAreaPoints();
        }

        protected PointCollection CalculateAreaPoints()
        {
            var newPoints = new PointCollection(LinePoints);

            if (newPoints.Count > 0)
            {
                var axisValue = AxisValue;
                var height = ActualHeight;
                double y;

                // Punkte nur Oberhalb von AxisValue
                if (YRange.End > axisValue && YRange.Start >= axisValue)
                {
                    y = height - (height * YRange.GetRelativePoint(YRange.Start));
                }// Punkte nur Unterhalb von AxisValue
                else if (YRange.End <= axisValue && YRange.Start < axisValue)
                {
                    y = height - (height * YRange.GetRelativePoint(YRange.End));
                }
                else
                {
                    y = height - (height * YRange.GetRelativePoint(axisValue));
                }

                var firstX = newPoints.First().X;
                var lastX = newPoints.Last().X;

                var firstPoint = new Point(firstX, y);
                var lastPoint = new Point(lastX, y);

                newPoints.Insert(0, firstPoint);
                newPoints.Add(lastPoint);
            }

            return newPoints;
        }

        private void UpdateAreaClip()
        {
            if (LinePoints.Count < 2) return;

            var yCoordinate = ActualHeight - (ActualHeight * YRange.GetRelativePoint(AxisValue));

            var positiveAreaRect = new Rect(0, 0, ActualWidth, yCoordinate);
            var negativeAreaRect = new Rect(0, yCoordinate, ActualWidth, ActualHeight * YRange.GetRelativePoint(AxisValue));

            PositiveAreaClip = new RectangleGeometry(positiveAreaRect);
            NegativeAreaClip = new RectangleGeometry(negativeAreaRect);
        }
    }
}