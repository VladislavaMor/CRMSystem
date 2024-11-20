using System.Net;

namespace CRM_APIRequests
{
	public class PicturesRequests : RequestController
	{
        public PicturesRequests(Func<string> getBaseUrl) : base(getBaseUrl, "Picture") {}

        public string GetURL(string Id) => Url + $"?id={Id}";

		public byte[]? GetPicture(string Id)
		{
            HttpWebRequest lxRequest = (HttpWebRequest)WebRequest.Create(GetURL(Id));
            byte[]? lnByte;
            try
            {
                using HttpWebResponse lxResponse = (HttpWebResponse)lxRequest.GetResponse();
                using BinaryReader reader = new(lxResponse.GetResponseStream());
                lnByte = reader.ReadBytes(1 * 1024 * 1024 * 10);
            }
            catch
            {
                lnByte = null;
            }

            return lnByte;

        }
		
	}
}
