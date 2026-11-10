using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Speurzoekers.Data.Migrations
{
    public partial class AddSpeciesTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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

            migrationBuilder.CreateIndex(
                name: "IX_SpeciesCoordinates_SpeciesId",
                table: "SpeciesCoordinates",
                column: "SpeciesId");

            migrationBuilder.AddForeignKey(
                name: "FK_SpeciesCoordinates_Species_SpeciesId",
                table: "SpeciesCoordinates",
                column: "SpeciesId",
                principalTable: "Species",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SpeciesCoordinates_Species_SpeciesId",
                table: "SpeciesCoordinates");

            migrationBuilder.DropTable(
                name: "Species");

            migrationBuilder.DropIndex(
                name: "IX_SpeciesCoordinates_SpeciesId",
                table: "SpeciesCoordinates");
        }
    }
}
