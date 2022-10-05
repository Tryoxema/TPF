using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Documents;

namespace TPF.DragDrop.Behaviors
{
    internal class DropAdorner : Adorner
    {
        internal DropAdorner(UIElement adornedElement, UIElement adornment) : base(adornedElement)
        {
            _position = new Point();
            _adornerLayer = AdornerLayer.GetAdornerLayer(adornedElement);
            _adornerLayer.Add(this);
            _adornment = adornment;
            IsHitTestVisible = false;
        }

        private readonly AdornerLayer _adornerLayer;
        private readonly UIElement _adornment;

        Point _position;
        private Point Position
        {
            get { return _position; }
            set
            {
                if (_position != value)
                {
                    _position = value;
                    _adornerLayer.Update(AdornedElement);
                }
            }
        }

        public void Remove()
        {
            _adornerLayer.Remove(this);
        }

        internal void MoveElement(Point point)
        {
            if (point.X < 0) point.X = 0;
            if (point.Y < 0) point.Y = 0;

            Position = point;
            
            InvalidateVisual();
        }

        public override GeneralTransform GetDesiredTransform(GeneralTransform transform)
        {
            var result = new GeneralTransformGroup();
            result.Children.Add(base.GetDesiredTransform(transform));
            result.Children.Add(new TranslateTransform(_position.X, _position.Y));
            return result;
        }

        protected override Visual GetVisualChild(int index)
        {
            return _adornment;
        }

        protected override int VisualChildrenCount
        {
            get { return 1; }
        }

        protected override Size MeasureOverride(Size constraint)
        {
            _adornment.Measure(constraint);
            return _adornment.DesiredSize;
        }

        protected override Size ArrangeOverride(Size finalSize)
        {
            _adornment.Arrange(new Rect(finalSize));
            return finalSize;
        }
    }
}