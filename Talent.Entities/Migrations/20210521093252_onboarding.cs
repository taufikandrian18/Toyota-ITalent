using Microsoft.EntityFrameworkCore.Migrations;

namespace Talent.Entities.Migrations
{
    public partial class onboarding : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "OnBoardings",
                columns: table => new
                {
                    OnBoardingID = table.Column<int>(unicode: false, nullable: false),
                    SectionNumber = table.Column<int>(unicode: false, nullable: false),
                    OnBoardingFileURL = table.Column<string>(unicode: false, nullable: true),
                    Description = table.Column<string>(unicode: false, maxLength: 1024, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OnBoardings", x => x.OnBoardingID);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "OnBoardings");
        }
    }
}
