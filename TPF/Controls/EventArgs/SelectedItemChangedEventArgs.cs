using System;
using System.Windows;

namespace TPF.Controls
{
    public class SelectedItemChangedEventArgs : RoutedEventArgs
    {
        public SelectedItemChangedEventArgs(RoutedEvent routedEvent, object oldItem, object newItem) : base(routedEvent)
        {
            RoutedEvent = routedEvent;
            OldItem = oldItem;
            NewItem = newItem;
        }
        
        public object OldItem { get; }

        public object NewItem { get; }
    }

    public delegate void SelectedItemChangedEventHandler(object sender, SelectedItemChangedEventArgs e);
}