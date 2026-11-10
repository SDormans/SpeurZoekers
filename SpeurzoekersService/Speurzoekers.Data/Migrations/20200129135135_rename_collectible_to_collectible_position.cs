using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Speurzoekers.Data.Migrations
{
    public partial class rename_collectible_to_collectible_position : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Collectible");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CollectibleType",
                table: "CollectibleTypes");

            migrationBuilder.RenameTable(
                name: "CollectibleTypes",
                newName: "CollectibleTypes");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CollectibleTypes",
                table: "CollectibleTypes",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "CollectiblePositions",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CollectibleTypeId = table.Column<Guid>(nullable: false),
                    Longitude = table.Column<float>(nullable: false),
                    Latitude = table.Column<float>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CollectiblePositions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CollectiblePositions_CollectibleTypes_CollectibleTypeId",
                        column: x => x.CollectibleTypeId,
                        principalTable: "CollectibleTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CollectiblePositions_CollectibleTypeId",
                table: "CollectiblePositions",
                column: "CollectibleTypeId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CollectiblePositions");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CollectibleTypes",
                table: "CollectibleTypes");

            migrationBuilder.RenameTable(
                name: "CollectibleTypes",
                newName: "CollectibleTypes");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CollectibleType",
                table: "CollectibleTypes",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "Collectible",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CollectibleTypeId = table.Column<Guid>(nullable: false),
                    Latitude = table.Column<float>(nullable: false),
                    Longitude = table.Column<float>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Collectible", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Collectible_CollectibleType_CollectibleTypeId",
                        column: x => x.CollectibleTypeId,
                        principalTable: "CollectibleTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Collectible_CollectibleTypeId",
                table: "Collectible",
                column: "CollectibleTypeId");
        }
    }
}
