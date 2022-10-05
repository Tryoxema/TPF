using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace TPF.Converter
{
    public class ThicknessToSpecificThicknessConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var thickness = (Thickness)value;

            if (parameter != null)
            {
                var parts = parameter.ToString().Split(',');

                if (parts.Length == 2)
                {
                    var leftRight = parts[0] == "#" ? thickness.Left : double.Parse(parts[0], CultureInfo.InvariantCulture);
                    var topBottom = parts[1] == "#" ? thickness.Top : double.Parse(parts[1], CultureInfo.InvariantCulture);

                    return new Thickness(leftRight, topBottom, leftRight, topBottom);
                }
                else if (parts.Length == 4)
                {
                    var left = parts[0] == "#" ? thickness.Left : double.Parse(parts[0], CultureInfo.InvariantCulture);
                    var top = parts[1] == "#" ? thickness.Top : double.Parse(parts[1], CultureInfo.InvariantCulture);
                    var right = parts[2] == "#" ? thickness.Right : double.Parse(parts[2], CultureInfo.InvariantCulture);
                    var bottom = parts[3] == "#" ? thickness.Bottom : double.Parse(parts[3], CultureInfo.InvariantCulture);

                    return new Thickness(left, top, right, bottom);
                }
            }

            return thickness;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}