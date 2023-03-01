using System.Windows;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Controls;
using System.Collections.Specialized;
using TPF.Controls.Specialized.TaskBoard;
using TPF.Internal;

namespace TPF.Controls
{
    public class TaskBoardColumn : HeaderedItemsControl
    {
        static TaskBoardColumn()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(TaskBoardColumn), new FrameworkPropertyMetadata(typeof(TaskBoardColumn)));
        }

        #region Value DependencyProperty
        public static readonly DependencyProperty ValueProperty = DependencyProperty.Register("Value",
            typeof(object),
            typeof(TaskBoardColumn),
            new PropertyMetadata(null));

        public object Value
        {
            get { return GetValue(ValueProperty); }
            set { SetValue(ValueProperty, value); }
        }
        #endregion

        #region IsCollapsed DependencyProperty
        public static readonly DependencyProperty IsCollapsedProperty = DependencyProperty.Register("IsCollapsed",
            typeof(bool),
            typeof(TaskBoardColumn),
            new PropertyMetadata(BooleanBoxes.FalseBox));

        public bool IsCollapsed
        {
            get { return (bool)GetValue(IsCollapsedProperty); }
            set { SetValue(IsCollapsedProperty, BooleanBoxes.Box(value)); }
        }
        #endregion

        #region AllowDrag DependencyProperty
        public static readonly DependencyProperty AllowDragProperty = DependencyProperty.Register("AllowDrag",
            typeof(bool),
            typeof(TaskBoardColumn),
            new PropertyMetadata(BooleanBoxes.TrueBox));

        public bool AllowDrag
        {
            get { return (bool)GetValue(AllowDragProperty); }
            set { SetValue(AllowDragProperty, BooleanBoxes.Box(value)); }
        }
        #endregion

        #region HeaderForeground DependencyProperty
        public static readonly DependencyProperty HeaderForegroundProperty = DependencyProperty.Register("HeaderForeground",
            typeof(Brush),
            typeof(TaskBoardColumn),
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
            typeof(TaskBoardColumn),
            new PropertyMetadata(null));

        public Brush HeaderBackground
        {
            get { return (Brush)GetValue(HeaderBackgroundProperty); }
            set { SetValue(HeaderBackgroundProperty, value); }
        }
        #endregion

        #region HeaderBorderThickness DependencyProperty
        public static readonly DependencyProperty HeaderBorderThicknessProperty = DependencyProperty.Register("HeaderBorderThickness",
            typeof(Thickness),
            typeof(TaskBoardColumn),
            new PropertyMetadata(default(Thickness)));

        public Thickness HeaderBorderThickness
        {
            get { return (Thickness)GetValue(HeaderBorderThicknessProperty); }
            set { SetValue(HeaderBorderThicknessProperty, value); }
        }
        #endregion

        #region CollapsedHeaderTemplate DependencyProperty
        public static readonly DependencyProperty CollapsedHeaderTemplateProperty = DependencyProperty.Register("CollapsedHeaderTemplate",
            typeof(DataTemplate),
            typeof(TaskBoardColumn),
            new PropertyMetadata(null));

        public DataTemplate CollapsedHeaderTemplate
        {
            get { return (DataTemplate)GetValue(CollapsedHeaderTemplateProperty); }
            set { SetValue(CollapsedHeaderTemplateProperty, value); }
        }
        #endregion

        #region Maximum DependencyProperty
        public static readonly DependencyProperty MaximumProperty = DependencyProperty.Register("Maximum",
            typeof(int),
            typeof(TaskBoardColumn),
            new PropertyMetadata(0, OnMaximumChanged));

        private static void OnMaximumChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            var instance = (TaskBoardColumn)sender;

            instance.EvaluateStateIndicator();
        }

        public int Maximum
        {
            get { return (int)GetValue(MaximumProperty); }
            set { SetValue(MaximumProperty, value); }
        }
        #endregion

        #region StateIndicator Readonly DependencyProperty
        internal static readonly DependencyPropertyKey StateIndicatorPropertyKey = DependencyProperty.RegisterReadOnly("StateIndicator",
            typeof(Brush),
            typeof(TaskBoardColumn),
            new PropertyMetadata(null));

        public static readonly DependencyProperty StateIndicatorProperty = StateIndicatorPropertyKey.DependencyProperty;

        public Brush StateIndicator
        {
            get { return (Brush)GetValue(StateIndicatorProperty); }
            private set { SetValue(StateIndicatorPropertyKey, value); }
        }
        #endregion

        #region StateIndicatorSelector DependencyProperty
        public static readonly DependencyProperty StateIndicatorSelectorProperty = DependencyProperty.Register("StateIndicatorSelector",
            typeof(TaskBoardColumnStateIndicatorSelector),
            typeof(TaskBoardColumn),
            new PropertyMetadata(null, OnStateIndicatorSelectorChanged));

        private static void OnStateIndicatorSelectorChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            var instance = (TaskBoardColumn)sender;

            instance.EvaluateStateIndicator();
        }

        public TaskBoardColumnStateIndicatorSelector StateIndicatorSelector
        {
            get { return (TaskBoardColumnStateIndicatorSelector)GetValue(StateIndicatorSelectorProperty); }
            set { SetValue(StateIndicatorSelectorProperty, value); }
        }
        #endregion

        #region CornerRadius DependencyProperty
        public static readonly DependencyProperty CornerRadiusProperty = DependencyProperty.Register("CornerRadius",
            typeof(CornerRadius),
            typeof(TaskBoardColumn),
            new PropertyMetadata(default(CornerRadius)));

        public CornerRadius CornerRadius
        {
            get { return (CornerRadius)GetValue(CornerRadiusProperty); }
            set { SetValue(CornerRadiusProperty, value); }
        }
        #endregion

        public TaskBoard TaskBoard { get; internal set; }

        internal bool IsAutoGenerated { get; set; }

        internal void EvaluateStateIndicator()
        {
            StateIndicator = StateIndicatorSelector?.SelectIndicatorBrush(this);
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            TaskBoard = this.ParentOfType<TaskBoard>();
        }

        protected override void OnItemsChanged(NotifyCollectionChangedEventArgs e)
        {
            base.OnItemsChanged(e);

            EvaluateStateIndicator();
        }

        protected override bool IsItemItsOwnContainerOverride(object item)
        {
            return item is TaskBoardItem;
        }

        protected override DependencyObject GetContainerForItemOverride()
        {
            return new TaskBoardItem();
        }

        protected override void PrepareContainerForItemOverride(DependencyObject element, object item)
        {
            base.PrepareContainerForItemOverride(element, item);

            if (element is TaskBoardItem taskBoardItem && TaskBoard != null)
            {
                taskBoardItem.SetBinding(MinHeightProperty, new Binding("MinimumCardHeight") { Source = TaskBoard });
                taskBoardItem.SetBinding(MaxHeightProperty, new Binding("MaximumCardHeight") { Source = TaskBoard });
            }
        }

        protected override void ClearContainerForItemOverride(DependencyObject element, object item)
        {
            base.ClearContainerForItemOverride(element, item);

            if (element is TaskBoardItem taskBoardItem)
            {
                BindingOperations.ClearBinding(taskBoardItem, MinHeightProperty);
                BindingOperations.ClearBinding(taskBoardItem, MaxHeightProperty);
            }
        }
    }
}