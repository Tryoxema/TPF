using System;
using System.Windows;
using System.Windows.Controls;

namespace TPF.Controls
{
    public class RadialPanel : Panel
    {
        #region TotalAngle DependencyProperty
        public static readonly DependencyProperty TotalAngleProperty = DependencyProperty.Register("TotalAngle",
            typeof(double),
            typeof(RadialPanel),
            new FrameworkPropertyMetadata(360.0, FrameworkPropertyMetadataOptions.AffectsArrange));

        public double TotalAngle
        {
            get { return (double)GetValue(TotalAngleProperty); }
            set { SetValue(TotalAngleProperty, value); }
        }
        #endregion

        #region StartAngle DependencyProperty
        public static readonly DependencyProperty StartAngleProperty = DependencyProperty.Register("StartAngle",
            typeof(double),
            typeof(RadialPanel),
            new FrameworkPropertyMetadata(0.0, FrameworkPropertyMetadataOptions.AffectsArrange));

        public double StartAngle
        {
            get { return (double)GetValue(StartAngleProperty); }
            set { SetValue(StartAngleProperty, value); }
        }
        #endregion

        protected override Size MeasureOverride(Size availableSize)
        {
            foreach (UIElement child in Children)
            {
                child.Measure(availableSize);
            }

            return base.MeasureOverride(availableSize);
        }

        protected override Size ArrangeOverride(Size finalSize)
        {
            if (Children.Count == 0) return finalSize;

            var startAngle = StartAngle * (Math.PI / 180);
            var anglePerChild = TotalAngle / Children.Count * (Math.PI / 180);
            var radiusX = finalSize.Width * 0.5;
            var radiusY = finalSize.Height * 0.5;
            var startPointX = radiusX + 1 * Math.Sin(startAngle) * radiusX / 2;
            var startPointY = radiusY / 2 + (1 - Math.Cos(startAngle)) * radiusY / 2;
            var currentPosition = new Point(startPointX, startPointY);

            for (int i = 0; i < Children.Count; i++)
            {
                var child = Children[i];

                var angle = (i + 1) * anglePerChild + startAngle;
                var offsetX = Math.Sin(angle) * radiusX / 2;
                var offsetY = (1 - Math.Cos(angle)) * radiusY / 2;

                var childRect = new Rect(new Point(currentPosition.X - child.DesiredSize.Width / 2,
                                                   currentPosition.Y - child.DesiredSize.Height / 2),
                                         new Point(currentPosition.X + child.DesiredSize.Width / 2,
                                                   currentPosition.Y + child.DesiredSize.Height / 2));

                child.Arrange(childRect);
                currentPosition.X = 1 * offsetX + radiusX;
                currentPosition.Y = offsetY + radiusY / 2;
            }

            return finalSize;
        }
    }
}