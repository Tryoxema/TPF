using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace TPF.Demo.Views
{
    public partial class BusyIndicatorDemoView : ViewBase
    {
        public BusyIndicatorDemoView()
        {
            InitializeComponent();
        }

        string _busyContent;
        public string BusyContent
        {
            get { return _busyContent; }
            set { SetProperty(ref _busyContent, value); }
        }

        private void ShowIndeterminateBusyIndicatorButton_Click(object sender, RoutedEventArgs e)
        {
            IndeterminateBusyIndicator.IsBusy = true;

            Task.Delay(TimeSpan.FromSeconds(5)).ContinueWith(r =>
            {
                IndeterminateBusyIndicator.IsBusy = false;
            }, TaskScheduler.FromCurrentSynchronizationContext());
        }

        private void ShowDeterminateBusyIndicatorButton_Click(object sender, RoutedEventArgs e)
        {
            DeterminateBusyIndicator.ProgressBarValue = 0;
            DeterminateBusyIndicator.IsBusy = true;

            Task.Run(() =>
            {
                for (int i = 0; i < 100; i++)
                {
                    Dispatcher.Invoke(() => DeterminateBusyIndicator.ProgressBarValue++);
                    Thread.Sleep(50);
                }
            }).ContinueWith(r =>
            {
                DeterminateBusyIndicator.IsBusy = false;
            }, TaskScheduler.FromCurrentSynchronizationContext());
        }
    }
}