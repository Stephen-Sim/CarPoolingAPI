using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CarPoolingAPI.Migrations
{
    public partial class AddTripNumberToTrip : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "TripNumber",
                table: "Trips",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TripNumber",
                table: "Trips");
        }
    }
}
