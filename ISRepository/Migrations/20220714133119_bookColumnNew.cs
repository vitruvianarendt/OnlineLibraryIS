using Microsoft.EntityFrameworkCore.Migrations;

namespace ISRepository.Migrations
{
    public partial class bookColumnNew : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BookYear",
                table: "Books");

            migrationBuilder.AddColumn<string>(
                name: "BookAuthor",
                table: "Books",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BookAuthor",
                table: "Books");

            migrationBuilder.AddColumn<string>(
                name: "BookYear",
                table: "Books",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
