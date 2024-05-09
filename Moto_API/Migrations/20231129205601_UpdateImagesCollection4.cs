using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MotoAPI.Migrations
{
    /// <inheritdoc />
    public partial class UpdateImagesCollection4 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_VehicleImagesDATA_Vehicles_VehicleId",
                table: "VehicleImagesDATA");

            migrationBuilder.DropForeignKey(
                name: "FK_VehicleImagesURL_Vehicles_VehicleId",
                table: "VehicleImagesURL");

            migrationBuilder.DropIndex(
                name: "IX_VehicleImagesURL_VehicleId",
                table: "VehicleImagesURL");

            migrationBuilder.DropIndex(
                name: "IX_VehicleImagesDATA_VehicleId",
                table: "VehicleImagesDATA");

            migrationBuilder.DropColumn(
                name: "VehicleId",
                table: "VehicleImagesDATA");

            migrationBuilder.RenameColumn(
                name: "VehicleId",
                table: "VehicleImagesURL",
                newName: "AdId");

            migrationBuilder.CreateIndex(
                name: "IX_VehicleImagesURL_AdId",
                table: "VehicleImagesURL",
                column: "AdId",
                unique: true);

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

            migrationBuilder.RenameColumn(
                name: "AdId",
                table: "VehicleImagesURL",
                newName: "VehicleId");

            migrationBuilder.AddColumn<int>(
                name: "VehicleId",
                table: "VehicleImagesDATA",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_VehicleImagesURL_VehicleId",
                table: "VehicleImagesURL",
                column: "VehicleId");

            migrationBuilder.CreateIndex(
                name: "IX_VehicleImagesDATA_VehicleId",
                table: "VehicleImagesDATA",
                column: "VehicleId");

            migrationBuilder.AddForeignKey(
                name: "FK_VehicleImagesDATA_Vehicles_VehicleId",
                table: "VehicleImagesDATA",
                column: "VehicleId",
                principalTable: "Vehicles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_VehicleImagesURL_Vehicles_VehicleId",
                table: "VehicleImagesURL",
                column: "VehicleId",
                principalTable: "Vehicles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
