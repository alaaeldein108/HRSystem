using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HrSystem.DAL.Migrations
{
    public partial class ApplicationRoleModifcation : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Creator",
                table: "Roles");

            migrationBuilder.AlterColumn<string>(
                name: "ModifiedBy",
                table: "Roles",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CreatorId",
                table: "Roles",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Roles_CreatorId",
                table: "Roles",
                column: "CreatorId");

            migrationBuilder.CreateIndex(
                name: "IX_Roles_ModifiedBy",
                table: "Roles",
                column: "ModifiedBy");

            migrationBuilder.AddForeignKey(
                name: "FK_Roles_Users_CreatorId",
                table: "Roles",
                column: "CreatorId",
                principalTable: "Users",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Roles_Users_ModifiedBy",
                table: "Roles",
                column: "ModifiedBy",
                principalTable: "Users",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Roles_Users_CreatorId",
                table: "Roles");

            migrationBuilder.DropForeignKey(
                name: "FK_Roles_Users_ModifiedBy",
                table: "Roles");

            migrationBuilder.DropIndex(
                name: "IX_Roles_CreatorId",
                table: "Roles");

            migrationBuilder.DropIndex(
                name: "IX_Roles_ModifiedBy",
                table: "Roles");

            migrationBuilder.DropColumn(
                name: "CreatorId",
                table: "Roles");

            migrationBuilder.AlterColumn<string>(
                name: "ModifiedBy",
                table: "Roles",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Creator",
                table: "Roles",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
