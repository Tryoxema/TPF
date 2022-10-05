using System;
using System.Windows;
using System.Windows.Shapes;
using System.Windows.Media;
using TPF.DragDrop.Behaviors;
using TPF.DragDrop;

namespace TPF.Controls.Specialized.Dashboard
{
    public class DashboardDropVisualProvider : DependencyObject, IDropVisualProvider
    {
        #region Stroke DependencyProperty
        public static readonly DependencyProperty StrokeProperty = DependencyProperty.Register("Stroke",
            typeof(Brush),
            typeof(DashboardDropVisualProvider),
            new PropertyMetadata(Brushes.DodgerBlue));

        public Brush Stroke
        {
            get { return (Brush)GetValue(StrokeProperty); }
            set { SetValue(StrokeProperty, value); }
        }
        #endregion

        #region Fill DependencyProperty
        public static readonly DependencyProperty FillProperty = DependencyProperty.Register("Fill",
            typeof(Brush),
            typeof(DashboardDropVisualProvider),
            new PropertyMetadata(Brushes.DodgerBlue));

        public Brush Fill
        {
            get { return (Brush)GetValue(FillProperty); }
            set { SetValue(FillProperty, value); }
        }
        #endregion

        public Controls.Dashboard Dashboard { get; set; }

        public FrameworkElement CreateDropVisual()
        {
            var rectangle = new Rectangle()
            {
                Stroke = Stroke,
                Fill = Fill,
                Opacity = 0.5,
                IsHitTestVisible = false
            };

            if (Dashboard?.DraggingWidget != null)
            {
                var widget = Dashboard.DraggingWidget;

                var width = widget.HorizontalSlots * Dashboard.SlotWidth;
                var horizontalGap = (widget.HorizontalSlots - 1) * Dashboard.Gap;
                var height = widget.VerticalSlots * Dashboard.SlotHeight;
                var verticalGap = (widget.VerticalSlots - 1) * Dashboard.Gap;

                rectangle.Width = width + horizontalGap;
                rectangle.Height = height + verticalGap;
            }

            return rectangle;
        }

        public Point GetPosition(DropInfo dropInfo)
        {
            var slotWidth = Dashboard?.SlotWidth ?? 0;
            var slotHeight = Dashboard?.SlotHeight ?? 0;
            var gap = Dashboard?.Gap ?? 0;

            // Den Slot berechnen den die Maus gerade belegt
            var horizontalSlot = (int)(dropInfo.PositionInTarget.X / (slotWidth + gap));
            var verticalSlot = (int)(dropInfo.PositionInTarget.Y / (slotHeight + gap));

            // Den Slot innerhalb des Widgets bestimmen in dem der Vorgang gestartet wurde
            var horizontalItemMouseSlot = (int)(DragDropManager.DragInfo.PointInItem.X / (slotWidth + gap));
            var verticalItemMouseSlot = (int)(DragDropManager.DragInfo.PointInItem.Y / (slotHeight + gap));

            var top = Math.Max(0, verticalSlot - verticalItemMouseSlot);
            var left = Math.Max(0, horizontalSlot - horizontalItemMouseSlot);

            var x = (left * slotWidth) + (left * gap);
            var y = (top * slotHeight) + (top * gap);

            return new Point(x, y);
        }
    }
}