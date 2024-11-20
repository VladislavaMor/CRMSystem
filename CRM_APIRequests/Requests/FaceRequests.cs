using CRM_Helper;

namespace CRM_APIRequests
{
    public class FaceRequests : RequestController
    {
        public FaceRequests(Func<string> getBaseUrl) : base(getBaseUrl, "VisualComponents/Face") { }
        public async Task<Face> GetAsync() => await Request.GetAsync<Face>(Url);

        public async Task<string> EditAsync(Face face) =>
            await Request.EditAsync(face, Url);

    }
}
