using System;
using System.Globalization;
using System.Windows.Data;

namespace TPF.Converter
{
    public class ToggleSwitchPositionConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values == null || values.Length != 3) return 0.0;

            var factor = double.Parse(values[0]?.ToString() ?? "0");
            var trackLength = (double)values[1];
            var switchWidth = (double)values[2];

            var result = factor * (trackLength - switchWidth);

            return result;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}