using System;
using System.Windows;
using System.Windows.Media.Animation;

namespace TPF.Animations
{
    public class GridLengthAnimation : AnimationTimeline
    {
        #region Konstruktoren
        public GridLengthAnimation()
        {

        }

        public GridLengthAnimation(Duration duration)
        {
            Duration = duration;
        }

        public GridLengthAnimation(double to, Duration duration)
        {
            To = new GridLength(to);
            Duration = duration;
        }

        public GridLengthAnimation(GridLength to, Duration duration)
        {
            To = to;
            Duration = duration;
        }

        public GridLengthAnimation(double from, double to, Duration duration)
        {
            From = new GridLength(from);
            To = new GridLength(to);
            Duration = duration;
        }

        public GridLengthAnimation(double from, GridLength to, Duration duration)
        {
            From = new GridLength(from);
            To = to;
            Duration = duration;
        }

        public GridLengthAnimation(GridLength from, double to, Duration duration)
        {
            From = from;
            To = new GridLength(to);
            Duration = duration;
        }

        public GridLengthAnimation(GridLength from, GridLength to, Duration duration)
        {
            From = from;
            To = to;
            Duration = duration;
        } 
        #endregion

        #region FromProperty DependencyProperty
        public static readonly DependencyProperty FromProperty = DependencyProperty.Register("From",
            typeof(GridLength),
            typeof(GridLengthAnimation),
            new PropertyMetadata(new GridLength(0)));

        public GridLength From
        {
            get { return (GridLength)GetValue(FromProperty); }
            set { SetValue(FromProperty, value); }
        }
        #endregion

        #region ToProperty DependencyProperty
        public static readonly DependencyProperty ToProperty = DependencyProperty.Register("To",
            typeof(GridLength),
            typeof(GridLengthAnimation),
            new PropertyMetadata(new GridLength(0)));

        public GridLength To
        {
            get { return (GridLength)GetValue(ToProperty); }
            set { SetValue(ToProperty, value); }
        }
        #endregion

        protected override Freezable CreateInstanceCore()
        {
            return new GridLengthAnimation();
        }

        public override Type TargetPropertyType
        {
            get { return typeof(GridLength); }
        }

        public override object GetCurrentValue(object defaultOriginValue, object defaultDestinationValue, AnimationClock animationClock)
        {
            double fromValue = ((GridLength)GetValue(FromProperty)).Value;
            double toValue = ((GridLength)GetValue(ToProperty)).Value;

            if (fromValue > toValue)
            {
                return new GridLength((1 - animationClock.CurrentProgress.Value) * (fromValue - toValue) + toValue, To.IsStar ? GridUnitType.Star : GridUnitType.Pixel);
            }
            else
            {
                return new GridLength((animationClock.CurrentProgress.Value) * (toValue - fromValue) + fromValue, To.IsStar ? GridUnitType.Star : GridUnitType.Pixel);
            }
        }
    }
}