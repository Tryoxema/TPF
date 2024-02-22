using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Threading.Tasks;
using System.Windows.Threading;
using TPF.Controls.Specialized.DialogHost;
using TPF.Internal;

namespace TPF.Controls
{
    public class DialogHost : ContentControl
    {
        static DialogHost()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(DialogHost), new FrameworkPropertyMetadata(typeof(DialogHost)));

            InitializeCommands();
        }

        public DialogHost()
        {
            Loaded += OnLoaded;
            Unloaded += OnUnloaded;
        }

        #region DialogOpened RoutedEvent
        public static readonly RoutedEvent DialogOpenedEvent = EventManager.RegisterRoutedEvent("DialogOpened",
            RoutingStrategy.Bubble,
            typeof(DialogOpenedEventHandler),
            typeof(DialogHost));

        public event DialogOpenedEventHandler DialogOpened
        {
            add => AddHandler(DialogOpenedEvent, value);
            remove => RemoveHandler(DialogOpenedEvent, value);
        }
        #endregion

        #region DialogClosing RoutedEvent
        public static readonly RoutedEvent DialogClosingEvent = EventManager.RegisterRoutedEvent("DialogClosing",
            RoutingStrategy.Bubble,
            typeof(DialogClosingEventHandler),
            typeof(DialogHost));

        public event DialogClosingEventHandler DialogClosing
        {
            add => AddHandler(DialogClosingEvent, value);
            remove => RemoveHandler(DialogClosingEvent, value);
        }
        #endregion

        #region DialogClosed RoutedEvent
        public static readonly RoutedEvent DialogClosedEvent = EventManager.RegisterRoutedEvent("DialogClosed",
            RoutingStrategy.Bubble,
            typeof(DialogClosedEventHandler),
            typeof(DialogHost));

        public event DialogClosedEventHandler DialogClosed
        {
            add => AddHandler(DialogClosedEvent, value);
            remove => RemoveHandler(DialogClosedEvent, value);
        }
        #endregion

        #region Id DependencyProperty
        public static readonly DependencyProperty IdProperty = DependencyProperty.Register("Id",
            typeof(object),
            typeof(DialogHost),
            new PropertyMetadata(null));

        public object Id
        {
            get { return GetValue(IdProperty); }
            set { SetValue(IdProperty, value); }
        }
        #endregion

        #region DialogContent DependencyProperty
        public static readonly DependencyProperty DialogContentProperty = DependencyProperty.Register("DialogContent",
            typeof(object),
            typeof(DialogHost),
            new PropertyMetadata(null));

        public object DialogContent
        {
            get { return GetValue(DialogContentProperty); }
            set { SetValue(DialogContentProperty, value); }
        }
        #endregion

        #region DialogContentTemplate DependencyProperty
        public static readonly DependencyProperty DialogContentTemplateProperty = DependencyProperty.Register("DialogContentTemplate",
            typeof(DataTemplate),
            typeof(DialogHost),
            new PropertyMetadata(null));

        public DataTemplate DialogContentTemplate
        {
            get { return (DataTemplate)GetValue(DialogContentTemplateProperty); }
            set { SetValue(DialogContentTemplateProperty, value); }
        }
        #endregion

        #region DialogContentTemplateSelector DependencyProperty
        public static readonly DependencyProperty DialogContentTemplateSelectorProperty = DependencyProperty.Register("DialogContentTemplateSelector",
            typeof(DataTemplateSelector),
            typeof(DialogHost),
            new PropertyMetadata(null));

        public DataTemplateSelector DialogContentTemplateSelector
        {
            get { return (DataTemplateSelector)GetValue(DialogContentTemplateSelectorProperty); }
            set { SetValue(DialogContentTemplateSelectorProperty, value); }
        }
        #endregion

        #region DialogMargin DependencyProperty
        public static readonly DependencyProperty DialogMarginProperty = DependencyProperty.Register("DialogMargin",
            typeof(Thickness),
            typeof(DialogHost),
            new PropertyMetadata(default(Thickness)));

        public Thickness DialogMargin
        {
            get { return (Thickness)GetValue(DialogMarginProperty); }
            set { SetValue(DialogMarginProperty, value); }
        }
        #endregion

        #region DialogBackground DependencyProperty
        public static readonly DependencyProperty DialogBackgroundProperty = DependencyProperty.Register("DialogBackground",
            typeof(Brush),
            typeof(DialogHost),
            new PropertyMetadata(null));

        public Brush DialogBackground
        {
            get { return (Brush)GetValue(DialogBackgroundProperty); }
            set { SetValue(DialogBackgroundProperty, value); }
        }
        #endregion

        #region DialogBorderBrush DependencyProperty
        public static readonly DependencyProperty DialogBorderBrushProperty = DependencyProperty.Register("DialogBorderBrush",
            typeof(Brush),
            typeof(DialogHost),
            new PropertyMetadata(null));

        public Brush DialogBorderBrush
        {
            get { return (Brush)GetValue(DialogBorderBrushProperty); }
            set { SetValue(DialogBorderBrushProperty, value); }
        }
        #endregion

        #region DialogBorderThickness DependencyProperty
        public static readonly DependencyProperty DialogBorderThicknessProperty = DependencyProperty.Register("DialogBorderThickness",
            typeof(Thickness),
            typeof(DialogHost),
            new PropertyMetadata(default(Thickness)));

        public Thickness DialogBorderThickness
        {
            get { return (Thickness)GetValue(DialogBorderThicknessProperty); }
            set { SetValue(DialogBorderThicknessProperty, value); }
        }
        #endregion

        #region DialogCornerRadius DependencyProperty
        public static readonly DependencyProperty DialogCornerRadiusProperty = DependencyProperty.Register("DialogCornerRadius",
            typeof(CornerRadius),
            typeof(DialogHost),
            new PropertyMetadata(default(CornerRadius)));

        public CornerRadius DialogCornerRadius
        {
            get { return (CornerRadius)GetValue(DialogCornerRadiusProperty); }
            set { SetValue(DialogCornerRadiusProperty, value); }
        }
        #endregion

        #region IsDialogOpen DependencyProperty
        public static readonly DependencyProperty IsDialogOpenProperty = DependencyProperty.Register("IsDialogOpen",
            typeof(bool),
            typeof(DialogHost),
            new FrameworkPropertyMetadata(BooleanBoxes.FalseBox, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnIsDialogOpenChanged));

        private static void OnIsDialogOpenChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            var instance = (DialogHost)sender;

            instance.UpdateVisualState();

            if (instance.IsDialogOpen)
            {
                instance.CurrentHandle = new DialogHandle(instance);

                // Fenster rausfinden in dem sich der DialogHost befindet
                var window = Window.GetWindow(instance);
                // Aktuelles Element mit Focus merken für später
                instance._restoreFocusDialogClose = window != null ? FocusManager.GetFocusedElement(window) : null;

                var dialogOpenedEventArgs = new DialogOpenedEventArgs(DialogOpenedEvent, instance.CurrentHandle);
                // Event auslösen
                instance.OnDialogOpened(dialogOpenedEventArgs);
                instance.DialogOpenedCallback?.Invoke(instance, dialogOpenedEventArgs);
                instance._dialogOpenedEventHandler?.Invoke(instance, dialogOpenedEventArgs);

                instance.Dispatcher.BeginInvoke(DispatcherPriority.Background, new Action(() =>
                {
                    CommandManager.InvalidateRequerySuggested();
                }));
            }
            else
            {
                instance._dialogClosingEventHandler = null;

                object resultValue = null;

                if (instance.CurrentHandle != null)
                {
                    if (!instance.CurrentHandle.IsClosed)
                    {
                        instance.CurrentHandle.Close(instance.CurrentHandle.Value);
                    }
                    // Wenn wir hier angelangt sind, ist es nicht mehr zulässig den Vorgang abzubrechen
                    if (!instance.CurrentHandle.IsClosed)
                    {
                        throw new InvalidOperationException($"Cannot cancel dialog closing after {nameof(IsDialogOpen)} has been set");
                    }
                    resultValue = instance.CurrentHandle.Value;
                    instance.CurrentHandle = null;
                }

                instance._dialogTaskCompletionSource?.TrySetResult(resultValue);

                var dialogClosedEventArgs = new DialogClosedEventArgs(DialogClosedEvent, resultValue, instance.DialogContent);
                // Event auslösen
                instance.OnDialogClosed(dialogClosedEventArgs);
                instance.DialogClosedCallback?.Invoke(instance, dialogClosedEventArgs);
                instance._dialogClosedEventHandler?.Invoke(instance, dialogClosedEventArgs);

                instance._dialogClosedEventHandler = null;

                // Focus wiederherstellen
                instance.Dispatcher.InvokeAsync(() => instance._restoreFocusDialogClose?.Focus(), DispatcherPriority.Input);
            }
        }

        public bool IsDialogOpen
        {
            get { return (bool)GetValue(IsDialogOpenProperty); }
            set { SetValue(IsDialogOpenProperty, BooleanBoxes.Box(value)); }
        }
        #endregion

        #region CloseOnOverlayClick DependencyProperty
        public static readonly DependencyProperty CloseOnOverlayClickProperty = DependencyProperty.Register("CloseOnOverlayClick",
            typeof(bool),
            typeof(DialogHost),
            new PropertyMetadata(BooleanBoxes.FalseBox));

        public bool CloseOnOverlayClick
        {
            get { return (bool)GetValue(CloseOnOverlayClickProperty); }
            set { SetValue(CloseOnOverlayClickProperty, BooleanBoxes.Box(value)); }
        }
        #endregion

        #region CloseOnOverlayClickParameter DependencyProperty
        public static readonly DependencyProperty CloseOnOverlayClickParameterProperty = DependencyProperty.Register("CloseOnOverlayClickParameter",
            typeof(object),
            typeof(DialogHost),
            new PropertyMetadata(null));

        public object CloseOnOverlayClickParameter
        {
            get { return GetValue(CloseOnOverlayClickParameterProperty); }
            set { SetValue(CloseOnOverlayClickParameterProperty, value); }
        }
        #endregion

        #region OverlayBackground DependencyProperty
        public static readonly DependencyProperty OverlayBackgroundProperty = DependencyProperty.Register("OverlayBackground",
            typeof(Brush),
            typeof(DialogHost),
            new PropertyMetadata(Brushes.Black));

        public Brush OverlayBackground
        {
            get { return (Brush)GetValue(OverlayBackgroundProperty); }
            set { SetValue(OverlayBackgroundProperty, value); }
        }
        #endregion

        #region DialogHostOpenCommandContextSource DependencyProperty
        public static readonly DependencyProperty DialogHostOpenCommandContextSourceProperty = DependencyProperty.Register("DialogHostOpenCommandContextSource",
            typeof(DialogHostOpenCommandContextSource),
            typeof(DialogHost),
            new PropertyMetadata(DialogHostOpenCommandContextSource.Sender));

        public DialogHostOpenCommandContextSource DialogHostOpenCommandContextSource
        {
            get { return (DialogHostOpenCommandContextSource)GetValue(DialogHostOpenCommandContextSourceProperty); }
            set { SetValue(DialogHostOpenCommandContextSourceProperty, value); }
        }
        #endregion

        #region DialogOpenedCallback Attached DependencyProperty
        public static readonly DependencyProperty DialogOpenedCallbackProperty = DependencyProperty.RegisterAttached("DialogOpenedCallback",
            typeof(DialogOpenedEventHandler),
            typeof(DialogHost),
            new PropertyMetadata(default(DialogOpenedEventHandler)));

        public static DialogOpenedEventHandler GetDialogOpenedCallback(DependencyObject element)
        {
            return (DialogOpenedEventHandler)element.GetValue(DialogOpenedCallbackProperty);
        }

        public static void SetDialogOpenedCallback(DependencyObject element, DialogOpenedEventHandler value)
        {
            element.SetValue(DialogOpenedCallbackProperty, value);
        }

        public DialogOpenedEventHandler DialogOpenedCallback
        {
            get { return (DialogOpenedEventHandler)GetValue(DialogOpenedCallbackProperty); }
            set { SetValue(DialogOpenedCallbackProperty, value); }
        }
        #endregion

        #region DialogClosingCallback Attached DependencyProperty
        public static readonly DependencyProperty DialogClosingCallbackProperty = DependencyProperty.RegisterAttached("DialogClosingCallback",
            typeof(DialogClosingEventHandler),
            typeof(DialogHost),
            new PropertyMetadata(default(DialogClosingEventHandler)));

        public static DialogClosingEventHandler GetDialogClosingCallback(DependencyObject element)
        {
            return (DialogClosingEventHandler)element.GetValue(DialogClosingCallbackProperty);
        }

        public static void SetDialogClosingCallback(DependencyObject element, DialogClosingEventHandler value)
        {
            element.SetValue(DialogClosingCallbackProperty, value);
        }

        public DialogClosingEventHandler DialogClosingCallback
        {
            get { return (DialogClosingEventHandler)GetValue(DialogOpenedCallbackProperty); }
            set { SetValue(DialogOpenedCallbackProperty, value); }
        }
        #endregion

        #region DialogClosedCallback Attached DependencyProperty
        public static readonly DependencyProperty DialogClosedCallbackProperty = DependencyProperty.RegisterAttached("DialogClosedCallback",
            typeof(DialogClosedEventHandler),
            typeof(DialogHost),
            new PropertyMetadata(default(DialogClosedEventHandler)));

        public static DialogClosedEventHandler GetDialogClosedCallback(DependencyObject element)
        {
            return (DialogClosedEventHandler)element.GetValue(DialogClosedCallbackProperty);
        }

        public static void SetDialogClosedCallback(DependencyObject element, DialogClosingEventHandler value)
        {
            element.SetValue(DialogClosedCallbackProperty, value);
        }

        public DialogClosedEventHandler DialogClosedCallback
        {
            get { return (DialogClosedEventHandler)GetValue(DialogOpenedCallbackProperty); }
            set { SetValue(DialogOpenedCallbackProperty, value); }
        }
        #endregion

        private UIElement _overlayElement;
        private FrameworkElement _dialogContentElement;
        private IInputElement _restoreFocusDialogClose;

        private TaskCompletionSource<object> _dialogTaskCompletionSource;
        private DialogOpenedEventHandler _dialogOpenedEventHandler;
        private DialogClosingEventHandler _dialogClosingEventHandler;
        private DialogClosedEventHandler _dialogClosedEventHandler;

        private static readonly HashSet<WeakReference<DialogHost>> _loadedInstances = new HashSet<WeakReference<DialogHost>>();

        public static RoutedCommand OpenDialog { get; private set; }
        public static RoutedCommand CloseDialog { get; private set; }

        public DialogHandle CurrentHandle { get; private set; }

        private static void InitializeCommands()
        {
            var type = typeof(DialogHost);

            OpenDialog = new RoutedCommand(nameof(OpenDialog), type);
            CloseDialog = new RoutedCommand(nameof(CloseDialog), type);

            CommandManager.RegisterClassCommandBinding(type, new CommandBinding(OpenDialog, OnOpenDialogCommand));
            CommandManager.RegisterClassCommandBinding(type, new CommandBinding(CloseDialog, OnCloseDialogCommand, CanExecuteCloseDialogCommand));
        }

        private static void OnOpenDialogCommand(object sender, ExecutedRoutedEventArgs e)
        {
            var instance = (DialogHost)sender;

            if (e.Handled) return;

            if (e.OriginalSource is DependencyObject dependencyObject)
            {
                // Falls ein Element welches das Command ausgelöst hat Callbacks angehangen bekommen hat, diese holen
                instance._dialogOpenedEventHandler = GetDialogOpenedCallback(dependencyObject);
                instance._dialogClosingEventHandler = GetDialogClosingCallback(dependencyObject);
                instance._dialogClosedEventHandler = GetDialogClosedCallback(dependencyObject);
            }

            // Wurde ein Parameter mit übergeben?
            if (e.Parameter != null)
            {
                if (instance._dialogContentElement != null)
                {
                    switch (instance.DialogHostOpenCommandContextSource)
                    {
                        case DialogHostOpenCommandContextSource.Sender:
                        {
                            instance._dialogContentElement.DataContext = (e.OriginalSource as FrameworkElement)?.DataContext;
                            break;
                        }
                        case DialogHostOpenCommandContextSource.DialogHost:
                        {
                            instance._dialogContentElement.DataContext = instance.DataContext;
                            break;
                        }
                        case DialogHostOpenCommandContextSource.None:
                        {
                            instance._dialogContentElement.DataContext = null;
                            break;
                        }
                    }
                }
                // Parameter als DialogContent nutzen
                instance.DialogContent = e.Parameter;
            }

            instance.SetCurrentValue(IsDialogOpenProperty, true);

            e.Handled = true;
        }

        private static void OnCloseDialogCommand(object sender, ExecutedRoutedEventArgs e)
        {
            var instance = (DialogHost)sender;

            if (e.Handled) return;

            instance.CloseDialogInternal(e.Parameter);

            e.Handled = true;
        }

        private static void CanExecuteCloseDialogCommand(object sender, CanExecuteRoutedEventArgs e)
        {
            var instance = (DialogHost)sender;

            e.CanExecute = instance.CurrentHandle != null;
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            var instances = _loadedInstances.ToList();

            for (var i = 0; i < instances.Count; i++)
            {
                var weakReference = instances[i];

                if (!weakReference.TryGetTarget(out var dialogHost)) _loadedInstances.Remove(weakReference);

                if (Equals(dialogHost == this)) return;
            }

            _loadedInstances.Add(new WeakReference<DialogHost>(this));
        }

        private void OnUnloaded(object sender, RoutedEventArgs e)
        {
            var instances = _loadedInstances.ToList();

            for (var i = 0; i < instances.Count; i++)
            {
                var weakReference = instances[i];

                if (!weakReference.TryGetTarget(out var dialogHost) || Equals(dialogHost, this))
                {
                    _loadedInstances.Remove(weakReference);
                }
            }
        }

        private static DialogHost GetInstanceById(object dialogId)
        {
            if (_loadedInstances.Count == 0) return null;

            DialogHost result = null;

            var instances = _loadedInstances.ToList();

            for (var i = 0; i < instances.Count; i++)
            {
                var instance = instances[i];

                if (instance.TryGetTarget(out var dialogInstance))
                {
                    dialogInstance.Dispatcher.VerifyAccess();

                    if (Equals(dialogId, dialogInstance.Id))
                    {
                        result = dialogInstance;
                        break;
                    }
                }
                else _loadedInstances.Remove(instance);
            }

            return result;
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            UpdateVisualState(false);

            if (_overlayElement != null) _overlayElement.MouseLeftButtonUp -= OverlayElement_MouseLeftButtonUp;

            _overlayElement = GetTemplateChild("PART_OverlayPanel") as UIElement;
            _dialogContentElement = GetTemplateChild("PART_DialogContentElement") as FrameworkElement;

            if (_overlayElement != null) _overlayElement.MouseLeftButtonUp += OverlayElement_MouseLeftButtonUp;
        }

        private void OverlayElement_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (CloseOnOverlayClick && CurrentHandle != null && !CurrentHandle.IsClosed) CloseDialogInternal(CloseOnOverlayClickParameter);
        }

        internal void FocusDialog()
        {
            if (_dialogContentElement == null) return;

            CommandManager.InvalidateRequerySuggested();

            var focusableElement = _dialogContentElement.ChildrenOfType<UIElement>().FirstOrDefault(x => x.Focusable && x.IsVisible);

            focusableElement?.Dispatcher.InvokeAsync(() =>
            {
                if (!focusableElement.Focus()) return;

                focusableElement.MoveFocus(new TraversalRequest(FocusNavigationDirection.First));
            }, DispatcherPriority.Background);
        }

        internal async Task<object> ShowInternal(object content, DialogOpenedEventHandler openedEventHandler, DialogClosingEventHandler closingEventHandler, DialogClosedEventHandler closedEventHandler)
        {
            if (IsDialogOpen) throw new InvalidOperationException("DialogHost is already open.");

            _dialogTaskCompletionSource = new TaskCompletionSource<object>();

            if (content != null) DialogContent = content;

            _dialogOpenedEventHandler = openedEventHandler;
            _dialogClosingEventHandler = closingEventHandler;
            _dialogClosedEventHandler = closedEventHandler;
            SetCurrentValue(IsDialogOpenProperty, true);

            var result = await _dialogTaskCompletionSource.Task;

            _dialogOpenedEventHandler = null;
            _dialogClosingEventHandler = null;
            _dialogClosedEventHandler = null;

            return result;
        }

        internal void CloseDialogInternal(object result)
        {
            var handle = CurrentHandle;

            if (handle == null) return;

            handle.Value = result;
            handle.IsClosed = true;

            var dialogClosingEventArgs = new DialogClosingEventArgs(DialogClosingEvent, handle);

            OnDialogClosing(dialogClosingEventArgs);
            DialogClosingCallback?.Invoke(this, dialogClosingEventArgs);
            _dialogClosingEventHandler?.Invoke(this, dialogClosingEventArgs);

            if (dialogClosingEventArgs.Cancel)
            {
                handle.IsClosed = false;
                return;
            }

            SetCurrentValue(IsDialogOpenProperty, false);
        }

        protected virtual void OnDialogOpened(DialogOpenedEventArgs eventArgs)
        {
            RaiseEvent(eventArgs);
        }

        protected virtual void OnDialogClosing(DialogClosingEventArgs eventArgs)
        {
            RaiseEvent(eventArgs);
        }

        protected virtual void OnDialogClosed(DialogClosedEventArgs eventArgs)
        {
            RaiseEvent(eventArgs);
        }

        protected virtual void UpdateVisualState()
        {
            UpdateVisualState(true);
        }

        protected virtual void UpdateVisualState(bool useTransitions)
        {
            if (IsDialogOpen)
            {
                VisualStateManager.GoToState(this, "Open", useTransitions);
            }
            else
            {
                VisualStateManager.GoToState(this, "Closed", useTransitions);
            }
        }

        public static async Task<object> Show(object content)
        {
            return await Show(content, null, null, null, null);
        }

        public static async Task<object> Show(object content, DialogOpenedEventHandler openedEventHandler)
        {
            return await Show(content, null, openedEventHandler, null, null);
        }

        public static async Task<object> Show(object content, DialogClosingEventHandler closingEventHandler)
        {
            return await Show(content, null, null, closingEventHandler, null);
        }

        public static async Task<object> Show(object content, DialogClosedEventHandler closedEventHandler)
        {
            return await Show(content, null, null, null, closedEventHandler);
        }

        public static async Task<object> Show(object content, DialogOpenedEventHandler openedEventHandler, DialogClosingEventHandler closingEventHandler)
        {
            return await Show(content, null, openedEventHandler, closingEventHandler, null);
        }

        public static async Task<object> Show(object content, DialogOpenedEventHandler openedEventHandler, DialogClosedEventHandler closedEventHandler)
        {
            return await Show(content, null, openedEventHandler, null, closedEventHandler);
        }

        public static async Task<object> Show(object content, DialogClosingEventHandler closingEventHandler, DialogClosedEventHandler closedEventHandler)
        {
            return await Show(content, null, null, closingEventHandler, closedEventHandler);
        }

        public static async Task<object> Show(object content, DialogOpenedEventHandler openedEventHandler, DialogClosingEventHandler closingEventHandler, DialogClosedEventHandler closedEventHandler)
        {
            return await Show(content, null, openedEventHandler, closingEventHandler, closedEventHandler);
        }

        public static async Task<object> Show(object content, object hostId)
        {
            return await Show(content, hostId, null, null, null);
        }

        public static Task<object> Show(object content, object hostId, DialogOpenedEventHandler openedEventHandler)
        {
            return Show(content, hostId, openedEventHandler, null, null);
        }

        public static Task<object> Show(object content, object hostId, DialogClosingEventHandler closingEventHandler)
        {
            return Show(content, hostId, null, closingEventHandler, null);
        }

        public static Task<object> Show(object content, object hostId, DialogClosedEventHandler closedEventHandler)
        {
            return Show(content, hostId, null, null, closedEventHandler);
        }

        public static async Task<object> Show(object content, object hostId, DialogOpenedEventHandler openedEventHandler, DialogClosingEventHandler closingEventHandler)
        {
            return await Show(content, hostId, openedEventHandler, closingEventHandler, null);
        }

        public static async Task<object> Show(object content, object hostId, DialogOpenedEventHandler openedEventHandler, DialogClosedEventHandler closedEventHandler)
        {
            return await Show(content, hostId, openedEventHandler, null, closedEventHandler);
        }

        public static async Task<object> Show(object content, object hostId, DialogClosingEventHandler closingEventHandler, DialogClosedEventHandler closedEventHandler)
        {
            return await Show(content, hostId, null, closingEventHandler, closedEventHandler);
        }

        public static async Task<object> Show(object content, object hostId, DialogOpenedEventHandler openedEventHandler, DialogClosingEventHandler closingEventHandler, DialogClosedEventHandler closedEventHandler)
        {
            if (content == null) throw new ArgumentNullException(nameof(content));

            return await GetInstanceById(hostId)?.ShowInternal(content, openedEventHandler, closingEventHandler, closedEventHandler);
        }

        public static void Close(object hostId)
        {
            Close(hostId, null);
        }

        public static void Close(object hostId, object resultValue)
        {
            var dialogHost = GetInstanceById(hostId);

            if (dialogHost.CurrentHandle != null && !dialogHost.CurrentHandle.IsClosed)
            {
                dialogHost.CurrentHandle.Close(resultValue);
            }
        }

        public static DialogHandle GetDialogHandle(object hostId)
        {
            var dialogHost = GetInstanceById(hostId);

            return dialogHost.CurrentHandle;
        }

        public static bool HasOpenDialog(object hostId)
        {
            return GetDialogHandle(hostId)?.IsClosed == false;
        }
    }
}