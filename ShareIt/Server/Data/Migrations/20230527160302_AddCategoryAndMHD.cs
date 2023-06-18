using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ShareIt.Server.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddCategoryAndMHD : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Category",
                table: "FoodItems",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "MHD",
                table: "FoodItems",
                type: "datetime2",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Category",
                table: "FoodItems");

            migrationBuilder.DropColumn(
                name: "MHD",
                table: "FoodItems");
        }
    }
}
