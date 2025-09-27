using System.Windows;

namespace TPF.Controls
{
    public class CancelableRoutedEventArgs : RoutedEventArgs
    {
        public CancelableRoutedEventArgs(RoutedEvent routedEvent) : base(routedEvent) { }

        public bool Cancel { get; set; }
    }

    public delegate void CancelableRoutedEventHandler(object sender, CancelableRoutedEventArgs e);
}