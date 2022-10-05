using System;
using System.Windows;

namespace TPF.Controls.Specialized.DialogHost
{
    public class DialogClosingEventArgs : RoutedEventArgs
    {
        public DialogClosingEventArgs(RoutedEvent routedEvent, DialogHandle handle) : base(routedEvent)
        {
            if (handle == null) throw new ArgumentNullException(nameof(handle));

            Handle = handle;
        }

        public DialogHandle Handle { get; }

        public bool Cancel { get; set; }

        public object DialogValue
        {
            get { return Handle.Value; }
        }
    }

    public delegate void DialogClosingEventHandler(object sender, DialogClosingEventArgs e);
}