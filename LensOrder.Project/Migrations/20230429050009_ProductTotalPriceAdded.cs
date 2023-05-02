using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LensOrder.Project.Migrations
{
    public partial class ProductTotalPriceAdded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ProductTotalPrice",
                table: "Orders",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ProductTotalPrice",
                table: "Orders");
        }
    }
}
