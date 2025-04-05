using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;

namespace TPF.Controls
{
    public class NotificationsHost : ItemsControl
    {
        static NotificationsHost()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(NotificationsHost), new FrameworkPropertyMetadata(typeof(NotificationsHost)));
        }

        #region Manager DependencyProperty
        public static readonly DependencyProperty ManagerProperty = DependencyProperty.Register("Manager",
            typeof(NotificationManager),
            typeof(NotificationsHost),
            new PropertyMetadata(null, OnManagerChanged));

        private static void OnManagerChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            var instance = (NotificationsHost)sender;

            if (e.OldValue is NotificationManager oldManager) instance.RemoveEvents(oldManager);

            if (e.NewValue is NotificationManager newManager) instance.AttachEvents(newManager);
        }

        public NotificationManager Manager
        {
            get { return (NotificationManager)GetValue(ManagerProperty); }
            set { SetValue(ManagerProperty, value); }
        }
        #endregion

        private void AttachEvents(NotificationManager manager)
        {
            manager.MessageQueued += OnMessageQueued;
            manager.MessageDismissed += OnMessageDismissed;
        }

        private void RemoveEvents(NotificationManager manager)
        {
            manager.MessageQueued -= OnMessageQueued;
            manager.MessageDismissed -= OnMessageDismissed;
        }

        private void OnMessageQueued(object sender, NotificationEventArgs e)
        {
            if (ItemsSource != null) throw new NotSupportedException("ItemsSource and NotificationManager are not supported at the same time.");

            Items.Add(e.Notification);

            if (e.Notification.UseAnimation)
            {
                var property = e.Notification.AnimationInDependencyProperty ?? OpacityProperty;

                var animation = e.Notification.AnimationIn ?? new DoubleAnimation()
                {
                    From = 0,
                    To = 1,
                    BeginTime = TimeSpan.Zero,
                    FillBehavior = FillBehavior.HoldEnd
                };

                animation.Duration = TimeSpan.FromSeconds(e.Notification.AnimationInDuration);

                e.Notification.BeginAnimation(property, animation);
            }
        }

        private void OnMessageDismissed(object sender, NotificationEventArgs e)
        {
            if (ItemsSource != null) throw new NotSupportedException("ItemsSource and NotificationManager are not supported at the same time.");

            Items.Remove(e.Notification);
        }

        protected override bool IsItemItsOwnContainerOverride(object item)
        {
            return item is Notification;
        }

        protected override DependencyObject GetContainerForItemOverride()
        {
            return new Notification();
        }
    }
}