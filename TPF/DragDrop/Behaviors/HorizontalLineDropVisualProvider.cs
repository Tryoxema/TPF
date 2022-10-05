using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace TPF.DragDrop.Behaviors
{
    public class HorizontalLineDropVisualProvider : DependencyObject, IDropVisualProvider
    {
        #region Fill DependencyProperty
        public static readonly DependencyProperty FillProperty = DependencyProperty.Register("Fill",
            typeof(Brush),
            typeof(HorizontalLineDropVisualProvider),
            new PropertyMetadata(Brushes.DodgerBlue));

        public Brush Fill
        {
            get { return (Brush)GetValue(FillProperty); }
            set { SetValue(FillProperty, value); }
        }
        #endregion

        #region Height DependencyProperty
        public static readonly DependencyProperty HeightProperty = DependencyProperty.Register("Height",
            typeof(double),
            typeof(HorizontalLineDropVisualProvider),
            new PropertyMetadata(3.0));

        public double Height
        {
            get { return (double)GetValue(HeightProperty); }
            set { SetValue(HeightProperty, value); }
        }
        #endregion

        #region Width DependencyProperty
        public static readonly DependencyProperty WidthProperty = DependencyProperty.Register("Width",
            typeof(double),
            typeof(HorizontalLineDropVisualProvider),
            new PropertyMetadata(double.NaN));

        public double Width
        {
            get { return (double)GetValue(WidthProperty); }
            set { SetValue(WidthProperty, value); }
        }
        #endregion

        public FrameworkElement CreateDropVisual()
        {
            var rectangle = new Rectangle()
            {
                Height = Height,
                Width = Width,
                Fill = Fill,
                IsHitTestVisible = false
            };

            if (double.IsNaN(rectangle.Width)) rectangle.Stretch = Stretch.UniformToFill;

            return rectangle;
        }

        public Point GetPosition(DropInfo dropInfo)
        {
            if (dropInfo.InsertIndex == -1)
            {
                if (dropInfo.Target is ItemsControl itemsControl)
                {
                    if (itemsControl.Items.Count == 0) return new Point();

                    if (itemsControl.ItemContainerGenerator.ContainerFromIndex(itemsControl.Items.Count - 1) is UIElement lastContainer)
                    {
                        var containerStartPoint = lastContainer.TransformToAncestor(dropInfo.AdornedElement).Transform(new Point());

                        containerStartPoint.Offset(0, lastContainer.RenderSize.Height);

                        return containerStartPoint;
                    }
                }

                return new Point();
            }

            var itemStartPoint = dropInfo.TargetItem.TransformToAncestor(dropInfo.AdornedElement).Transform(new Point());

            if (dropInfo.DropPosition == RelativeDropPosition.After) itemStartPoint.Offset(0, dropInfo.TargetItem.RenderSize.Height);

            return itemStartPoint;
        }
    }
}