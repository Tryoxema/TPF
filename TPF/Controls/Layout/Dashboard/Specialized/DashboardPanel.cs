using System.Windows;
using System.Windows.Controls;
using System.Collections.Generic;
using TPF.Internal;

namespace TPF.Controls.Specialized.Dashboard
{
    public class DashboardPanel : Panel
    {
        private Controls.Dashboard _dashboard;
        internal Controls.Dashboard Dashboard
        {
            get
            {
                if (_dashboard == null) _dashboard = this.ParentOfType<Controls.Dashboard>();

                return _dashboard;
            }
        }

        internal Widget[,] LastMatrix { get; private set; }

        protected override Size MeasureOverride(Size availableSize)
        {
            var count = InternalChildren.Count;

            var variablePositionItems = new List<Widget>();

            var matrix = new Widget[5, 5];

            for (int i = 0; i < count; i++)
            {
                var child = InternalChildren[i];

                if (child is Widget widget)
                {
                    if (widget.InvalidPosition)
                    {
                        widget.InvalidPosition = false;
                        variablePositionItems.Add(widget);
                        continue;
                    }

                    var top = widget.Top;
                    var left = widget.Left;
                    var right = left + widget.HorizontalSlots;
                    var bottom = top + widget.VerticalSlots;

                    var length = matrix.GetLength(0);
                    var height = matrix.GetLength(1);

                    int lengthIncrease = 0, heightIncrease = 0;

                    if (length < right) lengthIncrease = right - length;
                    if (height < bottom) heightIncrease = bottom - height;

                    if (lengthIncrease > 0 || heightIncrease > 0) matrix = ArrayHelper.CreateLargerCopy(matrix, lengthIncrease, heightIncrease);

                    var isValid = true;

                    for (int x = left; x < right; x++)
                    {
                        for (int y = top; y < bottom; y++)
                        {
                            if (matrix[x, y] != null)
                            {
                                isValid = false;
                                break;
                            }
                        }

                        if (!isValid) break;
                    }

                    if (isValid)
                    {
                        for (int x = left; x < right; x++)
                        {
                            for (int y = top; y < bottom; y++)
                            {
                                matrix[x, y] = widget;
                            }
                        }
                    }
                    else variablePositionItems.Add(widget);
                }
            }

            while (variablePositionItems.Count > 0)
            {
                var widget = variablePositionItems[0];

                var foundSlot = false;

                var horizontalSlots = widget.HorizontalSlots;
                var verticalSlots = widget.VerticalSlots;

                var length = matrix.GetLength(0);
                var height = matrix.GetLength(1);

                for (int y = 0; y < height; y++)
                {
                    // Falls das Widget hier auf keinen Fall mehr in diese Spalte passt, überspringen wir diesen Durchgang
                    if (y + verticalSlots > height) break;

                    for (int x = 0; x < length; x++)
                    {
                        // Falls das Widget hier auf keinen fall mehr in diese Zeile passt, überspringen wir die
                        if (x + horizontalSlots > length) break;

                        // Ist hier noch Platz?
                        if (matrix[x, y] == null)
                        {
                            var invalid = false;

                            for (int i = 0; i < horizontalSlots; i++)
                            {
                                for (int j = 0; j < verticalSlots; j++)
                                {
                                    if (matrix[x + i, y + j] != null)
                                    {
                                        invalid = true;
                                        break;
                                    }
                                }

                                if (invalid) break;
                            }

                            if (!invalid)
                            {
                                foundSlot = true;
                                widget.SetPosition(y, x);
                                // Position des Widgets in der Matrix blockieren
                                for (int i = 0; i < horizontalSlots; i++)
                                {
                                    for (int j = 0; j < verticalSlots; j++)
                                    {
                                        matrix[x + i, y + j] = widget;
                                    }
                                }

                                break;
                            }
                        }
                    }

                    if (foundSlot) break;
                }

                if (foundSlot) variablePositionItems.RemoveAt(0);
                else matrix = ArrayHelper.CreateLargerCopy(matrix, 1, 1);
            }

            LastMatrix = matrix;

            if (Dashboard != null)
            {
                // Für das ermitteln der Größe nehmen wir den größten Punkt jeweils rechts und unten der belegt ist
                GetLargestUsedPoint(matrix, out var rightPoint, out var bottomPoint);

                var width = (rightPoint + 1) * Dashboard.SlotWidth;
                var horizontalGap = rightPoint > 0 ? rightPoint * Dashboard.Gap : 0;
                var height = (bottomPoint + 1) * Dashboard.SlotHeight;
                var verticalGap = bottomPoint > 0 ? bottomPoint * Dashboard.Gap : 0;

                var size = new Size(width + horizontalGap, height + verticalGap);

                return size;
            }

            return base.MeasureOverride(availableSize);
        }

        protected override Size ArrangeOverride(Size finalSize)
        {
            if (Dashboard != null)
            {
                var slotWidth = Dashboard.SlotWidth;
                var slotHeight = Dashboard.SlotHeight;
                var gap = Dashboard.Gap;

                var count = InternalChildren.Count;

                for (int i = 0; i < count; i++)
                {
                    var child = InternalChildren[i];

                    if (child is Widget widget)
                    {
                        var width = widget.HorizontalSlots * slotWidth;
                        var horizontalGap = (widget.HorizontalSlots - 1) * gap;
                        var height = widget.VerticalSlots * slotHeight;
                        var verticalGap = (widget.VerticalSlots - 1) * gap;

                        var size = new Size(width + horizontalGap, height + verticalGap);

                        var x = (widget.Left * slotWidth) + (widget.Left * gap);
                        var y = (widget.Top * slotHeight) + (widget.Top * gap);

                        var topLeft = new Point(x, y);

                        var rect = new Rect(topLeft, size);

                        widget.Arrange(rect);
                    }
                }
            }

            return base.ArrangeOverride(finalSize);
        }

        private static void GetLargestUsedPoint(Widget[,] matrix, out int x, out int y)
        {
            x = -1;
            y = -1;

            var length = matrix.GetLength(0);
            var height = matrix.GetLength(1);

            for (int i = 0; i < length; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    if (matrix[i, j] != null)
                    {
                        if (i > x) x = i;
                        if (j > y) y = j;
                    }
                }
            }
        }
    }
}