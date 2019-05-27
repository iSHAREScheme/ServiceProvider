using System;
using iSHARE.EntityFramework.Migrations.Seed;
using iSHARE.ServiceProvider.Core.Models;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace iSHARE.ServiceProvider.Data.Migrations.Seeders
{
    internal class DatabaseSeeder : ReplaceCollectionDatabaseSeeder<ServiceProviderDbContext>, IDatabaseSeeder<ServiceProviderDbContext>
    {
        public DatabaseSeeder(
            ISeedDataProvider<ServiceProviderDbContext> seedDataProvider,
            ServiceProviderDbContext context,
            string environment,
            ILogger<DatabaseSeeder> logger)
            : base(seedDataProvider, context, environment, logger)
        {
        }

        public string EnvironmentName => Environment;

        public void Seed()
        {
            Logger.LogInformation("Seed ServiceProvider {environment}", Environment);

            AddOrUpdateCollection<Container>("containers.json");
            AddOrUpdateCollection<Order>("orders.json");

            Context.SaveChanges();
        }

        public static DatabaseSeeder CreateSeeder(IServiceProvider srv, string environment) => new DatabaseSeeder(srv.GetService<ISeedDataProvider<ServiceProviderDbContext>>(),
            srv.GetService<ServiceProviderDbContext>(),
            environment,
            srv.GetService<ILogger<DatabaseSeeder>>());
    }
}