using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProductCatalog.DataAccess.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddIsConfirmedToShoppingCartTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsConfirmed",
                schema: "Catalog",
                table: "ShoppingCarts",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsConfirmed",
                schema: "Catalog",
                table: "ShoppingCarts");
        }
    }
}
