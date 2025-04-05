using System;
using System.Windows;
using TPF.Controls;

namespace TPF.Demo.Views
{
    public partial class NotificationDemoView : ViewBase
    {
        public NotificationDemoView()
        {
            InitializeComponent();

            Manager = new NotificationManager();
            DemoNotificationHost.Manager = Manager;
        }

        public readonly NotificationManager Manager;

        string _notificationHeader;
        public string NotificationHeader
        {
            get { return _notificationHeader; }
            set { SetProperty(ref _notificationHeader, value); }
        }

        string _message;
        public string Message
        {
            get { return _message; }
            set { SetProperty(ref _message, value); }
        }

        string _badgeText;
        public string BadgeText
        {
            get { return _badgeText; }
            set { SetProperty(ref _badgeText, value); }
        }

        string _dismissButtonText = "OK";
        public string DismissButtonText
        {
            get { return _dismissButtonText; }
            set { SetProperty(ref _dismissButtonText, value); }
        }

        int _dismissAfterSeconds = 5;
        public int DismissAfterSeconds
        {
            get { return _dismissAfterSeconds; }
            set { SetProperty(ref _dismissAfterSeconds, value); }
        }

        bool _dismissWithButton = true;
        public bool DismissWithButton
        {
            get { return _dismissWithButton; }
            set { SetProperty(ref _dismissWithButton, value); }
        }

        bool _dismissWithDelay;
        public bool DismissWithDelay
        {
            get { return _dismissWithDelay; }
            set { SetProperty(ref _dismissWithDelay, value); }
        }

        bool _useAnimation;
        public bool UseAnimation
        {
            get { return _useAnimation; }
            set { SetProperty(ref _useAnimation, value); }
        }

        private void ShowNotificationButton_Click(object sender, RoutedEventArgs e)
        {
            var notification = Manager
                .CreateNotification()
                .UseAnimation(UseAnimation);

            if (!string.IsNullOrWhiteSpace(NotificationHeader)) notification.Header(NotificationHeader);
            if (!string.IsNullOrWhiteSpace(Message)) notification.Header(Message);
            if (!string.IsNullOrWhiteSpace(BadgeText)) notification.Badge(BadgeText);

            if (DismissWithButton) notification.Dismiss().WithButton(DismissButtonText);
            if (DismissWithDelay) notification.Dismiss().WithDelay(TimeSpan.FromSeconds(DismissAfterSeconds));

            notification.Queue();
        }

        private void ClearButton_Click(object sender, RoutedEventArgs e)
        {
            Manager.Clear();
        }
    }
}