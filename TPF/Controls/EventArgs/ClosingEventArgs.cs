using System.Windows;

namespace TPF.Controls
{
    public class ClosingEventArgs<T> : RoutedEventArgs
    {
        public ClosingEventArgs(RoutedEvent routedEvent, T item) : base(routedEvent)
        {
            Item = item;
        }

        public bool Cancel { get; set; }

        public T Item { get; protected set; }
    }

    public delegate void ClosingEventHandler<T>(object sender, ClosingEventArgs<T> e);
}