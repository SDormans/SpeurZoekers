using Microsoft.EntityFrameworkCore.Migrations;

namespace Speurzoekers.Data.Migrations
{
    public partial class QuestionCorrect : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "QuestionCorrect",
                table: "UserCollectibles",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "QuestionCorrect",
                table: "UserCollectibles");
        }
    }
}
