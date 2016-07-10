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
        private const string endpoint = "http://localhost:5000/api/Users";
        private Host host = new Host();
        private HttpClient client = new HttpClient();
        private readonly List<IDisposable> disposables = new List<IDisposable>();

        public UserCRUDTest()
        {
            host.StartOnBackgroundThread();
            disposables.Add(client);
        }

        // automatically called at end of each test
        public void Dispose()
        {
            host.Stop();
            disposables.ForEach(disposable => disposable.Dispose());
        }

        // call to queue items for disposal at end of each test
        public void Dispose(params IDisposable[] toDispose)
        {
            disposables.AddRange(toDispose);
        }

        #region Tests

        [Fact]
        public async Task test_invalid_GET_endpoint_returns_400()
        {
            string invalidEndpoint = endpoint + "s";

            HttpResponseMessage response = await client.GetAsync(invalidEndpoint);
            HttpStatusCode expectedStatusCode = HttpStatusCode.NotFound;
            HttpStatusCode observedStatusCode = response.StatusCode;

            Assert.Equal(expectedStatusCode, observedStatusCode);
        }

        [Fact]
        public async Task test_valid_GET_all_users_returns_200()
        {
            HttpResponseMessage response = await client.GetAsync(endpoint);
            HttpStatusCode expectedStatusCode = HttpStatusCode.OK;
            HttpStatusCode observedStatusCode = response.StatusCode;

            Assert.Equal(expectedStatusCode, observedStatusCode);
        }

        [Fact]
        public async Task test_when_0_users_GET_all_users_returns_0_users()
        {
            HttpResponseMessage response = await GetAllUsers();
            string observedResponseContent = await response.Content.ReadAsStringAsync();
            string expectedResponseContent = "[]";

            Assert.Equal(expectedResponseContent, observedResponseContent);
        }

        [Fact]
        public async Task test_when_1_user_GET_all_users_returns_1_correct_user()
        {
            User user = new User("Ryan");
            await PostUser(user);

            HttpResponseMessage response = await GetAllUsers();
            string jsonContent = await response.Content.ReadAsStringAsync();

            List<User> observedUsers = JsonConvert.DeserializeObject<List<User>>(jsonContent);
            List<User> expectedUsers = new List<User>{user};

            Assert.Equal(expectedUsers, observedUsers);
        }

        [Fact]
        public async Task test_valid_POST_user_returns_201()
        {
            User user = new User("Ryan");
            HttpResponseMessage response = await PostUser(user);
            HttpStatusCode expectedStatusCode = HttpStatusCode.Created;
            HttpStatusCode observedStatusCode = response.StatusCode;

            Assert.Equal(expectedStatusCode, observedStatusCode);
        }

        [Fact]
        public async Task test_valid_POST_user_returns_correct_location_of_new_resource()
        {
            User user = new User("Ryan");
            HttpResponseMessage response = await PostUser(user);
            string expectedLocation = endpoint + "/0";
            string observedLocation = response.Headers.Location.ToString();

            Assert.Equal(expectedLocation, observedLocation);
        }

        #endregion Tests

        #region Util

        private HttpContent UserContent(User user)
        {
            string jsonUser = JsonConvert.SerializeObject(user);
            HttpContent content = new StringContent(jsonUser);
            content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            Dispose(content);

            return content;
        }

        private async Task<HttpResponseMessage> PostUser(User user)
        {
            HttpContent content = UserContent(user);
            HttpResponseMessage response = await client.PostAsync(endpoint, content);

            Dispose(response);

            return response;
        }

        private async Task<HttpResponseMessage> GetAllUsers()
        {
            HttpResponseMessage response = await client.GetAsync(endpoint);

            Dispose(response);

            return response;
        }

        #endregion Util
    }
}
