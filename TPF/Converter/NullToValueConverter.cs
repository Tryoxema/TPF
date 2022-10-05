using System;
using System.Windows.Data;

namespace TPF.Converter
{
    public abstract class NullToValueConverter<T> : IValueConverter
    {
        private readonly T NullValue;
        private readonly T NotNullValue;

        protected NullToValueConverter(T nullValue, T notNullValue)
        {
            NullValue = nullValue;
            NotNullValue = notNullValue;
        }

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value == null) return NullValue;
            else return NotNullValue;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value == null) return NullValue;
            else return NotNullValue;
        }
    }
}