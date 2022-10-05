using System;
using System.Collections;
using System.Collections.Generic;
using System.Windows;
using TPF.DragDrop.Behaviors;

namespace TPF.Controls.Specialized.Dashboard
{
    public class DashboardDragDropHelper : DragDropHelper<DashboardDragDropBehavior, DragDropState>
    {
        protected override FrameworkElement GetDragSource(UIElement element)
        {
            if (element is Controls.Dashboard dashboard)
            {
                return dashboard;
            }
            else if (element is Widget widget)
            {
                return widget.Dashboard;
            }
            else return element.ParentOfType<Controls.Dashboard>();
        }

        protected override IEnumerable GetDraggedItems(FrameworkElement dragSource)
        {
            if (dragSource is Widget widget)
            {
                return new List<Widget>() { widget };
            }
            else return null;
        }

        protected override DragDropState CreateState(UIElement sourceElement, UIElement targetElement, DragEventArgs e)
        {
            var state = base.CreateState(sourceElement, targetElement, e);

            state.DraggedItems = GetDraggedItems(sourceElement as FrameworkElement);

            return state;
        }
    }
}