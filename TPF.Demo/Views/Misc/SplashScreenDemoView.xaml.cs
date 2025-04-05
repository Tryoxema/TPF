using System.Windows;
using TPF.Controls;

namespace TPF.Demo.Views
{
    public partial class SplashScreenDemoView : ViewBase
    {
        public SplashScreenDemoView()
        {
            InitializeComponent();
        }

        private void ShowSplashScreenButton_Click(object sender, RoutedEventArgs e)
        {
            var data = SplashScreenManager.CreateDataContext();

            data.Title = "SplashScreen";
            data.SubTitle = "Jetzt neu";
            data.StatusText = "Anfrage wird ignoriert...";
            data.Footer = "Das liest doch eh keiner";

            SplashScreenManager.DataContext = data;
            SplashScreenManager.Show();
        }

        private void CloseSplashScreenButton_Click(object sender, RoutedEventArgs e)
        {
            SplashScreenManager.Close();
        }
    }
}