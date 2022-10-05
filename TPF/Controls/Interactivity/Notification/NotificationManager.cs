using System;
using System.Linq;
using System.Windows;
using System.Windows.Media.Animation;
using System.Collections.Generic;

namespace TPF.Controls
{
    public class NotificationManager
    {
        private readonly List<Notification> _notifications = new List<Notification>();

        public int Count
        {
            get { return _notifications.Count; }
        }

        public event NotificationEventHandler MessageQueued;
        public event NotificationEventHandler MessageDismissed;

        private void RaiseMessageQueued(NotificationEventArgs e)
        {
            MessageQueued?.Invoke(this, e);
        }

        private void RaiseMessageDismissed(NotificationEventArgs e)
        {
            MessageDismissed?.Invoke(this, e);
        }

        public void Queue(Notification notification)
        {
            if (notification == null || _notifications.Contains(notification)) return;

            _notifications.Add(notification);

            var eventArgs = new NotificationEventArgs(notification);

            RaiseMessageQueued(eventArgs);
        }

        public void Dismiss(Notification notification)
        {
            if (notification == null || !_notifications.Contains(notification)) return;

            _notifications.Remove(notification);

            var eventArgs = new NotificationEventArgs(notification);

            if (notification.UseAnimation)
            {
                var property = notification.AnimationOutDependencyProperty ?? UIElement.OpacityProperty;

                var animation = notification.AnimationOut ?? new DoubleAnimation()
                {
                    From = 1,
                    To = 0,
                    BeginTime = TimeSpan.Zero,
                    FillBehavior = FillBehavior.HoldEnd
                };

                animation.Duration = TimeSpan.FromSeconds(notification.AnimationOutDuration);

                animation.Completed += (s, a) =>
                {
                    RaiseMessageDismissed(eventArgs);
                };

                notification.BeginAnimation(property, animation);
            }
            else RaiseMessageDismissed(eventArgs);
        }

        public Notification GetNotificationByHeader(string value)
        {
            return _notifications.FirstOrDefault(x => x.Header == value);
        }

        public Notification GetNotificationByMessage(string value)
        {
            return _notifications.FirstOrDefault(x => x.Message == value);
        }

        public Notification GetNotificationByBadge(string value)
        {
            return _notifications.FirstOrDefault(x => x.BadgeText == value);
        }

        public Notification GetNotificationByTag(object value)
        {
            return _notifications.FirstOrDefault(x => x.Tag == value);
        }

        public IEnumerable<Notification> GetNotificationsByHeader(string value)
        {
            return _notifications.Where(x => x.Header == value);
        }

        public IEnumerable<Notification> GetNotificationsByMessage(string value)
        {
            return _notifications.Where(x => x.Message == value);
        }

        public IEnumerable<Notification> GetNotificationsByBadge(string value)
        {
            return _notifications.Where(x => x.BadgeText == value);
        }

        public IEnumerable<Notification> GetNotificationsByTag(object value)
        {
            return _notifications.Where(x => x.Tag == value);
        }
    }
}