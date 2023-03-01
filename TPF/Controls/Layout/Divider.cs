using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Markup;

namespace TPF.Controls
{
    [ContentProperty("Content")]
    public class Divider : Control
    {
        static Divider()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(Divider), new FrameworkPropertyMetadata(typeof(Divider)));
        }

        public Divider()
        {
            LineStrokeDashArray = new DoubleCollection();
        }

        #region Content DependencyProperty
        public static readonly DependencyProperty ContentProperty = DependencyProperty.Register("Content",
            typeof(object),
            typeof(Divider),
            new PropertyMetadata(null));

        public object Content
        {
            get { return GetValue(ContentProperty); }
            set { SetValue(ContentProperty, value); }
        }
        #endregion

        #region ContentTemplate DependencyProperty
        public static readonly DependencyProperty ContentTemplateProperty = DependencyProperty.Register("ContentTemplate",
            typeof(DataTemplate),
            typeof(Divider),
            new PropertyMetadata(null));

        public DataTemplate ContentTemplate
        {
            get { return (DataTemplate)GetValue(ContentTemplateProperty); }
            set { SetValue(ContentTemplateProperty, value); }
        }
        #endregion

        #region ContentTemplateSelector DependencyProperty
        public static readonly DependencyProperty ContentTemplateSelectorProperty = DependencyProperty.Register("ContentTemplateSelector",
            typeof(DataTemplateSelector),
            typeof(Divider),
            new PropertyMetadata(null));

        public DataTemplateSelector ContentTemplateSelector
        {
            get { return (DataTemplateSelector)GetValue(ContentTemplateSelectorProperty); }
            set { SetValue(ContentTemplateSelectorProperty, value); }
        }
        #endregion

        #region ContentStringFormat DependencyProperty
        public static readonly DependencyProperty ContentStringFormatProperty = DependencyProperty.Register("ContentStringFormat",
            typeof(string),
            typeof(Divider),
            new PropertyMetadata(null));

        public string ContentStringFormat
        {
            get { return (string)GetValue(ContentStringFormatProperty); }
            set { SetValue(ContentStringFormatProperty, value); }
        }
        #endregion

        #region Orientation DependencyProperty
        public static readonly DependencyProperty OrientationProperty = DependencyProperty.Register("Orientation",
            typeof(Orientation),
            typeof(Divider),
            new PropertyMetadata(Orientation.Horizontal));

        public Orientation Orientation
        {
            get { return (Orientation)GetValue(OrientationProperty); }
            set { SetValue(OrientationProperty, value); }
        }
        #endregion

        #region LineStroke DependencyProperty
        public static readonly DependencyProperty LineStrokeProperty = DependencyProperty.Register("LineStroke",
            typeof(Brush),
            typeof(Divider),
            new PropertyMetadata(null));

        public Brush LineStroke
        {
            get { return (Brush)GetValue(LineStrokeProperty); }
            set { SetValue(LineStrokeProperty, value); }
        }
        #endregion

        #region LineStrokeThickness DependencyProperty
        public static readonly DependencyProperty LineStrokeThicknessProperty = DependencyProperty.Register("LineStrokeThickness",
            typeof(double),
            typeof(Divider),
            new PropertyMetadata(1.0));

        public double LineStrokeThickness
        {
            get { return (double)GetValue(LineStrokeThicknessProperty); }
            set { SetValue(LineStrokeThicknessProperty, value); }
        }
        #endregion

        #region LineStrokeDashArray DependencyProperty
        public static readonly DependencyProperty LineStrokeDashArrayProperty = DependencyProperty.Register("LineStrokeDashArray",
            typeof(DoubleCollection),
            typeof(Divider),
            new PropertyMetadata(null));

        public DoubleCollection LineStrokeDashArray
        {
            get { return (DoubleCollection)GetValue(LineStrokeDashArrayProperty); }
            set { SetValue(LineStrokeDashArrayProperty, value); }
        }
        #endregion
    }
}