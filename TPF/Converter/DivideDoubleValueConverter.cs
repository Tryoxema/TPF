using System;
using System.Globalization;
using System.Windows.Data;

namespace TPF.Converter
{
    public class DivideDoubleValueConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var doubleValue = (double)value;

            if (parameter != null)
            {
                var parameterString = parameter.ToString();

                if (double.TryParse(parameterString, out var result))
                {
                    return doubleValue / result;
                }
            }

            return doubleValue;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}