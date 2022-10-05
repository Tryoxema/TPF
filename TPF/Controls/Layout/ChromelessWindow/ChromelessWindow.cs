using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Data;
using System.Windows.Shell;
using System.Windows.Interop;
using TPF.Internal;
using TPF.Internal.Interop;
using System.Runtime.InteropServices;

namespace TPF.Controls
{
    public class ChromelessWindow : Window
    {
        static ChromelessWindow()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ChromelessWindow), new FrameworkPropertyMetadata(typeof(ChromelessWindow)));

            InitializeCommands();
        }

        public ChromelessWindow()
        {
            WindowStyle = WindowStyle.None;

            // Sämtliches Standard-Aussehen überschreiben
            var windowChrome = new WindowChrome()
            {
                CaptionHeight = 0,
                UseAeroCaptionButtons = false,
                GlassFrameThickness = default,
                CornerRadius = default
            };

            WindowChrome.SetWindowChrome(this, windowChrome);
        }

        #region BorderBrushWhenActive DependencyProperty
        public static readonly DependencyProperty BorderBrushWhenActiveProperty = DependencyProperty.Register("BorderBrushWhenActive",
            typeof(Brush),
            typeof(ChromelessWindow),
            new PropertyMetadata(null));

        public Brush BorderBrushWhenActive
        {
            get { return (Brush)GetValue(BorderBrushWhenActiveProperty); }
            set { SetValue(BorderBrushWhenActiveProperty, value); }
        }
        #endregion

        #region TitleBarHeight DependencyProperty
        public static readonly DependencyProperty TitleBarHeightProperty = DependencyProperty.Register("TitleBarHeight",
            typeof(double),
            typeof(ChromelessWindow),
            new PropertyMetadata(double.NaN));

        public double TitleBarHeight
        {
            get { return (double)GetValue(TitleBarHeightProperty); }
            set { SetValue(TitleBarHeightProperty, value); }
        }
        #endregion

        #region TitleBarForeground DependencyProperty
        public static readonly DependencyProperty TitleBarForegroundProperty = DependencyProperty.Register("TitleBarForeground",
            typeof(Brush),
            typeof(ChromelessWindow),
            new PropertyMetadata(null));

        public Brush TitleBarForeground
        {
            get { return (Brush)GetValue(TitleBarForegroundProperty); }
            set { SetValue(TitleBarForegroundProperty, value); }
        }
        #endregion

        #region TitleBarBackground DependencyProperty
        public static readonly DependencyProperty TitleBarBackgroundProperty = DependencyProperty.Register("TitleBarBackground",
            typeof(Brush),
            typeof(ChromelessWindow),
            new PropertyMetadata(null));

        public Brush TitleBarBackground
        {
            get { return (Brush)GetValue(TitleBarBackgroundProperty); }
            set { SetValue(TitleBarBackgroundProperty, value); }
        }
        #endregion

        #region TitleBarContextMenu DependencyProperty
        public static readonly DependencyProperty TitleBarContextMenuProperty = DependencyProperty.Register("TitleBarContextMenu",
            typeof(ContextMenu),
            typeof(ChromelessWindow),
            new PropertyMetadata(null));

        public ContextMenu TitleBarContextMenu
        {
            get { return (ContextMenu)GetValue(TitleBarContextMenuProperty); }
            set { SetValue(TitleBarContextMenuProperty, value); }
        }
        #endregion

        #region ShowIcon DependencyProperty
        public static readonly DependencyProperty ShowIconProperty = WindowTitleBar.ShowIconProperty.AddOwner(typeof(ChromelessWindow),
            new PropertyMetadata(BooleanBoxes.TrueBox));

        public bool ShowIcon
        {
            get { return (bool)GetValue(ShowIconProperty); }
            set { SetValue(ShowIconProperty, BooleanBoxes.Box(value)); }
        }
        #endregion

        #region IconMargin DependencyProperty
        public static readonly DependencyProperty IconMarginProperty = WindowTitleBar.IconMarginProperty.AddOwner(typeof(ChromelessWindow),
            new PropertyMetadata(default(Thickness)));

        public Thickness IconMargin
        {
            get { return (Thickness)GetValue(IconMarginProperty); }
            set { SetValue(IconMarginProperty, value); }
        }
        #endregion

        #region ShowTitle DependencyProperty
        public static readonly DependencyProperty ShowTitleProperty = WindowTitleBar.ShowTitleProperty.AddOwner(typeof(ChromelessWindow),
            new PropertyMetadata(BooleanBoxes.TrueBox));

        public bool ShowTitle
        {
            get { return (bool)GetValue(ShowTitleProperty); }
            set { SetValue(ShowTitleProperty, BooleanBoxes.Box(value)); }
        }
        #endregion

        #region TitleMargin DependencyProperty
        public static readonly DependencyProperty TitleMarginProperty = WindowTitleBar.TitleMarginProperty.AddOwner(typeof(ChromelessWindow),
            new PropertyMetadata(default(Thickness)));

        public Thickness TitleMargin
        {
            get { return (Thickness)GetValue(TitleMarginProperty); }
            set { SetValue(TitleMarginProperty, value); }
        }
        #endregion

        #region TitleAlignment DependencyProperty
        public static readonly DependencyProperty TitleAlignmentProperty = WindowTitleBar.TitleAlignmentProperty.AddOwner(typeof(ChromelessWindow),
            new PropertyMetadata(HorizontalAlignment.Left));

        public string TitleAlignment
        {
            get { return (string)GetValue(TitleAlignmentProperty); }
            set { SetValue(TitleAlignmentProperty, value); }
        }
        #endregion

        #region TitleFontFamily DependencyProperty
        public static readonly DependencyProperty TitleFontFamilyProperty = WindowTitleBar.TitleFontFamilyProperty.AddOwner(typeof(ChromelessWindow),
            new PropertyMetadata(SystemFonts.MessageFontFamily));

        public FontFamily TitleFontFamily
        {
            get { return (FontFamily)GetValue(TitleFontFamilyProperty); }
            set { SetValue(TitleFontFamilyProperty, value); }
        }
        #endregion

        #region TitleFontSize DependencyProperty
        public static readonly DependencyProperty TitleFontSizeProperty = WindowTitleBar.TitleFontSizeProperty.AddOwner(typeof(ChromelessWindow),
            new PropertyMetadata(SystemFonts.MessageFontSize));

        public double TitleFontSize
        {
            get { return (double)GetValue(TitleFontSizeProperty); }
            set { SetValue(TitleFontSizeProperty, value); }
        }
        #endregion

        #region MinimizeButtonStyle DependencyProperty
        public static readonly DependencyProperty MinimizeButtonStyleProperty = WindowTitleBar.MinimizeButtonStyleProperty.AddOwner(typeof(ChromelessWindow),
            new PropertyMetadata(null));

        public Style MinimizeButtonStyle
        {
            get { return (Style)GetValue(MinimizeButtonStyleProperty); }
            set { SetValue(MinimizeButtonStyleProperty, value); }
        }
        #endregion

        #region MaximizeButtonStyle DependencyProperty
        public static readonly DependencyProperty MaximizeButtonStyleProperty = WindowTitleBar.MaximizeButtonStyleProperty.AddOwner(typeof(ChromelessWindow),
            new PropertyMetadata(null));

        public Style MaximizeButtonStyle
        {
            get { return (Style)GetValue(MaximizeButtonStyleProperty); }
            set { SetValue(MaximizeButtonStyleProperty, value); }
        }
        #endregion

        #region RestoreButtonStyle DependencyProperty
        public static readonly DependencyProperty RestoreButtonStyleProperty = WindowTitleBar.RestoreButtonStyleProperty.AddOwner(typeof(ChromelessWindow),
            new PropertyMetadata(null));

        public Style RestoreButtonStyle
        {
            get { return (Style)GetValue(RestoreButtonStyleProperty); }
            set { SetValue(RestoreButtonStyleProperty, value); }
        }
        #endregion

        #region CloseButtonStyle DependencyProperty
        public static readonly DependencyProperty CloseButtonStyleProperty = WindowTitleBar.CloseButtonStyleProperty.AddOwner(typeof(ChromelessWindow),
            new PropertyMetadata(null));

        public Style CloseButtonStyle
        {
            get { return (Style)GetValue(CloseButtonStyleProperty); }
            set { SetValue(CloseButtonStyleProperty, value); }
        }
        #endregion

        #region ShowMinimizeButton DependencyProperty
        public static readonly DependencyProperty ShowMinimizeButtonProperty = WindowTitleBar.ShowMinimizeButtonProperty.AddOwner(typeof(ChromelessWindow),
            new PropertyMetadata(BooleanBoxes.TrueBox));

        public bool ShowMinimizeButton
        {
            get { return (bool)GetValue(ShowMinimizeButtonProperty); }
            set { SetValue(ShowMinimizeButtonProperty, BooleanBoxes.Box(value)); }
        }
        #endregion

        #region ShowMaximizeButton DependencyProperty
        public static readonly DependencyProperty ShowMaximizeButtonProperty = WindowTitleBar.ShowMaximizeButtonProperty.AddOwner(typeof(ChromelessWindow),
            new PropertyMetadata(BooleanBoxes.TrueBox));

        public bool ShowMaximizeButton
        {
            get { return (bool)GetValue(ShowMaximizeButtonProperty); }
            set { SetValue(ShowMaximizeButtonProperty, BooleanBoxes.Box(value)); }
        }
        #endregion

        #region ButtonsAreaMargin DependencyProperty
        public static readonly DependencyProperty ButtonsAreaMarginProperty = WindowTitleBar.ButtonsAreaMarginProperty.AddOwner(typeof(ChromelessWindow),
            new PropertyMetadata(default(Thickness)));

        public Thickness ButtonsAreaMargin
        {
            get { return (Thickness)GetValue(ButtonsAreaMarginProperty); }
            set { SetValue(ButtonsAreaMarginProperty, value); }
        }
        #endregion

        #region ExtendContentAreaIntoTitleBar DependencyProperty
        public static readonly DependencyProperty ExtendContentAreaIntoTitleBarProperty = WindowTitleBar.ExtendContentAreaIntoTitleBarProperty.AddOwner(typeof(ChromelessWindow),
            new PropertyMetadata(BooleanBoxes.FalseBox));

        public bool ExtendContentAreaIntoTitleBar
        {
            get { return (bool)GetValue(ExtendContentAreaIntoTitleBarProperty); }
            set { SetValue(ExtendContentAreaIntoTitleBarProperty, BooleanBoxes.Box(value)); }
        }
        #endregion

        #region LeftExtraTitleContent DependencyProperty
        public static readonly DependencyProperty LeftExtraTitleContentProperty = WindowTitleBar.LeftExtraTitleContentProperty.AddOwner(typeof(ChromelessWindow),
            new PropertyMetadata(null));

        public object LeftExtraTitleContent
        {
            get { return GetValue(LeftExtraTitleContentProperty); }
            set { SetValue(LeftExtraTitleContentProperty, value); }
        }
        #endregion

        #region LeftExtraTitleContentTemplate DependencyProperty
        public static readonly DependencyProperty LeftExtraTitleContentTemplateProperty = WindowTitleBar.LeftExtraTitleContentTemplateProperty.AddOwner(typeof(ChromelessWindow),
            new PropertyMetadata(null));

        public DataTemplate LeftExtraTitleContentTemplate
        {
            get { return (DataTemplate)GetValue(LeftExtraTitleContentTemplateProperty); }
            set { SetValue(LeftExtraTitleContentTemplateProperty, value); }
        }
        #endregion

        #region LeftExtraTitleContentTemplateSelector DependencyProperty
        public static readonly DependencyProperty LeftExtraTitleContentTemplateSelectorProperty = WindowTitleBar.LeftExtraTitleContentTemplateSelectorProperty.AddOwner(typeof(ChromelessWindow),
            new PropertyMetadata(null));

        public DataTemplate LeftExtraTitleContentTemplateSelector
        {
            get { return (DataTemplate)GetValue(LeftExtraTitleContentTemplateSelectorProperty); }
            set { SetValue(LeftExtraTitleContentTemplateSelectorProperty, value); }
        }
        #endregion

        #region RightExtraTitleContent DependencyProperty
        public static readonly DependencyProperty RightExtraTitleContentProperty = WindowTitleBar.RightExtraTitleContentProperty.AddOwner(typeof(ChromelessWindow),
            new PropertyMetadata(null));

        public object RightExtraTitleContent
        {
            get { return GetValue(RightExtraTitleContentProperty); }
            set { SetValue(RightExtraTitleContentProperty, value); }
        }
        #endregion

        #region RightExtraTitleContentTemplate DependencyProperty
        public static readonly DependencyProperty RightExtraTitleContentTemplateProperty = WindowTitleBar.RightExtraTitleContentTemplateProperty.AddOwner(typeof(ChromelessWindow),
            new PropertyMetadata(null));

        public DataTemplate RightExtraTitleContentTemplate
        {
            get { return (DataTemplate)GetValue(RightExtraTitleContentTemplateProperty); }
            set { SetValue(RightExtraTitleContentTemplateProperty, value); }
        }
        #endregion

        #region RightExtraTitleContentTemplateSelector DependencyProperty
        public static readonly DependencyProperty RightExtraTitleContentTemplateSelectorProperty = WindowTitleBar.RightExtraTitleContentTemplateSelectorProperty.AddOwner(typeof(ChromelessWindow),
            new PropertyMetadata(null));

        public DataTemplate RightExtraTitleContentTemplateSelector
        {
            get { return (DataTemplate)GetValue(RightExtraTitleContentTemplateSelectorProperty); }
            set { SetValue(RightExtraTitleContentTemplateSelectorProperty, value); }
        }
        #endregion

        #region CornerRadius DependencyProperty
        public static readonly DependencyProperty CornerRadiusProperty = DependencyProperty.Register("CornerRadius",
            typeof(CornerRadius),
            typeof(ChromelessWindow),
            new PropertyMetadata(default(CornerRadius)));

        public CornerRadius CornerRadius
        {
            get { return (CornerRadius)GetValue(CornerRadiusProperty); }
            set { SetValue(CornerRadiusProperty, value); }
        }
        #endregion

        public static RoutedUICommand MinimizeWindow { get; private set; }
        public static RoutedUICommand MaximizeWindow { get; private set; }
        public static RoutedUICommand RestoreWindow { get; private set; }
        public static RoutedUICommand CloseWindow { get; private set; }

        internal WindowTitleBar TitleBar;

        private IntPtr _hwnd;
        private HwndSource _hwndSource;
        private bool _hooked;
        private const int MONITOR_DEFAULTTONEAREST = 0x00000002;

        #region RoutedCommands
        private static void InitializeCommands()
        {
            var type = typeof(ChromelessWindow);

            MinimizeWindow = new RoutedUICommand("Minimize", nameof(MinimizeWindow), type);
            MaximizeWindow = new RoutedUICommand("Maximize", nameof(MaximizeWindow), type);
            RestoreWindow = new RoutedUICommand("Restore", nameof(RestoreWindow), type);
            CloseWindow = new RoutedUICommand("Close", nameof(CloseWindow), type);

            CommandManager.RegisterClassCommandBinding(type, new CommandBinding(MinimizeWindow, OnMinimizeWindowCommand, CanExecuteMinimizeWindowCommand));
            CommandManager.RegisterClassCommandBinding(type, new CommandBinding(MaximizeWindow, OnMaximizeWindowCommand, CanExecuteMaximizeWindowCommand));
            CommandManager.RegisterClassCommandBinding(type, new CommandBinding(RestoreWindow, OnRestoreWindowCommand, CanExecuteRestoreWindowCommand));
            CommandManager.RegisterClassCommandBinding(type, new CommandBinding(CloseWindow, OnCloseWindowCommand));
        }

        private static void OnMinimizeWindowCommand(object sender, ExecutedRoutedEventArgs e)
        {
            var instance = (ChromelessWindow)sender;

            if (e.Handled) return;

            instance.WindowState = WindowState.Minimized;
        }

        private static void OnMaximizeWindowCommand(object sender, ExecutedRoutedEventArgs e)
        {
            var instance = (ChromelessWindow)sender;

            if (e.Handled) return;

            instance.WindowState = WindowState.Maximized;
        }

        private static void OnRestoreWindowCommand(object sender, ExecutedRoutedEventArgs e)
        {
            var instance = (ChromelessWindow)sender;

            if (e.Handled) return;

            instance.WindowState = WindowState.Normal;
        }

        private static void OnCloseWindowCommand(object sender, ExecutedRoutedEventArgs e)
        {
            var instance = (ChromelessWindow)sender;

            if (e.Handled) return;

            instance.Close();
        }

        private static void CanExecuteMinimizeWindowCommand(object sender, CanExecuteRoutedEventArgs e)
        {
            var instance = (ChromelessWindow)sender;

            e.CanExecute = instance.WindowState != WindowState.Minimized && instance.ResizeMode != ResizeMode.NoResize;
        }

        private static void CanExecuteMaximizeWindowCommand(object sender, CanExecuteRoutedEventArgs e)
        {
            var instance = (ChromelessWindow)sender;

            e.CanExecute = instance.WindowState != WindowState.Maximized && instance.ResizeMode != ResizeMode.NoResize && instance.ResizeMode != ResizeMode.CanMinimize;
        }

        private static void CanExecuteRestoreWindowCommand(object sender, CanExecuteRoutedEventArgs e)
        {
            var instance = (ChromelessWindow)sender;

            e.CanExecute = instance.WindowState != WindowState.Normal && instance.ResizeMode != ResizeMode.NoResize && instance.ResizeMode != ResizeMode.CanMinimize;
        }
        #endregion

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            if (TitleBar != null)
            {
                UnhookTitleBar(TitleBar);
            }

            TitleBar = GetTemplateChild("PART_TitleBar") as WindowTitleBar;

            if (TitleBar != null)
            {
                HookupTitleBar(TitleBar, this);
            }
        }

        private static void HookupTitleBar(WindowTitleBar titleBar, ChromelessWindow window)
        {
            titleBar.ParentWindow = window;

            titleBar.SetBinding(WindowStateProperty, new Binding("WindowState") { Source = window });
            titleBar.SetBinding(ResizeModeProperty, new Binding("ResizeMode") { Source = window });
            titleBar.SetBinding(HeightProperty, new Binding("TitleBarHeight") { Source = window });
            titleBar.SetBinding(ForegroundProperty, new Binding("TitleBarForeground") { Source = window });
            titleBar.SetBinding(BackgroundProperty, new Binding("TitleBarBackground") { Source = window });
            titleBar.SetBinding(ContextMenuProperty, new Binding("TitleBarContextMenu") { Source = window });
            titleBar.SetBinding(WindowTitleBar.IconProperty, new Binding("Icon") { Source = window });
            titleBar.SetBinding(WindowTitleBar.ShowIconProperty, new Binding("ShowIcon") { Source = window });
            titleBar.SetBinding(WindowTitleBar.IconMarginProperty, new Binding("IconMargin") { Source = window });
            titleBar.SetBinding(WindowTitleBar.TitleProperty, new Binding("Title") { Source = window });
            titleBar.SetBinding(WindowTitleBar.ShowTitleProperty, new Binding("ShowTitle") { Source = window });
            titleBar.SetBinding(WindowTitleBar.TitleMarginProperty, new Binding("TitleMargin") { Source = window });
            titleBar.SetBinding(WindowTitleBar.TitleAlignmentProperty, new Binding("TitleAlignment") { Source = window });
            titleBar.SetBinding(WindowTitleBar.TitleFontFamilyProperty, new Binding("TitleFontFamily") { Source = window });
            titleBar.SetBinding(WindowTitleBar.TitleFontSizeProperty, new Binding("TitleFontSize") { Source = window });
            titleBar.SetBinding(WindowTitleBar.MinimizeButtonStyleProperty, new Binding("MinimizeButtonStyle") { Source = window });
            titleBar.SetBinding(WindowTitleBar.MaximizeButtonStyleProperty, new Binding("MaximizeButtonStyle") { Source = window });
            titleBar.SetBinding(WindowTitleBar.RestoreButtonStyleProperty, new Binding("RestoreButtonStyle") { Source = window });
            titleBar.SetBinding(WindowTitleBar.CloseButtonStyleProperty, new Binding("CloseButtonStyle") { Source = window });
            titleBar.SetBinding(WindowTitleBar.ShowMinimizeButtonProperty, new Binding("ShowMinimizeButton") { Source = window });
            titleBar.SetBinding(WindowTitleBar.ShowMaximizeButtonProperty, new Binding("ShowMaximizeButton") { Source = window });
            titleBar.SetBinding(WindowTitleBar.ButtonsAreaMarginProperty, new Binding("ButtonsAreaMargin") { Source = window });
            titleBar.SetBinding(WindowTitleBar.ExtendContentAreaIntoTitleBarProperty, new Binding("ExtendContentAreaIntoTitleBar") { Source = window });
            titleBar.SetBinding(WindowTitleBar.LeftExtraTitleContentProperty, new Binding("LeftExtraTitleContent") { Source = window });
            titleBar.SetBinding(WindowTitleBar.LeftExtraTitleContentTemplateProperty, new Binding("LeftExtraTitleContentTemplate") { Source = window });
            titleBar.SetBinding(WindowTitleBar.LeftExtraTitleContentTemplateSelectorProperty, new Binding("LeftExtraTitleContentTemplateSelector") { Source = window });
            titleBar.SetBinding(WindowTitleBar.RightExtraTitleContentProperty, new Binding("RightExtraTitleContent") { Source = window });
            titleBar.SetBinding(WindowTitleBar.RightExtraTitleContentTemplateProperty, new Binding("RightExtraTitleContentTemplate") { Source = window });
            titleBar.SetBinding(WindowTitleBar.RightExtraTitleContentTemplateSelectorProperty, new Binding("RightExtraTitleContentTemplateSelector") { Source = window });
            titleBar.SetBinding(WindowTitleBar.CornerRadiusProperty, new Binding("CornerRadius") { Source = window, Converter = new Converter.CornerRadiusToSpecificCornerRadiusConverter(), ConverterParameter = "#,#,0,0" });

            titleBar.MouseDown += TitleBar_MouseDown;
            titleBar.MouseMove += TitleBar_MouseMove;
        }

        private static void UnhookTitleBar(WindowTitleBar titleBar)
        {
            titleBar.ParentWindow = null;

            BindingOperations.ClearBinding(titleBar, WindowStateProperty);
            BindingOperations.ClearBinding(titleBar, ResizeModeProperty);
            BindingOperations.ClearBinding(titleBar, HeightProperty);
            BindingOperations.ClearBinding(titleBar, ForegroundProperty);
            BindingOperations.ClearBinding(titleBar, BackgroundProperty);
            BindingOperations.ClearBinding(titleBar, ContextMenuProperty);
            BindingOperations.ClearBinding(titleBar, WindowTitleBar.IconProperty);
            BindingOperations.ClearBinding(titleBar, WindowTitleBar.ShowIconProperty);
            BindingOperations.ClearBinding(titleBar, WindowTitleBar.IconMarginProperty);
            BindingOperations.ClearBinding(titleBar, WindowTitleBar.TitleProperty);
            BindingOperations.ClearBinding(titleBar, WindowTitleBar.ShowTitleProperty);
            BindingOperations.ClearBinding(titleBar, WindowTitleBar.TitleMarginProperty);
            BindingOperations.ClearBinding(titleBar, WindowTitleBar.TitleAlignmentProperty);
            BindingOperations.ClearBinding(titleBar, WindowTitleBar.TitleFontFamilyProperty);
            BindingOperations.ClearBinding(titleBar, WindowTitleBar.TitleFontSizeProperty);
            BindingOperations.ClearBinding(titleBar, WindowTitleBar.MinimizeButtonStyleProperty);
            BindingOperations.ClearBinding(titleBar, WindowTitleBar.MaximizeButtonStyleProperty);
            BindingOperations.ClearBinding(titleBar, WindowTitleBar.RestoreButtonStyleProperty);
            BindingOperations.ClearBinding(titleBar, WindowTitleBar.CloseButtonStyleProperty);
            BindingOperations.ClearBinding(titleBar, WindowTitleBar.ShowMinimizeButtonProperty);
            BindingOperations.ClearBinding(titleBar, WindowTitleBar.ShowMaximizeButtonProperty);
            BindingOperations.ClearBinding(titleBar, WindowTitleBar.ButtonsAreaMarginProperty);
            BindingOperations.ClearBinding(titleBar, WindowTitleBar.ExtendContentAreaIntoTitleBarProperty);
            BindingOperations.ClearBinding(titleBar, WindowTitleBar.LeftExtraTitleContentProperty);
            BindingOperations.ClearBinding(titleBar, WindowTitleBar.LeftExtraTitleContentTemplateProperty);
            BindingOperations.ClearBinding(titleBar, WindowTitleBar.LeftExtraTitleContentTemplateSelectorProperty);
            BindingOperations.ClearBinding(titleBar, WindowTitleBar.RightExtraTitleContentProperty);
            BindingOperations.ClearBinding(titleBar, WindowTitleBar.RightExtraTitleContentTemplateProperty);
            BindingOperations.ClearBinding(titleBar, WindowTitleBar.RightExtraTitleContentTemplateSelectorProperty);
            BindingOperations.ClearBinding(titleBar, WindowTitleBar.CornerRadiusProperty);

            titleBar.MouseDown -= TitleBar_MouseDown;
            titleBar.MouseMove -= TitleBar_MouseMove;
        }

        protected override void OnStateChanged(EventArgs e)
        {
            base.OnStateChanged(e);

            // Wenn über Drag nach oben das Fenster maximiert wurde, ist Top negativ wenn das Fenster über den RestoreButton zurückgesetzt wird
            // Damit das Fenster nicht falsch aussieht weil es zur Hälfte außerhalb des Bilds liegt, setzen wir Top dann auf 0
            if (WindowState == WindowState.Normal && Top < 0 && Top > 0 - TitleBarHeight)
            {
                Top = 0;
            }
        }

        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);

            if (_hooked)
            {
                _hwndSource.RemoveHook(WndProc);
                _hooked = false;
            }
        }

        protected override void OnSourceInitialized(EventArgs e)
        {
            base.OnSourceInitialized(e);

            _hwnd = new WindowInteropHelper(this).Handle;
            _hwndSource = HwndSource.FromHwnd(_hwnd);

            if (!_hooked)
            {
                _hwndSource.AddHook(WndProc);
                _hooked = true;
            }
        }

        private IntPtr WndProc(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
        {
            var message = (WM)msg;

            switch (message)
            {
                case WM.GETMINMAXINFO:
                {
                    GetMinMaxInfo(hwnd, lParam);
                    break;
                }
            }

            return IntPtr.Zero;
        }

        private static void GetMinMaxInfo(IntPtr hwnd, IntPtr lParam)
        {
            var monitorHandle = NativeMethods.MonitorFromWindow(hwnd, MONITOR_DEFAULTTONEAREST);

            var monitorInfo = NativeMethods.GetMonitorInfo(monitorHandle);

            var minMaxInfo = (Win32MinMaxInfo)Marshal.PtrToStructure(lParam, typeof(Win32MinMaxInfo));

            minMaxInfo.ptMaxPosition.X = Math.Abs(monitorInfo.rcWork.Left - monitorInfo.rcMonitor.Left);
            minMaxInfo.ptMaxPosition.Y = Math.Abs(monitorInfo.rcWork.Top - monitorInfo.rcMonitor.Top);
            minMaxInfo.ptMaxSize.X = Math.Abs(monitorInfo.rcWork.Right - monitorInfo.rcWork.Left);
            minMaxInfo.ptMaxSize.Y = Math.Abs(monitorInfo.rcWork.Bottom - monitorInfo.rcWork.Top);

            var windowPlacement = NativeMethods.GetWindowPlacement(hwnd);

            if (windowPlacement.showCmd == SW.SHOWMAXIMIZED)
            {
                minMaxInfo.ptMaxTrackSize.X = minMaxInfo.ptMaxSize.X;
                minMaxInfo.ptMaxTrackSize.Y = minMaxInfo.ptMaxSize.Y;
            }

            // Wenn die Taskleiste so eingestellt ist, dass sie sich ausblendet muss die MinMaxInfo angepasst werden
            if (IsTaskbarAutoHide(out var edge))
            {
                switch (edge)
                {
                    case ABEdge.LEFT:
                    {
                        minMaxInfo.ptMaxPosition.X = 2;
                        minMaxInfo.ptMaxSize.X -= 2;
                        break;
                    }
                    case ABEdge.TOP:
                    {
                        minMaxInfo.ptMaxPosition.Y = 2;
                        minMaxInfo.ptMaxSize.Y -= 2;
                        break;
                    }
                    case ABEdge.RIGHT:
                    {
                        minMaxInfo.ptMaxSize.X -= 2;
                        break;
                    }
                    case ABEdge.BOTTOM:
                    {
                        minMaxInfo.ptMaxSize.Y -= 2;
                        break;
                    }
                }
            }

            Marshal.StructureToPtr(minMaxInfo, lParam, true);
        }

        private static bool IsTaskbarAutoHide(out ABEdge edge)
        {
            var trayWnd = NativeMethods.FindWindow("Shell_TrayWnd", null);

            if (trayWnd != IntPtr.Zero)
            {
                var appBarData = new Win32AppBarData();
                appBarData.cbSize = Marshal.SizeOf(appBarData);
                appBarData.hWnd = trayWnd;

                NativeMethods.SHAppBarMessage(ABMsg.GETTASKBARPOS, ref appBarData);

                var autoHide = Convert.ToBoolean(NativeMethods.SHAppBarMessage(ABMsg.GETSTATE, ref appBarData));
                edge = autoHide ? GetEdge(appBarData.rc) : default;

                return autoHide;
            }

            edge = default;
            return false;
        }

        private static ABEdge GetEdge(Win32Rect rect)
        {
            if (rect.Top == rect.Left && rect.Bottom > rect.Right) return ABEdge.LEFT;
            else if (rect.Top == rect.Left && rect.Bottom < rect.Right) return ABEdge.TOP;
            else if (rect.Top > rect.Left) return ABEdge.BOTTOM;
            else return ABEdge.RIGHT;
        }

        private static void TitleBar_MouseDown(object sender, MouseButtonEventArgs e)
        {
            var instance = (WindowTitleBar)sender;

            var window = instance.ParentWindow;

            if (window == null) return;

            if (e.ChangedButton == MouseButton.Left)
            {
                if (e.ClickCount == 2) window.ToggleMaximizedState();
                else window.DragMove();
            }
        }

        private static void TitleBar_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton != MouseButtonState.Pressed) return;

            var instance = (WindowTitleBar)sender;

            var window = instance.ParentWindow;

            if (window == null) return;

            if (window.WindowState == WindowState.Maximized)
            {
                var position = e.GetPosition(instance);
                var absolutePosition = NativeMethods.GetCursorPosition();

                var restoreWidth = window.RestoreBounds.Width;
                var screenWidth = instance.ActualWidth;

                var x = position.X - restoreWidth / 2;

                if (x < 0) x = 0;
                else if (x + restoreWidth > screenWidth) x = screenWidth - restoreWidth;

                var top = absolutePosition.Y - position.Y + 3;
                var left = absolutePosition.X - position.X + x;

                window.WindowState = WindowState.Normal;
                window.Top = top;
                window.Left = left;
                window.DragMove();
            }
        }

        private void ToggleMaximizedState()
        {
            if (ResizeMode == ResizeMode.NoResize || ResizeMode == ResizeMode.CanMinimize) return;

            if (WindowState == WindowState.Maximized)
            {
                WindowState = WindowState.Normal;
            }
            else
            {
                WindowState = WindowState.Maximized;
            }
        }
    }
}