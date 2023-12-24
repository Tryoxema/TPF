using System;
using System.Threading;
using System.Windows;
using System.Windows.Data;
using System.Windows.Threading;
using TPF.Converter;

namespace TPF.Controls
{
    public static class SplashScreenManager
    {
        static SplashScreenManager()
        {
            ResetAll();
        }

        public static object DataContext { get; set; }

        public static bool AllowsTransparency { get; set; }

        internal static SplashScreenWindow Window { get; private set; }

        private static Thread _splashScreenThread;
        private static bool _abortCreation;

        public static bool IsOpen
        {
            get { return Window != null; }
        }

        public static SplashScreenData CreateDataContext()
        {
            return new SplashScreenData();
        }

        public static void ResetAll()
        {
            DataContext = CreateDataContext();
            AllowsTransparency = true;
        }

        public static void Show()
        {
            Show(typeof(SplashScreen));
        }

        public static void ShowCustom<T>() where T : FrameworkElement
        {
            Show(typeof(T));
        }

        public static void ShowCustom(Type controlType)
        {
            if (!controlType.IsAssignableFrom(typeof(FrameworkElement))) return;
            
            Show(controlType);
        }

        private static void Show(Type contentType)
        {
            if (_splashScreenThread != null || IsOpen) return;

            // Da wir kein Window als Content von einem Window anzeigen können, wird der Fall ignoriert
            if (contentType.IsAssignableFrom(typeof(Window))) return;

            _splashScreenThread = new Thread(() => CreateAndShowWindow(contentType));

            _splashScreenThread.SetApartmentState(ApartmentState.STA);
            _splashScreenThread.IsBackground = true;
            _splashScreenThread.Start();
        }

        private static void CreateAndShowWindow(Type contentType)
        {
            var window = new SplashScreenWindow()
            {
                DataContext = DataContext,
                AllowsTransparency = AllowsTransparency
            };

            var content = Activator.CreateInstance(contentType);

            window.Content = content;

            if (content is SplashScreen splashScreen && DataContext is SplashScreenData data)
            {
                splashScreen.SetBinding(SplashScreen.TitleProperty, new Binding("Title"));
                splashScreen.SetBinding(SplashScreen.SubTitleProperty, new Binding("SubTitle"));
                splashScreen.SetBinding(SplashScreen.StatusTextProperty, new Binding("StatusText"));
                splashScreen.SetBinding(SplashScreen.FooterProperty, new Binding("Footer"));
                splashScreen.SetBinding(SplashScreen.HorizontalStatusAlignmentProperty, new Binding("HorizontalStatusAlignment"));
                splashScreen.SetBinding(SplashScreen.HorizontalFooterAlignmentProperty, new Binding("HorizontalFooterAlignment"));
                splashScreen.SetBinding(SplashScreen.ProgressBarVisibilityProperty, new Binding("ShowProgressBar") { Converter = new BooleanToVisibilityConverter() });
                splashScreen.SetBinding(SplashScreen.ProgressProperty, new Binding("Progress"));
                splashScreen.SetBinding(SplashScreen.SecondaryProgressProperty, new Binding("SecondaryProgress"));
                splashScreen.SetBinding(SplashScreen.ProgressMinimumProperty, new Binding("ProgressMinimum"));
                splashScreen.SetBinding(SplashScreen.ProgressMaximumProperty, new Binding("ProgressMaximum"));
                splashScreen.SetBinding(SplashScreen.IsIndeterminateProperty, new Binding("IsIndeterminate"));
                splashScreen.SetBinding(SplashScreen.LogoProperty, new Binding("Logo"));
                splashScreen.SetBinding(SplashScreen.LogoWidthProperty, new Binding("LogoWidth"));
                splashScreen.SetBinding(SplashScreen.LogoHeightProperty, new Binding("LogoHeight"));
                splashScreen.SetBinding(SplashScreen.LogoStretchProperty, new Binding("LogoStretch"));
                splashScreen.SetBinding(SplashScreen.LogoPositionProperty, new Binding("LogoPosition"));
            }

            Window = window;
            if (_abortCreation)
            {
                _abortCreation = false;
                Window = null;
                _splashScreenThread = null;
            }
            else Window.Show();

            Dispatcher.Run();
        }

        public static void Close()
        {
            if (!IsOpen)
            {
                if (_splashScreenThread != null)
                {
                    _abortCreation = true;
                }
                return;
            }

            Window.Dispatcher.InvokeShutdown();
            Window = null;
            _splashScreenThread = null;
        }
    }
}