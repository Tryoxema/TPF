using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Markup;
using TPF.Internal;

namespace TPF.Controls
{
    [ContentProperty("DropDownContent")]
    public class SplitButton : ContentControl, ICommandSource
    {
        static SplitButton()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(SplitButton), new FrameworkPropertyMetadata(typeof(SplitButton)));

            EventManager.RegisterClassHandler(typeof(SplitButton), Mouse.MouseDownEvent, new MouseButtonEventHandler(OnMouseButtonDown), true);
        }

        #region Click RoutedEvent
        public static readonly RoutedEvent ClickEvent = EventManager.RegisterRoutedEvent("Click",
            RoutingStrategy.Bubble,
            typeof(RoutedEventHandler),
            typeof(SplitButton));

        public event RoutedEventHandler Click
        {
            add => AddHandler(ClickEvent, value);
            remove => RemoveHandler(ClickEvent, value);
        }
        #endregion

        #region Checked RoutedEvent
        public static readonly RoutedEvent CheckedEvent = EventManager.RegisterRoutedEvent("Checked",
            RoutingStrategy.Bubble,
            typeof(RoutedEventHandler),
            typeof(SplitButton));

        public event RoutedEventHandler Checked
        {
            add => AddHandler(CheckedEvent, value);
            remove => RemoveHandler(CheckedEvent, value);
        }
        #endregion

        #region Unchecked RoutedEvent
        public static readonly RoutedEvent UncheckedEvent = EventManager.RegisterRoutedEvent("Unchecked",
            RoutingStrategy.Bubble,
            typeof(RoutedEventHandler),
            typeof(SplitButton));

        public event RoutedEventHandler Unchecked
        {
            add => AddHandler(UncheckedEvent, value);
            remove => RemoveHandler(UncheckedEvent, value);
        }
        #endregion

        #region Toggle RoutedEvent
        public static readonly RoutedEvent ToggleEvent = EventManager.RegisterRoutedEvent("Toggle",
            RoutingStrategy.Bubble,
            typeof(RoutedPropertyChangedEventHandler<bool>),
            typeof(SplitButton));

        public event RoutedPropertyChangedEventHandler<bool> Toggle
        {
            add => AddHandler(ToggleEvent, value);
            remove => RemoveHandler(ToggleEvent, value);
        }
        #endregion

        #region DropDownOpened RoutedEvent
        public static readonly RoutedEvent DropDownOpenedEvent = EventManager.RegisterRoutedEvent("DropDownOpened",
            RoutingStrategy.Bubble,
            typeof(RoutedEventHandler),
            typeof(SplitButton));

        public event RoutedEventHandler DropDownOpened
        {
            add => AddHandler(DropDownOpenedEvent, value);
            remove => RemoveHandler(DropDownOpenedEvent, value);
        }
        #endregion

        #region DropDownClosed RoutedEvent
        public static readonly RoutedEvent DropDownClosedEvent = EventManager.RegisterRoutedEvent("DropDownClosed",
            RoutingStrategy.Bubble,
            typeof(RoutedEventHandler),
            typeof(SplitButton));

        public event RoutedEventHandler DropDownClosed
        {
            add => AddHandler(DropDownClosedEvent, value);
            remove => RemoveHandler(DropDownClosedEvent, value);
        }
        #endregion

        #region IsChecked DependencyProperty
        public static readonly DependencyProperty IsCheckedProperty = DependencyProperty.Register("IsChecked",
            typeof(bool),
            typeof(SplitButton),
            new FrameworkPropertyMetadata(BooleanBoxes.FalseBox, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnIsCheckedChanged));

        private static void OnIsCheckedChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            var instance = (SplitButton)sender;

            var newValue = (bool)e.NewValue;

            var eventArgs = new RoutedPropertyChangedEventArgs<bool>((bool)e.OldValue, (bool)e.NewValue) { RoutedEvent = ToggleEvent };

            // Toggle-Event triggern
            instance.RaiseEvent(eventArgs);

            // Abhängig vom neuen Wert Checked oder Unchecked triggern
            if (newValue)
            {
                instance.OnChecked();
            }
            else
            {
                instance.OnUnchecked();
            }
        }

        public bool IsChecked
        {
            get { return (bool)GetValue(IsCheckedProperty); }
            set { SetValue(IsCheckedProperty, BooleanBoxes.Box(value)); }
        }
        #endregion

        #region IsToggle DependencyProperty
        public static readonly DependencyProperty IsToggleProperty = DependencyProperty.Register("IsToggle",
            typeof(bool),
            typeof(SplitButton),
            new PropertyMetadata(BooleanBoxes.FalseBox));

        public bool IsToggle
        {
            get { return (bool)GetValue(IsToggleProperty); }
            set { SetValue(IsToggleProperty, BooleanBoxes.Box(value)); }
        }
        #endregion

        #region IsDropDownOpen DependencyProperty
        public static readonly DependencyProperty IsDropDownOpenProperty = DependencyProperty.Register("IsDropDownOpen",
            typeof(bool),
            typeof(SplitButton),
            new FrameworkPropertyMetadata(BooleanBoxes.FalseBox, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnIsDropDownOpenChanged));

        private static void OnIsDropDownOpenChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            var instance = (SplitButton)sender;

            var open = (bool)e.NewValue;

            if (open)
            {
                Mouse.Capture(instance, CaptureMode.SubTree);

                instance.OnDropDownOpened();
            }
            else
            {
                if (Mouse.Captured == instance) Mouse.Capture(null);

                instance.OnDropDownClosed();
            }
        }

        public bool IsDropDownOpen
        {
            get { return (bool)GetValue(IsDropDownOpenProperty); }
            set { SetValue(IsDropDownOpenProperty, BooleanBoxes.Box(value)); }
        }
        #endregion

        #region DropDownMinHeight DependencyProperty
        public static readonly DependencyProperty DropDownMinHeightProperty = DependencyProperty.Register("DropDownMinHeight",
            typeof(double),
            typeof(SplitButton),
            new PropertyMetadata(double.NaN));

        public double DropDownMinHeight
        {
            get { return (double)GetValue(DropDownMinHeightProperty); }
            set { SetValue(DropDownMinHeightProperty, value); }
        }
        #endregion

        #region DropDownMinWidth DependencyProperty
        public static readonly DependencyProperty DropDownMinWidthProperty = DependencyProperty.Register("DropDownMinWidth",
            typeof(double),
            typeof(SplitButton),
            new PropertyMetadata(double.NaN));

        public double DropDownMinWidth
        {
            get { return (double)GetValue(DropDownMinWidthProperty); }
            set { SetValue(DropDownMinWidthProperty, value); }
        }
        #endregion

        #region DropDownHeight DependencyProperty
        public static readonly DependencyProperty DropDownHeightProperty = DependencyProperty.Register("DropDownHeight",
            typeof(double),
            typeof(SplitButton),
            new PropertyMetadata(double.NaN));

        public double DropDownHeight
        {
            get { return (double)GetValue(DropDownHeightProperty); }
            set { SetValue(DropDownHeightProperty, value); }
        }
        #endregion

        #region DropDownWidth DependencyProperty
        public static readonly DependencyProperty DropDownWidthProperty = DependencyProperty.Register("DropDownWidth",
            typeof(double),
            typeof(SplitButton),
            new PropertyMetadata(double.NaN));

        public double DropDownWidth
        {
            get { return (double)GetValue(DropDownWidthProperty); }
            set { SetValue(DropDownWidthProperty, value); }
        }
        #endregion

        #region DropDownMaxHeight DependencyProperty
        public static readonly DependencyProperty DropDownMaxHeightProperty = DependencyProperty.Register("DropDownMaxHeight",
            typeof(double),
            typeof(SplitButton),
            new PropertyMetadata(double.NaN));

        public double DropDownMaxHeight
        {
            get { return (double)GetValue(DropDownMaxHeightProperty); }
            set { SetValue(DropDownMaxHeightProperty, value); }
        }
        #endregion

        #region DropDownMaxWidth DependencyProperty
        public static readonly DependencyProperty DropDownMaxWidthProperty = DependencyProperty.Register("DropDownMaxWidth",
            typeof(double),
            typeof(SplitButton),
            new PropertyMetadata(double.NaN));

        public double DropDownMaxWidth
        {
            get { return (double)GetValue(DropDownMaxWidthProperty); }
            set { SetValue(DropDownMaxWidthProperty, value); }
        }
        #endregion

        #region DropDownContent DependencyProperty
        public static readonly DependencyProperty DropDownContentProperty = DependencyProperty.Register("DropDownContent",
            typeof(object),
            typeof(SplitButton),
            new PropertyMetadata(null));

        public object DropDownContent
        {
            get { return GetValue(DropDownContentProperty); }
            set { SetValue(DropDownContentProperty, value); }
        }
        #endregion

        #region DropDownContentTemplate DependencyProperty
        public static readonly DependencyProperty DropDownContentTemplateProperty = DependencyProperty.Register("DropDownContentTemplate",
            typeof(DataTemplate),
            typeof(SplitButton),
            new PropertyMetadata(null));

        public DataTemplate DropDownContentTemplate
        {
            get { return (DataTemplate)GetValue(DropDownContentTemplateProperty); }
            set { SetValue(DropDownContentTemplateProperty, value); }
        }
        #endregion

        #region DropDownContentTemplateSelector DependencyProperty
        public static readonly DependencyProperty DropDownContentTemplateSelectorProperty = DependencyProperty.Register("DropDownContentTemplateSelector",
            typeof(DataTemplateSelector),
            typeof(SplitButton),
            new PropertyMetadata(null));

        public DataTemplateSelector DropDownContentTemplateSelector
        {
            get { return (DataTemplateSelector)GetValue(DropDownContentTemplateSelectorProperty); }
            set { SetValue(DropDownContentTemplateSelectorProperty, value); }
        }
        #endregion

        #region ButtonStyle DependencyProperty
        public static readonly DependencyProperty ButtonStyleProperty = DependencyProperty.Register("ButtonStyle",
            typeof(Style),
            typeof(SplitButton),
            new PropertyMetadata(null));

        public Style ButtonStyle
        {
            get { return (Style)GetValue(ButtonStyleProperty); }
            set { SetValue(ButtonStyleProperty, value); }
        }
        #endregion

        #region ToggleButtonStyle DependencyProperty
        public static readonly DependencyProperty ToggleButtonStyleProperty = DependencyProperty.Register("ToggleButtonStyle",
            typeof(Style),
            typeof(SplitButton),
            new PropertyMetadata(null));

        public Style ToggleButtonStyle
        {
            get { return (Style)GetValue(ToggleButtonStyleProperty); }
            set { SetValue(ToggleButtonStyleProperty, value); }
        }
        #endregion

        #region Command DependencyProperty
        public static readonly DependencyProperty CommandProperty = DependencyProperty.Register("Command",
            typeof(ICommand),
            typeof(SplitButton),
            new PropertyMetadata(null, OnCommandChanged));

        private static void OnCommandChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            var instance = (SplitButton)sender;

            if (e.OldValue is ICommand oldCommand)
            {
                instance.UnhookCommand(oldCommand);
            }

            if (e.NewValue is ICommand newCommand)
            {
                instance.HookCommand(newCommand);
            }
        }

        private void UnhookCommand(ICommand command)
        {
            command.CanExecuteChanged -= CanExecuteChanged;
        }

        private void HookCommand(ICommand command)
        {
            command.CanExecuteChanged += CanExecuteChanged;
        }

        private void CanExecuteChanged(object sender, EventArgs e)
        {
            if (Command == null) return;

            if (Command is RoutedCommand command)
            {
                if (command.CanExecute(CommandParameter, CommandTarget)) SetPrimaryButtonIsEnabled(true);
                else SetPrimaryButtonIsEnabled(false);
            }
            else
            {
                if (Command.CanExecute(CommandParameter)) SetPrimaryButtonIsEnabled(true);
                else SetPrimaryButtonIsEnabled(false);
            }
        }

        public ICommand Command
        {
            get { return (ICommand)GetValue(CommandProperty); }
            set { SetValue(CommandProperty, value); }
        }
        #endregion

        #region CommandParameter DependencyProperty
        public static readonly DependencyProperty CommandParameterProperty = DependencyProperty.Register("CommandParameter",
            typeof(object),
            typeof(SplitButton),
            new PropertyMetadata(null));

        public object CommandParameter
        {
            get { return GetValue(CommandParameterProperty); }
            set { SetValue(CommandParameterProperty, value); }
        }
        #endregion

        #region CommandTarget DependencyProperty
        public static readonly DependencyProperty CommandTargetProperty = DependencyProperty.Register("CommandTarget",
            typeof(IInputElement),
            typeof(SplitButton),
            new PropertyMetadata(null));

        public IInputElement CommandTarget
        {
            get { return (IInputElement)GetValue(CommandTargetProperty); }
            set { SetValue(CommandTargetProperty, value); }
        }
        #endregion

        #region CornerRadius DependencyProperty
        public static readonly DependencyProperty CornerRadiusProperty = DependencyProperty.Register("CornerRadius",
            typeof(CornerRadius),
            typeof(SplitButton),
            new PropertyMetadata(default(CornerRadius)));

        public CornerRadius CornerRadius
        {
            get { return (CornerRadius)GetValue(CornerRadiusProperty); }
            set { SetValue(CornerRadiusProperty, value); }
        }
        #endregion

        private ButtonBase PrimaryButton;

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            if (PrimaryButton != null)
            {
                PrimaryButton.Click -= PrimaryButton_Click;
            }

            PrimaryButton = GetTemplateChild("PART_Button") as ButtonBase;

            if (PrimaryButton != null)
            {
                PrimaryButton.Click += PrimaryButton_Click;
            }
        }

        private void SetPrimaryButtonIsEnabled(bool value)
        {
            if (PrimaryButton == null) return;

            PrimaryButton.IsEnabled = value;
        }

        private void PrimaryButton_Click(object sender, RoutedEventArgs e)
        {
            if (!IsKeyboardFocusWithin) Focus();
            OnClick();
        }

        private void ToggleChecked()
        {
            IsChecked = !IsChecked;
        }

        private void ExecuteCommand()
        {
            if (Command == null) return;

            if (Command is RoutedCommand routedCommand)
            {
                var target = CommandTarget;

                if (target == null)
                {
                    target = this;
                }

                if (routedCommand.CanExecute(CommandParameter, target)) routedCommand.Execute(CommandParameter, target);
            }
            else if (Command.CanExecute(CommandParameter))
            {
                Command.Execute(CommandParameter);
            }
        }

        private static void OnMouseButtonDown(object sender, MouseButtonEventArgs e)
        {
            var instance = (SplitButton)sender;

            // Wenn wir MouseCapture haben, dann Dropdown schließen wenn als Source das Control angegeben ist
            if (Mouse.Captured == instance && e.OriginalSource == instance)
            {
                e.Handled = true;

                instance.IsDropDownOpen = false;
            }
        }

        // Löst das Click-Event aus
        protected virtual void OnClick()
        {
            // Wenn der Button im Toggle-Modus ist, dann Wert invertieren
            if (IsToggle) ToggleChecked();

            var eventArgs = new RoutedEventArgs() { RoutedEvent = ClickEvent };

            // Click-Event auslösen
            RaiseEvent(eventArgs);

            // Command ausführen
            ExecuteCommand();

            if (IsDropDownOpen) IsDropDownOpen = false;
        }

        // Löst das Click-Event aus
        protected virtual void OnChecked()
        {
            var eventArgs = new RoutedEventArgs() { RoutedEvent = CheckedEvent };

            RaiseEvent(eventArgs);
        }

        // Löst das Click-Event aus
        protected virtual void OnUnchecked()
        {
            var eventArgs = new RoutedEventArgs() { RoutedEvent = UncheckedEvent };

            RaiseEvent(eventArgs);
        }

        // Löst das DropDownOpened-Event aus
        protected virtual void OnDropDownOpened()
        {
            var eventArgs = new RoutedEventArgs() { RoutedEvent = DropDownOpenedEvent };

            RaiseEvent(eventArgs);
        }

        // Löst das DropDownClosed-Event aus
        protected virtual void OnDropDownClosed()
        {
            var eventArgs = new RoutedEventArgs() { RoutedEvent = DropDownClosedEvent };

            RaiseEvent(eventArgs);
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            if (e.OriginalSource == this)
            {
                if (e.Key == Key.Enter)
                {
                    OnClick();
                    e.Handled = true;
                }
                else if (e.Key == Key.Down)
                {
                    IsDropDownOpen = true;
                    e.Handled = true;
                }
            }

            base.OnKeyDown(e);
        }
    }
}