using CRM_Helper;

namespace CRM_APIRequests
{
    public class ContactsRequests : RequestController
    {
        public ContactsRequests(Func<string> getBaseUrl) : base(getBaseUrl, "Contacts") {}

        public async Task<Contact> GetAsync() => 
           await Request.GetAsync<Contact>(Url);      

        public async Task<string> EditAsync(ContactsTransfer contact) => 
            await Request.EditAsync(contact, Url);

    }
}
