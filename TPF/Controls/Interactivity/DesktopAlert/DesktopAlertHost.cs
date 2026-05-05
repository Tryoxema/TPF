using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Threading;

namespace TPF.Controls
{
    internal class DesktopAlertHost : Window
    {
        public DesktopAlertHost(DesktopAlert alert, DesktopAlertManager manager, TimeSpan? duration)
        {
            _duration = duration;

            WindowStyle = WindowStyle.None;
            AllowsTransparency = true;
            Background = Brushes.Transparent;
            ShowInTaskbar = false;
            Topmost = true;
            ResizeMode = ResizeMode.NoResize;
            SizeToContent = SizeToContent.WidthAndHeight;
            ShowActivated = false;
            Focusable = false;

            Content = alert;

            alert.CloseRequested += (_, __) => BeginClose();

            if (duration.HasValue && duration.Value > TimeSpan.Zero)
            {
                _autoCloseTimer = new DispatcherTimer { Interval = duration.Value };
                _autoCloseTimer.Tick += (_, __) => BeginClose();
            }

            Loaded += OnLoaded;
            MouseEnter += (_, __) => _autoCloseTimer?.Stop();
            MouseLeave += (_, __) =>
            {
                if (!_isClosing && _duration.HasValue)
                    _autoCloseTimer?.Start();
            };
            Closed += (_, __) => alert.RaiseClosed();
        }

        private readonly DispatcherTimer _autoCloseTimer;
        private readonly TimeSpan? _duration;
        private bool _isClosing;

        // Animation
        private EventHandler _renderingHandler;
        private DateTime _moveStartTime;
        private double _moveStartLeft, _moveStartTop;
        private double _moveTargetLeft, _moveTargetTop;
        private TimeSpan _moveDuration;

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            // Fade-In
            var fadeIn = new DoubleAnimation
            {
                From = 0,
                To = 1,
                Duration = TimeSpan.FromMilliseconds(250)
            };
            BeginAnimation(OpacityProperty, fadeIn);

            _autoCloseTimer?.Start();
        }

        public void MoveTo(double left, double top)
        {
            // Beim ersten Positionieren (vor dem Show) direkt setzen, ohne Animation
            if (!IsLoaded)
            {
                Left = left;
                Top = top;
                return;
            }

            // Wenn bereits am Ziel: nichts tun
            if (Math.Abs(Left - left) < 0.5 && Math.Abs(Top - top) < 0.5)
                return;

            _moveStartLeft = double.IsNaN(Left) ? left : Left;
            _moveStartTop = double.IsNaN(Top) ? top : Top;
            _moveTargetLeft = left;
            _moveTargetTop = top;
            _moveStartTime = DateTime.UtcNow;
            _moveDuration = TimeSpan.FromMilliseconds(200);

            // Falls bereits eine Animation läuft, vorher abmelden
            StopMoveAnimation();

            _renderingHandler = OnRenderingForMove;
            CompositionTarget.Rendering += _renderingHandler;
        }

        private void OnRenderingForMove(object sender, EventArgs e)
        {
            var elapsed = DateTime.UtcNow - _moveStartTime;
            var progress = Math.Min(1.0, elapsed.TotalMilliseconds / _moveDuration.TotalMilliseconds);

            // Easing (ease-out quad) - fühlt sich natürlicher an als linear
            var eased = 1 - Math.Pow(1 - progress, 2);

            Left = _moveStartLeft + (_moveTargetLeft - _moveStartLeft) * eased;
            Top = _moveStartTop + (_moveTargetTop - _moveStartTop) * eased;

            if (progress >= 1.0)
            {
                Left = _moveTargetLeft;
                Top = _moveTargetTop;
                StopMoveAnimation();
            }
        }

        private void StopMoveAnimation()
        {
            if (_renderingHandler != null)
            {
                CompositionTarget.Rendering -= _renderingHandler;
                _renderingHandler = null;
            }
        }

        public void BeginClose()
        {
            if (_isClosing) return;
            _isClosing = true;
            _autoCloseTimer?.Stop();

            var fadeOut = new DoubleAnimation
            {
                From = Opacity,
                To = 0,
                Duration = TimeSpan.FromMilliseconds(200)
            };
            fadeOut.Completed += (_, __) => Close();
            BeginAnimation(OpacityProperty, fadeOut);
        }

        protected override void OnClosed(EventArgs e)
        {
            // Beim Schließen aufräumen
            StopMoveAnimation();
            base.OnClosed(e);
        }
    }
}