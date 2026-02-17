using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class updatedproject2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ApprovalStatus",
                table: "Cars");

            migrationBuilder.AddColumn<int>(
                name: "CarId1",
                table: "CarDocuments",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_CarDocuments_CarId1",
                table: "CarDocuments",
                column: "CarId1");

            migrationBuilder.AddForeignKey(
                name: "FK_CarDocuments_Cars_CarId1",
                table: "CarDocuments",
                column: "CarId1",
                principalTable: "Cars",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CarDocuments_Cars_CarId1",
                table: "CarDocuments");

            migrationBuilder.DropIndex(
                name: "IX_CarDocuments_CarId1",
                table: "CarDocuments");

            migrationBuilder.DropColumn(
                name: "CarId1",
                table: "CarDocuments");

            migrationBuilder.AddColumn<int>(
                name: "ApprovalStatus",
                table: "Cars",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }
    }
}
