using System;
using System.Windows;
using System.Windows.Controls;

namespace TPF.Controls
{
    public class SimplePanel : Panel
    {
        protected override Size MeasureOverride(Size availableSize)
        {
            var maximumSize = new Size();

            for (int i = 0, count = InternalChildren.Count; i < count; i++)
            {
                var child = InternalChildren[i];

                if (child != null)
                {
                    child.Measure(availableSize);
                    maximumSize.Width = Math.Max(maximumSize.Width, child.DesiredSize.Width);
                    maximumSize.Height = Math.Max(maximumSize.Height, child.DesiredSize.Height);
                }
            }

            return maximumSize;
        }

        protected override Size ArrangeOverride(Size finalSize)
        {
            for (int i = 0, count = InternalChildren.Count; i < count; i++)
            {
                var child = InternalChildren[i];

                child?.Arrange(new Rect(finalSize));
            }

            return finalSize;
        }
    }
}