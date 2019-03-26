using Microsoft.EntityFrameworkCore.Migrations;

namespace HatShop.Data.Migrations
{
    public partial class HatSize : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "HatSize",
                table: "AspNetUsers",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "HatSize",
                table: "AspNetUsers");
        }
    }
}
