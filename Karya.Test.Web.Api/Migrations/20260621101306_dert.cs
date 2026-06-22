using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Karya.Test.Web.Api.Migrations
{
    /// <inheritdoc />
    public partial class dert : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Inventories_InventoryCategories_TenantId_CategoryId",
                table: "Inventories");

            migrationBuilder.AddColumn<string>(
                name: "MainCategoryId",
                table: "InventoryCategories",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MainCategoryId",
                table: "Inventories",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "InventoryMainCategory",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    TenantId = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InventoryMainCategory", x => new { x.TenantId, x.Id });
                });

            migrationBuilder.CreateIndex(
                name: "IX_InventoryCategories_TenantId_MainCategoryId",
                table: "InventoryCategories",
                columns: new[] { "TenantId", "MainCategoryId" });

            migrationBuilder.CreateIndex(
                name: "IX_Inventories_TenantId_MainCategoryId",
                table: "Inventories",
                columns: new[] { "TenantId", "MainCategoryId" });

            migrationBuilder.AddForeignKey(
                name: "FK_Inventories_InventoryCategories_TenantId_CategoryId",
                table: "Inventories",
                columns: new[] { "TenantId", "CategoryId" },
                principalTable: "InventoryCategories",
                principalColumns: new[] { "TenantId", "Id" },
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Inventories_InventoryMainCategory_TenantId_MainCategoryId",
                table: "Inventories",
                columns: new[] { "TenantId", "MainCategoryId" },
                principalTable: "InventoryMainCategory",
                principalColumns: new[] { "TenantId", "Id" },
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_InventoryCategories_InventoryMainCategory_TenantId_MainCategoryId",
                table: "InventoryCategories",
                columns: new[] { "TenantId", "MainCategoryId" },
                principalTable: "InventoryMainCategory",
                principalColumns: new[] { "TenantId", "Id" },
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Inventories_InventoryCategories_TenantId_CategoryId",
                table: "Inventories");

            migrationBuilder.DropForeignKey(
                name: "FK_Inventories_InventoryMainCategory_TenantId_MainCategoryId",
                table: "Inventories");

            migrationBuilder.DropForeignKey(
                name: "FK_InventoryCategories_InventoryMainCategory_TenantId_MainCategoryId",
                table: "InventoryCategories");

            migrationBuilder.DropTable(
                name: "InventoryMainCategory");

            migrationBuilder.DropIndex(
                name: "IX_InventoryCategories_TenantId_MainCategoryId",
                table: "InventoryCategories");

            migrationBuilder.DropIndex(
                name: "IX_Inventories_TenantId_MainCategoryId",
                table: "Inventories");

            migrationBuilder.DropColumn(
                name: "MainCategoryId",
                table: "InventoryCategories");

            migrationBuilder.DropColumn(
                name: "MainCategoryId",
                table: "Inventories");

            migrationBuilder.AddForeignKey(
                name: "FK_Inventories_InventoryCategories_TenantId_CategoryId",
                table: "Inventories",
                columns: new[] { "TenantId", "CategoryId" },
                principalTable: "InventoryCategories",
                principalColumns: new[] { "TenantId", "Id" },
                onDelete: ReferentialAction.Cascade);
        }
    }
}
