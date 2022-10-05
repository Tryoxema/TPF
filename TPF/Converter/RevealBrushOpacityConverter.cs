using System;
using System.Windows.Data;
using System.Globalization;

namespace TPF.Converter
{
    public class RevealBrushOpacityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var boolValue = (bool)value;
            var opacity = (double)parameter;

            return boolValue ? opacity : 0;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}