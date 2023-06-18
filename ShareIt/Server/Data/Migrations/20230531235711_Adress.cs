using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ShareIt.Server.Data.Migrations
{
    /// <inheritdoc />
    public partial class Adress : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "City",
                table: "FoodItems",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "PostalCode",
                table: "FoodItems",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Street",
                table: "FoodItems",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "City",
                table: "FoodItems");

            migrationBuilder.DropColumn(
                name: "PostalCode",
                table: "FoodItems");

            migrationBuilder.DropColumn(
                name: "Street",
                table: "FoodItems");
        }
    }
}
