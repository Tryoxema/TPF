using System;
using System.Windows;
using System.Windows.Controls;
using TPF.Controls.Specialized.Dashboard;
using TPF.DragDrop.Behaviors;
using TPF.Internal;

namespace TPF.Controls
{
    public class Dashboard : HeaderedItemsControl
    {
        static Dashboard()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(Dashboard), new FrameworkPropertyMetadata(typeof(Dashboard)));

            ItemsPanelProperty.OverrideMetadata(typeof(Dashboard), new FrameworkPropertyMetadata(new ItemsPanelTemplate(new FrameworkElementFactory(typeof(DashboardPanel)))));
        }

        public Dashboard()
        {
            var behavior = new DashboardDragDropBehavior();

            DashboardDragDrop.SetBehavior(this, behavior);
            AllowDrop = true;
        }

        #region SlotHeight DependencyProperty
        public static readonly DependencyProperty SlotHeightProperty = DependencyProperty.Register("SlotHeight",
            typeof(double),
            typeof(Dashboard),
            new PropertyMetadata(150.0, OnLayoutPropertyChanged, CoerceDoubleToPositiveValue));

        public double SlotHeight
        {
            get { return (double)GetValue(SlotHeightProperty); }
            set { SetValue(SlotHeightProperty, value); }
        }
        #endregion

        #region SlotWidth DependencyProperty
        public static readonly DependencyProperty SlotWidthProperty = DependencyProperty.Register("SlotWidth",
            typeof(double),
            typeof(Dashboard),
            new PropertyMetadata(150.0, OnLayoutPropertyChanged, CoerceDoubleToPositiveValue));

        public double SlotWidth
        {
            get { return (double)GetValue(SlotWidthProperty); }
            set { SetValue(SlotWidthProperty, value); }
        }
        #endregion

        #region Gap DependencyProperty
        public static readonly DependencyProperty GapProperty = DependencyProperty.Register("Gap",
            typeof(double),
            typeof(Dashboard),
            new PropertyMetadata(5.0, OnLayoutPropertyChanged, CoerceDoubleToPositiveValue));

        public double Gap
        {
            get { return (double)GetValue(GapProperty); }
            set { SetValue(GapProperty, value); }
        }
        #endregion

        #region AllowDrag DependencyProperty
        public static readonly DependencyProperty AllowDragProperty = DependencyProperty.Register("AllowDrag",
            typeof(bool),
            typeof(Dashboard),
            new PropertyMetadata(BooleanBoxes.TrueBox));

        public bool AllowDrag
        {
            get { return (bool)GetValue(AllowDragProperty); }
            set { SetValue(AllowDragProperty, BooleanBoxes.Box(value)); }
        }
        #endregion

        #region DropVisualProvider DependencyProperty
        public static readonly DependencyProperty DropVisualProviderProperty = DependencyProperty.Register("DropVisualProvider",
            typeof(IDropVisualProvider),
            typeof(Dashboard),
            new PropertyMetadata(null, OnDropVisualProviderChanged));

        private static void OnDropVisualProviderChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            var instance = (Dashboard)sender;

            if (instance.DropVisualProvider is DashboardDropVisualProvider provider) provider.Dashboard = instance;

            DashboardDragDrop.SetDropVisualProvider(instance, instance.DropVisualProvider);
        }

        public IDropVisualProvider DropVisualProvider
        {
            get { return (IDropVisualProvider)GetValue(DropVisualProviderProperty); }
            set { SetValue(DropVisualProviderProperty, value); }
        }
        #endregion

        private static object CoerceDoubleToPositiveValue(DependencyObject sender, object value)
        {
            var doubleValue = (double)value;

            if (doubleValue < 0) doubleValue = 0;

            return doubleValue;
        }

        private static void OnLayoutPropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            var instance = (Dashboard)sender;

            instance.InvalidateWidgets();
        }

        private DashboardTransaction _transaction;

        public bool IsEditing
        {
            get { return _transaction != null; }
        }

        internal Widget DraggingWidget { get; set; }

        internal void InvalidateWidgets()
        {
            var panel = this.ChildOfType<DashboardPanel>();

            if (panel == null || (_transaction != null && _transaction.IsBulkEditing)) return;

            panel.InvalidateMeasure();
            panel.InvalidateArrange();
        }

        public void BeginEdit()
        {
            _transaction = new DashboardTransaction(this);
        }

        public void CommitEdit()
        {
            _transaction = null;
        }

        public void CancelEdit()
        {
            if (_transaction == null) return;

            _transaction.Cancel();

            InvalidateWidgets();
        }

        protected override DependencyObject GetContainerForItemOverride()
        {
            return new Widget();
        }

        protected override bool IsItemItsOwnContainerOverride(object item)
        {
            return item is Widget;
        }
    }
}