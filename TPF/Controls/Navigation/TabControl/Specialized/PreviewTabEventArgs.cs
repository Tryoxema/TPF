using System.Windows;

namespace TPF.Controls.Specialized.TabControl
{
    public class PreviewTabEventArgs : RoutedEventArgs
    {
        public PreviewTabEventArgs(RoutedEvent routedEvent, TabItem tabItem, object item) : base(routedEvent)
        {
            TabItem = tabItem;
            Item = item;
        }

        public TabItem TabItem { get; private set; }

        public object Item { get; private set; }

        public bool Cancel { get; set; }
    }

    public delegate void PreviewTabEventHandler(object sender, PreviewTabEventArgs e);
}