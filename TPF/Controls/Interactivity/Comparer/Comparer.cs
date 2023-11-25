using System.Windows;
using System.Windows.Controls;

namespace TPF.Controls
{
    public class Comparer : Control
    {
        static Comparer()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(Comparer), new FrameworkPropertyMetadata(typeof(Comparer)));
        }

        #region Value DependencyProperty
        public static readonly DependencyProperty ValueProperty = DependencyProperty.Register("Value",
            typeof(double),
            typeof(Comparer),
            new FrameworkPropertyMetadata(0.5, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, null, CoerceValue));

        private static object CoerceValue(DependencyObject sender, object value)
        {
            var number = (double)value;

            if (number < 0) number = 0;
            else if (number > 1) number = 1;
            
            return number;
        }

        public double Value
        {
            get { return (double)GetValue(ValueProperty); }
            set { SetValue(ValueProperty, value); }
        }
        #endregion

        #region Orientation DependencyProperty
        public static readonly DependencyProperty OrientationProperty = DependencyProperty.Register("Orientation",
            typeof(Orientation),
            typeof(Comparer),
            new PropertyMetadata(Orientation.Horizontal));

        public Orientation Orientation
        {
            get { return (Orientation)GetValue(OrientationProperty); }
            set { SetValue(OrientationProperty, value); }
        }
        #endregion

        #region FirstContent DependencyProperty
        public static readonly DependencyProperty FirstContentProperty = DependencyProperty.Register("FirstContent",
            typeof(object),
            typeof(Comparer),
            new PropertyMetadata(null));

        public object FirstContent
        {
            get { return GetValue(FirstContentProperty); }
            set { SetValue(FirstContentProperty, value); }
        }
        #endregion

        #region FirstContentTemplate DependencyProperty
        public static readonly DependencyProperty FirstContentTemplateProperty = DependencyProperty.Register("FirstContentTemplate",
            typeof(DataTemplate),
            typeof(Comparer),
            new PropertyMetadata(null));

        public DataTemplate FirstContentTemplate
        {
            get { return (DataTemplate)GetValue(FirstContentTemplateProperty); }
            set { SetValue(FirstContentTemplateProperty, value); }
        }
        #endregion

        #region FirstContentTemplateSelector DependencyProperty
        public static readonly DependencyProperty FirstContentTemplateSelectorProperty = DependencyProperty.Register("FirstContentTemplateSelector",
            typeof(DataTemplateSelector),
            typeof(Comparer),
            new PropertyMetadata(null));

        public DataTemplateSelector FirstContentTemplateSelector
        {
            get { return (DataTemplateSelector)GetValue(FirstContentTemplateSelectorProperty); }
            set { SetValue(FirstContentTemplateSelectorProperty, value); }
        }
        #endregion

        #region SecondContent DependencyProperty
        public static readonly DependencyProperty SecondContentProperty = DependencyProperty.Register("SecondContent",
            typeof(object),
            typeof(Comparer),
            new PropertyMetadata(null));

        public object SecondContent
        {
            get { return GetValue(SecondContentProperty); }
            set { SetValue(SecondContentProperty, value); }
        }
        #endregion

        #region SecondContentTemplate DependencyProperty
        public static readonly DependencyProperty SecondContentTemplateProperty = DependencyProperty.Register("SecondContentTemplate",
            typeof(DataTemplate),
            typeof(Comparer),
            new PropertyMetadata(null));

        public DataTemplate SecondContentTemplate
        {
            get { return (DataTemplate)GetValue(SecondContentTemplateProperty); }
            set { SetValue(SecondContentTemplateProperty, value); }
        }
        #endregion

        #region SecondContentTemplateSelector DependencyProperty
        public static readonly DependencyProperty SecondContentTemplateSelectorProperty = DependencyProperty.Register("SecondContentTemplateSelector",
            typeof(DataTemplateSelector),
            typeof(Comparer),
            new PropertyMetadata(null));

        public DataTemplateSelector SecondContentTemplateSelector
        {
            get { return (DataTemplateSelector)GetValue(SecondContentTemplateSelectorProperty); }
            set { SetValue(SecondContentTemplateSelectorProperty, value); }
        }
        #endregion

        #region ThumbStyle DependencyProperty
        public static readonly DependencyProperty ThumbStyleProperty = DependencyProperty.Register("ThumbStyle",
            typeof(Style),
            typeof(Comparer),
            new PropertyMetadata(null));

        public Style ThumbStyle
        {
            get { return (Style)GetValue(ThumbStyleProperty); }
            set { SetValue(ThumbStyleProperty, value); }
        }
        #endregion
    }
}