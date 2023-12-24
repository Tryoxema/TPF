using System.Windows;

namespace TPF.Controls
{
    internal class SplashScreenWindow : Window
    {
        public SplashScreenWindow()
        {
            WindowStyle = WindowStyle.None;
            Topmost = true;
            ShowInTaskbar = false;
            WindowStartupLocation = WindowStartupLocation.CenterScreen;
            SizeToContent = SizeToContent.WidthAndHeight;
        }
    }
}