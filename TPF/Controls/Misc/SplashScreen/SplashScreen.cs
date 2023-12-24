using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using TPF.Internal;

namespace TPF.Controls
{
    public class SplashScreen : Control
    {
        static SplashScreen()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(SplashScreen), new FrameworkPropertyMetadata(typeof(SplashScreen)));
        }

        #region Title DependencyProperty
        public static readonly DependencyProperty TitleProperty = DependencyProperty.Register("Title",
            typeof(string),
            typeof(SplashScreen),
            new PropertyMetadata(null, OnTitlePropertyChanged));

        private static void OnTitlePropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            var instance = (SplashScreen)sender;

            instance.UpdateHasTitle();
        }

        public string Title
        {
            get { return (string)GetValue(TitleProperty); }
            set { SetValue(TitleProperty, value); }
        }
        #endregion

        #region SubTitle DependencyProperty
        public static readonly DependencyProperty SubTitleProperty = DependencyProperty.Register("SubTitle",
            typeof(string),
            typeof(SplashScreen),
            new PropertyMetadata(null, OnSubTitlePropertyChanged));

        private static void OnSubTitlePropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            var instance = (SplashScreen)sender;

            instance.UpdateHasTitle();
        }

        public string SubTitle
        {
            get { return (string)GetValue(SubTitleProperty); }
            set { SetValue(SubTitleProperty, value); }
        }
        #endregion

        #region HasTitle ReadOnly DependencyProperty
        private static readonly DependencyPropertyKey HasTitlePropertyKey = DependencyProperty.RegisterReadOnly("HasTitle",
            typeof(bool),
            typeof(SplashScreen),
            new PropertyMetadata(BooleanBoxes.FalseBox));

        public static readonly DependencyProperty HasTitleProperty = HasTitlePropertyKey.DependencyProperty;

        public bool HasTitle
        {
            get { return (bool)GetValue(HasTitleProperty); }
            internal set { SetValue(HasTitlePropertyKey, BooleanBoxes.Box(value)); }
        }
        #endregion

        #region StatusText DependencyProperty
        public static readonly DependencyProperty StatusTextProperty = DependencyProperty.Register("StatusText",
            typeof(string),
            typeof(SplashScreen),
            new PropertyMetadata(null));

        public string StatusText
        {
            get { return (string)GetValue(StatusTextProperty); }
            set { SetValue(StatusTextProperty, value); }
        }
        #endregion

        #region Footer DependencyProperty
        public static readonly DependencyProperty FooterProperty = DependencyProperty.Register("Footer",
            typeof(object),
            typeof(SplashScreen),
            new PropertyMetadata(null));

        public object Footer
        {
            get { return GetValue(FooterProperty); }
            set { SetValue(FooterProperty, value); }
        }
        #endregion

        #region HorizontalStatusAlignment DependencyProperty
        public static readonly DependencyProperty HorizontalStatusAlignmentProperty = DependencyProperty.Register("HorizontalStatusAlignment",
            typeof(HorizontalAlignment),
            typeof(SplashScreen),
            new PropertyMetadata(HorizontalAlignment.Center));

        public HorizontalAlignment HorizontalStatusAlignment
        {
            get { return (HorizontalAlignment)GetValue(HorizontalStatusAlignmentProperty); }
            set { SetValue(HorizontalStatusAlignmentProperty, value); }
        }
        #endregion

        #region HorizontalFooterAlignment DependencyProperty
        public static readonly DependencyProperty HorizontalFooterAlignmentProperty = DependencyProperty.Register("HorizontalFooterAlignment",
            typeof(HorizontalAlignment),
            typeof(SplashScreen),
            new PropertyMetadata(HorizontalAlignment.Left));

        public HorizontalAlignment HorizontalFooterAlignment
        {
            get { return (HorizontalAlignment)GetValue(HorizontalFooterAlignmentProperty); }
            set { SetValue(HorizontalFooterAlignmentProperty, value); }
        }
        #endregion

        #region ProgressBarVisibility DependencyProperty
        public static readonly DependencyProperty ProgressBarVisibilityProperty = DependencyProperty.Register("ProgressBarVisibility",
            typeof(Visibility),
            typeof(SplashScreen),
            new PropertyMetadata(Visibility.Visible));

        public Visibility ProgressBarVisibility
        {
            get { return (Visibility)GetValue(ProgressBarVisibilityProperty); }
            set { SetValue(ProgressBarVisibilityProperty, value); }
        }
        #endregion

        #region Progress DependencyProperty
        public static readonly DependencyProperty ProgressProperty = DependencyProperty.Register("Progress",
            typeof(double),
            typeof(SplashScreen),
            new PropertyMetadata(0d));

        public double Progress
        {
            get { return (double)GetValue(ProgressProperty); }
            set { SetValue(ProgressProperty, value); }
        }
        #endregion

        #region SecondaryProgress DependencyProperty
        public static readonly DependencyProperty SecondaryProgressProperty = DependencyProperty.Register("SecondaryProgress",
            typeof(double),
            typeof(SplashScreen),
            new PropertyMetadata(0d));

        public double SecondaryProgress
        {
            get { return (double)GetValue(SecondaryProgressProperty); }
            set { SetValue(SecondaryProgressProperty, value); }
        }
        #endregion

        #region ProgressMinimum DependencyProperty
        public static readonly DependencyProperty ProgressMinimumProperty = DependencyProperty.Register("ProgressMinimum",
            typeof(double),
            typeof(SplashScreen),
            new PropertyMetadata(0d));

        public double ProgressMinimum
        {
            get { return (double)GetValue(ProgressMinimumProperty); }
            set { SetValue(ProgressMinimumProperty, value); }
        }
        #endregion

        #region ProgressMaximum DependencyProperty
        public static readonly DependencyProperty ProgressMaximumProperty = DependencyProperty.Register("ProgressMaximum",
            typeof(double),
            typeof(SplashScreen),
            new PropertyMetadata(100d));

        public double ProgressMaximum
        {
            get { return (double)GetValue(ProgressMaximumProperty); }
            set { SetValue(ProgressMaximumProperty, value); }
        }
        #endregion

        #region IsIndeterminate DependencyProperty
        public static readonly DependencyProperty IsIndeterminateProperty = DependencyProperty.Register("IsIndeterminate",
            typeof(bool),
            typeof(SplashScreen),
            new PropertyMetadata(BooleanBoxes.TrueBox));

        public bool IsIndeterminate
        {
            get { return (bool)GetValue(IsIndeterminateProperty); }
            set { SetValue(IsIndeterminateProperty, BooleanBoxes.Box(value)); }
        }
        #endregion

        #region Logo DependencyProperty
        public static readonly DependencyProperty LogoProperty = DependencyProperty.Register("Logo",
            typeof(Uri),
            typeof(SplashScreen),
            new PropertyMetadata(null));

        public Uri Logo
        {
            get { return (Uri)GetValue(LogoProperty); }
            set { SetValue(LogoProperty, value); }
        }
        #endregion

        #region LogoWidth DependencyProperty
        public static readonly DependencyProperty LogoWidthProperty = DependencyProperty.Register("LogoWidth",
            typeof(double),
            typeof(SplashScreen),
            new PropertyMetadata(double.NaN));

        public double LogoWidth
        {
            get { return (double)GetValue(LogoWidthProperty); }
            set { SetValue(LogoWidthProperty, value); }
        }
        #endregion

        #region LogoHeight DependencyProperty
        public static readonly DependencyProperty LogoHeightProperty = DependencyProperty.Register("LogoHeight",
            typeof(double),
            typeof(SplashScreen),
            new PropertyMetadata(double.NaN));

        public double LogoHeight
        {
            get { return (double)GetValue(LogoHeightProperty); }
            set { SetValue(LogoHeightProperty, value); }
        }
        #endregion

        #region LogoStretch DependencyProperty
        public static readonly DependencyProperty LogoStretchProperty = DependencyProperty.Register("LogoStretch",
            typeof(Stretch),
            typeof(SplashScreen),
            new PropertyMetadata(Stretch.None));

        public Stretch LogoStretch
        {
            get { return (Stretch)GetValue(LogoStretchProperty); }
            set { SetValue(LogoStretchProperty, value); }
        }
        #endregion

        #region LogoPosition DependencyProperty
        public static readonly DependencyProperty LogoPositionProperty = DependencyProperty.Register("LogoPosition",
            typeof(SplashScreenLogoPosition),
            typeof(SplashScreen),
            new PropertyMetadata(SplashScreenLogoPosition.TopLeft));

        public SplashScreenLogoPosition LogoPosition
        {
            get { return (SplashScreenLogoPosition)GetValue(LogoPositionProperty); }
            set { SetValue(LogoPositionProperty, value); }
        }
        #endregion

        private void UpdateHasTitle()
        {
            HasTitle = !string.IsNullOrWhiteSpace(Title) || !string.IsNullOrWhiteSpace(SubTitle);
        }
    }
}