using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using TPF.DragDrop;

namespace TPF.Controls
{
    public class TaskBoardItem : ContentControl
    {
        static TaskBoardItem()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(TaskBoardItem), new FrameworkPropertyMetadata(typeof(TaskBoardItem)));
        }

        public TaskBoardItem()
        {
            DragDropManager.SetAllowDrag(this, true);
        }

        public TaskBoardColumn Column
        {
            get { return ItemsControl.ItemsControlFromItemContainer(this) as TaskBoardColumn; }
        }

        private bool _pressed;

        protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed) _pressed = true;

            if (e.ClickCount == 2)
            {
                // Wenn mehr als einmal geklickt wird, dann pressed auf false setzen, um Click nach DoubleClick zu verhindern
                _pressed = false;

                Column?.TaskBoard?.ItemDoubleClicked(this);
            }

            base.OnMouseLeftButtonDown(e);
        }

        protected override void OnMouseLeftButtonUp(MouseButtonEventArgs e)
        {
            if (_pressed)
            {
                Column?.TaskBoard?.ItemClicked(this);
            }

            base.OnMouseLeftButtonUp(e);
        }

        protected override void OnMouseLeave(MouseEventArgs e)
        {
            base.OnMouseLeave(e);

            _pressed = false;
        }
    }
}