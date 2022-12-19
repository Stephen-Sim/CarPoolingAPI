using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CarPoolingAPI.Migrations
{
    public partial class AddStatusToTrip : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Status",
                table: "Trips",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Status",
                table: "Trips");
        }
    }
}
