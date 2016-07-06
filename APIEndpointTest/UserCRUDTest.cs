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
        public async void test_valid_GET_all_users_returns_200()
        {
            using (HttpClient client = new HttpClient())
            {
                HttpResponseMessage response = await client.GetAsync(endpoint);

                Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            }
        }

        [Fact]
        public async void test_valid_POST_user_returns_201()
        {
            User user = new User();
            user.Name = "Ryan";

            string jsonUser = JsonConvert.SerializeObject(user);
            HttpContent content = new StringContent(jsonUser);
            content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            using (HttpClient client = new HttpClient())
            {
                HttpResponseMessage response = await client.PostAsync(endpoint, content);

                Assert.Equal(HttpStatusCode.Created, response.StatusCode);
            }
        }
    }
}
