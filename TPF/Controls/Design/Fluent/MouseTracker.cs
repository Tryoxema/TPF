using System.Windows;
using System.Windows.Input;
using TPF.Internal;

namespace TPF.Controls
{
    public static class MouseTracker
    {
        #region RootObject Attached DependencyProperty
        public static readonly DependencyProperty RootObjectProperty = DependencyProperty.RegisterAttached("RootObject",
            typeof(UIElement),
            typeof(MouseTracker),
            new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.Inherits));

        public static UIElement GetRootObject(DependencyObject element)
        {
            return (UIElement)element.GetValue(RootObjectProperty);
        }

        public static void SetRootObject(DependencyObject element, UIElement value)
        {
            element.SetValue(RootObjectProperty, value);
        }
        #endregion

        #region IsInside Attached DependencyProperty
        public static readonly DependencyProperty IsInsideProperty = DependencyProperty.RegisterAttached("IsInside",
            typeof(bool),
            typeof(MouseTracker),
            new FrameworkPropertyMetadata(BooleanBoxes.FalseBox, FrameworkPropertyMetadataOptions.Inherits));

        public static bool GetIsInside(DependencyObject element)
        {
            return (bool)element.GetValue(IsInsideProperty);
        }

        public static void SetIsInside(DependencyObject element, bool value)
        {
            element.SetValue(IsInsideProperty, BooleanBoxes.Box(value));
        }
        #endregion

        #region Position Attached DependencyProperty
        public static readonly DependencyProperty PositionProperty = DependencyProperty.RegisterAttached("Position",
            typeof(Point),
            typeof(MouseTracker),
            new FrameworkPropertyMetadata(default(Point), FrameworkPropertyMetadataOptions.Inherits));

        public static Point GetPosition(DependencyObject element)
        {
            return (Point)element.GetValue(PositionProperty);
        }

        public static void SetPosition(DependencyObject element, Point value)
        {
            element.SetValue(PositionProperty, value);
        }
        #endregion

        #region IsEnabled Attached DependencyProperty
        public static readonly DependencyProperty IsEnabledProperty = DependencyProperty.RegisterAttached("IsEnabled",
            typeof(bool),
            typeof(MouseTracker),
            new PropertyMetadata(BooleanBoxes.FalseBox, OnIsEnabledChanged));

        private static void OnIsEnabledChanged(DependencyObject element, DependencyPropertyChangedEventArgs e)
        {
            if (!(element is UIElement instance)) return;

            var newValue = (bool)e.NewValue;
            var oldValue = (bool)e.OldValue;

            // Wenn der Tracker vorher aktiviert war und jetzt deaktiviert ist, Events abhängen
            if (oldValue && !newValue)
            {
                instance.MouseEnter -= TrackedElement_MouseEnter;
                instance.PreviewMouseMove -= TrackedElement_PreviewMouseMove;
                instance.MouseLeave -= TrackedElement_MouseLeave;

                instance.ClearValue(RootObjectProperty);
            }

            // Wenn der Tracker vorher deaktiviert war und jetzt aktiviert ist, Events anhängen
            if (!oldValue && newValue)
            {
                instance.MouseEnter += TrackedElement_MouseEnter;
                instance.PreviewMouseMove += TrackedElement_PreviewMouseMove;
                instance.MouseLeave += TrackedElement_MouseLeave;

                SetRootObject(instance, instance);
            }
        }

        public static bool GetIsEnabled(DependencyObject element)
        {
            return (bool)element.GetValue(IsEnabledProperty);
        }

        public static void SetIsEnabled(DependencyObject element, bool value)
        {
            element.SetValue(IsEnabledProperty, BooleanBoxes.Box(value));
        }
        #endregion

        private static void TrackedElement_MouseEnter(object sender, MouseEventArgs e)
        {
            if (sender is UIElement instance)
            {
                SetIsInside(instance, true);
            }
        }

        private static void TrackedElement_PreviewMouseMove(object sender, MouseEventArgs e)
        {
            if (sender is UIElement instance)
            {
                var position = e.GetPosition(instance);

                SetPosition(instance, position);
            }
        }

        private static void TrackedElement_MouseLeave(object sender, MouseEventArgs e)
        {
            if (sender is UIElement instance)
            {
                SetIsInside(instance, false);
            }
        }
    }
}