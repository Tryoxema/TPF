using System.Windows;
using System.Windows.Controls;

namespace TPF.Controls.Specialized.Sparkline
{
    public class IndicatorItem : Control
    {
        static IndicatorItem()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(IndicatorItem), new FrameworkPropertyMetadata(typeof(IndicatorItem)));
        }

        #region Type DependencyProperty
        public static readonly DependencyProperty TypeProperty = DependencyProperty.Register("Type",
            typeof(IndicatorType),
            typeof(IndicatorItem),
            new PropertyMetadata(IndicatorType.Normal));

        public IndicatorType Type
        {
            get { return (IndicatorType)GetValue(TypeProperty); }
            set { SetValue(TypeProperty, value); }
        }
        #endregion

        #region ToolTipTemplate DependencyProperty
        public static readonly DependencyProperty ToolTipTemplateProperty = DependencyProperty.Register("ToolTipTemplate",
            typeof(DataTemplate),
            typeof(IndicatorItem),
            new PropertyMetadata(null));

        public DataTemplate ToolTipTemplate
        {
            get { return (DataTemplate)GetValue(ToolTipTemplateProperty); }
            set { SetValue(ToolTipTemplateProperty, value); }
        }
        #endregion

        internal double RelativeX { get; set; }

        internal double RelativeY { get; set; }
    }
}