using System;
using System.Collections;
using System.Collections.Generic;
using System.Windows;

namespace TPF.DragDrop.Behaviors
{
    public class DragVisualProviderData
    {
        public DragVisualProviderData(FrameworkElement hostElement, IEnumerable<DependencyObject> itemContainers, IEnumerable items, Point relativeStartPoint)
        {
            HostElement = hostElement;
            ItemContainers = itemContainers;
            Items = items;
            RelativeStartPoint = relativeStartPoint;
        }

        public FrameworkElement HostElement { get; }

        public IEnumerable<DependencyObject> ItemContainers { get; }

        public IEnumerable Items { get; }

        public Point RelativeStartPoint { get; }

        double _opacity = 1.0;
        public double Opacity
        {
            get { return _opacity; }
            set
            {
                if (value < 0.0) _opacity = 0.0;
                else if (value > 1.0) _opacity = 1.0;
                else _opacity = value;
            }
        }
    }
}