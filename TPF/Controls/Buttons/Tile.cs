using System.Windows;

namespace TPF.Controls
{
    public class Tile : Button
    {
        static Tile()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(Tile), new FrameworkPropertyMetadata(typeof(Tile)));
        }

        #region Title DependencyProperty
        public static readonly DependencyProperty TitleProperty = DependencyProperty.Register("Title",
            typeof(string),
            typeof(Tile),
            new PropertyMetadata(null));

        public string Title
        {
            get { return (string)GetValue(TitleProperty); }
            set { SetValue(TitleProperty, value); }
        }
        #endregion

        #region HorizontalTitleAlignment DependencyProperty
        public static readonly DependencyProperty HorizontalTitleAlignmentProperty = DependencyProperty.Register("HorizontalTitleAlignment",
            typeof(HorizontalAlignment),
            typeof(Tile),
            new PropertyMetadata(HorizontalAlignment.Left));

        public HorizontalAlignment HorizontalTitleAlignment
        {
            get { return (HorizontalAlignment)GetValue(HorizontalTitleAlignmentProperty); }
            set { SetValue(HorizontalTitleAlignmentProperty, value); }
        }
        #endregion

        #region VerticalTitleAlignment DependencyProperty
        public static readonly DependencyProperty VerticalTitleAlignmentProperty = DependencyProperty.Register("VerticalTitleAlignment",
            typeof(VerticalAlignment),
            typeof(Tile),
            new PropertyMetadata(VerticalAlignment.Bottom));

        public VerticalAlignment VerticalTitleAlignment
        {
            get { return (VerticalAlignment)GetValue(VerticalTitleAlignmentProperty); }
            set { SetValue(VerticalTitleAlignmentProperty, value); }
        }
        #endregion

        #region TitleFontSize DependencyProperty
        public static readonly DependencyProperty TitleFontSizeProperty = DependencyProperty.Register("TitleFontSize",
            typeof(double),
            typeof(Tile),
            new PropertyMetadata(16.0));

        public double TitleFontSize
        {
            get { return (double)GetValue(TitleFontSizeProperty); }
            set { SetValue(TitleFontSizeProperty, value); }
        }
        #endregion
    }
}