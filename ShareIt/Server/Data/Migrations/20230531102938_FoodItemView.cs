using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ShareIt.Server.Data.Migrations
{
    /// <inheritdoc />
    public partial class FoodItemView : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Views",
                table: "FoodItems",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Views",
                table: "FoodItems");
        }
    }
}
