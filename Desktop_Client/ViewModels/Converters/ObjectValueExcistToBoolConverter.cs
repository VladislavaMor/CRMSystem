using System;
using System.Globalization;
using System.Windows.Data;

namespace Desktop_Client.ViewModels.Converters
{
    internal class ObjectValueExcistToBoolConverter : IValueConverter
    {
        public object Convert(object values, Type targetType, object parameter, CultureInfo culture)
        {

            if (values != null)
            {
                return true;
            }
            return false;
        }

        public object ConvertBack(object value, Type targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
