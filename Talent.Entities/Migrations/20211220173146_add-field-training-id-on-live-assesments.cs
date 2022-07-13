using Microsoft.EntityFrameworkCore.Migrations;

namespace Talent.Entities.Migrations
{
    public partial class addfieldtrainingidonliveassesments : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "TrainingId",
                table: "LiveAssesmentSkillChecks",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_LiveAssesmentSkillChecks_TrainingId",
                table: "LiveAssesmentSkillChecks",
                column: "TrainingId");

            migrationBuilder.AddForeignKey(
                name: "FK_LiveAssesmentSkillChecks_Trainings_TrainingId",
                table: "LiveAssesmentSkillChecks",
                column: "TrainingId",
                principalTable: "Trainings",
                principalColumn: "TrainingId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LiveAssesmentSkillChecks_Trainings_TrainingId",
                table: "LiveAssesmentSkillChecks");

            migrationBuilder.DropIndex(
                name: "IX_LiveAssesmentSkillChecks_TrainingId",
                table: "LiveAssesmentSkillChecks");

            migrationBuilder.DropColumn(
                name: "TrainingId",
                table: "LiveAssesmentSkillChecks");
        }
    }
}
