using LunchService.Hosting;

namespace LunchService
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Host host = new Host();
            host.StartOnMainThread();
        }
    }
}
