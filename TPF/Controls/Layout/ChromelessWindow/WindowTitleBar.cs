using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Controls;
using TPF.Internal;

namespace TPF.Controls
{
    public class WindowTitleBar : Control
    {
        static WindowTitleBar()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(WindowTitleBar), new FrameworkPropertyMetadata(typeof(WindowTitleBar)));
        }

        #region WindowState DependencyProperty
        public static readonly DependencyProperty WindowStateProperty = Window.WindowStateProperty.AddOwner(typeof(WindowTitleBar),
            new PropertyMetadata(WindowState.Normal));

        public WindowState WindowState
        {
            get { return (WindowState)GetValue(WindowStateProperty); }
            set { SetValue(WindowStateProperty, value); }
        }
        #endregion

        #region ResizeMode DependencyProperty
        public static readonly DependencyProperty ResizeModeProperty = Window.ResizeModeProperty.AddOwner(typeof(WindowTitleBar),
            new PropertyMetadata(ResizeMode.CanResize));

        public ResizeMode ResizeMode
        {
            get { return (ResizeMode)GetValue(ResizeModeProperty); }
            set { SetValue(ResizeModeProperty, value); }
        }
        #endregion

        #region Icon DependencyProperty
        public static readonly DependencyProperty IconProperty = DependencyProperty.Register("Icon",
            typeof(ImageSource),
            typeof(WindowTitleBar),
            new PropertyMetadata(null));

        public ImageSource Icon
        {
            get { return (ImageSource)GetValue(IconProperty); }
            set { SetValue(IconProperty, value); }
        }
        #endregion

        #region ShowIcon DependencyProperty
        public static readonly DependencyProperty ShowIconProperty = DependencyProperty.Register("ShowIcon",
            typeof(bool),
            typeof(WindowTitleBar),
            new PropertyMetadata(BooleanBoxes.TrueBox));

        public bool ShowIcon
        {
            get { return (bool)GetValue(ShowIconProperty); }
            set { SetValue(ShowIconProperty, BooleanBoxes.Box(value)); }
        }
        #endregion

        #region IconMargin DependencyProperty
        public static readonly DependencyProperty IconMarginProperty = DependencyProperty.Register("IconMargin",
            typeof(Thickness),
            typeof(WindowTitleBar),
            new PropertyMetadata(default(Thickness)));

        public Thickness IconMargin
        {
            get { return (Thickness)GetValue(IconMarginProperty); }
            set { SetValue(IconMarginProperty, value); }
        }
        #endregion

        #region Title DependencyProperty
        public static readonly DependencyProperty TitleProperty = DependencyProperty.Register("Title",
            typeof(string),
            typeof(WindowTitleBar),
            new PropertyMetadata(null));

        public string Title
        {
            get { return (string)GetValue(TitleProperty); }
            set { SetValue(TitleProperty, value); }
        }
        #endregion

        #region ShowTitle DependencyProperty
        public static readonly DependencyProperty ShowTitleProperty = DependencyProperty.Register("ShowTitle",
            typeof(bool),
            typeof(WindowTitleBar),
            new PropertyMetadata(BooleanBoxes.TrueBox));

        public bool ShowTitle
        {
            get { return (bool)GetValue(ShowTitleProperty); }
            set { SetValue(ShowTitleProperty, BooleanBoxes.Box(value)); }
        }
        #endregion

        #region TitleMargin DependencyProperty
        public static readonly DependencyProperty TitleMarginProperty = DependencyProperty.Register("TitleMargin",
            typeof(Thickness),
            typeof(WindowTitleBar),
            new PropertyMetadata(default(Thickness)));

        public Thickness TitleMargin
        {
            get { return (Thickness)GetValue(TitleMarginProperty); }
            set { SetValue(TitleMarginProperty, value); }
        }
        #endregion

        #region TitleAlignment DependencyProperty
        public static readonly DependencyProperty TitleAlignmentProperty = DependencyProperty.Register("TitleAlignment",
            typeof(HorizontalAlignment),
            typeof(WindowTitleBar),
            new PropertyMetadata(HorizontalAlignment.Left));

        public string TitleAlignment
        {
            get { return (string)GetValue(TitleAlignmentProperty); }
            set { SetValue(TitleAlignmentProperty, value); }
        }
        #endregion

        #region TitleFontFamily DependencyProperty
        public static readonly DependencyProperty TitleFontFamilyProperty = DependencyProperty.Register("TitleFontFamily",
            typeof(FontFamily),
            typeof(WindowTitleBar),
            new PropertyMetadata(SystemFonts.MessageFontFamily));

        public FontFamily TitleFontFamily
        {
            get { return (FontFamily)GetValue(TitleFontFamilyProperty); }
            set { SetValue(TitleFontFamilyProperty, value); }
        }
        #endregion

        #region TitleFontSize DependencyProperty
        public static readonly DependencyProperty TitleFontSizeProperty = DependencyProperty.Register("TitleFontSize",
            typeof(double),
            typeof(WindowTitleBar),
            new PropertyMetadata(SystemFonts.MessageFontSize));

        public double TitleFontSize
        {
            get { return (double)GetValue(TitleFontSizeProperty); }
            set { SetValue(TitleFontSizeProperty, value); }
        }
        #endregion

        #region MinimizeButtonStyle DependencyProperty
        public static readonly DependencyProperty MinimizeButtonStyleProperty = DependencyProperty.Register("MinimizeButtonStyle",
            typeof(Style),
            typeof(WindowTitleBar),
            new PropertyMetadata(null));

        public Style MinimizeButtonStyle
        {
            get { return (Style)GetValue(MinimizeButtonStyleProperty); }
            set { SetValue(MinimizeButtonStyleProperty, value); }
        }
        #endregion

        #region MaximizeButtonStyle DependencyProperty
        public static readonly DependencyProperty MaximizeButtonStyleProperty = DependencyProperty.Register("MaximizeButtonStyle",
            typeof(Style),
            typeof(WindowTitleBar),
            new PropertyMetadata(null));

        public Style MaximizeButtonStyle
        {
            get { return (Style)GetValue(MaximizeButtonStyleProperty); }
            set { SetValue(MaximizeButtonStyleProperty, value); }
        }
        #endregion

        #region RestoreButtonStyle DependencyProperty
        public static readonly DependencyProperty RestoreButtonStyleProperty = DependencyProperty.Register("RestoreButtonStyle",
            typeof(Style),
            typeof(WindowTitleBar),
            new PropertyMetadata(null));

        public Style RestoreButtonStyle
        {
            get { return (Style)GetValue(RestoreButtonStyleProperty); }
            set { SetValue(RestoreButtonStyleProperty, value); }
        }
        #endregion

        #region CloseButtonStyle DependencyProperty
        public static readonly DependencyProperty CloseButtonStyleProperty = DependencyProperty.Register("CloseButtonStyle",
            typeof(Style),
            typeof(WindowTitleBar),
            new PropertyMetadata(null));

        public Style CloseButtonStyle
        {
            get { return (Style)GetValue(CloseButtonStyleProperty); }
            set { SetValue(CloseButtonStyleProperty, value); }
        }
        #endregion

        #region ShowMinimizeButton DependencyProperty
        public static readonly DependencyProperty ShowMinimizeButtonProperty = DependencyProperty.Register("ShowMinimizeButton",
            typeof(bool),
            typeof(WindowTitleBar),
            new PropertyMetadata(BooleanBoxes.TrueBox));

        public bool ShowMinimizeButton
        {
            get { return (bool)GetValue(ShowMinimizeButtonProperty); }
            set { SetValue(ShowMinimizeButtonProperty, BooleanBoxes.Box(value)); }
        }
        #endregion

        #region ShowMaximizeButton DependencyProperty
        public static readonly DependencyProperty ShowMaximizeButtonProperty = DependencyProperty.Register("ShowMaximizeButton",
            typeof(bool),
            typeof(WindowTitleBar),
            new PropertyMetadata(BooleanBoxes.TrueBox));

        public bool ShowMaximizeButton
        {
            get { return (bool)GetValue(ShowMaximizeButtonProperty); }
            set { SetValue(ShowMaximizeButtonProperty, BooleanBoxes.Box(value)); }
        }
        #endregion

        #region ButtonsAreaMargin DependencyProperty
        public static readonly DependencyProperty ButtonsAreaMarginProperty = DependencyProperty.Register("ButtonsAreaMargin",
            typeof(Thickness),
            typeof(WindowTitleBar),
            new PropertyMetadata(default(Thickness)));

        public Thickness ButtonsAreaMargin
        {
            get { return (Thickness)GetValue(ButtonsAreaMarginProperty); }
            set { SetValue(ButtonsAreaMarginProperty, value); }
        }
        #endregion

        #region ExtendContentAreaIntoTitleBar DependencyProperty
        public static readonly DependencyProperty ExtendContentAreaIntoTitleBarProperty = DependencyProperty.Register("ExtendContentAreaIntoTitleBar",
            typeof(bool),
            typeof(WindowTitleBar),
            new PropertyMetadata(BooleanBoxes.FalseBox));

        public bool ExtendContentAreaIntoTitleBar
        {
            get { return (bool)GetValue(ExtendContentAreaIntoTitleBarProperty); }
            set { SetValue(ExtendContentAreaIntoTitleBarProperty, BooleanBoxes.Box(value)); }
        }
        #endregion

        #region LeftExtraTitleContent DependencyProperty
        public static readonly DependencyProperty LeftExtraTitleContentProperty = DependencyProperty.Register("LeftExtraTitleContent",
            typeof(object),
            typeof(WindowTitleBar),
            new PropertyMetadata(null));

        public object LeftExtraTitleContent
        {
            get { return GetValue(LeftExtraTitleContentProperty); }
            set { SetValue(LeftExtraTitleContentProperty, value); }
        }
        #endregion

        #region LeftExtraTitleContentTemplate DependencyProperty
        public static readonly DependencyProperty LeftExtraTitleContentTemplateProperty = DependencyProperty.Register("LeftExtraTitleContentTemplate",
            typeof(DataTemplate),
            typeof(WindowTitleBar),
            new PropertyMetadata(null));

        public DataTemplate LeftExtraTitleContentTemplate
        {
            get { return (DataTemplate)GetValue(LeftExtraTitleContentTemplateProperty); }
            set { SetValue(LeftExtraTitleContentTemplateProperty, value); }
        }
        #endregion

        #region LeftExtraTitleContentTemplateSelector DependencyProperty
        public static readonly DependencyProperty LeftExtraTitleContentTemplateSelectorProperty = DependencyProperty.Register("LeftExtraTitleContentTemplateSelector",
            typeof(DataTemplateSelector),
            typeof(WindowTitleBar),
            new PropertyMetadata(null));

        public DataTemplate LeftExtraTitleContentTemplateSelector
        {
            get { return (DataTemplate)GetValue(LeftExtraTitleContentTemplateSelectorProperty); }
            set { SetValue(LeftExtraTitleContentTemplateSelectorProperty, value); }
        }
        #endregion

        #region RightExtraTitleContent DependencyProperty
        public static readonly DependencyProperty RightExtraTitleContentProperty = DependencyProperty.Register("RightExtraTitleContent",
            typeof(object),
            typeof(WindowTitleBar),
            new PropertyMetadata(null));

        public object RightExtraTitleContent
        {
            get { return GetValue(RightExtraTitleContentProperty); }
            set { SetValue(RightExtraTitleContentProperty, value); }
        }
        #endregion

        #region RightExtraTitleContentTemplate DependencyProperty
        public static readonly DependencyProperty RightExtraTitleContentTemplateProperty = DependencyProperty.Register("RightExtraTitleContentTemplate",
            typeof(DataTemplate),
            typeof(WindowTitleBar),
            new PropertyMetadata(null));

        public DataTemplate RightExtraTitleContentTemplate
        {
            get { return (DataTemplate)GetValue(RightExtraTitleContentTemplateProperty); }
            set { SetValue(RightExtraTitleContentTemplateProperty, value); }
        }
        #endregion

        #region RightExtraTitleContentTemplateSelector DependencyProperty
        public static readonly DependencyProperty RightExtraTitleContentTemplateSelectorProperty = DependencyProperty.Register("RightExtraTitleContentTemplateSelector",
            typeof(DataTemplateSelector),
            typeof(WindowTitleBar),
            new PropertyMetadata(null));

        public DataTemplate RightExtraTitleContentTemplateSelector
        {
            get { return (DataTemplate)GetValue(RightExtraTitleContentTemplateSelectorProperty); }
            set { SetValue(RightExtraTitleContentTemplateSelectorProperty, value); }
        }
        #endregion

        #region CornerRadius DependencyProperty
        public static readonly DependencyProperty CornerRadiusProperty = DependencyProperty.Register("CornerRadius",
            typeof(CornerRadius),
            typeof(WindowTitleBar),
            new PropertyMetadata(default(CornerRadius)));

        public CornerRadius CornerRadius
        {
            get { return (CornerRadius)GetValue(CornerRadiusProperty); }
            set { SetValue(CornerRadiusProperty, value); }
        }
        #endregion

        internal ChromelessWindow ParentWindow;
    }
}