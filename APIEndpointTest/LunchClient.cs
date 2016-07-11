using System;
using LunchService.Models;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace APIEndpoint.Rest
{
    public class LunchClient: IDisposable
    {
        private string endpoint;
        private HttpClient client = new HttpClient();

        public Uri BaseAddress
        {
            get
            {
                return client.BaseAddress;
            }
        }

        public LunchClient(string apiEndpoint)
        {
            client.BaseAddress = new Uri("http://localhost:5000/api/");
            endpoint = apiEndpoint;
        }

        public void Dispose()
        {
            client.Dispose();
        }

        private HttpContent HttpContentFrom(object obj)
        {
            string json = JsonConvert.SerializeObject(obj);
            HttpContent content = new StringContent(json);
            content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            return content;
        }

        public async Task<HttpResponseMessage> Post(object obj)
        {
            HttpContent content = HttpContentFrom(obj);
            HttpResponseMessage response = await client.PostAsync(endpoint, content);

            return response;
        }

        public async Task<HttpResponseMessage> GetAllUsers()
        {
            HttpResponseMessage response = await client.GetAsync(endpoint);

            return response;
        }
    }
}
