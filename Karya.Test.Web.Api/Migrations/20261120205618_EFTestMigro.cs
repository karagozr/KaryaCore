using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Karya.Test.Web.Api.Migrations
{
    /// <inheritdoc />
    public partial class EFTestMigro : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AppRoleGroupRoles_AspNetRoles_RoleId",
                table: "AppRoleGroupRoles");

            migrationBuilder.DropForeignKey(
                name: "FK_AppUserRoleGroups_AspNetUsers_UserId",
                table: "AppUserRoleGroups");

            migrationBuilder.DropForeignKey(
                name: "FK_AppUserTenants_AspNetUsers_UserId",
                table: "AppUserTenants");

            migrationBuilder.DropForeignKey(
                name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
                table: "AspNetRoleClaims");

            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUserClaims_AspNetUsers_UserId",
                table: "AspNetUserClaims");

            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUserLogins_AspNetUsers_UserId",
                table: "AspNetUserLogins");

            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
                table: "AspNetUserRoles");

            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUserRoles_AspNetUsers_UserId",
                table: "AspNetUserRoles");

            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUserTokens_AspNetUsers_UserId",
                table: "AspNetUserTokens");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AppUserTenants",
                table: "AppUserTenants");

            migrationBuilder.DropIndex(
                name: "IX_AppUserRoleGroups_TenantId_UserId_RoleGroupId",
                table: "AppUserRoleGroups");

            migrationBuilder.DropIndex(
                name: "IX_AppUserRoleGroups_UserId",
                table: "AppUserRoleGroups");

            migrationBuilder.DropIndex(
                name: "IX_AppRoleGroups_TenantId_Name",
                table: "AppRoleGroups");

            migrationBuilder.DropIndex(
                name: "IX_AppRoleGroupRoles_RoleGroupId",
                table: "AppRoleGroupRoles");

            migrationBuilder.DropIndex(
                name: "IX_AppRoleGroupRoles_TenantId_RoleGroupId_RoleId",
                table: "AppRoleGroupRoles");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AspNetUserTokens",
                table: "AspNetUserTokens");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AspNetUsers",
                table: "AspNetUsers");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AspNetUserRoles",
                table: "AspNetUserRoles");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUserRoles_TenantId_UserId_RoleId",
                table: "AspNetUserRoles");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUserRoles_UserId",
                table: "AspNetUserRoles");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AspNetUserLogins",
                table: "AspNetUserLogins");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AspNetUserClaims",
                table: "AspNetUserClaims");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AspNetRoles",
                table: "AspNetRoles");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AspNetRoleClaims",
                table: "AspNetRoleClaims");

            migrationBuilder.DropColumn(
                name: "TenantId",
                table: "AppUserRoleGroups");

            migrationBuilder.DropColumn(
                name: "TenantId",
                table: "AppRoleGroups");

            migrationBuilder.DropColumn(
                name: "TenantId",
                table: "AppRoleGroupRoles");

            migrationBuilder.DropColumn(
                name: "ErpPersonId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "ErpUsername",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "Language",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "Theme",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "TenantId",
                table: "AspNetUserRoles");

            migrationBuilder.RenameTable(
                name: "AspNetUserTokens",
                newName: "AppUserTokens");

            migrationBuilder.RenameTable(
                name: "AspNetUsers",
                newName: "AppUsers");

            migrationBuilder.RenameTable(
                name: "AspNetUserRoles",
                newName: "AppUserRoles");

            migrationBuilder.RenameTable(
                name: "AspNetUserLogins",
                newName: "AppUserLogins");

            migrationBuilder.RenameTable(
                name: "AspNetUserClaims",
                newName: "AppUserClaims");

            migrationBuilder.RenameTable(
                name: "AspNetRoles",
                newName: "AppRoles");

            migrationBuilder.RenameTable(
                name: "AspNetRoleClaims",
                newName: "AppRoleClaims");

            migrationBuilder.RenameIndex(
                name: "IX_AspNetUserRoles_RoleId",
                table: "AppUserRoles",
                newName: "IX_AppUserRoles_RoleId");

            migrationBuilder.RenameIndex(
                name: "IX_AspNetUserLogins_UserId",
                table: "AppUserLogins",
                newName: "IX_AppUserLogins_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_AspNetUserClaims_UserId",
                table: "AppUserClaims",
                newName: "IX_AppUserClaims_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_AspNetRoleClaims_RoleId",
                table: "AppRoleClaims",
                newName: "IX_AppRoleClaims_RoleId");

            migrationBuilder.AddColumn<Guid>(
                name: "Id",
                table: "AppUserTenants",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"))
                .Annotation("Relational:ColumnOrder", 0);

            migrationBuilder.AddColumn<bool>(
                name: "IsSystemRoleGroup",
                table: "AppRoleGroups",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddPrimaryKey(
                name: "PK_AppUserTenants",
                table: "AppUserTenants",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_AppUserTokens",
                table: "AppUserTokens",
                columns: new[] { "UserId", "LoginProvider", "Name" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_AppUsers",
                table: "AppUsers",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_AppUserRoles",
                table: "AppUserRoles",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_AppUserLogins",
                table: "AppUserLogins",
                columns: new[] { "LoginProvider", "ProviderKey" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_AppUserClaims",
                table: "AppUserClaims",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_AppRoles",
                table: "AppRoles",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_AppRoleClaims",
                table: "AppRoleClaims",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_AppUserTenants_UserId_TenantId",
                table: "AppUserTenants",
                columns: new[] { "UserId", "TenantId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AppUserRoleGroups_UserId_RoleGroupId",
                table: "AppUserRoleGroups",
                columns: new[] { "UserId", "RoleGroupId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AppRoleGroups_Name",
                table: "AppRoleGroups",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AppRoleGroupRoles_RoleGroupId_RoleId",
                table: "AppRoleGroupRoles",
                columns: new[] { "RoleGroupId", "RoleId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AppUserRoles_UserId_RoleId",
                table: "AppUserRoles",
                columns: new[] { "UserId", "RoleId" },
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_AppRoleClaims_AppRoles_RoleId",
                table: "AppRoleClaims",
                column: "RoleId",
                principalTable: "AppRoles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AppRoleGroupRoles_AppRoles_RoleId",
                table: "AppRoleGroupRoles",
                column: "RoleId",
                principalTable: "AppRoles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AppUserClaims_AppUsers_UserId",
                table: "AppUserClaims",
                column: "UserId",
                principalTable: "AppUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AppUserLogins_AppUsers_UserId",
                table: "AppUserLogins",
                column: "UserId",
                principalTable: "AppUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AppUserRoleGroups_AppUsers_UserId",
                table: "AppUserRoleGroups",
                column: "UserId",
                principalTable: "AppUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AppUserRoles_AppRoles_RoleId",
                table: "AppUserRoles",
                column: "RoleId",
                principalTable: "AppRoles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AppUserRoles_AppUsers_UserId",
                table: "AppUserRoles",
                column: "UserId",
                principalTable: "AppUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AppUserTenants_AppTenants_TenantId",
                table: "AppUserTenants",
                column: "TenantId",
                principalTable: "AppTenants",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AppUserTenants_AppUsers_UserId",
                table: "AppUserTenants",
                column: "UserId",
                principalTable: "AppUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AppUserTokens_AppUsers_UserId",
                table: "AppUserTokens",
                column: "UserId",
                principalTable: "AppUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AppRoleClaims_AppRoles_RoleId",
                table: "AppRoleClaims");

            migrationBuilder.DropForeignKey(
                name: "FK_AppRoleGroupRoles_AppRoles_RoleId",
                table: "AppRoleGroupRoles");

            migrationBuilder.DropForeignKey(
                name: "FK_AppUserClaims_AppUsers_UserId",
                table: "AppUserClaims");

            migrationBuilder.DropForeignKey(
                name: "FK_AppUserLogins_AppUsers_UserId",
                table: "AppUserLogins");

            migrationBuilder.DropForeignKey(
                name: "FK_AppUserRoleGroups_AppUsers_UserId",
                table: "AppUserRoleGroups");

            migrationBuilder.DropForeignKey(
                name: "FK_AppUserRoles_AppRoles_RoleId",
                table: "AppUserRoles");

            migrationBuilder.DropForeignKey(
                name: "FK_AppUserRoles_AppUsers_UserId",
                table: "AppUserRoles");

            migrationBuilder.DropForeignKey(
                name: "FK_AppUserTenants_AppTenants_TenantId",
                table: "AppUserTenants");

            migrationBuilder.DropForeignKey(
                name: "FK_AppUserTenants_AppUsers_UserId",
                table: "AppUserTenants");

            migrationBuilder.DropForeignKey(
                name: "FK_AppUserTokens_AppUsers_UserId",
                table: "AppUserTokens");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AppUserTenants",
                table: "AppUserTenants");

            migrationBuilder.DropIndex(
                name: "IX_AppUserTenants_UserId_TenantId",
                table: "AppUserTenants");

            migrationBuilder.DropIndex(
                name: "IX_AppUserRoleGroups_UserId_RoleGroupId",
                table: "AppUserRoleGroups");

            migrationBuilder.DropIndex(
                name: "IX_AppRoleGroups_Name",
                table: "AppRoleGroups");

            migrationBuilder.DropIndex(
                name: "IX_AppRoleGroupRoles_RoleGroupId_RoleId",
                table: "AppRoleGroupRoles");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AppUserTokens",
                table: "AppUserTokens");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AppUsers",
                table: "AppUsers");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AppUserRoles",
                table: "AppUserRoles");

            migrationBuilder.DropIndex(
                name: "IX_AppUserRoles_UserId_RoleId",
                table: "AppUserRoles");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AppUserLogins",
                table: "AppUserLogins");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AppUserClaims",
                table: "AppUserClaims");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AppRoles",
                table: "AppRoles");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AppRoleClaims",
                table: "AppRoleClaims");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "AppUserTenants");

            migrationBuilder.DropColumn(
                name: "IsSystemRoleGroup",
                table: "AppRoleGroups");

            migrationBuilder.RenameTable(
                name: "AppUserTokens",
                newName: "AspNetUserTokens");

            migrationBuilder.RenameTable(
                name: "AppUsers",
                newName: "AspNetUsers");

            migrationBuilder.RenameTable(
                name: "AppUserRoles",
                newName: "AspNetUserRoles");

            migrationBuilder.RenameTable(
                name: "AppUserLogins",
                newName: "AspNetUserLogins");

            migrationBuilder.RenameTable(
                name: "AppUserClaims",
                newName: "AspNetUserClaims");

            migrationBuilder.RenameTable(
                name: "AppRoles",
                newName: "AspNetRoles");

            migrationBuilder.RenameTable(
                name: "AppRoleClaims",
                newName: "AspNetRoleClaims");

            migrationBuilder.RenameIndex(
                name: "IX_AppUserRoles_RoleId",
                table: "AspNetUserRoles",
                newName: "IX_AspNetUserRoles_RoleId");

            migrationBuilder.RenameIndex(
                name: "IX_AppUserLogins_UserId",
                table: "AspNetUserLogins",
                newName: "IX_AspNetUserLogins_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_AppUserClaims_UserId",
                table: "AspNetUserClaims",
                newName: "IX_AspNetUserClaims_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_AppRoleClaims_RoleId",
                table: "AspNetRoleClaims",
                newName: "IX_AspNetRoleClaims_RoleId");

            migrationBuilder.AddColumn<string>(
                name: "TenantId",
                table: "AppUserRoleGroups",
                type: "nvarchar(256)",
                maxLength: 256,
                nullable: false,
                defaultValue: "")
                .Annotation("Relational:ColumnOrder", 1);

            migrationBuilder.AddColumn<string>(
                name: "TenantId",
                table: "AppRoleGroups",
                type: "nvarchar(256)",
                maxLength: 256,
                nullable: false,
                defaultValue: "")
                .Annotation("Relational:ColumnOrder", 1);

            migrationBuilder.AddColumn<string>(
                name: "TenantId",
                table: "AppRoleGroupRoles",
                type: "nvarchar(256)",
                maxLength: 256,
                nullable: false,
                defaultValue: "")
                .Annotation("Relational:ColumnOrder", 1);

            migrationBuilder.AddColumn<string>(
                name: "ErpPersonId",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ErpUsername",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Language",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Theme",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TenantId",
                table: "AspNetUserRoles",
                type: "nvarchar(256)",
                maxLength: 256,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddPrimaryKey(
                name: "PK_AppUserTenants",
                table: "AppUserTenants",
                columns: new[] { "UserId", "TenantId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_AspNetUserTokens",
                table: "AspNetUserTokens",
                columns: new[] { "UserId", "LoginProvider", "Name" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_AspNetUsers",
                table: "AspNetUsers",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_AspNetUserRoles",
                table: "AspNetUserRoles",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_AspNetUserLogins",
                table: "AspNetUserLogins",
                columns: new[] { "LoginProvider", "ProviderKey" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_AspNetUserClaims",
                table: "AspNetUserClaims",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_AspNetRoles",
                table: "AspNetRoles",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_AspNetRoleClaims",
                table: "AspNetRoleClaims",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_AppUserRoleGroups_TenantId_UserId_RoleGroupId",
                table: "AppUserRoleGroups",
                columns: new[] { "TenantId", "UserId", "RoleGroupId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AppUserRoleGroups_UserId",
                table: "AppUserRoleGroups",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AppRoleGroups_TenantId_Name",
                table: "AppRoleGroups",
                columns: new[] { "TenantId", "Name" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AppRoleGroupRoles_RoleGroupId",
                table: "AppRoleGroupRoles",
                column: "RoleGroupId");

            migrationBuilder.CreateIndex(
                name: "IX_AppRoleGroupRoles_TenantId_RoleGroupId_RoleId",
                table: "AppRoleGroupRoles",
                columns: new[] { "TenantId", "RoleGroupId", "RoleId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserRoles_TenantId_UserId_RoleId",
                table: "AspNetUserRoles",
                columns: new[] { "TenantId", "UserId", "RoleId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserRoles_UserId",
                table: "AspNetUserRoles",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_AppRoleGroupRoles_AspNetRoles_RoleId",
                table: "AppRoleGroupRoles",
                column: "RoleId",
                principalTable: "AspNetRoles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AppUserRoleGroups_AspNetUsers_UserId",
                table: "AppUserRoleGroups",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AppUserTenants_AspNetUsers_UserId",
                table: "AppUserTenants",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
                table: "AspNetRoleClaims",
                column: "RoleId",
                principalTable: "AspNetRoles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUserClaims_AspNetUsers_UserId",
                table: "AspNetUserClaims",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUserLogins_AspNetUsers_UserId",
                table: "AspNetUserLogins",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
                table: "AspNetUserRoles",
                column: "RoleId",
                principalTable: "AspNetRoles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUserRoles_AspNetUsers_UserId",
                table: "AspNetUserRoles",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUserTokens_AspNetUsers_UserId",
                table: "AspNetUserTokens",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
