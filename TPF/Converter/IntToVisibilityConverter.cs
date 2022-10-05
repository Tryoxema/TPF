using System;
using System.Windows;
using System.Globalization;
using System.Windows.Data;

namespace TPF.Converter
{
    public class IntToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var visibility = Visibility.Collapsed;

            var valueString = value?.ToString();

            if (!string.IsNullOrWhiteSpace(valueString) && long.TryParse(valueString, out var result))
            {
                visibility = result > 0 ? Visibility.Visible : Visibility.Collapsed;
            }

            return visibility;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}