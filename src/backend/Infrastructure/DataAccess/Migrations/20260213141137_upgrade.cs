using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class upgrade : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_RentalCompanies_OwnerId_Name",
                table: "RentalCompanies");

            migrationBuilder.DropIndex(
                name: "IX_Locations_Country_City",
                table: "Locations");

            migrationBuilder.DropIndex(
                name: "IX_CarPrices_CarId",
                table: "CarPrices");

            migrationBuilder.AddColumn<int>(
                name: "LocationId",
                table: "RentalCompanies",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ApprovalStatus",
                table: "Cars",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "CarId1",
                table: "CarPrices",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsMain",
                table: "CarImage",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "VerifiedAt",
                table: "CarDocuments",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)));

            migrationBuilder.AddColumn<int>(
                name: "VerifiedByAdminId",
                table: "CarDocuments",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_RentalCompanies_LocationId",
                table: "RentalCompanies",
                column: "LocationId");

            migrationBuilder.CreateIndex(
                name: "IX_RentalCompanies_OwnerId",
                table: "RentalCompanies",
                column: "OwnerId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Locations_Country_City",
                table: "Locations",
                columns: new[] { "Country", "City" },
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

            migrationBuilder.CreateIndex(
                name: "IX_CarDocuments_VerifiedByAdminId",
                table: "CarDocuments",
                column: "VerifiedByAdminId");

            migrationBuilder.AddForeignKey(
                name: "FK_CarDocuments_Users_VerifiedByAdminId",
                table: "CarDocuments",
                column: "VerifiedByAdminId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_CarPrices_Cars_CarId1",
                table: "CarPrices",
                column: "CarId1",
                principalTable: "Cars",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_RentalCompanies_Locations_LocationId",
                table: "RentalCompanies",
                column: "LocationId",
                principalTable: "Locations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CarDocuments_Users_VerifiedByAdminId",
                table: "CarDocuments");

            migrationBuilder.DropForeignKey(
                name: "FK_CarPrices_Cars_CarId1",
                table: "CarPrices");

            migrationBuilder.DropForeignKey(
                name: "FK_RentalCompanies_Locations_LocationId",
                table: "RentalCompanies");

            migrationBuilder.DropIndex(
                name: "IX_RentalCompanies_LocationId",
                table: "RentalCompanies");

            migrationBuilder.DropIndex(
                name: "IX_RentalCompanies_OwnerId",
                table: "RentalCompanies");

            migrationBuilder.DropIndex(
                name: "IX_Locations_Country_City",
                table: "Locations");

            migrationBuilder.DropIndex(
                name: "IX_CarPrices_CarId",
                table: "CarPrices");

            migrationBuilder.DropIndex(
                name: "IX_CarPrices_CarId1",
                table: "CarPrices");

            migrationBuilder.DropIndex(
                name: "IX_CarDocuments_VerifiedByAdminId",
                table: "CarDocuments");

            migrationBuilder.DropColumn(
                name: "LocationId",
                table: "RentalCompanies");

            migrationBuilder.DropColumn(
                name: "ApprovalStatus",
                table: "Cars");

            migrationBuilder.DropColumn(
                name: "CarId1",
                table: "CarPrices");

            migrationBuilder.DropColumn(
                name: "IsMain",
                table: "CarImage");

            migrationBuilder.DropColumn(
                name: "VerifiedAt",
                table: "CarDocuments");

            migrationBuilder.DropColumn(
                name: "VerifiedByAdminId",
                table: "CarDocuments");

            migrationBuilder.CreateIndex(
                name: "IX_RentalCompanies_OwnerId_Name",
                table: "RentalCompanies",
                columns: new[] { "OwnerId", "Name" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Locations_Country_City",
                table: "Locations",
                columns: new[] { "Country", "City" });

            migrationBuilder.CreateIndex(
                name: "IX_CarPrices_CarId",
                table: "CarPrices",
                column: "CarId");
        }
    }
}
