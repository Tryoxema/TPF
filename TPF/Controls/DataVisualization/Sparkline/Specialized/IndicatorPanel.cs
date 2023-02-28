using System.Windows;
using System.Windows.Controls;

namespace TPF.Controls.Specialized.Sparkline
{
    public class IndicatorPanel : Panel
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
            for (int i = 0, count = InternalChildren.Count; i < count; i++)
            {
                if (InternalChildren[i] is IndicatorItem child)
                {
                    var x = finalSize.Width * child.RelativeX;
                    var y = finalSize.Height - (finalSize.Height * child.RelativeY);

                    var point = new Point(x - (child.DesiredSize.Width / 2), y - (child.DesiredSize.Height / 2));

                    child.Arrange(new Rect(point, child.DesiredSize));
                }
            }

            return finalSize;
        }
    }
}