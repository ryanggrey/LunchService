using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using System.Net.Http;
using LunchService;
using System.Threading;
using System.Net;
using Microsoft.AspNetCore.Hosting;
using LunchService.Hosting;

namespace APIEndpointTest
{
    // see example explanation on xUnit.net website:
    // https://xunit.github.io/docs/getting-started-dotnet-core.html
    public class GETTest: IDisposable
    {
        private Host host = new Host();

        public GETTest()
        {
            host.StartOnBackgroundThread();
        }

        public void Dispose()
        {
            host.Stop();
        }

        [Fact]
        public async void test_can_GET_all_users()
        {
            const string endpoint = "http://localhost:5000/api/users";

            using (HttpClient client = new HttpClient())
            using (HttpResponseMessage response = await client.GetAsync(endpoint))
            {
                Assert.Equal(response.StatusCode, HttpStatusCode.OK);
            }
        }
    }
}
