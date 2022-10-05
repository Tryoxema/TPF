using System;
using System.Windows;

namespace TPF.DragDrop
{
    public sealed class DragDropCompletedEventArgs : RoutedEventArgs
    {
        internal DragDropCompletedEventArgs(DragDropEffects effects, object data) : base(DragDropManager.DragDropCompletedEvent)
        {
            Effects = effects;
            Data = data;
        }

        public DragDropEffects Effects { get; }

        public object Data { get; set; }
    }

    public delegate void DragDropCompletedEventHandler(object sender, DragDropCompletedEventArgs e);
}