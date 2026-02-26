using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class updatecardocumentconfig : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_CarDocuments_CarId_DocumentType",
                table: "CarDocuments");

            migrationBuilder.CreateIndex(
                name: "IX_CarDocuments_CarId_DocumentType",
                table: "CarDocuments",
                columns: new[] { "CarId", "DocumentType" },
                unique: true,
                filter: "\"IsDeleted\" = false");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_CarDocuments_CarId_DocumentType",
                table: "CarDocuments");

            migrationBuilder.CreateIndex(
                name: "IX_CarDocuments_CarId_DocumentType",
                table: "CarDocuments",
                columns: new[] { "CarId", "DocumentType" },
                unique: true);
        }
    }
}
