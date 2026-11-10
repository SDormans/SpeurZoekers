using Microsoft.EntityFrameworkCore.Migrations;
using Speurzoekers.Common.Helper;

namespace Speurzoekers.Data.Migrations
{
    public partial class AddAvatarPriceConfig : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "AvatarPrice",
                table: "Configuration",
                nullable: false,
                defaultValue: Configuration.AvatarPrice);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AvatarPrice",
                table: "Configuration");
        }
    }
}
