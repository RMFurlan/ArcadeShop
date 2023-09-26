using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace meusite.Migrations
{
    /// <inheritdoc />
    public partial class AddGameKey : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "GameKey",
                table: "OrderDetails",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "GameKey",
                table: "OrderDetails");
        }
    }
}
