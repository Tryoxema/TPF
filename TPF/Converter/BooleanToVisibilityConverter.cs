using System.Windows;

namespace TPF.Converter
{
    public class BooleanToVisibilityConverter : BooleanToValueConverter<Visibility>
    {
        public BooleanToVisibilityConverter() : base(Visibility.Visible, Visibility.Collapsed) { }
    }
}