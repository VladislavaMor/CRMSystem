using System.ComponentModel.DataAnnotations;

namespace CRM_Helper
{
    public class ContactsTransfer
    {
        [MaxLength(256)]
        public string? Adress { get; set; }

        [DataType(DataType.PhoneNumber)]
        public string? PhoneNumber { get; set; }

        [DataType(DataType.EmailAddress)]
        public string? Email { get; set; }

        [DataType(DataType.Url)]
        [MaxLength(4096)]
        public string? LinkToMapContructor { get; set; }
    }
}
