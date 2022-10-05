using System;
using System.Windows;

namespace TPF.DragDrop
{
    public sealed class DragInitializeEventArgs : RoutedEventArgs
    {
        internal DragInitializeEventArgs(DragInfo info) : base(DragDropManager.DragInitializeEvent)
        {
            SourceElement = info.SourceElement;
            SourceElementItem = info.SourceElementItem;
            SourceItem = info.SourceItem;
            VisualOffset = info.PointInItem;
        }

        public DragDropEffects AllowedEffects { get; set; }

        public bool Cancel { get; set; }

        public object Data { get; set; }

        public object DragVisual { get; set; }

        public UIElement SourceElement { get; }

        public UIElement SourceElementItem { get; }

        public object SourceItem { get; }

        public Point VisualOffset { get; set; }
    }

    public delegate void DragInitializeEventHandler(object sender, DragInitializeEventArgs e);
}