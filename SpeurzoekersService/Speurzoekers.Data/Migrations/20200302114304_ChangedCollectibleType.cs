using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Speurzoekers.Data.Migrations
{
    public partial class ChangedCollectibleType : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CollectibleTypeBiotopes");

            migrationBuilder.AddColumn<short>(
                name: "Biotopes",
                table: "CollectibleTypes",
                nullable: false,
                defaultValue: (short)0);

            migrationBuilder.AddColumn<short>(
                name: "MonthsAvailable",
                table: "CollectibleTypes",
                nullable: false,
                defaultValue: (short)0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Biotopes",
                table: "CollectibleTypes");

            migrationBuilder.DropColumn(
                name: "MonthsAvailable",
                table: "CollectibleTypes");

            migrationBuilder.CreateTable(
                name: "CollectibleTypeBiotopes",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    BiotopeId = table.Column<byte>(nullable: false),
                    CollectibleTypeId = table.Column<Guid>(nullable: false)
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

            migrationBuilder.CreateIndex(
                name: "IX_CollectibleTypeBiotopes_CollectibleTypeId_BiotopeId",
                table: "CollectibleTypeBiotopes",
                columns: new[] { "CollectibleTypeId", "BiotopeId" },
                unique: true);
        }
    }
}
