using Newtonsoft.Json;
using CRM_APIRequests.Exceptions;
using System;
using System.Net;
using System.Text;

namespace CRM_APIRequests
{
    public static class Request
    {

        private static readonly CookieContainer _cookieContainer = new();

        public async static Task<T> GetAsync<T>(string url)
        {
            T obg;

            using (var httpClientHandler = new HttpClientHandler
            {
                CookieContainer = _cookieContainer
            })
            {
                using (HttpClient client = new(httpClientHandler))
                {
                    string ServicesJson = await client.GetStringAsync(url);
                    obg = JsonConvert.DeserializeObject<T>(ServicesJson);
                }
            }
            return obg;
        }

        public async static Task<string> AddAsync<T>(T obj, string url)
        {
            string result;
            using (var httpClientHandler = new HttpClientHandler
            {
                CookieContainer = _cookieContainer
            })
            {
                using (HttpClient client = new(httpClientHandler))
                {
                    StringContent content = CreateContent(obj);
                    HttpResponseMessage response = await client.PostAsync(url, content);

                    if (response.StatusCode == HttpStatusCode.Unauthorized)
                        throw new SkillProfiUnauthorizedException(url);
                    
                    response.EnsureSuccessStatusCode();
                    Console.WriteLine(response.StatusCode.ToString());
                    result = response.Content.ReadAsStringAsync().Result;
                }
            }
            return result;
        }

        public async static Task<string> EditAsync<T>(T obj, string url, string? id = null)
        {
            string uri = (id == null ? url : $"{url}/{id}");

            string result;
            using (var httpClientHandler = new HttpClientHandler
            {
                CookieContainer = _cookieContainer
            })
            {
                using (HttpClient client = new(httpClientHandler))
                {
                    StringContent content = CreateContent(obj);
                    HttpResponseMessage response = await client.PutAsync(uri, content);

					if (response.StatusCode == HttpStatusCode.Unauthorized)
						throw new SkillProfiUnauthorizedException(url);

					response.EnsureSuccessStatusCode();
                    result = response.Content.ReadAsStringAsync().Result;
                }
            }
            return result;
        }

        public async static Task<string> DeleteAsync(string id, string url)
        {
            var uri = $"{url}/{id}";

            string result;
            using (var httpClientHandler = new HttpClientHandler
            {
                CookieContainer = _cookieContainer
            })
            {
                using (HttpClient client = new(httpClientHandler))
                {
                    HttpResponseMessage response = await client.DeleteAsync(uri);

					if (response.StatusCode == HttpStatusCode.Unauthorized)
						throw new SkillProfiUnauthorizedException(url);

					response.EnsureSuccessStatusCode();
                    result = response.Content.ReadAsStringAsync().Result;
                }
            }
            return result;
        }

        private static StringContent CreateContent<T>(T obj) 
        {
            string json = JsonConvert.SerializeObject(obj);
            return new(json, Encoding.UTF8, "application/json");
        }
    }
}
