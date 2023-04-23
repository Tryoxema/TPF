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

        #region RadiusRatio DependencyProperty
        public static readonly DependencyProperty RadiusRatioProperty = DependencyProperty.Register("RadiusRatio",
            typeof(double),
            typeof(RadialPanel),
            new FrameworkPropertyMetadata(0.5, FrameworkPropertyMetadataOptions.AffectsArrange, null, ConstrainRadiusRatio));

        private static object ConstrainRadiusRatio(DependencyObject d, object baseValue)
        {
            var doubleValue = (double)baseValue;

            if (doubleValue < 0) doubleValue = 0;
            else if (doubleValue > 1) doubleValue = 1;

            return doubleValue;
        }

        public double RadiusRatio
        {
            get { return (double)GetValue(RadiusRatioProperty); }
            set { SetValue(RadiusRatioProperty, value); }
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

            var start = StartAngle * (Math.PI / 180);
            var anglePerItem = TotalAngle / Children.Count;
            var radiansPerItem = anglePerItem * (Math.PI / 180);
            var radiusX = finalSize.Width * 0.5;
            var radiusY = finalSize.Height * 0.5;
            var hypotenuseRadius = radiusX * RadiusRatio;

            for (int i = 0; i < Children.Count; i++)
            {
                var child = Children[i];

                var adjacent = Math.Cos((i * radiansPerItem) + start) * hypotenuseRadius;
                var opposite = Math.Sin((i * radiansPerItem) + start) * hypotenuseRadius;

                var buttonCentreX = radiusX + opposite;
                var buttonCentreY = radiusY - adjacent;

                var buttonX = buttonCentreX - child.DesiredSize.Width / 2;
                var buttonY = buttonCentreY - child.DesiredSize.Height / 2;

                var rect = new Rect(buttonX, buttonY, child.DesiredSize.Width, child.DesiredSize.Height);

                child.Arrange(rect);
            }

            return finalSize;
        }
    }
}