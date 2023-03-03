using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;
using TPF.Internal;

namespace TPF.Controls
{
    public class LinearSparkline : LinearSparklineBase
    {
        static LinearSparkline()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(LinearSparkline), new FrameworkPropertyMetadata(typeof(LinearSparkline)));
        }

        #region NormalRangeBrush DependencyProperty
        public static readonly DependencyProperty NormalRangeBrushProperty = DependencyProperty.Register("NormalRangeBrush",
            typeof(Brush),
            typeof(LinearSparkline),
            new PropertyMetadata(null));

        public Brush NormalRangeBrush
        {
            get { return (Brush)GetValue(NormalRangeBrushProperty); }
            set { SetValue(NormalRangeBrushProperty, value); }
        }
        #endregion

        #region ShowNormalRange DependencyProperty
        public static readonly DependencyProperty ShowNormalRangeProperty = DependencyProperty.Register("ShowNormalRange",
            typeof(bool),
            typeof(LinearSparkline),
            new PropertyMetadata(BooleanBoxes.FalseBox, ShowNormalRangePropertyChanged));

        private static void ShowNormalRangePropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            var instance = (LinearSparkline)sender;

            instance.UpdateNormalRangeVisibility();
        }

        public bool ShowNormalRange
        {
            get { return (bool)GetValue(ShowNormalRangeProperty); }
            set { SetValue(ShowNormalRangeProperty, BooleanBoxes.Box(value)); }
        }
        #endregion

        #region NormalRangeTop DependencyProperty
        public static readonly DependencyProperty NormalRangeTopProperty = DependencyProperty.Register("NormalRangeTop",
            typeof(double),
            typeof(LinearSparkline),
            new PropertyMetadata(double.NaN, NormalRangePropertyChanged));

        public double NormalRangeTop
        {
            get { return (double)GetValue(NormalRangeTopProperty); }
            set { SetValue(NormalRangeTopProperty, value); }
        }
        #endregion

        #region NormalRangeBottom DependencyProperty
        public static readonly DependencyProperty NormalRangeBottomProperty = DependencyProperty.Register("NormalRangeBottom",
            typeof(double),
            typeof(LinearSparkline),
            new PropertyMetadata(double.NaN, NormalRangePropertyChanged));

        public double NormalRangeBottom
        {
            get { return (double)GetValue(NormalRangeBottomProperty); }
            set { SetValue(NormalRangeBottomProperty, value); }
        }
        #endregion

        private static void NormalRangePropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            var instance = (LinearSparkline)sender;

            instance.UpdateNormalRange();
        }

        private Path _normalRange;

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            _normalRange = GetTemplateChild("PART_NormalRange") as Path;

            UpdateNormalRange();
            UpdateNormalRangeVisibility();
        }

        protected override void OnRenderSizeChanged(SizeChangedInfo sizeInfo)
        {
            base.OnRenderSizeChanged(sizeInfo);

            UpdateNormalRange();
        }

        protected override void OnDataChanged()
        {
            base.OnDataChanged();

            UpdateNormalRange();
        }

        private void UpdateNormalRangeVisibility()
        {
            if (_normalRange == null) return;

            _normalRange.Visibility = ShowNormalRange ? Visibility.Visible : Visibility.Collapsed;
        }

        private void UpdateNormalRange()
        {
            if (_normalRange == null) return;

            _normalRange.Data = null;

            var rangeTop = NormalRangeTop;
            var rangeBottom = NormalRangeBottom;

            if (double.IsNaN(rangeTop) && double.IsNaN(rangeBottom)) return;

            if (!YRange.Contains(rangeTop) && !YRange.Contains(rangeBottom)) return;

            var size = RenderSize;
            double top, bottom;

            top = Math.Max(rangeTop, rangeBottom);
            bottom = Math.Min(rangeTop, rangeBottom);

            top = Math.Min(YRange.End, top);
            bottom = Math.Max(YRange.Start, bottom);

            var topY = size.Height - (YRange.GetRelativePoint(top) * size.Height);
            var bottomY = size.Height - (YRange.GetRelativePoint(bottom) * size.Height);

            var rect = new Rect(0, topY, size.Width, bottomY - topY);

            _normalRange.Data = new RectangleGeometry(rect);
        }
    }
}