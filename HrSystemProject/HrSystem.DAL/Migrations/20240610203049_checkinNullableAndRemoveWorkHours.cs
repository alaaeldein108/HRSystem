using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HrSystem.DAL.Migrations
{
    public partial class checkinNullableAndRemoveWorkHours : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "WorkHours",
                table: "Attendances");

            migrationBuilder.AlterColumn<TimeSpan>(
                name: "CheckInTime",
                table: "Attendances",
                type: "time",
                nullable: true,
                oldClrType: typeof(TimeSpan),
                oldType: "time");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<TimeSpan>(
                name: "CheckInTime",
                table: "Attendances",
                type: "time",
                nullable: false,
                defaultValue: new TimeSpan(0, 0, 0, 0, 0),
                oldClrType: typeof(TimeSpan),
                oldType: "time",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "WorkHours",
                table: "Attendances",
                type: "int",
                nullable: true,
                computedColumnSql: "DATEDIFF(HOUR, CheckInTime, ISNULL(CheckOutTime, CURRENT_TIMESTAMP))");
        }
    }
}
