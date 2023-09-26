using Microsoft.EntityFrameworkCore.Migrations;

namespace WordsmithWarehouse.Migrations
{
    public partial class AddBooksToBooksReservations : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Books_BookReservations_BookReservationId",
                table: "Books");

            migrationBuilder.DropTable(
                name: "BookTags");

            migrationBuilder.DropIndex(
                name: "IX_Books_BookReservationId",
                table: "Books");

            migrationBuilder.DropColumn(
                name: "BookReservationId",
                table: "Books");

            migrationBuilder.AddColumn<string>(
                name: "BookIds",
                table: "BookReservations",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BookIds",
                table: "BookReservations");

            migrationBuilder.AddColumn<int>(
                name: "BookReservationId",
                table: "Books",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "BookTags",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BookId = table.Column<int>(type: "int", nullable: false),
                    TagId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BookTags", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BookTags_Books_BookId",
                        column: x => x.BookId,
                        principalTable: "Books",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BookTags_Tags_TagId",
                        column: x => x.TagId,
                        principalTable: "Tags",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Books_BookReservationId",
                table: "Books",
                column: "BookReservationId");

            migrationBuilder.CreateIndex(
                name: "IX_BookTags_BookId",
                table: "BookTags",
                column: "BookId");

            migrationBuilder.CreateIndex(
                name: "IX_BookTags_TagId",
                table: "BookTags",
                column: "TagId");

            migrationBuilder.AddForeignKey(
                name: "FK_Books_BookReservations_BookReservationId",
                table: "Books",
                column: "BookReservationId",
                principalTable: "BookReservations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
