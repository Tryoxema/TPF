using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using TPF.Controls.Specialized.TabControl;
using TPF.DragDrop;
using TPF.Internal;

namespace TPF.Controls
{
    public class TabControl : System.Windows.Controls.TabControl
    {
        static TabControl()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(TabControl), new FrameworkPropertyMetadata(typeof(TabControl)));

            RegisterCommands();
        }

        public TabControl()
        {
            DragDropManager.AddDragInitializeHandler(this, OnDragInitialize);
            DragOver += OnDragOver;

            ItemContainerGenerator.StatusChanged += ItemContainerGenerator_StatusChanged;

            _pinnedItems = new List<TabItem>();
            PinnedItems = new ReadOnlyCollection<TabItem>(_pinnedItems);
        }

        #region Closing RoutedEvent
        public static readonly RoutedEvent ClosingEvent = EventManager.RegisterRoutedEvent("Closing",
            RoutingStrategy.Bubble,
            typeof(ClosingEventHandler<TabItem>),
            typeof(TabControl));

        public event ClosingEventHandler<TabItem> Closing
        {
            add => AddHandler(ClosingEvent, value);
            remove => RemoveHandler(ClosingEvent, value);
        }
        #endregion

        #region Closed RoutedEvent
        public static readonly RoutedEvent ClosedEvent = EventManager.RegisterRoutedEvent("Closed",
            RoutingStrategy.Bubble,
            typeof(ClosedEventHandler<TabItem>),
            typeof(TabControl));

        public event ClosedEventHandler<TabItem> Closed
        {
            add => AddHandler(ClosedEvent, value);
            remove => RemoveHandler(ClosedEvent, value);
        }
        #endregion

        #region PreviewTabPinned RoutedEvent
        public static readonly RoutedEvent PreviewTabPinnedEvent = EventManager.RegisterRoutedEvent("PreviewTabPinned",
            RoutingStrategy.Tunnel,
            typeof(PreviewTabEventHandler),
            typeof(TabControl));

        public event PreviewTabEventHandler PreviewTabPinned
        {
            add => AddHandler(PreviewTabPinnedEvent, value);
            remove => RemoveHandler(PreviewTabPinnedEvent, value);
        }
        #endregion

        #region TabPinned RoutedEvent
        public static readonly RoutedEvent TabPinnedEvent = EventManager.RegisterRoutedEvent("TabPinned",
            RoutingStrategy.Bubble,
            typeof(TabEventHandler),
            typeof(TabControl));

        public event TabEventHandler TabPinned
        {
            add => AddHandler(TabPinnedEvent, value);
            remove => RemoveHandler(TabPinnedEvent, value);
        }
        #endregion

        #region PreviewTabUnpinned RoutedEvent
        public static readonly RoutedEvent PreviewTabUnpinnedEvent = EventManager.RegisterRoutedEvent("PreviewTabUnpinned",
            RoutingStrategy.Tunnel,
            typeof(PreviewTabEventHandler),
            typeof(TabControl));

        public event PreviewTabEventHandler PreviewTabUnpinned
        {
            add => AddHandler(PreviewTabUnpinnedEvent, value);
            remove => RemoveHandler(PreviewTabUnpinnedEvent, value);
        }
        #endregion

        #region TabUnpinned RoutedEvent
        public static readonly RoutedEvent TabUnpinnedEvent = EventManager.RegisterRoutedEvent("TabUnpinned",
            RoutingStrategy.Bubble,
            typeof(TabEventHandler),
            typeof(TabControl));

        public event TabEventHandler TabUnpinned
        {
            add => AddHandler(TabUnpinnedEvent, value);
            remove => RemoveHandler(TabUnpinnedEvent, value);
        }
        #endregion

        #region AddButtonClicked RoutedEvent
        public static readonly RoutedEvent AddButtonClickedEvent = EventManager.RegisterRoutedEvent("AddButtonClicked",
            RoutingStrategy.Bubble,
            typeof(RoutedEventHandler),
            typeof(TabControl));

        public event RoutedEventHandler AddButtonClicked
        {
            add => AddHandler(AddButtonClickedEvent, value);
            remove => RemoveHandler(AddButtonClickedEvent, value);
        }
        #endregion

        #region HeaderBackground DependencyProperty
        public static readonly DependencyProperty HeaderBackgroundProperty = DependencyProperty.Register("HeaderBackground",
            typeof(Brush),
            typeof(TabControl),
            new PropertyMetadata(null));

        public Brush HeaderBackground
        {
            get { return (Brush)GetValue(HeaderBackgroundProperty); }
            set { SetValue(HeaderBackgroundProperty, value); }
        }
        #endregion

        #region AllowDragReorder DependencyProperty
        public static readonly DependencyProperty AllowDragReorderProperty = DependencyProperty.Register("AllowDragReorder",
            typeof(bool),
            typeof(TabControl),
            new PropertyMetadata(BooleanBoxes.FalseBox, OnAllowDragReorderChanged));

        private static void OnAllowDragReorderChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            var instance = (TabControl)sender;

            instance.OnAllowDragReorderChanged();
        }

        public bool AllowDragReorder
        {
            get { return (bool)GetValue(AllowDragReorderProperty); }
            set { SetValue(AllowDragReorderProperty, BooleanBoxes.Box(value)); }
        }
        #endregion

        #region ShowAddButton DependencyProperty
        public static readonly DependencyProperty ShowAddButtonProperty = DependencyProperty.Register("ShowAddButton",
            typeof(bool),
            typeof(TabControl),
            new PropertyMetadata(BooleanBoxes.FalseBox));

        public bool ShowAddButton
        {
            get { return (bool)GetValue(ShowAddButtonProperty); }
            set { SetValue(ShowAddButtonProperty, BooleanBoxes.Box(value)); }
        }
        #endregion

        #region AddButtonContentTemplate DependencyProperty
        public static readonly DependencyProperty AddButtonContentTemplateProperty = DependencyProperty.Register("AddButtonContentTemplate",
            typeof(DataTemplate),
            typeof(TabControl),
            new PropertyMetadata(null));

        public DataTemplate AddButtonContentTemplate
        {
            get { return (DataTemplate)GetValue(AddButtonContentTemplateProperty); }
            set { SetValue(AddButtonContentTemplateProperty, value); }
        }
        #endregion

        #region ShowDropDownButton DependencyProperty
        public static readonly DependencyProperty ShowDropDownButtonProperty = DependencyProperty.Register("ShowDropDownButton",
            typeof(bool),
            typeof(TabControl),
            new PropertyMetadata(BooleanBoxes.FalseBox));

        public bool ShowDropDownButton
        {
            get { return (bool)GetValue(ShowDropDownButtonProperty); }
            set { SetValue(ShowDropDownButtonProperty, BooleanBoxes.Box(value)); }
        }
        #endregion

        #region DropDownButtonContentTemplate DependencyProperty
        public static readonly DependencyProperty DropDownButtonContentTemplateProperty = DependencyProperty.Register("DropDownButtonContentTemplate",
            typeof(DataTemplate),
            typeof(TabControl),
            new PropertyMetadata(null));

        public DataTemplate DropDownButtonContentTemplate
        {
            get { return (DataTemplate)GetValue(DropDownButtonContentTemplateProperty); }
            set { SetValue(DropDownButtonContentTemplateProperty, value); }
        }
        #endregion

        #region IsDropDownOpen DependencyProperty
        public static readonly DependencyProperty IsDropDownOpenProperty = DependencyProperty.Register("IsDropDownOpen",
            typeof(bool),
            typeof(TabControl),
            new PropertyMetadata(BooleanBoxes.FalseBox, OnIsDropDownOpenChanged));

        private static void OnIsDropDownOpenChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            var instance = (TabControl)sender;

            instance.OnIsDropDownOpenChanged();
        }

        public bool IsDropDownOpen
        {
            get { return (bool)GetValue(IsDropDownOpenProperty); }
            set { SetValue(IsDropDownOpenProperty, BooleanBoxes.Box(value)); }
        }
        #endregion

        #region ShowCloseButton Attached DependencyProperty
        public static readonly DependencyProperty ShowCloseButtonProperty = DependencyProperty.RegisterAttached("ShowCloseButton",
            typeof(bool),
            typeof(TabControl),
            new FrameworkPropertyMetadata(BooleanBoxes.FalseBox, FrameworkPropertyMetadataOptions.Inherits));

        public static object GetShowCloseButton(DependencyObject element)
        {
            return element.GetValue(ShowCloseButtonProperty);
        }

        public static void SetShowCloseButton(DependencyObject element, bool value)
        {
            element.SetValue(ShowCloseButtonProperty, BooleanBoxes.Box(value));
        }

        public bool ShowCloseButton
        {
            get { return (bool)GetValue(ShowCloseButtonProperty); }
            set { SetValue(ShowCloseButtonProperty, BooleanBoxes.Box(value)); }
        }
        #endregion

        #region ShowPinButton Attached DependencyProperty
        public static readonly DependencyProperty ShowPinButtonProperty = DependencyProperty.RegisterAttached("ShowPinButton",
            typeof(bool),
            typeof(TabControl),
            new FrameworkPropertyMetadata(BooleanBoxes.FalseBox, FrameworkPropertyMetadataOptions.Inherits));

        public static object GetShowPinButton(DependencyObject element)
        {
            return element.GetValue(ShowPinButtonProperty);
        }

        public static void SetShowPinButton(DependencyObject element, bool value)
        {
            element.SetValue(ShowPinButtonProperty, BooleanBoxes.Box(value));
        }

        public bool ShowPinButton
        {
            get { return (bool)GetValue(ShowPinButtonProperty); }
            set { SetValue(ShowPinButtonProperty, BooleanBoxes.Box(value)); }
        }
        #endregion

        #region CloseTabOnMiddleMouseButtonDown Attached DependencyProperty
        public static readonly DependencyProperty CloseTabOnMiddleMouseButtonDownProperty = DependencyProperty.RegisterAttached("CloseTabOnMiddleMouseButtonDown",
            typeof(bool),
            typeof(TabControl),
            new FrameworkPropertyMetadata(BooleanBoxes.FalseBox, FrameworkPropertyMetadataOptions.Inherits));

        public static object GetCloseTabOnMiddleMouseButtonDown(DependencyObject element)
        {
            return element.GetValue(CloseTabOnMiddleMouseButtonDownProperty);
        }

        public static void SetCloseTabOnMiddleMouseButtonDown(DependencyObject element, bool value)
        {
            element.SetValue(CloseTabOnMiddleMouseButtonDownProperty, BooleanBoxes.Box(value));
        }

        public bool CloseTabOnMiddleMouseButtonDown
        {
            get { return (bool)GetValue(CloseTabOnMiddleMouseButtonDownProperty); }
            set { SetValue(CloseTabOnMiddleMouseButtonDownProperty, BooleanBoxes.Box(value)); }
        }
        #endregion

        private readonly List<TabItem> _pinnedItems;
        public ReadOnlyCollection<TabItem> PinnedItems { get; private set; }

        internal readonly ObservableCollection<TabControlDropdownItem> DropDownItems = new ObservableCollection<TabControlDropdownItem>();

        private ScrollViewer ScrollViewer;
        private Panel TabPanel;
        private ButtonBase AddButton;
        private ItemsControl DropDownItemsPresenter;

        internal TabItem DragTarget { get; set; }

        #region Commands
        private static void RegisterCommands()
        {
            CommandManager.RegisterClassCommandBinding(typeof(TabItem), new CommandBinding(TabItemCommands.Close, OnCloseTabItemCommand, CanExecuteCloseTabItemCommand));
            CommandManager.RegisterClassCommandBinding(typeof(TabItem), new CommandBinding(TabItemCommands.TogglePin, OnTogglePinCommand, CanExecuteTogglePinCommand));
        }

        private static void CanExecuteCloseTabItemCommand(object sender, CanExecuteRoutedEventArgs e)
        {
            if (sender is TabItem item)
            {
                e.CanExecute = item.IsEnabled;
            }
        }

        private static void OnCloseTabItemCommand(object sender, ExecutedRoutedEventArgs e)
        {
            if (!(sender is TabItem tabItem)) return;

            var owner = tabItem.Owner;

            if (owner == null) return;

            var items = owner.GetItemsList();

            if (items == null) return;

            var closingEventArgs = new ClosingEventArgs<TabItem>(ClosingEvent, tabItem);
            owner.RaiseEvent(closingEventArgs);

            if (closingEventArgs.Cancel) return;

            var closedEventArgs = new ClosedEventArgs<TabItem>(ClosedEvent, tabItem);
            owner.RaiseEvent(closedEventArgs);

            var item = owner.ItemContainerGenerator.ItemFromContainer(tabItem);

            var itemsList = owner.GetItemsList();

            itemsList?.Remove(item);
            owner.RemoveFromPinnedItems(tabItem);
        }

        private static void CanExecuteTogglePinCommand(object sender, CanExecuteRoutedEventArgs e)
        {
            if (sender is TabItem item)
            {
                e.CanExecute = item.IsEnabled;
            }
        }

        private static void OnTogglePinCommand(object sender, ExecutedRoutedEventArgs e)
        {
            if (!(sender is TabItem tabItem)) return;

            tabItem.IsPinned = !tabItem.IsPinned;
        }
        #endregion

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            if (AddButton != null)
            {
                AddButton.Click -= AddButton_Click;
            }

            ScrollViewer = GetTemplateChild("PART_ScrollViewer") as ScrollViewer;
            TabPanel = GetTemplateChild("PART_TabPanel") as Panel;
            AddButton = GetTemplateChild("PART_AddButton") as ButtonBase;
            DropDownItemsPresenter = GetTemplateChild("PART_DropDownItemsPresenter") as ItemsControl;

            if (AddButton != null)
            {
                AddButton.Click += AddButton_Click;
            }
        }

        internal IList GetItemsList()
        {
            return ItemsSource != null ? ItemsSource as IList : Items;
        }

        protected override void ClearContainerForItemOverride(DependencyObject element, object item)
        {
            base.ClearContainerForItemOverride(element, item);

            var tabItem = element as TabItem;

            if (DragTarget == tabItem) DragTarget = null;

            RemoveFromPinnedItems(tabItem);

            var dropDownItem = DropDownItems.FirstOrDefault(x => x.TabItem == tabItem);

            if (dropDownItem != null) DropDownItems.Remove(dropDownItem);
        }

        private void OnAllowDragReorderChanged()
        {
            UpdateContainerAllowDrag();

            AllowDrop = AllowDragReorder;
        }

        private void UpdateContainerAllowDrag()
        {
            var items = GetItemsList();

            if (items == null) return;

            for (int i = 0; i < items.Count; i++)
            {
                var item = items[i];

                var container = ItemContainerGenerator.ContainerFromItem(item) as TabItem;

                if (container == null) continue;

                DragDropManager.SetAllowDrag(container, AllowDragReorder);
            }
        }

        private void OnDragInitialize(object sender, DragInitializeEventArgs e)
        {
            var tabItem = GetTabItemForObject(e.OriginalSource);

            // Gehört uns das TabItem?
            if (!IsOwnedContainer(tabItem)) return;

            // Ist aktuell ein TabItem als DragTarget ausgewählt?
            if (DragTarget == null)
            {
                e.Cancel = true;
                return;
            }

            // Ist DragReorder erlaubt?
            if (!AllowDragReorder) return;

            e.Handled = true;
            e.AllowedEffects = DragDropEffects.Move;
            e.Data = tabItem;
        }

        private void OnDragOver(object sender, DragEventArgs e)
        {
            var tabItem = GetTabItemForObject(e.OriginalSource);

            // Gehört uns das TabItem?
            if (!IsOwnedContainer(tabItem)) return;

            // Haben wir ein DragTarget und ist Umsortieren erlaubt?
            if (DragTarget == null || !AllowDragReorder) return;

            e.Handled = true;

            if (DragTarget != tabItem && DragTarget.IsPinned == tabItem.IsPinned)
            {
                var position = e.GetPosition(tabItem);

                OnDragMoveItems(DragTarget, tabItem, position);
            }
        }

        private static TabItem GetTabItemForObject(object source)
        {
            if (source is TabItem tabItem) return tabItem;

            if (source is UIElement element)
            {
                return element.ParentOfType<TabItem>();
            }

            return null;
        }

        private bool IsOwnedContainer(TabItem tabItem)
        {
            if (tabItem == null) return false;

            var item = ItemContainerGenerator.ItemFromContainer(tabItem);

            if (item == null || item == DependencyProperty.UnsetValue) return false;

            return true;
        }

        private void OnDragMoveItems(TabItem movingTabItem, TabItem targetTabItem, Point position)
        {
            var sourceItem = ItemContainerGenerator.ItemFromContainer(movingTabItem);
            var targetItem = ItemContainerGenerator.ItemFromContainer(targetTabItem);

            var items = GetItemsList();

            // Sicherstellen das alles benötigte da ist
            if (sourceItem == null || targetItem == null || items == null) return;

            var sourceIndex = items.IndexOf(sourceItem);
            var targetIndex = items.IndexOf(targetItem);

            // Richtung für das Tauschen bestimmen
            var direction = sourceIndex <= targetIndex ? 1 : -1;
            bool putBefore;

            if (TabStripPlacement == Dock.Top || TabStripPlacement == Dock.Bottom)
            {
                putBefore = position.X < targetTabItem.ActualWidth / 2;
            }
            else
            {
                putBefore = position.Y < targetTabItem.ActualHeight / 2;
            }

            // Anzahl an zu tauschender TabItems berechnen
            var switchCount = Math.Abs(sourceIndex - targetIndex);

            // Je nach Richtung und Position für das zu bewegende TabItem muss switchCount noch um einen verringert werden
            if (putBefore)
            {
                if (sourceIndex < targetIndex) switchCount--;
            }
            else
            {
                if (sourceIndex > targetIndex) switchCount--;
            }

            // Jetzt Items durchgehen und Positionen Tauschen
            for (int i = 1; i <= switchCount; i++)
            {
                var itemToMove = items[sourceIndex + (i * direction)];

                var container = ItemContainerGenerator.ContainerFromItem(itemToMove) as TabItem;
                // Da wir das Item aus der Collection entfernen kann der Container neu generiert werden.
                // Um den Pin-Status zu behalten müssen wir uns den also hier merken
                var isPinned = container?.IsPinned ?? false;

                if (isPinned) RemoveFromPinnedItems(container);

                items.Remove(itemToMove);
                // Neuen Index berechnen
                var newIndex = sourceIndex + ((i - 1) * direction);
                // Item an neuer Stelle wieder in Liste einfügen
                items.Insert(newIndex, itemToMove);

                // Wenn das TabItem vorher angepinnt war, dann das wieder tun
                if (isPinned)
                {
                    if (ItemContainerGenerator.ContainerFromIndex(newIndex) is TabItem newContainer)
                    {
                        newContainer.SetIsPinnedInternal(isPinned);
                        AddToPinnedItems(newContainer);
                    }
                }
            }
        }

        private void ItemContainerGenerator_StatusChanged(object sender, EventArgs e)
        {
            if (ItemContainerGenerator.Status != GeneratorStatus.ContainersGenerated) return;

            UpdateContainerAllowDrag();
        }

        internal bool TrySetPinnedState(TabItem tabItem, bool isPinned)
        {
            var item = ItemsSource != null ? ItemContainerGenerator.ItemFromContainer(tabItem) : null;

            if (isPinned)
            {
                if (RaisePreviewTabPinned(tabItem, item))
                {
                    PinItem(tabItem);
                    RaiseTabPinned(tabItem, item);
                    return true;
                }
            }
            else
            {
                if (RaisePreviewTabUnpinned(tabItem, item))
                {
                    UnpinItem(tabItem);
                    RaiseTabUnpinned(tabItem, item);
                    return true;
                }
            }

            return false;
        }

        // Preview-Event auslösen und Gegenteil von e.Cancel zurückgeben
        private bool RaisePreviewTabPinned(TabItem tabItem, object item)
        {
            var eventArgs = new PreviewTabEventArgs(PreviewTabPinnedEvent, tabItem, item);

            RaiseEvent(eventArgs);

            return !eventArgs.Cancel;
        }

        private void RaiseTabPinned(TabItem tabItem, object item)
        {
            var eventArgs = new TabEventArgs(TabPinnedEvent, tabItem, item);

            RaiseEvent(eventArgs);
        }

        // Preview-Event auslösen und Gegenteil von e.Cancel zurückgeben
        private bool RaisePreviewTabUnpinned(TabItem tabItem, object item)
        {
            var eventArgs = new PreviewTabEventArgs(PreviewTabUnpinnedEvent, tabItem, item);

            RaiseEvent(eventArgs);

            return !eventArgs.Cancel;
        }

        private void RaiseTabUnpinned(TabItem tabItem, object item)
        {
            var eventArgs = new TabEventArgs(TabUnpinnedEvent, tabItem, item);

            RaiseEvent(eventArgs);
        }

        private void PinItem(TabItem tabItem)
        {
            SetItemPinState(tabItem, true);
        }

        private void UnpinItem(TabItem tabItem)
        {
            SetItemPinState(tabItem, false);
        }

        private void SetItemPinState(TabItem tabItem, bool pin)
        {
            var targetIndex = pin ? _pinnedItems.Count : _pinnedItems.Count - 1;

            var currentIndex = ItemContainerGenerator.IndexFromContainer(tabItem);

            var item = ItemContainerGenerator.ItemFromContainer(tabItem);

            if (targetIndex != currentIndex)
            {
                var itemsList = GetItemsList();

                if (itemsList == null) return;

                // Wer weiß, sicher ist sicher
                if (item == null) return;

                var selected = tabItem.IsSelected;

                itemsList.Remove(item);
                itemsList.Insert(targetIndex, item);

                if (selected)
                {
                    SelectedItem = item;
                    Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Background, new Action(() => ScrollIntoView(item)));
                }
            }

            var container = ItemContainerGenerator.ContainerFromItem(item) as TabItem;

            if (pin) AddToPinnedItems(container);
            else RemoveFromPinnedItems(tabItem);
        }

        private void AddToPinnedItems(TabItem tabItem)
        {
            if (_pinnedItems.Contains(tabItem)) return;

            _pinnedItems.Add(tabItem);
        }

        private void RemoveFromPinnedItems(TabItem tabItem)
        {
            _pinnedItems.Remove(tabItem);
        }

        public void ScrollIntoView(object item)
        {
            // Wenn kein ScrollViewer oder item vorhanden ist können wir auch nicht scrollen
            if (ScrollViewer == null || item == null) return;

            var index = Items.IndexOf(item);

            // Ist das Item in der Liste?
            if (index == -1) return;

            var container = ItemContainerGenerator.ContainerFromIndex(index) as TabItem;

            ScrollContainerIntoView(container);
        }

        private void ScrollContainerIntoView(TabItem tabItem)
        {
            if (ScrollViewer == null || TabPanel == null) return;

            var scrollAdjustment = 0.0;

            if (TabStripPlacement == Dock.Top || TabStripPlacement == Dock.Bottom)
            {
                if (ScrollViewer.ComputedHorizontalScrollBarVisibility == Visibility.Collapsed) return;

                // Linke und Rechte Kante bestimmen
                var leftEdge = tabItem.TransformToVisual(TabPanel).Transform(new Point()).X;
                var rightEdge = leftEdge + tabItem.ActualWidth;

                var margin = TabPanel.Margin.Left + TabPanel.Margin.Right;

                // Ist die Linke Kante vor dem Viewport?
                if (leftEdge - ScrollViewer.HorizontalOffset < 0) scrollAdjustment = leftEdge - ScrollViewer.HorizontalOffset + margin;
                // Ist die Rechte Kante nach dem Viewport?
                else if (rightEdge - ScrollViewer.HorizontalOffset > ScrollViewer.ViewportWidth) scrollAdjustment = rightEdge - ScrollViewer.HorizontalOffset - ScrollViewer.ViewportWidth + margin;

                ScrollViewer.ScrollToHorizontalOffset(scrollAdjustment);
            }
            else
            {
                if (ScrollViewer.ComputedVerticalScrollBarVisibility == Visibility.Collapsed) return;

                // Obere und Untere Kante bestimmen
                var topEdge = tabItem.TransformToVisual(TabPanel).Transform(new Point()).Y;
                var bottomEdge = topEdge + tabItem.ActualHeight;

                var margin = TabPanel.Margin.Top + TabPanel.Margin.Bottom;

                // Ist die Obere Kante vor dem Viewport?
                if (topEdge - ScrollViewer.VerticalOffset < 0) scrollAdjustment = topEdge - ScrollViewer.VerticalOffset + margin;
                // Ist die Untere Kante nach dem Viewport?
                else if (bottomEdge - ScrollViewer.VerticalOffset > ScrollViewer.ViewportHeight) scrollAdjustment = bottomEdge - ScrollViewer.VerticalOffset - ScrollViewer.ViewportHeight + margin;

                ScrollViewer.ScrollToVerticalOffset(scrollAdjustment);
            }
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            var eventArgs = new RoutedEventArgs(AddButtonClickedEvent);

            RaiseEvent(eventArgs);
        }

        private void OnIsDropDownOpenChanged()
        {
            DropDownItems.Clear();

            if (IsDropDownOpen)
            {
                for (int i = 0; i < Items.Count; i++)
                {
                    var item = Items[i];

                    if (ItemContainerGenerator.ContainerFromItem(item) is TabItem tabItem)
                    {
                        // Nur sichtbare TabItems anzeigen
                        if (tabItem.Visibility != Visibility.Visible) continue;

                        var dropDownItem = new TabControlDropdownItem(tabItem);

                        DropDownItems.Add(dropDownItem);
                    }
                }

                if (DropDownItemsPresenter != null) DropDownItemsPresenter.ItemsSource = DropDownItems;
            }
        }
    }
}