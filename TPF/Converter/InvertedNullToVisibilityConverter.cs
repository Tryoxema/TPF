using System.Windows;

namespace TPF.Converter
{
    public class InvertedNullToVisibilityConverter : NullToValueConverter<Visibility>
    {
        public InvertedNullToVisibilityConverter() : base(Visibility.Visible, Visibility.Collapsed) { }
    }
}