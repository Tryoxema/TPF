using System.Windows;
using System.Windows.Controls.Primitives;

namespace TPF.Controls.Specialized.TabControl
{
    public class TabControlDropdownItem : ButtonBase
    {
        static TabControlDropdownItem()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(TabControlDropdownItem), new FrameworkPropertyMetadata(typeof(TabControlDropdownItem)));
        }

        public TabControlDropdownItem(TabItem tabItem)
        {
            TabItem = tabItem;
            DataContext = TabItem;
        }

        public TabItem TabItem { get; internal set; }

        protected override void OnClick()
        {
            base.OnClick();

            TabItem.IsSelected = true;
        }
    }
}