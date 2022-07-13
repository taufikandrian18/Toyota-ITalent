using Microsoft.EntityFrameworkCore.Migrations;

namespace Talent.Entities.Migrations
{
    public partial class assesmentId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AssesmentGUID",
                table: "LiveAssesmentSkillChecks",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_LiveAssesmentSkillChecks_AssesmentGUID",
                table: "LiveAssesmentSkillChecks",
                column: "AssesmentGUID");

            migrationBuilder.AddForeignKey(
                name: "FK_LiveAssesmentSkillChecks_Assesments_AssesmentGUID",
                table: "LiveAssesmentSkillChecks",
                column: "AssesmentGUID",
                principalTable: "Assesments",
                principalColumn: "GUID",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LiveAssesmentSkillChecks_Assesments_AssesmentGUID",
                table: "LiveAssesmentSkillChecks");

            migrationBuilder.DropIndex(
                name: "IX_LiveAssesmentSkillChecks_AssesmentGUID",
                table: "LiveAssesmentSkillChecks");

            migrationBuilder.DropColumn(
                name: "AssesmentGUID",
                table: "LiveAssesmentSkillChecks");
        }
    }
}
