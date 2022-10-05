using System;

namespace TPF.Controls
{
    public class ItemSelectedEventArgs : EventArgs
    {
        public object SelectedItem { get; }

        public object SelectedValue { get; }

        public ItemSelectedEventArgs() { }

        public ItemSelectedEventArgs(object item)
        {
            SelectedItem = item;
        }

        public ItemSelectedEventArgs(object item, object value)
        {
            SelectedItem = item;
            SelectedValue = value;
        }
    }
}