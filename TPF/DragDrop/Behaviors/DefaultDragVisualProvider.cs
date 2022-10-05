using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using TPF.Internal;

namespace TPF.DragDrop.Behaviors
{
    public class DefaultDragVisualProvider : IDragVisualProvider
    {
        public FrameworkElement CreateDragVisual(DragVisualProviderData providerData)
        {
            var count = providerData.ItemContainers.Count();

            if (count == 1)
            {
                if (providerData.ItemContainers.First() is Visual visual)
                {
                    var imageSource = visual.ToBitmapSource();

                    var image = new Image()
                    {
                        Source = imageSource,
                        Width = imageSource.Width,
                        Height = imageSource.Height,
                        HorizontalAlignment = HorizontalAlignment.Left,
                        VerticalAlignment = VerticalAlignment.Top
                    };

                    image.SetValue(RenderOptions.EdgeModeProperty, EdgeMode.Aliased);
                    image.SetValue(RenderOptions.BitmapScalingModeProperty, BitmapScalingMode.HighQuality);

                    var dragVisual = new ContentControl()
                    {
                        Content = image,
                        Opacity = providerData.Opacity
                    };

                    return dragVisual;
                }
                else return null;
            }
            else
            {
                var images = new List<Image>(count);

                foreach (var item in providerData.ItemContainers)
                {
                    if (item is Visual visual)
                    {
                        var imageSource = visual.ToBitmapSource();

                        var image = new Image()
                        {
                            Source = imageSource,
                            Width = imageSource.Width,
                            Height = imageSource.Height,
                            HorizontalAlignment = HorizontalAlignment.Left,
                            VerticalAlignment = VerticalAlignment.Top
                        };

                        image.SetValue(RenderOptions.EdgeModeProperty, EdgeMode.Aliased);
                        image.SetValue(RenderOptions.BitmapScalingModeProperty, BitmapScalingMode.HighQuality);

                        images.Add(image);
                    }
                }

                var dragVisual = new ItemsControl()
                {
                    ItemsSource = images,
                    Opacity = providerData.Opacity
                };

                return dragVisual;
            }
        }
    }
}