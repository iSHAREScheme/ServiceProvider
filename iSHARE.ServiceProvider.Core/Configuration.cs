using iSHARE.ServiceProvider.Core.Api;
using Microsoft.Extensions.DependencyInjection;

namespace iSHARE.ServiceProvider.Core
{
    public static class Configuration
    {
        public static void AddCore(this IServiceCollection services)
        {
            services.AddTransient<IContainersService, ContainersService>();
            services.AddTransient<IOrdersService, OrdersService>();
        }
    }
}
