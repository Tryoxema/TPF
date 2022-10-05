using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace TPF.Converter
{
    public class CornerRadiusToSpecificCornerRadiusConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var cornerRadius = (CornerRadius)value;

            if (parameter != null)
            {
                var parts = parameter.ToString().Split(',');

                if (parts.Length == 2)
                {
                    var top = parts[0] == "#" ? cornerRadius.TopLeft : double.Parse(parts[0], CultureInfo.InvariantCulture);
                    var bottom = parts[1] == "#" ? cornerRadius.BottomLeft : double.Parse(parts[1], CultureInfo.InvariantCulture);

                    return new CornerRadius(top, top, bottom, bottom);
                }
                else if (parts.Length == 4)
                {
                    var topLeft = parts[0] == "#" ? cornerRadius.TopLeft : double.Parse(parts[0], CultureInfo.InvariantCulture);
                    var topRight = parts[1] == "#" ? cornerRadius.TopRight : double.Parse(parts[1], CultureInfo.InvariantCulture);
                    var bottomRight = parts[2] == "#" ? cornerRadius.BottomRight : double.Parse(parts[2], CultureInfo.InvariantCulture);
                    var bottomLeft = parts[3] == "#" ? cornerRadius.BottomLeft : double.Parse(parts[3], CultureInfo.InvariantCulture);

                    return new CornerRadius(topLeft, topRight, bottomRight, bottomLeft);
                }
            }

            return cornerRadius;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}