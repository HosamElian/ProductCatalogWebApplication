using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProductCatalog.DataAccess.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddShoppingCartTableToDB : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "Catalog");

            migrationBuilder.RenameTable(
                name: "Products",
                newName: "Products",
                newSchema: "Catalog");

            migrationBuilder.RenameTable(
                name: "Categories",
                newName: "Categories",
                newSchema: "Catalog");

            migrationBuilder.CreateTable(
                name: "ShoppingCarts",
                schema: "Catalog",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProductId = table.Column<int>(type: "int", nullable: false),
                    Count = table.Column<int>(type: "int", nullable: false),
                    IdentityUserId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ShoppingCarts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ShoppingCarts_Products_ProductId",
                        column: x => x.ProductId,
                        principalSchema: "Catalog",
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ShoppingCarts_Users_IdentityUserId",
                        column: x => x.IdentityUserId,
                        principalSchema: "Security",
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ShoppingCarts_IdentityUserId",
                schema: "Catalog",
                table: "ShoppingCarts",
                column: "IdentityUserId");

            migrationBuilder.CreateIndex(
                name: "IX_ShoppingCarts_ProductId",
                schema: "Catalog",
                table: "ShoppingCarts",
                column: "ProductId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ShoppingCarts",
                schema: "Catalog");

            migrationBuilder.RenameTable(
                name: "Products",
                schema: "Catalog",
                newName: "Products");

            migrationBuilder.RenameTable(
                name: "Categories",
                schema: "Catalog",
                newName: "Categories");
        }
    }
}
