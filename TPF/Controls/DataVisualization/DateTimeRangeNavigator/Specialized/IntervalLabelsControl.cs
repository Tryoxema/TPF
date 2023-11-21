using System.Windows;
using System.Windows.Controls;

namespace TPF.Controls.Specialized.DateTimeRangeNavigator
{
    public abstract class IntervalLabelsControl : ItemsControl
    {
        static IntervalLabelsControl()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(IntervalLabelsControl), new FrameworkPropertyMetadata(typeof(IntervalLabelsControl)));
        }

        internal abstract LabelType LabelType { get; }
    }
}