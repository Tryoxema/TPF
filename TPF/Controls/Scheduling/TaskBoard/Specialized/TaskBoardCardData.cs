using System.Collections.Generic;

namespace TPF.Controls.Specialized.TaskBoard
{
    public class TaskBoardCardData : NotifyObject, ITaskBoardCardData
    {
        object _id;
        public object Id
        {
            get { return _id; }
            set { SetProperty(ref _id, value); }
        }

        string _assignee;
        public string Assignee
        {
            get { return _assignee; }
            set { SetProperty(ref _assignee, value); }
        }

        string _title;
        public string Title
        {
            get { return _title; }
            set { SetProperty(ref _title, value); }
        }

        string _description;
        public string Description
        {
            get { return _description; }
            set { SetProperty(ref _description, value); }
        }

        object _state;
        public object State
        {
            get { return _state; }
            set { SetProperty(ref _state, value); }
        }

        object _type;
        public object Type
        {
            get { return _type; }
            set { SetProperty(ref _type, value); }
        }

        object _icon;
        public object Icon
        {
            get { return _icon; }
            set { SetProperty(ref _icon, value); }
        }

        IEnumerable<object> _tags;
        public IEnumerable<object> Tags
        {
            get { return _tags; }
            set { SetProperty(ref _tags, value); }
        }
    }
}