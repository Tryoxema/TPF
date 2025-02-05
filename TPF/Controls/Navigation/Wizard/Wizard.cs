using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Markup;
using System.Windows.Controls;
using TPF.Controls.Specialized.Wizard;
using TPF.Internal;

namespace TPF.Controls
{
    [ContentProperty("Pages")]
    public class Wizard : Control
    {
        static Wizard()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(Wizard), new FrameworkPropertyMetadata(typeof(Wizard)));

            RegisterCommands();
        }

        #region Previous RoutedEvent
        public static readonly RoutedEvent PreviousEvent = EventManager.RegisterRoutedEvent("Previous",
            RoutingStrategy.Bubble,
            typeof(WizardPageNavigationEventHandler),
            typeof(Wizard));

        public event WizardPageNavigationEventHandler Previous
        {
            add => AddHandler(PreviousEvent, value);
            remove => RemoveHandler(PreviousEvent, value);
        }
        #endregion

        #region Next RoutedEvent
        public static readonly RoutedEvent NextEvent = EventManager.RegisterRoutedEvent("Next",
            RoutingStrategy.Bubble,
            typeof(WizardPageNavigationEventHandler),
            typeof(Wizard));

        public event WizardPageNavigationEventHandler Next
        {
            add => AddHandler(NextEvent, value);
            remove => RemoveHandler(NextEvent, value);
        }
        #endregion

        #region Finish RoutedEvent
        public static readonly RoutedEvent FinishEvent = EventManager.RegisterRoutedEvent("Finish",
            RoutingStrategy.Bubble,
            typeof(RoutedEventHandler),
            typeof(Wizard));

        public event RoutedEventHandler Finish
        {
            add => AddHandler(FinishEvent, value);
            remove => RemoveHandler(FinishEvent, value);
        }
        #endregion

        #region Cancel RoutedEvent
        public static readonly RoutedEvent CancelEvent = EventManager.RegisterRoutedEvent("Cancel",
            RoutingStrategy.Bubble,
            typeof(RoutedEventHandler),
            typeof(Wizard));

        public event RoutedEventHandler Cancel
        {
            add => AddHandler(CancelEvent, value);
            remove => RemoveHandler(CancelEvent, value);
        }
        #endregion

        #region SelectedPageChanged RoutedEvent
        public static readonly RoutedEvent SelectedPageChangedEvent = EventManager.RegisterRoutedEvent("SelectedPageChanged",
            RoutingStrategy.Bubble,
            typeof(SelectedPageChangedEventHandler),
            typeof(Wizard));

        public event SelectedPageChangedEventHandler SelectedPageChanged
        {
            add => AddHandler(SelectedPageChangedEvent, value);
            remove => RemoveHandler(SelectedPageChangedEvent, value);
        }
        #endregion

        #region SelectedPage DependencyProperty
        public static readonly DependencyProperty SelectedPageProperty = DependencyProperty.Register("SelectedPage",
            typeof(WizardPage),
            typeof(Wizard),
            new PropertyMetadata(null, OnSelectedPageChanged, CoerceSelectedPage));

        private static void OnSelectedPageChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            var instance = (Wizard)sender;

            var oldPage = (WizardPage)e.OldValue;
            var newPage = (WizardPage)e.NewValue;

            instance.OnSelectedPageChanged(oldPage, newPage);
            instance.SetPropertiesFromSelectedPage();
        }

        private static object CoerceSelectedPage(DependencyObject sender, object value)
        {
            if (value == null) return value;
            
            var instance = (Wizard)sender;
            var page = value as WizardPage;

            if (instance.Pages.Contains(page)) return page;
            else return null;
        }

        public WizardPage SelectedPage
        {
            get { return (WizardPage)GetValue(SelectedPageProperty); }
            set { SetValue(SelectedPageProperty, value); }
        }
        #endregion

        #region SelectedIndex DependencyProperty
        public static readonly DependencyProperty SelectedIndexProperty = DependencyProperty.Register("SelectedIndex",
            typeof(int),
            typeof(Wizard),
            new PropertyMetadata(-1, OnSelectedIndexChanged, CoerceSelectedIndex));

        private static void OnSelectedIndexChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            var instance = (Wizard)sender;

            var oldIndex = (int)e.OldValue;
            var newIndex = (int)e.NewValue;

            instance.OnSelectedIndexChanged(oldIndex, newIndex);
        }

        private static object CoerceSelectedIndex(DependencyObject sender, object value)
        {
            var instance = (Wizard)sender;
            var index = (int)value;

            if (index >= instance.Pages.Count)
            {
                return instance.Pages.Count - 1;
            }

            return index;
        }

        public int SelectedIndex
        {
            get { return (int)GetValue(SelectedIndexProperty); }
            set { SetValue(SelectedIndexProperty, value); }
        }
        #endregion

        #region FooterBackground DependencyProperty
        public static readonly DependencyProperty FooterBackgroundProperty = DependencyProperty.Register("FooterBackground",
            typeof(Brush),
            typeof(Wizard),
            new PropertyMetadata(null));

        public Brush FooterBackground
        {
            get { return (Brush)GetValue(FooterBackgroundProperty); }
            set { SetValue(FooterBackgroundProperty, value); }
        }
        #endregion

        #region FooterPadding DependencyProperty
        public static readonly DependencyProperty FooterPaddingProperty = DependencyProperty.Register("FooterPadding",
            typeof(Thickness),
            typeof(Wizard),
            new PropertyMetadata(default(Thickness)));

        public Thickness FooterPadding
        {
            get { return (Thickness)GetValue(FooterPaddingProperty); }
            set { SetValue(FooterPaddingProperty, value); }
        }
        #endregion

        #region CanGoToPrevious ReadOnly DependencyProperty
        private static readonly DependencyPropertyKey CanGoToPreviousPropertyKey = DependencyProperty.RegisterReadOnly("CanGoToPrevious",
            typeof(bool),
            typeof(Wizard),
            new PropertyMetadata(BooleanBoxes.FalseBox, OnCanPropertyChanged));

        public static readonly DependencyProperty CanGoToPreviousProperty = CanGoToPreviousPropertyKey.DependencyProperty;

        public bool CanGoToPrevious
        {
            get { return (bool)GetValue(CanGoToPreviousProperty); }
            private set { SetValue(CanGoToPreviousPropertyKey, BooleanBoxes.Box(value)); }
        }
        #endregion

        #region GoToPreviousButtonVisibility ReadOnly DependencyProperty
        private static readonly DependencyPropertyKey GoToPreviousButtonVisibilityPropertyKey = DependencyProperty.RegisterReadOnly("GoToPreviousButtonVisibility",
            typeof(Visibility),
            typeof(Wizard),
            new PropertyMetadata(VisibilityBoxes.VisibleBox));

        public static readonly DependencyProperty GoToPreviousButtonVisibilityProperty = GoToPreviousButtonVisibilityPropertyKey.DependencyProperty;

        public Visibility GoToPreviousButtonVisibility
        {
            get { return (Visibility)GetValue(GoToPreviousButtonVisibilityProperty); }
            private set { SetValue(GoToPreviousButtonVisibilityPropertyKey, VisibilityBoxes.Box(value)); }
        }
        #endregion

        #region GoToPreviousButtonContent ReadOnly DependencyProperty
        private static readonly DependencyPropertyKey GoToPreviousButtonContentPropertyKey = DependencyProperty.RegisterReadOnly("GoToPreviousButtonContent",
            typeof(object),
            typeof(Wizard),
            new PropertyMetadata("Zurück"));

        public static readonly DependencyProperty GoToPreviousButtonContentProperty = GoToPreviousButtonContentPropertyKey.DependencyProperty;

        public object GoToPreviousButtonContent
        {
            get { return GetValue(GoToPreviousButtonContentProperty); }
            private set { SetValue(GoToPreviousButtonContentPropertyKey, value); }
        }
        #endregion

        #region CanGoToNext ReadOnly DependencyProperty
        private static readonly DependencyPropertyKey CanGoToNextPropertyKey = DependencyProperty.RegisterReadOnly("CanGoToNext",
            typeof(bool),
            typeof(Wizard),
            new PropertyMetadata(BooleanBoxes.FalseBox, OnCanPropertyChanged));

        public static readonly DependencyProperty CanGoToNextProperty = CanGoToNextPropertyKey.DependencyProperty;

        public bool CanGoToNext
        {
            get { return (bool)GetValue(CanGoToNextProperty); }
            private set { SetValue(CanGoToNextPropertyKey, BooleanBoxes.Box(value)); }
        }
        #endregion

        #region GoToNextButtonVisibility ReadOnly DependencyProperty
        private static readonly DependencyPropertyKey GoToNextButtonVisibilityPropertyKey = DependencyProperty.RegisterReadOnly("GoToNextButtonVisibility",
            typeof(Visibility),
            typeof(Wizard),
            new PropertyMetadata(VisibilityBoxes.VisibleBox));

        public static readonly DependencyProperty GoToNextButtonVisibilityProperty = GoToNextButtonVisibilityPropertyKey.DependencyProperty;

        public Visibility GoToNextButtonVisibility
        {
            get { return (Visibility)GetValue(GoToNextButtonVisibilityProperty); }
            private set { SetValue(GoToNextButtonVisibilityPropertyKey, VisibilityBoxes.Box(value)); }
        }
        #endregion

        #region GoToNextButtonContent ReadOnly DependencyProperty
        private static readonly DependencyPropertyKey GoToNextButtonContentPropertyKey = DependencyProperty.RegisterReadOnly("GoToNextButtonContent",
            typeof(object),
            typeof(Wizard),
            new PropertyMetadata("Weiter"));

        public static readonly DependencyProperty GoToNextButtonContentProperty = GoToNextButtonContentPropertyKey.DependencyProperty;

        public object GoToNextButtonContent
        {
            get { return GetValue(GoToNextButtonContentProperty); }
            private set { SetValue(GoToNextButtonContentPropertyKey, value); }
        }
        #endregion

        #region CanFinish ReadOnly DependencyProperty
        private static readonly DependencyPropertyKey CanFinishPropertyKey = DependencyProperty.RegisterReadOnly("CanFinish",
            typeof(bool),
            typeof(Wizard),
            new PropertyMetadata(BooleanBoxes.FalseBox, OnCanPropertyChanged));

        public static readonly DependencyProperty CanFinishProperty = CanFinishPropertyKey.DependencyProperty;

        public bool CanFinish
        {
            get { return (bool)GetValue(CanFinishProperty); }
            private set { SetValue(CanFinishPropertyKey, BooleanBoxes.Box(value)); }
        }
        #endregion

        #region FinishButtonVisibility ReadOnly DependencyProperty
        private static readonly DependencyPropertyKey FinishButtonVisibilityPropertyKey = DependencyProperty.RegisterReadOnly("FinishButtonVisibility",
            typeof(Visibility),
            typeof(Wizard),
            new PropertyMetadata(VisibilityBoxes.VisibleBox));

        public static readonly DependencyProperty FinishButtonVisibilityProperty = FinishButtonVisibilityPropertyKey.DependencyProperty;

        public Visibility FinishButtonVisibility
        {
            get { return (Visibility)GetValue(FinishButtonVisibilityProperty); }
            private set { SetValue(FinishButtonVisibilityPropertyKey, VisibilityBoxes.Box(value)); }
        }
        #endregion

        #region FinishButtonContent ReadOnly DependencyProperty
        private static readonly DependencyPropertyKey FinishButtonContentPropertyKey = DependencyProperty.RegisterReadOnly("FinishButtonContent",
            typeof(object),
            typeof(Wizard),
            new PropertyMetadata("Fertig"));

        public static readonly DependencyProperty FinishButtonContentProperty = FinishButtonContentPropertyKey.DependencyProperty;

        public object FinishButtonContent
        {
            get { return GetValue(FinishButtonContentProperty); }
            private set { SetValue(FinishButtonContentPropertyKey, value); }
        }
        #endregion

        #region CanCancel ReadOnly DependencyProperty
        private static readonly DependencyPropertyKey CanCancelPropertyKey = DependencyProperty.RegisterReadOnly("CanCancel",
            typeof(bool),
            typeof(Wizard),
            new PropertyMetadata(BooleanBoxes.FalseBox, OnCanPropertyChanged));

        public static readonly DependencyProperty CanCancelProperty = CanCancelPropertyKey.DependencyProperty;

        public bool CanCancel
        {
            get { return (bool)GetValue(CanCancelProperty); }
            private set { SetValue(CanCancelPropertyKey, BooleanBoxes.Box(value)); }
        }
        #endregion

        #region CancelButtonVisibility ReadOnly DependencyProperty
        private static readonly DependencyPropertyKey CancelButtonVisibilityPropertyKey = DependencyProperty.RegisterReadOnly("CancelButtonVisibility",
            typeof(Visibility),
            typeof(Wizard),
            new PropertyMetadata(VisibilityBoxes.VisibleBox));

        public static readonly DependencyProperty CancelButtonVisibilityProperty = CancelButtonVisibilityPropertyKey.DependencyProperty;

        public Visibility CancelButtonVisibility
        {
            get { return (Visibility)GetValue(CancelButtonVisibilityProperty); }
            private set { SetValue(CancelButtonVisibilityPropertyKey, VisibilityBoxes.Box(value)); }
        }
        #endregion

        #region CancelButtonContent ReadOnly DependencyProperty
        private static readonly DependencyPropertyKey CancelButtonContentPropertyKey = DependencyProperty.RegisterReadOnly("CancelButtonContent",
            typeof(object),
            typeof(Wizard),
            new PropertyMetadata("Abbrechen"));

        public static readonly DependencyProperty CancelButtonContentProperty = CancelButtonContentPropertyKey.DependencyProperty;

        public object CancelButtonContent
        {
            get { return GetValue(CancelButtonContentProperty); }
            private set { SetValue(CancelButtonContentPropertyKey, value); }
        }
        #endregion

        private WizardPageCollection _pages;
        public WizardPageCollection Pages
        {
            get
            {
                if (_pages == null)
                {
                    _pages = new WizardPageCollection();
                    _pages.CollectionChanged += Pages_CollectionChanged; ;
                }
                return _pages;
            }
        }

        #region Commands
        private static void OnCanPropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            CommandManager.InvalidateRequerySuggested();
        }

        private static void RegisterCommands()
        {
            CommandManager.RegisterClassCommandBinding(typeof(Wizard), new CommandBinding(WizardCommands.GoToPrevious, OnGoToPreviousCommand, CanExecuteGoToPreviousCommand));
            CommandManager.RegisterClassCommandBinding(typeof(Wizard), new CommandBinding(WizardCommands.GoToNext, OnGoToNextCommand, CanExecuteGoToNextCommand));
            CommandManager.RegisterClassCommandBinding(typeof(Wizard), new CommandBinding(WizardCommands.Finish, OnFinishCommand, CanExecuteFinishCommand));
            CommandManager.RegisterClassCommandBinding(typeof(Wizard), new CommandBinding(WizardCommands.Cancel, OnCancelCommand, CanExecuteCancelCommand));
        }

        private static void CanExecuteGoToPreviousCommand(object sender, CanExecuteRoutedEventArgs e)
        {
            if (sender is Wizard wizard)
            {
                e.CanExecute = wizard.CanGoToPrevious;
            }
        }

        private static void OnGoToPreviousCommand(object sender, ExecutedRoutedEventArgs e)
        {
            if (sender is Wizard wizard) wizard.OnGoToPrevious();
        }

        private static void CanExecuteGoToNextCommand(object sender, CanExecuteRoutedEventArgs e)
        {
            if (sender is Wizard wizard)
            {
                e.CanExecute = wizard.CanGoToNext;
            }
        }

        private static void OnGoToNextCommand(object sender, ExecutedRoutedEventArgs e)
        {
            if (sender is Wizard wizard) wizard.OnGoToNext();
        }

        private static void CanExecuteFinishCommand(object sender, CanExecuteRoutedEventArgs e)
        {
            if (sender is Wizard wizard)
            {
                e.CanExecute = wizard.CanFinish;
            }
        }

        private static void OnFinishCommand(object sender, ExecutedRoutedEventArgs e)
        {
            if (sender is Wizard wizard) wizard.OnFinish();
        }

        private static void CanExecuteCancelCommand(object sender, CanExecuteRoutedEventArgs e)
        {
            if (sender is Wizard wizard)
            {
                e.CanExecute = wizard.CanCancel;
            }
        }

        private static void OnCancelCommand(object sender, ExecutedRoutedEventArgs e)
        {
            if (sender is Wizard wizard) wizard.OnCancel();
        }
        #endregion

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            SetPropertiesFromSelectedPage();
        }

        private void OnSelectedPageChanged(WizardPage oldPage, WizardPage newPage)
        {
            var e = new SelectedPageChangedEventArgs(SelectedPageChangedEvent, oldPage, newPage);

            RaiseEvent(e);

            var newIndex = Pages.IndexOf(newPage);

            SelectedIndex = newIndex;
        }

        private void OnSelectedIndexChanged(int oldIndex, int newIndex)
        {
            if (newIndex > -1 && newIndex < Pages.Count)
            {
                SelectedPage = Pages[newIndex];
            }
        }

        private void SetPropertiesFromSelectedPage()
        {
            if (SelectedPage == null) return;

            RefreshGoToPreviousButton();
            RefreshGoToNextButton();
            RefreshFinishButton();
            RefreshCancelButton();
        }

        internal void RefreshGoToPreviousButton()
        {
            if (SelectedPage == null) return;

            CanGoToPrevious = Pages.Count > 1 && SelectedIndex > 0 && SelectedPage.CanGoToPrevious;
            GoToPreviousButtonVisibility = SelectedPage.ShowGoToPreviousButton ? Visibility.Visible : Visibility.Collapsed;
            GoToPreviousButtonContent = SelectedPage.GoToPreviousButtonContent;
        }

        internal void RefreshGoToNextButton()
        {
            if (SelectedPage == null) return;

            CanGoToNext = Pages.Count > 1 && SelectedIndex < Pages.Count - 1 && SelectedPage.CanGoToNext;
            GoToNextButtonVisibility = SelectedPage.ShowGoToNextButton ? Visibility.Visible : Visibility.Collapsed;
            GoToNextButtonContent = SelectedPage.GoToNextButtonContent;
        }

        internal void RefreshFinishButton()
        {
            if (SelectedPage == null) return;

            CanFinish = SelectedPage.CanFinish;
            FinishButtonVisibility = SelectedPage.ShowFinishButton ? Visibility.Visible : Visibility.Collapsed;
            FinishButtonContent = SelectedPage.FinishButtonContent;
        }

        internal void RefreshCancelButton()
        {
            if (SelectedPage == null) return;

            CanCancel = SelectedPage.CanCancel;
            CancelButtonVisibility = SelectedPage.ShowCancelButton ? Visibility.Visible : Visibility.Collapsed;
            CancelButtonContent = SelectedPage.CancelButtonContent;
        }

        private void Pages_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            for (int i = 0; i < Pages.Count; i++)
            {
                var page = Pages[i];

                page.Owner = this;
            }

            if (SelectedPage == null && Pages.Count > 0)
            {
                SelectedPage = Pages[0];
            }
        }

        protected virtual void OnGoToPrevious()
        {
            var currentIndex = SelectedIndex;
            var nextIndex = currentIndex - 1;

            var e = new WizardPageNavigationEventArgs(PreviousEvent, currentIndex, nextIndex);

            RaiseEvent(e);

            if (e.Cancel) return;

            SelectedIndex = e.NewIndex;
        }

        protected virtual void OnGoToNext()
        {
            var currentIndex = SelectedIndex;
            var nextIndex = currentIndex + 1;

            var e = new WizardPageNavigationEventArgs(NextEvent, currentIndex, nextIndex);

            RaiseEvent(e);

            if (e.Cancel) return;

            SelectedIndex = e.NewIndex;
        }

        protected virtual void OnFinish()
        {
            var e = new RoutedEventArgs(FinishEvent);

            RaiseEvent(e);
        }

        protected virtual void OnCancel()
        {
            var e = new RoutedEventArgs(CancelEvent);

            RaiseEvent(e);
        }
    }
}