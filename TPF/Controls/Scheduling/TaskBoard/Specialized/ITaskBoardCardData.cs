using System.Collections.Generic;

namespace TPF.Controls.Specialized.TaskBoard
{
    public interface ITaskBoardCardData
    {
        object Id { get; set; }

        string Assignee { get; set; }

        string Title { get; set; }

        string Description { get; set; }

        object State { get; set; }

        object Type { get; set; }

        object Icon { get; set; }

        IEnumerable<object> Tags { get; set; }
    }
}