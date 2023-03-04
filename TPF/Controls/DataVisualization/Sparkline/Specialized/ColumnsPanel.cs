using System.Windows;
using System.Windows.Controls;

namespace TPF.Controls.Specialized.Sparkline
{
    public class ColumnsPanel : Panel
    {
        #region ColumnWidthFactor DependencyProperty
        public static readonly DependencyProperty ColumnWidthFactorProperty = DependencyProperty.Register("ColumnWidthFactor",
            typeof(double),
            typeof(ColumnsPanel),
            new FrameworkPropertyMetadata(0.9, FrameworkPropertyMetadataOptions.AffectsArrange, null, ConstrainColumnWidthFactor));

        private static object ConstrainColumnWidthFactor(DependencyObject d, object baseValue)
        {
            var doubleValue = (double)baseValue;

            if (doubleValue < 0) doubleValue = 0;
            else if (doubleValue > 1) doubleValue = 1;

            return doubleValue;
        }

        public double ColumnWidthFactor
        {
            get { return (double)GetValue(ColumnWidthFactorProperty); }
            set { SetValue(ColumnWidthFactorProperty, value); }
        }
        #endregion

        protected override Size MeasureOverride(Size availableSize)
        {
            var size = base.MeasureOverride(availableSize);

            for (int i = 0, count = InternalChildren.Count; i < count; i++)
            {
                var child = InternalChildren[i];

                child.Measure(availableSize);
            }

            return size;
        }

        protected override Size ArrangeOverride(Size finalSize)
        {
            var dataPointsCount = InternalChildren.Count;

            if (dataPointsCount == 0) return finalSize;

            var width = finalSize.Width;
            var height = finalSize.Height;

            var segmentWidth = width / dataPointsCount;
            var columnPadding = segmentWidth - (segmentWidth * ColumnWidthFactor);
            var columnWidth = segmentWidth - columnPadding;

            for (int i = 0, count = InternalChildren.Count; i < count; i++)
            {
                if (InternalChildren[i] is ColumnItem child)
                {
                    var x = (child.RelativeX * (dataPointsCount - 1) / dataPointsCount * width) + (columnPadding / 2d);
                    var y = height - (height * child.RelativeYTop);

                    var relativeheight = child.RelativeYTop - child.RelativeYBottom;

                    var point = new Point(x, y);
                    var size = new Size(columnWidth, relativeheight * height);

                    child.Arrange(new Rect(point, size));
                }
            }

            return finalSize;
        }
    }
}