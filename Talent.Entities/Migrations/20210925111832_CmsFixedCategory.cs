using Microsoft.EntityFrameworkCore.Migrations;

namespace Talent.Entities.Migrations
{
    public partial class CmsFixedCategory : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Cms_SubContentCategory",
                table: "Cms_SubContents",
                unicode: false,
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Cms_MenuCategory",
                table: "Cms_Menus",
                unicode: false,
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Cms_ContentCategory",
                table: "Cms_Contents",
                unicode: false,
                maxLength: 50,
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Cms_SubContentCategory",
                table: "Cms_SubContents");

            migrationBuilder.DropColumn(
                name: "Cms_MenuCategory",
                table: "Cms_Menus");

            migrationBuilder.DropColumn(
                name: "Cms_ContentCategory",
                table: "Cms_Contents");
        }
    }
}
