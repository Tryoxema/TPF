using System;

namespace TPF.Controls.Specialized.TaskBoard
{
    public class CardDragEndedEventArgs : EventArgs
    {
        internal CardDragEndedEventArgs(object item, TaskBoardColumn oldColumn, TaskBoardColumn newColumn, int oldIndex, int newIndex)
        {
            MovedItem = item;
            OldValue = oldColumn?.Value;
            NewValue = newColumn?.Value;
            OldIndex = oldIndex;
            NewIndex = newIndex;
            OldColumn = oldColumn;
            NewColumn = newColumn;
        }

        public object MovedItem { get; }

        public object OldValue { get; }

        public object NewValue { get; }

        public int OldIndex { get; }

        public int NewIndex { get; }

        public TaskBoardColumn OldColumn { get; }

        public TaskBoardColumn NewColumn { get; }
    }
}