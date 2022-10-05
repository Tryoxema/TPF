using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace TPF.Controls.Specialized.TaskBoard
{
    public class TaskBoardCardIconTemplateSelector : DataTemplateSelector
    {
        public DataTemplate ImageTemplate { get; set; }

        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            if (item is ImageSource) return ImageTemplate;

            return base.SelectTemplate(item, container);
        }
    }
}