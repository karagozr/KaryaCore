using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Karya.Test.Web.Api.Migrations
{
    /// <inheritdoc />
    public partial class inital : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Inventories_InventoryCategories_CategoryTanentId_CategoryId",
                table: "Inventories");

            migrationBuilder.RenameColumn(
                name: "TanentId",
                table: "InventoryCategories",
                newName: "TenantId");

            migrationBuilder.RenameColumn(
                name: "CategoryTanentId",
                table: "Inventories",
                newName: "CategoryTenantId");

            migrationBuilder.RenameColumn(
                name: "TanentId",
                table: "Inventories",
                newName: "TenantId");

            migrationBuilder.RenameIndex(
                name: "IX_Inventories_CategoryTanentId_CategoryId",
                table: "Inventories",
                newName: "IX_Inventories_CategoryTenantId_CategoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_Inventories_InventoryCategories_CategoryTenantId_CategoryId",
                table: "Inventories",
                columns: new[] { "CategoryTenantId", "CategoryId" },
                principalTable: "InventoryCategories",
                principalColumns: new[] { "TenantId", "Id" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Inventories_InventoryCategories_CategoryTenantId_CategoryId",
                table: "Inventories");

            migrationBuilder.RenameColumn(
                name: "TenantId",
                table: "InventoryCategories",
                newName: "TanentId");

            migrationBuilder.RenameColumn(
                name: "CategoryTenantId",
                table: "Inventories",
                newName: "CategoryTanentId");

            migrationBuilder.RenameColumn(
                name: "TenantId",
                table: "Inventories",
                newName: "TanentId");

            migrationBuilder.RenameIndex(
                name: "IX_Inventories_CategoryTenantId_CategoryId",
                table: "Inventories",
                newName: "IX_Inventories_CategoryTanentId_CategoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_Inventories_InventoryCategories_CategoryTanentId_CategoryId",
                table: "Inventories",
                columns: new[] { "CategoryTanentId", "CategoryId" },
                principalTable: "InventoryCategories",
                principalColumns: new[] { "TanentId", "Id" });
        }
    }
}
