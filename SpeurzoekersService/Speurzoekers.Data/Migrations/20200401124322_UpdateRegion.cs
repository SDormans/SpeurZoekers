using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Speurzoekers.Data.Migrations
{
    public partial class UpdateRegion : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "MaxPositions",
                table: "Regions",
                nullable: false,
                defaultValue: 15);

            migrationBuilder.AddColumn<int>(
                name: "MinPositions",
                table: "Regions",
                nullable: false,
                defaultValue: 5);

            migrationBuilder.AddColumn<Guid>(
                name: "ParentId",
                table: "Regions",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Regions",
                nullable: true,
                maxLength: 256,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Regions_Name",
                table: "Regions",
                column: "Name",
                unique: true,
                filter: "[Name] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Regions_ParentId",
                table: "Regions",
                column: "ParentId");

            migrationBuilder.AddForeignKey(
                name: "FK_Regions_Regions_ParentId",
                table: "Regions",
                column: "ParentId",
                principalTable: "Regions",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Regions_Regions_ParentId",
                table: "Regions");

            migrationBuilder.DropIndex(
                name: "IX_Regions_ParentId",
                table: "Regions");

            migrationBuilder.DropColumn(
                name: "MaxPositions",
                table: "Regions");

            migrationBuilder.DropColumn(
                name: "MinPositions",
                table: "Regions");

            migrationBuilder.DropColumn(
                name: "ParentId",
                table: "Regions");
            
            migrationBuilder.DropIndex(
                name: "IX_Regions_Name",
                table: "Regions");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Regions",
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 256,
                oldNullable: true);
        }
    }
}
