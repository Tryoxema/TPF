using System;
using System.Linq;
using System.Collections;
using System.Windows;
using System.Windows.Controls;

namespace TPF.DragDrop.Behaviors
{
    public class ListBoxDragDropHelper : DragDropHelper<ListBoxDragDropBehavior, DragDropState>
    {
        public ListBoxDragDropHelper()
        {

        }

        protected override FrameworkElement GetDragSource(UIElement element)
        {
            if (element is ListBox listBox)
            {
                return listBox;
            }
            else if (element is ListBoxItem item)
            {
                return ItemsControl.ItemsControlFromItemContainer(item);
            }
            else return null;
        }

        protected override IEnumerable GetDraggedItems(FrameworkElement dragSource)
        {
            if (dragSource is ListBox listBox)
            {
                return listBox.SelectedItems.Cast<object>().ToList();
            }
            else return null;
        }
    }
}