namespace CRM_Helper
{
    public class SocialMedia : IImage
    {
        public Guid Id { get; set; }

        public string? Link { get; set; }

        public string? ImageName { get; set; }
    }
}
