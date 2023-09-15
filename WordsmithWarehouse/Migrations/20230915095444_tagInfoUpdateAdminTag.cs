using Microsoft.EntityFrameworkCore.Migrations;

namespace WordsmithWarehouse.Migrations
{
    public partial class tagInfoUpdateAdminTag : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "isAdmin",
                table: "Tags",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "CoverType",
                table: "Books",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Pages",
                table: "Books",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Publisher",
                table: "Books",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Synopsis",
                table: "Books",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "isAdmin",
                table: "Tags");

            migrationBuilder.DropColumn(
                name: "CoverType",
                table: "Books");

            migrationBuilder.DropColumn(
                name: "Pages",
                table: "Books");

            migrationBuilder.DropColumn(
                name: "Publisher",
                table: "Books");

            migrationBuilder.DropColumn(
                name: "Synopsis",
                table: "Books");
        }
    }
}
