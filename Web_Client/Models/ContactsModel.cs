using CRM_Helper;

namespace Web_Client.Models
{
    public class ContactsModel
	{
		public string? Adress { get; set; }
		public string? PhoneNumber { get; set; }
		public string? Email { get; set; }

		public string? LinkToMapContructor { get; set; }

		public List<ModelCustom<SocialMedia>>? SocialNetworks { get; set; }
	}
}
