using System;
using System.Windows.Data;

namespace TPF.Converter
{
    public abstract class BooleanToValueConverter<T> : IValueConverter
    {
        private readonly T TrueValue;
        private readonly T FalseValue;

        protected BooleanToValueConverter(T trueValue, T falseValue)
        {
            TrueValue = trueValue;
            FalseValue = falseValue;
        }

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value == null) return FalseValue;
            return (bool)value ? TrueValue : FalseValue;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value == null) return FalseValue;
            return value.Equals(TrueValue) ? true : false;
        }
    }
}