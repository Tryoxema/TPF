using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using TPF.Internal;

namespace TPF.Controls
{
    public class ComboBoxItem : ListBoxItem
    {
        static ComboBoxItem()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ComboBoxItem), new FrameworkPropertyMetadata(typeof(ComboBoxItem)));
        }

        #region IsHighlighted ReadOnly DependencyProperty
        private static readonly DependencyPropertyKey IsHighlightedPropertyKey = DependencyProperty.RegisterReadOnly("IsHighlighted",
            typeof(bool),
            typeof(ComboBoxItem),
            new PropertyMetadata(BooleanBoxes.FalseBox));

        public static readonly DependencyProperty IsHighlightedProperty = IsHighlightedPropertyKey.DependencyProperty;

        public bool IsHighlighted
        {
            get { return (bool)GetValue(IsHighlightedProperty); }
            protected set { SetValue(IsHighlightedPropertyKey, BooleanBoxes.Box(value)); }
        } 
        #endregion

        private ComboBox ParentComboBox
        {
            get { return ItemsControl.ItemsControlFromItemContainer(this) as ComboBox; }
        }

        protected override void OnMouseLeftButtonUp(MouseButtonEventArgs e)
        {
            e.Handled = true;

            if (ParentComboBox != null)
            {
                ParentComboBox.Select(this);
            }
            else IsSelected = !IsSelected;

            base.OnMouseLeftButtonUp(e);
        }

        protected override void OnMouseEnter(MouseEventArgs e)
        {
            e.Handled = true;

            if (ParentComboBox != null) ParentComboBox.Highlight(this);

            base.OnMouseEnter(e);
        }

        protected override void OnContentChanged(object oldContent, object newContent)
        {
            base.OnContentChanged(oldContent, newContent);

            if (IsSelected && ParentComboBox != null)
            {
                ParentComboBox.UpdateDisplayedSelectionText();
            }
        }

        internal void SetIsHighlighted(bool value)
        {
            IsHighlighted = value;
        }
    }
}