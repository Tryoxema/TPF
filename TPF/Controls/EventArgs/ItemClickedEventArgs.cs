using System.Windows;

namespace TPF.Controls
{
    public class ItemClickedEventArgs : RoutedEventArgs
    {
        public ItemClickedEventArgs(RoutedEvent routedEvent, object clickedItem) : base(routedEvent)
        {
            ClickedItem = clickedItem;
        }

        public object ClickedItem { get; }
    }

    public delegate void ItemClickedEventHandler(object sender, ItemClickedEventArgs e);
}