using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Speurzoekers.Data.Migrations
{
    public partial class RefactorChanges : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SpeciesCoordinates");

            migrationBuilder.DropTable(
                name: "Species");

            migrationBuilder.CreateTable(
                name: "CollectibleTypes",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    TagName = table.Column<string>(maxLength: 256, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CollectibleType", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Collectible",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CollectibleTypeId = table.Column<Guid>(nullable: false),
                    Longitude = table.Column<float>(nullable: false),
                    Latitude = table.Column<float>(nullable: false)
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

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Collectible");

            migrationBuilder.DropTable(
                name: "CollectibleTypes");

            migrationBuilder.CreateTable(
                name: "Species",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    SpecPicName = table.Column<string>(maxLength: 256, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Species", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SpeciesCoordinates",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Latitude = table.Column<float>(nullable: false),
                    Longitude = table.Column<float>(nullable: false),
                    SpecPicName = table.Column<string>(nullable: true),
                    SpeciesId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SpeciesCoordinates", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SpeciesCoordinates_Species_SpeciesId",
                        column: x => x.SpeciesId,
                        principalTable: "Species",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SpeciesCoordinates_SpeciesId",
                table: "SpeciesCoordinates",
                column: "SpeciesId");
        }
    }
}
