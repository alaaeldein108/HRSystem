using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HrSystem.DAL.Migrations
{
    public partial class AddWorksHoursinEmplyeeTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "WorkHours",
                table: "Employees",
                type: "int",
                nullable: true,
                computedColumnSql: "DATEDIFF(HOUR, CheckInTime, ISNULL(CheckOutTime, CURRENT_TIMESTAMP))");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "WorkHours",
                table: "Employees");
        }
    }
}
