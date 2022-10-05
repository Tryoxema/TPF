using System;
using System.Collections;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using TPF.Internal;
using TPF.DragDrop.Behaviors;

namespace TPF.DragDrop
{
    public class DragInfo
    {
        public DragInfo(object sender, MouseButtonEventArgs e)
        {
            SourceElement = sender as UIElement;
            StartingPoint = e.GetPosition(SourceElement);

            var originalSource = e.OriginalSource as UIElement;

            if (originalSource == null && e.OriginalSource is FrameworkContentElement contentElement)
            {
                originalSource = contentElement.Parent as UIElement;
            }

            if (sender is ItemsControl itemsControl)
            {
                UIElement item = null;

                if (originalSource != null) item = itemsControl.GetItemContainer(originalSource);

                if (item != null)
                {
                    PointInItem = e.GetPosition(item);

                    var itemParent = ItemsControl.ItemsControlFromItemContainer(item);

                    if (itemParent != null)
                    {
                        SourceCollection = itemParent.ItemsSource ?? itemParent.Items;
                        SourceIndex = itemParent.ItemContainerGenerator.IndexFromContainer(item);
                        SourceItem = itemParent.ItemContainerGenerator.ItemFromContainer(item);
                    }
                    else SourceIndex = -1;

                    SourceElementItem = item;
                }
                else SourceCollection = itemsControl.ItemsSource ?? itemsControl.Items;
            }
            else
            {
                // Schauen ob es sich bei unserem Element um einen ItemContainer handelt
                itemsControl = ItemsControl.ItemsControlFromItemContainer(SourceElement);

                if (itemsControl != null)
                {
                    SourceCollection = itemsControl.ItemsSource ?? itemsControl.Items;
                    SourceIndex = itemsControl.ItemContainerGenerator.IndexFromContainer(SourceElement);
                    SourceItem = itemsControl.ItemContainerGenerator.ItemFromContainer(SourceElement);
                }
                else
                {
                    SourceItem = (sender as FrameworkElement)?.DataContext;
                }

                PointInItem = StartingPoint;
                SourceElementItem = originalSource;
            }
        }

        public Point StartingPoint { get; private set; }

        public Point PointInItem { get; private set; }

        public Point VisualOffset { get; set; }

        public UIElement SourceElement { get; private set; }

        public UIElement SourceElementItem { get; private set; }

        public object SourceItem { get; private set; }

        public IEnumerable SourceCollection { get; private set; }

        public int SourceIndex { get; private set; }

        public DragDropEffects Effects { get; set; }

        internal DragDropState LastDragDropState { get; set; }
    }
}