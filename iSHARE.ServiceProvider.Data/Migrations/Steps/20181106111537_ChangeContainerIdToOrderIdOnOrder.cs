using Microsoft.EntityFrameworkCore.Migrations;

namespace iSHARE.ServiceProvider.Data.Migrations.Steps
{
    public partial class ChangeContainerIdToOrderIdOnOrder : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ContainerId",
                table: "Orders",
                newName: "OrderId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "OrderId",
                table: "Orders",
                newName: "ContainerId");
        }
    }
}
