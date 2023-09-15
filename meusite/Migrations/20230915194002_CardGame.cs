using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace meusite.Migrations
{
    /// <inheritdoc />
    public partial class CardGame : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CardGame",
                table: "Items",
                type: "varchar(1024)",
                maxLength: 1024,
                nullable: false,
                defaultValue: "")
                .Annotation("MySql:CharSet", "utf8mb4");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CardGame",
                table: "Items");
        }
    }
}
