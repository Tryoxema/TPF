using System;
using System.Windows;
using System.Collections;
using System.Windows.Input;

namespace TPF.DragDrop.Behaviors
{
    public class DragDropState
    {
        public UIElement SourceControl { get; protected internal set; }

        public IList SourceItemsSource { get; protected internal set; }

        public UIElement TargetControl { get; protected internal set; }

        public IList TargetItemsSource { get; protected internal set; }

        public IEnumerable DraggedItems { get; protected internal set; }

        public int InsertIndex { get; set; }

        public bool IsControlDown { get; protected internal set; }

        public bool IsShiftDown { get; protected internal set; }

        private DragEventArgs _eventArgs;

        protected internal void SetDragEventArgs(DragEventArgs e)
        {
            _eventArgs = e;
        }

        public Point GetPosition(IInputElement relativeTo)
        {
            if (_eventArgs != null) return _eventArgs.GetPosition(relativeTo);
            else return Mouse.GetPosition(relativeTo);
        }
    }
}