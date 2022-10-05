using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using TPF.Internal;

namespace TPF.Controls
{
    public class RadialMenuNavigationButton : Control
    {
        static RadialMenuNavigationButton()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(RadialMenuNavigationButton), new FrameworkPropertyMetadata(typeof(RadialMenuNavigationButton)));
        }

        public RadialMenuNavigationButton()
        {
            IsEnabledChanged += (_, __) =>
            {
                UpdateVisualState();
            };
        }

        internal Path BackgroundPath;

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            BackgroundPath = (Path)GetTemplateChild("PART_Background");
        }

        protected virtual void UpdateVisualState()
        {
            UpdateVisualState(true);
        }

        protected virtual void UpdateVisualState(bool useTransitions)
        {
            if (!IsEnabled)
            {
                VisualStateManager.GoToState(this, "Disabled", useTransitions);
            }
            else if (IsMouseOver)
            {
                VisualStateManager.GoToState(this, "MouseOver", useTransitions);
            }
            else
            {
                VisualStateManager.GoToState(this, "Normal", useTransitions);
            }
        }

        protected override void OnMouseEnter(MouseEventArgs e)
        {
            base.OnMouseEnter(e);

            UpdateVisualState();
        }

        protected override void OnMouseLeave(MouseEventArgs e)
        {
            base.OnMouseLeave(e);

            UpdateVisualState();
        }

        internal void DrawBackground(double centerX, double centerY, double innerRadius, double outerRadius, double startAngle, double angleDelta, double padding)
        {
            if (BackgroundPath == null || angleDelta <= 0) return;

            // Wenn wir einen ganzen Kreis erstellen sollen, benutzen wir eine andere Geometry als bei einem Teilkreis
            var geometry = angleDelta >= 360.0 ? DrawFullCircleGeometry(centerX, centerY, innerRadius, outerRadius)
                                               : DrawPartialCircleGeometry(centerX, centerY, innerRadius, outerRadius, startAngle, angleDelta, padding);

            BackgroundPath.Data = geometry;
        }

        private static Geometry DrawFullCircleGeometry(double centerX, double centerY, double innerRadius, double outerRadius)
        {
            // Zwei Ellipsen erstellen und mit XOR kombinieren
            var geometry = new CombinedGeometry()
            {
                Geometry1 = new EllipseGeometry(new Point(centerX, centerY), outerRadius, outerRadius),
                Geometry2 = new EllipseGeometry(new Point(centerX, centerY), innerRadius, innerRadius),
                GeometryCombineMode = GeometryCombineMode.Xor
            };

            // Freeze für Performance
            geometry.Freeze();

            return geometry;
        }

        private static Geometry DrawPartialCircleGeometry(double centerX, double centerY, double innerRadius, double outerRadius, double startAngle, double angleDelta, double padding)
        {
            var geometry = new StreamGeometry();

            using (var context = geometry.Open())
            {
                var outerStartAngle = startAngle;
                var outerAngleDelta = angleDelta;
                var innerStartAngle = startAngle;
                var innerAngleDelta = angleDelta;
                var arcCenter = new Point(centerX, centerY);
                var outerArcSize = new Size(outerRadius, outerRadius);
                var innerArcSize = new Size(innerRadius, innerRadius);

                if (padding > 0)
                {
                    var outerAngleVariation = outerRadius == 0 ? 0 : 180 * (padding / outerRadius) / Math.PI;
                    var innerAngleVariation = innerRadius == 0 ? 0 : 180 * (padding / innerRadius) / Math.PI;

                    outerStartAngle += outerAngleVariation;
                    outerAngleDelta -= outerAngleVariation * 2;
                    innerStartAngle += innerAngleVariation;
                    innerAngleDelta -= innerAngleVariation * 2;
                }

                var outerArcStartPoint = Helper.ComputeCartesianCoordinate(arcCenter, outerStartAngle, outerRadius);
                var outerArcEndPoint = Helper.ComputeCartesianCoordinate(arcCenter, outerStartAngle + outerAngleDelta, outerRadius);
                var innerArcStartPoint = Helper.ComputeCartesianCoordinate(arcCenter, innerStartAngle, innerRadius);
                var innerArcEndPoint = Helper.ComputeCartesianCoordinate(arcCenter, innerStartAngle + innerAngleDelta, innerRadius);

                var largeArcOuter = outerAngleDelta > 180.0;
                var largeArcInner = innerAngleDelta > 180.0;

                context.BeginFigure(innerArcStartPoint, true, true);
                context.LineTo(outerArcStartPoint, true, true);
                context.ArcTo(outerArcEndPoint, outerArcSize, 0, largeArcOuter, SweepDirection.Clockwise, true, true);
                context.LineTo(innerArcEndPoint, true, true);
                context.ArcTo(innerArcStartPoint, innerArcSize, 0, largeArcInner, SweepDirection.Counterclockwise, true, true);
            }

            geometry.Freeze();

            return geometry;
        }
    }
}