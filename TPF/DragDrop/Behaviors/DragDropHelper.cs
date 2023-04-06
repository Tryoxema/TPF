using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using TPF.Controls;
using TPF.Internal;

namespace TPF.DragDrop.Behaviors
{
    public abstract class DragDropHelper<TBehavior, TState>
        where TBehavior : DragDropBehavior<TState>
        where TState : DragDropState, new()
    {
        protected DragDropHelper()
        {

        }

        public TBehavior DragDropBehavior { get; set; }

        public IDragVisualProvider DragVisualProvider { get; set; }

        public IDropVisualProvider DropVisualProvider { get; set; }

        private DropAdorner _dropAdorner;

        internal void HookupEvents(FrameworkElement element)
        {
            DragDropManager.AddDragInitializeHandler(element, new DragInitializeEventHandler(OnDragInitialize));
            DragDropManager.AddDragDropCompletedEventHandler(element, new DragDropCompletedEventHandler(OnDragDropCompleted), true);
            DragDropManager.AddDragDropCanceledEventHandler(element, new DragDropCanceledEventHandler(OnDragDropCanceled), true);
            element.DragEnter += Element_DragEnter;
            element.DragOver += Element_DragOver;
            element.DragLeave += Element_DragLeave;
            element.Drop += Element_Drop;
        }

        internal void UnhookEvents(FrameworkElement element)
        {
            DragDropManager.RemoveDragInitializeHandler(element, new DragInitializeEventHandler(OnDragInitialize));
            DragDropManager.RemoveDragDropCompletedHandler(element, new DragDropCompletedEventHandler(OnDragDropCompleted));
            DragDropManager.RemoveDragDropCanceledHandler(element, new DragDropCanceledEventHandler(OnDragDropCanceled));
            element.DragEnter -= Element_DragEnter;
            element.DragOver -= Element_DragOver;
            element.DragLeave -= Element_DragLeave;
            element.Drop -= Element_Drop;
        }

        protected virtual TState CreateState(UIElement sourceElement, UIElement targetElement, DragEventArgs e)
        {
            var state = new TState();

            var dragSource = GetDragSource(sourceElement);
            var sourceItemsSource = GetItemsSource(dragSource);
            var dragTarget = GetDragSource(targetElement);
            var targetItemsSource = GetItemsSource(dragTarget);
            var draggedItems = GetDraggedItems(dragSource);
            var isControlDown = Keyboard.IsKeyDown(Key.LeftCtrl) || Keyboard.IsKeyDown(Key.RightCtrl);
            var isShiftDown = Keyboard.IsKeyDown(Key.LeftShift) || Keyboard.IsKeyDown(Key.RightShift);

            state.SourceControl = dragSource;
            state.SourceItemsSource = sourceItemsSource;
            state.TargetControl = dragTarget;
            state.TargetItemsSource = targetItemsSource;
            state.DraggedItems = draggedItems;
            state.IsControlDown = isControlDown;
            state.IsShiftDown = isShiftDown;
            state.SetDragEventArgs(e);

            return state;
        }

        protected virtual DragVisualProviderData CreateDragVisualProviderData(TState state, Point relativeStartPoint)
        {
            var host = state.SourceControl as FrameworkElement;

            var containers = new List<DependencyObject>();

            foreach (var item in state.DraggedItems)
            {
                containers.Add(GetContainerForItem(host, item));
            }

            var providerData = new DragVisualProviderData(host, containers, state.DraggedItems, relativeStartPoint);

            return providerData;
        }

        protected abstract FrameworkElement GetDragSource(UIElement element);

        protected abstract IEnumerable GetDraggedItems(FrameworkElement dragSource);

        protected virtual DependencyObject GetContainerForItem(FrameworkElement host, object item)
        {
            if (host is ItemsControl itemsControl)
            {
                return itemsControl.ItemContainerGenerator.ContainerFromItem(item);
            }
            else return null;
        }

        protected virtual IList GetItemsSource(FrameworkElement element)
        {
            if (element is ItemsControl itemsControl)
            {
                var itemsSource = itemsControl.ItemsSource ?? itemsControl.Items;

                return itemsSource as IList;
            }
            else return null;
        }

        protected virtual ScrollViewer GetScrollViewer(UIElement host, DragEventArgs e)
        {
            return host.ChildOfType<ScrollViewer>();
        }

        protected virtual DropInfo GetDropInfoForPoint(UIElement host, Point relativePoint, DragEventArgs e)
        {
            var result = new DropInfo()
            {
                Target = host,
                PositionInTarget = relativePoint
            };

            result.TargetScrollViewer = GetScrollViewer(host, e);

            if (host is ItemsControl itemsControl)
            {
                var container = itemsControl.GetItemContainerAt(relativePoint);

                if (container != null)
                {
                    result.TargetItem = container;

                    var insertIndex = itemsControl.ItemContainerGenerator.IndexFromContainer(container);

                    var orientation = DragDropManager.GetOrientation(itemsControl);

                    if (orientation == Orientation.Vertical)
                    {
                        var y = e.GetPosition(container).Y;
                        var height = container.RenderSize.Height;

                        if (y > height / 2)
                        {
                            result.DropPosition = RelativeDropPosition.After;
                            insertIndex++;
                        }
                        else result.DropPosition = RelativeDropPosition.Before;
                    }
                    else
                    {
                        var x = e.GetPosition(container).X;
                        var width = container.RenderSize.Width;

                        if (x > width / 2)
                        {
                            result.DropPosition = RelativeDropPosition.After;
                            insertIndex++;
                        }
                        else result.DropPosition = RelativeDropPosition.Before;
                    }

                    result.InsertIndex = insertIndex;
                }
            }

            result.AdornedElement = host.ChildOfType<ItemsPresenter>();
            if (result.AdornedElement == null) result.AdornedElement = host.ChildOfType<ScrollContentPresenter>();
            if (result.AdornedElement == null) result.AdornedElement = host.ChildOfType<ContentPresenter>();
            if (result.AdornedElement == null) result.AdornedElement = host;

            return result;
        }

        protected virtual bool ShouldShowDropVisual(TState state)
        {
            return true;
        }

        protected virtual void DoScrolling(ScrollViewer scrollViewer, DragEventArgs e)
        {
            if (scrollViewer == null) return;

            var position = e.GetPosition(scrollViewer);
            var verticalScrollMargin = Math.Min(20, scrollViewer.ActualHeight / 2);
            var horizontalScrollMargin = Math.Min(20, scrollViewer.ActualWidth / 2);

            var maximumHorizontalOffset = scrollViewer.ExtentWidth - scrollViewer.ViewportWidth;
            var maximumVerticalOffset = scrollViewer.ExtentHeight - scrollViewer.ViewportHeight;

            // Horizontal
            if (position.X < horizontalScrollMargin && scrollViewer.HorizontalOffset > 0)
            {
                var targetOffset = Math.Max(0, scrollViewer.HorizontalOffset - horizontalScrollMargin);

                scrollViewer.ScrollToHorizontalOffset(targetOffset);
            }
            else if (position.X >= scrollViewer.ActualWidth - horizontalScrollMargin && scrollViewer.HorizontalOffset < maximumHorizontalOffset)
            {
                var targetOffset = Math.Min(maximumHorizontalOffset, scrollViewer.HorizontalOffset + horizontalScrollMargin);

                scrollViewer.ScrollToHorizontalOffset(targetOffset);
            }

            // Vertikal
            if (position.Y < verticalScrollMargin && scrollViewer.VerticalOffset > 0)
            {
                var targetOffset = Math.Max(0, scrollViewer.VerticalOffset - verticalScrollMargin);

                scrollViewer.ScrollToVerticalOffset(targetOffset);
            }
            else if (position.Y >= scrollViewer.ActualHeight - verticalScrollMargin && scrollViewer.VerticalOffset < maximumVerticalOffset)
            {
                var targetOffset = Math.Min(maximumVerticalOffset, scrollViewer.VerticalOffset + verticalScrollMargin);

                scrollViewer.ScrollToVerticalOffset(targetOffset);
            }
        }

        private void OnDragInitialize(object sender, DragInitializeEventArgs e)
        {
            if (DragDropBehavior == null) return;

            // DragDropEffects festlegen
            e.AllowedEffects = DragDropEffects.All;
            // DragDropState erstellen
            var state = CreateState(e.SourceElement, null, null);
            // e.Cancel anhand DragDropBehavior ermitteln
            e.Cancel = !DragDropBehavior.CanStartDrag(state);
            if (e.Cancel) return;
            // DragVisualProviderData erstellen
            var providerData = CreateDragVisualProviderData(state, DragDropManager.DragInfo.PointInItem);
            // DragVisual ermitteln
            e.DragVisual = DragVisualProvider != null ? DragVisualProvider.CreateDragVisual(providerData) : new DefaultDragVisualProvider().CreateDragVisual(providerData);
            // Data ermitteln
            e.Data = new DataObject("DraggedItems", state.DraggedItems);

            DragDropManager.DragInfo.LastDragDropState = state;
        }

        private void Element_DragEnter(object sender, DragEventArgs e)
        {
            // DragDropState erstellen
            var state = CreateState(DragDropManager.DragInfo?.SourceElement, e.Source as UIElement, e);

            if (DragDropBehavior != null)
            {
                var canDrop = DragDropBehavior.CanDrop(state);

                if (canDrop) e.Effects = e.AllowedEffects;
                else e.Effects = DragDropEffects.None;

                e.Handled = true;
            }

            // Haben wir einen DropVisualProvider?
            if (DropVisualProvider != null && ShouldShowDropVisual(state))
            {
                var dropPosition = state.TargetControl != null ? e.GetPosition(state.TargetControl) : new Point();

                var dropInfo = GetDropInfoForPoint(state.TargetControl, dropPosition, e);

                var dropVisual = DropVisualProvider.CreateDropVisual();

                // DropAdorner erstellen
                _dropAdorner = new DropAdorner(dropInfo.AdornedElement ?? state.TargetControl, dropVisual);
                // Und dann noch Positionieren
                _dropAdorner.MoveElement(DropVisualProvider.GetPosition(dropInfo));

                if (e.Effects == DragDropEffects.None) _dropAdorner.Visibility = Visibility.Hidden;
                else _dropAdorner.Visibility = Visibility.Visible;
            }

            if (DragDropManager.DragInfo != null) DragDropManager.DragInfo.LastDragDropState = state;
        }

        private void Element_DragOver(object sender, DragEventArgs e)
        {
            // DragDropState erstellen
            var state = CreateState(DragDropManager.DragInfo?.SourceElement, e.Source as UIElement, e);

            if (DragDropBehavior != null)
            {
                var canDrop = DragDropBehavior.CanDrop(state);

                if (canDrop) e.Effects = e.AllowedEffects;
                else e.Effects = DragDropEffects.None;

                e.Handled = true;
            }

            var scrollViewer = GetScrollViewer(state.TargetControl, e);

            if (scrollViewer != null) DoScrolling(scrollViewer, e);

            // DropVisual eventuell bewegen
            if (DropVisualProvider != null && ShouldShowDropVisual(state))
            {
                var dropPosition = state.TargetControl != null ? e.GetPosition(state.TargetControl) : new Point();

                var dropInfo = GetDropInfoForPoint(state.TargetControl, dropPosition, e);

                dropInfo.AdornedElement = _dropAdorner.AdornedElement;

                _dropAdorner.MoveElement(DropVisualProvider.GetPosition(dropInfo));

                if (e.Effects == DragDropEffects.None) _dropAdorner.Visibility = Visibility.Hidden;
                else _dropAdorner.Visibility = Visibility.Visible;
            }

            if (DragDropManager.DragInfo != null) DragDropManager.DragInfo.LastDragDropState = state;
        }

        private void Element_DragLeave(object sender, DragEventArgs e)
        {
            // DropVisual entfernen
            if (_dropAdorner != null)
            {
                _dropAdorner.Remove();
                _dropAdorner = null;
            }
        }

        private void Element_Drop(object sender, DragEventArgs e)
        {
            // DropVisual entfernen
            if (_dropAdorner != null)
            {
                _dropAdorner.Remove();
                _dropAdorner = null;
            }

            // DragDropState erstellen
            var state = CreateState(DragDropManager.DragInfo?.SourceElement, e.Source as UIElement, e);

            var dropPosition = state.TargetControl != null ? e.GetPosition(state.TargetControl) : new Point();

            var dropInfo = GetDropInfoForPoint(state.TargetControl, dropPosition, e);

            state.InsertIndex = dropInfo.InsertIndex;

            if (DragDropBehavior != null) DragDropBehavior.Drop(state);

            if (DragDropManager.DragInfo != null) DragDropManager.DragInfo.LastDragDropState = state;
        }

        private void OnDragDropCompleted(object sender, DragDropCompletedEventArgs e)
        {
            if (DragDropBehavior == null) return;

            // Den letzten DragDropState aus der DragInfo verwenden
            DragDropBehavior.DragDropCompleted(DragDropManager.DragInfo?.LastDragDropState as TState);
        }

        private void OnDragDropCanceled(object sender, DragDropCanceledEventArgs e)
        {
            // DropVisual entfernen
            if (_dropAdorner != null)
            {
                _dropAdorner.Remove();
                _dropAdorner = null;
            }

            if (DragDropBehavior == null) return;

            // Den letzten DragDropState aus der DragInfo verwenden
            DragDropBehavior.DragDropCanceled(DragDropManager.DragInfo?.LastDragDropState as TState);
        }
    }
}