using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using System.Net.Http;
using LunchService;
using System.Threading;
using System.Net;
using System.Net.Http.Headers;
using Microsoft.AspNetCore.Hosting;
using LunchService.Hosting;
using LunchService.Models;
using Newtonsoft.Json;

namespace APIEndpointTest
{
    // see example explanation on xUnit.net website:
    // https://xunit.github.io/docs/getting-started-dotnet-core.html
    public class UserCRUDTest: IDisposable
    {
        private const string endpoint = "http://localhost:5000/api/users";
        private Host host = new Host();

        public UserCRUDTest()
        {
            host.StartOnBackgroundThread();
        }

        public void Dispose()
        {
            host.Stop();
        }

        [Fact]
        public async Task test_invalid_GET_endpoint_returns_400()
        {
            string invalidEndpoint = endpoint + "s";

            using (HttpClient client = new HttpClient())
            {
                HttpResponseMessage response = await client.GetAsync(invalidEndpoint);
                HttpStatusCode expectedStatusCode = HttpStatusCode.NotFound;
                HttpStatusCode observedStatusCode = response.StatusCode;

                Assert.Equal(expectedStatusCode, observedStatusCode);
            }
        }

        [Fact]
        public async Task test_valid_GET_all_users_returns_200()
        {
            using (HttpClient client = new HttpClient())
            {
                HttpResponseMessage response = await client.GetAsync(endpoint);
                HttpStatusCode expectedStatusCode = HttpStatusCode.OK;
                HttpStatusCode observedStatusCode = response.StatusCode;

                Assert.Equal(expectedStatusCode, observedStatusCode);
            }
        }

        [Fact]
        public async Task test_when_0_users_GET_all_users_returns_0_users()
        {
            using (HttpClient client = new HttpClient())
            {
                HttpResponseMessage response = await client.GetAsync(endpoint);
                string observedResponseContent = await response.Content.ReadAsStringAsync();
                string expectedResponseContent = "[]";

                Assert.Equal(expectedResponseContent, observedResponseContent);
            }
        }

        [Fact]
        public async Task test_valid_POST_user_returns_201()
        {
            User user = new User();
            user.Name = "Ryan";

            string jsonUser = JsonConvert.SerializeObject(user);
            HttpContent content = new StringContent(jsonUser);
            content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            using (HttpClient client = new HttpClient())
            {
                HttpResponseMessage response = await client.PostAsync(endpoint, content);
                HttpStatusCode expectedStatusCode = HttpStatusCode.Created;
                HttpStatusCode observedStatusCode = response.StatusCode;

                Assert.Equal(expectedStatusCode, observedStatusCode);
            }
        }
    }
}
