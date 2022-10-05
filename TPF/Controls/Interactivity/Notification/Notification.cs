using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media;
using System.Windows.Media.Animation;
using TPF.Internal;

namespace TPF.Controls
{
    public class Notification : Control
    {
        static Notification()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(Notification), new FrameworkPropertyMetadata(typeof(Notification)));
        }

        public Notification()
        {
            Buttons = new ObservableCollection<ButtonBase>();
        }

        #region AccentBrush DependencyProperty
        public static readonly DependencyProperty AccentBrushProperty = DependencyProperty.Register("AccentBrush",
            typeof(Brush),
            typeof(Notification),
            new PropertyMetadata(null, OnAccentBrushChanged));

        private static void OnAccentBrushChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            var instance = (Notification)sender;

            if (instance.BadgeBackground == null) instance.BadgeBackground = (Brush)e.NewValue;

            if (instance.ButtonBackground == null) instance.ButtonBackground = (Brush)e.NewValue;
        }

        public Brush AccentBrush
        {
            get { return (Brush)GetValue(AccentBrushProperty); }
            set { SetValue(AccentBrushProperty, value); }
        }
        #endregion

        #region ButtonForeground DependencyProperty
        public static readonly DependencyProperty ButtonForegroundProperty = DependencyProperty.Register("ButtonForeground",
            typeof(Brush),
            typeof(Notification),
            new PropertyMetadata(null));

        public Brush ButtonForeground
        {
            get { return (Brush)GetValue(ButtonForegroundProperty); }
            set { SetValue(ButtonForegroundProperty, value); }
        }
        #endregion

        #region ButtonBackground DependencyProperty
        public static readonly DependencyProperty ButtonBackgroundProperty = DependencyProperty.Register("ButtonBackground",
            typeof(Brush),
            typeof(Notification),
            new PropertyMetadata(null));

        public Brush ButtonBackground
        {
            get { return (Brush)GetValue(ButtonBackgroundProperty); }
            set { SetValue(ButtonBackgroundProperty, value); }
        }
        #endregion

        #region Header DependencyProperty
        public static readonly DependencyProperty HeaderProperty = DependencyProperty.Register("Header",
            typeof(string),
            typeof(Notification),
            new PropertyMetadata(null, OnHeaderChanged));

        private static void OnHeaderChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            var instance = (Notification)sender;

            instance.HeaderVisibility = string.IsNullOrWhiteSpace((string)e.NewValue) ? Visibility.Collapsed : Visibility.Visible;
        }

        public string Header
        {
            get { return (string)GetValue(HeaderProperty); }
            set { SetValue(HeaderProperty, value); }
        }
        #endregion

        #region HeaderVisibility DependencyProperty
        public static readonly DependencyProperty HeaderVisibilityProperty = DependencyProperty.Register("HeaderVisibility",
            typeof(Visibility),
            typeof(Notification),
            new PropertyMetadata(VisibilityBoxes.CollapsedBox));

        public Visibility HeaderVisibility
        {
            get { return (Visibility)GetValue(HeaderVisibilityProperty); }
            set { SetValue(HeaderVisibilityProperty, VisibilityBoxes.Box(value)); }
        }
        #endregion

        #region Message DependencyProperty
        public static readonly DependencyProperty MessageProperty = DependencyProperty.Register("Message",
            typeof(string),
            typeof(Notification),
            new PropertyMetadata(null, OnMessageChanged));

        private static void OnMessageChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            var instance = (Notification)sender;

            instance.MessageVisibility = string.IsNullOrWhiteSpace((string)e.NewValue) ? Visibility.Collapsed : Visibility.Visible;
        }

        public string Message
        {
            get { return (string)GetValue(MessageProperty); }
            set { SetValue(MessageProperty, value); }
        }
        #endregion

        #region MessageVisibility DependencyProperty
        public static readonly DependencyProperty MessageVisibilityProperty = DependencyProperty.Register("MessageVisibility",
            typeof(Visibility),
            typeof(Notification),
            new PropertyMetadata(VisibilityBoxes.CollapsedBox));

        public Visibility MessageVisibility
        {
            get { return (Visibility)GetValue(MessageVisibilityProperty); }
            set { SetValue(MessageVisibilityProperty, VisibilityBoxes.Box(value)); }
        }
        #endregion

        #region BadgeText DependencyProperty
        public static readonly DependencyProperty BadgeTextProperty = DependencyProperty.Register("BadgeText",
            typeof(string),
            typeof(Notification),
            new PropertyMetadata(null, OnBadgeTextChanged));

        private static void OnBadgeTextChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            var instance = (Notification)sender;

            instance.BadgeVisibility = string.IsNullOrWhiteSpace((string)e.NewValue) ? Visibility.Collapsed : Visibility.Visible;
        }

        public string BadgeText
        {
            get { return (string)GetValue(BadgeTextProperty); }
            set { SetValue(BadgeTextProperty, value); }
        }
        #endregion

        #region BadgeForeground DependencyProperty
        public static readonly DependencyProperty BadgeForegroundProperty = DependencyProperty.Register("BadgeForeground",
            typeof(Brush),
            typeof(Notification),
            new PropertyMetadata(null));

        public Brush BadgeForeground
        {
            get { return (Brush)GetValue(BadgeForegroundProperty); }
            set { SetValue(BadgeForegroundProperty, value); }
        }
        #endregion

        #region BadgeBackground DependencyProperty
        public static readonly DependencyProperty BadgeBackgroundProperty = DependencyProperty.Register("BadgeBackground",
            typeof(Brush),
            typeof(Notification),
            new PropertyMetadata(null));

        public Brush BadgeBackground
        {
            get { return (Brush)GetValue(BadgeBackgroundProperty); }
            set { SetValue(BadgeBackgroundProperty, value); }
        }
        #endregion

        #region BadgeVisibility DependencyProperty
        public static readonly DependencyProperty BadgeVisibilityProperty = DependencyProperty.Register("BadgeVisibility",
            typeof(Visibility),
            typeof(Notification),
            new PropertyMetadata(VisibilityBoxes.CollapsedBox));

        public Visibility BadgeVisibility
        {
            get { return (Visibility)GetValue(BadgeVisibilityProperty); }
            set { SetValue(BadgeVisibilityProperty, VisibilityBoxes.Box(value)); }
        }
        #endregion

        #region Buttons DependencyProperty
        public static readonly DependencyProperty ButtonsProperty = DependencyProperty.Register("Buttons",
            typeof(ObservableCollection<ButtonBase>),
            typeof(Notification),
            new PropertyMetadata(null));

        public ObservableCollection<ButtonBase> Buttons
        {
            get { return (ObservableCollection<ButtonBase>)GetValue(ButtonsProperty); }
            set { SetValue(ButtonsProperty, value); }
        }
        #endregion

        #region OverlayContent DependencyProperty
        public static readonly DependencyProperty OverlayContentProperty = DependencyProperty.Register("OverlayContent",
            typeof(object),
            typeof(Notification),
            new PropertyMetadata(null));

        public object OverlayContent
        {
            get { return GetValue(OverlayContentProperty); }
            set { SetValue(OverlayContentProperty, value); }
        }
        #endregion

        #region AdditionalContentTop DependencyProperty
        public static readonly DependencyProperty AdditionalContentTopProperty = DependencyProperty.Register("AdditionalContentTop",
            typeof(object),
            typeof(Notification),
            new PropertyMetadata(null));

        public object AdditionalContentTop
        {
            get { return GetValue(AdditionalContentTopProperty); }
            set { SetValue(AdditionalContentTopProperty, value); }
        }
        #endregion

        #region AdditionalContentBottom DependencyProperty
        public static readonly DependencyProperty AdditionalContentBottomProperty = DependencyProperty.Register("AdditionalContentBottom",
            typeof(object),
            typeof(Notification),
            new PropertyMetadata(null));

        public object AdditionalContentBottom
        {
            get { return GetValue(AdditionalContentBottomProperty); }
            set { SetValue(AdditionalContentBottomProperty, value); }
        }
        #endregion

        #region AdditionalContentLeft DependencyProperty
        public static readonly DependencyProperty AdditionalContentLeftProperty = DependencyProperty.Register("AdditionalContentLeft",
            typeof(object),
            typeof(Notification),
            new PropertyMetadata(null));

        public object AdditionalContentLeft
        {
            get { return GetValue(AdditionalContentLeftProperty); }
            set { SetValue(AdditionalContentLeftProperty, value); }
        }
        #endregion

        #region AdditionalContentRight DependencyProperty
        public static readonly DependencyProperty AdditionalContentRightProperty = DependencyProperty.Register("AdditionalContentRight",
            typeof(object),
            typeof(Notification),
            new PropertyMetadata(null));

        public object AdditionalContentRight
        {
            get { return GetValue(AdditionalContentRightProperty); }
            set { SetValue(AdditionalContentRightProperty, value); }
        }
        #endregion

        #region UseAnimation DependencyProperty
        public static readonly DependencyProperty UseAnimationProperty = DependencyProperty.Register("UseAnimation",
            typeof(bool),
            typeof(Notification),
            new PropertyMetadata(BooleanBoxes.FalseBox));

        public bool UseAnimation
        {
            get { return (bool)GetValue(UseAnimationProperty); }
            set { SetValue(UseAnimationProperty, BooleanBoxes.Box(value)); }
        }
        #endregion

        #region AnimationIn DependencyProperty
        public static readonly DependencyProperty AnimationInProperty = DependencyProperty.Register("AnimationIn",
            typeof(AnimationTimeline),
            typeof(Notification),
            new PropertyMetadata(null));

        public AnimationTimeline AnimationIn
        {
            get { return (AnimationTimeline)GetValue(AnimationInProperty); }
            set { SetValue(AnimationInProperty, value); }
        }
        #endregion

        #region AnimationOut DependencyProperty
        public static readonly DependencyProperty AnimationOutProperty = DependencyProperty.Register("AnimationOut",
            typeof(AnimationTimeline),
            typeof(Notification),
            new PropertyMetadata(null));

        public AnimationTimeline AnimationOut
        {
            get { return (AnimationTimeline)GetValue(AnimationOutProperty); }
            set { SetValue(AnimationOutProperty, value); }
        }
        #endregion

        #region AnimationInDependencyProperty DependencyProperty
        public static readonly DependencyProperty AnimationInDependencyPropertyProperty = DependencyProperty.Register("AnimationInDependencyProperty",
            typeof(DependencyProperty),
            typeof(Notification),
            new PropertyMetadata(null));

        public DependencyProperty AnimationInDependencyProperty
        {
            get { return (DependencyProperty)GetValue(AnimationInDependencyPropertyProperty); }
            set { SetValue(AnimationInDependencyPropertyProperty, value); }
        }
        #endregion

        #region AnimationOutDependencyProperty DependencyProperty
        public static readonly DependencyProperty AnimationOutDependencyPropertyProperty = DependencyProperty.Register("AnimationOutDependencyProperty",
            typeof(DependencyProperty),
            typeof(Notification),
            new PropertyMetadata(null));

        public DependencyProperty AnimationOutDependencyProperty
        {
            get { return (DependencyProperty)GetValue(AnimationOutDependencyPropertyProperty); }
            set { SetValue(AnimationOutDependencyPropertyProperty, value); }
        }
        #endregion

        #region AnimationInDuration DependencyProperty
        public static readonly DependencyProperty AnimationInDurationProperty = DependencyProperty.Register("AnimationInDuration",
            typeof(double),
            typeof(Notification),
            new PropertyMetadata(0.25));

        public double AnimationInDuration
        {
            get { return (double)GetValue(AnimationInDurationProperty); }
            set { SetValue(AnimationInDurationProperty, value); }
        }
        #endregion

        #region AnimationOutDuration DependencyProperty
        public static readonly DependencyProperty AnimationOutDurationProperty = DependencyProperty.Register("AnimationOutDuration",
            typeof(double),
            typeof(Notification),
            new PropertyMetadata(0.25));

        public double AnimationOutDuration
        {
            get { return (double)GetValue(AnimationOutDurationProperty); }
            set { SetValue(AnimationOutDurationProperty, value); }
        }
        #endregion
    }
}