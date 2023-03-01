using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace TPF.Controls
{
    public class Pad : ContentControl
    {
        static Pad() => DefaultStyleKeyProperty.OverrideMetadata(typeof(Pad), new FrameworkPropertyMetadata(typeof(Pad)));

        private readonly TranslateTransform _translateTransform = new TranslateTransform();

        #region MovementDirection DependencyProperty
        public static readonly DependencyProperty MovementDirectionProperty = DependencyProperty.Register("MovementDirection",
            typeof(MovementDirection),
            typeof(Pad),
            new PropertyMetadata(MovementDirection.Both));

        public MovementDirection MovementDirection
        {
            get { return (MovementDirection)GetValue(MovementDirectionProperty); }
            set { SetValue(MovementDirectionProperty, value); }
        }
        #endregion

        #region RelativePositionPoint DependencyProperty
        public static readonly DependencyProperty RelativePositionPointProperty = DependencyProperty.Register("RelativePositionPoint",
            typeof(Point),
            typeof(Pad),
            new PropertyMetadata(new Point(0, 0), OnRelativePositionPointChanged, CoerceRelativePoint));

        private static void OnRelativePositionPointChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            var instance = (Pad)sender;

            instance.UpdateCursorPosition();

            instance.RaisePositionPointChanging();
        }

        private static object CoerceRelativePoint(DependencyObject sender, object value)
        {
            var instance = (Pad)sender;

            var point = (Point)value;

            instance.KeepPointInBounds(ref point, false);

            return point;
        }

        public Point RelativePositionPoint
        {
            get { return (Point)GetValue(RelativePositionPointProperty); }
            set { SetValue(RelativePositionPointProperty, value); }
        }
        #endregion

        public event EventHandler<PositionPointChangeEventArgs> PositionPointChanging;

        public event EventHandler<PositionPointChangeEventArgs> PositionPointChanged;

        protected void RaisePositionPointChanging()
        {
            PositionPointChanging?.Invoke(this, new PositionPointChangeEventArgs(RelativePositionPoint));
        }

        protected void RaisePositionPointChanged()
        {
            PositionPointChanged?.Invoke(this, new PositionPointChangeEventArgs(RelativePositionPoint));
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            UpdateCursorPosition();
        }

        private void KeepPointInBounds(ref Point point, bool isAbsolute)
        {
            if (isAbsolute)
            {
                if (point.X < 0) point.X = 0;
                else if (point.X > ActualWidth - 1) point.X = ActualWidth - 1;

                if (point.Y < 0) point.Y = 0;
                else if (point.Y > ActualHeight - 1) point.Y = ActualHeight - 1;
            }
            else
            {
                if (point.X < 0) point.X = 0;
                else if (point.X > 1) point.X = 1;

                if (point.Y < 0) point.Y = 0;
                else if (point.Y > 1) point.Y = 1;
            }
        }

        protected Point GetRelativePoint(Point point)
        {
            var calculationWidth = ActualWidth - 1;
            var calculationHeight = ActualHeight - 1;

            return new Point()
            {
                X = point.X / calculationWidth,
                Y = point.Y / calculationHeight
            };
        }

        protected void UpdateCursorPosition()
        {
            var absolutePoint = new Point()
            {
                X = ActualWidth * RelativePositionPoint.X,
                Y = ActualHeight * RelativePositionPoint.Y
            };

            if (Content is FrameworkElement element)
            {
                switch (MovementDirection)
                {
                    case MovementDirection.Both:
                    {
                        _translateTransform.X = absolutePoint.X - element.Width / 2;
                        _translateTransform.Y = absolutePoint.Y - element.Height / 2;
                        break;
                    }
                    case MovementDirection.X:
                    {
                        _translateTransform.X = absolutePoint.X - element.Width / 2;
                        _translateTransform.Y = 0;
                        break;
                    }
                    case MovementDirection.Y:
                    {
                        _translateTransform.X = 0;
                        _translateTransform.Y = absolutePoint.Y - element.Height / 2;
                        break;
                    }
                }
            }
        }

        protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            var point = e.GetPosition(this);

            KeepPointInBounds(ref point, true);

            RelativePositionPoint = GetRelativePoint(point);

            CaptureMouse();
        }

        protected override void OnMouseLeftButtonUp(MouseButtonEventArgs e)
        {
            ReleaseMouseCapture();

            RaisePositionPointChanged();
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                var point = e.GetPosition(this);

                KeepPointInBounds(ref point, true);

                RelativePositionPoint = GetRelativePoint(point);

                Mouse.Synchronize();
            }
        }

        protected override void OnRenderSizeChanged(SizeChangedInfo sizeInfo)
        {
            base.OnRenderSizeChanged(sizeInfo);

            UpdateCursorPosition();
        }

        protected override void OnContentChanged(object oldContent, object newContent)
        {
            if (oldContent is FrameworkElement oldElement && oldElement.RenderTransform == _translateTransform)
            {
                oldElement.RenderTransform = null;
            }
            if (newContent is FrameworkElement newElement)
            {
                newElement.RenderTransform = _translateTransform;
            }
            base.OnContentChanged(oldContent, newContent);
        }
    }

    public enum MovementDirection
    {
        Both = 0,
        X = 1,
        Y = 2
    }
}