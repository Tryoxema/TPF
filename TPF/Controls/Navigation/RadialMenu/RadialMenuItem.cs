using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Markup;

namespace TPF.Controls
{
    [ContentProperty("ChildItems")]
    public class RadialMenuItem : Control
    {
        static RadialMenuItem()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(RadialMenuItem), new FrameworkPropertyMetadata(typeof(RadialMenuItem)));
        }

        public RadialMenuItem()
        {
            _childItems = new RadialMenuItemCollection();

            IsEnabledChanged += (_, __) =>
            {
                UpdateVisualState();
            };

            MouseEnter += (_, __) =>
            {
                UpdateVisualState();
            };

            MouseLeave += (_, __) =>
            {
                UpdateVisualState();
            };

            MouseLeftButtonUp += (_, __) =>
            {
                RaiseClickEventAndCommand();
            };
        }

        RadialMenuItemCollection _childItems;
        public ObservableCollection<RadialMenuItem> ChildItems
        {
            get { return _childItems; }
        }

        #region Click RoutedEvent
        public static readonly RoutedEvent ClickEvent = EventManager.RegisterRoutedEvent("Click",
            RoutingStrategy.Bubble,
            typeof(RoutedEventHandler),
            typeof(RadialMenuItem));

        public event RoutedEventHandler Click
        {
            add => AddHandler(ClickEvent, value);
            remove => RemoveHandler(ClickEvent, value);
        }
        #endregion

        #region Command DependencyProperty
        public static readonly DependencyProperty CommandProperty = DependencyProperty.Register("Command",
            typeof(ICommand),
            typeof(RadialMenuItem),
            new PropertyMetadata(null, CommandChanged));

        private static void CommandChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var instance = (RadialMenuItem)d;
            instance.HookUpCommand((ICommand)e.OldValue, (ICommand)e.NewValue);
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
            typeof(RadialMenuItem),
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
            typeof(RadialMenuItem),
            new PropertyMetadata(null));

        public IInputElement CommandTarget
        {
            get { return (IInputElement)GetValue(CommandTargetProperty); }
            set { SetValue(CommandTargetProperty, value); }
        }
        #endregion

        #region IconContent DependencyProperty
        public static readonly DependencyProperty IconContentProperty = DependencyProperty.Register("IconContent",
            typeof(object),
            typeof(RadialMenuItem),
            new PropertyMetadata(null));

        public object IconContent
        {
            get { return GetValue(IconContentProperty); }
            set { SetValue(IconContentProperty, value); }
        }
        #endregion

        #region HeaderContent DependencyProperty
        public static readonly DependencyProperty HeaderContentProperty = DependencyProperty.Register("HeaderContent",
            typeof(object),
            typeof(RadialMenuItem),
            new PropertyMetadata(null));

        public object HeaderContent
        {
            get { return GetValue(HeaderContentProperty); }
            set { SetValue(HeaderContentProperty, value); }
        }
        #endregion

        private void HookUpCommand(ICommand oldCommand, ICommand newCommand)
        {
            // Wenn oldCommand nicht null ist muss der alte Handler entfernt werden
            if (oldCommand != null) RemoveCommand(oldCommand);

            AddCommand(newCommand);
        }

        private void RemoveCommand(ICommand oldCommand)
        {
            EventHandler handler = CanExecuteChanged;
            oldCommand.CanExecuteChanged -= handler;
        }

        private void AddCommand(ICommand newCommand)
        {
            EventHandler handler = new EventHandler(CanExecuteChanged);

            if (newCommand != null) newCommand.CanExecuteChanged += handler;
        }

        private void CanExecuteChanged(object sender, EventArgs e)
        {
            if (Command != null)
            {
                if (Command is RoutedCommand command)
                {
                    if (command.CanExecute(CommandParameter, CommandTarget)) IsEnabled = true;
                    else IsEnabled = false;
                }
                else
                {
                    if (Command.CanExecute(CommandParameter)) IsEnabled = true;
                    else IsEnabled = false;
                }
            }
        }

        internal void RaiseClickEventAndCommand()
        {
            if (!IsEnabled) return;

            if (Command != null)
            {
                if (Command is RoutedCommand routedCommand) routedCommand.Execute(CommandParameter, CommandTarget);
                else Command.Execute(CommandParameter);
            }

            var eventArgs = new RoutedEventArgs() { RoutedEvent = ClickEvent };

            RaiseEvent(eventArgs);
        }

        internal RadialMenu ParentMenu;

        protected virtual void UpdateVisualState()
        {
            UpdateVisualState(true);
        }

        protected virtual void UpdateVisualState(bool useTransitions)
        {
            if (!IsEnabled)
            {
                VisualStateManager.GoToState(this, "Disabled", useTransitions);
            }
            else if (IsMouseOver)
            {
                VisualStateManager.GoToState(this, "MouseOver", useTransitions);
            }
            else
            {
                VisualStateManager.GoToState(this, "Normal", useTransitions);
            }
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            ParentMenu = this.ParentOfType<RadialMenu>();
        }
    }
}