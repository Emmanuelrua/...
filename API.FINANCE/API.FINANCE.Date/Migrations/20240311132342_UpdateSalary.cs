using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace API.FINANCE.Data.Migrations
{
    /// <inheritdoc />
    public partial class UpdateSalary : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "AddedDate",
                table: "Salaries",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "IsExpired",
                table: "Salaries",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "SalaryIn",
                table: "Salaries",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AddedDate",
                table: "Salaries");

            migrationBuilder.DropColumn(
                name: "IsExpired",
                table: "Salaries");

            migrationBuilder.DropColumn(
                name: "SalaryIn",
                table: "Salaries");
        }
    }
}
