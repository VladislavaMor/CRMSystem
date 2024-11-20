using System.ComponentModel.DataAnnotations;

namespace CRM_Helper
{
    public class Appeal
    {
        [Required]
        public Guid Id { get; set; }

        [Required]
        [MaxLength(30)]
        public string? Name { get; set; }

        [Required]
        [MaxLength(254)]
        public string? EMail { get; set; }

        [Required]
        [MaxLength(1024)]
        public string? Description { get; set; }

        [Required]
        public AppealStatus Status { get; set; }

        [Required]
        public DateTime Created { get; set; }

        public static List<AppealStatus> Statuses
        {
            get
            {
                var curStatus = new List<AppealStatus>();
                foreach (AppealStatus status in Enum.GetValues(typeof(AppealStatus)))
                    curStatus.Add(status);
                return curStatus;
            }
        }
    }
}
