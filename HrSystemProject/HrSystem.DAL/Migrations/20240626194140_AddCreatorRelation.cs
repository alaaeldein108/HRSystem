using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HrSystem.DAL.Migrations
{
    public partial class AddCreatorRelation : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Creator",
                table: "Users");

            migrationBuilder.AddColumn<string>(
                name: "CreatorId",
                table: "Users",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Users_CreatorId",
                table: "Users",
                column: "CreatorId");

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Users_CreatorId",
                table: "Users",
                column: "CreatorId",
                principalTable: "Users",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Users_Users_CreatorId",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Users_CreatorId",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "CreatorId",
                table: "Users");

            migrationBuilder.AddColumn<string>(
                name: "Creator",
                table: "Users",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
