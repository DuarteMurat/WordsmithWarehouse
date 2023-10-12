using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace WordsmithWarehouse.Migrations
{
    public partial class AddLeasesAndReservations : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BookReservations_AspNetUsers_UserId",
                table: "BookReservations");

            migrationBuilder.DropForeignKey(
                name: "FK_BookReservations_Libraries_LibraryId",
                table: "BookReservations");

            migrationBuilder.DropIndex(
                name: "IX_BookReservations_UserId",
                table: "BookReservations");

            migrationBuilder.DropColumn(
                name: "BookIds",
                table: "BookReservations");

            migrationBuilder.DropColumn(
                name: "IsCancelled",
                table: "BookReservations");

            migrationBuilder.DropColumn(
                name: "IsCompleted",
                table: "BookReservations");

            migrationBuilder.DropColumn(
                name: "PickupDate",
                table: "BookReservations");

            migrationBuilder.DropColumn(
                name: "ReturnDate",
                table: "BookReservations");

            migrationBuilder.AlterColumn<int>(
                name: "ShelfId",
                table: "Books",
                type: "int",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "UserId",
                table: "BookReservations",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "LibraryId",
                table: "BookReservations",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "BookId",
                table: "BookReservations",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "Lease",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BookId = table.Column<int>(type: "int", nullable: false),
                    LibraryId = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    PickUpDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ReturnDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LeaseTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    OnGoing = table.Column<bool>(type: "bit", nullable: false),
                    IsCompleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Lease", x => x.Id);
                });

            migrationBuilder.AddForeignKey(
                name: "FK_BookReservations_Libraries_LibraryId",
                table: "BookReservations",
                column: "LibraryId",
                principalTable: "Libraries",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BookReservations_Libraries_LibraryId",
                table: "BookReservations");

            migrationBuilder.DropTable(
                name: "Lease");

            migrationBuilder.DropColumn(
                name: "BookId",
                table: "BookReservations");

            migrationBuilder.AlterColumn<string>(
                name: "ShelfId",
                table: "Books",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "BookReservations",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "LibraryId",
                table: "BookReservations",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<string>(
                name: "BookIds",
                table: "BookReservations",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsCancelled",
                table: "BookReservations",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsCompleted",
                table: "BookReservations",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "PickupDate",
                table: "BookReservations",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "ReturnDate",
                table: "BookReservations",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.CreateIndex(
                name: "IX_BookReservations_UserId",
                table: "BookReservations",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_BookReservations_AspNetUsers_UserId",
                table: "BookReservations",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_BookReservations_Libraries_LibraryId",
                table: "BookReservations",
                column: "LibraryId",
                principalTable: "Libraries",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
