using System.ComponentModel.DataAnnotations;

namespace CRM_Helper
{
    public class Blog : IImage
    {
        [Required]
        public Guid Id { get; set; }

        [Required]
        [MaxLength(48)]
        public string? Title { get; set; }

        [Required]
        [MaxLength(4096)]
        public string? Description { get; set; }

        [Required]
        public string? ImageName { get; set; }

        [Required]
        public DateTime Created { get; set; }
    }
}
