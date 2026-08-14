using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Karya.Test.Web.Api.Migrations
{
    /// <inheritdoc />
    public partial class FixIdentityRelations : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetRoleClaims_AspNetRoles_AppRoleId",
                table: "AspNetRoleClaims");

            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUserClaims_AspNetUsers_AppUserId",
                table: "AspNetUserClaims");

            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUserLogins_AspNetUsers_UserId1",
                table: "AspNetUserLogins");

            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUserRoles_AspNetRoles_AppRoleId",
                table: "AspNetUserRoles");

            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUserRoles_AspNetUsers_AppUserId",
                table: "AspNetUserRoles");

            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUserTokens_AspNetUsers_AppUserId",
                table: "AspNetUserTokens");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUserTokens_AppUserId",
                table: "AspNetUserTokens");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUserRoles_AppRoleId",
                table: "AspNetUserRoles");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUserRoles_AppUserId",
                table: "AspNetUserRoles");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUserLogins_UserId1",
                table: "AspNetUserLogins");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUserClaims_AppUserId",
                table: "AspNetUserClaims");

            migrationBuilder.DropIndex(
                name: "IX_AspNetRoleClaims_AppRoleId",
                table: "AspNetRoleClaims");

            migrationBuilder.DropColumn(
                name: "AppUserId",
                table: "AspNetUserTokens");

            migrationBuilder.DropColumn(
                name: "AppRoleId",
                table: "AspNetUserRoles");

            migrationBuilder.DropColumn(
                name: "AppUserId",
                table: "AspNetUserRoles");

            migrationBuilder.DropColumn(
                name: "UserId1",
                table: "AspNetUserLogins");

            migrationBuilder.DropColumn(
                name: "AppUserId",
                table: "AspNetUserClaims");

            migrationBuilder.DropColumn(
                name: "AppRoleId",
                table: "AspNetRoleClaims");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "AppUserId",
                table: "AspNetUserTokens",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "AppRoleId",
                table: "AspNetUserRoles",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "AppUserId",
                table: "AspNetUserRoles",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "UserId1",
                table: "AspNetUserLogins",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "AppUserId",
                table: "AspNetUserClaims",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "AppRoleId",
                table: "AspNetRoleClaims",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserTokens_AppUserId",
                table: "AspNetUserTokens",
                column: "AppUserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserRoles_AppRoleId",
                table: "AspNetUserRoles",
                column: "AppRoleId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserRoles_AppUserId",
                table: "AspNetUserRoles",
                column: "AppUserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserLogins_UserId1",
                table: "AspNetUserLogins",
                column: "UserId1");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserClaims_AppUserId",
                table: "AspNetUserClaims",
                column: "AppUserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetRoleClaims_AppRoleId",
                table: "AspNetRoleClaims",
                column: "AppRoleId");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetRoleClaims_AspNetRoles_AppRoleId",
                table: "AspNetRoleClaims",
                column: "AppRoleId",
                principalTable: "AspNetRoles",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUserClaims_AspNetUsers_AppUserId",
                table: "AspNetUserClaims",
                column: "AppUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUserLogins_AspNetUsers_UserId1",
                table: "AspNetUserLogins",
                column: "UserId1",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUserRoles_AspNetRoles_AppRoleId",
                table: "AspNetUserRoles",
                column: "AppRoleId",
                principalTable: "AspNetRoles",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUserRoles_AspNetUsers_AppUserId",
                table: "AspNetUserRoles",
                column: "AppUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUserTokens_AspNetUsers_AppUserId",
                table: "AspNetUserTokens",
                column: "AppUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }
    }
}
