using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Collections.Generic;
using TPF.Internal;

namespace TPF.Controls
{
    public static class CalloutService
    {
        #region CalloutOpening RoutedEvent
        public static readonly RoutedEvent CalloutOpeningEvent = EventManager.RegisterRoutedEvent("CalloutOpening",
            RoutingStrategy.Bubble,
            typeof(CancelableRoutedEventHandler),
            typeof(CalloutService));

        public static void AddCalloutOpeningHandler(DependencyObject element, CancelableRoutedEventHandler handler)
        {
            element.AddHandler(CalloutOpeningEvent, handler);
        }

        public static void AddCalloutOpeningEventHandler(DependencyObject element, CancelableRoutedEventHandler handler, bool handledEventsToo)
        {
            if (element is UIElement uiElement) uiElement.AddHandler(CalloutOpeningEvent, handler, handledEventsToo);
            else if (element is ContentElement contentElement) contentElement.AddHandler(CalloutOpeningEvent, handler, handledEventsToo);
            else element.AddHandler(CalloutOpeningEvent, handler);
        }

        public static void RemoveCalloutOpeningHandler(DependencyObject element, CancelableRoutedEventHandler handler)
        {
            element.RemoveHandler(CalloutOpeningEvent, handler);
        }
        #endregion

        #region CalloutOpened RoutedEvent
        public static readonly RoutedEvent CalloutOpenedEvent = EventManager.RegisterRoutedEvent("CalloutOpened",
            RoutingStrategy.Bubble,
            typeof(RoutedEventHandler),
            typeof(CalloutService));

        public static void AddCalloutOpenedHandler(DependencyObject element, RoutedEventHandler handler)
        {
            element.AddHandler(CalloutOpenedEvent, handler);
        }

        public static void AddCalloutOpenedEventHandler(DependencyObject element, RoutedEventHandler handler, bool handledEventsToo)
        {
            if (element is UIElement uiElement) uiElement.AddHandler(CalloutOpenedEvent, handler, handledEventsToo);
            else if (element is ContentElement contentElement) contentElement.AddHandler(CalloutOpenedEvent, handler, handledEventsToo);
            else element.AddHandler(CalloutOpenedEvent, handler);
        }

        public static void RemoveCalloutOpenedHandler(DependencyObject element, RoutedEventHandler handler)
        {
            element.RemoveHandler(CalloutOpenedEvent, handler);
        }
        #endregion

        #region CalloutClosing RoutedEvent
        public static readonly RoutedEvent CalloutClosingEvent = EventManager.RegisterRoutedEvent("CalloutClosing",
            RoutingStrategy.Bubble,
            typeof(CancelableRoutedEventHandler),
            typeof(CalloutService));

        public static void AddCalloutClosingHandler(DependencyObject element, CancelableRoutedEventHandler handler)
        {
            element.AddHandler(CalloutClosingEvent, handler);
        }

        public static void AddCalloutClosingEventHandler(DependencyObject element, CancelableRoutedEventHandler handler, bool handledEventsToo)
        {
            if (element is UIElement uiElement) uiElement.AddHandler(CalloutClosingEvent, handler, handledEventsToo);
            else if (element is ContentElement contentElement) contentElement.AddHandler(CalloutClosingEvent, handler, handledEventsToo);
            else element.AddHandler(CalloutClosingEvent, handler);
        }

        public static void RemoveCalloutClosingHandler(DependencyObject element, CancelableRoutedEventHandler handler)
        {
            element.RemoveHandler(CalloutClosingEvent, handler);
        }
        #endregion

        #region CalloutClosed RoutedEvent
        public static readonly RoutedEvent CalloutClosedEvent = EventManager.RegisterRoutedEvent("CalloutClosed",
            RoutingStrategy.Bubble,
            typeof(RoutedEventHandler),
            typeof(CalloutService));

        public static void AddCalloutClosedHandler(DependencyObject element, RoutedEventHandler handler)
        {
            element.AddHandler(CalloutClosedEvent, handler);
        }

        public static void AddCalloutClosedEventHandler(DependencyObject element, RoutedEventHandler handler, bool handledEventsToo)
        {
            if (element is UIElement uiElement) uiElement.AddHandler(CalloutClosedEvent, handler, handledEventsToo);
            else if (element is ContentElement contentElement) contentElement.AddHandler(CalloutClosedEvent, handler, handledEventsToo);
            else element.AddHandler(CalloutClosedEvent, handler);
        }

        public static void RemoveCalloutClosedHandler(DependencyObject element, RoutedEventHandler handler)
        {
            element.RemoveHandler(CalloutClosedEvent, handler);
        }
        #endregion

        private static readonly HashSet<Callout> _openCallouts = new HashSet<Callout>();
        private const string _canvasMarker = "InternalCalloutContainer";

        public static void ShowPopup(Callout callout, FrameworkElement placementTarget, CalloutPopupOptions options = null)
        {
            if (callout == null) throw new ArgumentNullException(nameof(callout));
            if (placementTarget == null) throw new ArgumentNullException(nameof(placementTarget));

            options = options ?? new CalloutPopupOptions();

            if (_openCallouts.Contains(callout))
            {
                if (callout.Parent is Canvas canvas && canvas.Tag as string == _canvasMarker)
                {
                    var oldPopup = canvas.ParentOfType<Popup>();

                    if (oldPopup != null)
                    {
                        oldPopup.IsOpen = false;
                        canvas.Children.Remove(callout);

                        if (oldPopup.PlacementTarget != null)
                        {
                            var closedArgs = new RoutedEventArgs(CalloutClosedEvent);
                            oldPopup.PlacementTarget.RaiseEvent(closedArgs);
                        }
                    }
                }
                // Wenn das Callout nicht direkt in unserem erzeugten Canvas ist, aber trotzdem ein Parent hat, können wir nicht weiter machen
                // In dem Fall befindet sich das Callout noch in einer anderen Struktur und wir kriegen eine Fehlermeldung,
                // wenn wir es in eine zweite Struktur einfügen wollen
                else if (callout.Parent != null) return;

                // Wir entfernen das Callout aus der Liste, da ja eventuell über das Opening-Event der Vorgang noch abgebrochen werden kann
                _openCallouts.Remove(callout);
            }

            var openingArgs = new CancelableRoutedEventArgs(CalloutOpeningEvent);
            // Das Opening-Event fragen wir ab bevor wir das Popup erstellen, da wir bei einem Abbruch den ganzen Aufwand nicht betreiben müssen
            placementTarget.RaiseEvent(openingArgs);

            if (openingArgs.Cancel) return;

            var popup = CreatePopup(callout, placementTarget, options);

            popup.IsOpen = true;

            _openCallouts.Add(callout);

            // Wenn StaysOpen auf false ist, dann EventHandler für das Closed-Event anhängen, damit wir unsere interne Liste aufräumen können
            if (!options.StaysOpen)
            {
                popup.Closed += Popup_Closed;
            }

            var openedEventArgs = new RoutedEventArgs(CalloutOpenedEvent);
            
            placementTarget.RaiseEvent(openedEventArgs);
        }

        public static void ClosePopup(Callout callout)
        {
            if (callout == null) throw new ArgumentNullException(nameof(callout));

            var popup = callout.ParentOfType<Popup>();

            if (popup == null || popup.PlacementTarget == null || !popup.IsOpen) return;

            var closingEventArgs = new CancelableRoutedEventArgs(CalloutClosingEvent);

            popup.PlacementTarget.RaiseEvent(closingEventArgs);

            if (closingEventArgs.Cancel) return;

            popup.IsOpen = false;

            _openCallouts.Remove(callout);
            CleanupPopup(popup);

            var closedEventArgs = new RoutedEventArgs(CalloutOpenedEvent);

            popup.PlacementTarget.RaiseEvent(closedEventArgs);
        }

        public static void CloseAllPopups()
        {
            var callouts = _openCallouts.ToList();

            for (int i = 0; i < callouts.Count; i++)
            {
                ClosePopup(callouts[i]);
            }
        }

        private static Popup CreatePopup(Callout callout, FrameworkElement placementTarget, CalloutPopupOptions options)
        {
            var popup = new Popup()
            {
                PlacementTarget = placementTarget,
                Placement = PlacementMode.Custom,
                AllowsTransparency = true,
                StaysOpen = options.StaysOpen,
                PopupAnimation = options.PopupAnimation,
            };

            // Wenn das Popup offen bleiben soll, bis es manuell geschlossen wird, bewegen wir unser Popup zusammen mit seinem Ziel mit
            if (options.StaysOpen)
            {
                var parentWindow = placementTarget.ParentOfType<Window>();

                if (parentWindow != null)
                {
                    parentWindow.SizeChanged += (_, __) => RefreshPopupPosition(popup);
                    parentWindow.LocationChanged += (_, __) => RefreshPopupPosition(popup);
                } 
            }

            var size = MeasureAndCalculateCalloutSize(callout, out var xOffset, out var yOffset);

            var canvas = new Canvas()
            {
                Width = size.Width,
                Height = size.Height,
                // Ein Marker für den Canvas, damit wir ihn von anderen Canvas-Elementen unterscheiden können als von uns generiert
                Tag = _canvasMarker,
            };

            canvas.Children.Add(callout);

            UpdateCalloutCanvasPosition(callout, xOffset, yOffset);

            popup.Child = canvas;

            UpdatePopupLocation(popup, placementTarget, size, options);

            // Sollte sich der Inhalt des Callouts ändern wärend es offen ist, müssen wir das ganze neu positionieren
            callout.SizeChanged += (_, __) =>
            {
                if (!popup.IsOpen) return;
                
                var newSize = MeasureAndCalculateCalloutSize(callout, out xOffset, out yOffset);

                canvas.Width = newSize.Width;
                canvas.Height = newSize.Height;

                UpdateCalloutCanvasPosition(callout, xOffset, yOffset);
                UpdatePopupLocation(popup, placementTarget, newSize, options);
            };

            // Wenn sich die Größe unseres Zielelements ändert, muss das Popup auch neu positioniert werden
            placementTarget.SizeChanged += (_, __) =>
            {
                if (!popup.IsOpen) return;
                
                var newSize = MeasureAndCalculateCalloutSize(callout, out xOffset, out yOffset);

                UpdatePopupLocation(popup, placementTarget, newSize, options);
            };

            return popup;
        }

        private static void RefreshPopupPosition(Popup popup)
        {
            if (!popup.IsOpen) return;

            popup.HorizontalOffset += 1;
            popup.HorizontalOffset -= 1;
        }

        private static void UpdateCalloutCanvasPosition(Callout callout, double xOffset, double yOffset)
        {
            Canvas.SetTop(callout, -1 * yOffset);
            Canvas.SetLeft(callout, -1 * xOffset);
        }

        private static Size MeasureAndCalculateCalloutSize(Callout callout, out double xOffset, out double yOffset)
        {
            xOffset = yOffset = 0;

            callout.Measure(Utility.InfiniteSize);

            var desiredSize = callout.DesiredSize.Width == 0 && callout.DesiredSize.Height == 0 ? new Size(100, 40) : callout.DesiredSize;

            if (callout.Type == CalloutType.Custom || callout.ArrowType == CalloutArrowType.None) return desiredSize;

            var anchorX = callout.ArrowTargetAnchorPoint.X;
            var anchorY = callout.ArrowTargetAnchorPoint.Y;

            var width = desiredSize.Width;
            var height = desiredSize.Height;

            if (anchorX < 0 || anchorX > 1)
            {
                var addition = anchorX > 1 ? anchorX : 1 + Math.Abs(anchorX);

                if (anchorX < 0) xOffset = anchorX * width;

                width *= addition;
            }

            if (anchorY < 0 || anchorY > 1)
            {
                var addition = anchorY > 1 ? anchorY : 1 + Math.Abs(anchorY);

                if (anchorY < 0) yOffset = anchorY * height;

                height *= addition;
            }

            return new Size(width, height);
        }

        private static void UpdatePopupLocation(Popup popup, FrameworkElement target, Size popupSize, CalloutPopupOptions options)
        {
            var targetWidth = target.RenderSize.Width;
            var targetHeight = target.RenderSize.Height;

            var size = popupSize;

            var width = size.Width;
            var height = size.Height;
            var placement = options.Placement;

            popup.VerticalOffset = options.VerticalOffset;
            popup.HorizontalOffset = options.HorizontalOffset;

            double x = 0d, y = 0d, offsetX = 0d, offsetY = 0d;

            switch (placement)
            {
                case CalloutPlacement.TopLeft:
                {
                    y = -height;
                    break;
                }
                case CalloutPlacement.Top:
                {
                    offsetX = (targetWidth - width) / 2;
                    y = -height;
                    break;
                }
                case CalloutPlacement.TopRight:
                {
                    offsetX = targetWidth - width;
                    y = -height;
                    break;
                }
                case CalloutPlacement.BottomLeft:
                {
                    y = targetHeight;
                    break;
                }
                case CalloutPlacement.Bottom:
                {
                    offsetX = (targetWidth - width) / 2;
                    y = targetHeight;
                    break;
                }
                case CalloutPlacement.BottomRight:
                {
                    offsetX = targetWidth - width;
                    y = targetHeight;
                    break;
                }
                case CalloutPlacement.LeftTop:
                {
                    x = -width;
                    break;
                }
                case CalloutPlacement.Left:
                {
                    offsetY = (targetHeight - height) / 2;
                    x = -width;
                    break;
                }
                case CalloutPlacement.LeftBottom:
                {
                    offsetY = targetHeight - height;
                    x = -width;
                    break;
                }
                case CalloutPlacement.RightTop:
                {
                    x = targetWidth;
                    break;
                }
                case CalloutPlacement.Right:
                {
                    offsetY = (targetHeight - height) / 2;
                    x = targetWidth;
                    break;
                }
                case CalloutPlacement.RightBottom:
                {
                    offsetY = targetHeight - height;
                    x = targetWidth;
                    break;
                }
            }

            popup.HorizontalOffset += offsetX;
            popup.VerticalOffset += offsetY;
            // Wenn in Windows der Linkshänder-Modus aktiviert ist stimmt die Automatisch bestimmte Position nicht mehr, also müssen wir die hier selbst festlegen
            var popupPlacement = new CustomPopupPlacement(new Point(x, y), PopupPrimaryAxis.None);
            popup.CustomPopupPlacementCallback = new CustomPopupPlacementCallback((_, __, ___) => new[] { popupPlacement });
        }

        private static void CleanupPopup(Popup popup)
        {
            var callout = popup.Child?.ChildOfType<Callout>();

            if (callout != null)
            {
                _openCallouts.Remove(callout);

                if (callout.Parent is Canvas canvas) canvas.Children.Remove(callout);
            }
        }

        private static void Popup_Closed(object sender, EventArgs e)
        {
            var popup = (Popup)sender;

            CleanupPopup(popup);

            popup.Closed -= Popup_Closed;
        }
    }
}