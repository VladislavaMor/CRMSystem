﻿using System;
using System.Globalization;
using System.Windows.Data;

namespace Desktop_Client.ViewModels.Converters
{
    public class ObjectValueExcistToVisibilityCombinConverter : IValueConverter
    {
        public IValueConverter Converter1 { get; set; }
        public IValueConverter Converter2 { get; set; }

        public object Convert(
            object value, Type targetType, object parameter, CultureInfo culture)
        {
            object convertedValue =
                Converter1.Convert(value, targetType, parameter, culture);
            return Converter2.Convert(
                convertedValue, targetType, parameter, culture);
        }

        public object ConvertBack(
            object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
