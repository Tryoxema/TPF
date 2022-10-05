using System.Windows;

namespace TPF.Converter
{
    public class InvertedBooleanToVisibilityConverter : BooleanToValueConverter<Visibility>
    {
        public InvertedBooleanToVisibilityConverter() : base(Visibility.Collapsed, Visibility.Visible) { }
    }
}