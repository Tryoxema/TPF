using System;
using System.Globalization;
using System.Windows.Data;

namespace TPF.Converter
{
    public class IntToBooleanConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var boolean = false;

            var valueString = value?.ToString();

            if (!string.IsNullOrWhiteSpace(valueString) && long.TryParse(valueString, out var result))
            {
                boolean = result > 0;
            }

            return boolean;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}