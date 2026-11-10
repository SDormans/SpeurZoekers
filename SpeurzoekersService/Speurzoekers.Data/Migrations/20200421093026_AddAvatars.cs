using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Speurzoekers.Data.Migrations
{
    public partial class AddAvatars : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CatchRange");

            migrationBuilder.DropColumn(
                name: "AvatarTagName",
                table: "UserInfos");

            migrationBuilder.AddColumn<long>(
                name: "ActiveAvatarIndex",
                table: "UserInfos",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "CollectedAvatars",
                table: "UserInfos",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.CreateTable(
                name: "AvatarTypes",
                columns: table => new
                {
                    Index = table.Column<long>(nullable: false),
                    Name = table.Column<string>(maxLength: 256, nullable: true),
                    TagName = table.Column<string>(maxLength: 256, nullable: true),
                    Points = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AvatarTypes", x => x.Index);
                });

            migrationBuilder.CreateTable(
                name: "Configuration",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CatchRange = table.Column<float>(nullable: false),
                    SpottingRange = table.Column<float>(nullable: false),
                    StartAvatarIndex = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Configuration", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AvatarTypeTexts",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    TextId = table.Column<byte>(nullable: false),
                    Text = table.Column<string>(nullable: true),
                    AvatarTypeIndex = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AvatarTypeTexts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AvatarTypeTexts_AvatarTypes_AvatarTypeIndex",
                        column: x => x.AvatarTypeIndex,
                        principalTable: "AvatarTypes",
                        principalColumn: "Index",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserInfos_ActiveAvatarIndex",
                table: "UserInfos",
                column: "ActiveAvatarIndex");

            migrationBuilder.CreateIndex(
                name: "IX_AvatarTypes_Index",
                table: "AvatarTypes",
                column: "Index",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AvatarTypes_Name",
                table: "AvatarTypes",
                column: "Name",
                unique: true,
                filter: "[Name] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_AvatarTypeTexts_AvatarTypeIndex",
                table: "AvatarTypeTexts",
                column: "AvatarTypeIndex");

            migrationBuilder.AddForeignKey(
                name: "FK_UserInfos_AvatarTypes_ActiveAvatarIndex",
                table: "UserInfos",
                column: "ActiveAvatarIndex",
                principalTable: "AvatarTypes",
                principalColumn: "Index",
                onDelete: ReferentialAction.SetNull);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Regions_Regions_ParentId",
                table: "Regions");

            migrationBuilder.DropForeignKey(
                name: "FK_UserInfos_AvatarTypes_ActiveAvatarIndex",
                table: "UserInfos");

            migrationBuilder.DropTable(
                name: "AvatarTypeTexts");

            migrationBuilder.DropTable(
                name: "Configuration");

            migrationBuilder.DropTable(
                name: "AvatarTypes");

            migrationBuilder.DropIndex(
                name: "IX_UserInfos_ActiveAvatarIndex",
                table: "UserInfos");

            migrationBuilder.DropColumn(
                name: "ActiveAvatarIndex",
                table: "UserInfos");

            migrationBuilder.DropColumn(
                name: "CollectedAvatars",
                table: "UserInfos");

            migrationBuilder.AddColumn<string>(
                name: "AvatarTagName",
                table: "UserInfos",
                maxLength: 256,
                nullable: true);

            migrationBuilder.CreateTable(
                name: "CatchRange",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CatchRange = table.Column<float>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CatchRange", x => x.Id);
                });
        }
    }
}
