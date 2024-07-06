using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HrSystem.DAL.Migrations
{
    public partial class HrSystemUpdates : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SalaryReports_Attendances_AttendanceId",
                table: "SalaryReports");

            migrationBuilder.DropColumn(
                name: "EmployeeName",
                table: "SalaryReports");

            migrationBuilder.DropColumn(
                name: "HourRate",
                table: "SalaryReports");

            migrationBuilder.DropColumn(
                name: "PhoneNumber",
                table: "SalaryReports");

            migrationBuilder.DropColumn(
                name: "Salary",
                table: "SalaryReports");

            migrationBuilder.DropColumn(
                name: "AbsentDays",
                table: "Attendances");

            migrationBuilder.DropColumn(
                name: "AttendanceDays",
                table: "Attendances");

            migrationBuilder.AlterColumn<int>(
                name: "AttendanceId",
                table: "SalaryReports",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "GeneralSettingId",
                table: "SalaryReports",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<decimal>(
                name: "HourRate",
                table: "Employees",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_SalaryReports_GeneralSettingId",
                table: "SalaryReports",
                column: "GeneralSettingId");

            migrationBuilder.AddForeignKey(
                name: "FK_SalaryReports_Attendances_AttendanceId",
                table: "SalaryReports",
                column: "AttendanceId",
                principalTable: "Attendances",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SalaryReports_GeneralSettings_GeneralSettingId",
                table: "SalaryReports",
                column: "GeneralSettingId",
                principalTable: "GeneralSettings",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SalaryReports_Attendances_AttendanceId",
                table: "SalaryReports");

            migrationBuilder.DropForeignKey(
                name: "FK_SalaryReports_GeneralSettings_GeneralSettingId",
                table: "SalaryReports");

            migrationBuilder.DropIndex(
                name: "IX_SalaryReports_GeneralSettingId",
                table: "SalaryReports");

            migrationBuilder.DropColumn(
                name: "GeneralSettingId",
                table: "SalaryReports");

            migrationBuilder.DropColumn(
                name: "HourRate",
                table: "Employees");

            migrationBuilder.AlterColumn<int>(
                name: "AttendanceId",
                table: "SalaryReports",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<string>(
                name: "EmployeeName",
                table: "SalaryReports",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<decimal>(
                name: "HourRate",
                table: "SalaryReports",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PhoneNumber",
                table: "SalaryReports",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<decimal>(
                name: "Salary",
                table: "SalaryReports",
                type: "money",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<int>(
                name: "AbsentDays",
                table: "Attendances",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "AttendanceDays",
                table: "Attendances",
                type: "int",
                nullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_SalaryReports_Attendances_AttendanceId",
                table: "SalaryReports",
                column: "AttendanceId",
                principalTable: "Attendances",
                principalColumn: "Id");
        }
    }
}
