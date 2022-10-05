using System;
using System.Windows;

namespace TPF.Controls.Specialized.DialogHost
{
    public class DialogOpenedEventArgs : RoutedEventArgs
    {
        public DialogOpenedEventArgs(RoutedEvent routedEvent, DialogHandle handle) : base(routedEvent)
        {
            if (handle == null) throw new ArgumentNullException(nameof(handle));

            Handle = handle;
        }

        public DialogHandle Handle { get; }
    }

    public delegate void DialogOpenedEventHandler(object sender, DialogOpenedEventArgs e);
}