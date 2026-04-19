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
            migrationBuilder.DropIndex(
                name: "IX_CarPrices_CarId",
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

            migrationBuilder.CreateIndex(
                name: "IX_CarPrices_CarId",
                table: "CarPrices",
                column: "CarId",
                unique: true);

        }
    }
}
