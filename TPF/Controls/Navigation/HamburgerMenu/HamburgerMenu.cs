using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using TPF.Internal;

namespace TPF.Controls
{
    public class HamburgerMenu : ItemsControl
    {
        static HamburgerMenu()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(HamburgerMenu), new FrameworkPropertyMetadata(typeof(HamburgerMenu)));

            EventManager.RegisterClassHandler(typeof(HamburgerMenu), Selector.SelectedEvent, new RoutedEventHandler(OnSelected));
            EventManager.RegisterClassHandler(typeof(HamburgerMenu), Selector.UnselectedEvent, new RoutedEventHandler(OnUnselected));
        }

        #region MenuOpened RoutedEvent
        public static readonly RoutedEvent MenuOpenedEvent = EventManager.RegisterRoutedEvent("MenuOpened",
            RoutingStrategy.Bubble,
            typeof(RoutedEventHandler),
            typeof(HamburgerMenu));

        public event RoutedEventHandler MenuOpened
        {
            add => AddHandler(MenuOpenedEvent, value);
            remove => RemoveHandler(MenuOpenedEvent, value);
        }
        #endregion

        #region MenuClosed RoutedEvent
        public static readonly RoutedEvent MenuClosedEvent = EventManager.RegisterRoutedEvent("MenuClosed",
            RoutingStrategy.Bubble,
            typeof(RoutedEventHandler),
            typeof(HamburgerMenu));

        public event RoutedEventHandler MenuClosed
        {
            add => AddHandler(MenuClosedEvent, value);
            remove => RemoveHandler(MenuClosedEvent, value);
        }
        #endregion

        #region ItemClicked RoutedEvent
        public static readonly RoutedEvent ItemClickedEvent = EventManager.RegisterRoutedEvent("ItemClicked",
            RoutingStrategy.Bubble,
            typeof(ItemClickedEventHandler),
            typeof(HamburgerMenu));

        public event ItemClickedEventHandler ItemClicked
        {
            add => AddHandler(ItemClickedEvent, value);
            remove => RemoveHandler(ItemClickedEvent, value);
        }
        #endregion

        #region SelectedItemChanged RoutedEvent
        public static readonly RoutedEvent SelectedItemChangedEvent = EventManager.RegisterRoutedEvent("SelectedItemChanged",
            RoutingStrategy.Bubble,
            typeof(SelectedItemChangedEventHandler),
            typeof(HamburgerMenu));

        public event SelectedItemChangedEventHandler SelectedItemChanged
        {
            add => AddHandler(SelectedItemChangedEvent, value);
            remove => RemoveHandler(SelectedItemChangedEvent, value);
        }
        #endregion

        #region AutoLoadSelectedContent DependencyProperty
        public static readonly DependencyProperty AutoLoadSelectedContentProperty = DependencyProperty.Register("AutoLoadSelectedContent",
            typeof(bool),
            typeof(HamburgerMenu),
            new PropertyMetadata(BooleanBoxes.TrueBox));

        public bool AutoLoadSelectedContent
        {
            get { return (bool)GetValue(AutoLoadSelectedContentProperty); }
            set { SetValue(AutoLoadSelectedContentProperty, BooleanBoxes.Box(value)); }
        }
        #endregion

        #region IsMenuOpen DependencyProperty
        public static readonly DependencyProperty IsMenuOpenProperty = DependencyProperty.Register("IsMenuOpen",
            typeof(bool),
            typeof(HamburgerMenu),
            new PropertyMetadata(BooleanBoxes.FalseBox, OnIsMenuOpenChanged));

        private static void OnIsMenuOpenChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            var instance = (HamburgerMenu)sender;

            if (instance._templateApplied) instance.OnIsMenuOpenChanged();
        }

        public bool IsMenuOpen
        {
            get { return (bool)GetValue(IsMenuOpenProperty); }
            set { SetValue(IsMenuOpenProperty, BooleanBoxes.Box(value)); }
        }
        #endregion

        #region CloseMenuOnSelection DependencyProperty
        public static readonly DependencyProperty CloseMenuOnSelectionProperty = DependencyProperty.Register("CloseMenuOnSelection",
            typeof(bool),
            typeof(HamburgerMenu),
            new PropertyMetadata(BooleanBoxes.FalseBox));

        public bool CloseMenuOnSelection
        {
            get { return (bool)GetValue(CloseMenuOnSelectionProperty); }
            set { SetValue(CloseMenuOnSelectionProperty, BooleanBoxes.Box(value)); }
        }
        #endregion

        #region DisplayMode DependencyProperty
        public static readonly DependencyProperty DisplayModeProperty = DependencyProperty.Register("DisplayMode",
            typeof(HamburgerMenuDisplayMode),
            typeof(HamburgerMenu),
            new PropertyMetadata(HamburgerMenuDisplayMode.Compact, OnDisplayModeChanged));

        private static void OnDisplayModeChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            var instance = (HamburgerMenu)sender;

            switch (instance.DisplayMode)
            {
                case HamburgerMenuDisplayMode.Minimal:
                {
                    instance.IsMenuOpen = false;
                    instance.MenuWidth = 0;
                    break;
                }
                case HamburgerMenuDisplayMode.Compact:
                {
                    instance.IsMenuOpen = false;
                    instance.MenuWidth = instance.MenuCollapsedWidth;
                    break;
                }
                case HamburgerMenuDisplayMode.Expanded:
                {
                    instance.IsMenuOpen = true;
                    instance.MenuWidth = instance.MenuExpandedWidth;
                    break;
                }
            }
        }

        public HamburgerMenuDisplayMode DisplayMode
        {
            get { return (HamburgerMenuDisplayMode)GetValue(DisplayModeProperty); }
            set { SetValue(DisplayModeProperty, value); }
        }
        #endregion

        #region MenuCollapsedWidth DependencyProperty
        public static readonly DependencyProperty MenuCollapsedWidthProperty = DependencyProperty.Register("MenuCollapsedWidth",
            typeof(double),
            typeof(HamburgerMenu),
            new PropertyMetadata(40.0, OnMenuCollapsedWidthChanged));

        private static void OnMenuCollapsedWidthChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            var instance = (HamburgerMenu)sender;

            if (!instance.IsMenuOpen && instance.DisplayMode != HamburgerMenuDisplayMode.Minimal) instance.MenuWidth = instance.MenuCollapsedWidth;
        }

        public double MenuCollapsedWidth
        {
            get { return (double)GetValue(MenuCollapsedWidthProperty); }
            set { SetValue(MenuCollapsedWidthProperty, value); }
        }
        #endregion

        #region MenuExpandedWidth DependencyProperty
        public static readonly DependencyProperty MenuExpandedWidthProperty = DependencyProperty.Register("MenuExpandedWidth",
            typeof(double),
            typeof(HamburgerMenu),
            new PropertyMetadata(200.0, OnMenuExpandedWidthChanged));

        private static void OnMenuExpandedWidthChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            var instance = (HamburgerMenu)sender;

            if (instance.IsMenuOpen) instance.MenuWidth = instance.MenuExpandedWidth;
        }

        public double MenuExpandedWidth
        {
            get { return (double)GetValue(MenuExpandedWidthProperty); }
            set { SetValue(MenuExpandedWidthProperty, value); }
        }
        #endregion

        #region MenuWidth ReadOnly DependencyProperty
        private static readonly DependencyPropertyKey MenuWidthPropertyKey = DependencyProperty.RegisterReadOnly("MenuWidth",
            typeof(double),
            typeof(HamburgerMenu),
            new PropertyMetadata(40.0, OnMenuWidthChanged));

        public static readonly DependencyProperty MenuWidthProperty = MenuWidthPropertyKey.DependencyProperty;

        private static void OnMenuWidthChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            var instance = (HamburgerMenu)sender;

            instance.OnMenuWidthChanged((double)e.OldValue, (double)e.NewValue);
        }

        public double MenuWidth
        {
            get { return (double)GetValue(MenuWidthProperty); }
            protected set { SetValue(MenuWidthPropertyKey, value); }
        }
        #endregion

        #region MenuBackground DependencyProperty
        public static readonly DependencyProperty MenuBackgroundProperty = DependencyProperty.Register("MenuBackground",
            typeof(Brush),
            typeof(HamburgerMenu),
            new PropertyMetadata(null));

        public Brush MenuBackground
        {
            get { return (Brush)GetValue(MenuBackgroundProperty); }
            set { SetValue(MenuBackgroundProperty, value); }
        }
        #endregion

        #region MenuHeader DependencyProperty
        public static readonly DependencyProperty MenuHeaderProperty = DependencyProperty.Register("MenuHeader",
            typeof(object),
            typeof(HamburgerMenu),
            new PropertyMetadata(null));

        public object MenuHeader
        {
            get { return GetValue(MenuHeaderProperty); }
            set { SetValue(MenuHeaderProperty, value); }
        }
        #endregion

        #region MenuHeaderBackground DependencyProperty
        public static readonly DependencyProperty MenuHeaderBackgroundProperty = DependencyProperty.Register("MenuHeaderBackground",
            typeof(Brush),
            typeof(HamburgerMenu),
            new PropertyMetadata(null));

        public Brush MenuHeaderBackground
        {
            get { return (Brush)GetValue(MenuHeaderBackgroundProperty); }
            set { SetValue(MenuHeaderBackgroundProperty, value); }
        }
        #endregion

        #region MenuHeaderForeground DependencyProperty
        public static readonly DependencyProperty MenuHeaderForegroundProperty = DependencyProperty.Register("MenuHeaderForeground",
            typeof(Brush),
            typeof(HamburgerMenu),
            new PropertyMetadata(null));

        public Brush MenuHeaderForeground
        {
            get { return (Brush)GetValue(MenuHeaderForegroundProperty); }
            set { SetValue(MenuHeaderForegroundProperty, value); }
        }
        #endregion

        #region MenuHeaderHeight DependencyProperty
        public static readonly DependencyProperty MenuHeaderHeightProperty = DependencyProperty.Register("MenuHeaderHeight",
            typeof(double),
            typeof(HamburgerMenu),
            new PropertyMetadata(36.0));

        public double MenuHeaderHeight
        {
            get { return (double)GetValue(MenuHeaderHeightProperty); }
            set { SetValue(MenuHeaderHeightProperty, value); }
        }
        #endregion

        #region MenuFooter DependencyProperty
        public static readonly DependencyProperty MenuFooterProperty = DependencyProperty.Register("MenuFooter",
            typeof(object),
            typeof(HamburgerMenu),
            new PropertyMetadata(null));

        public object MenuFooter
        {
            get { return GetValue(MenuFooterProperty); }
            set { SetValue(MenuFooterProperty, value); }
        }
        #endregion

        #region MenuFooterTemplate DependencyProperty
        public static readonly DependencyProperty MenuFooterTemplateProperty = DependencyProperty.Register("MenuFooterTemplate",
            typeof(DataTemplate),
            typeof(HamburgerMenu),
            new PropertyMetadata(null));

        public DataTemplate MenuFooterTemplate
        {
            get { return (DataTemplate)GetValue(MenuFooterTemplateProperty); }
            set { SetValue(MenuFooterTemplateProperty, value); }
        }
        #endregion

        #region MenuFooterTemplateSelector DependencyProperty
        public static readonly DependencyProperty MenuFooterTemplateSelectorProperty = DependencyProperty.Register("MenuFooterTemplateSelector",
            typeof(DataTemplateSelector),
            typeof(HamburgerMenu),
            new PropertyMetadata(null));

        public DataTemplateSelector MenuFooterTemplateSelector
        {
            get { return (DataTemplateSelector)GetValue(MenuFooterTemplateSelectorProperty); }
            set { SetValue(MenuFooterTemplateSelectorProperty, value); }
        }
        #endregion

        #region MenuToggleButtonContent DependencyProperty
        public static readonly DependencyProperty MenuToggleButtonContentProperty = DependencyProperty.Register("MenuToggleButtonContent",
            typeof(object),
            typeof(HamburgerMenu),
            new PropertyMetadata(null));

        public object MenuToggleButtonContent
        {
            get { return GetValue(MenuToggleButtonContentProperty); }
            set { SetValue(MenuToggleButtonContentProperty, value); }
        }
        #endregion

        #region MenuToggleButtonContentTemplate DependencyProperty
        public static readonly DependencyProperty MenuToggleButtonContentTemplateProperty = DependencyProperty.Register("MenuToggleButtonContentTemplate",
            typeof(DataTemplate),
            typeof(HamburgerMenu),
            new PropertyMetadata(null));

        public DataTemplate MenuToggleButtonContentTemplate
        {
            get { return (DataTemplate)GetValue(MenuToggleButtonContentTemplateProperty); }
            set { SetValue(MenuToggleButtonContentTemplateProperty, value); }
        }
        #endregion

        #region MenuToggleButtonStyle DependencyProperty
        public static readonly DependencyProperty MenuToggleButtonStyleProperty = DependencyProperty.Register("MenuToggleButtonStyle",
            typeof(Style),
            typeof(HamburgerMenu),
            new PropertyMetadata(null));

        public Style MenuToggleButtonStyle
        {
            get { return (Style)GetValue(MenuToggleButtonStyleProperty); }
            set { SetValue(MenuToggleButtonStyleProperty, value); }
        }
        #endregion

        #region MenuToggleButtonVisibility DependencyProperty
        public static readonly DependencyProperty MenuToggleButtonVisibilityProperty = DependencyProperty.Register("MenuToggleButtonVisibility",
            typeof(Visibility),
            typeof(HamburgerMenu),
            new PropertyMetadata(VisibilityBoxes.VisibleBox));

        public Visibility MenuToggleButtonVisibility
        {
            get { return (Visibility)GetValue(MenuToggleButtonVisibilityProperty); }
            set { SetValue(MenuToggleButtonVisibilityProperty, VisibilityBoxes.Box(value)); }
        }
        #endregion

        #region Content DependencyProperty
        public static readonly DependencyProperty ContentProperty = DependencyProperty.Register("Content",
            typeof(object),
            typeof(HamburgerMenu),
            new PropertyMetadata(null));

        public object Content
        {
            get { return GetValue(ContentProperty); }
            set { SetValue(ContentProperty, value); }
        }
        #endregion

        #region SelectedItem DependencyProperty
        public static readonly DependencyProperty SelectedItemProperty = DependencyProperty.Register("SelectedItem",
            typeof(object),
            typeof(HamburgerMenu),
            new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnSelectedItemChanged));

        private static void OnSelectedItemChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            var instance = (HamburgerMenu)sender;

            instance.OnSelectionChanged(e.OldValue, e.NewValue);
        }

        public object SelectedItem
        {
            get { return GetValue(SelectedItemProperty); }
            set { SetValue(SelectedItemProperty, value); }
        }
        #endregion

        bool _templateApplied;
        bool _selectionChanging;
        HamburgerMenuItem _nextContainer;
        internal HamburgerMenuItem SelectedContainer;

        internal Grid MenuRoot;
        UIElement DismissalOverlay;

        HamburgerMenuItem _highlightedItem;
        private HamburgerMenuItem HighlightedItem
        {
            get { return _highlightedItem; }
            set
            {
                if (_highlightedItem == value) return;
                if (_highlightedItem != null) _highlightedItem.SetIsHighlighted(false);
                _highlightedItem = value;
                if (_highlightedItem != null) _highlightedItem.SetIsHighlighted(true);
            }
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            if (DismissalOverlay != null)
            {
                DismissalOverlay.MouseUp -= DismissalOverlay_MouseUp;
            }

            MenuRoot = (Grid)GetTemplateChild("PART_MenuRoot");
            DismissalOverlay = GetTemplateChild("PART_DismissalOverlay") as UIElement;

            if (DismissalOverlay != null)
            {
                DismissalOverlay.MouseUp += DismissalOverlay_MouseUp;
            }

            OnIsMenuOpenChanged();

            if (SelectedItem == null && Items.Count > 0) SelectedItem = Items[0];

            _templateApplied = true;
        }

        private void DismissalOverlay_MouseUp(object sender, MouseButtonEventArgs e)
        {
            IsMenuOpen = false;
        }

        protected virtual void OnIsMenuOpenChanged()
        {
            var eventArgs = new RoutedEventArgs();

            if (IsMenuOpen)
            {
                eventArgs.RoutedEvent = MenuOpenedEvent;
                MenuWidth = MenuExpandedWidth;
            }
            else
            {
                eventArgs.RoutedEvent = MenuClosedEvent;
                if (DisplayMode == HamburgerMenuDisplayMode.Minimal) MenuWidth = 0.0;
                else MenuWidth = MenuCollapsedWidth;
            }

            if (_templateApplied) RaiseEvent(eventArgs);
        }

        protected virtual void OnMenuWidthChanged(double oldValue, double newValue)
        {
            if (MenuRoot == null) return;

            var duration = _templateApplied ? new Duration(TimeSpan.FromMilliseconds(100)) : new Duration(TimeSpan.FromMilliseconds(0));
            var animation = new DoubleAnimation(oldValue, newValue, duration);

            // Ziel der Animation setzen
            Storyboard.SetTarget(animation, MenuRoot);
            Storyboard.SetTargetProperty(animation, new PropertyPath("Width"));

            // Storyboard erstellen und Animationen hinzufügen
            var story = new Storyboard();
            story.Children.Add(animation);
            story.Begin();
        }

        protected override DependencyObject GetContainerForItemOverride()
        {
            return new HamburgerMenuItem();
        }

        protected override bool IsItemItsOwnContainerOverride(object item)
        {
            return item is HamburgerMenuItem;
        }

        protected override void PrepareContainerForItemOverride(DependencyObject element, object item)
        {
            base.PrepareContainerForItemOverride(element, item);

            // Wenn ein neuer Container generiert wurde und es sich bei dem Item um das gleiche item wie in SelectedItem handelt, dann IsSelected setzen
            if (element is HamburgerMenuItem menuItem && item == SelectedItem)
            {
                menuItem.SetCurrentValue(HamburgerMenuItem.IsSelectedProperty, true);
                SelectedContainer = menuItem;
            }
        }

        protected override void ClearContainerForItemOverride(DependencyObject element, object item)
        {
            base.ClearContainerForItemOverride(element, item);

            // Wenn unser item ein HamburgerMenuItem ist, dann abbrechen
            if (item is HamburgerMenuItem) return;
            // Es wird davon ausgegangen, dass es sich bei dem Container um ein HamburgerMenuItem handelt
            // Von diesem Container soll nun der Wert der IsSelected-Property geleert werden
            element.ClearValue(HamburgerMenuItem.IsSelectedProperty);
        }

        // Setzt IsHighlighted bei dem angegebenen Container
        internal void Highlight(HamburgerMenuItem item)
        {
            HighlightedItem = item;
        }

        internal void RaiseItemClicked(HamburgerMenuItem item)
        {
            var eventArgs = new ItemClickedEventArgs(ItemClickedEvent, item);

            RaiseEvent(eventArgs);
        }

        private static void OnSelected(object sender, RoutedEventArgs e)
        {
            var instance = (HamburgerMenu)sender;

            e.Handled = true;

            instance.ProcessSelection(e.OriginalSource as HamburgerMenuItem, true);
        }

        private static void OnUnselected(object sender, RoutedEventArgs e)
        {
            var instance = (HamburgerMenu)sender;

            e.Handled = true;

            instance.ProcessSelection(e.OriginalSource as HamburgerMenuItem, false);
        }

        private void ProcessSelection(HamburgerMenuItem container, bool isSelected)
        {
            if (_selectionChanging) return;

            if (isSelected)
            {
                var generator = GetContainerGenerator(container);

                var item = generator.ItemFromContainer(container);

                if (item == DependencyProperty.UnsetValue && IsOwnedItem(container) && IsItemItsOwnContainer(container))
                {
                    item = container;
                }

                _nextContainer = container;
                SelectedItem = item;
            }
            else
            {
                SelectedItem = null;
            }
        }

        private ItemContainerGenerator GetContainerGenerator(HamburgerMenuItem menuItem)
        {
            var parentItem = menuItem.ParentOfType<HamburgerMenuItem>();

            return parentItem != null ? parentItem.ItemContainerGenerator : ItemContainerGenerator;
        }

        private bool IsOwnedItem(HamburgerMenuItem menuItem)
        {
            var parentItem = menuItem.ParentOfType<HamburgerMenuItem>();

            if (parentItem != null) return IsOwnedItem(parentItem);
            else return ItemsControlFromItemContainer(menuItem) == this;
        }

        private HamburgerMenuItem GetContainerFromItem(object item)
        {
            // Gehört uns das Item direkt?
            var result = ItemContainerGenerator.ContainerFromItem(item);

            if (result == null)
            {
                result = SearchItems(this, item);
            }

            return result as HamburgerMenuItem;
        }

        private HamburgerMenuItem SearchItems(ItemsControl itemsControl, object target)
        {
            var result = itemsControl.ItemContainerGenerator.ContainerFromItem(target) as HamburgerMenuItem;

            if (result == null)
            {
                for (int i = 0; i < itemsControl.Items.Count; i++)
                {
                    var item = itemsControl.Items[i];

                    var container = itemsControl.ItemContainerGenerator.ContainerFromItem(item) as ItemsControl;

                    if (container == null) continue;

                    result = SearchItems(container, target);

                    if (result != null) break;
                }
            }

            return result;
        }

        protected virtual void OnSelectionChanged(object oldValue, object newValue)
        {
            if (_selectionChanging) return;

            if (CloseMenuOnSelection) IsMenuOpen = false;

            _selectionChanging = true;

            if (SelectedContainer != null)
            {
                SelectedContainer.IsSelected = false;
                SelectedContainer = null;
            }

            if (oldValue is HamburgerMenuItem oldItem)
            {
                oldItem.IsSelected = false;
            }

            if (newValue is HamburgerMenuItem newItem)
            {
                newItem.IsSelected = true;
                _nextContainer = newItem;
            }

            if (newValue != null && _nextContainer == null)
            {
                _nextContainer = GetContainerFromItem(newValue);
            }

            SelectedContainer = _nextContainer;
            if (SelectedContainer != null) SelectedContainer.IsSelected = true;
            _nextContainer = null;

            if (AutoLoadSelectedContent)
            {
                if (SelectedContainer == null) Content = null;
                else if (SelectedContainer.Items.Count == 0) Content = SelectedContainer.Content;
            }

            _selectionChanging = false;

            var eventArgs = new SelectedItemChangedEventArgs(SelectedItemChangedEvent, oldValue, newValue);

            RaiseEvent(eventArgs);
        }
    }
}