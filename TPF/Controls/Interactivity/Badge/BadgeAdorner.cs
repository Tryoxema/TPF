using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Documents;

namespace TPF.Controls
{
    internal class BadgeAdorner : Adorner
    {
        internal BadgeAdorner(UIElement adornedElement, Badge badge) : base(adornedElement)
        {
            _position = new Point();
            _adornerLayer = AdornerLayer.GetAdornerLayer(adornedElement);
            _adornerLayer.Add(this);
            _badge = badge;
            IsHitTestVisible = false;
        }

        private readonly AdornerLayer _adornerLayer;
        private readonly Badge _badge;

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

        internal void Update()
        {
            _adornerLayer.Update(AdornedElement);
        }

        internal void MoveElement(Point point)
        {
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
            return _badge;
        }

        protected override int VisualChildrenCount
        {
            get { return 1; }
        }

        protected override Size MeasureOverride(Size constraint)
        {
            _badge.Measure(constraint);
            return _badge.DesiredSize;
        }

        protected override Size ArrangeOverride(Size finalSize)
        {
            _badge.Arrange(new Rect(finalSize));
            return finalSize;
        }
    }
}