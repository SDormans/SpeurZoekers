using Microsoft.EntityFrameworkCore.Migrations;

namespace Speurzoekers.Data.Migrations
{
    public partial class PutCatchPointsAndAnswerPointsInUserCollectible : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "AnswerPoints",
                table: "UserCollectibles",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "CatchPoints",
                table: "UserCollectibles",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AnswerPoints",
                table: "UserCollectibles");

            migrationBuilder.DropColumn(
                name: "CatchPoints",
                table: "UserCollectibles");
        }
    }
}
