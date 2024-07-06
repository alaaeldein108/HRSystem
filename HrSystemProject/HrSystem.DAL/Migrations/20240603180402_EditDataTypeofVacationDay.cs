using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HrSystem.DAL.Migrations
{
    public partial class EditDataTypeofVacationDay : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "WeeklyDay",
                table: "GeneralSettingsNew");

            migrationBuilder.AlterColumn<int>(
                name: "VacationDay",
                table: "Vacations",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "VacationDay",
                table: "Vacations",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<string>(
                name: "WeeklyDay",
                table: "GeneralSettingsNew",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
