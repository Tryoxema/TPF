using System;
using System.Windows.Threading;

namespace TPF.Demo.Views
{
    public partial class ButtonsDemoView : ViewBase
    {
        public ButtonsDemoView()
        {
            InitializeComponent();

            DisplayTimer.Tick += (s, e) =>
            {
                if (Progress >= 100.0)
                {
                    Progress = 0.0;
                    DisplayTimer.Stop();
                }
                else Progress++;
            };
        }

        private readonly DispatcherTimer DisplayTimer = new DispatcherTimer();

        double _progress;
        public double Progress
        {
            get { return _progress; }
            set { SetProperty(ref _progress, value); }
        }

        private void ProgressButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            Progress = 0;
            DisplayTimer.Interval = TimeSpan.FromMilliseconds(40);
            DisplayTimer.Start();
        }
    }
}