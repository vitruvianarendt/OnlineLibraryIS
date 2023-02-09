using Microsoft.EntityFrameworkCore.Migrations;

namespace ISRepository.Migrations
{
    public partial class removedtype : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "bookType",
                table: "BooksInShoppingCarts");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "bookType",
                table: "BooksInShoppingCarts",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
