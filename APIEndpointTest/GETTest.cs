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

namespace APIEndpointTest
{
    // see example explanation on xUnit.net website:
    // https://xunit.github.io/docs/getting-started-dotnet-core.html
    public class GETTest: IDisposable
    {
        private IWebHost host { get; set; }

        public GETTest()
        {
            host = LunchService.Program.GetHost();
            RunRestAppInBackground(host);
        }

        public void Dispose()
        {
            host.Dispose();
        }

        private void RunRestAppInBackground(IWebHost host)
        {
            AutoResetEvent eventWaitHandle = new AutoResetEvent(false);
            Thread backgroundApp = new Thread(() => StartBackgroundApp(host, eventWaitHandle));
            backgroundApp.Start();
            eventWaitHandle.WaitOne();
        }

        private void StartBackgroundApp(IWebHost host, EventWaitHandle waitHandle)
        {
            host.Start();
            waitHandle.Set();
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
