using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace TPF.Controls
{
    public class Clipper : ContentControl
    {
        public Clipper()
        {
            SizeChanged += Clipper_SizeChanged;
        }

        #region VisibleRatio DependencyProperty
        public static readonly DependencyProperty VisibleRatioProperty = DependencyProperty.Register("VisibleRatio",
            typeof(double),
            typeof(Clipper),
            new PropertyMetadata(0.0, VisibleRatioPropertyChanged, ConstrainVisibleRatio));

        static void VisibleRatioPropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            var instance = (Clipper)sender;

            instance.OnVisibleRatioChanged();
        }

        internal static object ConstrainVisibleRatio(DependencyObject sender, object value)
        {
            var doubleValue = (double)value;

            if (doubleValue < 0.0) doubleValue = 0.0;
            else if (doubleValue > 1.0) doubleValue = 1.0;

            return doubleValue;
        }

        public double VisibleRatio
        {
            get { return (double)GetValue(VisibleRatioProperty); }
            set { SetValue(VisibleRatioProperty, value); }
        }
        #endregion

        #region ClippingDirection DependencyProperty
        public static readonly DependencyProperty ClippingDirectionProperty = DependencyProperty.Register("ClippingDirection",
            typeof(ClippingDirection),
            typeof(Clipper),
            new PropertyMetadata(ClippingDirection.Right, ClippingDirectionPropertyChanged));

        private static void ClippingDirectionPropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            var instance = (Clipper)sender;

            instance.OnClippingDirectionChanged();
        }

        public ClippingDirection ClippingDirection
        {
            get { return (ClippingDirection)GetValue(ClippingDirectionProperty); }
            set { SetValue(ClippingDirectionProperty, value); }
        }
        #endregion

        public void ClipContent()
        {
            Rect rectangle;

            switch (ClippingDirection)
            {
                case ClippingDirection.Up:
                {
                    var visibleHeight = ActualHeight * VisibleRatio;
                    var y = ActualHeight - visibleHeight;
                    rectangle = new Rect(0, y, ActualWidth, visibleHeight);
                    break;
                }
                case ClippingDirection.Down:
                {
                    rectangle = new Rect(0, 0, ActualWidth, ActualHeight * VisibleRatio);
                    break;
                }
                case ClippingDirection.Left:
                {
                    var visibleWidth = ActualWidth * VisibleRatio;
                    var x = ActualWidth - visibleWidth;
                    rectangle = new Rect(x, 0, visibleWidth, ActualHeight);
                    break;
                }
                case ClippingDirection.Right:
                {
                    rectangle = new Rect(0, 0, ActualWidth * VisibleRatio, ActualHeight);
                    break;
                }
                default:
                {
                    rectangle = new Rect();
                    break;
                }
            }

            var clip = new RectangleGeometry(rectangle);

            Clip = clip;
        }

        protected virtual void OnVisibleRatioChanged()
        {
            ClipContent();
        }

        protected virtual void OnClippingDirectionChanged()
        {
            ClipContent();
        }

        private void Clipper_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            ClipContent();
        }
    }
}