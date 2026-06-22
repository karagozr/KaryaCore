using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Karya.Test.Web.Api.Migrations
{
    /// <inheritdoc />
    public partial class mltiprikey : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Inventories_InventoryCategories_CategoryTenantId_CategoryId",
                table: "Inventories");

            migrationBuilder.DropIndex(
                name: "IX_Inventories_CategoryTenantId_CategoryId",
                table: "Inventories");

            migrationBuilder.DropColumn(
                name: "CategoryTenantId",
                table: "Inventories");

            migrationBuilder.CreateIndex(
                name: "IX_Inventories_TenantId_CategoryId",
                table: "Inventories",
                columns: new[] { "TenantId", "CategoryId" });

            migrationBuilder.AddForeignKey(
                name: "FK_Inventories_InventoryCategories_TenantId_CategoryId",
                table: "Inventories",
                columns: new[] { "TenantId", "CategoryId" },
                principalTable: "InventoryCategories",
                principalColumns: new[] { "TenantId", "Id" },
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Inventories_InventoryCategories_TenantId_CategoryId",
                table: "Inventories");

            migrationBuilder.DropIndex(
                name: "IX_Inventories_TenantId_CategoryId",
                table: "Inventories");

            migrationBuilder.AddColumn<string>(
                name: "CategoryTenantId",
                table: "Inventories",
                type: "nvarchar(10)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Inventories_CategoryTenantId_CategoryId",
                table: "Inventories",
                columns: new[] { "CategoryTenantId", "CategoryId" });

            migrationBuilder.AddForeignKey(
                name: "FK_Inventories_InventoryCategories_CategoryTenantId_CategoryId",
                table: "Inventories",
                columns: new[] { "CategoryTenantId", "CategoryId" },
                principalTable: "InventoryCategories",
                principalColumns: new[] { "TenantId", "Id" });
        }
    }
}
