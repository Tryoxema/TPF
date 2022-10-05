using System;
using System.Windows;

namespace TPF.DragDrop.Behaviors
{
    public interface IDragVisualProvider
    {
        FrameworkElement CreateDragVisual(DragVisualProviderData providerData);
    }
}