using System.IO;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Builder;
using System.Threading;

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
