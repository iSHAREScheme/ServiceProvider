using System.Reflection;
using iSHARE.EntityFramework;
using iSHARE.ServiceProvider.Core;
using iSHARE.ServiceProvider.Data.Migrations.Seeders;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace iSHARE.ServiceProvider.Data
{
    public static class Configuration
    {
        public static void AddDb(this IServiceCollection services,
            IConfiguration configuration,
            IHostingEnvironment environment,
            string @namespace,
            Assembly assembly)
        {
            services.AddDbContext<ServiceProviderDbContext>(opts => opts.UseSqlServer(configuration.GetConnectionString("Main")));

            services.AddSeedServices<ServiceProviderDbContext>(
                environment,
                @namespace,
                assembly,
                DatabaseSeeder.CreateSeeder
            );

            services.AddTransient<IContainerRepository, ContainerRepository>();
            services.AddTransient<IOrderRepository, OrderRepository>();
        }

        public static void UseMigrations(this IApplicationBuilder app,
            IConfiguration configuration,
            IHostingEnvironment environment)
        {
            app.UseMigrations<ServiceProviderDbContext>(configuration);
            app.UseSeed<ServiceProviderDbContext>(environment);
        }
    }
}
