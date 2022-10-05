using System.Windows;
using System.Windows.Controls;

namespace TPF.Controls
{
    public class StepItemIndicatorTemplateSelector : DataTemplateSelector
    {
        public DataTemplate CompleteTemplate { get; set; }

        public DataTemplate InactiveTemplate { get; set; }

        public DataTemplate IndeterminateTemplate { get; set; }

        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            StepItem stepItem = null;

            if (item is StepItem)
            {
                stepItem = (StepItem)item;
            }
            else if (container is StepItem)
            {
                stepItem = (StepItem)container;
            }

            if (stepItem != null)
            {
                switch (stepItem.StepStatus)
                {
                    case StepStatus.Complete: return CompleteTemplate;
                    case StepStatus.Inactive: return InactiveTemplate;
                    case StepStatus.Indeterminate: return IndeterminateTemplate;
                }
            }

            return null;
        }
    }
}