using System;
using System.Collections.Generic;
using System.Windows.Controls;
using System.Windows.Threading;

namespace TPF.Demo.Net461.Views
{
    public partial class RadialProgressBarView : UserControl, System.ComponentModel.INotifyPropertyChanged
    {
        public RadialProgressBarView()
        {
            InitializeComponent();

            DisplayTimer = new DispatcherTimer();
            DisplayTimer.Tick += DisplayTimer_Tick;
            DisplayTimer.Interval = TimeSpan.FromMilliseconds(40);
            DisplayTimer.Start();
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

        #region SetProperty
        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;

        bool SetProperty<T>(ref T field, T value, [System.Runtime.CompilerServices.CallerMemberName] string propertyName = null)
        {
            if (EqualityComparer<T>.Default.Equals(field, value)) return false;
            field = value;
            PropertyChanged?.Invoke(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            return true;
        }
        #endregion

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
    }
}