using System;
using System.Windows;
using System.Windows.Controls;

namespace TPF.Controls
{
    public class Widget : HeaderedContentControl
    {
        static Widget()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(Widget), new FrameworkPropertyMetadata(typeof(Widget)));
        }

        public Widget()
        {
            DragDrop.DragDropManager.SetAllowDrag(this, true);
        }

        #region CornerRadius DependencyProperty
        public static readonly DependencyProperty CornerRadiusProperty = DependencyProperty.Register("CornerRadius",
            typeof(CornerRadius),
            typeof(Widget),
            new PropertyMetadata(default(CornerRadius)));

        public CornerRadius CornerRadius
        {
            get { return (CornerRadius)GetValue(CornerRadiusProperty); }
            set { SetValue(CornerRadiusProperty, value); }
        }
        #endregion

        #region HeaderAlignment DependencyProperty
        public static readonly DependencyProperty HeaderAlignmentProperty = DependencyProperty.Register("HeaderAlignment",
            typeof(HorizontalAlignment),
            typeof(Widget),
            new PropertyMetadata(HorizontalAlignment.Left));

        public HorizontalAlignment HeaderAlignment
        {
            get { return (HorizontalAlignment)GetValue(HeaderAlignmentProperty); }
            set { SetValue(HeaderAlignmentProperty, value); }
        }
        #endregion

        #region HorizontalSlots DependencyProperty
        public static readonly DependencyProperty HorizontalSlotsProperty = DependencyProperty.Register("HorizontalSlots",
            typeof(int),
            typeof(Widget),
            new PropertyMetadata(1, OnLayoutPropertyChanged, CoerceSlot));

        public int HorizontalSlots
        {
            get { return (int)GetValue(HorizontalSlotsProperty); }
            set { SetValue(HorizontalSlotsProperty, value); }
        }
        #endregion

        #region VerticalSlots DependencyProperty
        public static readonly DependencyProperty VerticalSlotsProperty = DependencyProperty.Register("VerticalSlots",
            typeof(int),
            typeof(Widget),
            new PropertyMetadata(1, OnLayoutPropertyChanged, CoerceSlot));

        public int VerticalSlots
        {
            get { return (int)GetValue(VerticalSlotsProperty); }
            set { SetValue(VerticalSlotsProperty, value); }
        }
        #endregion

        #region Top DependencyProperty
        public static readonly DependencyProperty TopProperty = DependencyProperty.Register("Top",
            typeof(int),
            typeof(Widget),
            new PropertyMetadata(0, OnLayoutPropertyChanged, CoercePosition));

        public int Top
        {
            get { return (int)GetValue(TopProperty); }
            set { SetValue(TopProperty, value); }
        }
        #endregion

        #region Left DependencyProperty
        public static readonly DependencyProperty LeftProperty = DependencyProperty.Register("Left",
            typeof(int),
            typeof(Widget),
            new PropertyMetadata(0, OnLayoutPropertyChanged, CoercePosition));

        public int Left
        {
            get { return (int)GetValue(LeftProperty); }
            set { SetValue(LeftProperty, value); }
        }
        #endregion

        private bool _settingPosition;

        internal bool InvalidPosition { get; set; }

        private static object CoerceSlot(DependencyObject sender, object value)
        {
            var intValue = (int)value;

            if (intValue < 1) intValue = 1;

            return intValue;
        }

        private static object CoercePosition(DependencyObject sender, object value)
        {
            var intValue = (int)value;

            if (intValue < 0) intValue = 0;

            return intValue;
        }

        private static void OnLayoutPropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            var instance = (Widget)sender;

            instance.OnLayoutPropertyChanged();
        }

        internal void SetPosition(int top, int left)
        {
            _settingPosition = true;
            Top = top;
            Left = left;
            _settingPosition = false;
        }

        private void OnLayoutPropertyChanged()
        {
            if (_settingPosition) return;

            Dashboard?.InvalidateWidgets();
        }

        public Dashboard Dashboard
        {
            get { return ItemsControl.ItemsControlFromItemContainer(this) as Dashboard; }
        }
    }
}