using Microsoft.EntityFrameworkCore.Migrations;

namespace Speurzoekers.Data.Migrations
{
    public partial class ChangedCollectibleTypeChoiceIdName : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "CollectibleChoiceId",
                table: "CollectibleTypeChoices",
                newName: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Id",
                table: "CollectibleTypeChoices",
                newName: "CollectibleChoiceId");
        }
    }
}
