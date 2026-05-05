using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using TPF.Internal;

namespace TPF.Controls
{
    public class DesktopAlert : ContentControl
    {
        static DesktopAlert()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(DesktopAlert), new FrameworkPropertyMetadata(typeof(DesktopAlert)));
        }

        #region Header DependencyProperty
        public static readonly DependencyProperty HeaderProperty = DependencyProperty.Register(nameof(Header),
            typeof(object),
            typeof(DesktopAlert),
            new PropertyMetadata(null));

        public object Header
        {
            get { return GetValue(HeaderProperty); }
            set { SetValue(HeaderProperty, value); }
        }
        #endregion

        #region HeaderTemplate DependencyProperty
        public static readonly DependencyProperty HeaderTemplateProperty = DependencyProperty.Register(nameof(HeaderTemplate),
            typeof(DataTemplate),
            typeof(DesktopAlert),
            new PropertyMetadata(null));

        public DataTemplate HeaderTemplate
        {
            get { return (DataTemplate)GetValue(HeaderTemplateProperty); }
            set { SetValue(HeaderTemplateProperty, value); }
        }
        #endregion

        #region Command DependencyProperty
        public static readonly DependencyProperty CommandProperty = DependencyProperty.Register(nameof(Command),
            typeof(ICommand),
            typeof(DesktopAlert),
            new PropertyMetadata(null));

        public ICommand Command
        {
            get { return (ICommand)GetValue(CommandProperty); }
            set { SetValue(CommandProperty, value); }
        }
        #endregion

        #region CommandParameter DependencyProperty
        public static readonly DependencyProperty CommandParameterProperty = DependencyProperty.Register(nameof(CommandParameter),
            typeof(object),
            typeof(DesktopAlert),
            new PropertyMetadata(null));

        public object CommandParameter
        {
            get { return GetValue(CommandParameterProperty); }
            set { SetValue(CommandParameterProperty, value); }
        }
        #endregion

        #region IsClosable DependencyProperty
        public static readonly DependencyProperty IsClosableProperty = DependencyProperty.Register(nameof(IsClosable),
            typeof(bool),
            typeof(DesktopAlert),
            new PropertyMetadata(BooleanBoxes.TrueBox));

        public bool IsClosable
        {
            get { return (bool)GetValue(IsClosableProperty); }
            set { SetValue(IsClosableProperty, BooleanBoxes.Box(value)); }
        }
        #endregion

        #region CornerRadius DependencyProperty
        public static readonly DependencyProperty CornerRadiusProperty = DependencyProperty.Register(nameof(CornerRadius),
            typeof(CornerRadius),
            typeof(DesktopAlert),
            new PropertyMetadata(default(CornerRadius)));

        public CornerRadius CornerRadius
        {
            get { return (CornerRadius)GetValue(CornerRadiusProperty); }
            set { SetValue(CornerRadiusProperty, value); }
        }
        #endregion

        // Wird vom Manager abonniert, um den Alert programmatisch zu schließen (z. B. via Close-Button im Template).
        public event EventHandler CloseRequested;

        // Wird gefeuert wenn der Alert geklickt wird (z.B. um Command auszuführen).
        public event EventHandler Clicked;

        // Wird gefeuert, nachdem der Alert tatsächlich geschlossen wurde.
        public event EventHandler Closed;

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            if (GetTemplateChild("PART_CloseButton") is ButtonBase closeButton)
            {
                closeButton.Click -= OnCloseButtonClick;
                closeButton.Click += OnCloseButtonClick;
            }
        }

        protected override void OnMouseLeftButtonUp(MouseButtonEventArgs e)
        {
            base.OnMouseLeftButtonUp(e);

            if (Command?.CanExecute(CommandParameter) == true)
                Command.Execute(CommandParameter);

            Clicked?.Invoke(this, EventArgs.Empty);
        }

        private void OnCloseButtonClick(object sender, RoutedEventArgs e)
        {
            // Verhindern, dass der Click bis zum Alert hochbubble't und Command auslöst
            e.Handled = true;
            RaiseCloseRequested();
        }

        internal void RaiseCloseRequested()
        {
            CloseRequested?.Invoke(this, EventArgs.Empty);
        }

        internal void RaiseClosed()
        {
            Closed?.Invoke(this, EventArgs.Empty);
        }
    }
}