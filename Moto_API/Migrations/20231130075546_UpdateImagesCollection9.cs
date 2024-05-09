using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MotoAPI.Migrations
{
    /// <inheritdoc />
    public partial class UpdateImagesCollection9 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "AdId",
                table: "VehicleImagesDATA",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_VehicleImagesDATA_AdId",
                table: "VehicleImagesDATA",
                column: "AdId");

            migrationBuilder.AddForeignKey(
                name: "FK_VehicleImagesDATA_Ads_AdId",
                table: "VehicleImagesDATA",
                column: "AdId",
                principalTable: "Ads",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_VehicleImagesDATA_Ads_AdId",
                table: "VehicleImagesDATA");

            migrationBuilder.DropIndex(
                name: "IX_VehicleImagesDATA_AdId",
                table: "VehicleImagesDATA");

            migrationBuilder.DropColumn(
                name: "AdId",
                table: "VehicleImagesDATA");
        }
    }
}
