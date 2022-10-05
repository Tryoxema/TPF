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
    [ContentProperty("DropDownContent")]
    public class DropDownButton : ButtonBase
    {
        static DropDownButton()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(DropDownButton), new FrameworkPropertyMetadata(typeof(DropDownButton)));

            EventManager.RegisterClassHandler(typeof(DropDownButton), Mouse.MouseDownEvent, new MouseButtonEventHandler(OnMouseButtonDown), true);
        }

        #region DropDownOpened RoutedEvent
        public static readonly RoutedEvent DropDownOpenedEvent = EventManager.RegisterRoutedEvent("DropDownOpened",
            RoutingStrategy.Bubble,
            typeof(RoutedEventHandler),
            typeof(DropDownButton));

        public event RoutedEventHandler DropDownOpened
        {
            add => AddHandler(DropDownOpenedEvent, value);
            remove => RemoveHandler(DropDownOpenedEvent, value);
        }
        #endregion

        #region DropDownClosed RoutedEvent
        public static readonly RoutedEvent DropDownClosedEvent = EventManager.RegisterRoutedEvent("DropDownClosed",
            RoutingStrategy.Bubble,
            typeof(RoutedEventHandler),
            typeof(DropDownButton));

        public event RoutedEventHandler DropDownClosed
        {
            add => AddHandler(DropDownClosedEvent, value);
            remove => RemoveHandler(DropDownClosedEvent, value);
        }
        #endregion

        #region IsDropDownOpen DependencyProperty
        public static readonly DependencyProperty IsDropDownOpenProperty = DependencyProperty.Register("IsDropDownOpen",
            typeof(bool),
            typeof(DropDownButton),
            new FrameworkPropertyMetadata(BooleanBoxes.FalseBox, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnIsDropDownOpenChanged));

        private static void OnIsDropDownOpenChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            var instance = (DropDownButton)sender;

            var open = (bool)e.NewValue;

            if (open)
            {
                Mouse.Capture(instance, CaptureMode.SubTree);

                instance.OnDropDownOpened(EventArgs.Empty);
            }
            else
            {
                if (Mouse.Captured == instance) Mouse.Capture(null);

                instance.OnDropDownClosed(EventArgs.Empty);
            }
        }

        public bool IsDropDownOpen
        {
            get { return (bool)GetValue(IsDropDownOpenProperty); }
            set { SetValue(IsDropDownOpenProperty, BooleanBoxes.Box(value)); }
        }
        #endregion

        #region DropDownMinHeight DependencyProperty
        public static readonly DependencyProperty DropDownMinHeightProperty = DependencyProperty.Register("DropDownMinHeight",
            typeof(double),
            typeof(DropDownButton),
            new PropertyMetadata(double.NaN));

        public double DropDownMinHeight
        {
            get { return (double)GetValue(DropDownMinHeightProperty); }
            set { SetValue(DropDownMinHeightProperty, value); }
        }
        #endregion

        #region DropDownMinWidth DependencyProperty
        public static readonly DependencyProperty DropDownMinWidthProperty = DependencyProperty.Register("DropDownMinWidth",
            typeof(double),
            typeof(DropDownButton),
            new PropertyMetadata(double.NaN));

        public double DropDownMinWidth
        {
            get { return (double)GetValue(DropDownMinWidthProperty); }
            set { SetValue(DropDownMinWidthProperty, value); }
        }
        #endregion

        #region DropDownHeight DependencyProperty
        public static readonly DependencyProperty DropDownHeightProperty = DependencyProperty.Register("DropDownHeight",
            typeof(double),
            typeof(DropDownButton),
            new PropertyMetadata(double.NaN));

        public double DropDownHeight
        {
            get { return (double)GetValue(DropDownHeightProperty); }
            set { SetValue(DropDownHeightProperty, value); }
        }
        #endregion

        #region DropDownWidth DependencyProperty
        public static readonly DependencyProperty DropDownWidthProperty = DependencyProperty.Register("DropDownWidth",
            typeof(double),
            typeof(DropDownButton),
            new PropertyMetadata(double.NaN));

        public double DropDownWidth
        {
            get { return (double)GetValue(DropDownWidthProperty); }
            set { SetValue(DropDownWidthProperty, value); }
        }
        #endregion

        #region DropDownMaxHeight DependencyProperty
        public static readonly DependencyProperty DropDownMaxHeightProperty = DependencyProperty.Register("DropDownMaxHeight",
            typeof(double),
            typeof(DropDownButton),
            new PropertyMetadata(double.NaN));

        public double DropDownMaxHeight
        {
            get { return (double)GetValue(DropDownMaxHeightProperty); }
            set { SetValue(DropDownMaxHeightProperty, value); }
        }
        #endregion

        #region DropDownMaxWidth DependencyProperty
        public static readonly DependencyProperty DropDownMaxWidthProperty = DependencyProperty.Register("DropDownMaxWidth",
            typeof(double),
            typeof(DropDownButton),
            new PropertyMetadata(double.NaN));

        public double DropDownMaxWidth
        {
            get { return (double)GetValue(DropDownMaxWidthProperty); }
            set { SetValue(DropDownMaxWidthProperty, value); }
        }
        #endregion

        #region DropDownContent DependencyProperty
        public static readonly DependencyProperty DropDownContentProperty = DependencyProperty.Register("DropDownContent",
            typeof(object),
            typeof(DropDownButton),
            new PropertyMetadata(null));

        public object DropDownContent
        {
            get { return GetValue(DropDownContentProperty); }
            set { SetValue(DropDownContentProperty, value); }
        }
        #endregion

        #region DropDownContentTemplate DependencyProperty
        public static readonly DependencyProperty DropDownContentTemplateProperty = DependencyProperty.Register("DropDownContentTemplate",
            typeof(DataTemplate),
            typeof(DropDownButton),
            new PropertyMetadata(null));

        public DataTemplate DropDownContentTemplate
        {
            get { return (DataTemplate)GetValue(DropDownContentTemplateProperty); }
            set { SetValue(DropDownContentTemplateProperty, value); }
        }
        #endregion

        #region DropDownContentTemplateSelector DependencyProperty
        public static readonly DependencyProperty DropDownContentTemplateSelectorProperty = DependencyProperty.Register("DropDownContentTemplateSelector",
            typeof(DataTemplateSelector),
            typeof(DropDownButton),
            new PropertyMetadata(null));

        public DataTemplateSelector DropDownContentTemplateSelector
        {
            get { return (DataTemplateSelector)GetValue(DropDownContentTemplateSelectorProperty); }
            set { SetValue(DropDownContentTemplateSelectorProperty, value); }
        }
        #endregion

        private static void OnMouseButtonDown(object sender, MouseButtonEventArgs e)
        {
            var instance = (DropDownButton)sender;

            // Wenn wir MouseCapture haben, dann Dropdown schließen wenn als Source das Control angegeben ist
            if (Mouse.Captured == instance && e.OriginalSource == instance)
            {
                e.Handled = true;

                instance.IsDropDownOpen = false;
            }
        }

        // Löst das DropDownOpened-Event aus
        protected virtual void OnDropDownOpened(EventArgs e)
        {
            var eventArgs = new RoutedEventArgs() { RoutedEvent = DropDownOpenedEvent };

            RaiseEvent(eventArgs);
        }

        // Löst das DropDownClosed-Event aus
        protected virtual void OnDropDownClosed(EventArgs e)
        {
            var eventArgs = new RoutedEventArgs() { RoutedEvent = DropDownClosedEvent };

            RaiseEvent(eventArgs);
        }

        protected override void OnClick()
        {
            base.OnClick();

            IsDropDownOpen = !IsDropDownOpen;
        }
    }
}