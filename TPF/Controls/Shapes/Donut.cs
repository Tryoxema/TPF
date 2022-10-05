using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;

namespace TPF.Controls
{
    public class Donut : Shape
    {
        #region InnerRadius DependencyProperty
        public static readonly DependencyProperty InnerRadiusProperty = DependencyProperty.Register("InnerRadius",
            typeof(double),
            typeof(Donut),
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
            typeof(Donut),
            new FrameworkPropertyMetadata(0.0, FrameworkPropertyMetadataOptions.AffectsRender | FrameworkPropertyMetadataOptions.AffectsMeasure));

        public double OuterRadius
        {
            get { return (double)GetValue(OuterRadiusProperty); }
            set { SetValue(OuterRadiusProperty, value); }
        }
        #endregion

        #region CenterX DependencyProperty
        public static readonly DependencyProperty CenterXProperty = DependencyProperty.Register("CenterX",
            typeof(double),
            typeof(Donut),
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
            typeof(Donut),
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
                // Zwei Ellipsen erstellen und mit XOR kombinieren
                var geometry = new CombinedGeometry()
                {
                    Geometry1 = new EllipseGeometry(new Point(CenterX, CenterY), OuterRadius, OuterRadius),
                    Geometry2 = new EllipseGeometry(new Point(CenterX, CenterY), InnerRadius, InnerRadius),
                    GeometryCombineMode = GeometryCombineMode.Xor
                };

                // Freeze für Performance
                geometry.Freeze();

                return geometry;
            }
        }
    }
}