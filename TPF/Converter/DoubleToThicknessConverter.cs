using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace TPF.Converter
{
    public class DoubleToThicknessConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var doubleValue = (double)value;

            if (parameter != null)
            {
                var parts = parameter.ToString().Split(',');

                if (parts.Length == 2)
                {
                    var first = parts[0] == "#" ? doubleValue : parts[0] == "-#" ? -doubleValue : double.Parse(parts[0], CultureInfo.InvariantCulture);
                    var second = parts[1] == "#" ? doubleValue : parts[1] == "-#" ? -doubleValue : double.Parse(parts[1], CultureInfo.InvariantCulture);

                    return new Thickness(first, second, first, second);
                }
                else if (parts.Length == 4)
                {
                    var first = parts[0] == "#" ? doubleValue : parts[0] == "-#" ? -doubleValue : double.Parse(parts[0], CultureInfo.InvariantCulture);
                    var second = parts[1] == "#" ? doubleValue : parts[1] == "-#" ? -doubleValue : double.Parse(parts[1], CultureInfo.InvariantCulture);
                    var third = parts[2] == "#" ? doubleValue : parts[2] == "-#" ? -doubleValue : double.Parse(parts[2], CultureInfo.InvariantCulture);
                    var fourth = parts[3] == "#" ? doubleValue : parts[3] == "-#" ? -doubleValue : double.Parse(parts[3], CultureInfo.InvariantCulture);

                    return new Thickness(first, second, third, fourth);
                }
            }

            return new Thickness(doubleValue);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}