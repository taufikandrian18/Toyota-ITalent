using Microsoft.EntityFrameworkCore.Migrations;

namespace Talent.Entities.Migrations
{
    public partial class fieldfiletype : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "TitleFileID",
                table: "Announcements",
                newName: "AnnouncementFileID");

            migrationBuilder.AddColumn<string>(
                name: "OnBoardingFileType",
                table: "OnBoardings",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AnnouncementFileType",
                table: "Announcements",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "OnBoardingFileType",
                table: "OnBoardings");

            migrationBuilder.DropColumn(
                name: "AnnouncementFileType",
                table: "Announcements");

            migrationBuilder.RenameColumn(
                name: "AnnouncementFileID",
                table: "Announcements",
                newName: "TitleFileID");
        }
    }
}
