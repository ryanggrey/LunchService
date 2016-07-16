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

    public class UserCreateTests: IDisposable
    {
        private const string endpoint = "Users";
        private LunchClient client = new LunchClient(endpoint);
        private Host host = new Host();
        private readonly List<IDisposable> disposables = new List<IDisposable>();

        public UserCreateTests()
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
        public async Task test_valid_POST_user_returns_201()
        {
            User user = new User("Ryan");
            HttpResponseMessage response = await client.Post(user);
            HttpStatusCode expectedStatusCode = HttpStatusCode.Created;
            HttpStatusCode observedStatusCode = response.StatusCode;

            Assert.Equal(expectedStatusCode, observedStatusCode);

            Dispose(response);
        }

        [Fact]
        public async Task test_valid_POST_user_returns_correct_location_of_new_resource()
        {
            User user = new User("Ryan");
            HttpResponseMessage response = await client.Post(user);
            string jsonContent = await response.Content.ReadAsStringAsync();
            User createdUser = JsonConvert.DeserializeObject<User>(jsonContent);

            string expectedLocation = client.BaseAddress.ToString() + endpoint + "/" + createdUser.ID;
            string observedLocation = response.Headers.Location.ToString();

            Dispose(response);
        }

        [Fact]
        public async Task test_valid_POST_user_returns_content_with_new_user()
        {
            User expectedUser = new User("Ryan");
            HttpResponseMessage response = await client.Post(expectedUser);
            string jsonContent = await response.Content.ReadAsStringAsync();

            User observedUser = JsonConvert.DeserializeObject<User>(jsonContent);

            Assert.Equal(expectedUser, observedUser);

            Dispose(response);
        }

        [Fact]
        public async Task test_POST_with_no_user_returns_400()
        {
            HttpResponseMessage response = await client.Post((object)null);
            HttpStatusCode expectedStatusCode = HttpStatusCode.BadRequest;
            HttpStatusCode observedStatusCode = response.StatusCode;

            Assert.Equal(expectedStatusCode, observedStatusCode);

            Dispose(response);
        }

        [Fact]
        public async Task test_POST_with_no_user_returns_error_content()
        {
            HttpResponseMessage response = await client.Post((object)null);
            string expectedResponseContent = "User required in request body.";
            string observedResponseContent = await response.Content.ReadAsStringAsync();

            Assert.Equal(expectedResponseContent, observedResponseContent);

            Dispose(response);
        }

        [Fact]
        public async Task test_POST_with_malformed_user_returns_400()
        {
            object malformedUser = new
            {
                Pie = "Ryan"
            };

            HttpResponseMessage response = await client.Post(malformedUser);
            HttpStatusCode expectedStatusCode = HttpStatusCode.BadRequest;
            HttpStatusCode observedStatusCode = response.StatusCode;

            Assert.Equal(expectedStatusCode, observedStatusCode);

            Dispose(response);
        }

        [Fact]
        public async Task test_POST_with_malformed_user_returns_error_content()
        {
            object malformedUser = new
            {
                Pudding = "Rice"
            };

            HttpResponseMessage response = await client.Post(malformedUser);
            string expectedResponseContent = @"{""Name"":[""The Name field is required.""]}";
            string observedResponseContent = await response.Content.ReadAsStringAsync();

            Assert.Equal(expectedResponseContent, observedResponseContent);

            Dispose(response);
        }

        [Fact]
        public async Task test_POST_with_malformed_json_content_returns_400()
        {
            string malformedJson = @"""{ ""Name"" ""Ryan""}"""; // missing key-value colon

            HttpResponseMessage response = await client.Post(malformedJson);
            HttpStatusCode expectedStatusCode = HttpStatusCode.BadRequest;
            HttpStatusCode observedStatusCode = response.StatusCode;

            Assert.Equal(expectedStatusCode, observedStatusCode);

            Dispose(response);
        }

        [Fact]
        public async Task test_POST_with_malformed_json_content_returns_error_content()
        {
            string malformedJson = @"""{ ""Name"" ""Ryan""}"""; // missing key-value colon

            HttpResponseMessage response = await client.Post(malformedJson);
            string expectedResponseContent = @"{"""":[""The input was not valid.""]}";
            string observedResponseContent = await response.Content.ReadAsStringAsync();

            Assert.Equal(expectedResponseContent, observedResponseContent);

            Dispose(response);
        }

        #endregion Tests
    }
}
