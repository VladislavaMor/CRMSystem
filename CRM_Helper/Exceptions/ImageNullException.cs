
namespace CRM_Helper
{
    public class ImageNullException : ArgumentNullException
    {
        public ImageNullException(string nameOf) : base(nameOf, "Пустое изображение.") { }
    }
}
