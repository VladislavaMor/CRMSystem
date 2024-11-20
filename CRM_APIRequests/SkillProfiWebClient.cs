using Newtonsoft.Json.Linq;
using System.Net;

namespace CRM_APIRequests
{
    public class SkillProfiWebClient
    {
        private const string SETTINGS_FILE_NAME = "appsettings.json";

		private const string DOMAIN_FIELD = "serverUrl";

		public SkillProfiWebClient()
        {
			BaseURL = ReadServerUrl;
			Accounts = new(GetBaseUrl);
			Blogs = new(GetBaseUrl);
			Contacts = new(GetBaseUrl);
			Consultations = new(GetBaseUrl);
			SocialNetworks = new(GetBaseUrl);
			Pictures = new(GetBaseUrl);
			Projects = new(GetBaseUrl);
			Services = new(GetBaseUrl);
			Face = new(GetBaseUrl);
		}


		public string BaseURL { get; set; }
        private string GetBaseUrl() => BaseURL;

		private static string ReadServerUrl
		{
			get
			{
				if (!File.Exists(SETTINGS_FILE_NAME))
				{
					throw new FileNotFoundException($"Для работы клиента должен быть файл {SETTINGS_FILE_NAME}, с параметром '{DOMAIN_FIELD}'");
				}
				string text = File.ReadAllText(SETTINGS_FILE_NAME);

				JObject configJs = JObject.Parse(text);

				string serverUrl = configJs[DOMAIN_FIELD].Value<string>() ?? throw new Exception("Не получилось прочитать Url сервера");

				return serverUrl;
			}
		}


		public readonly AccountsRequests Accounts;

        public readonly BlogsRequests Blogs;

        public readonly ConsultationsRequests Consultations;

        public readonly ContactsRequests Contacts;

        public readonly SocialNetworksRequests SocialNetworks;

        public readonly PicturesRequests Pictures;

        public readonly ProjectsRequests Projects;

        public readonly ServicesRequests Services;

        public readonly FaceRequests Face;

    }
}
