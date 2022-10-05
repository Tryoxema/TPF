using System;

namespace TPF.Controls
{
    public class TaskBoardAutoGeneratingColumnEventArgs : EventArgs
    {
        public TaskBoardAutoGeneratingColumnEventArgs(TaskBoardColumn column)
        {
            Column = column;
        }

        public TaskBoardColumn Column { get; set; }

        public bool Cancel { get; set; }
    }
}