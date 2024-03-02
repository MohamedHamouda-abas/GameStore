using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace BULKY.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class customcodeforCompany : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Companies",
                columns: new[] { "Id", "City", "Name", "PhoneNumber", "PostalCode", "State", "StreetAddress" },
                values: new object[,]
                {
                    { 1, "egypt", "wewe", 12112121, "778", "cairo", "vsssss" },
                    { 2, "egypt", "wewe", 12112121, "778", "cairo", "vsv" },
                    { 3, "egypt", "wewe", 12112121, "778", "cairo", "fdf" },
                    { 4, "egypt", "wewe", 12112121, "778", "cairo", "fdf" },
                    { 5, "egypt", "wewe", 12112121, "778", "cairo", "sdc" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Companies",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Companies",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Companies",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Companies",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Companies",
                keyColumn: "Id",
                keyValue: 5);
        }
    }
}
