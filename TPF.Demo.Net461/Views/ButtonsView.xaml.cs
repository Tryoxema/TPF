﻿using System;
using System.Collections.Generic;
using System.Windows.Controls;
using System.Windows.Threading;

namespace TPF.Demo.Net461.Views
{
    public partial class ButtonsView : UserControl, System.ComponentModel.INotifyPropertyChanged
    {
        public ButtonsView()
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