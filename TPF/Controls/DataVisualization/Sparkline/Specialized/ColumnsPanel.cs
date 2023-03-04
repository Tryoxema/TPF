using System.Windows;
using System.Windows.Controls;

namespace TPF.Controls.Specialized.Sparkline
{
    public class ColumnsPanel : Panel
    {
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

            var segmentWidth = width / dataPointsCount;
            var columnPadding = segmentWidth - (segmentWidth * 0.9);
            var columnWidth = segmentWidth - columnPadding;

            for (int i = 0, count = InternalChildren.Count; i < count; i++)
            {
                if (InternalChildren[i] is ColumnItem child)
                {
                    var x = (child.RelativeX * (dataPointsCount - 1) / dataPointsCount * width) + (columnPadding / 2d);
                    var y = finalSize.Height - (finalSize.Height * child.RelativeYTop);

                    var relativeheight = child.RelativeYTop - child.RelativeYBottom;

                    var point = new Point(x, y);
                    var size = new Size(columnWidth, relativeheight * finalSize.Height);

                    child.Arrange(new Rect(point, size));
                }
            }

            return finalSize;
        }
    }
}