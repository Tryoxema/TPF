using TPF.DragDrop.Behaviors;

namespace TPF.Controls.Specialized.TaskBoard
{
    public class TaskBoardDragDropState : DragDropState
    {
        public TaskBoardColumn TargetColumn { get; protected internal set; }
    }
}