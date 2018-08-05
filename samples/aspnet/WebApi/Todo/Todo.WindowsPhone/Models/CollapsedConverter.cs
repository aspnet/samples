using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace Todo.WindowsPhone.Models
{
    public class CollapsedConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool collapsed = (bool)value;
            return collapsed ? Visibility.Collapsed : Visibility.Visible;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            Visibility visibility = (Visibility)value;
            return (visibility == Visibility.Collapsed);
        }
    }
}
