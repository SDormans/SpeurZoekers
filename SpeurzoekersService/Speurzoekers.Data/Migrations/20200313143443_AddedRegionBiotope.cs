using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Speurzoekers.Data.Migrations
{
    public partial class AddedRegionBiotope : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<short>(
                name: "Biotope",
                table: "UserCollectibles",
                nullable: false,
                defaultValue: (short)0);

            migrationBuilder.AddColumn<short>(
                name: "Biotope",
                table: "Regions",
                nullable: false,
                defaultValue: (short)0);

            migrationBuilder.AddColumn<Guid>(
                name: "RegionId",
                table: "CollectiblePositions",
                nullable: false);

            migrationBuilder.CreateIndex(
                name: "IX_CollectiblePositions_RegionId",
                table: "CollectiblePositions",
                column: "RegionId");

            migrationBuilder.AddForeignKey(
                name: "FK_CollectiblePositions_Regions_RegionId",
                table: "CollectiblePositions",
                column: "RegionId",
                principalTable: "Regions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CollectiblePositions_Regions_RegionId",
                table: "CollectiblePositions");

            migrationBuilder.DropIndex(
                name: "IX_CollectiblePositions_RegionId",
                table: "CollectiblePositions");

            migrationBuilder.DropColumn(
                name: "Biotope",
                table: "UserCollectibles");

            migrationBuilder.DropColumn(
                name: "Biotope",
                table: "Regions");

            migrationBuilder.DropColumn(
                name: "RegionId",
                table: "CollectiblePositions");
        }
    }
}
