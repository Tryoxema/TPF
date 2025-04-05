using System;
using System.Windows;
using System.Windows.Data;

namespace TPF.Demo.Converter
{
    public class VisibliltyToBooleanConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value == null) return false;
            return (Visibility)value == Visibility.Visible ? true : false;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value == null) return Visibility.Collapsed;
            return value.Equals(true) ? Visibility.Visible : Visibility.Collapsed;
        }
    }
}