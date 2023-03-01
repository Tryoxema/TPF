using System.Windows;
using System.Windows.Media;

namespace TPF.Controls
{
    public class PathButton : Button
    {
        static PathButton()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(PathButton), new FrameworkPropertyMetadata(typeof(PathButton)));
        }

        #region Data DependencyProperty
        public static readonly DependencyProperty DataProperty = DependencyProperty.Register("Data",
            typeof(Geometry),
            typeof(PathButton),
            new PropertyMetadata(null));

        public Geometry Data
        {
            get { return (Geometry)GetValue(DataProperty); }
            set { SetValue(DataProperty, value); }
        }
        #endregion

        #region Stroke DependencyProperty
        public static readonly DependencyProperty StrokeProperty = DependencyProperty.Register("Stroke",
            typeof(Brush),
            typeof(PathButton),
            new PropertyMetadata(null));

        public Brush Stroke
        {
            get { return (Brush)GetValue(StrokeProperty); }
            set { SetValue(StrokeProperty, value); }
        }
        #endregion

        #region StrokeThickness DependencyProperty
        public static readonly DependencyProperty StrokeThicknessProperty = DependencyProperty.Register("StrokeThickness",
            typeof(double),
            typeof(PathButton),
            new PropertyMetadata(1.0));

        public double StrokeThickness
        {
            get { return (double)GetValue(StrokeThicknessProperty); }
            set { SetValue(StrokeThicknessProperty, value); }
        }
        #endregion

        #region Fill DependencyProperty
        public static readonly DependencyProperty FillProperty = DependencyProperty.Register("Fill",
            typeof(Brush),
            typeof(PathButton),
            new PropertyMetadata(null));

        public Brush Fill
        {
            get { return (Brush)GetValue(FillProperty); }
            set { SetValue(FillProperty, value); }
        }
        #endregion

        #region Stretch DependencyProperty
        public static readonly DependencyProperty StretchProperty = DependencyProperty.Register("Stretch",
            typeof(Stretch),
            typeof(PathButton),
            new PropertyMetadata(Stretch.None));

        public Stretch Stretch
        {
            get { return (Stretch)GetValue(StretchProperty); }
            set { SetValue(StretchProperty, value); }
        }
        #endregion
    }
}