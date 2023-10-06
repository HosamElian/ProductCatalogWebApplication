using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProductCatalog.DataAccess.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddIsDeletedToShoppingCartTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
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
                name: "IsDeleted",
                schema: "Catalog",
                table: "ShoppingCarts");
        }
    }
}
