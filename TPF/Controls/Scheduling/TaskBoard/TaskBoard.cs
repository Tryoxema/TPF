using System;
using System.Linq;
using System.Windows;
using System.Windows.Markup;
using System.Windows.Controls;
using System.ComponentModel;
using System.Collections;
using System.Collections.Specialized;
using TPF.Internal;
using TPF.Controls.Specialized.TaskBoard;
using TPF.DragDrop.Behaviors;
using System.Windows.Media;
using System.Windows.Data;

namespace TPF.Controls
{
    [ContentProperty("Columns")]
    public class TaskBoard : Control
    {
        static TaskBoard()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(TaskBoard), new FrameworkPropertyMetadata(typeof(TaskBoard)));
        }

        public TaskBoard()
        {
            IndicatorMapping = new BrushMapCollection();
            Columns = new TaskBoardColumnCollection();
            DragDropBehavior = new TaskBoardDragDropBehavior();
        }

        #region CardClicked RoutedEvent
        public static readonly RoutedEvent CardClickedEvent = EventManager.RegisterRoutedEvent("CardClicked",
            RoutingStrategy.Bubble,
            typeof(CardClickedEventHandler),
            typeof(TaskBoard));

        public event CardClickedEventHandler CardClicked
        {
            add => AddHandler(CardClickedEvent, value);
            remove => RemoveHandler(CardClickedEvent, value);
        }
        #endregion

        #region CardDoubleClicked RoutedEvent
        public static readonly RoutedEvent CardDoubleClickedEvent = EventManager.RegisterRoutedEvent("CardDoubleClicked",
            RoutingStrategy.Bubble,
            typeof(CardClickedEventHandler),
            typeof(TaskBoard));

        public event CardClickedEventHandler CardDoubleClicked
        {
            add => AddHandler(CardDoubleClickedEvent, value);
            remove => RemoveHandler(CardDoubleClickedEvent, value);
        }
        #endregion

        #region IndicatorMapping DependencyProperty
        public static readonly DependencyProperty IndicatorMappingProperty = DependencyProperty.Register("IndicatorMapping",
            typeof(BrushMapCollection),
            typeof(TaskBoard),
            new PropertyMetadata(null, null, ConstrainIndicatorMappingValue));

        internal static object ConstrainIndicatorMappingValue(DependencyObject sender, object value)
        {
            return value ?? new BrushMapCollection();
        }

        public BrushMapCollection IndicatorMapping
        {
            get { return (BrushMapCollection)GetValue(IndicatorMappingProperty); }
            set { SetValue(IndicatorMappingProperty, value); }
        }
        #endregion

        #region Columns DependencyProperty
        public static readonly DependencyProperty ColumnsProperty = DependencyProperty.Register("Columns",
            typeof(TaskBoardColumnCollection),
            typeof(TaskBoard),
            new PropertyMetadata(null, OnColumnsChanged, ConstrainColumnsValue));

        private static void OnColumnsChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            var instance = (TaskBoard)sender;

            if (e.OldValue != null && e.OldValue is INotifyCollectionChanged oldCollection)
            {
                instance.UnHookColumns(oldCollection);
            }

            if (e.NewValue != null)
            {
                if (e.NewValue is INotifyCollectionChanged newCollection)
                {
                    instance.HookupColumns(newCollection);
                }

                var columns = e.NewValue as TaskBoardColumnCollection;

                for (int i = 0; i < columns.Count; i++)
                {
                    var column = columns[i];

                    column.TaskBoard = instance;
                    instance.SetDragDropProperties(column);
                }
            }
        }

        internal static object ConstrainColumnsValue(DependencyObject sender, object value)
        {
            return value ?? new TaskBoardColumnCollection();
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public TaskBoardColumnCollection Columns
        {
            get { return (TaskBoardColumnCollection)GetValue(ColumnsProperty); }
            set { SetValue(ColumnsProperty, value); }
        }
        #endregion

        #region ItemsSource DependencyProperty
        public static readonly DependencyProperty ItemsSourceProperty = DependencyProperty.Register("ItemsSource",
            typeof(IEnumerable),
            typeof(TaskBoard),
            new PropertyMetadata(null, OnItemsSourceChanged));

        private static void OnItemsSourceChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            var instance = (TaskBoard)sender;

            instance.OnItemsSourceChanged();
        }

        public IEnumerable ItemsSource
        {
            get { return (IEnumerable)GetValue(ItemsSourceProperty); }
            set { SetValue(ItemsSourceProperty, value); }
        }
        #endregion

        #region ColumnMappingPath DependencyProperty
        public static readonly DependencyProperty ColumnMappingPathProperty = DependencyProperty.Register("ColumnMappingPath",
            typeof(string),
            typeof(TaskBoard),
            new PropertyMetadata(null, OnColumnMappingPathChanged));

        private static void OnColumnMappingPathChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            var instance = (TaskBoard)sender;

            instance.OnColumnMappingPathChanged();
        }

        public string ColumnMappingPath
        {
            get { return (string)GetValue(ColumnMappingPathProperty); }
            set { SetValue(ColumnMappingPathProperty, value); }
        }
        #endregion

        #region AutoGenerateColumns DependencyProperty
        public static readonly DependencyProperty AutoGenerateColumnsProperty = DependencyProperty.Register("AutoGenerateColumns",
            typeof(bool),
            typeof(TaskBoard),
            new PropertyMetadata(BooleanBoxes.FalseBox));

        public bool AutoGenerateColumns
        {
            get { return (bool)GetValue(AutoGenerateColumnsProperty); }
            set { SetValue(AutoGenerateColumnsProperty, BooleanBoxes.Box(value)); }
        }
        #endregion

        #region Header DependencyProperty
        public static readonly DependencyProperty HeaderProperty = DependencyProperty.Register("Header",
            typeof(object),
            typeof(TaskBoard),
            new PropertyMetadata(null));

        public object Header
        {
            get { return GetValue(HeaderProperty); }
            set { SetValue(HeaderProperty, value); }
        }
        #endregion

        #region HeaderTemplate DependencyProperty
        public static readonly DependencyProperty HeaderTemplateProperty = DependencyProperty.Register("HeaderTemplate",
            typeof(DataTemplate),
            typeof(TaskBoard),
            new PropertyMetadata(null));

        public DataTemplate HeaderTemplate
        {
            get { return (DataTemplate)GetValue(HeaderTemplateProperty); }
            set { SetValue(HeaderTemplateProperty, value); }
        }
        #endregion

        #region HeaderTemplateSelector DependencyProperty
        public static readonly DependencyProperty HeaderTemplateSelectorProperty = DependencyProperty.Register("HeaderTemplateSelector",
            typeof(DataTemplateSelector),
            typeof(TaskBoard),
            new PropertyMetadata(null));

        public DataTemplateSelector HeaderTemplateSelector
        {
            get { return (DataTemplateSelector)GetValue(HeaderTemplateSelectorProperty); }
            set { SetValue(HeaderTemplateSelectorProperty, value); }
        }
        #endregion

        #region HeaderTextAlignment DependencyProperty
        public static readonly DependencyProperty HeaderTextAlignmentProperty = DependencyProperty.Register("HeaderTextAlignment",
            typeof(TextAlignment),
            typeof(TaskBoard),
            new PropertyMetadata(TextAlignment.Center));

        public TextAlignment HeaderTextAlignment
        {
            get { return (TextAlignment)GetValue(HeaderTextAlignmentProperty); }
            set { SetValue(HeaderTextAlignmentProperty, value); }
        }
        #endregion

        #region HeaderForeground DependencyProperty
        public static readonly DependencyProperty HeaderForegroundProperty = DependencyProperty.Register("HeaderForeground",
            typeof(Brush),
            typeof(TaskBoard),
            new PropertyMetadata(null));

        public Brush HeaderForeground
        {
            get { return (Brush)GetValue(HeaderForegroundProperty); }
            set { SetValue(HeaderForegroundProperty, value); }
        }
        #endregion

        #region HeaderBackground DependencyProperty
        public static readonly DependencyProperty HeaderBackgroundProperty = DependencyProperty.Register("HeaderBackground",
            typeof(Brush),
            typeof(TaskBoard),
            new PropertyMetadata(null));

        public Brush HeaderBackground
        {
            get { return (Brush)GetValue(HeaderBackgroundProperty); }
            set { SetValue(HeaderBackgroundProperty, value); }
        }
        #endregion

        #region HeaderVisibility DependencyProperty
        public static readonly DependencyProperty HeaderVisibilityProperty = DependencyProperty.Register("HeaderVisibility",
            typeof(Visibility),
            typeof(TaskBoard),
            new PropertyMetadata(Visibility.Collapsed));

        public Visibility HeaderVisibility
        {
            get { return (Visibility)GetValue(HeaderVisibilityProperty); }
            set { SetValue(HeaderVisibilityProperty, value); }
        }
        #endregion

        #region HeaderPadding DependencyProperty
        public static readonly DependencyProperty HeaderPaddingProperty = DependencyProperty.Register("HeaderPadding",
            typeof(Thickness),
            typeof(TaskBoard),
            new PropertyMetadata(default(Thickness)));

        public Thickness HeaderPadding
        {
            get { return (Thickness)GetValue(HeaderPaddingProperty); }
            set { SetValue(HeaderPaddingProperty, value); }
        }
        #endregion

        #region CornerRadius DependencyProperty
        public static readonly DependencyProperty CornerRadiusProperty = DependencyProperty.Register("CornerRadius",
            typeof(CornerRadius),
            typeof(TaskBoard),
            new PropertyMetadata(default(CornerRadius)));

        public CornerRadius CornerRadius
        {
            get { return (CornerRadius)GetValue(CornerRadiusProperty); }
            set { SetValue(CornerRadiusProperty, value); }
        }
        #endregion

        #region ColumnGap DependencyProperty
        public static readonly DependencyProperty ColumnGapProperty = DependencyProperty.Register("ColumnGap",
            typeof(double),
            typeof(TaskBoard),
            new PropertyMetadata(2.0, OnColumnLayoutPropertyChanged, ConstrainColumnGapValue));

        internal static object ConstrainColumnGapValue(DependencyObject sender, object value)
        {
            var doubleValue = (double)value;

            if (doubleValue < 0.0) doubleValue = 0;

            return doubleValue;
        }

        public double ColumnGap
        {
            get { return (double)GetValue(ColumnGapProperty); }
            set { SetValue(ColumnGapProperty, value); }
        }
        #endregion

        #region ColumnWidth DependencyProperty
        public static readonly DependencyProperty ColumnWidthProperty = DependencyProperty.Register("ColumnWidth",
            typeof(double),
            typeof(TaskBoard),
            new PropertyMetadata(200.0, OnColumnLayoutPropertyChanged, ConstrainColumnWidthValue));

        internal static object ConstrainColumnWidthValue(DependencyObject sender, object value)
        {
            var doubleValue = (double)value;

            if (doubleValue < 0.0) doubleValue = 0;

            return doubleValue;
        }

        public double ColumnWidth
        {
            get { return (double)GetValue(ColumnWidthProperty); }
            set { SetValue(ColumnWidthProperty, value); }
        }
        #endregion

        #region CollapsedColumnWidth DependencyProperty
        public static readonly DependencyProperty CollapsedColumnWidthProperty = DependencyProperty.Register("CollapsedColumnWidth",
            typeof(double),
            typeof(TaskBoard),
            new PropertyMetadata(40.0, OnColumnLayoutPropertyChanged, ConstrainCollapsedColumnWidthValue));

        internal static object ConstrainCollapsedColumnWidthValue(DependencyObject sender, object value)
        {
            var doubleValue = (double)value;

            if (doubleValue < 0.0) doubleValue = 0;

            return doubleValue;
        }

        public double CollapsedColumnWidth
        {
            get { return (double)GetValue(CollapsedColumnWidthProperty); }
            set { SetValue(CollapsedColumnWidthProperty, value); }
        }
        #endregion

        #region CanUserCollapseColumns DependencyProperty
        public static readonly DependencyProperty CanUserCollapseColumnsProperty = DependencyProperty.Register("CanUserCollapseColumns",
            typeof(bool),
            typeof(TaskBoard),
            new PropertyMetadata(BooleanBoxes.TrueBox));
   
        public bool CanUserCollapseColumns
        {
            get { return (bool)GetValue(CanUserCollapseColumnsProperty); }
            set { SetValue(CanUserCollapseColumnsProperty, BooleanBoxes.Box(value)); }
        }
        #endregion

        #region ColumnStateIndicatorSelector DependencyProperty
        public static readonly DependencyProperty ColumnStateIndicatorSelectorProperty = DependencyProperty.Register("ColumnStateIndicatorSelector",
            typeof(TaskBoardColumnStateIndicatorSelector),
            typeof(TaskBoard),
            new PropertyMetadata(null));

        public TaskBoardColumnStateIndicatorSelector ColumnStateIndicatorSelector
        {
            get { return (TaskBoardColumnStateIndicatorSelector)GetValue(ColumnStateIndicatorSelectorProperty); }
            set { SetValue(ColumnStateIndicatorSelectorProperty, value); }
        }
        #endregion

        #region ItemTemplate DependencyProperty
        public static readonly DependencyProperty ItemTemplateProperty = DependencyProperty.Register("ItemTemplate",
            typeof(DataTemplate),
            typeof(TaskBoard),
            new PropertyMetadata(null));

        public DataTemplate ItemTemplate
        {
            get { return (DataTemplate)GetValue(ItemTemplateProperty); }
            set { SetValue(ItemTemplateProperty, value); }
        }
        #endregion

        #region ItemTemplateSelector DependencyProperty
        public static readonly DependencyProperty ItemTemplateSelectorProperty = DependencyProperty.Register("ItemTemplateSelector",
            typeof(DataTemplateSelector),
            typeof(TaskBoard),
            new PropertyMetadata(null));

        public DataTemplateSelector ItemTemplateSelector
        {
            get { return (DataTemplateSelector)GetValue(ItemTemplateSelectorProperty); }
            set { SetValue(ItemTemplateSelectorProperty, value); }
        }
        #endregion

        #region ItemContainerStyle DependencyProperty
        public static readonly DependencyProperty ItemContainerStyleProperty = DependencyProperty.Register("ItemContainerStyle",
            typeof(Style),
            typeof(TaskBoard),
            new PropertyMetadata(null));

        public Style ItemContainerStyle
        {
            get { return (Style)GetValue(ItemContainerStyleProperty); }
            set { SetValue(ItemContainerStyleProperty, value); }
        }
        #endregion

        #region ItemContainerStyleSelector DependencyProperty
        public static readonly DependencyProperty ItemContainerStyleSelectorProperty = DependencyProperty.Register("ItemContainerStyleSelector",
            typeof(StyleSelector),
            typeof(TaskBoard),
            new PropertyMetadata(null));

        public StyleSelector ItemContainerStyleSelector
        {
            get { return (StyleSelector)GetValue(ItemContainerStyleSelectorProperty); }
            set { SetValue(ItemContainerStyleSelectorProperty, value); }
        }
        #endregion

        #region MinimumCardHeight DependencyProperty
        public static readonly DependencyProperty MinimumCardHeightProperty = DependencyProperty.Register("MinimumCardHeight",
            typeof(double),
            typeof(TaskBoard),
            new PropertyMetadata(0.0, null, ConstrainMinimumValue));

        internal static object ConstrainMinimumValue(DependencyObject sender, object value)
        {
            var instance = (TaskBoard)sender;

            var doubleValue = (double)value;

            if (doubleValue > instance.MaximumCardHeight) doubleValue = instance.MaximumCardHeight;

            return doubleValue;
        }

        public double MinimumCardHeight
        {
            get { return (double)GetValue(MinimumCardHeightProperty); }
            set { SetValue(MinimumCardHeightProperty, value); }
        }
        #endregion

        #region MaximumCardHeight DependencyProperty
        public static readonly DependencyProperty MaximumCardHeightProperty = DependencyProperty.Register("MaximumCardHeight",
            typeof(double),
            typeof(TaskBoard),
            new PropertyMetadata(150.0, null, ConstrainMaximumValue));

        internal static object ConstrainMaximumValue(DependencyObject sender, object value)
        {
            var instance = (TaskBoard)sender;

            var doubleValue = (double)value;

            if (doubleValue < instance.MinimumCardHeight) doubleValue = instance.MinimumCardHeight;

            return doubleValue;
        }

        public double MaximumCardHeight
        {
            get { return (double)GetValue(MaximumCardHeightProperty); }
            set { SetValue(MaximumCardHeightProperty, value); }
        }
        #endregion

        #region DragDropBehavior DependencyProperty
        public static readonly DependencyProperty DragDropBehaviorProperty = DependencyProperty.Register("DragDropBehavior",
            typeof(TaskBoardDragDropBehavior),
            typeof(TaskBoard),
            new PropertyMetadata(null, OnDragDropBehaviorChanged, ConstrainDragDropBehaviorValue));

        private static void OnDragDropBehaviorChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            var instance = (TaskBoard)sender;

            for (int i = 0; i < instance.Columns.Count; i++)
            {
                var column = instance.Columns[i];

                instance.SetDragDropProperties(column);
            }
        }

        internal static object ConstrainDragDropBehaviorValue(DependencyObject sender, object value)
        {
            if (value == null) return new TaskBoardDragDropBehavior();
            else return value;
        }

        public TaskBoardDragDropBehavior DragDropBehavior
        {
            get { return (TaskBoardDragDropBehavior)GetValue(DragDropBehaviorProperty); }
            set { SetValue(DragDropBehaviorProperty, value); }
        }
        #endregion

        #region DragVisualProvider DependencyProperty
        public static readonly DependencyProperty DragVisualProviderProperty = DependencyProperty.Register("DragVisualProvider",
            typeof(IDragVisualProvider),
            typeof(TaskBoard),
            new PropertyMetadata(null, OnDragVisualProviderChanged));

        private static void OnDragVisualProviderChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            var instance = (TaskBoard)sender;

            for (int i = 0; i < instance.Columns.Count; i++)
            {
                var column = instance.Columns[i];

                instance.SetDragDropProperties(column);
            }
        }

        public IDragVisualProvider DragVisualProvider
        {
            get { return (IDragVisualProvider)GetValue(DragVisualProviderProperty); }
            set { SetValue(DragVisualProviderProperty, value); }
        }
        #endregion

        #region DropVisualProvider DependencyProperty
        public static readonly DependencyProperty DropVisualProviderProperty = DependencyProperty.Register("DropVisualProvider",
            typeof(IDropVisualProvider),
            typeof(TaskBoard),
            new PropertyMetadata(null, OnDropVisualProviderChanged));

        private static void OnDropVisualProviderChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            var instance = (TaskBoard)sender;

            for (int i = 0; i < instance.Columns.Count; i++)
            {
                var column = instance.Columns[i];

                instance.SetDragDropProperties(column);
            }
        }

        public IDropVisualProvider DropVisualProvider
        {
            get { return (IDropVisualProvider)GetValue(DropVisualProviderProperty); }
            set { SetValue(DropVisualProviderProperty, value); }
        }
        #endregion

        private static void OnColumnLayoutPropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            var instance = (TaskBoard)sender;

            if (instance.ColumnsControl == null) return;

            var panel = instance.ColumnsControl.ChildOfType<Panel>();

            if (panel == null) return;

            panel.InvalidateMeasure();
            panel.InvalidateArrange();
        }

        public event EventHandler<TaskBoardAutoGeneratingColumnEventArgs> AutoGeneratingColumn;
        public event EventHandler<CardDragEndingEventArgs> CardDragEnding;
        public event EventHandler<CardDragEndedEventArgs> CardDragEnded;

        internal ItemsControl ColumnsControl;

        internal CollectionViewSource GroupedCollection;

        private bool _suppressItemsReset;

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            ColumnsControl = GetTemplateChild("PART_ColumnsControl") as ItemsControl;
        }

        internal void ItemClicked(TaskBoardItem item)
        {
            var eventArgs = new CardClickedEventArgs(CardClickedEvent, item);

            OnCardClicked(eventArgs);
        }

        internal void ItemDoubleClicked(TaskBoardItem item)
        {
            var eventArgs = new CardClickedEventArgs(CardDoubleClickedEvent, item);

            OnCardDoubleClicked(eventArgs);
        }

        protected virtual void OnAutoGeneratingColumn(TaskBoardAutoGeneratingColumnEventArgs e)
        {
            AutoGeneratingColumn?.Invoke(this, e);
        }

        protected virtual void OnCardClicked(CardClickedEventArgs e)
        {
            RaiseEvent(e);
        }

        protected virtual void OnCardDoubleClicked(CardClickedEventArgs e)
        {
            RaiseEvent(e);
        }

        protected internal virtual void OnCardDragEnding(CardDragEndingEventArgs e)
        {
            CardDragEnding?.Invoke(this, e);
        }

        protected internal virtual void OnCardDragEnded(CardDragEndedEventArgs e)
        {
            CardDragEnded?.Invoke(this, e);
        }

        private void GroupedView_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                {
                    AddItems(e.NewItems);
                    break;
                }
                case NotifyCollectionChangedAction.Remove:
                {
                    RemoveItems(e.OldItems);
                    break;
                }
                case NotifyCollectionChangedAction.Reset:
                {
                    ResetItems();
                    break;
                }
            }
        }

        private void HookupItem(object item)
        {
            if (item is INotifyPropertyChanged notifyItem)
            {
                // Zuerst deabonieren wir den Handler einmal aus Sicherheitsgründen, damit keiner doppelt dranhängt
                notifyItem.PropertyChanged -= Item_PropertyChanged;
                // Danach den Handler an das Item hängen
                notifyItem.PropertyChanged += Item_PropertyChanged;
            }
        }

        private void UnhookItem(object item)
        {
            if (item is INotifyPropertyChanged notifyItem)
            {
                notifyItem.PropertyChanged -= Item_PropertyChanged;
            }
        }

        private void Item_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (!e.PropertyName.Equals(ColumnMappingPath, StringComparison.Ordinal)) return;

            // Die aktuelle Spalte rausfinden
            var oldColumn = GetColumnContainingItem(sender);

            // Den aktuellen Wert der Property bestimmen
            var value = PropertyHelper.GetPropertyValueFromPath(sender, e.PropertyName);

            // Die passende Spalte für den neuen Wert finden oder generieren
            var newColumn = GetOrGenerateColumn(value);

            // Unterscheidet sich die neue Spalte von der alten?
            if (newColumn != oldColumn)
            {
                // Wenn das item vorher in einer Spalte war, dann aus dieser entfernen
                if (oldColumn != null) oldColumn.Items.Remove(sender);

                // Wenn eine Spalte gefunden oder generiert wurde, dann item dort einfügen
                if (newColumn != null) newColumn.Items.Add(sender);
            }
        }

        private void OnItemsSourceChanged()
        {
            if (GroupedCollection != null)
            {
                // Wenn wir vorger schon eine Collection hatten, dann das CollectionChanged-Event deabonieren und alle Items rauswerfen
                GroupedCollection.View.CollectionChanged -= GroupedView_CollectionChanged;

                ClearColumns();

                // Hier nochmal zur sicherheit alle Items durchgehen, damit selbst von Items die nicht in Columns waren der Eventhandler entfernt wird
                // Im Idealfall ist das zwar unnötig, aber immer noch besser als MemoryLeaks zu riskieren
                foreach (var item in GroupedCollection.View)
                {
                    UnhookItem(item);
                }
            }

            if (ItemsSource != null)
            {
                GroupedCollection = new CollectionViewSource() { Source = ItemsSource };

                GroupedCollection.View.CollectionChanged += GroupedView_CollectionChanged;

                ApplyGrouping();
            }
            else GroupedCollection = null;
        }

        private void OnColumnMappingPathChanged()
        {
            ApplyGrouping();
        }

        // Leert alle Spalten, löscht die automatisch generierten und entfernt den INotifyPropertyChanged Eventhandler von allen Items in der Spalte
        private void ClearColumns()
        {
            for (int i = 0; i < Columns.Count; i++)
            {
                var column = Columns[i];

                for (int j = 0; j < column.Items.Count; j++)
                {
                    var item = column.Items[j];

                    UnhookItem(item);
                }
            }

            Columns.DeleteAutoGeneratedColumns();
            Columns.ClearColumns();
        }

        private void ApplyGrouping()
        {
            if (GroupedCollection == null) return;

            _suppressItemsReset = true;
            GroupedCollection.GroupDescriptions.Clear();
            _suppressItemsReset = false;

            if (ColumnMappingPath == null) return;

            // Hier der Collection die Gruppierung hinzufügen.
            // Das löst ein Reset auf der Collection aus, bei dem alles neu zugewiesen wird
            GroupedCollection.GroupDescriptions.Add(new PropertyGroupDescription(ColumnMappingPath));
        }

        // Ordnet die Items aus den einzelnen Gruppen den entsprechenden Spalten zu und hängt den INotifyPropertyChanged Eventhandler dran
        private void AssignGroups()
        {
            if (GroupedCollection == null || GroupedCollection.View?.Groups == null) return;

            foreach (CollectionViewGroup group in GroupedCollection.View.Groups)
            {
                var column = GetOrGenerateColumn(group.Name);

                // Wenn es die Spalte nicht gibt überspringen wir die Gruppe
                if (column == null) continue;

                for (int i = 0; i < group.Items.Count; i++)
                {
                    var item = group.Items[i];

                    HookupItem(item);

                    column.Items.Add(item);
                }
            }
        }

        private TaskBoardColumn GetOrGenerateColumn(object value)
        {
            var column = Columns.GetColumnFromValue(value);

            // Gibt es die Spalte noch nicht und darf das TaskBoard Columns generieren?
            if (column == null && AutoGenerateColumns)
            {
                column = GenerateColumn(value);

                if (column != null) Columns.Add(column);
            }

            return column;
        }

        private TaskBoardColumn GenerateColumn(object value)
        {
            var e = new TaskBoardAutoGeneratingColumnEventArgs(new TaskBoardColumn()
            {
                Value = value,
                Header = value,
                IsAutoGenerated = true
            });

            AutoGeneratingColumn?.Invoke(this, e);

            return e.Cancel ? null : e.Column;
        }

        private TaskBoardColumn GetColumnContainingItem(object item)
        {
            for (int i = 0; i < Columns.Count; i++)
            {
                var column = Columns[i];

                if (column.Items.Contains(item)) return column;
            }

            return null;
        }

        private void AddItems(IList newItems)
        {
            // Sicherstellen das wir auch wirklich etwas haben
            if (newItems == null || newItems.Count == 0 || GroupedCollection?.View.Groups == null) return;

            for (int i = 0; i < newItems.Count; i++)
            {
                var item = newItems[i];

                HookupItem(item);

                // Schauen zu welcher Gruppe das Item gehört
                var group = GroupedCollection.View.Groups.OfType<CollectionViewGroup>().FirstOrDefault(x => x.Items.Contains(item));

                if (group == null) continue;

                var column = GetOrGenerateColumn(group.Name);

                // Wenn es die Spalte nicht gibt überspringen wir das Item
                if (column == null) continue;

                column.Items.Add(item);
            }
        }

        private void RemoveItems(IList removedItems)
        {
            // Sicherstellen das wir auch wirklich etwas haben
            if (removedItems == null || removedItems.Count == 0) return;

            for (int i = 0; i < removedItems.Count; i++)
            {
                var item = removedItems[i];

                // Die aktuelle Spalte für das item rausfinden
                var oldColumn = GetColumnContainingItem(item);
                // Wenn wir eine Spalte gefunden haben, dann aus dieser entfernen
                if (oldColumn != null) oldColumn.Items.Remove(item);
                // Eventhandler abhängen
                UnhookItem(item);
            }
        }

        private void ResetItems()
        {
            if (_suppressItemsReset) return;

            // Wenn ein Reset auf die Collection ausgeführt wird (z.B. .Clear() oder Grouping ändert sich) müssen wir alle Spalten leeren und neu zuweisen
            // Das zu nutzende Grouping sollte an der Stelle bereits durchgeführt worden sein
            ClearColumns();
            AssignGroups();
        }

        private void OnColumnsCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                {
                    for (int i = 0; i < e.NewItems.Count; i++)
                    {
                        var item = e.NewItems[i];

                        if (item is TaskBoardColumn column)
                        {
                            column.TaskBoard = this;
                            SetDragDropProperties(column);
                        }
                    }
                    break;
                }
            }
        }

        private void SetDragDropProperties(TaskBoardColumn column)
        {
            TaskBoardDragDrop.SetBehavior(column, DragDropBehavior);
            TaskBoardDragDrop.SetDragVisualProvider(column, DragVisualProvider);
            TaskBoardDragDrop.SetDropVisualProvider(column, DropVisualProvider);
        }

        private void HookupColumns(INotifyCollectionChanged collection)
        {
            collection.CollectionChanged += OnColumnsCollectionChanged;
        }

        private void UnHookColumns(INotifyCollectionChanged collection)
        {
            collection.CollectionChanged -= OnColumnsCollectionChanged;
        }
    }
}