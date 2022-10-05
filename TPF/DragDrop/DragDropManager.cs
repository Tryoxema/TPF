using System;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using TPF.Internal;
using TPF.Internal.Interop;

namespace TPF.DragDrop
{
    public static class DragDropManager
    {
        #region AllowDrag Attached DependencyProperty
        public static readonly DependencyProperty AllowDragProperty = DependencyProperty.RegisterAttached("AllowDrag",
            typeof(bool),
            typeof(DragDropManager),
            new UIPropertyMetadata(BooleanBoxes.FalseBox, OnAllowDragChanged));

        private static void OnAllowDragChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            var instance = (UIElement)sender;

            // Müssen die Events registriert oder abgemeldet werden?
            if ((bool)e.NewValue)
            {
                instance.PreviewMouseLeftButtonDown += OnMouseLeftButtonDown;
                instance.PreviewMouseLeftButtonUp += OnMouseLeftButtonUp;
                instance.PreviewMouseMove += OnMouseMove;
                instance.QueryContinueDrag += OnQueryContinueDrag;
                instance.GiveFeedback += OnGiveFeedback;
            }
            else
            {
                instance.PreviewMouseLeftButtonDown -= OnMouseLeftButtonDown;
                instance.PreviewMouseLeftButtonUp -= OnMouseLeftButtonUp;
                instance.PreviewMouseMove -= OnMouseMove;
                instance.QueryContinueDrag -= OnQueryContinueDrag;
                instance.GiveFeedback -= OnGiveFeedback;
            }
        }

        public static bool GetAllowDrag(UIElement element)
        {
            return (bool)element.GetValue(AllowDragProperty);
        }

        public static void SetAllowDrag(UIElement element, bool value)
        {
            element.SetValue(AllowDragProperty, BooleanBoxes.Box(value));
        }
        #endregion

        #region Orientation Attached DependencyProperty
        public static readonly DependencyProperty OrientationProperty = DependencyProperty.RegisterAttached("Orientation",
            typeof(Orientation),
            typeof(DragDropManager),
            new PropertyMetadata(Orientation.Vertical));

        public static Orientation GetOrientation(UIElement element)
        {
            return (Orientation)element.GetValue(OrientationProperty);
        }

        public static void SetOrientation(UIElement element, Orientation value)
        {
            element.SetValue(OrientationProperty, value);
        }
        #endregion

        #region MinimumHorizontalDragDistance Attached DependencyProperty
        public static readonly DependencyProperty MinimumHorizontalDragDistanceProperty = DependencyProperty.RegisterAttached("MinimumHorizontalDragDistance",
            typeof(double),
            typeof(DragDropManager),
            new PropertyMetadata(SystemParameters.MinimumHorizontalDragDistance));

        public static double GetMinimumHorizontalDragDistance(UIElement source)
        {
            return (double)source.GetValue(MinimumHorizontalDragDistanceProperty);
        }

        public static void SetMinimumHorizontalDragDistance(UIElement source, double value)
        {
            source.SetValue(MinimumHorizontalDragDistanceProperty, value);
        }
        #endregion

        #region MinimumVerticalDragDistance Attached DependencyProperty
        public static readonly DependencyProperty MinimumVerticalDragDistanceProperty = DependencyProperty.RegisterAttached("MinimumVerticalDragDistance",
            typeof(double),
            typeof(DragDropManager),
            new PropertyMetadata(SystemParameters.MinimumVerticalDragDistance));

        public static double GetMinimumVerticalDragDistance(UIElement source)
        {
            return (double)source.GetValue(MinimumVerticalDragDistanceProperty);
        }

        public static void SetMinimumVerticalDragDistance(UIElement source, double value)
        {
            source.SetValue(MinimumVerticalDragDistanceProperty, value);
        }
        #endregion

        #region DragInitialize RoutedEvent
        public static readonly RoutedEvent DragInitializeEvent = EventManager.RegisterRoutedEvent("DragInitialize",
            RoutingStrategy.Bubble,
            typeof(DragInitializeEventHandler),
            typeof(DragDropManager));

        public static void AddDragInitializeHandler(DependencyObject element, DragInitializeEventHandler handler)
        {
            element.AddHandler(DragInitializeEvent, handler);
        }

        public static void AddDragInitializeEventHandler(DependencyObject element, DragInitializeEventHandler handler, bool handledEventsToo)
        {
            if (element is UIElement uiElement) uiElement.AddHandler(DragInitializeEvent, handler, handledEventsToo);
            else if (element is ContentElement contentElement) contentElement.AddHandler(DragInitializeEvent, handler, handledEventsToo);
            else element.AddHandler(DragInitializeEvent, handler);
        }

        public static void RemoveDragInitializeHandler(DependencyObject element, DragInitializeEventHandler handler)
        {
            element.RemoveHandler(DragInitializeEvent, handler);
        }
        #endregion

        #region DragDropCompleted RoutedEvent
        public static readonly RoutedEvent DragDropCompletedEvent = EventManager.RegisterRoutedEvent("DragDropCompleted",
            RoutingStrategy.Bubble,
            typeof(DragDropCompletedEventHandler),
            typeof(DragDropManager));

        public static void AddDragDropCompletedHandler(DependencyObject element, DragDropCompletedEventHandler handler)
        {
            element.AddHandler(DragDropCompletedEvent, handler);
        }

        public static void AddDragDropCompletedEventHandler(DependencyObject element, DragDropCompletedEventHandler handler, bool handledEventsToo)
        {
            if (element is UIElement uiElement) uiElement.AddHandler(DragDropCompletedEvent, handler, handledEventsToo);
            else if (element is ContentElement contentElement) contentElement.AddHandler(DragDropCompletedEvent, handler, handledEventsToo);
            else element.AddHandler(DragDropCompletedEvent, handler);
        }

        public static void RemoveDragDropCompletedHandler(DependencyObject element, DragDropCompletedEventHandler handler)
        {
            element.RemoveHandler(DragDropCompletedEvent, handler);
        }
        #endregion

        #region DragDropCanceled RoutedEvent
        public static readonly RoutedEvent DragDropCanceledEvent = EventManager.RegisterRoutedEvent("DragDropCanceled",
            RoutingStrategy.Bubble,
            typeof(DragDropCanceledEventHandler),
            typeof(DragDropManager));

        public static void AddDragDropCanceledHandler(DependencyObject element, DragDropCanceledEventHandler handler)
        {
            element.AddHandler(DragDropCanceledEvent, handler);
        }

        public static void AddDragDropCanceledEventHandler(DependencyObject element, DragDropCanceledEventHandler handler, bool handledEventsToo)
        {
            if (element is UIElement uiElement) uiElement.AddHandler(DragDropCanceledEvent, handler, handledEventsToo);
            else if (element is ContentElement contentElement) contentElement.AddHandler(DragDropCanceledEvent, handler, handledEventsToo);
            else element.AddHandler(DragDropCanceledEvent, handler);
        }

        public static void RemoveDragDropCanceledHandler(DependencyObject element, DragDropCanceledEventHandler handler)
        {
            element.RemoveHandler(DragDropCanceledEvent, handler);
        }
        #endregion

        public static bool DragInProgress { get; private set; }
        private static object _selectionSupressedItem;
        internal static DragInfo DragInfo;
        private static Window _dragWindow;

        private static void OnMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            DragInfo = null;

            // Wenn mehr als einmal geklickt wurde, wird davon ausgegangen, dass etwas anderes als DragDrop passieren soll
            if (e.ClickCount != 1) return;

            // DragInfo für das geklickte Element erstellen
            var dragInfo = new DragInfo(sender, e);

            if (dragInfo.SourceElement is ItemsControl control && control.AllowsMultiSelection()) control.Focus();

            if (dragInfo.SourceElementItem == null) return;

            var itemsControl = sender as ItemsControl;

            // Bei Multi-Selection müssen extra Schritte gemacht werden, um DragDrop für mehrere Items zu ermöglichen
            if ((Keyboard.Modifiers & ModifierKeys.Shift) == 0 && (Keyboard.Modifiers & ModifierKeys.Control) == 0 && dragInfo.SourceElementItem != null && itemsControl != null && itemsControl.AllowsMultiSelection())
            {
                var selectedItems = itemsControl.GetSelectedItems().OfType<object>().ToList();

                if (selectedItems.Count > 1 && selectedItems.Contains(dragInfo.SourceItem))
                {
                    _selectionSupressedItem = dragInfo.SourceItem;
                    e.Handled = true;
                }
            }

            // DragInfo setzen
            DragInfo = dragInfo;
        }

        private static void OnMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            var elementPosition = e.GetPosition((IInputElement)sender);

            if ((sender is TabControl) && !HitTestHelper.HitTestForType<TabPanel>(sender, elementPosition))
            {
                DragInfo = null;
                _selectionSupressedItem = null;
                return;
            }

            var dragInfo = DragInfo;

            // Wenn der Selection-Mechanismus des Controls blockiert wurde, hier manuell ausführen
            if (sender is ItemsControl itemsControl && dragInfo != null && _selectionSupressedItem != null && _selectionSupressedItem == dragInfo.SourceItem)
            {
                if ((Keyboard.Modifiers & ModifierKeys.Control) != 0)
                {
                    itemsControl.SetItemSelected(dragInfo.SourceItem, false);
                }
                else if ((Keyboard.Modifiers & ModifierKeys.Shift) == 0)
                {
                    itemsControl.SetSelectedItem(dragInfo.SourceItem);
                }
            }

            DragInfo = null;
            _selectionSupressedItem = null;
        }

        private static void OnMouseMove(object sender, MouseEventArgs e)
        {
            // Es muss eine DragInfo vorhanden sein, um die erwartete Verhaltensweise zu garantieren
            if (DragInfo == null || DragInProgress) return;

            // Wenn der sender kein UIElement ist, kann der Vorgang nicht durchgeführt werden
            if (e.LeftButton == MouseButtonState.Pressed && sender is UIElement element)
            {
                DragInfo.SourceElement.ReleaseMouseCapture();

                var currentPosition = e.GetPosition(element);

                // Wenn das momentane Element nicht das SourceElement aus der DragInfo ist, kann es zu Problemen kommen
                if (element != DragInfo.SourceElement || (Math.Abs(currentPosition.X - DragInfo.StartingPoint.X) <= GetMinimumHorizontalDragDistance(DragInfo.SourceElement)
                    && Math.Abs(currentPosition.Y - DragInfo.StartingPoint.Y) <= GetMinimumVerticalDragDistance(DragInfo.SourceElement))) return;

                var eventArgs = new DragInitializeEventArgs(DragInfo);

                // Initialize-Event auslösen
                element.RaiseEvent(eventArgs);

                // Soll der Vorgang abgebrochen werden oder gibt es keine Daten?
                if (eventArgs.Cancel || eventArgs.Data == null) return;

                DragInfo.VisualOffset = eventArgs.VisualOffset;
                DragInfo.Effects = eventArgs.AllowedEffects;

                // DragWindow erstellen
                _dragWindow = new DragAdornerWindow(eventArgs.DragVisual);

                var point = NativeMethods.GetCursorPosition();

                // Position des DragWindows setzen und es einblenden
                _dragWindow.Left = point.X - DragInfo.VisualOffset.X;
                _dragWindow.Top = point.Y - DragInfo.VisualOffset.Y;
                _dragWindow.Show();

                try
                {
                    // Drag-Vorgang starten
                    DragInProgress = true;
                    var effects = System.Windows.DragDrop.DoDragDrop(element, eventArgs.Data, eventArgs.AllowedEffects);

                    if (effects == DragDropEffects.None)
                    {
                        var canceledEventArgs = new DragDropCanceledEventArgs(eventArgs.Data);

                        element.RaiseEvent(canceledEventArgs);
                    }
                    else
                    {
                        var completedEventArgs = new DragDropCompletedEventArgs(effects, eventArgs.Data);

                        element.RaiseEvent(completedEventArgs);
                    }
                }
                finally
                {
                    // Aufräumen
                    if (_dragWindow != null)
                    {
                        _dragWindow.Close();
                        _dragWindow = null;
                    }
                    DragInProgress = false;
                    DragInfo = null;
                }
            }
        }

        private static void OnQueryContinueDrag(object sender, QueryContinueDragEventArgs e)
        {
            if (e.Action == DragAction.Continue && e.KeyStates != DragDropKeyStates.LeftMouseButton)
            {
                if (_dragWindow == null) return;
                _dragWindow.Close();
                _dragWindow = null;
            }
        }

        private static void OnGiveFeedback(object sender, GiveFeedbackEventArgs e)
        {
            if (_dragWindow == null || DragInfo == null) return;

            var point = NativeMethods.GetCursorPosition();

            // Position des DragWindows updaten
            _dragWindow.Left = point.X - DragInfo.VisualOffset.X;
            _dragWindow.Top = point.Y - DragInfo.VisualOffset.Y;
        }
    }
}