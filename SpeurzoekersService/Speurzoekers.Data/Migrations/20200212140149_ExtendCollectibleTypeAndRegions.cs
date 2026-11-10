using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Speurzoekers.Data.Migrations
{
    public partial class ExtendCollectibleTypeAndRegions : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "CollectibleTypes",
                maxLength: 256,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Points",
                table: "CollectibleTypes",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "QuestionPoints",
                table: "CollectibleTypes",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "QuestionText",
                table: "CollectibleTypes",
                maxLength: 255,
                nullable: true);

            migrationBuilder.AddColumn<byte>(
                name: "Rarity",
                table: "CollectibleTypes",
                nullable: false,
                defaultValue: (byte)0);

            migrationBuilder.CreateTable(
                name: "CollectibleTypeBiotopes",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CollectibleTypeId = table.Column<Guid>(nullable: false),
                    BiotopeId = table.Column<byte>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CollectibleTypeBiotopes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CollectibleTypeBiotopes_CollectibleTypes_CollectibleTypeId",
                        column: x => x.CollectibleTypeId,
                        principalTable: "CollectibleTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CollectibleTypeChoices",
                columns: table => new
                {
                    CollectibleChoiceId = table.Column<Guid>(nullable: false),
                    CollectibleTypeId = table.Column<Guid>(nullable: false),
                    Text = table.Column<string>(maxLength: 255, nullable: true),
                    IsAnswer = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CollectibleTypeChoices", x => x.CollectibleChoiceId);
                    table.ForeignKey(
                        name: "FK_CollectibleTypeChoices_CollectibleTypes_CollectibleTypeId",
                        column: x => x.CollectibleTypeId,
                        principalTable: "CollectibleTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CollectibleTypeTexts",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CollectibleTypeId = table.Column<Guid>(nullable: false),
                    TextId = table.Column<byte>(nullable: false),
                    Text = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CollectibleTypeTexts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CollectibleTypeTexts_CollectibleTypes_CollectibleTypeId",
                        column: x => x.CollectibleTypeId,
                        principalTable: "CollectibleTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Regions",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    Longitude = table.Column<float>(nullable: false),
                    Latitude = table.Column<float>(nullable: false),
                    Polygon = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Regions", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CollectibleTypes_Name",
                table: "CollectibleTypes",
                column: "Name",
                unique: true,
                filter: "[Name] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_CollectibleTypeBiotopes_CollectibleTypeId_BiotopeId",
                table: "CollectibleTypeBiotopes",
                columns: new[] { "CollectibleTypeId", "BiotopeId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CollectibleTypeChoices_CollectibleTypeId",
                table: "CollectibleTypeChoices",
                column: "CollectibleTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_CollectibleTypeTexts_CollectibleTypeId_TextId",
                table: "CollectibleTypeTexts",
                columns: new[] { "CollectibleTypeId", "TextId" },
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CollectibleTypeBiotopes");

            migrationBuilder.DropTable(
                name: "CollectibleTypeChoices");

            migrationBuilder.DropTable(
                name: "CollectibleTypeTexts");

            migrationBuilder.DropTable(
                name: "Regions");

            migrationBuilder.DropIndex(
                name: "IX_CollectibleTypes_Name",
                table: "CollectibleTypes");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "CollectibleTypes");

            migrationBuilder.DropColumn(
                name: "Points",
                table: "CollectibleTypes");

            migrationBuilder.DropColumn(
                name: "QuestionPoints",
                table: "CollectibleTypes");

            migrationBuilder.DropColumn(
                name: "QuestionText",
                table: "CollectibleTypes");

            migrationBuilder.DropColumn(
                name: "Rarity",
                table: "CollectibleTypes");
        }
    }
}
