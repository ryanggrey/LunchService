using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using LunchService;
using System.Threading;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using Microsoft.AspNetCore.Hosting;
using LunchService.Hosting;
using LunchService.Models;
using Newtonsoft.Json;
using APIEndpoint.Rest;

namespace APIEndpoint.Test
{

    public class UserReadTests: IDisposable
    {
        private const string endpoint = "Users";
        private LunchClient client = new LunchClient(endpoint);
        private Host host = new Host();
        private readonly List<IDisposable> disposables = new List<IDisposable>();

        public UserReadTests()
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
        public async Task test_invalid_GET_endpoint_returns_404()
        {
            string invalidEndpoint = endpoint + "s";

            LunchClient restClient = new LunchClient(invalidEndpoint);
            HttpResponseMessage response = await restClient.GetAllUsers();
            HttpStatusCode expectedStatusCode = HttpStatusCode.NotFound;
            HttpStatusCode observedStatusCode = response.StatusCode;

            Assert.Equal(expectedStatusCode, observedStatusCode);

            Dispose(restClient, response);
        }

        [Fact]
        public async Task test_valid_GET_all_users_returns_200()
        {
            HttpResponseMessage response = await client.GetAllUsers();
            HttpStatusCode expectedStatusCode = HttpStatusCode.OK;
            HttpStatusCode observedStatusCode = response.StatusCode;

            Assert.Equal(expectedStatusCode, observedStatusCode);

            Dispose(response);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(10)]
        public async Task test_when_n_users_GET_all_users_returns_n_correct_users(int numberOfUsers)
        {
            List<User> expectedUsers = new List<User>();
            for (int userIndex = 0; userIndex < numberOfUsers; userIndex++)
            {
                User user = new User(userIndex.ToString());
                expectedUsers.Add(user);
                await client.Post(user);
            }

            HttpResponseMessage response = await client.GetAllUsers();
            string jsonContent = await response.Content.ReadAsStringAsync();

            List<User> observedUsers = JsonConvert.DeserializeObject<List<User>>(jsonContent);

            Assert.Equal(expectedUsers, observedUsers);

            Dispose(response);
        }

        [Fact]
        public async Task test_when_user_exists_GET_id_returns_200()
        {
            User user = new User("Ryan");
            HttpResponseMessage postResponse = await client.Post(user);
            string postResponseJson = await postResponse.Content.ReadAsStringAsync();
            User postResponseUser = JsonConvert.DeserializeObject<User>(postResponseJson);
            Console.WriteLine(postResponseUser.ID);

            HttpResponseMessage getResponse = await client.GetUser(postResponseUser.ID);
            HttpStatusCode expectedStatusCode = HttpStatusCode.OK;
            HttpStatusCode observedStatusCode = getResponse.StatusCode;

            Assert.Equal(expectedStatusCode, observedStatusCode);
        }

        [Fact]
        public async Task test_GET_returns_user_by_id()
        {
            User user = new User("Ryan");
            HttpResponseMessage postResponse = await client.Post(user);
            string postResponseJson = await postResponse.Content.ReadAsStringAsync();
            User postResponseUser = JsonConvert.DeserializeObject<User>(postResponseJson);

            HttpResponseMessage getResponse = await client.GetUser(postResponseUser.ID);
            string getResponseJson = await getResponse.Content.ReadAsStringAsync();
            User getResponseUser = JsonConvert.DeserializeObject<User>(getResponseJson);

            User expectedUser = postResponseUser;
            User observedUser = getResponseUser;

            Assert.Equal(expectedUser, observedUser);
        }

        #endregion Tests
    }
}
