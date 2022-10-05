using System;
using System.Windows;

namespace TPF.DragDrop.Behaviors
{
    public class DropInfo
    {
        public int InsertIndex { get; set; } = -1;

        public RelativeDropPosition DropPosition { get; set; }

        public UIElement Target { get; set; }

        public UIElement TargetItem { get; set; }

        public UIElement AdornedElement { get; set; }

        public Point PositionInTarget { get; set; }
    }

    public enum RelativeDropPosition
    {
        None = 0,
        Before = 1,
        Inside = 2,
        After = 3
    }
}