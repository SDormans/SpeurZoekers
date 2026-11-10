using Microsoft.EntityFrameworkCore.Migrations;

namespace Speurzoekers.Data.Migrations
{
    public partial class RemoveBoolUserCollectible : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "QuestionAnswered",
                table: "UserCollectibles");

            migrationBuilder.DropColumn(
                name: "QuestionCorrect",
                table: "UserCollectibles");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "QuestionAnswered",
                table: "UserCollectibles",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "QuestionCorrect",
                table: "UserCollectibles",
                nullable: false,
                defaultValue: false);
        }
    }
}
