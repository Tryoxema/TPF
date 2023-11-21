using System;
using System.Collections;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using TPF.Controls;

namespace TPF.Internal
{
    internal static class InternalExtensions
    {
        internal static void AddHandler(this DependencyObject element, RoutedEvent routedEvent, Delegate handler)
        {
            if (element is UIElement uiElement)
            {
                uiElement.AddHandler(routedEvent, handler);
            }
            else if (element is ContentElement contentElement)
            {
                contentElement.AddHandler(routedEvent, handler);
            }
            else if (element is UIElement3D uiElement3D)
            {
                uiElement3D.AddHandler(routedEvent, handler);
            }
            else
            {
                throw new ArgumentException("Invalid Element");
            }
        }

        internal static void RemoveHandler(this DependencyObject element, RoutedEvent routedEvent, Delegate handler)
        {
            if (element is UIElement uiElement)
            {
                uiElement.RemoveHandler(routedEvent, handler);
            }
            else if (element is ContentElement contentElement)
            {
                contentElement.RemoveHandler(routedEvent, handler);
            }
            else if (element is UIElement3D uiElement3D)
            {
                uiElement3D.RemoveHandler(routedEvent, handler);
            }
            else
            {
                throw new ArgumentException("Invalid Element");
            }
        }

        internal static Panel GetItemsPanel(this DependencyObject element)
        {
            var control = element as Control;
            
            control?.ApplyTemplate();

            var childrenCount = VisualTreeHelper.GetChildrenCount(element);

            for (int i = 0; i < childrenCount; i++)
            {
                if (VisualTreeHelper.GetChild(element, i) is UIElement child)
                {
                    // Wenn es sich um ein Panel handelt und das nächste höhere Element ein ItemsPresenter ist, dann haben wir das ItemsPanel gefunden
                    if (child is Panel panel && VisualTreeHelper.GetParent(child) is ItemsPresenter) return panel;

                    panel = GetItemsPanel(child);

                    if (panel != null) return panel;
                }
            }

            return null;
        }

        internal static Type GetItemContainerType(this ItemsControl itemsControl)
        {
            // Wenn es keine Items gibt haben wir keine Möglichkeit den Typen zu bestimmen
            if (itemsControl.Items.Count == 0) return null;

            foreach (var itemsPresenter in itemsControl.ChildrenOfType<ItemsPresenter>())
            {
                if (VisualTreeHelper.GetChildrenCount(itemsPresenter) == 0) continue;

                // Das Child-Element von einem ItemsPresenter sollte eigentlich immer sein Panel sein
                var panel = VisualTreeHelper.GetChild(itemsPresenter, 0);

                if (VisualTreeHelper.GetChildrenCount(panel) > 0)
                {
                    var itemContainer = VisualTreeHelper.GetChild(panel, 0);

                    if (!(itemContainer is GroupItem) && itemsControl.ItemContainerGenerator.IndexFromContainer(itemContainer) != -1)
                    {
                        return itemContainer.GetType();
                    }
                }
            }

            return null;
        }

        internal static UIElement GetItemContainer(this ItemsControl itemsControl, DependencyObject child)
        {
            if (itemsControl.ItemContainerGenerator.Items.Count > 0)
            {
                // Zuerst versuchen wir, ob wir den ersten Container nehmen können
                var firstContainer = itemsControl.ItemContainerGenerator.ContainerFromIndex(0);

                var type = firstContainer?.GetType();
                // Wenn der erste Container nicht vorhanden war (z.B. durch Virtualisierung) versuchen wir es anders
                if (type == null) type = GetItemContainerType(itemsControl);
                // Falls wir den Typen rausgefunden haben, nehmen wir das entsprechende Parent-Element des korrekten Typens als Container
                if (type != null) return (UIElement)child.ParentFromType(type);
            }

            return null;
        }

        internal static UIElement GetItemContainerAt(this ItemsControl itemsControl, Point position)
        {
            var inputElement = itemsControl.InputHitTest(position);

            if (inputElement is UIElement uiElement)
            {
                return GetItemContainer(itemsControl, uiElement);
            }

            if (inputElement is ContentElement contentElement)
            {
                return GetItemContainer(itemsControl, contentElement);
            }

            return null;
        }

        internal static bool AllowsMultiSelection(this ItemsControl itemsControl)
        {
            if (itemsControl is MultiSelector multiSelector)
            {
                // Cheats
                var propertyInfo = multiSelector.GetType().GetProperty("CanSelectMultipleItems", BindingFlags.Instance | BindingFlags.NonPublic);
                return propertyInfo != null && (bool)propertyInfo.GetValue(multiSelector, null);
            }
            else if (itemsControl is ListBox listBox)
            {
                return listBox.SelectionMode != SelectionMode.Single;
            }
            else return false;
        }

        internal static object GetSelectedItem(this ItemsControl itemsControl)
        {
            if (itemsControl is MultiSelector) return ((MultiSelector)itemsControl).SelectedItem;
            else if (itemsControl is ListBox) return ((ListBox)itemsControl).SelectedItem;
            else if (itemsControl is TreeView) return ((TreeView)itemsControl).GetValue(TreeView.SelectedItemProperty);
            else if (itemsControl is Selector) return ((Selector)itemsControl).SelectedItem;

            return null;
        }

        internal static IEnumerable GetSelectedItems(this ItemsControl itemsControl)
        {
            if (typeof(MultiSelector).IsAssignableFrom(itemsControl.GetType()))
            {
                return ((MultiSelector)itemsControl).SelectedItems;
            }
            else if (itemsControl is ListBox listBox)
            {
                if (listBox.SelectionMode == SelectionMode.Single) return Enumerable.Repeat(listBox.SelectedItem, 1);
                else return listBox.SelectedItems;
            }
            else if (typeof(TreeView).IsAssignableFrom(itemsControl.GetType()))
            {
                return Enumerable.Repeat(((TreeView)itemsControl).SelectedItem, 1);
            }
            else if (typeof(Selector).IsAssignableFrom(itemsControl.GetType()))
            {
                return Enumerable.Repeat(((Selector)itemsControl).SelectedItem, 1);
            }
            else return Enumerable.Empty<object>();
        }

        internal static void SetItemSelected(this ItemsControl itemsControl, object item, bool itemSelected)
        {
            if (itemsControl is MultiSelector multiSelector)
            {
                if (multiSelector.AllowsMultiSelection())
                {
                    if (itemSelected) multiSelector.SelectedItems.Add(item);
                    else multiSelector.SelectedItems.Remove(item);
                }
                else
                {
                    multiSelector.SelectedItem = null;

                    if (itemSelected) multiSelector.SelectedItem = item;
                }
            }
            else if (itemsControl is ListBox listBox)
            {
                if (listBox.SelectionMode != SelectionMode.Single)
                {
                    if (itemSelected) listBox.SelectedItems.Add(item);
                    else listBox.SelectedItems.Remove(item);
                }
                else
                {
                    listBox.SelectedItem = null;

                    if (itemSelected) listBox.SelectedItem = item;
                }
            }
            else
            {
                if (itemSelected) itemsControl.SetSelectedItem(item);
                else itemsControl.SetSelectedItem(null);
            }
        }

        internal static void SetSelectedItem(this ItemsControl itemsControl, object item)
        {
            if (itemsControl is MultiSelector multiSelector)
            {
                multiSelector.SelectedItem = null;
                multiSelector.SelectedItem = item;
            }
            else if (itemsControl is ListBox listBox)
            {
                var selectionMode = listBox.SelectionMode;

                try
                {
                    // SelectionMode ändern wegen UpdateAnchorAndActionItem
                    listBox.SelectionMode = SelectionMode.Single;
                    listBox.SelectedItem = null;
                    listBox.SelectedItem = item;
                }
                finally
                {
                    listBox.SelectionMode = selectionMode;
                }
            }
            else if (itemsControl is TreeViewItem treeViewItem)
            {
                // SelectedItem leeren
                var parentTreeView = ItemsControl.ItemsControlFromItemContainer(treeViewItem);

                if (parentTreeView != null)
                {
                    var previousSelectedItem = parentTreeView.GetValue(TreeView.SelectedItemProperty);

                    if (previousSelectedItem != null)
                    {
                        if (parentTreeView.ItemContainerGenerator.ContainerFromItem(previousSelectedItem) is TreeViewItem previousSelectedTreeViewItem)
                        {
                            previousSelectedTreeViewItem.IsSelected = false;
                        }
                    }
                }

                if (treeViewItem.ItemContainerGenerator.ContainerFromItem(item) is TreeViewItem treeViewItem2)
                {
                    treeViewItem2.IsSelected = true;
                }
            }
            else if (itemsControl is TreeView treeView)
            {
                // SelectedItem leeren
                var previousSelectedItem = treeView.GetValue(TreeView.SelectedItemProperty);

                if (previousSelectedItem != null)
                {
                    if (treeView.ItemContainerGenerator.ContainerFromItem(previousSelectedItem) is TreeViewItem previousSelectedTreeViewItem) previousSelectedTreeViewItem.IsSelected = false;
                }

                if (treeView.ItemContainerGenerator.ContainerFromItem(item) is TreeViewItem treeViewItem2) treeViewItem2.IsSelected = true;
            }
            else if (itemsControl is Selector selector)
            {
                selector.SelectedItem = null;
                selector.SelectedItem = item;
            }
        }

        internal static BitmapSource ToBitmapSource(this Visual visual)
        {
            if (visual == null) return null;

            var dpiX = DpiHelper.DpiX;
            var dpiY = DpiHelper.DpiY;

            var bounds = VisualTreeHelper.GetDescendantBounds(visual);
            var dpiBounds = DpiHelper.LogicalRectToDevice(bounds);

            var pixelWidth = (int)Math.Ceiling(dpiBounds.Width);
            var pixelHeight = (int)Math.Ceiling(dpiBounds.Height);

            if (pixelWidth < 0 || pixelHeight < 0) return null;

            var bitmap = new RenderTargetBitmap(pixelWidth, pixelHeight, dpiX, dpiY, PixelFormats.Pbgra32);

            var drawingVisual = new DrawingVisual();
            using (var context = drawingVisual.RenderOpen())
            {
                var brush = new VisualBrush(visual);

                context.DrawRectangle(brush, null, new Rect(new Point(), bounds.Size));
            }

            bitmap.Render(drawingVisual);

            return bitmap;
        }

        internal static Size CalculateSize(this Thickness thick)
        {
            return new Size(thick.Left + thick.Right, thick.Top + thick.Bottom);
        }

        internal static Rect Deflate(this Rect rect, Thickness thickness)
        {
            return new Rect(rect.Left + thickness.Left,
                            rect.Top + thickness.Top,
                            Math.Max(0.0, rect.Width - thickness.Left - thickness.Right),
                            Math.Max(0.0, rect.Height - thickness.Top - thickness.Bottom));
        }

        internal static bool IsZero(this Thickness thickness)
        {
            return thickness.Left.IsZero()
                   && thickness.Top.IsZero()
                   && thickness.Right.IsZero()
                   && thickness.Bottom.IsZero();
        }

        internal static bool IsZero(this CornerRadius cornerRadius)
        {
            return cornerRadius.TopLeft.IsZero()
                   && cornerRadius.TopRight.IsZero()
                   && cornerRadius.BottomLeft.IsZero()
                   && cornerRadius.BottomRight.IsZero();
        }

        internal static bool IsUniform(this Thickness thickness)
        {
            if (!thickness.Left.IsCloseOrEqual(thickness.Right)) return false;
            if (!thickness.Top.IsCloseOrEqual(thickness.Bottom)) return false;
            if (!thickness.Left.IsCloseOrEqual(thickness.Top)) return false;

            return true;
        }

        internal static bool IsUniform(this CornerRadius cornerRadius)
        {
            if (!cornerRadius.BottomLeft.IsCloseOrEqual(cornerRadius.BottomRight)) return false;
            if (!cornerRadius.TopLeft.IsCloseOrEqual(cornerRadius.TopRight)) return false;
            if (!cornerRadius.BottomLeft.IsCloseOrEqual(cornerRadius.TopRight)) return false;

            return true;
        }

        internal static bool IsOpaqueSolidColorBrush(this Brush brush)
        {
            return (brush as SolidColorBrush)?.Color.A == 0xff;
        }
    }
}