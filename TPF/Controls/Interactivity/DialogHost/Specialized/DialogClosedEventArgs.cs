using System.Windows;

namespace TPF.Controls.Specialized.DialogHost
{
    public class DialogClosedEventArgs : RoutedEventArgs
    {
        public DialogClosedEventArgs(RoutedEvent routedEvent, object value, object content) : base(routedEvent)
        {
            DialogValue = value;
            DialogContent = content;
        }

        public object DialogValue { get; }

        public object DialogContent { get; }
    }

    public delegate void DialogClosedEventHandler(object sender, DialogClosedEventArgs e);
}