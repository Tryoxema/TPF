using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;
using TPF.Internal;

namespace TPF.Converter
{
    public class HueToBrushConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (double.TryParse(value.ToString(), out var result))
            {
                var color = ColorHelper.ConvertHsvToRgb(360 * result, 1, 1);
                return new SolidColorBrush(color);
            }
            else return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}