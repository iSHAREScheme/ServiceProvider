using iSHARE.ServiceProvider.Core.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace iSHARE.ServiceProvider.Data.Configurations
{
    public class ContainerConfiguration : IEntityTypeConfiguration<Container>
    {
        public void Configure(EntityTypeBuilder<Container> builder)
        {
            builder.Property(c => c.Weight).HasColumnType("decimal(9,2)");
            builder.Property(c => c.Eta).IsRequired().HasColumnType("varchar(100)");
            builder.Property(c => c.ContainerId).IsRequired().HasColumnType("varchar(100)");
            builder.Property(c => c.EntitledPartyId).IsRequired().HasColumnType("varchar(100)");

            builder.HasIndex(c => c.ContainerId).IsUnique();
            builder.HasIndex(c => c.EntitledPartyId);
        }
    }
}
