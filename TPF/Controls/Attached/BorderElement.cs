using System.Windows;

namespace TPF.Controls
{
    public class BorderElement
    {
        #region CornerRadius Attached DependencyProperty
        public static readonly DependencyProperty CornerRadiusProperty = DependencyProperty.RegisterAttached("CornerRadius",
            typeof(CornerRadius),
            typeof(BorderElement),
            new PropertyMetadata(default(CornerRadius)));

        public static CornerRadius GetCornerRadius(DependencyObject element)
        {
            return (CornerRadius)element.GetValue(CornerRadiusProperty);
        }

        public static void SetCornerRadius(DependencyObject element, CornerRadius value)
        {
            element.SetValue(CornerRadiusProperty, value);
        }
        #endregion
    }
}