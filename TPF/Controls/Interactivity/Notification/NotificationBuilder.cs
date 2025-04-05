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

        public virtual Notification CreateNotification()
        {
            return new Notification();
        }

        public virtual ButtonBase CreateButton()
        {
            return new System.Windows.Controls.Button();
        }

        public NotificationBuilder Tag(object value)
        {
            Notification.Tag = value;

            return this;
        }

        public NotificationBuilder Header(string header)
        {
            Notification.Header = header;

            return this;
        }

        public NotificationBuilder Message(string message)
        {
            Notification.Message = message;

            return this;
        }

        public NotificationBuilder Badge(string text)
        {
            Notification.BadgeText = text;

            return this;
        }

        public NotificationBuilder AddButton(ButtonBase button)
        {
            if (button == null) return this;

            Notification.Buttons.Add(button);

            return this;
        }

        public NotificationBuilder Foreground(string brushString)
        {
            var brush = new BrushConverter().ConvertFrom(brushString) as Brush;

            Notification.Foreground = brush;

            return this;
        }

        public NotificationBuilder Foreground(Brush brush)
        {
            Notification.Foreground = brush;

            return this;
        }

        public NotificationBuilder Foreground(BindingBase binding)
        {
            Notification.SetBinding(Control.ForegroundProperty, binding);

            return this;
        }

        public NotificationBuilder Background(string brushString)
        {
            var brush = new BrushConverter().ConvertFrom(brushString) as Brush;

            Notification.Background = brush;

            return this;
        }

        public NotificationBuilder Background(Brush brush)
        {
            Notification.Background = brush;

            return this;
        }

        public NotificationBuilder Background(BindingBase binding)
        {
            Notification.SetBinding(Control.BackgroundProperty, binding);

            return this;
        }

        public NotificationBuilder Accent(string brushString)
        {
            var brush = new BrushConverter().ConvertFrom(brushString) as Brush;

            Notification.AccentBrush = brush;

            return this;
        }

        public NotificationBuilder Accent(Brush brush)
        {
            Notification.AccentBrush = brush;

            return this;
        }

        public NotificationBuilder Accent(BindingBase binding)
        {
            Notification.SetBinding(Notification.AccentBrushProperty, binding);

            return this;
        }

        public NotificationBuilder ButtonForeground(string brushString)
        {
            var brush = new BrushConverter().ConvertFrom(brushString) as Brush;

            Notification.ButtonForeground = brush;

            return this;
        }

        public NotificationBuilder ButtonForeground(Brush brush)
        {
            Notification.ButtonForeground = brush;

            return this;
        }

        public NotificationBuilder ButtonForeground(BindingBase binding)
        {
            Notification.SetBinding(Notification.ButtonForegroundProperty, binding);

            return this;
        }

        public NotificationBuilder ButtonBackground(string brushString)
        {
            var brush = new BrushConverter().ConvertFrom(brushString) as Brush;

            Notification.ButtonBackground = brush;

            return this;
        }

        public NotificationBuilder ButtonBackground(Brush brush)
        {
            Notification.ButtonBackground = brush;

            return this;
        }

        public NotificationBuilder ButtonBackground(BindingBase binding)
        {
            Notification.SetBinding(Notification.ButtonBackgroundProperty, binding);

            return this;
        }

        public NotificationBuilder BadgeForeground(string brushString)
        {
            var brush = new BrushConverter().ConvertFrom(brushString) as Brush;

            Notification.BadgeForeground = brush;

            return this;
        }

        public NotificationBuilder BadgeForeground(Brush brush)
        {
            Notification.BadgeForeground = brush;

            return this;
        }

        public NotificationBuilder BadgeForeground(BindingBase binding)
        {
            Notification.SetBinding(Notification.BadgeForegroundProperty, binding);

            return this;
        }

        public NotificationBuilder BadgeBackground(string brushString)
        {
            var brush = new BrushConverter().ConvertFrom(brushString) as Brush;

            Notification.BadgeBackground = brush;

            return this;
        }

        public NotificationBuilder BadgeBackground(Brush brush)
        {
            Notification.BadgeBackground = brush;

            return this;
        }

        public NotificationBuilder BadgeBackground(BindingBase binding)
        {
            Notification.SetBinding(Notification.BadgeBackgroundProperty, binding);

            return this;
        }

        public NotificationBuilder Overlay(object overlay)
        {
            Notification.OverlayContent = overlay;

            return this;
        }

        public NotificationBuilder AdditionalContentTop(object content)
        {
            Notification.AdditionalContentTop = content;

            return this;
        }

        public NotificationBuilder AdditionalContentBottom(object content)
        {
            Notification.AdditionalContentBottom = content;

            return this;
        }

        public NotificationBuilder AdditionalContentLeft(object content)
        {
            Notification.AdditionalContentLeft = content;

            return this;
        }

        public NotificationBuilder AdditionalContentRight(object content)
        {
            Notification.AdditionalContentRight = content;

            return this;
        }

        public NotificationBuilder UseAnimation(bool value)
        {
            Notification.UseAnimation = value;

            return this;
        }

        public NotificationBuilder AnimationIn(AnimationTimeline animation)
        {
            Notification.AnimationIn = animation;

            return this;
        }

        public NotificationBuilder AnimationOut(AnimationTimeline animation)
        {
            Notification.AnimationOut = animation;

            return this;
        }

        public NotificationBuilder AnimationInDuration(double seconds)
        {
            Notification.AnimationInDuration = seconds;

            return this;
        }

        public NotificationBuilder AnimationOutDuration(double seconds)
        {
            Notification.AnimationOutDuration = seconds;

            return this;
        }

        public NotificationBuilder AnimationInDependencyProperty(DependencyProperty property)
        {
            Notification.AnimationInDependencyProperty = property;

            return this;
        }

        public NotificationBuilder AnimationOutDependencyProperty(DependencyProperty property)
        {
            Notification.AnimationOutDependencyProperty = property;

            return this;
        }

        public NotificationBuilder Delay(int milliseconds, Action action)
        {
            Delay(TimeSpan.FromMilliseconds(milliseconds), action);

            return this;
        }

        public NotificationBuilder Delay(int milliseconds, Action<Notification> action)
        {
            Delay(TimeSpan.FromMilliseconds(milliseconds), action);

            return this;
        }

        public NotificationBuilder Delay(TimeSpan delay, Action action)
        {
            Task.Delay(delay).ContinueWith(r => action(), TaskScheduler.FromCurrentSynchronizationContext());

            return this;
        }

        public NotificationBuilder Delay(TimeSpan delay, Action<Notification> action)
        {
            Task.Delay(delay).ContinueWith(r => action(Notification), TaskScheduler.FromCurrentSynchronizationContext());

            return this;
        }

        public NotificationBuilder WithButton(object content)
        {
            return WithButton(content, o => { });
        }

        public NotificationBuilder WithButton(object content, Action callback)
        {
            return WithButton(content, o => callback());
        }

        private NotificationBuilder WithButton(object content, Action<object> callback)
        {
            var button = CreateButton();

            button.Content = content;
            if (callback != null) button.Command = new ActionCommand(callback);
            AddButton(button);

            return this;
        }

        public NotificationBuilder WithButton(NotificationButtonConfiguration configuration)
        {
            return WithButton(configuration, o => { });
        }

        public NotificationBuilder WithButton(NotificationButtonConfiguration configuration, Action callback)
        {
            return WithButton(configuration, o => callback());
        }

        private NotificationBuilder WithButton(NotificationButtonConfiguration configuration, Action<object> callback)
        {
            var button = CreateButton();

            configuration.Apply(button);
            if (callback != null) button.Command = new ActionCommand(callback);
            AddButton(button);

            return this;
        }

        public NotificationDismissBuilder Dismiss()
        {
            return new NotificationDismissBuilder(this);
        }

        public Notification Queue()
        {
            Manager.Queue(Notification);

            return Notification;
        }

        internal Action DismissBefore(Action callback)
        {
            return () =>
            {
                Manager.Dismiss(Notification);
                callback?.Invoke();
            };
        }

        internal Action DismissBefore(Action<Notification> callback)
        {
            return () =>
            {
                Manager.Dismiss(Notification);
                callback?.Invoke(Notification);
            };
        }
    }

    public class NotificationDismissBuilder
    {
        public NotificationDismissBuilder(NotificationBuilder builder)
        {
            if (builder == null) throw new ArgumentNullException(nameof(builder));

            Builder = builder;
        }

        public NotificationBuilder Builder { get; }

        public NotificationBuilder WithDelay(int milliseconds)
        {
            return WithDelay(milliseconds, () => { });
        }

        public NotificationBuilder WithDelay(int milliseconds, Action callback)
        {
            Builder.Delay(milliseconds, Builder.DismissBefore(callback));

            return Builder;
        }

        public NotificationBuilder WithDelay(int milliseconds, Action<Notification> callback)
        {
            Builder.Delay(milliseconds, Builder.DismissBefore(callback));

            return Builder;
        }

        public NotificationBuilder WithDelay(TimeSpan delay)
        {
            return WithDelay(delay, () => { });
        }

        public NotificationBuilder WithDelay(TimeSpan delay, Action callback)
        {
            Builder.Delay(delay, Builder.DismissBefore(callback));

            return Builder;
        }

        public NotificationBuilder WithDelay(TimeSpan delay, Action<Notification> callback)
        {
            Builder.Delay(delay, Builder.DismissBefore(callback));

            return Builder;
        }

        public NotificationBuilder WithButton(object content)
        {
            return WithButton(content, () => { });
        }

        public NotificationBuilder WithButton(object content, Action callback)
        {
            return Builder.WithButton(content, Builder.DismissBefore(callback));
        }

        public NotificationBuilder WithButton(NotificationButtonConfiguration configuration)
        {
            return WithButton(configuration, () => { });
        }

        public NotificationBuilder WithButton(NotificationButtonConfiguration configuration, Action callback)
        {
            return Builder.WithButton(configuration, Builder.DismissBefore(callback));
        }
    }
}