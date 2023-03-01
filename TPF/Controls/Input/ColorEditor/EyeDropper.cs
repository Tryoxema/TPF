using System.Windows;
using System.Windows.Data;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using TPF.Converter;
using TPF.Internal.Interop;

namespace TPF.Controls
{
    public class EyeDropper : Border
    {
        #region BeginColorPicking RoutedEvent
        public static readonly RoutedEvent BeginColorPickingEvent = EventManager.RegisterRoutedEvent("BeginColorPicking",
            RoutingStrategy.Bubble,
            typeof(RoutedEventHandler),
            typeof(EyeDropper));

        public event RoutedEventHandler BeginColorPicking
        {
            add => AddHandler(BeginColorPickingEvent, value);
            remove => RemoveHandler(BeginColorPickingEvent, value);
        }
        #endregion

        #region CancelColorPicking RoutedEvent
        public static readonly RoutedEvent CancelColorPickingEvent = EventManager.RegisterRoutedEvent("CancelColorPicking",
            RoutingStrategy.Bubble,
            typeof(RoutedEventHandler),
            typeof(EyeDropper));

        public event RoutedEventHandler CancelColorPicking
        {
            add => AddHandler(CancelColorPickingEvent, value);
            remove => RemoveHandler(CancelColorPickingEvent, value);
        }
        #endregion

        #region EndColorPicking RoutedEvent
        public static readonly RoutedEvent EndColorPickingEvent = EventManager.RegisterRoutedEvent("EndColorPicking",
            RoutingStrategy.Bubble,
            typeof(ColorChangeEventHandler),
            typeof(EyeDropper));

        public event ColorChangeEventHandler EndColorPicking
        {
            add => AddHandler(EndColorPickingEvent, value);
            remove => RemoveHandler(EndColorPickingEvent, value);
        }
        #endregion

        #region ColorChanged RoutedEvent
        public static readonly RoutedEvent ColorChangedEvent = EventManager.RegisterRoutedEvent("ColorChanged",
            RoutingStrategy.Bubble,
            typeof(ColorChangeEventHandler),
            typeof(EyeDropper));

        public event ColorChangeEventHandler ColorChanged
        {
            add => AddHandler(ColorChangedEvent, value);
            remove => RemoveHandler(ColorChangedEvent, value);
        }
        #endregion

        #region Color DependencyProperty
        public static readonly DependencyProperty ColorProperty = DependencyProperty.Register("Color",
            typeof(Color),
            typeof(EyeDropper),
            new FrameworkPropertyMetadata(Colors.White, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnColorChanged));

        private static void OnColorChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            var instance = (EyeDropper)sender;

            instance.OnColorChanged();
        }

        public Color Color
        {
            get { return (Color)GetValue(ColorProperty); }
            set { SetValue(ColorProperty, value); }
        }
        #endregion

        bool _pickingInProgress;
        Window _window;

        protected virtual void OnColorChanged()
        {
            var eventArgs = new ColorChangeEventArgs(Color)
            {
                RoutedEvent = ColorChangedEvent
            };

            RaiseEvent(eventArgs);
        }

        protected override void OnMouseDown(MouseButtonEventArgs e)
        {
            if (e.RightButton == MouseButtonState.Pressed)
            {
                EndPicking(true);
                return;
            }

            if (e.LeftButton == MouseButtonState.Pressed) StartPicking();
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            if (e.LeftButton != MouseButtonState.Pressed) return;

            var point = NativeMethods.GetCursorPosition();

            MoveWindow(point.X, point.Y);
            Color = NativeMethods.GetColorAt(point.X, point.Y);

            Mouse.Synchronize();
        }

        protected override void OnMouseUp(MouseButtonEventArgs e)
        {
            EndPicking(false);
        }

        private void StartPicking()
        {
            if (_pickingInProgress) return;

            CaptureMouse();
            _pickingInProgress = true;

            var point = NativeMethods.GetCursorPosition();

            _window = CreateWindow();
            MoveWindow(point.X, point.Y);

            var eventArgs = new RoutedEventArgs(BeginColorPickingEvent);

            RaiseEvent(eventArgs);
        }

        private Window CreateWindow()
        {
            var window = new Window()
            {
                WindowStyle = WindowStyle.None,
                Width = 100,
                Height = 125,
                AllowsTransparency = true,
                IsHitTestVisible = false,
                ShowInTaskbar = false,
                Background = Brushes.Transparent
            };

            var border = new Border()
            {
                BorderBrush = ResourceManager.Resources.BorderBrush,
                Background = ResourceManager.Resources.ApplicationBackground,
                BorderThickness = new Thickness(1),
                CornerRadius = new CornerRadius(5)
            };

            var rectangle = new Rectangle()
            {
                Margin = new Thickness(5),
                Stretch = Stretch.UniformToFill
            };

            rectangle.SetBinding(Shape.FillProperty, new Binding("Color") { Source = this, Converter = new ColorToBrushConverter() });

            var textBlock = new TextBlock()
            {
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center,
                Foreground = ResourceManager.Resources.TextBrush,
                FontSize = 14,
                Margin = new Thickness(0, 0, 0, 10)
            };

            textBlock.SetBinding(TextBlock.TextProperty, new Binding("Color") { Source = this, Converter = new ColorToHexConverter() });

            var panel = new StackPanel();

            panel.Children.Add(rectangle);
            panel.Children.Add(textBlock);
            border.Child = panel;
            window.Content = border;
            window.Show();

            return window;
        }

        private void MoveWindow(int x, int y)
        {
            if (_window == null) return;

            _window.Left = x - 20;
            _window.Top = y + 20;
        }

        private void EndPicking(bool cancel)
        {
            ReleaseMouseCapture();
            _pickingInProgress = false;

            _window?.Close();
            _window = null;

            RoutedEventArgs eventArgs;

            if (cancel) eventArgs = new RoutedEventArgs(CancelColorPickingEvent);
            else eventArgs = new ColorChangeEventArgs(Color) { RoutedEvent = EndColorPickingEvent };

            RaiseEvent(eventArgs);
        }
    }
}