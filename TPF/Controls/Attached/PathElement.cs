using System.Windows;
using System.Windows.Media;

namespace TPF.Controls
{
    public class PathElement
    {
        #region Data Attached DependencyProperty
        public static readonly DependencyProperty DataProperty = DependencyProperty.RegisterAttached("Data",
            typeof(Geometry),
            typeof(PathElement),
            new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.Inherits));

        public static Geometry GetData(DependencyObject element)
        {
            return (Geometry)element.GetValue(DataProperty);
        }

        public static void SetData(DependencyObject element, Geometry value)
        {
            element.SetValue(DataProperty, value);
        }
        #endregion

        #region Stroke Attached DependencyProperty
        public static readonly DependencyProperty StrokeProperty = DependencyProperty.RegisterAttached("Stroke",
            typeof(Brush),
            typeof(PathElement),
            new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.Inherits));

        public static Brush GetStroke(DependencyObject element)
        {
            return (Brush)element.GetValue(StrokeProperty);
        }

        public static void SetStroke(DependencyObject element, Brush value)
        {
            element.SetValue(StrokeProperty, value);
        }
        #endregion

        #region Fill Attached DependencyProperty
        public static readonly DependencyProperty FillProperty = DependencyProperty.RegisterAttached("Fill",
            typeof(Brush),
            typeof(PathElement),
            new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.Inherits));

        public static Brush GetFill(DependencyObject element)
        {
            return (Brush)element.GetValue(FillProperty);
        }

        public static void SetFill(DependencyObject element, Brush value)
        {
            element.SetValue(FillProperty, value);
        }
        #endregion
    }
}