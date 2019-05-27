﻿// <auto-generated />

using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace iSHARE.ServiceProvider.Data.Migrations.Steps
{
    [DbContext(typeof(ServiceProviderDbContext))]
    [Migration("20181105124527_AddNewContainerColumnsAndOrderEntity")]
    partial class AddNewContainerColumnsAndOrderEntity
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.1.4-rtm-31024")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("iSHARE.ServiceProvider.Data.Entities.Container", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ContainerId");

                    b.Property<string>("EntitledPartyId");

                    b.Property<string>("Eta");

                    b.Property<decimal?>("Weight")
                        .HasColumnType("decimal(18, 2)");

                    b.HasKey("Id");

                    b.ToTable("Containers");
                });

            modelBuilder.Entity("iSHARE.ServiceProvider.Data.Entities.Order", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ContainerId");

                    b.Property<string>("EntitledPartyId");

                    b.Property<int>("Packages");

                    b.Property<int>("Status");

                    b.Property<int?>("TruckbayPickup");

                    b.HasKey("Id");

                    b.ToTable("Orders");
                });
#pragma warning restore 612, 618
        }
    }
}
