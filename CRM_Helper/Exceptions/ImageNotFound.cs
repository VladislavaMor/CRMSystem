
namespace CRM_Helper
{
    public class ImageNotFoundException : FileNotFoundException
    {
        public ImageNotFoundException(string imageName) : base("Изображение не найдено.", imageName) { }
    }
}
