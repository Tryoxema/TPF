using System.Windows;

namespace TPF.Controls.Specialized.TaskBoard
{
    public class CardClickedEventArgs : RoutedEventArgs
    {
        internal CardClickedEventArgs(RoutedEvent routedEvent, TaskBoardItem item) : base(routedEvent)
        {
            Column = item.Column;
            Container = item;
            Item = item.Content;
        }

        public TaskBoardColumn Column { get; }

        public TaskBoardItem Container { get; }

        public object Item { get; }
    }

    public delegate void CardClickedEventHandler(object sender, CardClickedEventArgs e);
}