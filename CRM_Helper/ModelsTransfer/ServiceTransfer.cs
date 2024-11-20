using System.ComponentModel.DataAnnotations;

namespace CRM_Helper
{
    public class ServiceTransfer
    {
        [Required]
        [MaxLength(48)]
        public string? Title { get; set; }

        [Required(ErrorMessage = "Description is required field")]
        [MaxLength(4096, ErrorMessage = "Text is too long")]
        [DataType(DataType.MultilineText)]
        public string? Description { get; set; }
    }
}
