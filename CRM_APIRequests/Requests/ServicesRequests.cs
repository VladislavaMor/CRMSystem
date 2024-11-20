using CRM_Helper;

namespace CRM_APIRequests
{
    public class ServicesRequests : RequestController
    {
        public ServicesRequests(Func<string> getBaseUrl) : base(getBaseUrl, "Services")
        {
        }
        public async Task<List<Service>> GetListAsync() =>
            await Request.GetAsync<List<Service>>(Url);

        public async Task<Service> GetByIdAsync(string id) =>
            await Request.GetAsync<Service>($"{Url}/{id}");

        public async Task<string> AddAsync(ServiceTransfer Service) =>
            await Request.AddAsync(Service, Url);

        public async Task<string> EditAsync(string id, ServiceTransfer Service) =>
            await Request.EditAsync(Service, Url, id);

        public async Task<string> DeleteByIdAsync(string id) =>
            await Request.DeleteAsync(id, Url);

    }
}
