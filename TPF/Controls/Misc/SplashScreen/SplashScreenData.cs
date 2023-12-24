using System;
using System.Windows;
using System.Windows.Media;

namespace TPF.Controls
{
    public class SplashScreenData : NotifyObject
    {
        string _title;
        public string Title
        {
            get { return _title; }
            set { SetProperty(ref _title, value); }
        }

        string _subTitle;
        public string SubTitle
        {
            get { return _subTitle; }
            set { SetProperty(ref _subTitle, value); }
        }

        string _statusText;
        public string StatusText
        {
            get { return _statusText; }
            set { SetProperty(ref _statusText, value); }
        }

        object _footer;
        public object Footer
        {
            get { return _footer; }
            set { SetProperty(ref _footer, value); }
        }

        HorizontalAlignment _horizontalStatusAlignment;
        public HorizontalAlignment HorizontalStatusAlignment
        {
            get { return _horizontalStatusAlignment; }
            set { SetProperty(ref _horizontalStatusAlignment, value); }
        }

        HorizontalAlignment _horizontalFooterAlignment;
        public HorizontalAlignment HorizontalFooterAlignment
        {
            get { return _horizontalFooterAlignment; }
            set { SetProperty(ref _horizontalFooterAlignment, value); }
        }

        bool _showProgressBar = true;
        public bool ShowProgressBar
        {
            get { return _showProgressBar; }
            set { SetProperty(ref _showProgressBar, value); }
        }

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

        double _progressMinimum;
        public double ProgressMinimum
        {
            get { return _progressMinimum; }
            set { SetProperty(ref _progressMinimum, value); }
        }

        double _progressMaximum = 100;
        public double ProgressMaximum
        {
            get { return _progressMaximum; }
            set { SetProperty(ref _progressMaximum, value); }
        }

        bool _isIndeterminate = true;
        public bool IsIndeterminate
        {
            get { return _isIndeterminate; }
            set { SetProperty(ref _isIndeterminate, value); }
        }

        Uri _logo;
        public Uri Logo
        {
            get { return _logo; }
            set { SetProperty(ref _logo, value); }
        }

        double _logoWidth = double.NaN;
        public double LogoWidth
        {
            get { return _logoWidth; }
            set { SetProperty(ref _logoWidth, value); }
        }

        double _logoHeight = double.NaN;
        public double LogoHeight
        {
            get { return _logoHeight; }
            set { SetProperty(ref _logoHeight, value); }
        }

        Stretch _logoStretch;
        public Stretch LogoStretch
        {
            get { return _logoStretch; }
            set { SetProperty(ref _logoStretch, value); }
        }

        SplashScreenLogoPosition _logoPosition;
        public SplashScreenLogoPosition LogoPosition
        {
            get { return _logoPosition; }
            set { SetProperty(ref _logoPosition, value); }
        }

        object _data;
        public object Data
        {
            get { return _data; }
            set { SetProperty(ref _data, value); }
        }
    }
}