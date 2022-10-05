using System;
using System.Collections;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using TPF.DragDrop;
using TPF.DragDrop.Behaviors;
using TPF.Internal;

namespace TPF.Controls.Specialized.TaskBoard
{
    public class TaskBoardDragDropHelper : DragDropHelper<TaskBoardDragDropBehavior, TaskBoardDragDropState>
    {
        public TaskBoardDragDropHelper() { }

        protected override FrameworkElement GetDragSource(UIElement element)
        {
            if (element is Controls.TaskBoard taskBoard)
            {
                return taskBoard;
            }
            else if (element is TaskBoardColumn column)
            {
                return column.TaskBoard;
            }
            else if (element is TaskBoardItem item)
            {
                return item.Column?.TaskBoard;
            }
            else return element.ParentOfType<Controls.TaskBoard>();
        }

        protected override IEnumerable GetDraggedItems(FrameworkElement dragSource)
        {
            if (dragSource is TaskBoardItem taskBoardItem)
            {
                return new List<TaskBoardItem>() { taskBoardItem };
            }
            else return null;
        }

        protected override IList GetItemsSource(FrameworkElement element)
        {
            if (element is Controls.TaskBoard taskBoard)
            {
                return taskBoard.ItemsSource as IList;
            }
            return base.GetItemsSource(element);
        }

        protected virtual TaskBoardColumn GetColumn(UIElement element)
        {
            if (element is TaskBoardItem item)
            {
                return item.Column;
            }
            else if (element is TaskBoardColumn column)
            {
                return column;
            }

            return element.ParentOfType<TaskBoardColumn>();
        }

        protected override TaskBoardDragDropState CreateState(UIElement sourceElement, UIElement targetElement, DragEventArgs e = null)
        {
            var state = new TaskBoardDragDropState();

            var dragSource = GetDragSource(sourceElement);
            var sourceItemsSource = GetItemsSource(dragSource);
            var dragTarget = GetDragSource(targetElement);
            var targetItemsSource = GetItemsSource(dragTarget);
            var targetColumn = GetColumn(targetElement);
            var draggedItems = GetDraggedItems(sourceElement as FrameworkElement);
            var isControlDown = Keyboard.IsKeyDown(Key.LeftCtrl) || Keyboard.IsKeyDown(Key.RightCtrl);
            var isShiftDown = Keyboard.IsKeyDown(Key.LeftShift) || Keyboard.IsKeyDown(Key.RightShift);

            state.SourceControl = dragSource;
            state.SourceItemsSource = sourceItemsSource;
            state.TargetControl = dragTarget;
            state.TargetItemsSource = targetItemsSource;
            state.DraggedItems = draggedItems;
            state.IsControlDown = isControlDown;
            state.IsShiftDown = isShiftDown;
            state.TargetColumn = targetColumn;
            state.SetDragEventArgs(e);

            return state;
        }

        protected override DragVisualProviderData CreateDragVisualProviderData(TaskBoardDragDropState state, Point relativeStartPoint)
        {
            var host = state.SourceControl as FrameworkElement;

            var containers = new List<DependencyObject>();

            foreach (var item in state.DraggedItems)
            {
                containers.Add(item as TaskBoardItem);
            }

            var providerData = new DragVisualProviderData(host, containers, state.DraggedItems, relativeStartPoint)
            {
                Opacity = 0.5
            };

            return providerData;
        }

        protected override DropInfo GetDropInfoForPoint(UIElement host, Point relativePoint, DragEventArgs e)
        {
            var column = HitTestHelper.GetHitTestElementOfType<TaskBoardColumn>(host, relativePoint);

            var result = new DropInfo()
            {
                Target = column,
                PositionInTarget = relativePoint
            };

            if (!column.IsCollapsed)
            {
                var point = e.GetPosition(column);
                var container = column.GetItemContainerAt(point);

                if (container != null)
                {
                    result.TargetItem = container;

                    var insertIndex = column.ItemContainerGenerator.IndexFromContainer(container);

                    var orientation = DragDropManager.GetOrientation(column);

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

            result.AdornedElement = column.ChildOfType<ItemsPresenter>();
            if (result.AdornedElement == null) result.AdornedElement = column.ChildOfType<ScrollContentPresenter>();
            if (result.AdornedElement == null) result.AdornedElement = column.ChildOfType<ContentPresenter>();
            if (result.AdornedElement == null) result.AdornedElement = column;

            return result;
        }

        protected override bool ShouldShowDropVisual(TaskBoardDragDropState state)
        {
            if (state.TargetColumn != null && state.TargetColumn.IsCollapsed) return false;

            return base.ShouldShowDropVisual(state);
        }
    }
}