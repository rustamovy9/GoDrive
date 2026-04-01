using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class updateconfigcarprice : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CarPrices_Cars_CarId1",
                table: "CarPrices");

            migrationBuilder.DropIndex(
                name: "IX_Users_UserName",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_CarPrices_CarId",
                table: "CarPrices");

            migrationBuilder.DropIndex(
                name: "IX_CarPrices_CarId1",
                table: "CarPrices");

            migrationBuilder.DropColumn(
                name: "UserName",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "CarId1",
                table: "CarPrices");



            migrationBuilder.AlterColumn<string>(
                name: "PhoneNumber",
                table: "Users",
                type: "character varying(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "character varying(50)",
                oldMaxLength: 50,
                oldNullable: true);

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

            migrationBuilder.AlterColumn<string>(
                name: "PhoneNumber",
                table: "Users",
                type: "character varying(50)",
                maxLength: 50,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(50)",
                oldMaxLength: 50);

            migrationBuilder.AddColumn<string>(
                name: "UserName",
                table: "Users",
                type: "character varying(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "CarId1",
                table: "CarPrices",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "AiConfidenceScore",
                table: "CarDocuments",
                type: "double precision",
                precision: 5,
                scale: 4,
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Users_UserName",
                table: "Users",
                column: "UserName",
                unique: true);

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
