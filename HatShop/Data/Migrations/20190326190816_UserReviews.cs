using Microsoft.EntityFrameworkCore.Migrations;

namespace HatShop.Data.Migrations
{
    public partial class UserReviews : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "HatUserId",
                table: "Review",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Review_HatUserId",
                table: "Review",
                column: "HatUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Review_AspNetUsers_HatUserId",
                table: "Review",
                column: "HatUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Review_AspNetUsers_HatUserId",
                table: "Review");

            migrationBuilder.DropIndex(
                name: "IX_Review_HatUserId",
                table: "Review");

            migrationBuilder.DropColumn(
                name: "HatUserId",
                table: "Review");
        }
    }
}
