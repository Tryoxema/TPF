using System;
using System.Linq;
using System.Windows;
using System.Globalization;
using System.Windows.Data;

namespace TPF.Converter
{
    public class RelativePositionMultiValueConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values.Any(x => x == null || x == DependencyProperty.UnsetValue)) return new Point(0, 0);

            var parent = values[0] as UIElement;
            var control = values[1] as UIElement;
            var position = (Point)values[2];
            var relativePosition = parent.TranslatePoint(position, control);

            return relativePosition;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}