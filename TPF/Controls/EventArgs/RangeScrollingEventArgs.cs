using System.Windows;
using TPF.Data;

namespace TPF.Controls
{
    public class RangeScrollingEventArgs : RoutedEventArgs
    {
        public RangeScrollingEventArgs (RoutedEvent routedEvent, DoubleRange newRange) : base(routedEvent)
        {
            RoutedEvent = routedEvent;
            NewRange = newRange;
        }

        public DoubleRange NewRange { get; }
    }

    public delegate void RangeScrollingEventHandler(object sender, RangeScrollingEventArgs e); 
}