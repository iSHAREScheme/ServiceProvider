using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace iSHARE.ServiceProvider.Data.Migrations.Steps
{
    public partial class AddNewContainerColumnsAndOrderEntity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Eta",
                table: "Containers",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "Weight",
                table: "Containers",
                type: "decimal(18, 2)",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Orders",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    ContainerId = table.Column<string>(nullable: true),
                    EntitledPartyId = table.Column<string>(nullable: true),
                    Status = table.Column<int>(nullable: false),
                    Packages = table.Column<int>(nullable: false),
                    TruckbayPickup = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Orders", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Orders");

            migrationBuilder.DropColumn(
                name: "Eta",
                table: "Containers");

            migrationBuilder.DropColumn(
                name: "Weight",
                table: "Containers");
        }
    }
}
