using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HrSystem.DAL.Migrations
{
    public partial class RemoveGeneralSettingandAttendance : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
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

            migrationBuilder.AlterColumn<int>(
                name: "AttendanceId",
                table: "SalaryReports",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_SalaryReports_Attendances_AttendanceId",
                table: "SalaryReports",
                column: "AttendanceId",
                principalTable: "Attendances",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SalaryReports_Attendances_AttendanceId",
                table: "SalaryReports");

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
    }
}
