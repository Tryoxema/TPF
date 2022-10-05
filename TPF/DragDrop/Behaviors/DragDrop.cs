using System;
using System.Windows;

namespace TPF.DragDrop.Behaviors
{
    public abstract class DragDrop<THelper, TBehavior, TState> 
        where THelper : DragDropHelper<TBehavior, TState>, new() 
        where TBehavior : DragDropBehavior<TState> 
        where TState : DragDropState, new()
    {
        protected DragDrop()
        {
            
        }

        #region Behavior Attached DependencyProperty
        public static readonly DependencyProperty BehaviorProperty = DependencyProperty.RegisterAttached("Behavior",
            typeof(TBehavior),
            typeof(DragDrop<THelper, TBehavior, TState>),
            new PropertyMetadata(null, OnBehaviorChanged));

        private static void OnBehaviorChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            var helper = (THelper)sender.GetValue(HelperProperty);

            if (helper == null)
            {
                helper = new THelper();
                sender.SetValue(HelperProperty, helper);
            }

            helper.DragDropBehavior = (TBehavior)e.NewValue;
        }

        public static TBehavior GetBehavior(DependencyObject element)
        {
            return (TBehavior)element.GetValue(BehaviorProperty);
        }

        public static void SetBehavior(DependencyObject element, TBehavior value)
        {
            element.SetValue(BehaviorProperty, value);
        }
        #endregion

        #region DragVisualProvider Attached DependencyProperty
        public static readonly DependencyProperty DragVisualProviderProperty = DependencyProperty.RegisterAttached("DragVisualProvider",
            typeof(IDragVisualProvider),
            typeof(DragDrop<THelper, TBehavior, TState>),
            new PropertyMetadata(null, OnDragVisualProviderChanged));

        private static void OnDragVisualProviderChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            var helper = (THelper)sender.GetValue(HelperProperty);

            if (helper == null)
            {
                helper = new THelper();
                sender.SetValue(HelperProperty, helper);
            }

            helper.DragVisualProvider = (IDragVisualProvider)e.NewValue;
        }

        public static IDragVisualProvider GetDragVisualProvider(DependencyObject element)
        {
            return (IDragVisualProvider)element.GetValue(DragVisualProviderProperty);
        }

        public static void SetDragVisualProvider(DependencyObject element, IDragVisualProvider value)
        {
            element.SetValue(DragVisualProviderProperty, value);
        }
        #endregion

        #region DropVisualProvider Attached DependencyProperty
        public static readonly DependencyProperty DropVisualProviderProperty = DependencyProperty.RegisterAttached("DropVisualProvider",
            typeof(IDropVisualProvider),
            typeof(DragDrop<THelper, TBehavior, TState>),
            new PropertyMetadata(null, OnDropVisualProviderChanged));

        private static void OnDropVisualProviderChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            var helper = (THelper)sender.GetValue(HelperProperty);

            if (helper == null)
            {
                helper = new THelper();
                sender.SetValue(HelperProperty, helper);
            }

            helper.DropVisualProvider = (IDropVisualProvider)e.NewValue;
        }

        public static IDropVisualProvider GetDropVisualProvider(DependencyObject element)
        {
            return (IDropVisualProvider)element.GetValue(DropVisualProviderProperty);
        }

        public static void SetDropVisualProvider(DependencyObject element, IDropVisualProvider value)
        {
            element.SetValue(DropVisualProviderProperty, value);
        }
        #endregion

        #region Helper Attached DependencyProperty
        protected static readonly DependencyProperty HelperProperty = DependencyProperty.RegisterAttached("Helper",
            typeof(THelper),
            typeof(DragDrop<THelper, TBehavior, TState>),
            new PropertyMetadata(null, OnHelperChanged));

        private static void OnHelperChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            if (sender is FrameworkElement element)
            {
                var oldValue = (THelper)e.OldValue;
                var newValue = (THelper)e.NewValue;

                if (oldValue != null)
                {
                    oldValue.UnhookEvents(element);
                }

                if (newValue != null)
                {
                    newValue.HookupEvents(element);
                }
            }
        }
        #endregion
    }
}