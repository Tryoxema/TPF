using System.Windows.Controls;
using System.Windows;
using TPF.Internal;

namespace TPF.Controls
{
    public class WizardPage : HeaderedContentControl
    {
        static WizardPage()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(WizardPage), new FrameworkPropertyMetadata(typeof(WizardPage)));
        }

        #region HeaderHeight DependencyProperty
        public static readonly DependencyProperty HeaderHeightProperty = DependencyProperty.Register("HeaderHeight",
            typeof(double),
            typeof(WizardPage),
            new PropertyMetadata(60.0));

        public double HeaderHeight
        {
            get { return (double)GetValue(HeaderHeightProperty); }
            set { SetValue(HeaderHeightProperty, value); }
        }
        #endregion

        #region HeaderVisibility DependencyProperty
        public static readonly DependencyProperty HeaderVisibilityProperty = DependencyProperty.Register("HeaderVisibility",
            typeof(Visibility),
            typeof(WizardPage),
            new PropertyMetadata(VisibilityBoxes.VisibleBox));

        public Visibility HeaderVisibility
        {
            get { return (Visibility)GetValue(HeaderVisibilityProperty); }
            set { SetValue(HeaderVisibilityProperty, VisibilityBoxes.Box(value)); }
        }
        #endregion

        #region HeaderBorderHeight DependencyProperty
        public static readonly DependencyProperty HeaderBorderHeightProperty = DependencyProperty.Register("HeaderBorderHeight",
            typeof(double),
            typeof(WizardPage),
            new PropertyMetadata(1.0));

        public double HeaderBorderHeight
        {
            get { return (double)GetValue(HeaderBorderHeightProperty); }
            set { SetValue(HeaderBorderHeightProperty, value); }
        }
        #endregion

        #region HeaderPadding DependencyProperty
        public static readonly DependencyProperty HeaderPaddingProperty = DependencyProperty.Register("HeaderPadding",
            typeof(Thickness),
            typeof(WizardPage),
            new PropertyMetadata(default(Thickness)));

        public Thickness HeaderPadding
        {
            get { return (Thickness)GetValue(HeaderPaddingProperty); }
            set { SetValue(HeaderPaddingProperty, value); }
        }
        #endregion

        #region SideContentWidth DependencyProperty
        public static readonly DependencyProperty SideContentWidthProperty = DependencyProperty.Register("SideContentWidth",
            typeof(double),
            typeof(WizardPage),
            new PropertyMetadata(160.0));

        public double SideContentWidth
        {
            get { return (double)GetValue(SideContentWidthProperty); }
            set { SetValue(SideContentWidthProperty, value); }
        }
        #endregion

        #region SideContentBorderWidth DependencyProperty
        public static readonly DependencyProperty SideContentBorderWidthProperty = DependencyProperty.Register("SideContentBorderWidth",
            typeof(double),
            typeof(WizardPage),
            new PropertyMetadata(1.0));

        public double SideContentBorderWidth
        {
            get { return (double)GetValue(SideContentBorderWidthProperty); }
            set { SetValue(SideContentBorderWidthProperty, value); }
        }
        #endregion

        #region SideContentPadding DependencyProperty
        public static readonly DependencyProperty SideContentPaddingProperty = DependencyProperty.Register("SideContentPadding",
            typeof(Thickness),
            typeof(WizardPage),
            new PropertyMetadata(default(Thickness)));

        public Thickness SideContentPadding
        {
            get { return (Thickness)GetValue(SideContentPaddingProperty); }
            set { SetValue(SideContentPaddingProperty, value); }
        }
        #endregion

        #region SideContent DependencyProperty
        public static readonly DependencyProperty SideContentProperty = DependencyProperty.Register("SideContent",
            typeof(object),
            typeof(WizardPage),
            new PropertyMetadata(null));

        public object SideContent
        {
            get { return GetValue(SideContentProperty); }
            set { SetValue(SideContentProperty, value); }
        }
        #endregion

        #region SideContentTemplate DependencyProperty
        public static readonly DependencyProperty SideContentTemplateProperty = DependencyProperty.Register("SideContentTemplate",
            typeof(DataTemplate),
            typeof(WizardPage),
            new PropertyMetadata(null));

        public DataTemplate SideContentTemplate
        {
            get { return (DataTemplate)GetValue(SideContentTemplateProperty); }
            set { SetValue(SideContentTemplateProperty, value); }
        }
        #endregion

        #region SideContentTemplateSelector DependencyProperty
        public static readonly DependencyProperty SideContentTemplateSelectorProperty = DependencyProperty.Register("SideContentTemplateSelector",
            typeof(DataTemplateSelector),
            typeof(WizardPage),
            new PropertyMetadata(null));

        public DataTemplateSelector SideContentTemplateSelector
        {
            get { return (DataTemplateSelector)GetValue(SideContentTemplateSelectorProperty); }
            set { SetValue(SideContentTemplateSelectorProperty, value); }
        }
        #endregion

        #region SideContentVisibility DependencyProperty
        public static readonly DependencyProperty SideContentVisibilityProperty = DependencyProperty.Register("SideContentVisibility",
            typeof(Visibility),
            typeof(WizardPage),
            new PropertyMetadata(VisibilityBoxes.VisibleBox));

        public Visibility SideContentVisibility
        {
            get { return (Visibility)GetValue(SideContentVisibilityProperty); }
            set { SetValue(SideContentVisibilityProperty, VisibilityBoxes.Box(value)); }
        }
        #endregion

        #region CanGoToPrevious DependencyProperty
        public static readonly DependencyProperty CanGoToPreviousProperty = DependencyProperty.Register("CanGoToPrevious",
            typeof(bool),
            typeof(WizardPage),
            new PropertyMetadata(BooleanBoxes.TrueBox, OnGoToPreviousButtonPropertyChanged));

        public bool CanGoToPrevious
        {
            get { return (bool)GetValue(CanGoToPreviousProperty); }
            set { SetValue(CanGoToPreviousProperty, BooleanBoxes.Box(value)); }
        }
        #endregion

        #region ShowGoToPreviousButton DependencyProperty
        public static readonly DependencyProperty ShowGoToPreviousButtonProperty = DependencyProperty.Register("ShowGoToPreviousButton",
            typeof(bool),
            typeof(WizardPage),
            new PropertyMetadata(BooleanBoxes.TrueBox, OnGoToPreviousButtonPropertyChanged));

        public bool ShowGoToPreviousButton
        {
            get { return (bool)GetValue(ShowGoToPreviousButtonProperty); }
            set { SetValue(ShowGoToPreviousButtonProperty, BooleanBoxes.Box(value)); }
        }
        #endregion

        #region GoToPreviousButtonContent DependencyProperty
        public static readonly DependencyProperty GoToPreviousButtonContentProperty = DependencyProperty.Register("GoToPreviousButtonContent",
            typeof(object),
            typeof(WizardPage),
            new PropertyMetadata("Zurück", OnGoToPreviousButtonPropertyChanged));

        public object GoToPreviousButtonContent
        {
            get { return GetValue(GoToPreviousButtonContentProperty); }
            set { SetValue(GoToPreviousButtonContentProperty, value); }
        }
        #endregion

        #region CanGoToNext DependencyProperty
        public static readonly DependencyProperty CanGoToNextProperty = DependencyProperty.Register("CanGoToNext",
            typeof(bool),
            typeof(WizardPage),
            new PropertyMetadata(BooleanBoxes.TrueBox, OnGoToNextButtonPropertyChanged));

        public bool CanGoToNext
        {
            get { return (bool)GetValue(CanGoToNextProperty); }
            set { SetValue(CanGoToNextProperty, BooleanBoxes.Box(value)); }
        }
        #endregion

        #region ShowGoToNextButton DependencyProperty
        public static readonly DependencyProperty ShowGoToNextButtonProperty = DependencyProperty.Register("ShowGoToNextButton",
            typeof(bool),
            typeof(WizardPage),
            new PropertyMetadata(BooleanBoxes.TrueBox, OnGoToNextButtonPropertyChanged));

        public bool ShowGoToNextButton
        {
            get { return (bool)GetValue(ShowGoToNextButtonProperty); }
            set { SetValue(ShowGoToNextButtonProperty, BooleanBoxes.Box(value)); }
        }
        #endregion

        #region GoToNextButtonContent DependencyProperty
        public static readonly DependencyProperty GoToNextButtonContentProperty = DependencyProperty.Register("GoToNextButtonContent",
            typeof(object),
            typeof(WizardPage),
            new PropertyMetadata("Weiter", OnGoToNextButtonPropertyChanged));

        public object GoToNextButtonContent
        {
            get { return GetValue(GoToNextButtonContentProperty); }
            set { SetValue(GoToNextButtonContentProperty, value); }
        }
        #endregion

        #region CanFinish DependencyProperty
        public static readonly DependencyProperty CanFinishProperty = DependencyProperty.Register("CanFinish",
            typeof(bool),
            typeof(WizardPage),
            new PropertyMetadata(BooleanBoxes.FalseBox, OnFinishButtonPropertyChanged));

        public bool CanFinish
        {
            get { return (bool)GetValue(CanFinishProperty); }
            set { SetValue(CanFinishProperty, BooleanBoxes.Box(value)); }
        }
        #endregion

        #region ShowFinishButton DependencyProperty
        public static readonly DependencyProperty ShowFinishButtonProperty = DependencyProperty.Register("ShowFinishButton",
            typeof(bool),
            typeof(WizardPage),
            new PropertyMetadata(BooleanBoxes.FalseBox, OnFinishButtonPropertyChanged));

        public bool ShowFinishButton
        {
            get { return (bool)GetValue(ShowFinishButtonProperty); }
            set { SetValue(ShowFinishButtonProperty, BooleanBoxes.Box(value)); }
        }
        #endregion

        #region FinishButtonContent DependencyProperty
        public static readonly DependencyProperty FinishButtonContentProperty = DependencyProperty.Register("FinishButtonContent",
            typeof(object),
            typeof(WizardPage),
            new PropertyMetadata("Fertig", OnFinishButtonPropertyChanged));

        public object FinishButtonContent
        {
            get { return GetValue(FinishButtonContentProperty); }
            set { SetValue(FinishButtonContentProperty, value); }
        }
        #endregion

        #region CanCancel DependencyProperty
        public static readonly DependencyProperty CanCancelProperty = DependencyProperty.Register("CanCancel",
            typeof(bool),
            typeof(WizardPage),
            new PropertyMetadata(BooleanBoxes.TrueBox, OnCancelButtonPropertyChanged));

        public bool CanCancel
        {
            get { return (bool)GetValue(CanCancelProperty); }
            set { SetValue(CanCancelProperty, BooleanBoxes.Box(value)); }
        }
        #endregion

        #region ShowCancelButton DependencyProperty
        public static readonly DependencyProperty ShowCancelButtonProperty = DependencyProperty.Register("ShowCancelButton",
            typeof(bool),
            typeof(WizardPage),
            new PropertyMetadata(BooleanBoxes.TrueBox, OnCancelButtonPropertyChanged));

        public bool ShowCancelButton
        {
            get { return (bool)GetValue(ShowCancelButtonProperty); }
            set { SetValue(ShowCancelButtonProperty, BooleanBoxes.Box(value)); }
        }
        #endregion

        #region CancelButtonContent DependencyProperty
        public static readonly DependencyProperty CancelButtonContentProperty = DependencyProperty.Register("CancelButtonContent",
            typeof(object),
            typeof(WizardPage),
            new PropertyMetadata("Abbrechen", OnCancelButtonPropertyChanged));

        public object CancelButtonContent
        {
            get { return GetValue(CancelButtonContentProperty); }
            set { SetValue(CancelButtonContentProperty, value); }
        }
        #endregion

        private static void OnGoToPreviousButtonPropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            var page = (WizardPage)sender;

            if (page.Owner?.SelectedPage == page) page.Owner?.RefreshGoToPreviousButton();
        }

        private static void OnGoToNextButtonPropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            var page = (WizardPage)sender;

            if (page.Owner?.SelectedPage == page) page.Owner?.RefreshGoToNextButton();
        }

        private static void OnFinishButtonPropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            var page = (WizardPage)sender;

            if (page.Owner?.SelectedPage == page) page.Owner?.RefreshFinishButton();
        }

        private static void OnCancelButtonPropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            var page = (WizardPage)sender;

            if (page.Owner?.SelectedPage == page) page.Owner?.RefreshCancelButton();
        }

        private Wizard _owner;
        internal Wizard Owner
        {
            get
            {
                if (_owner == null) _owner = this.ParentOfType<Wizard>();

                return _owner;
            }
            set { _owner = value; }
        }
    }
}