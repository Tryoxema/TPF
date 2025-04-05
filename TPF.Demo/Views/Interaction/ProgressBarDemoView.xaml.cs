using System;
using System.Windows.Threading;

namespace TPF.Demo.Views
{
    public partial class ProgressBarDemoView : ViewBase
    {
        public ProgressBarDemoView()
        {
            InitializeComponent();

            DisplayTimer = new DispatcherTimer();
            DisplayTimer.Tick += DisplayTimer_Tick;
            DisplayTimer.Interval = TimeSpan.FromMilliseconds(40);
            DisplayTimer.Start();
        }

        private readonly DispatcherTimer DisplayTimer;

        double _progress;
        public double Progress
        {
            get { return _progress; }
            set { SetProperty(ref _progress, value); }
        }

        double _secondaryProgress;
        public double SecondaryProgress
        {
            get { return _secondaryProgress; }
            set { SetProperty(ref _secondaryProgress, value); }
        }

        private void DisplayTimer_Tick(object sender, EventArgs e)
        {
            if (Progress >= 100.0)
            {
                Progress = 0.0;
                SecondaryProgress = 0.0;
            }
            else Progress++;

            if (SecondaryProgress < 100.0) SecondaryProgress += 2;
        }
    }
}