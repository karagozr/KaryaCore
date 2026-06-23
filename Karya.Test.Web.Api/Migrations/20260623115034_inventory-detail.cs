using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Karya.Test.Web.Api.Migrations
{
    /// <inheritdoc />
    public partial class inventorydetail : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "InventoryDetail",
                columns: table => new
                {
                    TenantId = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    InventoryId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Note = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CategoryId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    MainCategoryId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    InventoryTenantId = table.Column<string>(type: "nvarchar(10)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InventoryDetail", x => new { x.TenantId, x.Id });
                    table.ForeignKey(
                        name: "FK_InventoryDetail_Inventories_InventoryTenantId_InventoryId",
                        columns: x => new { x.InventoryTenantId, x.InventoryId },
                        principalTable: "Inventories",
                        principalColumns: new[] { "TenantId", "Id" });
                    table.ForeignKey(
                        name: "FK_InventoryDetail_Inventories_TenantId_InventoryId",
                        columns: x => new { x.TenantId, x.InventoryId },
                        principalTable: "Inventories",
                        principalColumns: new[] { "TenantId", "Id" },
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_InventoryDetail_InventoryCategories_TenantId_CategoryId",
                        columns: x => new { x.TenantId, x.CategoryId },
                        principalTable: "InventoryCategories",
                        principalColumns: new[] { "TenantId", "Id" },
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_InventoryDetail_InventoryMainCategory_TenantId_MainCategoryId",
                        columns: x => new { x.TenantId, x.MainCategoryId },
                        principalTable: "InventoryMainCategory",
                        principalColumns: new[] { "TenantId", "Id" },
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_InventoryDetail_InventoryTenantId_InventoryId",
                table: "InventoryDetail",
                columns: new[] { "InventoryTenantId", "InventoryId" });

            migrationBuilder.CreateIndex(
                name: "IX_InventoryDetail_TenantId_CategoryId",
                table: "InventoryDetail",
                columns: new[] { "TenantId", "CategoryId" });

            migrationBuilder.CreateIndex(
                name: "IX_InventoryDetail_TenantId_InventoryId",
                table: "InventoryDetail",
                columns: new[] { "TenantId", "InventoryId" });

            migrationBuilder.CreateIndex(
                name: "IX_InventoryDetail_TenantId_MainCategoryId",
                table: "InventoryDetail",
                columns: new[] { "TenantId", "MainCategoryId" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "InventoryDetail");
        }
    }
}
