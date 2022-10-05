using System;
using System.Windows;
using System.Windows.Controls;

namespace TPF.Controls.Primitives
{
    public abstract class AttachableElement : Control, IDisposable
    {
        #region Target DependencyProperty
        public static readonly DependencyProperty TargetProperty = DependencyProperty.Register("Target",
            typeof(FrameworkElement),
            typeof(AttachableElement),
            new PropertyMetadata(null, OnTargetChanged));

        static void OnTargetChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            var instance = (AttachableElement)sender;

            instance.OnTargetChanged(e.OldValue as FrameworkElement, e.NewValue as FrameworkElement);
        }

        public FrameworkElement Target
        {
            get { return (FrameworkElement)GetValue(TargetProperty); }
            set { SetValue(TargetProperty, value); }
        }
        #endregion

        #region Instance Attached DependencyProperty
        public static readonly DependencyProperty InstanceProperty = DependencyProperty.RegisterAttached("Instance",
            typeof(AttachableElement),
            typeof(AttachableElement),
            new PropertyMetadata(null, OnInstanceChanged));

        static void OnInstanceChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            if (!(sender is FrameworkElement instance)) return;

            if (e.NewValue is AttachableElement element) element.OnInstanceChanged(instance);
        }

        public static AttachableElement GetInstance(DependencyObject element)
        {
            return (AttachableElement)element.GetValue(InstanceProperty);
        }

        public static void SetInstance(DependencyObject element, AttachableElement value)
        {
            element.SetValue(InstanceProperty, value);
        }
        #endregion

        protected virtual void OnTargetChanged(FrameworkElement oldValue, FrameworkElement newValue)
        {
            if (oldValue != null)
            {
                oldValue.Unloaded -= TargetElement_Unloaded;
            }

            if (newValue != null)
            {
                newValue.Unloaded += TargetElement_Unloaded;
            }
        }

        private void TargetElement_Unloaded(object sender, RoutedEventArgs e)
        {
            if (sender is FrameworkElement element)
            {
                element.Unloaded -= TargetElement_Unloaded;
                Dispose();
            }
        }

        protected virtual void OnInstanceChanged(FrameworkElement targetElement)
        {
            Target = targetElement;
        }

        #region IDisposable
        void IDisposable.Dispose()
        {
            Dispose();
        }

        protected virtual void Dispose() { } 
        #endregion
    }
}