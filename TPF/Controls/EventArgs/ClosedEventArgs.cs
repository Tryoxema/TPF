using System.Windows;

namespace TPF.Controls
{
    public class ClosedEventArgs<T> : RoutedEventArgs
    {
        public ClosedEventArgs(RoutedEvent routedEvent, T item) : base(routedEvent)
        {
            Item = item;
        }

        public T Item { get; protected set; }
    }

    public delegate void ClosedEventHandler<T>(object sender, ClosedEventArgs<T> e);
}