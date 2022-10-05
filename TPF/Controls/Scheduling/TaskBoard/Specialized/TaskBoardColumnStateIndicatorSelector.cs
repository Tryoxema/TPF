using System.Windows.Media;

namespace TPF.Controls.Specialized.TaskBoard
{
    public class TaskBoardColumnStateIndicatorSelector
    {
        public virtual Brush SelectIndicatorBrush(TaskBoardColumn column)
        {
            if (column.Maximum > 0 && column.Items.Count > column.Maximum) return Brushes.Crimson;

            return null;
        }
    }
}