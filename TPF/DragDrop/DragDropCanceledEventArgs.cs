using System;
using System.Windows;

namespace TPF.DragDrop
{
    public sealed class DragDropCanceledEventArgs : RoutedEventArgs
    {
        internal DragDropCanceledEventArgs(object data) : base(DragDropManager.DragDropCanceledEvent)
        {
            Data = data;
        }

        public object Data { get; set; }
    }

    public delegate void DragDropCanceledEventHandler(object sender, DragDropCanceledEventArgs e);
}