using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace WordsmithWarehouse.Migrations
{
    public partial class UpdateLibraryEntity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TimeSpan",
                table: "Libraries");

            migrationBuilder.AddColumn<DateTime>(
                name: "ClosingHour",
                table: "Libraries",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "OpeningHour",
                table: "Libraries",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ClosingHour",
                table: "Libraries");

            migrationBuilder.DropColumn(
                name: "OpeningHour",
                table: "Libraries");

            migrationBuilder.AddColumn<TimeSpan>(
                name: "TimeSpan",
                table: "Libraries",
                type: "time",
                nullable: false,
                defaultValue: new TimeSpan(0, 0, 0, 0, 0));
        }
    }
}
