using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Markup;
using TPF.Internal;

namespace TPF.Controls
{
    [ContentProperty("Content")]
    public class HamburgerMenuItem : ItemsControl
    {
        static HamburgerMenuItem()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(HamburgerMenuItem), new FrameworkPropertyMetadata(typeof(HamburgerMenuItem)));

            EventManager.RegisterClassHandler(typeof(HamburgerMenuItem), AccessKeyManager.AccessKeyPressedEvent, new AccessKeyPressedEventHandler(OnAccessKeyPressed));
            EventManager.RegisterClassHandler(typeof(HamburgerMenuItem), Selector.SelectedEvent, new RoutedEventHandler(OnSelected));
            EventManager.RegisterClassHandler(typeof(HamburgerMenuItem), Selector.UnselectedEvent, new RoutedEventHandler(OnUnselected));
        }

        #region Click RoutedEvent
        public static readonly RoutedEvent ClickEvent = EventManager.RegisterRoutedEvent("Click",
            RoutingStrategy.Bubble,
            typeof(RoutedEventHandler),
            typeof(HamburgerMenuItem));

        public event RoutedEventHandler Click
        {
            add => AddHandler(ClickEvent, value);
            remove => RemoveHandler(ClickEvent, value);
        }
        #endregion

        #region Icon DependencyProperty
        public static readonly DependencyProperty IconProperty = DependencyProperty.Register("Icon",
            typeof(object),
            typeof(HamburgerMenuItem),
            new PropertyMetadata(null));

        public object Icon
        {
            get { return GetValue(IconProperty); }
            set { SetValue(IconProperty, value); }
        }
        #endregion

        #region IconTemplate DependencyProperty
        public static readonly DependencyProperty IconTemplateProperty = DependencyProperty.Register("IconTemplate",
            typeof(DataTemplate),
            typeof(HamburgerMenuItem),
            new PropertyMetadata(null));

        public DataTemplate IconTemplate
        {
            get { return (DataTemplate)GetValue(IconTemplateProperty); }
            set { SetValue(IconTemplateProperty, value); }
        }
        #endregion

        #region ExpandIcon DependencyProperty
        public static readonly DependencyProperty ExpandIconProperty = DependencyProperty.Register("ExpandIcon",
            typeof(object),
            typeof(HamburgerMenuItem),
            new PropertyMetadata(null));

        public object ExpandIcon
        {
            get { return GetValue(ExpandIconProperty); }
            set { SetValue(ExpandIconProperty, value); }
        }
        #endregion

        #region ExpandIconTemplate DependencyProperty
        public static readonly DependencyProperty ExpandIconTemplateProperty = DependencyProperty.Register("ExpandIconTemplate",
            typeof(DataTemplate),
            typeof(HamburgerMenuItem),
            new PropertyMetadata(null));

        public DataTemplate ExpandIconTemplate
        {
            get { return (DataTemplate)GetValue(ExpandIconTemplateProperty); }
            set { SetValue(ExpandIconTemplateProperty, value); }
        }
        #endregion

        #region CollapseIcon DependencyProperty
        public static readonly DependencyProperty CollapseIconProperty = DependencyProperty.Register("CollapseIcon",
            typeof(object),
            typeof(HamburgerMenuItem),
            new PropertyMetadata(null));

        public object CollapseIcon
        {
            get { return GetValue(CollapseIconProperty); }
            set { SetValue(CollapseIconProperty, value); }
        }
        #endregion

        #region CollapseIconTemplate DependencyProperty
        public static readonly DependencyProperty CollapseIconTemplateProperty = DependencyProperty.Register("CollapseIconTemplate",
            typeof(DataTemplate),
            typeof(HamburgerMenuItem),
            new PropertyMetadata(null));

        public DataTemplate CollapseIconTemplate
        {
            get { return (DataTemplate)GetValue(CollapseIconTemplateProperty); }
            set { SetValue(CollapseIconTemplateProperty, value); }
        }
        #endregion

        #region IconTemplateSelector DependencyProperty
        public static readonly DependencyProperty IconTemplateSelectorProperty = DependencyProperty.Register("IconTemplateSelector",
            typeof(DataTemplateSelector),
            typeof(HamburgerMenuItem),
            new PropertyMetadata(null));

        public DataTemplateSelector IconTemplateSelector
        {
            get { return (DataTemplateSelector)GetValue(IconTemplateSelectorProperty); }
            set { SetValue(IconTemplateSelectorProperty, value); }
        }
        #endregion

        #region Label DependencyProperty
        public static readonly DependencyProperty LabelProperty = DependencyProperty.Register("Label",
            typeof(object),
            typeof(HamburgerMenuItem),
            new PropertyMetadata(null));

        public object Label
        {
            get { return GetValue(LabelProperty); }
            set { SetValue(LabelProperty, value); }
        }
        #endregion

        #region LabelTemplate DependencyProperty
        public static readonly DependencyProperty LabelTemplateProperty = DependencyProperty.Register("LabelTemplate",
            typeof(DataTemplate),
            typeof(HamburgerMenuItem),
            new PropertyMetadata(null));

        public DataTemplate LabelTemplate
        {
            get { return (DataTemplate)GetValue(LabelTemplateProperty); }
            set { SetValue(LabelTemplateProperty, value); }
        }
        #endregion

        #region LabelTemplateSelector DependencyProperty
        public static readonly DependencyProperty LabelTemplateSelectorProperty = DependencyProperty.Register("LabelTemplateSelector",
            typeof(DataTemplateSelector),
            typeof(HamburgerMenuItem),
            new PropertyMetadata(null));

        public DataTemplateSelector LabelTemplateSelector
        {
            get { return (DataTemplateSelector)GetValue(LabelTemplateSelectorProperty); }
            set { SetValue(LabelTemplateSelectorProperty, value); }
        }
        #endregion

        #region Content DependencyProperty
        public static readonly DependencyProperty ContentProperty = DependencyProperty.Register("Content",
            typeof(object),
            typeof(HamburgerMenuItem),
            new PropertyMetadata(null));

        public object Content
        {
            get { return GetValue(ContentProperty); }
            set { SetValue(ContentProperty, value); }
        }
        #endregion

        #region ContentTemplate DependencyProperty
        public static readonly DependencyProperty ContentTemplateProperty = DependencyProperty.Register("ContentTemplate",
            typeof(DataTemplate),
            typeof(HamburgerMenuItem),
            new PropertyMetadata(null));

        public DataTemplate ContentTemplate
        {
            get { return (DataTemplate)GetValue(ContentTemplateProperty); }
            set { SetValue(ContentTemplateProperty, value); }
        }
        #endregion

        #region ContentTemplateSelector DependencyProperty
        public static readonly DependencyProperty ContentTemplateSelectorProperty = DependencyProperty.Register("ContentTemplateSelector",
            typeof(DataTemplateSelector),
            typeof(HamburgerMenuItem),
            new PropertyMetadata(null));

        public DataTemplateSelector ContentTemplateSelector
        {
            get { return (DataTemplateSelector)GetValue(ContentTemplateSelectorProperty); }
            set { SetValue(ContentTemplateSelectorProperty, value); }
        }
        #endregion

        #region IsExpanded DependencyProperty
        public static readonly DependencyProperty IsExpandedProperty = DependencyProperty.Register("IsExpanded",
            typeof(bool),
            typeof(HamburgerMenuItem),
            new PropertyMetadata(BooleanBoxes.FalseBox));

        public bool IsExpanded
        {
            get { return (bool)GetValue(IsExpandedProperty); }
            set { SetValue(IsExpandedProperty, BooleanBoxes.Box(value)); }
        }
        #endregion

        #region IsSelected DependencyProperty
        public static readonly DependencyProperty IsSelectedProperty = Selector.IsSelectedProperty.AddOwner(typeof(HamburgerMenuItem),
            new FrameworkPropertyMetadata(BooleanBoxes.FalseBox, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault | FrameworkPropertyMetadataOptions.Journal, OnIsSelectedChanged));

        private static void OnIsSelectedChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            var instance = (HamburgerMenuItem)sender;

            instance.OnIsSelectedChanged();
        }

        public bool IsSelected
        {
            get { return (bool)GetValue(IsSelectedProperty); }
            set { SetValue(IsSelectedProperty, BooleanBoxes.Box(value)); }
        }
        #endregion

        #region IsHighlighted ReadOnly DependencyProperty
        private static readonly DependencyPropertyKey IsHighlightedPropertyKey = DependencyProperty.RegisterReadOnly("IsHighlighted",
            typeof(bool),
            typeof(HamburgerMenuItem),
            new PropertyMetadata(BooleanBoxes.FalseBox));

        public static readonly DependencyProperty IsHighlightedProperty = IsHighlightedPropertyKey.DependencyProperty;

        public bool IsHighlighted
        {
            get { return (bool)GetValue(IsHighlightedProperty); }
            protected set { SetValue(IsHighlightedPropertyKey, BooleanBoxes.Box(value)); }
        }
        #endregion

        #region HasSelectedSubItem ReadOnly DependencyProperty
        private static readonly DependencyPropertyKey HasSelectedSubItemPropertyKey = DependencyProperty.RegisterReadOnly("HasSelectedSubItem",
            typeof(bool),
            typeof(HamburgerMenuItem),
            new PropertyMetadata(BooleanBoxes.FalseBox));

        public static readonly DependencyProperty HasSelectedSubItemProperty = HasSelectedSubItemPropertyKey.DependencyProperty;

        public bool HasSelectedSubItem
        {
            get { return (bool)GetValue(HasSelectedSubItemProperty); }
            protected set { SetValue(HasSelectedSubItemPropertyKey, BooleanBoxes.Box(value)); }
        }
        #endregion

        #region IsPressed ReadOnly DependencyProperty
        private static readonly DependencyPropertyKey IsPressedPropertyKey = DependencyProperty.RegisterReadOnly("IsPressed",
            typeof(bool),
            typeof(HamburgerMenuItem),
            new PropertyMetadata(BooleanBoxes.FalseBox));

        public static readonly DependencyProperty IsPressedProperty = IsPressedPropertyKey.DependencyProperty;

        public bool IsPressed
        {
            get { return (bool)GetValue(IsPressedProperty); }
            protected set { SetValue(IsPressedPropertyKey, BooleanBoxes.Box(value)); }
        }
        #endregion

        #region Role DependencyProperty
        public static readonly DependencyProperty RoleProperty = DependencyProperty.Register("Role",
            typeof(HamburgerMenuItemRole),
            typeof(HamburgerMenuItem),
            new PropertyMetadata(HamburgerMenuItemRole.Item));

        public HamburgerMenuItemRole Role
        {
            get { return (HamburgerMenuItemRole)GetValue(RoleProperty); }
            set { SetValue(RoleProperty, value); }
        }
        #endregion

        internal HamburgerMenu ParentMenu;

        internal HamburgerMenuItem ParentItem
        {
            get { return ParentItemsControl as HamburgerMenuItem; }
        }

        internal ItemsControl ParentItemsControl
        {
            get { return ItemsControlFromItemContainer(this); }
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            if (ParentMenu != null)
            {
                ParentMenu.MenuClosed -= ParentMenu_MenuClosed;
            }

            ParentMenu = this.ParentOfType<HamburgerMenu>();

            if (ParentMenu != null)
            {
                ParentMenu.MenuClosed += ParentMenu_MenuClosed;
            }
        }

        private void ParentMenu_MenuClosed(object sender, RoutedEventArgs e)
        {
            IsExpanded = false;
        }

        private static void OnSelected(object sender, RoutedEventArgs e)
        {
            var instance = (HamburgerMenuItem)sender;

            if (e.Source == instance) return;

            instance.HasSelectedSubItem = true;
        }

        private static void OnUnselected(object sender, RoutedEventArgs e)
        {
            var instance = (HamburgerMenuItem)sender;

            if (e.Source == instance) return;

            instance.HasSelectedSubItem = false;
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
            if (element is HamburgerMenuItem menuItem && item == ParentMenu?.SelectedItem)
            {
                menuItem.SetCurrentValue(IsSelectedProperty, true);
            }
        }

        protected override void ClearContainerForItemOverride(DependencyObject element, object item)
        {
            base.ClearContainerForItemOverride(element, item);

            // Wenn unser item ein HamburgerMenuItem ist, dann abbrechen
            if (item is HamburgerMenuItem) return;
            // Es wird davon ausgegangen, dass es sich bei dem Container um ein HamburgerMenuItem handelt
            // Von diesem Container soll nun der Wert der IsSelected-Property geleert werden
            element.ClearValue(IsSelectedProperty);
        }

        protected override void OnMouseEnter(MouseEventArgs e)
        {
            base.OnMouseEnter(e);

            if (ParentMenu != null) ParentMenu.Highlight(this);
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);
            // Wenn wir kein Highlight haben aber sich die Maus innerhalb von diesem Element bewegt,
            // wird hier geprüft, ob es sich um eines der Unter-Elemente handelt oder wir uns auf diesem Element direkt befinden
            if (!IsHighlighted && e.Source == this) ParentMenu.Highlight(this);
        }

        protected override void OnMouseLeave(MouseEventArgs e)
        {
            base.OnMouseLeave(e);

            if (ParentMenu != null) ParentMenu.Highlight(null);
            IsPressed = false;
        }

        protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            base.OnMouseLeftButtonDown(e);
            // Da wir Sub-Items haben können die dieses Event ebenfalls auslösen
            // e.Handled zu setzen möchte ich aber nicht also brechen wir einfach ab, wenn wir nicht in Source drin stehen
            if (e.Source != this) return;

            IsPressed = true;
        }

        protected override void OnMouseLeftButtonUp(MouseButtonEventArgs e)
        {
            base.OnMouseLeftButtonUp(e);
            // Da wir Sub-Items haben können die dieses Event ebenfalls auslösen
            // e.Handled zu setzen möchte ich aber nicht also brechen wir einfach ab, wenn wir nicht in Source drin stehen
            if (e.Source != this) return;

            if (IsPressed) OnClick();
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            base.OnKeyDown(e);

            if (e.Key == Key.Enter)
            {
                if (e.OriginalSource == this)
                {
                    IsPressed = false;
                    OnClick();
                }
            }
        }

        private static void OnAccessKeyPressed(object sender, AccessKeyPressedEventArgs e)
        {
            if (!e.Handled && e.Scope == null && e.Target == null)
            {
                e.Target = (UIElement)sender;
            }
        }

        protected override void OnAccessKey(AccessKeyEventArgs e)
        {
            if (e.IsMultiple)
            {
                base.OnAccessKey(e);
            }
            else OnClick();
        }

        protected virtual void OnIsSelectedChanged()
        {
            if (IsSelected)
            {
                RaiseEvent(new RoutedEventArgs(Selector.SelectedEvent, this));
            }
            else
            {
                RaiseEvent(new RoutedEventArgs(Selector.UnselectedEvent, this));
            }
        }

        protected virtual void OnClick()
        {
            IsPressed = false;

            if (!IsEnabled || Role != HamburgerMenuItemRole.Item) return;

            IsSelected = true;
            if (ParentMenu?.IsMenuOpen == true && HasItems) IsExpanded = !IsExpanded;

            var eventArgs = new RoutedEventArgs(ClickEvent, this);

            RaiseEvent(eventArgs);

            if (ParentMenu != null) ParentMenu.RaiseItemClicked(this);
        }

        internal void SetIsHighlighted(bool value)
        {
            IsHighlighted = value;
        }

        public override string ToString()
        {
            if (Label is string label && !string.IsNullOrWhiteSpace(label)) return label;
            return base.ToString();
        }
    }
}