using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Controls;
using System.Collections.Generic;
using System.Windows.Media.Animation;
using TPF.Controls.Specialized.StepProgressBar;

namespace TPF.Controls
{
    public class StepProgressBar : ItemsControl
    {
        static StepProgressBar()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(StepProgressBar), new FrameworkPropertyMetadata(typeof(StepProgressBar)));
        }

        #region IndicatorClicked RoutedEvent
        public static readonly RoutedEvent IndicatorClickedEvent = EventManager.RegisterRoutedEvent("IndicatorClicked",
            RoutingStrategy.Bubble,
            typeof(IndicatorClickedEventHandler),
            typeof(StepProgressBar));

        public event IndicatorClickedEventHandler IndicatorClicked
        {
            add => AddHandler(IndicatorClickedEvent, value);
            remove => RemoveHandler(IndicatorClickedEvent, value);
        }
        #endregion

        #region ConnectorBrush DependencyProperty
        public static readonly DependencyProperty ConnectorBrushProperty = DependencyProperty.Register("ConnectorBrush",
            typeof(Brush),
            typeof(StepProgressBar),
            new PropertyMetadata(null, OnConnectorBrushChanged));

        static void OnConnectorBrushChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            var instance = (StepProgressBar)sender;

            for (int i = 0; i < instance.Items.Count; i++)
            {
                var item = instance.Items[i];

                if (item is StepItem stepItem)
                {
                    stepItem.ConnectorBrush = instance.ConnectorBrush;
                }
                else
                {
                    stepItem = (StepItem)instance.ItemContainerGenerator.ContainerFromItem(item);

                    if (stepItem != null) stepItem.ConnectorBrush = instance.ConnectorBrush;
                }
            }
        }

        public Brush ConnectorBrush
        {
            get { return (Brush)GetValue(ConnectorBrushProperty); }
            set { SetValue(ConnectorBrushProperty, value); }
        }
        #endregion

        #region ConnectorProgressBrush DependencyProperty
        public static readonly DependencyProperty ConnectorProgressBrushProperty = DependencyProperty.Register("ConnectorProgressBrush",
            typeof(Brush),
            typeof(StepProgressBar),
            new PropertyMetadata(null, OnConnectorProgressBrushChanged));

        static void OnConnectorProgressBrushChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            var instance = (StepProgressBar)sender;

            for (int i = 0; i < instance.Items.Count; i++)
            {
                var item = instance.Items[i];

                if (item is StepItem stepItem)
                {
                    stepItem.ConnectorProgressBrush = instance.ConnectorProgressBrush;
                }
                else
                {
                    stepItem = (StepItem)instance.ItemContainerGenerator.ContainerFromItem(item);

                    if (stepItem != null) stepItem.ConnectorProgressBrush = instance.ConnectorProgressBrush;
                }
            }
        }

        public Brush ConnectorProgressBrush
        {
            get { return (Brush)GetValue(ConnectorProgressBrushProperty); }
            set { SetValue(ConnectorProgressBrushProperty, value); }
        }
        #endregion

        #region ConnectorThickness DependencyProperty
        public static readonly DependencyProperty ConnectorThicknessProperty = DependencyProperty.Register("ConnectorThickness",
            typeof(double),
            typeof(StepProgressBar),
            new PropertyMetadata(2.0, OnConnectorThicknessChanged));

        static void OnConnectorThicknessChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            var instance = (StepProgressBar)sender;

            for (int i = 0; i < instance.Items.Count; i++)
            {
                var item = instance.Items[i];

                if (item is StepItem stepItem)
                {
                    stepItem.ConnectorThickness = instance.ConnectorThickness;
                }
                else
                {
                    stepItem = (StepItem)instance.ItemContainerGenerator.ContainerFromItem(item);

                    if (stepItem != null) stepItem.ConnectorThickness = instance.ConnectorThickness;
                }
            }
        }

        public double ConnectorThickness
        {
            get { return (double)GetValue(ConnectorThicknessProperty); }
            set { SetValue(ConnectorThicknessProperty, value); }
        }
        #endregion

        #region Orientation DependencyProperty
        public static readonly DependencyProperty OrientationProperty = DependencyProperty.Register("Orientation",
            typeof(Orientation),
            typeof(StepProgressBar),
            new PropertyMetadata(Orientation.Horizontal, OnOrientationChanged));

        static void OnOrientationChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            var instance = (StepProgressBar)sender;

            for (int i = 0; i < instance.Items.Count; i++)
            {
                var item = instance.Items[i];

                if (item is StepItem stepItem)
                {
                    stepItem.Orientation = instance.Orientation;
                }
                else
                {
                    stepItem = (StepItem)instance.ItemContainerGenerator.ContainerFromItem(item);

                    if (stepItem != null) stepItem.Orientation = instance.Orientation;
                }
            }
        }

        public Orientation Orientation
        {
            get { return (Orientation)GetValue(OrientationProperty); }
            set { SetValue(OrientationProperty, value); }
        }
        #endregion

        #region ItemSpacing DependencyProperty
        public static readonly DependencyProperty ItemSpacingProperty = DependencyProperty.Register("ItemSpacing",
            typeof(double),
            typeof(StepProgressBar),
            new PropertyMetadata(80.0, OnItemSpacingChanged));

        static void OnItemSpacingChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            var instance = (StepProgressBar)sender;

            for (int i = 0; i < instance.Items.Count; i++)
            {
                var item = instance.Items[i];

                if (item is StepItem stepItem)
                {
                    stepItem.ItemSpacing = instance.ItemSpacing;
                }
                else
                {
                    stepItem = (StepItem)instance.ItemContainerGenerator.ContainerFromItem(item);

                    if (stepItem != null) stepItem.ItemSpacing = instance.ItemSpacing;
                }
            }
        }

        public double ItemSpacing
        {
            get { return (double)GetValue(ItemSpacingProperty); }
            set { SetValue(ItemSpacingProperty, value); }
        }
        #endregion

        #region IndicatorTemplateSelector DependencyProperty
        public static readonly DependencyProperty IndicatorTemplateSelectorProperty = DependencyProperty.Register("IndicatorTemplateSelector",
            typeof(DataTemplateSelector),
            typeof(StepProgressBar),
            new PropertyMetadata(null, OnIndicatorTemplateSelectorChanged));

        static void OnIndicatorTemplateSelectorChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            var instance = (StepProgressBar)sender;

            for (int i = 0; i < instance.Items.Count; i++)
            {
                var item = instance.Items[i];

                if (item is StepItem stepItem)
                {
                    stepItem.IndicatorTemplate = instance.IndicatorTemplateSelector?.SelectTemplate(item, stepItem);
                }
                else
                {
                    stepItem = (StepItem)instance.ItemContainerGenerator.ContainerFromItem(item);

                    if (stepItem != null) stepItem.IndicatorTemplate = instance.IndicatorTemplateSelector?.SelectTemplate(item, stepItem);
                }
            }
        }

        public DataTemplateSelector IndicatorTemplateSelector
        {
            get { return (DataTemplateSelector)GetValue(IndicatorTemplateSelectorProperty); }
            set { SetValue(IndicatorTemplateSelectorProperty, value); }
        }
        #endregion

        #region AnimationDuration DependencyProperty
        public static readonly DependencyProperty AnimationDurationProperty = DependencyProperty.Register("AnimationDuration",
            typeof(TimeSpan),
            typeof(StepProgressBar),
            new PropertyMetadata(TimeSpan.FromMilliseconds(250)));

        public TimeSpan AnimationDuration
        {
            get { return (TimeSpan)GetValue(AnimationDurationProperty); }
            set { SetValue(AnimationDurationProperty, value); }
        }
        #endregion

        #region SelectedIndex DependencyProperty
        public static readonly DependencyProperty SelectedIndexProperty = DependencyProperty.Register("SelectedIndex",
            typeof(int),
            typeof(StepProgressBar),
            new FrameworkPropertyMetadata(-1, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnSelectedIndexChanged, ConstrainSelectedIndex));

        static void OnSelectedIndexChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            var instance = (StepProgressBar)sender;

            var oldValue = (int)e.OldValue;
            var newValue = (int)e.NewValue;

            // Fortschritt oder Rückschritt
            if (oldValue < newValue)
            {
                var stepsToAnimate = new List<StepItem>();
                StepItem selectedStep = null;

                for (int i = oldValue - 1; i <= newValue; i++)
                {
                    if (i < 0) continue;

                    var item = instance.Items[i];

                    StepItem stepItem = null;

                    if (item is StepItem) stepItem = (StepItem)item;
                    else stepItem = (StepItem)instance.ItemContainerGenerator.ContainerFromItem(item);

                    if (i == newValue)
                    {
                        selectedStep = stepItem;
                        stepItem.Progress = 0.0;
                    }
                    else
                    {
                        stepsToAnimate.Add(stepItem);
                    }
                }

                if (stepsToAnimate.Count > 0)
                {
                    var storyboard = instance.GetStoryboard(stepsToAnimate);

                    if (selectedStep != null)
                    {
                        storyboard.Completed += (_, __) =>
                        {
                            selectedStep.StepStatus = instance.SelectedItemStatus;
                        };
                    }

                    storyboard.Begin();
                }
                else if (selectedStep != null)
                {
                    selectedStep.StepStatus = instance.SelectedItemStatus;
                }
            }
            else
            {
                for (int i = newValue - 1; i < instance.Items.Count; i++)
                {
                    if (i < 0) continue;

                    var item = instance.Items[i];

                    StepItem stepItem = null;

                    if (item is StepItem) stepItem = (StepItem)item;
                    else stepItem = (StepItem)instance.ItemContainerGenerator.ContainerFromItem(item);

                    if (i == newValue - 1)
                    {
                        stepItem.StepStatus = StepStatus.Complete;
                        stepItem.Progress = instance.SelectedItemProgress;
                    }
                    else if (i == newValue)
                    {
                        stepItem.StepStatus = instance.SelectedItemStatus;
                        stepItem.Progress = 0.0;
                    }
                    else
                    {
                        stepItem.StepStatus = StepStatus.Inactive;
                        stepItem.Progress = 0.0;
                    }
                }
            }
        }

        internal static object ConstrainSelectedIndex(DependencyObject sender, object value)
        {
            var intValue = (int)value;

            var instance = (StepProgressBar)sender;

            if (intValue < -1) intValue = -1;
            else if (intValue >= instance.Items.Count)
            {
                if (!instance._templateApplied) instance._selectedIndexAfterLoad = intValue;
                intValue = instance.Items.Count - 1;
            }

            return intValue;
        }

        public int SelectedIndex
        {
            get { return (int)GetValue(SelectedIndexProperty); }
            set { SetValue(SelectedIndexProperty, value); }
        }
        #endregion

        #region SelectedItemProgress DependencyProperty
        public static readonly DependencyProperty SelectedItemProgressProperty = DependencyProperty.Register("SelectedItemProgress",
            typeof(double),
            typeof(StepItem),
            new PropertyMetadata(100.0, OnSelectedItemProgressChanged, ConstrainSelectedItemProgress));

        static void OnSelectedItemProgressChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            var instance = (StepProgressBar)sender;

            if (instance.SelectedIndex < 1 || instance.SelectedItemStatus == StepStatus.Complete) return;

            var oldValue = (double)e.OldValue;
            var newValue = (double)e.NewValue;

            var previousItem = (StepItem)instance.Items[instance.SelectedIndex - 1];

            if (oldValue >= newValue)
            {
                previousItem.Progress = newValue;
                return;
            }

            var storyboard = instance.GetStoryboard(oldValue, newValue, previousItem);

            storyboard.Begin();
        }

        internal static object ConstrainSelectedItemProgress(DependencyObject sender, object value)
        {
            var instance = (StepProgressBar)sender;

            if (instance.SelectedItemStatus == StepStatus.Complete) return 100.0;

            var doubleValue = (double)value;

            if (doubleValue < 0) doubleValue = 0.0;
            else if (doubleValue > 100.0) doubleValue = 100.0;

            return doubleValue;
        }

        public double SelectedItemProgress
        {
            get { return (double)GetValue(SelectedItemProgressProperty); }
            set { SetValue(SelectedItemProgressProperty, value); }
        }
        #endregion

        #region SelectedItemStatus DependencyProperty
        public static readonly DependencyProperty SelectedItemStatusProperty = DependencyProperty.Register("SelectedItemStatus",
            typeof(StepStatus),
            typeof(StepProgressBar),
            new PropertyMetadata(StepStatus.Complete, OnSelectedItemStatusChanged));

        static void OnSelectedItemStatusChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            var instance = (StepProgressBar)sender;

            if (instance.SelectedIndex < 0) return;

            var item = (StepItem)instance.Items[instance.SelectedIndex];

            item.StepStatus = instance.SelectedItemStatus;

            if (item.StepStatus == StepStatus.Complete && instance.SelectedIndex > 0)
            {
                var previousItem = (StepItem)instance.Items[instance.SelectedIndex - 1];

                if (previousItem.Progress == 100) return;

                var storyboard = instance.GetStoryboard(previousItem.Progress, 100.0, previousItem);

                storyboard.Begin();
            }
        }

        public StepStatus SelectedItemStatus
        {
            get { return (StepStatus)GetValue(SelectedItemStatusProperty); }
            set { SetValue(SelectedItemStatusProperty, value); }
        }
        #endregion

        private bool _templateApplied;
        private int? _selectedIndexAfterLoad;

        private Storyboard GetStoryboard(double from, double to, StepItem item)
        {
            var animation = new DoubleAnimation(from, to, AnimationDuration);

            animation.Completed += (_, __) =>
            {
                item.Progress = to;
            };

            Storyboard.SetTarget(animation, item);
            Storyboard.SetTargetProperty(animation, new PropertyPath("Progress"));

            var story = new Storyboard();
            story.Children.Add(animation);

            story.Completed += (_, __) =>
            {
                story.Remove();
            };

            return story;
        }

        private Storyboard GetStoryboard(List<StepItem> items)
        {
            var story = new Storyboard();

            var timeOffset = TimeSpan.Zero;

            for (int i = 0; i < items.Count; i++)
            {
                var item = items[i];

                // Falls der erste bereits 100 Progress hat, können wir den überspringen
                if (item.Progress == 100) continue;

                // Die Steps sollten in der Reihenfolge in der Liste stehen, wie sie angezeigt werden
                // Der Letzte Step ist also der, der den SelectedItemProgress bekommen soll
                if (i == items.Count - 1)
                {
                    var stateAnimation = new ObjectAnimationUsingKeyFrames()
                    {
                        Duration = TimeSpan.Zero,
                        BeginTime = timeOffset
                    };

                    Storyboard.SetTarget(stateAnimation, item);
                    Storyboard.SetTargetProperty(stateAnimation, new PropertyPath("StepStatus"));

                    stateAnimation.KeyFrames.Add(new DiscreteObjectKeyFrame(StepStatus.Complete, TimeSpan.Zero));

                    story.Children.Add(stateAnimation);

                    var factor = SelectedItemProgress / 100;

                    var duration = TimeSpan.FromMilliseconds(AnimationDuration.TotalMilliseconds * factor);

                    var animation = new DoubleAnimation(item.Progress, SelectedItemProgress, duration)
                    {
                        BeginTime = timeOffset
                    };

                    animation.Completed += (_, __) =>
                    {
                        item.Progress = SelectedItemProgress;
                    };

                    Storyboard.SetTarget(animation, item);
                    Storyboard.SetTargetProperty(animation, new PropertyPath("Progress"));

                    story.Children.Add(animation);

                    timeOffset = timeOffset.Add(duration);
                }
                else
                {
                    var stateAnimation = new ObjectAnimationUsingKeyFrames()
                    {
                        Duration = TimeSpan.Zero,
                        BeginTime = timeOffset
                    };

                    Storyboard.SetTarget(stateAnimation, item);
                    Storyboard.SetTargetProperty(stateAnimation, new PropertyPath("StepStatus"));

                    stateAnimation.KeyFrames.Add(new DiscreteObjectKeyFrame(StepStatus.Complete, TimeSpan.Zero));

                    story.Children.Add(stateAnimation);

                    var factor = (100 - item.Progress) / 100;

                    var duration = TimeSpan.FromMilliseconds(AnimationDuration.TotalMilliseconds * factor);

                    var animation = new DoubleAnimation(item.Progress, 100, duration)
                    {
                        BeginTime = timeOffset
                    };

                    animation.Completed += (_, __) =>
                    {
                        item.Progress = 100.0;
                    };

                    Storyboard.SetTarget(animation, item);
                    Storyboard.SetTargetProperty(animation, new PropertyPath("Progress"));

                    story.Children.Add(animation);

                    timeOffset = timeOffset.Add(duration);
                }
            }

            story.Completed += (_, __) =>
            {
                for (int i = 0; i < items.Count; i++)
                {
                    var item = items[i];

                    item.StepStatus = StepStatus.Complete;
                }
                story.Remove();
            };

            return story;
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            _templateApplied = true;

            if (_selectedIndexAfterLoad != null) SelectedIndex = _selectedIndexAfterLoad.Value;
        }

        internal void RaiseIndicatorClicked(StepItem item)
        {
            OnIndicatorClicked(item);
        }

        protected virtual void OnIndicatorClicked(StepItem item)
        {
            var eventArgs = new IndicatorClickedEventArgs(item);

            RaiseEvent(eventArgs);
        }

        protected override DependencyObject GetContainerForItemOverride()
        {
            return new StepItem();
        }

        protected override bool IsItemItsOwnContainerOverride(object item)
        {
            return item is StepItem;
        }

        protected override void PrepareContainerForItemOverride(DependencyObject element, object item)
        {
            base.PrepareContainerForItemOverride(element, item);

            if (element is StepItem stepItem)
            {
                if (stepItem.ReadLocalValue(StepItem.ConnectorBrushProperty) == DependencyProperty.UnsetValue) stepItem.ConnectorBrush = ConnectorBrush;
                if (stepItem.ReadLocalValue(StepItem.ConnectorProgressBrushProperty) == DependencyProperty.UnsetValue) stepItem.ConnectorProgressBrush = ConnectorProgressBrush;
                if (stepItem.ReadLocalValue(StepItem.ConnectorThicknessProperty) == DependencyProperty.UnsetValue) stepItem.ConnectorThickness = ConnectorThickness;
                if (stepItem.ReadLocalValue(StepItem.OrientationProperty) == DependencyProperty.UnsetValue) stepItem.Orientation = Orientation;
                if (stepItem.ReadLocalValue(StepItem.ItemSpacingProperty) == DependencyProperty.UnsetValue) stepItem.ItemSpacing = ItemSpacing;

                if (stepItem.ReadLocalValue(StepItem.IndicatorTemplateProperty) == DependencyProperty.UnsetValue) stepItem.IndicatorTemplate = IndicatorTemplateSelector?.SelectTemplate(item, stepItem);
            }
        }
    }
}