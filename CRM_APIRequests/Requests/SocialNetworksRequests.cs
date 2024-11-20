using CRM_Helper;

namespace CRM_APIRequests
{
    public class SocialNetworksRequests : RequestController
    {
        public SocialNetworksRequests(Func<string> getBaseUrl) : base(getBaseUrl, "Contacts/SocialMedias") { }

        public async Task<List<SocialMedia>> GetListAsync() =>
            await Request.GetAsync<List<SocialMedia>>(Url);

        public async Task<string> AddAsync(SocialMediaTransfer SocialMedia, Stream stream)
        {
			var obj = BuildObjectWithImage(SocialMedia, stream);
			return await Request.AddAsync(obj, Url);
        }

        public async Task<string> EditAsync(string id, SocialMediaTransfer SocialMedia, Stream? stream)
        {
			var obj = BuildObjectWithImage(SocialMedia, stream);
			return await Request.EditAsync(obj, Url, id); 
        }

        public async Task<string> DeleteByIdAsync(string id) =>
            await Request.DeleteAsync(id, Url);
    }
}
