using System;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace TPF.Converter
{
    public class IsLastItemConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is DependencyObject item)
            {
                var itemsControl = ItemsControl.ItemsControlFromItemContainer(item);
                // War unser item ein ItemContainer und ist es der Letzte?
                if (itemsControl != null && itemsControl.ItemContainerGenerator.IndexFromContainer(item) == itemsControl.Items.Count - 1) return true;
            }
            // Als Standard sonst false zurückgeben
            return false;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}