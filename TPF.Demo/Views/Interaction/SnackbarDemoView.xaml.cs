using System;
using System.Collections.ObjectModel;
using System.Windows;
using TPF.Controls;

namespace TPF.Demo.Views
{
    public partial class SnackbarDemoView : ViewBase
    {
        public SnackbarDemoView()
        {
            InitializeComponent();

            SnackbarSeverities.Add(SnackbarSeverity.None);
            SnackbarSeverities.Add(SnackbarSeverity.Info);
            SnackbarSeverities.Add(SnackbarSeverity.Success);
            SnackbarSeverities.Add(SnackbarSeverity.Warning);
            SnackbarSeverities.Add(SnackbarSeverity.Error);
        }

        public ObservableCollection<SnackbarSeverity> SnackbarSeverities { get; } = new ObservableCollection<SnackbarSeverity>();

        string _message;
        public string Message
        {
            get { return _message; }
            set { SetProperty(ref _message, value); }
        }

        string _actionText;
        public string ActionText
        {
            get { return _actionText; }
            set { SetProperty(ref _actionText, value); }
        }

        SnackbarSeverity _severity = SnackbarSeverity.None;
        public SnackbarSeverity Severity
        {
            get { return _severity; }
            set { SetProperty(ref _severity, value); }
        }

        int _duration = 3;
        public int Duration
        {
            get { return _duration; }
            set { SetProperty(ref _duration, value); }
        }

        private void EnqueueMessageButton_Click(object sender, RoutedEventArgs e)
        {
            DemoSnackbar.Enqueue(Message, Severity, TimeSpan.FromSeconds(Duration), ActionText);
        }

        private void EnqueuePriorityMessageButton_Click(object sender, RoutedEventArgs e)
        {
            DemoSnackbar.EnqueueWithPriority(Message, Severity, TimeSpan.FromSeconds(Duration), ActionText);
        }

        private void ClearButton_Click(object sender, RoutedEventArgs e)
        {
            var queue = DemoSnackbar.MessageQueue;

            queue.Clear();
        }
    }
}