using System.ComponentModel.DataAnnotations;

namespace CRM_Helper
{
    public class Service
    {
        public Guid Id { get; set; }

        [Required]
        public string? Title { get; set; }

        [Required]
        public string? Description { get; set; }

        public DateTime Created { get; set; }
    }
}
