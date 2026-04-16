using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Threading;
using TPF.Internal;

namespace TPF.Controls
{
    [TemplatePart(Name = PART_Root, Type = typeof(Border))]
    [TemplatePart(Name = PART_Message, Type = typeof(TextBlock))]
    [TemplatePart(Name = PART_ActionButton, Type = typeof(ButtonBase))]
    public class Snackbar : Control
    {
        static Snackbar()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(Snackbar), new FrameworkPropertyMetadata(typeof(Snackbar)));
        }

        public Snackbar()
        {
            _displayTimer.Tick += OnDisplayTimerTick;
            MessageQueue = new SnackbarMessageQueue();
        }

        #region ActionClicked RoutedEvent
        public static readonly RoutedEvent ActionClickedEvent = EventManager.RegisterRoutedEvent(nameof(ActionClicked),
            RoutingStrategy.Bubble,
            typeof(RoutedEventHandler),
            typeof(Snackbar));

        public event RoutedEventHandler ActionClicked
        {
            add { AddHandler(ActionClickedEvent, value); }
            remove { RemoveHandler(ActionClickedEvent, value); }
        }
        #endregion

        #region MessageQueue DependencyProperty
        public static readonly DependencyProperty MessageQueueProperty = DependencyProperty.Register(nameof(MessageQueue),
            typeof(SnackbarMessageQueue),
            typeof(Snackbar),
            new PropertyMetadata(null, MessageQueuePropertyChanged));

        private static void MessageQueuePropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            var instance = (Snackbar)sender;
            instance.OnMessageQueueChanged(e.OldValue as SnackbarMessageQueue, e.NewValue as SnackbarMessageQueue);
        }

        public SnackbarMessageQueue MessageQueue
        {
            get { return (SnackbarMessageQueue)GetValue(MessageQueueProperty); }
            set { SetValue(MessageQueueProperty, value); }
        }
        #endregion

        #region CurrentText Readonly DependencyProperty
        public static readonly DependencyPropertyKey CurrentTextPropertyKey = DependencyProperty.RegisterReadOnly(nameof(CurrentText),
            typeof(string),
            typeof(Snackbar),
            new PropertyMetadata(string.Empty));

        public static readonly DependencyProperty CurrentTextProperty = IsOpenPropertyKey.DependencyProperty;

        public string CurrentText
        {
            get { return (string)GetValue(CurrentTextProperty); }
            private set { SetValue(CurrentTextPropertyKey, value); }
        }
        #endregion

        #region CurrentActionText Readonly DependencyProperty
        public static readonly DependencyPropertyKey CurrentActionTextPropertyKey = DependencyProperty.RegisterReadOnly(nameof(CurrentActionText),
            typeof(string),
            typeof(Snackbar),
            new PropertyMetadata(string.Empty));

        public static readonly DependencyProperty CurrentActionTextProperty = IsOpenPropertyKey.DependencyProperty;

        public string CurrentActionText
        {
            get { return (string)GetValue(CurrentActionTextProperty); }
            private set { SetValue(CurrentActionTextPropertyKey, value); }
        }
        #endregion

        #region CurrentSeverity Readonly DependencyProperty
        public static readonly DependencyPropertyKey CurrentSeverityPropertyKey = DependencyProperty.RegisterReadOnly(nameof(CurrentSeverity),
            typeof(SnackbarSeverity),
            typeof(Snackbar),
            new PropertyMetadata(SnackbarSeverity.None));

        public static readonly DependencyProperty CurrentSeverityProperty = IsOpenPropertyKey.DependencyProperty;

        public SnackbarSeverity CurrentSeverity
        {
            get { return (SnackbarSeverity)GetValue(CurrentSeverityProperty); }
            private set { SetValue(CurrentSeverityPropertyKey, value); }
        }
        #endregion

        #region IsOpen Readonly DependencyProperty
        public static readonly DependencyPropertyKey IsOpenPropertyKey = DependencyProperty.RegisterReadOnly(nameof(IsOpen),
            typeof(bool),
            typeof(Snackbar),
            new PropertyMetadata(BooleanBoxes.FalseBox));

        public static readonly DependencyProperty IsOpenProperty = IsOpenPropertyKey.DependencyProperty;

        public bool IsOpen
        {
            get { return (bool)GetValue(IsOpenProperty); }
            private set { SetValue(IsOpenPropertyKey, BooleanBoxes.Box(value)); }
        }
        #endregion

        #region IsActionStacked Readonly DependencyProperty
        public static readonly DependencyPropertyKey IsActionStackedPropertyKey = DependencyProperty.RegisterReadOnly(nameof(IsActionStacked),
            typeof(bool),
            typeof(Snackbar),
            new PropertyMetadata(BooleanBoxes.FalseBox));

        public static readonly DependencyProperty IsActionStackedProperty = IsActionStackedPropertyKey.DependencyProperty;

        public bool IsActionStacked
        {
            get { return (bool)GetValue(IsActionStackedProperty); }
            private set { SetValue(IsActionStackedPropertyKey, BooleanBoxes.Box(value)); }
        }
        #endregion

        #region StackThreshold DependencyProperty
        public static readonly DependencyProperty StackThresholdProperty = DependencyProperty.Register(nameof(StackThreshold),
            typeof(double),
            typeof(Snackbar),
            new PropertyMetadata(400.0));

        public double StackThreshold
        {
            get { return (double)GetValue(StackThresholdProperty); }
            set { SetValue(StackThresholdProperty, value); }
        }
        #endregion

        #region CornerRadius DependencyProperty
        public static readonly DependencyProperty CornerRadiusProperty = DependencyProperty.Register(nameof(CornerRadius),
            typeof(CornerRadius),
            typeof(Snackbar),
            new PropertyMetadata(default(CornerRadius)));

        public CornerRadius CornerRadius
        {
            get { return (CornerRadius)GetValue(CornerRadiusProperty); }
            set { SetValue(CornerRadiusProperty, value); }
        }
        #endregion

        #region ActionButtonForeground DependencyProperty
        public static readonly DependencyProperty ActionButtonForegroundProperty = DependencyProperty.Register(nameof(ActionButtonForeground),
            typeof(Brush),
            typeof(Snackbar),
            new PropertyMetadata(null));

        public Brush ActionButtonForeground
        {
            get { return (Brush)GetValue(ActionButtonForegroundProperty); }
            set { SetValue(ActionButtonForegroundProperty, value); }
        }
        #endregion

        private const string PART_Root = "PART_Root";
        private const string PART_Message = "PART_Message";
        private const string PART_ActionButton = "PART_ActionButton";

        private Border _rootBorder;
        private TextBlock _messageText;
        private ButtonBase _actionButton;

        private readonly DispatcherTimer _displayTimer = new DispatcherTimer();
        private SnackbarMessage _currentMessage;
        private bool _isAnimating;

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            if (_actionButton != null) _actionButton.Click -= OnActionClick;

            _rootBorder = GetTemplateChild(PART_Root) as Border;
            _messageText = GetTemplateChild(PART_Message) as TextBlock;
            _actionButton = GetTemplateChild(PART_ActionButton) as ButtonBase;

            if (_actionButton != null) _actionButton.Click += OnActionClick;
        }

        private void UpdateStackedLayout()
        {
            if (_messageText == null || _actionButton == null ||
                string.IsNullOrEmpty(CurrentActionText))
            {
                IsActionStacked = false;
                return;
            }

            var typeface = new Typeface(_messageText.FontFamily, _messageText.FontStyle, _messageText.FontWeight, _messageText.FontStretch);

            var messageFormatted = new FormattedText(
                CurrentText,
                System.Globalization.CultureInfo.CurrentCulture,
                FlowDirection.LeftToRight,
                typeface,
                _messageText.FontSize,
                Brushes.Black);

            var actionTypeface = new Typeface(_actionButton.FontFamily, _actionButton.FontStyle, _actionButton.FontWeight, _actionButton.FontStretch);

            var actionFormatted = new FormattedText(
                CurrentActionText,
                System.Globalization.CultureInfo.CurrentCulture,
                FlowDirection.LeftToRight,
                actionTypeface,
                _actionButton.FontSize,
                Brushes.Black);

            const double extraSpace = 58;
            var totalWidth = messageFormatted.Width + actionFormatted.Width + extraSpace;

            IsActionStacked = totalWidth > StackThreshold;
        }

        private void OnMessageQueueChanged(SnackbarMessageQueue oldQueue, SnackbarMessageQueue newQueue)
        {
            if (oldQueue != null)
                oldQueue.MessageEnqueued -= OnMessageEnqueued;

            if (newQueue != null)
                newQueue.MessageEnqueued += OnMessageEnqueued;
        }

        private void OnMessageEnqueued(SnackbarMessage message)
        {
            Dispatcher.InvokeAsync(() => ShowNextIfIdle());
        }

        private void ShowNextIfIdle()
        {
            if (_isAnimating || IsOpen) return;

            if (MessageQueue == null || !MessageQueue.TryDequeue(out var message) || message == null)
                return;

            ShowMessage(message);
        }

        private void ShowMessage(SnackbarMessage message)
        {
            _currentMessage = message;

            CurrentText = message.Text;
            CurrentActionText = message.ActionText ?? string.Empty;
            CurrentSeverity = message.Severity;

            Dispatcher.InvokeAsync(() => UpdateStackedLayout(), DispatcherPriority.Loaded);

            IsOpen = true;
            AnimateIn(() =>
            {
                _displayTimer.Interval = message.Duration;
                _displayTimer.Start();
            });
        }

        private void DismissCurrent(bool invokeAction = false)
        {
            _displayTimer.Stop();

            if (invokeAction)
            {
                _currentMessage?.ActionCallback?.Invoke();
                RaiseEvent(new RoutedEventArgs(ActionClickedEvent));
            }

            AnimateOut(() =>
            {
                IsOpen = false;
                _currentMessage = null;
                Dispatcher.InvokeAsync(ShowNextIfIdle, DispatcherPriority.Background);
            });
        }

        private void OnDisplayTimerTick(object sender, EventArgs e)
        {
            _displayTimer.Stop();
            DismissCurrent();
        }

        private void OnActionClick(object sender, RoutedEventArgs e)
        {
            DismissCurrent(invokeAction: true);
        }

        private static readonly Duration AnimDuration = new Duration(TimeSpan.FromMilliseconds(250));
        private static readonly CubicEase EaseOut = new CubicEase() { EasingMode = EasingMode.EaseOut };
        private static readonly CubicEase EaseIn = new CubicEase() { EasingMode = EasingMode.EaseIn };

        private void AnimateIn(Action onComplete)
        {
            if (_rootBorder == null) { onComplete(); return; }

            _isAnimating = true;
            Visibility = Visibility.Visible;
            EnsureTranslateTransform();

            var fade = CreateAnimation(1.0, EaseOut);
            var slide = CreateAnimation(0.0, EaseOut);
            slide.Completed += (_, __) => { _isAnimating = false; onComplete(); };

            _rootBorder.BeginAnimation(OpacityProperty, fade);
            _rootBorder.RenderTransform.BeginAnimation(TranslateTransform.YProperty, slide);
        }

        private void AnimateOut(Action onComplete)
        {
            if (_rootBorder == null) { onComplete(); return; }

            _isAnimating = true;

            var fade = CreateAnimation(0.0, EaseIn);
            var slide = CreateAnimation(20.0, EaseIn);
            slide.Completed += (_, __) =>
            {
                Visibility = Visibility.Collapsed;
                _isAnimating = false;
                onComplete();
            };

            _rootBorder.BeginAnimation(OpacityProperty, fade);
            _rootBorder.RenderTransform.BeginAnimation(TranslateTransform.YProperty, slide);
        }

        private static DoubleAnimation CreateAnimation(double to, IEasingFunction easing) => new DoubleAnimation()
        {
            To = to,
            Duration = AnimDuration,
            EasingFunction = easing
        };

        private void EnsureTranslateTransform()
        {
            if (!(_rootBorder?.RenderTransform is TranslateTransform))
                _rootBorder.RenderTransform = new TranslateTransform(0, 20);
        }

        public void Enqueue(string text, SnackbarSeverity severity = SnackbarSeverity.None, TimeSpan? duration = null, string actionText = null, Action actionCallback = null)
        {
            MessageQueue?.Enqueue(text, severity, duration, actionText, actionCallback);
        }

        public void EnqueueWithPriority(string text, SnackbarSeverity severity = SnackbarSeverity.None, TimeSpan? duration = null, string actionText = null, Action actionCallback = null)
        {
            MessageQueue?.EnqueueWithPriority(text, severity, duration, actionText, actionCallback);
        }
    }
}