using System;

namespace TPF.Controls
{
    public class NotificationEventArgs : EventArgs
    {
        public NotificationEventArgs(Notification notification)
        {
            Notification = notification;
        }

        public Notification Notification { get; private set; }
    }

    public delegate void NotificationEventHandler(object sender, NotificationEventArgs e);
}