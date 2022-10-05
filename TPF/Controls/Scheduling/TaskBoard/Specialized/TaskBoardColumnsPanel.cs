using System.Windows;
using System.Windows.Controls;

namespace TPF.Controls.Specialized.TaskBoard
{
    public class TaskBoardColumnsPanel : Panel
    {
        private Controls.TaskBoard _taskBoard;
        internal Controls.TaskBoard TaskBoard
        {
            get
            {
                if (_taskBoard == null) _taskBoard = this.ParentOfType<Controls.TaskBoard>();

                return _taskBoard;
            }
        }

        protected override Size MeasureOverride(Size availableSize)
        {
            var totalWidth = 0.0;

            if (TaskBoard != null)
            {
                var gap = TaskBoard.ColumnGap;
                var columnWidth = TaskBoard.ColumnWidth;
                var collapsedWidth = TaskBoard.CollapsedColumnWidth;

                for (int i = 0, count = InternalChildren.Count; i < count; i++)
                {
                    var child = InternalChildren[i];

                    if (child is TaskBoardColumn column)
                    {
                        var width = column.IsCollapsed ? collapsedWidth : columnWidth;

                        totalWidth += width;

                        child.Measure(new Size(width, availableSize.Height));

                        if (i < count - 1) totalWidth += gap;
                    }
                }
            }

            return new Size(totalWidth, availableSize.Height);
        }

        protected override Size ArrangeOverride(Size finalSize)
        {
            if (TaskBoard != null)
            {
                var gap = TaskBoard.ColumnGap;
                var columnWidth = TaskBoard.ColumnWidth;
                var collapsedWidth = TaskBoard.CollapsedColumnWidth;

                var x = 0.0;

                for (int i = 0, count = InternalChildren.Count; i < count; i++)
                {
                    var child = InternalChildren[i];

                    if (child is TaskBoardColumn column)
                    {
                        var width = column.IsCollapsed ? collapsedWidth : columnWidth;

                        var arrangeRect = new Rect(x, 0, width, finalSize.Height);

                        child.Arrange(arrangeRect);

                        x += width;

                        if (i < count - 1) x += gap;
                    }
                }
            }

            return finalSize;
        }
    }
}