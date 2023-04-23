using System;
using System.Globalization;
using System.Windows.Data;

namespace TPF.Converter
{
    public class DateTimeToAngleConverter : IValueConverter
    {
        public TimePart TimePart { get; set; }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null) return 0.0;

            var dateTime = (DateTime)value;

            var result = 0.0;

            switch (TimePart)
            {
                case TimePart.Hour:
                {
                    result = dateTime.Hour % 12 * (360 / 12);
                    break;
                }
                case TimePart.Minute:
                {
                    result = dateTime.Minute * (360 / 60);
                    break;
                }
                case TimePart.Second:
                {
                    result = dateTime.Second * (360 / 60);
                    break;
                }
            }

            return result;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return Binding.DoNothing;
        }
    }

    public enum TimePart
    {
        Hour,
        Minute,
        Second
    }
}