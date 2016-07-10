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

        public LunchClient(string apiEndpoint)
        {
            endpoint = apiEndpoint;
        }

        public void Dispose()
        {
            client.Dispose();
        }

        private HttpContent UserContent(User user)
        {
            string jsonUser = JsonConvert.SerializeObject(user);
            HttpContent content = new StringContent(jsonUser);
            content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            return content;
        }

        public async Task<HttpResponseMessage> PostUser(User user)
        {
            HttpContent content = UserContent(user);
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
