using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Data;
using TPF.Controls;

namespace TPF.Demo.Views
{
    public partial class CalloutDemoView : ViewBase
    {
        public CalloutDemoView()
        {
            InitializeComponent();

            _callout = new Callout()
            {
                Content = "Wow was ein Feature",
            };

            ArrowBaseAnchorPoint1 = _callout.ArrowBaseAnchorPoint1;
            ArrowBaseAnchorPoint2 = _callout.ArrowBaseAnchorPoint2;
            ArrowTargetAnchorPoint = _callout.ArrowTargetAnchorPoint;

            _callout.SetBinding(Callout.TypeProperty, new Binding(nameof(Type))  { Source = this });
            _callout.SetBinding(Callout.ArrowBaseAnchorPoint1Property, new Binding(nameof(ArrowBaseAnchorPoint1))  { Source = this });
            _callout.SetBinding(Callout.ArrowBaseAnchorPoint2Property, new Binding(nameof(ArrowBaseAnchorPoint2))  { Source = this });
            _callout.SetBinding(Callout.ArrowTargetAnchorPointProperty, new Binding(nameof(ArrowTargetAnchorPoint))  { Source = this });

            CalloutTypes.Add(CalloutType.Rectangle);
            CalloutTypes.Add(CalloutType.RoundedRectangle);
            CalloutTypes.Add(CalloutType.Ellipse);

            CalloutPlacements.Add(CalloutPlacement.TopLeft);
            CalloutPlacements.Add(CalloutPlacement.Top);
            CalloutPlacements.Add(CalloutPlacement.TopRight);
            CalloutPlacements.Add(CalloutPlacement.BottomLeft);
            CalloutPlacements.Add(CalloutPlacement.Bottom);
            CalloutPlacements.Add(CalloutPlacement.BottomRight);
            CalloutPlacements.Add(CalloutPlacement.LeftTop);
            CalloutPlacements.Add(CalloutPlacement.Left);
            CalloutPlacements.Add(CalloutPlacement.LeftBottom);
            CalloutPlacements.Add(CalloutPlacement.RightTop);
            CalloutPlacements.Add(CalloutPlacement.Right);
            CalloutPlacements.Add(CalloutPlacement.RightBottom);
        }

        Callout _callout;

        CalloutType _type;
        public CalloutType Type
        {
            get { return _type; }
            set { SetProperty(ref _type, value); }
        }

        Point _arrowBaseAnchorPoint1;
        public Point ArrowBaseAnchorPoint1
        {
            get { return _arrowBaseAnchorPoint1; }
            set 
            { 
                if (SetProperty(ref _arrowBaseAnchorPoint1, value))
                {
                    ArrowBaseAnchorPoint1X = value.X;
                    ArrowBaseAnchorPoint1Y = value.Y;
                }
            }
        }

        double _arrowBaseAnchorPoint1X;
        public double ArrowBaseAnchorPoint1X
        {
            get { return _arrowBaseAnchorPoint1X; }
            set 
            { 
                if (SetProperty(ref _arrowBaseAnchorPoint1X, value))
                {
                    var point = ArrowBaseAnchorPoint1;
                    point.X = value;
                    ArrowBaseAnchorPoint1 = point;
                }
            }
        }

        double _arrowBaseAnchorPoint1Y;
        public double ArrowBaseAnchorPoint1Y
        {
            get { return _arrowBaseAnchorPoint1Y; }
            set 
            { 
                if (SetProperty(ref _arrowBaseAnchorPoint1Y, value))
                {
                    var point = ArrowBaseAnchorPoint1;
                    point.Y = value;
                    ArrowBaseAnchorPoint1 = point;
                }
            }
        }

        Point _arrowBaseAnchorPoint2;
        public Point ArrowBaseAnchorPoint2
        {
            get { return _arrowBaseAnchorPoint2; }
            set
            {
                if (SetProperty(ref _arrowBaseAnchorPoint2, value))
                {
                    ArrowBaseAnchorPoint2X = value.X;
                    ArrowBaseAnchorPoint2Y = value.Y;
                }
            }
        }

        double _arrowBaseAnchorPoint2X;
        public double ArrowBaseAnchorPoint2X
        {
            get { return _arrowBaseAnchorPoint2X; }
            set
            {
                if (SetProperty(ref _arrowBaseAnchorPoint2X, value))
                {
                    var point = ArrowBaseAnchorPoint2;
                    point.X = value;
                    ArrowBaseAnchorPoint2 = point;
                }
            }
        }

        double _arrowBaseAnchorPoint2Y;
        public double ArrowBaseAnchorPoint2Y
        {
            get { return _arrowBaseAnchorPoint2Y; }
            set
            {
                if (SetProperty(ref _arrowBaseAnchorPoint2Y, value))
                {
                    var point = ArrowBaseAnchorPoint2;
                    point.Y = value;
                    ArrowBaseAnchorPoint2 = point;
                }
            }
        }

        Point _arrowTargetAnchorPoint;
        public Point ArrowTargetAnchorPoint
        {
            get { return _arrowTargetAnchorPoint; }
            set
            {
                if (SetProperty(ref _arrowTargetAnchorPoint, value))
                {
                    ArrowTargetAnchorPointX = value.X;
                    ArrowTargetAnchorPointY = value.Y;
                }
            }
        }

        double _arrowTargetAnchorPointX;
        public double ArrowTargetAnchorPointX
        {
            get { return _arrowTargetAnchorPointX; }
            set
            {
                if (SetProperty(ref _arrowTargetAnchorPointX, value))
                {
                    var point = ArrowTargetAnchorPoint;
                    point.X = value;
                    ArrowTargetAnchorPoint = point;
                }
            }
        }

        double _arrowTargetAnchorPointY;
        public double ArrowTargetAnchorPointY
        {
            get { return _arrowTargetAnchorPointY; }
            set
            {
                if (SetProperty(ref _arrowTargetAnchorPointY, value))
                {
                    var point = ArrowTargetAnchorPoint;
                    point.Y = value;
                    ArrowTargetAnchorPoint = point;
                }
            }
        }

        CalloutPlacement _placement;
        public CalloutPlacement Placement
        {
            get { return _placement; }
            set { SetProperty(ref _placement, value); }
        }

        double _horizontalOffset;
        public double HorizontalOffset
        {
            get { return _horizontalOffset; }
            set { SetProperty(ref _horizontalOffset, value); }
        }

        double _verticalOffset;
        public double VerticalOffset
        {
            get { return _verticalOffset; }
            set { SetProperty(ref _verticalOffset, value); }
        }

        bool _staysOpen;
        public bool StaysOpen
        {
            get { return _staysOpen; }
            set { SetProperty(ref _staysOpen, value); }
        }

        public ObservableCollection<CalloutType> CalloutTypes { get; } = new ObservableCollection<CalloutType>();
        public ObservableCollection<CalloutPlacement> CalloutPlacements { get; } = new ObservableCollection<CalloutPlacement>();

        private void ShowCalloutButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            CalloutService.ShowPopup(_callout, DemoButton, new CalloutPopupOptions()
            {
                Placement = Placement,
                HorizontalOffset = HorizontalOffset,
                VerticalOffset = VerticalOffset,
                StaysOpen = StaysOpen,
            });
        }

        private void CloseCalloutButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            CalloutService.ClosePopup(_callout);
        }
    }
}