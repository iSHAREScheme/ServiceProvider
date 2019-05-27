using iSHARE.Api;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;

namespace iSHARE.ServiceProvider.Api.Warehouse13
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateWebHostBuilder(args)
                .Build()
                .Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseDefaultWebHostOptions<Startup>();
    }
}
