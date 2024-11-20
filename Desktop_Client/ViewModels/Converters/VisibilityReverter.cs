using System;
using System.Windows;
using System.Windows.Data;

namespace Desktop_Client.ViewModels.Converters
{
    internal class VisibilityReverter : IValueConverter
    {

        public object Convert(object item, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {

            var visibility = (Visibility)item;

            if (visibility == Visibility.Collapsed || visibility == Visibility.Hidden)
            {
                return Visibility.Visible;
            }
            return Visibility.Collapsed;

        }
        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return new NotSupportedException();
        }

    }
}
