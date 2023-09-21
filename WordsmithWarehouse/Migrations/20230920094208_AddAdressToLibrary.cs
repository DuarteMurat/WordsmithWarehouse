using Microsoft.EntityFrameworkCore.Migrations;

namespace WordsmithWarehouse.Migrations
{
    public partial class AddAdressToLibrary : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Adress",
                table: "Libraries",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Adress",
                table: "Libraries");
        }
    }
}
