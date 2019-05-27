using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace iSHARE.ServiceProvider.Api.Warehouse13
{
    public class Startup : Api.Startup
    {
        public Startup(IHostingEnvironment env, IConfiguration configuration, ILoggerFactory loggerFactory)
            : base(env, configuration, loggerFactory)
        {
            SwaggerName = "Warehouse 13";
            ApplicationInsightsName = "w13.api";
            IdentityServerSeedNamespace = "iSHARE.ServiceProvider.Api.Warehouse13.Seed.IdentityServer";
            IdentityServerSeedAssembly = typeof(Startup).Assembly;
            ServiceProviderSeedNamespace = "iSHARE.ServiceProvider.Api.Warehouse13.Seed.ServiceProvider";
            ServiceProviderSeedAssembly = typeof(Startup).Assembly;
        }
    }
}
