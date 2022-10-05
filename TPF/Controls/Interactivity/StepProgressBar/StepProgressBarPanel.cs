using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace TPF.Controls
{
    public class StepProgressBarPanel : Panel
    {
        private Orientation GetOrientation()
        {
            var orientation = Orientation.Horizontal;

            var stepProgressBar = this.ParentOfType<StepProgressBar>();

            if (stepProgressBar != null)
            {
                orientation = stepProgressBar.Orientation;
            }

            return orientation;
        }

        protected override Size MeasureOverride(Size availableSize)
        {
            var orientation = GetOrientation();

            var isHorizontal = orientation == Orientation.Horizontal;

            var desiredSize = new Size();
            var layoutSize = availableSize;

            if (isHorizontal) layoutSize.Width = double.PositiveInfinity;
            else layoutSize.Height = double.PositiveInfinity;

            for (int i = 0, count = InternalChildren.Count; i < count; i++)
            {
                var child = InternalChildren[i];

                // Sollte eigentlich nie greifen aber man weiß ja nie
                if (child == null) continue;

                child.Measure(layoutSize);

                var childSize = child.DesiredSize;

                if (isHorizontal)
                {
                    desiredSize.Width += childSize.Width;
                    desiredSize.Height = Math.Max(desiredSize.Height, childSize.Height);
                }
                else
                {
                    desiredSize.Width = Math.Max(desiredSize.Width, childSize.Width);
                    desiredSize.Height += childSize.Height;
                }
            }

            return desiredSize;
        }

        protected override Size ArrangeOverride(Size finalSize)
        {
            var orientation = GetOrientation();

            var isHorizontal = orientation == Orientation.Horizontal;

            var childBounds = new Rect();
            var previousChildSize = 0.0;

            for (int i = 0, count = InternalChildren.Count; i < count; i++)
            {
                var child = InternalChildren[i];

                // Sollte eigentlich nie greifen aber man weiß ja nie
                if (child == null) continue;

                var childSize = child.DesiredSize;

                if (isHorizontal)
                {
                    childBounds.X += previousChildSize;
                    childBounds.Width = childSize.Width;
                    childBounds.Height = Math.Max(finalSize.Height, childSize.Height);
                    previousChildSize = childSize.Width;
                }
                else
                {
                    childBounds.Y += previousChildSize;
                    childBounds.Height = childSize.Height;
                    childBounds.Width = Math.Max(finalSize.Width, childSize.Width);
                    previousChildSize = childSize.Height;
                }

                child.Arrange(childBounds);
            }

            

            return finalSize;
        }
    }
}