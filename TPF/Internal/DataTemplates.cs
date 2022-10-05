using System.Windows;
using System.Windows.Controls;

namespace TPF.Internal
{
    internal static class DataTemplates
    {
        static DataTemplates()
        {
            var template = new DataTemplate();
            var factory = new FrameworkElementFactory(typeof(TextBlock));
            factory.SetValue(TextBlock.TextProperty, new TemplateBindingExtension(ContentPresenter.ContentProperty));
            template.VisualTree = factory;
            template.Seal();
            StringTemplate = template;
        }

        internal static DataTemplate StringTemplate { get; private set; }
    }
}