using System.ComponentModel.DataAnnotations;

namespace CRM_Helper
{
    public class BlogTransfer
    {
        public BlogTransfer() { }
        public BlogTransfer(string title, string description)
        {
            Title = title;
            Description = description;
        }

        [Required(ErrorMessage = "Title is required field")]
        [MaxLength(48, ErrorMessage = "Maximum number of characters is 48")]
        public string? Title { get; set; }

        [Required(ErrorMessage = "Description is required field")]
        [MaxLength(4096, ErrorMessage = "Text is too long")]
        [DataType(DataType.MultilineText)]
        public string? Description { get; set; }
    }
}
