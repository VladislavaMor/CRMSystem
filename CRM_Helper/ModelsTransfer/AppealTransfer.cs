using System.ComponentModel.DataAnnotations;

namespace CRM_Helper
{
    public class AppealTransfer
    {
        [Required(ErrorMessage = "Name is required field")]
        [MaxLength(24, ErrorMessage = "Name is too long")]
        public string? Name { get; set; }

        [Required(ErrorMessage = "Email is required field")]
        [EmailAddress(ErrorMessage = "Email is invalid")]
        public string? EMail { get; set; }

        [Required(ErrorMessage = "Description is required field")]
        [DataType(DataType.MultilineText)]
        [MinLength(6, ErrorMessage = "If you don't explain the problem in detail, we won't be able to help you")]
        [MaxLength(1024, ErrorMessage = "Maximum number of characters is 1024")]
        public string? Description { get; set; }

    }
}
