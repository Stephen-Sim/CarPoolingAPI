using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CarPoolingAPI.Migrations
{
    public partial class AlterColumnPlatNoToUpnique : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "PlatNo",
                table: "Vehicles",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.CreateIndex(
                name: "IX_Vehicles_PlatNo",
                table: "Vehicles",
                column: "PlatNo",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Vehicles_PlatNo",
                table: "Vehicles");

            migrationBuilder.AlterColumn<string>(
                name: "PlatNo",
                table: "Vehicles",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");
        }
    }
}
