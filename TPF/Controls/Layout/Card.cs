using System.Windows;
using System.Windows.Controls;
using TPF.Internal;

namespace TPF.Controls
{
    public class Card : ContentControl
    {
        static Card()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(Card), new FrameworkPropertyMetadata(typeof(Card)));
        }

        #region CornerRadius DependencyProperty
        public static readonly DependencyProperty CornerRadiusProperty = DependencyProperty.Register("CornerRadius",
            typeof(CornerRadius),
            typeof(Card),
            new PropertyMetadata(default(CornerRadius)));

        public CornerRadius CornerRadius
        {
            get { return (CornerRadius)GetValue(CornerRadiusProperty); }
            set { SetValue(CornerRadiusProperty, value); }
        }
        #endregion

        #region ShadowDepth DependencyProperty
        public static readonly DependencyProperty ShadowDepthProperty = Shadow.ShadowDepthProperty.AddOwner(
            typeof(Card),
            new PropertyMetadata(ShadowDepth.Depth0));

        public ShadowDepth ShadowDepth
        {
            get { return (ShadowDepth)GetValue(ShadowDepthProperty); }
            set { SetValue(ShadowDepthProperty, value); }
        }
        #endregion

        #region Darken DependencyProperty
        public static readonly DependencyProperty DarkenProperty = Shadow.DarkenProperty.AddOwner(
            typeof(Card),
            new PropertyMetadata(BooleanBoxes.FalseBox));

        public bool Darken
        {
            get { return (bool)GetValue(DarkenProperty); }
            set { SetValue(DarkenProperty, BooleanBoxes.Box(value)); }
        }
        #endregion

        #region DarkenOnMouseOver DependencyProperty
        public static readonly DependencyProperty DarkenOnMouseOverProperty = DependencyProperty.Register("DarkenOnMouseOver",
            typeof(bool),
            typeof(Card),
            new PropertyMetadata(BooleanBoxes.FalseBox));

        public bool DarkenOnMouseOver
        {
            get { return (bool)GetValue(DarkenOnMouseOverProperty); }
            set { SetValue(DarkenOnMouseOverProperty, BooleanBoxes.Box(value)); }
        }
        #endregion
    }
}