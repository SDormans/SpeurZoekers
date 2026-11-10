using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Speurzoekers.Data.Migrations
{
    public partial class AddUserCollectiblesTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "UserCollectibles",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    ApplicationUserId = table.Column<Guid>(nullable: false),
                    CollectibleTypeId = table.Column<Guid>(nullable: false),
                    CollectiblePositionId = table.Column<Guid>(nullable: false),
                    QuestionAnswered = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserCollectibles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserCollectibles_AspNetUsers_ApplicationUserId",
                        column: x => x.ApplicationUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserCollectibles_CollectibleTypes_CollectibleTypeId",
                        column: x => x.CollectibleTypeId,
                        principalTable: "CollectibleTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserCollectibles_ApplicationUserId",
                table: "UserCollectibles",
                column: "ApplicationUserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserCollectibles_CollectibleTypeId",
                table: "UserCollectibles",
                column: "CollectibleTypeId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserCollectibles");
        }
    }
}
