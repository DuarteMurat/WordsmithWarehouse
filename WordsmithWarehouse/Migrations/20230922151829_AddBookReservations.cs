using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace WordsmithWarehouse.Migrations
{
    public partial class AddBookReservations : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "BookReservationId",
                table: "Books",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "BookReservations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    LibraryId = table.Column<int>(type: "int", nullable: true),
                    ReservationDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    PickupDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ReturnDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsCancelled = table.Column<bool>(type: "bit", nullable: false),
                    IsCompleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BookReservations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BookReservations_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_BookReservations_Libraries_LibraryId",
                        column: x => x.LibraryId,
                        principalTable: "Libraries",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Books_BookReservationId",
                table: "Books",
                column: "BookReservationId");

            migrationBuilder.CreateIndex(
                name: "IX_BookReservations_LibraryId",
                table: "BookReservations",
                column: "LibraryId");

            migrationBuilder.CreateIndex(
                name: "IX_BookReservations_UserId",
                table: "BookReservations",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Books_BookReservations_BookReservationId",
                table: "Books",
                column: "BookReservationId",
                principalTable: "BookReservations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Books_BookReservations_BookReservationId",
                table: "Books");

            migrationBuilder.DropTable(
                name: "BookReservations");

            migrationBuilder.DropIndex(
                name: "IX_Books_BookReservationId",
                table: "Books");

            migrationBuilder.DropColumn(
                name: "BookReservationId",
                table: "Books");
        }
    }
}
