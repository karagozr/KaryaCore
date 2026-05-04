using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Karya.Test.Web.Api.Migrations
{
    /// <inheritdoc />
    public partial class test1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "InventoryCategories",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    TanentId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InventoryCategories", x => new { x.TanentId, x.Id });
                });

            migrationBuilder.CreateTable(
                name: "Inventories",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    TanentId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Brand = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CategoryId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CategoryTanentId = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Inventories", x => new { x.TanentId, x.Id });
                    table.ForeignKey(
                        name: "FK_Inventories_InventoryCategories_CategoryTanentId_CategoryId",
                        columns: x => new { x.CategoryTanentId, x.CategoryId },
                        principalTable: "InventoryCategories",
                        principalColumns: new[] { "TanentId", "Id" });
                });

            migrationBuilder.CreateIndex(
                name: "IX_Inventories_CategoryTanentId_CategoryId",
                table: "Inventories",
                columns: new[] { "CategoryTanentId", "CategoryId" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Inventories");

            migrationBuilder.DropTable(
                name: "InventoryCategories");
        }
    }
}
