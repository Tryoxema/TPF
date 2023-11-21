using System.Windows;

namespace TPF.Controls
{
    public class RangeChangedEventArgs<T> : RoutedEventArgs
    {
        public RangeChangedEventArgs(RoutedEvent routedEvent, T rangeStart, T rangeEnd) : base(routedEvent)
        {
            RoutedEvent = routedEvent;
            RangeStart = rangeStart;
            RangeEnd = rangeEnd;
        }

        public T RangeStart { get; }
        public T RangeEnd { get; }
    }

    public delegate void RangeChangedEventHandler<T>(object sender, RangeChangedEventArgs<T> e);
}