using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class @new : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CarPrices_Cars_CarId1",
                table: "CarPrices");

            migrationBuilder.DropIndex(
                name: "IX_CarPrices_CarId",
                table: "CarPrices");

            migrationBuilder.DropIndex(
                name: "IX_CarPrices_CarId1",
                table: "CarPrices");

            migrationBuilder.DropColumn(
                name: "CarId1",
                table: "CarPrices");

            migrationBuilder.CreateIndex(
                name: "IX_CarPrices_CarId",
                table: "CarPrices",
                column: "CarId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_CarPrices_CarId",
                table: "CarPrices");

            migrationBuilder.AddColumn<int>(
                name: "CarId1",
                table: "CarPrices",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_CarPrices_CarId",
                table: "CarPrices",
                column: "CarId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CarPrices_CarId1",
                table: "CarPrices",
                column: "CarId1");

            migrationBuilder.AddForeignKey(
                name: "FK_CarPrices_Cars_CarId1",
                table: "CarPrices",
                column: "CarId1",
                principalTable: "Cars",
                principalColumn: "Id");
        }
    }
}
