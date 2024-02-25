using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using TPF.Internal;

namespace TPF.Controls
{
    public class Skeleton : ContentControl
    {
        static Skeleton()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(Skeleton), new FrameworkPropertyMetadata(typeof(Skeleton)));

            LoadPresets();
        }

        private static void LoadPresets()
        {
            var uri = "pack://application:,,,/TPF;component/Controls/Misc/Skeleton/Presets.xaml";
            
            var dictionary = new ResourceDictionary()
            {
                Source = new Uri(uri, UriKind.Absolute)
            };

            _typePresets.Add(SkeletonType.Rectangle, dictionary["Rectangle"] as VisualBrush);
            _typePresets.Add(SkeletonType.Square, dictionary["Square"] as VisualBrush);
            _typePresets.Add(SkeletonType.Circle, dictionary["Circle"] as VisualBrush);
            _typePresets.Add(SkeletonType.CirclePersona, dictionary["CirclePersona"] as VisualBrush);
            _typePresets.Add(SkeletonType.SquarePersona, dictionary["SquarePersona"] as VisualBrush);
            _typePresets.Add(SkeletonType.Text, dictionary["Text"] as VisualBrush);
            _typePresets.Add(SkeletonType.Article, dictionary["Article"] as VisualBrush);
            _typePresets.Add(SkeletonType.Video, dictionary["Video"] as VisualBrush);
        }

        #region ShowContent DependencyProperty
        public static readonly DependencyProperty ShowContentProperty = DependencyProperty.Register("ShowContent",
            typeof(bool),
            typeof(Skeleton),
            new PropertyMetadata(BooleanBoxes.FalseBox, OnShimmerAnimationPropertyChanged));

        public bool ShowContent
        {
            get { return (bool)GetValue(ShowContentProperty); }
            set { SetValue(ShowContentProperty, BooleanBoxes.Box(value)); }
        }
        #endregion

        #region ShimmerColor DependencyProperty
        public static readonly DependencyProperty ShimmerColorProperty = DependencyProperty.Register("ShimmerColor",
            typeof(Color),
            typeof(Skeleton),
            new PropertyMetadata(Colors.White, OnShimmerBrushPropertyChanged));

        public Color ShimmerColor
        {
            get { return (Color)GetValue(ShimmerColorProperty); }
            set { SetValue(ShimmerColorProperty, value); }
        }
        #endregion

        #region ShimmerWidth DependencyProperty
        public static readonly DependencyProperty ShimmerWidthProperty = DependencyProperty.Register("ShimmerWidth",
            typeof(double),
            typeof(Skeleton),
            new PropertyMetadata(100d, OnShimmerAnimationPropertyChanged));

        public double ShimmerWidth
        {
            get { return (double)GetValue(ShimmerWidthProperty); }
            set { SetValue(ShimmerWidthProperty, value); }
        }
        #endregion

        #region ShimmerDirection DependencyProperty
        public static readonly DependencyProperty ShimmerDirectionProperty = DependencyProperty.Register("ShimmerDirection",
            typeof(ShimmerDirection),
            typeof(Skeleton),
            new PropertyMetadata(ShimmerDirection.LeftToRight, OnShimmerDirectionPropertyChanged));

        private static void OnShimmerDirectionPropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            var instance = (Skeleton)sender;

            instance.SetShimmerBrush();
            instance.UpdateShimmerAnimation();
        }

        public ShimmerDirection ShimmerDirection
        {
            get { return (ShimmerDirection)GetValue(ShimmerDirectionProperty); }
            set { SetValue(ShimmerDirectionProperty, value); }
        }
        #endregion

        #region ShimmerDuration DependencyProperty
        public static readonly DependencyProperty ShimmerDurationProperty = DependencyProperty.Register("ShimmerDuration",
            typeof(TimeSpan),
            typeof(Skeleton),
            new PropertyMetadata(TimeSpan.FromSeconds(1), OnShimmerAnimationPropertyChanged));

        public TimeSpan ShimmerDuration
        {
            get { return (TimeSpan)GetValue(ShimmerDurationProperty); }
            set { SetValue(ShimmerDurationProperty, value); }
        }
        #endregion

        #region Type DependencyProperty
        public static readonly DependencyProperty TypeProperty = DependencyProperty.Register("Type",
            typeof(SkeletonType),
            typeof(Skeleton),
            new PropertyMetadata(SkeletonType.Rectangle, OnOpacityMaskPropertyChanged));

        public SkeletonType Type
        {
            get { return (SkeletonType)GetValue(TypeProperty); }
            set { SetValue(TypeProperty, value); }
        }
        #endregion

        #region CustomLayout DependencyProperty
        public static readonly DependencyProperty CustomLayoutProperty = DependencyProperty.Register("CustomLayout",
            typeof(Visual),
            typeof(Skeleton),
            new PropertyMetadata(null, OnOpacityMaskPropertyChanged));

        public Visual CustomLayout
        {
            get { return (Visual)GetValue(CustomLayoutProperty); }
            set { SetValue(CustomLayoutProperty, value); }
        }
        #endregion

        private static void OnOpacityMaskPropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            var instance = (Skeleton)sender;

            instance.UpdateOpacityMask();
        }

        private static void OnShimmerBrushPropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            var instance = (Skeleton)sender;

            instance.SetShimmerBrush();
        }

        private static void OnShimmerAnimationPropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            var instance = (Skeleton)sender;

            instance.UpdateShimmerAnimation();
        }

        private static readonly Dictionary<SkeletonType, VisualBrush> _typePresets = new Dictionary<SkeletonType, VisualBrush>();

        private FrameworkElement _placeholderContainer;
        private Rectangle _shimmerRectangle;
        private Storyboard _storyboard;

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            _placeholderContainer = GetTemplateChild("PART_PlaceholderContainer") as FrameworkElement;
            _shimmerRectangle = GetTemplateChild("PART_ShimmerRectangle") as Rectangle;

            if (_shimmerRectangle != null)
            {
                _shimmerRectangle.HorizontalAlignment = HorizontalAlignment.Left;
                _shimmerRectangle.VerticalAlignment = VerticalAlignment.Top;
                _shimmerRectangle.RenderTransformOrigin = new Point(0.5, 0.5);
                _shimmerRectangle.RenderTransform = new TranslateTransform();
            }

            UpdateOpacityMask();
            SetShimmerBrush();
            UpdateShimmerAnimation();
        }

        protected override void OnRenderSizeChanged(SizeChangedInfo sizeInfo)
        {
            base.OnRenderSizeChanged(sizeInfo);

            UpdateShimmerAnimation();
        }

        private void UpdateOpacityMask()
        {
            if (_placeholderContainer == null) return;

            var type = Type;

            VisualBrush mask;

            if (type == SkeletonType.Custom)
            {
                mask = new VisualBrush()
                {
                    AlignmentX = AlignmentX.Left,
                    AlignmentY = AlignmentY.Top,
                    Stretch = Stretch.None,
                    Visual = CustomLayout
                };
            }
            else
            {
                mask = _typePresets[type];
            }

            _placeholderContainer.OpacityMask = mask;
        }

        private void SetShimmerBrush()
        {
            if (_shimmerRectangle == null) return;

            var direction = ShimmerDirection;

            var isHorizontal = direction == ShimmerDirection.LeftToRight || direction == ShimmerDirection.RightToLeft;

            var endPoint = isHorizontal ? new Point(1, 0) : new Point(0, 1);

            var brush = new LinearGradientBrush()
            {
                StartPoint = new Point(0, 0),
                EndPoint = endPoint
            };

            brush.GradientStops.Add(new GradientStop(Colors.Transparent, 0));
            brush.GradientStops.Add(new GradientStop(ShimmerColor, 0.5));
            brush.GradientStops.Add(new GradientStop(Colors.Transparent, 1));

            _shimmerRectangle.Fill = brush;
        }

        private void UpdateShimmerAnimation()
        {
            _storyboard?.Stop();

            if (ShowContent) return;

            if (_shimmerRectangle == null) return;

            var width = ActualWidth;
            var height = ActualHeight;

            if (width <= 0 || height <= 0) return;

            var direction = ShimmerDirection;

            var isHorizontal = direction == ShimmerDirection.LeftToRight || direction == ShimmerDirection.RightToLeft;

            if (isHorizontal)
            {
                _shimmerRectangle.Width = ShimmerWidth;
                _shimmerRectangle.Height = height;
                _shimmerRectangle.HorizontalAlignment = HorizontalAlignment.Left;
            }
            else
            {
                _shimmerRectangle.Width = width;
                _shimmerRectangle.Height = ShimmerWidth;
                _shimmerRectangle.HorizontalAlignment = HorizontalAlignment.Center;
            }

            if (ShimmerWidth <= 0) return;

            var contentWidth = _shimmerRectangle.Width;
            var contentHeight = _shimmerRectangle.Height;
            double from, to;
            PropertyPath propertyPath;

            switch (ShimmerDirection)
            {
                case ShimmerDirection.LeftToRight:
                {
                    from = -contentWidth;
                    to = width;
                    propertyPath = new PropertyPath("(UIElement.RenderTransform).(TranslateTransform.X)");
                    break;
                }
                case ShimmerDirection.RightToLeft:
                {
                    from = width;
                    to = -contentWidth;
                    propertyPath = new PropertyPath("(UIElement.RenderTransform).(TranslateTransform.X)");
                    break;
                }
                case ShimmerDirection.TopToBottom:
                {
                    from = -contentHeight;
                    to = height;
                    propertyPath = new PropertyPath("(UIElement.RenderTransform).(TranslateTransform.Y)");
                    break;
                }
                case ShimmerDirection.BottomToTop:
                {
                    from = height;
                    to = -contentHeight;
                    propertyPath = new PropertyPath("(UIElement.RenderTransform).(TranslateTransform.Y)");
                    break;
                }
                default: return;
            }

            var animation = new DoubleAnimationUsingKeyFrames();

            // Startpunkt
            animation.KeyFrames.Add(new EasingDoubleKeyFrame(from, KeyTime.FromTimeSpan(TimeSpan.Zero)));

            var duration = ShimmerDuration;

            // Endpunkt einfügen
            animation.KeyFrames.Add(new EasingDoubleKeyFrame(to, KeyTime.FromTimeSpan(duration)));

            var finalTime = duration.Add(TimeSpan.FromMilliseconds(500));
            // Einen letzten Punkt in die Animation einfügen, der die Pause am Ende simuliert
            animation.KeyFrames.Add(new EasingDoubleKeyFrame(to, KeyTime.FromTimeSpan(finalTime)));

            Storyboard.SetTarget(animation, _shimmerRectangle);
            Storyboard.SetTargetProperty(animation, propertyPath);

            _storyboard = new Storyboard()
            {
                RepeatBehavior = RepeatBehavior.Forever
            };
            _storyboard.Children.Add(animation);
            _storyboard.Begin();
        }
    }
}