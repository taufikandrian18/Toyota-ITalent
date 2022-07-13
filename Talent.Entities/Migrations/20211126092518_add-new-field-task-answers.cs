using Microsoft.EntityFrameworkCore.Migrations;

namespace Talent.Entities.Migrations
{
    public partial class addnewfieldtaskanswers : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "TaskAnswerDetailID",
                table: "TaskScores",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "TaskAnswerID",
                table: "TaskScores",
                nullable: true);

            migrationBuilder.AddColumn<float>(
                name: "Attempts",
                table: "TaskAnswerDetails",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.CreateIndex(
                name: "IX_TaskScores_TaskAnswerDetailID",
                table: "TaskScores",
                column: "TaskAnswerDetailID");

            migrationBuilder.CreateIndex(
                name: "IX_TaskScores_TaskAnswerID",
                table: "TaskScores",
                column: "TaskAnswerID");

            migrationBuilder.AddForeignKey(
                name: "FK_TaskScores_TaskAnswerDetails_TaskAnswerDetailID",
                table: "TaskScores",
                column: "TaskAnswerDetailID",
                principalTable: "TaskAnswerDetails",
                principalColumn: "TaskAnswerDetailId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TaskScores_TaskAnswers_TaskAnswerID",
                table: "TaskScores",
                column: "TaskAnswerID",
                principalTable: "TaskAnswers",
                principalColumn: "TaskAnswerId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TaskScores_TaskAnswerDetails_TaskAnswerDetailID",
                table: "TaskScores");

            migrationBuilder.DropForeignKey(
                name: "FK_TaskScores_TaskAnswers_TaskAnswerID",
                table: "TaskScores");

            migrationBuilder.DropIndex(
                name: "IX_TaskScores_TaskAnswerDetailID",
                table: "TaskScores");

            migrationBuilder.DropIndex(
                name: "IX_TaskScores_TaskAnswerID",
                table: "TaskScores");

            migrationBuilder.DropColumn(
                name: "TaskAnswerDetailID",
                table: "TaskScores");

            migrationBuilder.DropColumn(
                name: "TaskAnswerID",
                table: "TaskScores");

            migrationBuilder.DropColumn(
                name: "Attempts",
                table: "TaskAnswerDetails");
        }
    }
}
