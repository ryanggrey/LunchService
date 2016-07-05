using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Builder;
using System.Threading;
using Microsoft.AspNetCore.Hosting;

namespace LunchService.Hosting
{
    public class Host
    {
        private IWebHost delegateHost = InitialiseHost();

        public void StartOnMainThread()
        {
            delegateHost.Run();
        }

        public void StartOnBackgroundThread()
        {
            AutoResetEvent eventWaitHandle = new AutoResetEvent(false);
            Thread thread = new Thread(() => StartOnBackgroundThread(eventWaitHandle));
            thread.Start();
            eventWaitHandle.WaitOne();
        }

        public void Stop()
        {
            delegateHost.Dispose();
        }

        private static IWebHost InitialiseHost()
        {
            var host = new WebHostBuilder()
            .UseKestrel()
            .UseContentRoot(Directory.GetCurrentDirectory())
            .UseIISIntegration()
            .UseStartup<Startup>()
            .Build();

            return host;
        }

        private void StartOnBackgroundThread(EventWaitHandle waitHandle)
        {
            delegateHost.Start();
            waitHandle.Set();
        }
    }
}
