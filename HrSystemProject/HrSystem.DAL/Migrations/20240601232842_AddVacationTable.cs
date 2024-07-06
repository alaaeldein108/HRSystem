using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HrSystem.DAL.Migrations
{
    public partial class AddVacationTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Vacations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    VacationDay = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Vacations", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "GeneralSettingsNew",
                columns: table => new
                {
                    EmployeeId = table.Column<int>(type: "int", nullable: false),
                    VacationId = table.Column<int>(type: "int", nullable: false),
                    OvertimeHours = table.Column<int>(type: "int", nullable: true),
                    DiscountHours = table.Column<int>(type: "int", nullable: true),
                    WeeklyDay = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GeneralSettingsNew", x => new { x.EmployeeId, x.VacationId });
                    table.ForeignKey(
                        name: "FK_GeneralSettingsNew_Employees_EmployeeId",
                        column: x => x.EmployeeId,
                        principalTable: "Employees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_GeneralSettingsNew_Vacations_VacationId",
                        column: x => x.VacationId,
                        principalTable: "Vacations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_GeneralSettingsNew_VacationId",
                table: "GeneralSettingsNew",
                column: "VacationId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "GeneralSettingsNew");

            migrationBuilder.DropTable(
                name: "Vacations");
        }
    }
}
