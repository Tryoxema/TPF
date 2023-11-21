using System.Windows;

namespace TPF.Controls.Specialized.DateTimeRangeNavigator
{
    public class GroupIntervalLabelsControl : IntervalLabelsControl
    {
        static GroupIntervalLabelsControl()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(GroupIntervalLabelsControl), new FrameworkPropertyMetadata(typeof(GroupIntervalLabelsControl)));
        }

        internal override LabelType LabelType
        {
            get
            {
                return LabelType.Group;
            }
        }

        protected override DependencyObject GetContainerForItemOverride()
        {
            return new GroupIntervalLabel();
        }
    }
}