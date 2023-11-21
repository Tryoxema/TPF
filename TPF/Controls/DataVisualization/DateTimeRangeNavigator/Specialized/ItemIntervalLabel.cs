using System.Windows;
using System.Windows.Controls;
using TPF.Internal;

namespace TPF.Controls.Specialized.DateTimeRangeNavigator
{
    public class ItemIntervalLabel : Control
    {
        static ItemIntervalLabel()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ItemIntervalLabel), new FrameworkPropertyMetadata(typeof(ItemIntervalLabel)));
        }

        #region IsHighlighted ReadOnly DependencyProperty
        private static readonly DependencyPropertyKey IsHighlightedPropertyKey = DependencyProperty.RegisterReadOnly("IsHighlighted",
            typeof(bool),
            typeof(ItemIntervalLabel),
            new PropertyMetadata(BooleanBoxes.FalseBox));

        public static readonly DependencyProperty IsHighlightedProperty = IsHighlightedPropertyKey.DependencyProperty;

        public bool IsHighlighted
        {
            get { return (bool)GetValue(IsHighlightedProperty); }
            internal set { SetValue(IsHighlightedPropertyKey, BooleanBoxes.Box(value)); }
        }
        #endregion
    }
}