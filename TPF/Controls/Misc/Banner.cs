using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;
using TPF.Internal;

namespace TPF.Controls
{
    public class Banner : ContentControl
    {
        static Banner()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(Banner), new FrameworkPropertyMetadata(typeof(Banner)));
        }

        #region RunCompleted RoutedEvent
        public static readonly RoutedEvent RunCompletedEvent = EventManager.RegisterRoutedEvent("RunCompleted",
            RoutingStrategy.Bubble,
            typeof(RoutedEventHandler),
            typeof(Banner));

        public event RoutedEventHandler RunCompleted
        {
            add => AddHandler(RunCompletedEvent, value);
            remove => RemoveHandler(RunCompletedEvent, value);
        }
        #endregion

        #region CornerRadius DependencyProperty
        public static readonly DependencyProperty CornerRadiusProperty = DependencyProperty.Register("CornerRadius",
            typeof(CornerRadius),
            typeof(Banner),
            new PropertyMetadata(default(CornerRadius)));

        public CornerRadius CornerRadius
        {
            get { return (CornerRadius)GetValue(CornerRadiusProperty); }
            set { SetValue(CornerRadiusProperty, value); }
        }
        #endregion

        #region Direction DependencyProperty
        public static readonly DependencyProperty DirectionProperty = DependencyProperty.Register("Direction",
            typeof(BannerDirection),
            typeof(Banner),
            new PropertyMetadata(BannerDirection.LeftToRight, OnBannerAnimationPropertyChanged));

        public BannerDirection Direction
        {
            get { return (BannerDirection)GetValue(DirectionProperty); }
            set { SetValue(DirectionProperty, value); }
        }
        #endregion

        #region AutoReverse DependencyProperty
        public static readonly DependencyProperty AutoReverseProperty = DependencyProperty.Register("AutoReverse",
            typeof(bool),
            typeof(Banner),
            new PropertyMetadata(BooleanBoxes.FalseBox, OnBannerAnimationPropertyChanged));

        public bool AutoReverse
        {
            get { return (bool)GetValue(AutoReverseProperty); }
            set { SetValue(AutoReverseProperty, BooleanBoxes.Box(value)); }
        }
        #endregion

        #region Speed DependencyProperty
        public static readonly DependencyProperty SpeedProperty = DependencyProperty.Register("Speed",
            typeof(double),
            typeof(Banner),
            new PropertyMetadata(double.NaN, OnBannerAnimationPropertyChanged));

        public double Speed
        {
            get { return (double)GetValue(SpeedProperty); }
            set { SetValue(SpeedProperty, value); }
        }
        #endregion

        #region Duration DependencyProperty
        public static readonly DependencyProperty DurationProperty = DependencyProperty.Register("Duration",
            typeof(TimeSpan),
            typeof(Banner),
            new PropertyMetadata(TimeSpan.FromSeconds(5), OnBannerAnimationPropertyChanged));

        public TimeSpan Duration
        {
            get { return (TimeSpan)GetValue(DurationProperty); }
            set { SetValue(DurationProperty, value); }
        }
        #endregion

        #region RepeatDelay DependencyProperty
        public static readonly DependencyProperty RepeatDelayProperty = DependencyProperty.Register("RepeatDelay",
            typeof(TimeSpan),
            typeof(Banner),
            new PropertyMetadata(TimeSpan.FromSeconds(1), OnBannerAnimationPropertyChanged));

        public TimeSpan RepeatDelay
        {
            get { return (TimeSpan)GetValue(RepeatDelayProperty); }
            set { SetValue(RepeatDelayProperty, value); }
        }
        #endregion

        #region IsRunning DependencyProperty
        public static readonly DependencyProperty IsRunningProperty = DependencyProperty.Register("IsRunning",
            typeof(bool),
            typeof(Banner),
            new PropertyMetadata(BooleanBoxes.FalseBox, OnIsRunningChanged));

        private static void OnIsRunningChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            var instance = (Banner)sender;

            var isRunning = (bool)e.NewValue;

            instance.UpdateStoryboardState(isRunning);
        }

        public bool IsRunning
        {
            get { return (bool)GetValue(IsRunningProperty); }
            set { SetValue(IsRunningProperty, BooleanBoxes.Box(value)); }
        }
        #endregion

        private static void OnBannerAnimationPropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            var instance = (Banner)sender;

            instance.UpdateStoryboard();
        }

        private Storyboard _storyboard;
        private FrameworkElement _contentElement;

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            if (_contentElement != null)
            {
                _contentElement.SizeChanged -= ContentElement_SizeChanged;
            }

            _contentElement = GetTemplateChild("PART_Content") as FrameworkElement;

            if (_contentElement != null)
            {
                _contentElement.SizeChanged += ContentElement_SizeChanged;
            }

            UpdateStoryboard();
        }

        private void ContentElement_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            UpdateStoryboard();
        }

        protected override void OnRenderSizeChanged(SizeChangedInfo sizeInfo)
        {
            base.OnRenderSizeChanged(sizeInfo);

            UpdateStoryboard();
        }

        private void UpdateStoryboard()
        {
            if (_contentElement == null || _contentElement.ActualWidth.IsZero() || _contentElement.ActualHeight.IsZero()) return;

            if (_storyboard != null)
            {
                _storyboard.Completed -= Storyboard_Completed;
                _storyboard.Stop();
            }

            var width = ActualWidth;
            var height = ActualHeight;
            var contentWidth = _contentElement.ActualWidth;
            var contentHeight = _contentElement.ActualHeight;
            double from, to;
            PropertyPath propertyPath;

            switch (Direction)
            {
                case BannerDirection.LeftToRight:
                {
                    from = -contentWidth;
                    to = width;
                    propertyPath = new PropertyPath("(UIElement.RenderTransform).(TransformGroup.Children)[0].(TranslateTransform.X)");
                    break;
                }
                case BannerDirection.RightToLeft:
                {
                    from = width;
                    to = -contentWidth;
                    propertyPath = new PropertyPath("(UIElement.RenderTransform).(TransformGroup.Children)[0].(TranslateTransform.X)");
                    break;
                }
                case BannerDirection.TopToBottom:
                {
                    from = -contentHeight;
                    to = height;
                    propertyPath = new PropertyPath("(UIElement.RenderTransform).(TransformGroup.Children)[0].(TranslateTransform.Y)");
                    break;
                }
                case BannerDirection.BottomToTop:
                {
                    from = height;
                    to = -contentHeight;
                    propertyPath = new PropertyPath("(UIElement.RenderTransform).(TransformGroup.Children)[0].(TranslateTransform.Y)");
                    break;
                }
                default: return;
            }

            var animation = new DoubleAnimationUsingKeyFrames();

            // Startpunkt
            animation.KeyFrames.Add(new EasingDoubleKeyFrame(from, KeyTime.FromTimeSpan(TimeSpan.Zero)));

            // Soll eine Pause zwischen den einzelnen Durchgängen gemacht werden und ist AutoReverse = true
            if (RepeatDelay > TimeSpan.Zero && AutoReverse)
            {
                // Zweiten Punkt in die Animation einfügen, um eine Pause zu simulieren
                animation.KeyFrames.Add(new EasingDoubleKeyFrame(from, KeyTime.FromTimeSpan(RepeatDelay)));
            }

            var duration = Duration;

            // Ist eine gültige Geschwindigkeit angegeben?
            if (!double.IsNaN(Speed) && Speed > 0d && !Speed.IsZero())
            {
                duration = TimeSpan.FromSeconds(Math.Abs(to - from) / Speed);
            }

            // Wurde am Anfang eine Pause gemacht?
            if (RepeatDelay > TimeSpan.Zero && AutoReverse) duration = duration.Add(RepeatDelay);

            // Endpunkt einfügen
            animation.KeyFrames.Add(new EasingDoubleKeyFrame(to, KeyTime.FromTimeSpan(duration)));

            // Soll eine Pause vor dem nächsten Durchgang gemacht werden?
            if (RepeatDelay > TimeSpan.Zero)
            {
                var finalTime = duration.Add(RepeatDelay);
                // Einen letzten Punkt in die Animation einfügen, der die Pause am Ende simuliert
                animation.KeyFrames.Add(new EasingDoubleKeyFrame(to, KeyTime.FromTimeSpan(finalTime)));
            }

            Storyboard.SetTarget(animation, _contentElement);
            Storyboard.SetTargetProperty(animation, propertyPath);

            _storyboard = new Storyboard()
            {
                AutoReverse = AutoReverse
            };
            _storyboard.Children.Add(animation);
            _storyboard.Completed += Storyboard_Completed;
            _storyboard.Begin();

            UpdateStoryboardState(IsRunning);
        }

        private void UpdateStoryboardState(bool isRunning)
        {
            if (_storyboard == null) return;

            if (isRunning) _storyboard.Resume();
            else _storyboard.Pause();
        }

        private void Storyboard_Completed(object sender, EventArgs e)
        {
            var eventArgs = new RoutedEventArgs(RunCompletedEvent);
            RaiseEvent(eventArgs);

            if (IsRunning) _storyboard.Begin();
        }
    }
}