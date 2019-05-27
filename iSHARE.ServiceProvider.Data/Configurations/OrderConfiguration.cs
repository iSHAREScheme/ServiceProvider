using iSHARE.ServiceProvider.Core.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace iSHARE.ServiceProvider.Data.Configurations
{
    public class OrderConfiguration : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            builder.Property(c => c.EntitledPartyId).IsRequired().HasColumnType("varchar(100)");
            builder.Property(c => c.OrderId).IsRequired().HasColumnType("varchar(100)");

            builder.HasIndex(c => c.OrderId).IsUnique();
            builder.HasIndex(c => c.EntitledPartyId);
        }
    }
}
