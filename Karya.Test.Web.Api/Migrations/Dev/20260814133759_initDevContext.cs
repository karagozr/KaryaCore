using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Karya.Test.Web.Api.Migrations.Dev
{
    /// <inheritdoc />
    public partial class initDevContext : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "InventoryMainCategories",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    TenantId = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InventoryMainCategories", x => new { x.TenantId, x.Id });
                });

            migrationBuilder.CreateTable(
                name: "InventoryCategories",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    TenantId = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MainCategoryId = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InventoryCategories", x => new { x.TenantId, x.Id });
                    table.ForeignKey(
                        name: "FK_InventoryCategories_InventoryMainCategories_TenantId_MainCategoryId",
                        columns: x => new { x.TenantId, x.MainCategoryId },
                        principalTable: "InventoryMainCategories",
                        principalColumns: new[] { "TenantId", "Id" },
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Inventories",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    TenantId = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Brand = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CategoryId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    MainCategoryId = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Inventories", x => new { x.TenantId, x.Id });
                    table.ForeignKey(
                        name: "FK_Inventories_InventoryCategories_TenantId_CategoryId",
                        columns: x => new { x.TenantId, x.CategoryId },
                        principalTable: "InventoryCategories",
                        principalColumns: new[] { "TenantId", "Id" },
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Inventories_InventoryMainCategories_TenantId_MainCategoryId",
                        columns: x => new { x.TenantId, x.MainCategoryId },
                        principalTable: "InventoryMainCategories",
                        principalColumns: new[] { "TenantId", "Id" },
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "InventoryDetails",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenantId = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    InventoryId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Note = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CategoryId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    MainCategoryId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    InventoryTenantId = table.Column<string>(type: "nvarchar(10)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InventoryDetails", x => new { x.TenantId, x.Id });
                    table.ForeignKey(
                        name: "FK_InventoryDetails_Inventories_InventoryTenantId_InventoryId",
                        columns: x => new { x.InventoryTenantId, x.InventoryId },
                        principalTable: "Inventories",
                        principalColumns: new[] { "TenantId", "Id" });
                    table.ForeignKey(
                        name: "FK_InventoryDetails_Inventories_TenantId_InventoryId",
                        columns: x => new { x.TenantId, x.InventoryId },
                        principalTable: "Inventories",
                        principalColumns: new[] { "TenantId", "Id" },
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_InventoryDetails_InventoryCategories_TenantId_CategoryId",
                        columns: x => new { x.TenantId, x.CategoryId },
                        principalTable: "InventoryCategories",
                        principalColumns: new[] { "TenantId", "Id" },
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_InventoryDetails_InventoryMainCategories_TenantId_MainCategoryId",
                        columns: x => new { x.TenantId, x.MainCategoryId },
                        principalTable: "InventoryMainCategories",
                        principalColumns: new[] { "TenantId", "Id" },
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Inventories_TenantId_CategoryId",
                table: "Inventories",
                columns: new[] { "TenantId", "CategoryId" });

            migrationBuilder.CreateIndex(
                name: "IX_Inventories_TenantId_MainCategoryId",
                table: "Inventories",
                columns: new[] { "TenantId", "MainCategoryId" });

            migrationBuilder.CreateIndex(
                name: "IX_InventoryCategories_TenantId_MainCategoryId",
                table: "InventoryCategories",
                columns: new[] { "TenantId", "MainCategoryId" });

            migrationBuilder.CreateIndex(
                name: "IX_InventoryDetails_InventoryTenantId_InventoryId",
                table: "InventoryDetails",
                columns: new[] { "InventoryTenantId", "InventoryId" });

            migrationBuilder.CreateIndex(
                name: "IX_InventoryDetails_TenantId_CategoryId",
                table: "InventoryDetails",
                columns: new[] { "TenantId", "CategoryId" });

            migrationBuilder.CreateIndex(
                name: "IX_InventoryDetails_TenantId_InventoryId",
                table: "InventoryDetails",
                columns: new[] { "TenantId", "InventoryId" });

            migrationBuilder.CreateIndex(
                name: "IX_InventoryDetails_TenantId_MainCategoryId",
                table: "InventoryDetails",
                columns: new[] { "TenantId", "MainCategoryId" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "InventoryDetails");

            migrationBuilder.DropTable(
                name: "Inventories");

            migrationBuilder.DropTable(
                name: "InventoryCategories");

            migrationBuilder.DropTable(
                name: "InventoryMainCategories");
        }
    }
}
