using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using TPF.Internal;

namespace TPF.Controls.Specialized.DateTimeRangeNavigator
{
    public class ItemIntervalLabelsControl : IntervalLabelsControl
    {
        static ItemIntervalLabelsControl()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ItemIntervalLabelsControl), new FrameworkPropertyMetadata(typeof(ItemIntervalLabelsControl)));
        }

        internal override LabelType LabelType
        {
            get
            {
                return LabelType.Item;
            }
        }

        Controls.DateTimeRangeNavigator _owner;
        private Controls.DateTimeRangeNavigator Owner
        {
            get
            {
                if (_owner == null) _owner = this.ParentOfType<Controls.DateTimeRangeNavigator>();
                
                return _owner;
            }
        }

        private bool _isDragging;
        private Point _dragStartPoint;

        protected override DependencyObject GetContainerForItemOverride()
        {
            return new ItemIntervalLabel();
        }

        protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            base.OnMouseLeftButtonDown(e);

            _isDragging = true;
            _dragStartPoint = e.GetPosition(this);
            var dragItems = GetDragItems(_dragStartPoint, _dragStartPoint);
            
            for (int i = 0; i < dragItems.Count; i++)
            {
                var item = dragItems[i];

                item.IsHighlighted = true;
            }

            CaptureMouse();
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);

            if (!_isDragging) return;

            var panel = this.GetItemsPanel();

            // Falls aus irgendeinem Grund das Panel nicht gibt, machen wir hier auch nichts
            if (panel == null) return;

            var position = e.GetPosition(this);

            var allItems = panel.Children.OfType<ItemIntervalLabel>().ToList();
            var dragItems = GetDragItems(_dragStartPoint, position);

            for (var i = 0; i < allItems.Count; i++)
            {
                var item = allItems[i];

                item.IsHighlighted = dragItems.Contains(item);
            }
        }

        protected override void OnMouseLeftButtonUp(MouseButtonEventArgs e)
        {
            base.OnMouseLeftButtonUp(e);

            ReleaseMouseCapture();

            if (!_isDragging) return;

            _isDragging = false;
            var panel = this.GetItemsPanel();

            // Falls aus irgendeinem Grund das Panel nicht gibt, machen wir hier auch nichts
            if (panel == null) return;

            var position = e.GetPosition(this);

            var allItems = panel.Children.OfType<ItemIntervalLabel>().ToList();
            var dragItems = GetDragItems(_dragStartPoint, position);

            for (int i = 0; i < allItems.Count; i++)
            {
                var item = allItems[i];

                item.IsHighlighted = false;
            }

            var periods = new List<IntervalPeriod>();

            for (int i = 0; i < dragItems.Count; i++)
            {
                var item = dragItems[i];

                if (item.DataContext is IntervalPeriod period) periods.Add(period);
            }

            if (periods.Count == 0) return;

            // Liste einmal aufsteigend sortieren
            periods = periods.OrderBy(x => x).ToList();

            var first = periods[0];
            var last = periods.Last();

            Owner.SetSelectedInterval(first.Start, last.End);
        }

        private List<ItemIntervalLabel> GetDragItems(Point start, Point end)
        {
            var itemIntervals = new HashSet<ItemIntervalLabel>();

            if (start == end)
            {
                var hitTest = VisualTreeHelper.HitTest(this, start);

                var itemInterval = hitTest.VisualHit.ParentOfType<ItemIntervalLabel>();

                if (itemInterval != null) itemIntervals.Add(itemInterval);
            }
            else
            {
                var bounds = new Rect(start, end);

                var items = HitTestHelper.GetHitTestElementsInBoundsOfType<ItemIntervalLabel>(this, bounds);

                if (items != null)
                {
                    for (int i = 0; i < items.Count; i++)
                    {
                        var item = items[i];

                        itemIntervals.Add(item);
                    }
                }
            }

            return itemIntervals.ToList();
        }
    }
}