using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using TPF.Internal;

namespace TPF.Controls
{
    public class TabItem : System.Windows.Controls.TabItem
    {
        static TabItem()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(TabItem), new FrameworkPropertyMetadata(typeof(TabItem)));
        }

        #region Icon DependencyProperty
        public static readonly DependencyProperty IconProperty = DependencyProperty.Register("Icon",
            typeof(object),
            typeof(TabItem),
            new PropertyMetadata(null));

        public object Icon
        {
            get { return GetValue(IconProperty); }
            set { SetValue(IconProperty, value); }
        }
        #endregion

        #region IconMargin DependencyProperty
        public static readonly DependencyProperty IconMarginProperty = DependencyProperty.Register("IconMargin",
            typeof(Thickness),
            typeof(TabItem),
            new PropertyMetadata(default(Thickness)));

        public Thickness IconMargin
        {
            get { return (Thickness)GetValue(IconMarginProperty); }
            set { SetValue(IconMarginProperty, value); }
        }
        #endregion

        #region HeaderForeground DependencyProperty
        public static readonly DependencyProperty HeaderForegroundProperty = DependencyProperty.Register("HeaderForeground",
            typeof(Brush),
            typeof(TabItem),
            new PropertyMetadata(null));

        public Brush HeaderForeground
        {
            get { return (Brush)GetValue(HeaderForegroundProperty); }
            set { SetValue(HeaderForegroundProperty, value); }
        }
        #endregion

        #region HeaderToolTip DependencyProperty
        public static readonly DependencyProperty HeaderToolTipProperty = DependencyProperty.Register("HeaderToolTip",
            typeof(object),
            typeof(TabItem),
            new PropertyMetadata(null));

        public object HeaderToolTip
        {
            get { return GetValue(HeaderToolTipProperty); }
            set { SetValue(HeaderToolTipProperty, value); }
        }
        #endregion

        #region ShowCloseButton DependencyProperty
        public static readonly DependencyProperty ShowCloseButtonProperty = TabControl.ShowCloseButtonProperty.AddOwner(typeof(TabItem));

        public bool ShowCloseButton
        {
            get { return (bool)GetValue(ShowCloseButtonProperty); }
            set { SetValue(ShowCloseButtonProperty, BooleanBoxes.Box(value)); }
        }
        #endregion

        #region ShowPinButton DependencyProperty
        public static readonly DependencyProperty ShowPinButtonProperty = TabControl.ShowPinButtonProperty.AddOwner(typeof(TabItem));

        public bool ShowPinButton
        {
            get { return (bool)GetValue(ShowPinButtonProperty); }
            set { SetValue(ShowPinButtonProperty, BooleanBoxes.Box(value)); }
        }
        #endregion

        #region IsPinned DependencyProperty
        public static readonly DependencyProperty IsPinnedProperty = DependencyProperty.Register("IsPinned",
            typeof(bool),
            typeof(TabItem),
            new PropertyMetadata(BooleanBoxes.FalseBox, OnIsPinnedChanged));

        private static void OnIsPinnedChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            var instance = (TabItem)sender;

            instance.OnIsPinnedChanged();
        }

        public bool IsPinned
        {
            get { return (bool)GetValue(IsPinnedProperty); }
            set { SetValue(IsPinnedProperty, BooleanBoxes.Box(value)); }
        }
        #endregion

        #region CloseTabOnMiddleMouseButtonDown DependencyProperty
        public static readonly DependencyProperty CloseTabOnMiddleMouseButtonDownProperty = TabControl.CloseTabOnMiddleMouseButtonDownProperty.AddOwner(typeof(TabItem));

        public bool CloseTabOnMiddleMouseButtonDown
        {
            get { return (bool)GetValue(CloseTabOnMiddleMouseButtonDownProperty); }
            set { SetValue(CloseTabOnMiddleMouseButtonDownProperty, BooleanBoxes.Box(value)); }
        }
        #endregion

        internal TabControl Owner
        {
            get { return ItemsControl.ItemsControlFromItemContainer(this) as TabControl; }
        }

        private UIElement HeaderRoot;

        private bool _ignoreIsPinnedChanged;

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            if (HeaderRoot != null)
            {
                HeaderRoot.MouseDown -= HeaderRoot_MouseDown;
                HeaderRoot.MouseLeftButtonDown -= HeaderRoot_MouseLeftButtonDown;
                HeaderRoot.MouseLeftButtonUp -= HeaderRoot_MouseLeftButtonUp;
            }

            HeaderRoot = GetTemplateChild("PART_Root") as UIElement;

            if (HeaderRoot != null)
            {
                HeaderRoot.MouseDown += HeaderRoot_MouseDown;
                HeaderRoot.MouseLeftButtonDown += HeaderRoot_MouseLeftButtonDown;
                HeaderRoot.MouseLeftButtonUp += HeaderRoot_MouseLeftButtonUp;
            }
        }

        public void Close()
        {
            TabItemCommands.Close.Execute(null, this);
        }

        private void OnIsPinnedChanged()
        {
            if (Owner == null || _ignoreIsPinnedChanged) return;

            // Versuchen den Pinned-Zustand zu ändern
            if (!Owner.TrySetPinnedState(this, IsPinned))
            {
                // Zurücksetzen auf alten Wert wenn abgebrochen wurde
                SetIsPinnedInternal(!IsPinned);
            }
        }

        internal void SetIsPinnedInternal(bool isPinned)
        {
            _ignoreIsPinnedChanged = true;
            IsPinned = isPinned;
            _ignoreIsPinnedChanged = false;
        }

        private void HeaderRoot_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Middle && CloseTabOnMiddleMouseButtonDown)
            {
                TabItemCommands.Close.Execute(null, this);
            }
        }

        private void HeaderRoot_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (Owner == null) return;

            Owner.DragTarget = this;
        }

        private void HeaderRoot_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (Owner == null) return;

            Owner.DragTarget = null;
        }
    }
}