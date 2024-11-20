using CRM_Helper;
using System.IO;
using System.Text;

namespace CRM_APIRequests
{
    public class ProjectsRequests : RequestController
    {
        public ProjectsRequests(Func<string> getBaseUrl) : base(getBaseUrl, "Projects") {}

        public async Task<List<Project>> GetListAsync() => await Request.GetAsync<List<Project>>(Url);

        public async Task<Project> GetByIdAsync(string id) => await Request.GetAsync<Project>(Url + $"/{id}");

        public async Task<string> AddAsync(ProjectTransfer project, Stream? stream)
        {
			var obj = BuildObjectWithImage(project, stream);
			return await Request.AddAsync(obj, Url);
		}
             
        public async Task<string> EditAsync(string id, ProjectTransfer project, Stream? stream)
        {
			var obj = BuildObjectWithImage(project, stream);
			return await Request.EditAsync(obj, Url, id);
		}

        public async Task<string> DeleteByIdAsync(string id) =>
            await Request.DeleteAsync(id, Url);

    }
}
