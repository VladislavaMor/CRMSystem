using CRM_Helper;

namespace CRM_API
{
    internal static class ImageDirectory
    {
        private const string REPOSITORY = "Images";

        private static string PathToImage(string imageName) => Path.Combine(REPOSITORY, imageName);

        public static async Task<byte[]?> GetImageAsync(Guid guid)
        {
            string path = PathToImage(guid.ToString());
            if (!File.Exists(path)) return null;
            return await File.ReadAllBytesAsync(path);
        }

        private static string SetOriginalName(this IImage pic)
        {
            pic.ImageName = Guid.NewGuid().ToString();

            if (File.Exists(PathToImage(pic.ImageName)))
            {
                pic.SetOriginalName();
            }
            return pic.ImageName;
        }

        public static async Task SaveImageAsync(IImage pic, byte[]? Image)
        {
            if (Image == null)
            {
                if (string.IsNullOrEmpty(pic.ImageName) || !pic.PictureExsist())
                    throw new ImageNullException(nameof(pic));

                else if (pic.PictureExsist())
                    return;
            }

            if (!string.IsNullOrEmpty(pic.ImageName) && pic.PictureExsist())
                RemovePicture(pic);

            pic.SetOriginalName();
            string path = PathToImage(pic.ImageName);
            await File.WriteAllBytesAsync(path, Image);
        }


        public static void RemovePicture(this IImage image)
        {
            string path = PathToImage(image.ImageName);
            if (!File.Exists(path)) throw new ImageNotFoundException(image.ImageName);
            File.Delete(path);
        }

        private static bool PictureExsist(this IImage pic) => File.Exists(PathToImage(pic.ImageName));

    }
}
