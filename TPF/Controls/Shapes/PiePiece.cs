using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;
using TPF.Internal;

namespace TPF.Controls
{
    public class PiePiece : Shape
    {
        #region InnerRadius DependencyProperty
        public static readonly DependencyProperty InnerRadiusProperty = DependencyProperty.Register("InnerRadius",
            typeof(double),
            typeof(PiePiece),
            new FrameworkPropertyMetadata(0.0, FrameworkPropertyMetadataOptions.AffectsRender | FrameworkPropertyMetadataOptions.AffectsMeasure));

        public double InnerRadius
        {
            get { return (double)GetValue(InnerRadiusProperty); }
            set { SetValue(InnerRadiusProperty, value); }
        }
        #endregion

        #region OuterRadius DependencyProperty
        public static readonly DependencyProperty OuterRadiusProperty = DependencyProperty.Register("OuterRadius",
            typeof(double),
            typeof(PiePiece),
            new FrameworkPropertyMetadata(0.0, FrameworkPropertyMetadataOptions.AffectsRender | FrameworkPropertyMetadataOptions.AffectsMeasure));

        public double OuterRadius
        {
            get { return (double)GetValue(OuterRadiusProperty); }
            set { SetValue(OuterRadiusProperty, value); }
        }
        #endregion

        #region Padding DependencyProperty
        public static readonly DependencyProperty PaddingProperty = DependencyProperty.Register("Padding",
            typeof(double),
            typeof(PiePiece),
            new FrameworkPropertyMetadata(0.0, FrameworkPropertyMetadataOptions.AffectsRender | FrameworkPropertyMetadataOptions.AffectsMeasure));

        public double Padding
        {
            get { return (double)GetValue(PaddingProperty); }
            set { SetValue(PaddingProperty, value); }
        }
        #endregion

        #region PushOut DependencyProperty
        public static readonly DependencyProperty PushOutProperty = DependencyProperty.Register("PushOut",
            typeof(double),
            typeof(PiePiece),
            new FrameworkPropertyMetadata(0.0, FrameworkPropertyMetadataOptions.AffectsRender | FrameworkPropertyMetadataOptions.AffectsMeasure));

        public double PushOut
        {
            get { return (double)GetValue(PushOutProperty); }
            set { SetValue(PushOutProperty, value); }
        }
        #endregion

        #region AngleDelta DependencyProperty
        public static readonly DependencyProperty AngleDeltaProperty = DependencyProperty.Register("AngleDelta",
            typeof(double),
            typeof(PiePiece),
            new FrameworkPropertyMetadata(0.0, FrameworkPropertyMetadataOptions.AffectsRender | FrameworkPropertyMetadataOptions.AffectsMeasure));

        public double AngleDelta
        {
            get { return (double)GetValue(AngleDeltaProperty); }
            set { SetValue(AngleDeltaProperty, value); }
        }
        #endregion

        #region StartAngle DependencyProperty
        public static readonly DependencyProperty StartAngleProperty = DependencyProperty.Register("StartAngle",
            typeof(double),
            typeof(PiePiece),
            new FrameworkPropertyMetadata(0.0, FrameworkPropertyMetadataOptions.AffectsRender | FrameworkPropertyMetadataOptions.AffectsMeasure));

        public double StartAngle
        {
            get { return (double)GetValue(StartAngleProperty); }
            set { SetValue(StartAngleProperty, value); }
        }
        #endregion

        #region CenterX DependencyProperty
        public static readonly DependencyProperty CenterXProperty = DependencyProperty.Register("CenterX",
            typeof(double),
            typeof(PiePiece),
            new FrameworkPropertyMetadata(0.0, FrameworkPropertyMetadataOptions.AffectsRender | FrameworkPropertyMetadataOptions.AffectsMeasure));

        public double CenterX
        {
            get { return (double)GetValue(CenterXProperty); }
            set { SetValue(CenterXProperty, value); }
        }
        #endregion

        #region CenterY DependencyProperty
        public static readonly DependencyProperty CenterYProperty = DependencyProperty.Register("CenterY",
            typeof(double),
            typeof(PiePiece),
            new FrameworkPropertyMetadata(0.0, FrameworkPropertyMetadataOptions.AffectsRender | FrameworkPropertyMetadataOptions.AffectsMeasure));

        public double CenterY
        {
            get { return (double)GetValue(CenterYProperty); }
            set { SetValue(CenterYProperty, value); }
        }
        #endregion

        protected override Geometry DefiningGeometry
        {
            get
            {
                var geometry = new StreamGeometry { FillRule = FillRule.EvenOdd };

                using (var context = geometry.Open())
                {
                    DrawGeometry(context);
                }

                // Freeze für Performance
                geometry.Freeze();

                return geometry;
            }
        }

        private void DrawGeometry(StreamGeometryContext context)
        {
            if (AngleDelta <= 0) return;

            var outerStartAngle = StartAngle;
            var outerAngleDelta = AngleDelta;
            var innerStartAngle = StartAngle;
            var innerAngleDelta = AngleDelta;
            var arcCenter = new Point(CenterX, CenterY);
            var outerArcSize = new Size(OuterRadius, OuterRadius);
            var innerArcSize = new Size(InnerRadius, InnerRadius);

            if (AngleDelta >= 360)
            {
                var outerArcTopPoint = Helper.ComputeCartesianCoordinate(arcCenter, outerStartAngle, OuterRadius + PushOut);
                var outerArcBottomPoint = Helper.ComputeCartesianCoordinate(arcCenter, outerStartAngle + 180, OuterRadius + PushOut);
                var innerArcTopPoint = Helper.ComputeCartesianCoordinate(arcCenter, innerStartAngle, InnerRadius + PushOut);
                var innerArcBottomPoint = Helper.ComputeCartesianCoordinate(arcCenter, innerStartAngle + 180, InnerRadius + PushOut);

                context.BeginFigure(innerArcTopPoint, true, true);
                context.LineTo(outerArcTopPoint, true, true);
                context.ArcTo(outerArcBottomPoint, outerArcSize, 0, false, SweepDirection.Clockwise, true, true);
                context.ArcTo(outerArcTopPoint, outerArcSize, 0, false, SweepDirection.Clockwise, true, true);
                context.LineTo(innerArcTopPoint, true, true);
                context.ArcTo(innerArcBottomPoint, innerArcSize, 0, false, SweepDirection.Counterclockwise, true, true);
                context.ArcTo(innerArcTopPoint, innerArcSize, 0, false, SweepDirection.Counterclockwise, true, true);
            }
            else
            {
                if (Padding > 0)
                {
                    var outerAngleVariation = OuterRadius == 0 ? 0 : 180 * (Padding / OuterRadius) / Math.PI;
                    var innerAngleVariation = InnerRadius == 0 ? 0 : 180 * (Padding / InnerRadius) / Math.PI;

                    outerStartAngle += outerAngleVariation;
                    outerAngleDelta -= outerAngleVariation * 2;
                    innerStartAngle += innerAngleVariation;
                    innerAngleDelta -= innerAngleVariation * 2;
                }

                var outerArcStartPoint = Helper.ComputeCartesianCoordinate(arcCenter, outerStartAngle, OuterRadius + PushOut);
                var outerArcEndPoint = Helper.ComputeCartesianCoordinate(arcCenter, outerStartAngle + outerAngleDelta, OuterRadius + PushOut);
                var innerArcStartPoint = Helper.ComputeCartesianCoordinate(arcCenter, innerStartAngle, InnerRadius + PushOut);
                var innerArcEndPoint = Helper.ComputeCartesianCoordinate(arcCenter, innerStartAngle + innerAngleDelta, InnerRadius + PushOut);

                var largeArcOuter = outerAngleDelta > 180.0;
                var largeArcInner = innerAngleDelta > 180.0;

                context.BeginFigure(innerArcStartPoint, true, true);
                context.LineTo(outerArcStartPoint, true, true);
                context.ArcTo(outerArcEndPoint, outerArcSize, 0, largeArcOuter, SweepDirection.Clockwise, true, true);
                context.LineTo(innerArcEndPoint, true, true);
                context.ArcTo(innerArcStartPoint, innerArcSize, 0, largeArcInner, SweepDirection.Counterclockwise, true, true);
            }
        }
    }
}