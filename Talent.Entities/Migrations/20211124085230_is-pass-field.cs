using Microsoft.EntityFrameworkCore.Migrations;

namespace Talent.Entities.Migrations
{
    public partial class ispassfield : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<float>(
                name: "HighestScore",
                table: "LiveAssesmentSkillCheckScores",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.AddColumn<bool>(
                name: "IsPass",
                table: "LiveAssesmentSkillCheckScores",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<float>(
                name: "LowestScore",
                table: "LiveAssesmentSkillCheckScores",
                nullable: false,
                defaultValue: 0f);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "HighestScore",
                table: "LiveAssesmentSkillCheckScores");

            migrationBuilder.DropColumn(
                name: "IsPass",
                table: "LiveAssesmentSkillCheckScores");

            migrationBuilder.DropColumn(
                name: "LowestScore",
                table: "LiveAssesmentSkillCheckScores");
        }
    }
}
