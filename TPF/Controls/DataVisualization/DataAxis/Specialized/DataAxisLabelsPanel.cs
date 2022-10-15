using System;
using System.Windows;
using System.Windows.Controls;

namespace TPF.Controls.Specialized.DataAxis
{
    public class DataAxisLabelsPanel : Panel
    {
        public DataAxisLabelsPanel(DataAxisLabelsControl parent)
        {
            if (parent == null) throw new ArgumentNullException(nameof(parent));

            _parent = parent;

            SetBinding(OrientationProperty, new System.Windows.Data.Binding("Orientation") { Source = _parent });
        }

        #region Orientation DependencyProperty
        public static readonly DependencyProperty OrientationProperty = DependencyProperty.Register("Orientation",
            typeof(Orientation),
            typeof(DataAxisLabelsPanel),
            new FrameworkPropertyMetadata(Orientation.Horizontal, FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsArrange));

        public Orientation Orientation
        {
            get { return (Orientation)GetValue(OrientationProperty); }
            set { SetValue(OrientationProperty, value); }
        }
        #endregion

        private DataAxisLabelsControl _parent;

        protected override Size MeasureOverride(Size availableSize)
        {
            var resultWidth = 0.0;
            var resultHeight = 0.0;

            for (int i = 0, count = InternalChildren.Count; i < count; i++)
            {
                var child = InternalChildren[i];

                child.Measure(new Size(double.PositiveInfinity, double.PositiveInfinity));

                if (Orientation == Orientation.Horizontal)
                {
                    resultWidth += child.DesiredSize.Width;
                    resultHeight = Math.Max(resultHeight, child.DesiredSize.Height);
                }
                else
                {
                    resultWidth = Math.Max(resultWidth, child.DesiredSize.Width);
                    resultHeight += child.DesiredSize.Height;
                }
            }

            if (Orientation == Orientation.Horizontal)
            {
                if (!double.IsInfinity(availableSize.Width)) resultWidth = availableSize.Width;
            }
            else
            {
                if (!double.IsInfinity(availableSize.Height)) resultHeight = availableSize.Height;
            }

            return new Size(resultWidth, resultHeight);
        }

        protected override Size ArrangeOverride(Size finalSize)
        {
            for (int i = 0, count = InternalChildren.Count; i < count; i++)
            {
                var child = InternalChildren[i] as FrameworkElement;

                var tick = child?.DataContext as DataAxisTick;

                if (child == null || tick == null) continue;

                if (Orientation == Orientation.Horizontal)
                {
                    var left = (tick.NormalizedValue * finalSize.Width) - (child.DesiredSize.Width / 2);

                    var rect = new Rect(new Point(left, 0), new Point(left + child.DesiredSize.Width, child.DesiredSize.Height));

                    child.Arrange(rect);
                }
                else
                {
                    var top = ((1 - tick.NormalizedValue) * finalSize.Height) - (child.DesiredSize.Height / 2);

                    var rect = new Rect(new Point(finalSize.Width - child.DesiredSize.Width, top), new Point(finalSize.Width, top + child.DesiredSize.Height));

                    child.Arrange(rect);
                }
            }

            return finalSize;
        }
    }
}