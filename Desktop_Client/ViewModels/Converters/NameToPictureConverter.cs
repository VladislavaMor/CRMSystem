using CRM_APIRequests;
using System;
using System.IO;
using System.Windows.Data;

namespace Desktop_Client.ViewModels.Converters
{
    internal class NameToPictureConverter : IValueConverter
    {
        public object Convert(object item, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            SkillProfiWebClient _spClient = new();

            var name = (string)item;

            if (File.Exists(name))
            {
                return File.ReadAllBytes(name);
            }

            byte[] puctire = _spClient.Pictures.GetPicture(name);

            return puctire ?? File.ReadAllBytes("./Assets/ImageError.png");

        }
        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return new NotSupportedException();
        }
    }
}
