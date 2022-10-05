using System;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace TPF.Controls
{
    public static class NotificationBuilderExtensions
    {
        public static NotificationBuilder CreateNotification(this NotificationManager manager)
        {
            return new NotificationBuilder(manager);
        }

        public static NotificationBuilder Tag(this NotificationBuilder builder, object value)
        {
            builder.SetTag(value);

            return builder;
        }

        public static NotificationBuilder Header(this NotificationBuilder builder, string header)
        {
            builder.SetHeader(header);

            return builder;
        }

        public static NotificationBuilder Message(this NotificationBuilder builder, string message)
        {
            builder.SetMessage(message);

            return builder;
        }

        public static NotificationBuilder Badge(this NotificationBuilder builder, string text)
        {
            builder.SetBadge(text);

            return builder;
        }

        public static NotificationBuilder Foreground(this NotificationBuilder builder, string brushString)
        {
            var brush = new BrushConverter().ConvertFrom(brushString) as Brush;

            builder.SetForeground(brush);

            return builder;
        }

        public static NotificationBuilder Foreground(this NotificationBuilder builder, Brush brush)
        {
            builder.SetForeground(brush);

            return builder;
        }

        public static NotificationBuilder Foreground(this NotificationBuilder builder, BindingBase binding)
        {
            builder.SetForeground(binding);

            return builder;
        }

        public static NotificationBuilder Background(this NotificationBuilder builder, string brushString)
        {
            var brush = new BrushConverter().ConvertFrom(brushString) as Brush;

            builder.SetBackground(brush);

            return builder;
        }

        public static NotificationBuilder Background(this NotificationBuilder builder, Brush brush)
        {
            builder.SetBackground(brush);

            return builder;
        }

        public static NotificationBuilder Background(this NotificationBuilder builder, BindingBase binding)
        {
            builder.SetBackground(binding);

            return builder;
        }

        public static NotificationBuilder Accent(this NotificationBuilder builder, string brushString)
        {
            var brush = new BrushConverter().ConvertFrom(brushString) as Brush;

            builder.SetAccent(brush);

            return builder;
        }

        public static NotificationBuilder Accent(this NotificationBuilder builder, Brush brush)
        {
            builder.SetAccent(brush);

            return builder;
        }

        public static NotificationBuilder Accent(this NotificationBuilder builder, BindingBase binding)
        {
            builder.SetAccent(binding);

            return builder;
        }

        public static NotificationBuilder ButtonForeground(this NotificationBuilder builder, string brushString)
        {
            var brush = new BrushConverter().ConvertFrom(brushString) as Brush;

            builder.SetButtonForeground(brush);

            return builder;
        }

        public static NotificationBuilder ButtonForeground(this NotificationBuilder builder, Brush brush)
        {
            builder.SetButtonForeground(brush);

            return builder;
        }

        public static NotificationBuilder ButtonForeground(this NotificationBuilder builder, BindingBase binding)
        {
            builder.SetButtonForeground(binding);

            return builder;
        }

        public static NotificationBuilder ButtonBackground(this NotificationBuilder builder, string brushString)
        {
            var brush = new BrushConverter().ConvertFrom(brushString) as Brush;

            builder.SetButtonBackground(brush);

            return builder;
        }

        public static NotificationBuilder ButtonBackground(this NotificationBuilder builder, Brush brush)
        {
            builder.SetButtonBackground(brush);

            return builder;
        }

        public static NotificationBuilder ButtonBackground(this NotificationBuilder builder, BindingBase binding)
        {
            builder.SetButtonBackground(binding);

            return builder;
        }

        public static NotificationBuilder BadgeForeground(this NotificationBuilder builder, string brushString)
        {
            var brush = new BrushConverter().ConvertFrom(brushString) as Brush;

            builder.SetBadgeForeground(brush);

            return builder;
        }

        public static NotificationBuilder BadgeForeground(this NotificationBuilder builder, Brush brush)
        {
            builder.SetBadgeForeground(brush);

            return builder;
        }

        public static NotificationBuilder BadgeForeground(this NotificationBuilder builder, BindingBase binding)
        {
            builder.SetBadgeForeground(binding);

            return builder;
        }

        public static NotificationBuilder BadgeBackground(this NotificationBuilder builder, string brushString)
        {
            var brush = new BrushConverter().ConvertFrom(brushString) as Brush;

            builder.SetBadgeBackground(brush);

            return builder;
        }

        public static NotificationBuilder BadgeBackground(this NotificationBuilder builder, Brush brush)
        {
            builder.SetBadgeBackground(brush);

            return builder;
        }

        public static NotificationBuilder BadgeBackground(this NotificationBuilder builder, BindingBase binding)
        {
            builder.SetBadgeBackground(binding);

            return builder;
        }

        public static NotificationBuilder Overlay(this NotificationBuilder builder, object overlay)
        {
            builder.SetOverlay(overlay);

            return builder;
        }

        public static NotificationBuilder AdditionalContentTop(this NotificationBuilder builder, object content)
        {
            builder.SetAdditionalContentTop(content);

            return builder;
        }

        public static NotificationBuilder AdditionalContentBottom(this NotificationBuilder builder, object content)
        {
            builder.SetAdditionalContentBottom(content);

            return builder;
        }

        public static NotificationBuilder AdditionalContentLeft(this NotificationBuilder builder, object content)
        {
            builder.SetAdditionalContentLeft(content);

            return builder;
        }

        public static NotificationBuilder AdditionalContentRight(this NotificationBuilder builder, object content)
        {
            builder.SetAdditionalContentRight(content);

            return builder;
        }

        public static NotificationBuilder UseAnimation(this NotificationBuilder builder, bool value)
        {
            builder.SetUseAnimation(value);

            return builder;
        }

        public static NotificationBuilder AnimationIn(this NotificationBuilder builder, AnimationTimeline animation)
        {
            builder.SetAnimationIn(animation);

            return builder;
        }

        public static NotificationBuilder AnimationOut(this NotificationBuilder builder, AnimationTimeline animation)
        {
            builder.SetAnimationOut(animation);

            return builder;
        }

        public static NotificationBuilder AnimationInDuration(this NotificationBuilder builder, double seconds)
        {
            builder.SetAnimationInDuration(seconds);

            return builder;
        }

        public static NotificationBuilder AnimationOutDuration(this NotificationBuilder builder, double seconds)
        {
            builder.SetAnimationOutDuration(seconds);

            return builder;
        }

        public static NotificationBuilder AnimationInDependencyProperty(this NotificationBuilder builder, DependencyProperty property)
        {
            builder.SetAnimationInDependencyProperty(property);

            return builder;
        }

        public static NotificationBuilder AnimationOutDependencyProperty(this NotificationBuilder builder, DependencyProperty property)
        {
            builder.SetAnimationOutDependencyProperty(property);

            return builder;
        }

        public static NotificationBuilder.DismissNotification Dismiss(this NotificationBuilder builder)
        {
            return new NotificationBuilder.DismissNotification(builder);
        }

        private static Action DismissBefore(this NotificationBuilder builder, Action callback)
        {
            return () =>
            {
                builder.Manager.Dismiss(builder.Notification);
                callback?.Invoke();
            };
        }

        private static Action DismissBefore(this NotificationBuilder builder, Action<Notification> callback)
        {
            return () =>
            {
                builder.Manager.Dismiss(builder.Notification);
                callback?.Invoke(builder.Notification);
            };
        }

        public static NotificationBuilder WithDelay(this NotificationBuilder.DismissNotification dismiss, int milliseconds)
        {
            return dismiss.WithDelay(milliseconds, () => { });
        }

        public static NotificationBuilder WithDelay(this NotificationBuilder.DismissNotification dismiss, int milliseconds, Action callback)
        {
            dismiss.Builder.Delay(milliseconds, dismiss.Builder.DismissBefore(callback));

            return dismiss.Builder;
        }

        public static NotificationBuilder WithDelay(this NotificationBuilder.DismissNotification dismiss, int milliseconds, Action<Notification> callback)
        {
            dismiss.Builder.Delay(milliseconds, dismiss.Builder.DismissBefore(callback));

            return dismiss.Builder;
        }

        public static NotificationBuilder WithDelay(this NotificationBuilder.DismissNotification dismiss, TimeSpan delay)
        {
            return dismiss.WithDelay(delay, () => { });
        }

        public static NotificationBuilder WithDelay(this NotificationBuilder.DismissNotification dismiss, TimeSpan delay, Action callback)
        {
            dismiss.Builder.Delay(delay, dismiss.Builder.DismissBefore(callback));

            return dismiss.Builder;
        }

        public static NotificationBuilder WithDelay(this NotificationBuilder.DismissNotification dismiss, TimeSpan delay, Action<Notification> callback)
        {
            dismiss.Builder.Delay(delay, dismiss.Builder.DismissBefore(callback));

            return dismiss.Builder;
        }

        public static NotificationBuilder WithButton(this NotificationBuilder builder, object content)
        {
            return builder.WithButton(content, o => { });
        }

        public static NotificationBuilder WithButton(this NotificationBuilder builder, object content, Action callback)
        {
            return builder.WithButton(content, o => callback());
        }

        private static NotificationBuilder WithButton(this NotificationBuilder builder, object content, Action<object> callback)
        {
            var button = builder.CreateButton();

            button.Content = content;
            if (callback != null) button.Command = new ActionCommand(callback);
            builder.AddButton(button);

            return builder;
        }

        public static NotificationBuilder WithButton(this NotificationBuilder builder, NotificationButtonConfiguration configuration)
        {
            return builder.WithButton(configuration, o => { });
        }

        public static NotificationBuilder WithButton(this NotificationBuilder builder, NotificationButtonConfiguration configuration, Action callback)
        {
            return builder.WithButton(configuration, o => callback());
        }

        private static NotificationBuilder WithButton(this NotificationBuilder builder, NotificationButtonConfiguration configuration, Action<object> callback)
        {
            var button = builder.CreateButton();

            configuration.Apply(button);
            if (callback != null) button.Command = new ActionCommand(callback);
            builder.AddButton(button);

            return builder;
        }

        public static NotificationBuilder WithButton(this NotificationBuilder.DismissNotification dismiss, object content)
        {
            return dismiss.WithButton(content, () => { });
        }

        public static NotificationBuilder WithButton(this NotificationBuilder.DismissNotification dismiss, object content, Action callback)
        {
            return dismiss.Builder.WithButton(content, dismiss.Builder.DismissBefore(callback));
        }

        public static NotificationBuilder WithButton(this NotificationBuilder.DismissNotification dismiss, NotificationButtonConfiguration configuration)
        {
            return dismiss.WithButton(configuration, () => { });
        }

        public static NotificationBuilder WithButton(this NotificationBuilder.DismissNotification dismiss, NotificationButtonConfiguration configuration, Action callback)
        {
            return dismiss.Builder.WithButton(configuration, dismiss.Builder.DismissBefore(callback));
        }
    }
}