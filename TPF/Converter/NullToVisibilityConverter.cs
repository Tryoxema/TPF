using System.Windows;

namespace TPF.Converter
{
    public class NullToVisibilityConverter : NullToValueConverter<Visibility>
    {
        public NullToVisibilityConverter() : base(Visibility.Collapsed, Visibility.Visible) { }
    }
}