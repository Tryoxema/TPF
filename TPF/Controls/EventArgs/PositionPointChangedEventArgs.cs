using System;
using System.Windows;

namespace TPF.Controls
{
    public class PositionPointChangeEventArgs : EventArgs
    {
        public Point RelativePosition { get; }

        public PositionPointChangeEventArgs(Point point)
        {
            RelativePosition = point;
        }
    }
}