using System.Windows;

namespace TPF.Controls.Specialized.TabControl
{
    public class TabEventArgs : RoutedEventArgs
    {
        public TabEventArgs(RoutedEvent routedEvent, TabItem tabItem, object item) : base(routedEvent)
        {
            TabItem = tabItem;
            Item = item;
        }

        public TabItem TabItem { get; private set; }

        public object Item { get; private set; }
    }

    public delegate void TabEventHandler(object sender, PreviewTabEventArgs e);
}