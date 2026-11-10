using Microsoft.EntityFrameworkCore.Migrations;

namespace Speurzoekers.Data.Migrations
{
    public partial class AddMapRefreshTime : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "MapRefreshRate",
                table: "Configuration",
                nullable: false,
                defaultValue: 15);

            migrationBuilder.AddColumn<int>(
                name: "MapRefreshStart",
                table: "Configuration",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MapRefreshRate",
                table: "Configuration");

            migrationBuilder.DropColumn(
                name: "MapRefreshStart",
                table: "Configuration");
        }
    }
}
