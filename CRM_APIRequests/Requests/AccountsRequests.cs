using Newtonsoft.Json.Linq;
using CRM_Helper;

namespace CRM_APIRequests
{
    public class AccountsRequests : RequestController
    {
        public AccountsRequests(Func<string> getBaseUrl) : base(getBaseUrl, "Authentification") { }

        public async Task<string> LoginAsync(Account account) =>
            await Request.AddAsync(account, Url);
        

    }
}
