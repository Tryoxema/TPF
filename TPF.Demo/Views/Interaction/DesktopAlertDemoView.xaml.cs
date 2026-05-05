using System;
using System.Collections.ObjectModel;
using System.Windows;
using TPF.Controls;

namespace TPF.Demo.Views
{
    public partial class DesktopAlertDemoView : ViewBase
    {
        public DesktopAlertDemoView()
        {
            InitializeComponent();

            _topLeftManager = new DesktopAlertManager(DesktopAlertPosition.TopLeft);
            _topCenterManager = new DesktopAlertManager(DesktopAlertPosition.TopCenter);
            _topRightManager = new DesktopAlertManager(DesktopAlertPosition.TopRight);
            _bottomLeftManager = new DesktopAlertManager(DesktopAlertPosition.BottomLeft);
            _bottomCenterManager = new DesktopAlertManager(DesktopAlertPosition.BottomCenter);
            _bottomRightManager = new DesktopAlertManager(DesktopAlertPosition.BottomRight);

            DesktopAlertPositions.Add(DesktopAlertPosition.TopLeft);
            DesktopAlertPositions.Add(DesktopAlertPosition.TopCenter);
            DesktopAlertPositions.Add(DesktopAlertPosition.TopRight);
            DesktopAlertPositions.Add(DesktopAlertPosition.BottomLeft);
            DesktopAlertPositions.Add(DesktopAlertPosition.BottomCenter);
            DesktopAlertPositions.Add(DesktopAlertPosition.BottomRight);
        }

        DesktopAlertManager _topLeftManager;
        DesktopAlertManager _topCenterManager;
        DesktopAlertManager _topRightManager;
        DesktopAlertManager _bottomLeftManager;
        DesktopAlertManager _bottomCenterManager;
        DesktopAlertManager _bottomRightManager;

        public ObservableCollection<DesktopAlertPosition> DesktopAlertPositions { get; } = new ObservableCollection<DesktopAlertPosition>();

        string _header;
        public string Header
        {
            get { return _header; }
            set { SetProperty(ref _header, value); }
        }

        string _message;
        public string Message
        {
            get { return _message; }
            set { SetProperty(ref _message, value); }
        }

        int? _duration;
        public int? Duration
        {
            get { return _duration; }
            set { SetProperty(ref _duration, value); }
        }

        DesktopAlertPosition _position = DesktopAlertPosition.BottomRight;
        public DesktopAlertPosition Position
        {
            get { return _position; }
            set { SetProperty(ref _position, value); }
        }

        private void ShowAlertButton_Click(object sender, RoutedEventArgs e)
        {
            var config = new DesktopAlertConfig()
            {
                Header = Header,
                Content = Message,
            };

            if (Duration != null) config.Duration = TimeSpan.FromSeconds(Duration.Value);

            switch (Position)
            {
                case DesktopAlertPosition.TopLeft: _topLeftManager.ShowAlert(config); break;
                case DesktopAlertPosition.TopCenter: _topCenterManager.ShowAlert(config); break;
                case DesktopAlertPosition.TopRight: _topRightManager.ShowAlert(config); break;
                case DesktopAlertPosition.BottomLeft: _bottomLeftManager.ShowAlert(config); break;
                case DesktopAlertPosition.BottomCenter: _bottomCenterManager.ShowAlert(config); break;
                case DesktopAlertPosition.BottomRight: _bottomRightManager.ShowAlert(config); break;
            }
        }

        private void CloseAllButton_Click(object sender, RoutedEventArgs e)
        {
            _topLeftManager.CloseAll();
            _topCenterManager.CloseAll();
            _topRightManager.CloseAll();
            _bottomLeftManager.CloseAll();
            _bottomCenterManager.CloseAll();
            _bottomRightManager.CloseAll();
        }
    }
}