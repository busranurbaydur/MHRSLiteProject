using Microsoft.EntityFrameworkCore.Migrations;

namespace MHRSLiteDataAccess.Migrations
{
    public partial class appointmentStatusAdded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<byte>(
                name: "AppointmentStatus",
                table: "Appointments",
                type: "tinyint",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AppointmentStatus",
                table: "Appointments");
        }
    }
}
