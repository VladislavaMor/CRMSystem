using CRM_Helper;

namespace CRM_APIRequests
{
    public class ConsultationsRequests : RequestController
    {
        public ConsultationsRequests(Func<string> getBaseUrl) : base(getBaseUrl, "Consultations"){}

        public async Task<List<Appeal>> GetListAsync() => 
            await Request.GetAsync<List<Appeal>>(Url);

        public async Task<Appeal> GetByIdAsync(string id) =>
            await Request.GetAsync<Appeal>($"{Url}/{id}");

        public async Task<string> AddAsync(AppealTransfer Appeal) => 
            await Request.AddAsync(Appeal, Url);

        public async Task<string> EditAsync(string id, Appeal Appeal) =>
            await Request.EditAsync(Appeal,Url, id);

        public async Task<string> DeleteByIdAsync(string id) => 
            await Request.DeleteAsync(id, Url);

    }
}
