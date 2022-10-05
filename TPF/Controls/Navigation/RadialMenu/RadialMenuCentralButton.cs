using System;
using System.Windows;
using System.Windows.Controls;

namespace TPF.Controls
{
    public class RadialMenuCentralButton : Button
    {
        static RadialMenuCentralButton()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(RadialMenuCentralButton), new FrameworkPropertyMetadata(typeof(RadialMenuCentralButton)));
        }

        #region BackContentTemplate DependencyProperty
        public static readonly DependencyProperty BackContentTemplateProperty = DependencyProperty.Register("BackContentTemplate",
            typeof(DataTemplate),
            typeof(RadialMenuCentralButton),
            new PropertyMetadata(null));

        public DataTemplate BackContentTemplate
        {
            get { return (DataTemplate)GetValue(BackContentTemplateProperty); }
            set { SetValue(BackContentTemplateProperty, value); }
        }
        #endregion

        internal RadialMenu Menu;

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            Menu = this.ParentOfType<RadialMenu>();

            if (Menu == null) return;

            Menu.Navigated += (s, e) =>
            {
                if (Menu.SubMenuItemsPath.Count > 0) VisualStateManager.GoToState(this, "Back", true);
                else VisualStateManager.GoToState(this, "Base", true);
            };
        }

        protected override void OnClick()
        {
            if (Menu == null) return;

            if (Menu.SubMenuItemsPath.Count > 0) Menu.NavigateBack();
            else Menu.IsOpen = !Menu.IsOpen;
        }
    }
}