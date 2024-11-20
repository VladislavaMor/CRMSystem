using CRM_Helper;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Web_Client.Models
{
    public class ModelCustom<T>
    {

        public string? Id { get; set; }
        public T Target { get; set; }

        public string? ImageLink { get; set; }

        [NotMapped]
        [DataType(DataType.Upload)]
        [DisplayName("Upload File")]
        public IFormFile? ImageFile { get; set; }
    }
}
