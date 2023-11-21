using System.Windows;
using System.Windows.Controls;

namespace TPF.Controls.Specialized.DateTimeRangeNavigator
{
    public class GroupIntervalLabel : Control
    {
        static GroupIntervalLabel()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(GroupIntervalLabel), new FrameworkPropertyMetadata(typeof(GroupIntervalLabel)));
        }
    }
}