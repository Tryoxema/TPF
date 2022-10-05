using System;
using System.Linq;
using TPF.DragDrop.Behaviors;
using TPF.Internal;

namespace TPF.Controls.Specialized.TaskBoard
{
    public class TaskBoardDragDropBehavior : DragDropBehavior<TaskBoardDragDropState>
    {
        public override bool CanStartDrag(TaskBoardDragDropState state)
        {
            var item = state.DraggedItems.OfType<TaskBoardItem>().FirstOrDefault();

            return item?.Column.AllowDrag == true;
        }

        public override bool CanDrop(TaskBoardDragDropState state)
        {
            if (state.DraggedItems == null || state.TargetColumn == null || state.TargetColumn.TaskBoard == null) return false;

            return true;
        }

        public override void Drop(TaskBoardDragDropState state)
        {
            if (state.DraggedItems == null || state.TargetColumn == null || state.TargetColumn.TaskBoard == null) return;

            var item = state.DraggedItems.OfType<TaskBoardItem>().FirstOrDefault();

            if (item == null) return;

            var content = item.Content;

            var taskBoard = state.TargetColumn.TaskBoard;

            // Sind Source und Target gleich?
            if (state.SourceControl == state.TargetControl)
            {
                // Wenn bewegen in der Collection erlaubt ist, dann Items bewegen
                if (AllowReorder)
                {
                    if (item.Column == state.TargetColumn)
                    {
                        var index = state.InsertIndex;

                        var oldIndex = state.TargetColumn.Items.IndexOf(content);

                        if (oldIndex != -1 && oldIndex < index) index--;

                        var endingEventArgs = new CardDragEndingEventArgs(item, state.TargetColumn, state.TargetColumn, oldIndex, index);

                        taskBoard.OnCardDragEnding(endingEventArgs);

                        if (endingEventArgs.Cancel) return;

                        state.TargetColumn.Items.Remove(content);

                        if (index == -1) state.TargetColumn.Items.Add(content);
                        else
                        {
                            state.TargetColumn.Items.Insert(index, content);
                            index++;
                        }

                        var endedEventArgs = new CardDragEndedEventArgs(content, state.TargetColumn, state.TargetColumn, oldIndex, index);

                        taskBoard.OnCardDragEnded(endedEventArgs);
                    }
                    else
                    {
                        var oldColumn = item.Column;

                        var index = state.InsertIndex;

                        var oldIndex = oldColumn.Items.IndexOf(content);

                        var endingEventArgs = new CardDragEndingEventArgs(item, oldColumn, state.TargetColumn, oldIndex, index);

                        taskBoard.OnCardDragEnding(endingEventArgs);

                        if (endingEventArgs.Cancel) return;

                        PropertyHelper.SetPropertyValueFromPath(item.Content, taskBoard.ColumnMappingPath, state.TargetColumn.Value);

                        state.TargetColumn.Items.Remove(content);

                        if (index == -1) state.TargetColumn.Items.Add(content);
                        else
                        {
                            state.TargetColumn.Items.Insert(index, content);
                            index++;
                        }

                        var endedEventArgs = new CardDragEndedEventArgs(content, oldColumn, state.TargetColumn, oldIndex, index);

                        taskBoard.OnCardDragEnded(endedEventArgs);
                    }
                }
            }
            else
            {
                state.TargetItemsSource.Insert(state.InsertIndex, content);
            }
        }

        public override void DragDropCompleted(TaskBoardDragDropState state)
        {
            if (!ShouldRemoveItemsFromSource(state)) return;

            var item = state.DraggedItems.OfType<TaskBoardItem>().FirstOrDefault();

            if (item == null) return;

            state.SourceItemsSource.Remove(item.Content);
        }
    }
}