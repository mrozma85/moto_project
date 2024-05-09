using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MotoAPI.Migrations
{
    /// <inheritdoc />
    public partial class UpdateImagesCollection5 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_VehicleImagesURL_AdId",
                table: "VehicleImagesURL");

            migrationBuilder.CreateIndex(
                name: "IX_VehicleImagesURL_AdId",
                table: "VehicleImagesURL",
                column: "AdId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_VehicleImagesURL_AdId",
                table: "VehicleImagesURL");

            migrationBuilder.CreateIndex(
                name: "IX_VehicleImagesURL_AdId",
                table: "VehicleImagesURL",
                column: "AdId",
                unique: true);
        }
    }
}
