using Microsoft.EntityFrameworkCore.Migrations;

namespace WordsmithWarehouse.Migrations
{
    public partial class FixLeases : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "Lease",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.CreateIndex(
                name: "IX_Lease_UserId",
                table: "Lease",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Lease_AspNetUsers_UserId",
                table: "Lease",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Lease_AspNetUsers_UserId",
                table: "Lease");

            migrationBuilder.DropIndex(
                name: "IX_Lease_UserId",
                table: "Lease");

            migrationBuilder.AlterColumn<int>(
                name: "UserId",
                table: "Lease",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);
        }
    }
}
