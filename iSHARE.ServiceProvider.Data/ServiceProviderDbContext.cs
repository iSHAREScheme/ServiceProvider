using iSHARE.EntityFramework.Migrations;
using iSHARE.ServiceProvider.Core.Models;
using Microsoft.EntityFrameworkCore;

namespace iSHARE.ServiceProvider.Data
{
    public class ServiceProviderDbContext : DbContext
    {
        public DbSet<Container> Containers { get; set; }
        public DbSet<Order> Orders { get; set; }

        public ServiceProviderDbContext(DbContextOptions<ServiceProviderDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.RegisterConfigurations<ServiceProviderDbContext>();
        }
    }
}
