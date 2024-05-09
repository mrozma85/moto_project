using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MotoAPI.Migrations
{
    /// <inheritdoc />
    public partial class updateVehicle3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Vehicles_Companies_CompanyId1",
                table: "Vehicles");

            migrationBuilder.DropForeignKey(
                name: "FK_Vehicles_Models_ModelId1",
                table: "Vehicles");

            migrationBuilder.DropIndex(
                name: "IX_Vehicles_CompanyId1",
                table: "Vehicles");

            migrationBuilder.DropIndex(
                name: "IX_Vehicles_ModelId1",
                table: "Vehicles");

            migrationBuilder.DropColumn(
                name: "CompanyId1",
                table: "Vehicles");

            migrationBuilder.DropColumn(
                name: "ModelId1",
                table: "Vehicles");

            migrationBuilder.DropColumn(
                name: "VehicleId",
                table: "Models");

            migrationBuilder.DropColumn(
                name: "VehicleId",
                table: "Companies");

            migrationBuilder.CreateIndex(
                name: "IX_Vehicles_ModelId",
                table: "Vehicles",
                column: "ModelId");

            migrationBuilder.AddForeignKey(
                name: "FK_Vehicles_Models_ModelId",
                table: "Vehicles",
                column: "ModelId",
                principalTable: "Models",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Vehicles_Models_ModelId",
                table: "Vehicles");

            migrationBuilder.DropIndex(
                name: "IX_Vehicles_ModelId",
                table: "Vehicles");

            migrationBuilder.AddColumn<int>(
                name: "CompanyId1",
                table: "Vehicles",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ModelId1",
                table: "Vehicles",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "VehicleId",
                table: "Models",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "VehicleId",
                table: "Companies",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Vehicles_CompanyId1",
                table: "Vehicles",
                column: "CompanyId1");

            migrationBuilder.CreateIndex(
                name: "IX_Vehicles_ModelId1",
                table: "Vehicles",
                column: "ModelId1");

            migrationBuilder.AddForeignKey(
                name: "FK_Vehicles_Companies_CompanyId1",
                table: "Vehicles",
                column: "CompanyId1",
                principalTable: "Companies",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Vehicles_Models_ModelId1",
                table: "Vehicles",
                column: "ModelId1",
                principalTable: "Models",
                principalColumn: "Id");
        }
    }
}
