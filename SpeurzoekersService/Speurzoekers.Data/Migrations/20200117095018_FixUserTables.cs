using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Speurzoekers.Data.Migrations
{
    public partial class FixUserTables : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_UserCoordinates",
                table: "UserCoordinates");

            migrationBuilder.DropIndex(
                name: "IX_UserCoordinates_UserId",
                table: "UserCoordinates");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserAvatars",
                table: "UserAvatars");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "UserCoordinates");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "UserAvatars");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserCoordinates",
                table: "UserCoordinates",
                column: "UserId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserAvatars",
                table: "UserAvatars",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_UserAvatars_AspNetUsers_UserId",
                table: "UserAvatars",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserAvatars_AspNetUsers_UserId",
                table: "UserAvatars");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserCoordinates",
                table: "UserCoordinates");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserAvatars",
                table: "UserAvatars");

            migrationBuilder.AddColumn<Guid>(
                name: "Id",
                table: "UserCoordinates",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "Id",
                table: "UserAvatars",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserCoordinates",
                table: "UserCoordinates",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserAvatars",
                table: "UserAvatars",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_UserCoordinates_UserId",
                table: "UserCoordinates",
                column: "UserId");
        }
    }
}
