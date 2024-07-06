using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HrSystem.DAL.Migrations
{
    public partial class RemoveForeignKeyEmployeeFromGeneralSetting : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GeneralSettings_Employees_EmployeeId",
                table: "GeneralSettings");

            migrationBuilder.DropIndex(
                name: "IX_GeneralSettings_EmployeeId",
                table: "GeneralSettings");

            migrationBuilder.DropColumn(
                name: "EmployeeId",
                table: "GeneralSettings");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "EmployeeId",
                table: "GeneralSettings",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_GeneralSettings_EmployeeId",
                table: "GeneralSettings",
                column: "EmployeeId");

            migrationBuilder.AddForeignKey(
                name: "FK_GeneralSettings_Employees_EmployeeId",
                table: "GeneralSettings",
                column: "EmployeeId",
                principalTable: "Employees",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
