using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace API.FINANCE.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddColumsporcentaje : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Percentage",
                table: "Salaries",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Percentage",
                table: "Categories",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Percentage",
                table: "Salaries");

            migrationBuilder.DropColumn(
                name: "Percentage",
                table: "Categories");
        }
    }
}
