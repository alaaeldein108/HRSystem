using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HrSystem.DAL.Migrations
{
    public partial class deleteSalarReportFromAttendance : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SalaryReports_Attendances_AttendanceId",
                table: "SalaryReports");

            migrationBuilder.DropIndex(
                name: "IX_SalaryReports_AttendanceId",
                table: "SalaryReports");

            migrationBuilder.DropColumn(
                name: "AttendanceId",
                table: "SalaryReports");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "AttendanceId",
                table: "SalaryReports",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_SalaryReports_AttendanceId",
                table: "SalaryReports",
                column: "AttendanceId");

            migrationBuilder.AddForeignKey(
                name: "FK_SalaryReports_Attendances_AttendanceId",
                table: "SalaryReports",
                column: "AttendanceId",
                principalTable: "Attendances",
                principalColumn: "Id");
        }
    }
}
