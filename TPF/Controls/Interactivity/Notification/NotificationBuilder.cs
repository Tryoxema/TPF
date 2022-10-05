using System;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Controls;
using System.Windows.Media.Animation;
using System.Threading.Tasks;
using System.Windows.Controls.Primitives;

namespace TPF.Controls
{
    public class NotificationBuilder
    {
        public NotificationBuilder(NotificationManager manager)
        {
            if (manager == null) throw new ArgumentNullException(nameof(manager));

            Manager = manager;
            Notification = CreateNotification();
        }

        public Notification Notification { get; }

        public NotificationManager Manager { get; }

        public static NotificationBuilder CreateNotification(NotificationManager manager)
        {
            return new NotificationBuilder(manager);
        }

        public virtual Notification CreateNotification()
        {
            return new Notification();
        }

        public virtual ButtonBase CreateButton()
        {
            return new System.Windows.Controls.Button();
        }

        public void SetTag(object value)
        {
            Notification.Tag = value;
        }

        public void SetHeader(string header)
        {
            Notification.Header = header;
        }

        public void SetMessage(string message)
        {
            Notification.Message = message;
        }

        public void SetBadge(string text)
        {
            Notification.BadgeText = text;
        }

        public void AddButton(ButtonBase button)
        {
            if (button == null) return;

            Notification.Buttons.Add(button);
        }

        public void SetForeground(Brush brush)
        {
            Notification.Foreground = brush;
        }

        public void SetForeground(BindingBase binding)
        {
            Notification.SetBinding(Control.ForegroundProperty, binding);
        }

        public void SetBackground(Brush brush)
        {
            Notification.Background = brush;
        }

        public void SetBackground(BindingBase binding)
        {
            Notification.SetBinding(Control.BackgroundProperty, binding);
        }

        public void SetAccent(Brush brush)
        {
            Notification.AccentBrush = brush;
        }

        public void SetAccent(BindingBase binding)
        {
            Notification.SetBinding(Notification.AccentBrushProperty, binding);
        }

        public void SetButtonForeground(Brush brush)
        {
            Notification.ButtonForeground = brush;
        }

        public void SetButtonForeground(BindingBase binding)
        {
            Notification.SetBinding(Notification.ButtonForegroundProperty, binding);
        }

        public void SetButtonBackground(Brush brush)
        {
            Notification.ButtonBackground = brush;
        }

        public void SetButtonBackground(BindingBase binding)
        {
            Notification.SetBinding(Notification.ButtonBackgroundProperty, binding);
        }

        public void SetBadgeForeground(Brush brush)
        {
            Notification.BadgeForeground = brush;
        }

        public void SetBadgeForeground(BindingBase binding)
        {
            Notification.SetBinding(Notification.BadgeForegroundProperty, binding);
        }

        public void SetBadgeBackground(Brush brush)
        {
            Notification.BadgeBackground = brush;
        }

        public void SetBadgeBackground(BindingBase binding)
        {
            Notification.SetBinding(Notification.BadgeBackgroundProperty, binding);
        }

        public void SetOverlay(object overlay)
        {
            Notification.OverlayContent = overlay;
        }

        public void SetAdditionalContentTop(object content)
        {
            Notification.AdditionalContentTop = content;
        }

        public void SetAdditionalContentBottom(object content)
        {
            Notification.AdditionalContentBottom = content;
        }

        public void SetAdditionalContentLeft(object content)
        {
            Notification.AdditionalContentLeft = content;
        }

        public void SetAdditionalContentRight(object content)
        {
            Notification.AdditionalContentRight = content;
        }

        public void SetUseAnimation(bool value)
        {
            Notification.UseAnimation = value;
        }

        public void SetAnimationIn(AnimationTimeline animation)
        {
            Notification.AnimationIn = animation;
        }

        public void SetAnimationOut(AnimationTimeline animation)
        {
            Notification.AnimationOut = animation;
        }

        public void SetAnimationInDuration(double seconds)
        {
            Notification.AnimationInDuration = seconds;
        }

        public void SetAnimationOutDuration(double seconds)
        {
            Notification.AnimationOutDuration = seconds;
        }

        public void SetAnimationInDependencyProperty(DependencyProperty property)
        {
            Notification.AnimationInDependencyProperty = property;
        }

        public void SetAnimationOutDependencyProperty(DependencyProperty property)
        {
            Notification.AnimationOutDependencyProperty = property;
        }

        public void Delay(int milliseconds, Action action)
        {
            Delay(TimeSpan.FromMilliseconds(milliseconds), action);
        }

        public void Delay(int milliseconds, Action<Notification> action)
        {
            Delay(TimeSpan.FromMilliseconds(milliseconds), action);
        }

        public void Delay(TimeSpan delay, Action action)
        {
            Task.Delay(delay).ContinueWith(r => action(), TaskScheduler.FromCurrentSynchronizationContext());
        }

        public void Delay(TimeSpan delay, Action<Notification> action)
        {
            Task.Delay(delay).ContinueWith(r => action(Notification), TaskScheduler.FromCurrentSynchronizationContext());
        }

        public Notification Queue()
        {
            Manager.Queue(Notification);

            return Notification;
        }

        public class DismissNotification
        {
            public DismissNotification(NotificationBuilder builder)
            {
                if (builder == null) throw new ArgumentNullException(nameof(builder));

                Builder = builder;
            }

            public NotificationBuilder Builder { get; }
        }
    }
}