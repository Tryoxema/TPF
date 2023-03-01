using System.Windows;
using System.Windows.Media;
using System.Windows.Controls.Primitives;

namespace TPF.Controls
{
    public class Shield : ButtonBase
    {
        static Shield()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(Shield), new FrameworkPropertyMetadata(typeof(Shield)));
        }

        #region ImageSource DependencyProperty
        public static readonly DependencyProperty ImageSourceProperty = DependencyProperty.Register("ImageSource",
            typeof(ImageSource),
            typeof(Shield),
            new PropertyMetadata(null));

        public ImageSource ImageSource
        {
            get { return (ImageSource)GetValue(ImageSourceProperty); }
            set { SetValue(ImageSourceProperty, value); }
        }
        #endregion

        #region ImageWidth DependencyProperty
        public static readonly DependencyProperty ImageWidthProperty = DependencyProperty.Register("ImageWidth",
            typeof(double),
            typeof(Shield),
            new PropertyMetadata(20.0));

        public double ImageWidth
        {
            get { return (double)GetValue(ImageWidthProperty); }
            set { SetValue(ImageWidthProperty, value); }
        }
        #endregion

        #region ImageHeight DependencyProperty
        public static readonly DependencyProperty ImageHeightProperty = DependencyProperty.Register("ImageHeight",
            typeof(double),
            typeof(Shield),
            new PropertyMetadata(20.0));

        public double ImageHeight
        {
            get { return (double)GetValue(ImageHeightProperty); }
            set { SetValue(ImageHeightProperty, value); }
        }
        #endregion

        #region Label DependencyProperty
        public static readonly DependencyProperty LabelProperty = DependencyProperty.Register("Label",
            typeof(string),
            typeof(Shield),
            new PropertyMetadata(null));

        public string Label
        {
            get { return (string)GetValue(LabelProperty); }
            set { SetValue(LabelProperty, value); }
        }
        #endregion

        #region LabelForeground DependencyProperty
        public static readonly DependencyProperty LabelForegroundProperty = DependencyProperty.Register("LabelForeground",
            typeof(Brush),
            typeof(Shield),
            new PropertyMetadata(null));

        public Brush LabelForeground
        {
            get { return (Brush)GetValue(LabelForegroundProperty); }
            set { SetValue(LabelForegroundProperty, value); }
        }
        #endregion

        #region LabelBackground DependencyProperty
        public static readonly DependencyProperty LabelBackgroundProperty = DependencyProperty.Register("LabelBackground",
            typeof(Brush),
            typeof(Shield),
            new PropertyMetadata(null));

        public Brush LabelBackground
        {
            get { return (Brush)GetValue(LabelBackgroundProperty); }
            set { SetValue(LabelBackgroundProperty, value); }
        }
        #endregion

        #region CornerRadius DependencyProperty
        public static readonly DependencyProperty CornerRadiusProperty = DependencyProperty.Register("CornerRadius",
            typeof(CornerRadius),
            typeof(Shield),
            new PropertyMetadata(default(CornerRadius)));

        public CornerRadius CornerRadius
        {
            get { return (CornerRadius)GetValue(CornerRadiusProperty); }
            set { SetValue(CornerRadiusProperty, value); }
        }
        #endregion

        public override string ToString()
        {
            if (string.IsNullOrWhiteSpace(Label) && Content == null) return base.ToString();
            else
            {
                if (string.IsNullOrWhiteSpace(Label)) return $"{base.ToString()} {Content}";
                if (Content == null) return $"{base.ToString()} {Label}";
                return $"{base.ToString()} {Label} {Content}";
            }
        }
    }
}