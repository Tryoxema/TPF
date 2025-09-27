using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using TPF.Internal;

namespace TPF.Controls
{
    public class Callout : ContentControl
    {
        static Callout()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(Callout), new FrameworkPropertyMetadata(typeof(Callout)));
        }

        public Callout()
        {
            Loaded += Callout_Loaded;

            StrokeDashArray = new DoubleCollection();
        }

        #region CustomGeometry DependencyProperty
        public static readonly DependencyProperty CustomGeometryProperty = DependencyProperty.Register("CustomGeometry",
            typeof(Geometry),
            typeof(Callout),
            new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.AffectsMeasure));

        public Geometry CustomGeometry
        {
            get { return (Geometry)GetValue(CustomGeometryProperty); }
            set { SetValue(CustomGeometryProperty, value); }
        }
        #endregion

        #region ActualGeometry ReadOnly DependencyProperty
        internal static readonly DependencyPropertyKey ActualGeometryPropertyKey = DependencyProperty.RegisterReadOnly("ActualGeometry",
            typeof(Geometry),
            typeof(Callout),
            new PropertyMetadata(null));

        public static readonly DependencyProperty ActualGeometryProperty = ActualGeometryPropertyKey.DependencyProperty;

        public Geometry ActualGeometry
        {
            get { return (Geometry)GetValue(ActualGeometryProperty); }
            protected set { SetValue(ActualGeometryPropertyKey, value); }
        }
        #endregion

        #region Type DependencyProperty
        public static readonly DependencyProperty TypeProperty = DependencyProperty.Register("Type",
            typeof(CalloutType),
            typeof(Callout),
            new FrameworkPropertyMetadata(CalloutType.Rectangle, FrameworkPropertyMetadataOptions.AffectsMeasure));

        public CalloutType Type
        {
            get { return (CalloutType)GetValue(TypeProperty); }
            set { SetValue(TypeProperty, value); }
        }
        #endregion

        #region ArrowType DependencyProperty
        public static readonly DependencyProperty ArrowTypeProperty = DependencyProperty.Register("ArrowType",
            typeof(CalloutArrowType),
            typeof(Callout),
            new FrameworkPropertyMetadata(CalloutArrowType.Triangle, FrameworkPropertyMetadataOptions.AffectsMeasure));

        public CalloutArrowType ArrowType
        {
            get { return (CalloutArrowType)GetValue(ArrowTypeProperty); }
            set { SetValue(ArrowTypeProperty, value); }
        }
        #endregion

        #region ArrowBaseAnchorPoint1 DependencyProperty
        public static readonly DependencyProperty ArrowBaseAnchorPoint1Property = DependencyProperty.Register("ArrowBaseAnchorPoint1",
            typeof(Point),
            typeof(Callout),
            new FrameworkPropertyMetadata(new Point(0.25, 0.5), FrameworkPropertyMetadataOptions.AffectsMeasure));

        public Point ArrowBaseAnchorPoint1
        {
            get { return (Point)GetValue(ArrowBaseAnchorPoint1Property); }
            set { SetValue(ArrowBaseAnchorPoint1Property, value); }
        }
        #endregion

        #region ArrowBaseAnchorPoint2 DependencyProperty
        public static readonly DependencyProperty ArrowBaseAnchorPoint2Property = DependencyProperty.Register("ArrowBaseAnchorPoint2",
            typeof(Point),
            typeof(Callout),
            new FrameworkPropertyMetadata(new Point(0.75, 0.5), FrameworkPropertyMetadataOptions.AffectsMeasure));

        public Point ArrowBaseAnchorPoint2
        {
            get { return (Point)GetValue(ArrowBaseAnchorPoint2Property); }
            set { SetValue(ArrowBaseAnchorPoint2Property, value); }
        }
        #endregion

        #region ArrowTargetAnchorPoint DependencyProperty
        public static readonly DependencyProperty ArrowTargetAnchorPointProperty = DependencyProperty.Register("ArrowTargetAnchorPoint",
            typeof(Point),
            typeof(Callout),
            new FrameworkPropertyMetadata(new Point(0.5, 1.25), FrameworkPropertyMetadataOptions.AffectsMeasure));

        public Point ArrowTargetAnchorPoint
        {
            get { return (Point)GetValue(ArrowTargetAnchorPointProperty); }
            set { SetValue(ArrowTargetAnchorPointProperty, value); }
        }
        #endregion

        #region StrokeThickness DependencyProperty
        public static readonly DependencyProperty StrokeThicknessProperty = DependencyProperty.Register("StrokeThickness",
            typeof(double),
            typeof(Callout),
            new FrameworkPropertyMetadata(1.0, FrameworkPropertyMetadataOptions.AffectsMeasure));

        public double StrokeThickness
        {
            get { return (double)GetValue(StrokeThicknessProperty); }
            set { SetValue(StrokeThicknessProperty, value); }
        }
        #endregion

        #region StrokeDashArray DependencyProperty
        public static readonly DependencyProperty StrokeDashArrayProperty = DependencyProperty.Register("StrokeDashArray",
            typeof(DoubleCollection),
            typeof(Callout),
            new PropertyMetadata(null));

        public DoubleCollection StrokeDashArray
        {
            get { return (DoubleCollection)GetValue(StrokeDashArrayProperty); }
            set { SetValue(StrokeDashArrayProperty, value); }
        }
        #endregion

        #region CornerRadius DependencyProperty
        public static readonly DependencyProperty CornerRadiusProperty = DependencyProperty.Register("CornerRadius",
            typeof(double),
            typeof(Callout),
            new FrameworkPropertyMetadata(4.0, FrameworkPropertyMetadataOptions.AffectsMeasure));

        public double CornerRadius
        {
            get { return (double)GetValue(CornerRadiusProperty); }
            set { SetValue(CornerRadiusProperty, value); }
        }
        #endregion

        #region Stretch DependencyProperty
        public static readonly DependencyProperty StretchProperty = DependencyProperty.Register("Stretch",
            typeof(Stretch),
            typeof(Callout),
            new FrameworkPropertyMetadata(Stretch.None, FrameworkPropertyMetadataOptions.AffectsMeasure));

        public Stretch Stretch
        {
            get { return (Stretch)GetValue(StretchProperty); }
            set { SetValue(StretchProperty, value); }
        }
        #endregion

        private ContentPresenter _contentPresenter;
        private Size _targetSize;

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            _contentPresenter = GetTemplateChild("PART_ContentPresenter") as ContentPresenter;

            UpdateGeometry();
        }

        protected override Size MeasureOverride(Size constraint)
        {
            UpdateGeometry();

            return base.MeasureOverride(constraint);
        }

        private void UpdateGeometry()
        {
            if (!IsLoaded) return;

            UpdateTargetSize();

            var body = GetBodyGeometry();
            var arrow = GetArrowGeometry();

            ActualGeometry = new CombinedGeometry(body, arrow);
        }

        private void UpdateTargetSize()
        {
            var targetWidth = double.IsNaN(Width) ? MinWidth : Width;
            var targetHeight = double.IsNaN(Height) ? MinHeight : Height;

            if (!targetWidth.IsNumber()) targetWidth = 100d;
            if (!targetHeight.IsNumber()) targetHeight = 40d;

            if (_contentPresenter != null)
            {
                var measureWidth = double.IsNaN(Width) ? double.PositiveInfinity : Width;
                var measureHeight = double.IsNaN(Height) ? double.PositiveInfinity : Height;
                
                _contentPresenter.Measure(new Size(measureWidth, measureHeight));

                targetWidth = Math.Max(targetWidth, _contentPresenter.DesiredSize.Width);
                targetHeight = Math.Max(targetHeight, _contentPresenter.DesiredSize.Height);
            }

            _targetSize = new Size(targetWidth, targetHeight);
        }

        protected virtual Geometry GetBodyGeometry()
        {
            switch (Type)
            {
                case CalloutType.Rectangle:
                case CalloutType.RoundedRectangle:
                {
                    var geometry = new RectangleGeometry
                    {
                        Rect = new Rect(_targetSize)
                    };

                    if (Type == CalloutType.RoundedRectangle)
                    {
                        geometry.RadiusX = geometry.RadiusY = CornerRadius;
                    }

                    return geometry;
                }
                case CalloutType.Ellipse:
                {
                    var center = new Point(_targetSize.Width / 2, _targetSize.Height / 2);

                    var geometry = new EllipseGeometry()
                    {
                        Center = center,
                        RadiusX = center.X,
                        RadiusY = center.Y,
                    };

                    return geometry;
                }
                case CalloutType.Custom: return CustomGeometry;
                default: return null;
            }
        }

        protected virtual Geometry GetArrowGeometry()
        {
            switch (ArrowType)
            {
                case CalloutArrowType.Triangle:
                {
                    var geometry = new PathGeometry();
                    var figure = new PathFigure();
                    var basePoint1 = ArrowBaseAnchorPoint1;
                    var basePoint2 = ArrowBaseAnchorPoint2;
                    var targetPoint = ArrowTargetAnchorPoint;

                    figure.StartPoint = new Point(basePoint1.X * _targetSize.Width, basePoint1.Y * _targetSize.Height);
                    figure.Segments.Add(new LineSegment(new Point(basePoint2.X * _targetSize.Width, basePoint2.Y * _targetSize.Height), true));
                    figure.Segments.Add(new LineSegment(new Point(targetPoint.X * _targetSize.Width, targetPoint.Y * _targetSize.Height), true));

                    geometry.Figures.Add(figure);

                    return geometry;
                }
                case CalloutArrowType.None:
                default: return null;
            }
        }

        private void Callout_Loaded(object sender, RoutedEventArgs e)
        {
            UpdateGeometry();
        }
    }
}