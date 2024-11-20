using CRM_Helper;

namespace CRM_APIRequests
{
    public class BlogsRequests : RequestController
    {
        public BlogsRequests(Func<string> getBaseUrl) : base(getBaseUrl, "Blogs") {}

        public async Task<List<Blog>> GetListAsync() => await Request.GetAsync<List<Blog>>(Url);

        public async Task<Blog> GetByIdAsync(string id) => await Request.GetAsync<Blog>($"{Url}/{id}");


        public async Task<string> AddAsync(BlogTransfer blogTransfer, Stream stream)
        {
			var obj = BuildObjectWithImage(blogTransfer, stream);
			return await Request.AddAsync(obj, Url);
        }

        public async Task<string> EditAsync(string id, BlogTransfer blogTransfer, Stream? stream)
        {
			var obj = BuildObjectWithImage(blogTransfer, stream);
			return await Request.EditAsync(obj, Url, id);
        }

        public async Task<string> DeleteByIdAsync(string id) =>
            await Request.DeleteAsync(id, Url);
    }
}
