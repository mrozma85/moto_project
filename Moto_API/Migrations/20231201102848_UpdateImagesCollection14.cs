using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MotoAPI.Migrations
{
    /// <inheritdoc />
    public partial class UpdateImagesCollection14 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_VehicleImagesURL_AdId",
                table: "VehicleImagesURL",
                column: "AdId");

            migrationBuilder.AddForeignKey(
                name: "FK_VehicleImagesURL_Ads_AdId",
                table: "VehicleImagesURL",
                column: "AdId",
                principalTable: "Ads",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_VehicleImagesURL_Ads_AdId",
                table: "VehicleImagesURL");

            migrationBuilder.DropIndex(
                name: "IX_VehicleImagesURL_AdId",
                table: "VehicleImagesURL");
        }
    }
}
