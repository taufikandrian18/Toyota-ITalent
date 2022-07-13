using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Talent.Entities.Migrations
{
    public partial class remedialandcertificationupdate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Feedback",
                table: "TaskAnswerDetails",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsFinished",
                table: "TaskAnswerDetails",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "ScorerGUID",
                table: "TaskAnswerDetails",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "TrainingCertifications",
                columns: table => new
                {
                    GUID = table.Column<string>(nullable: false),
                    TrainingId = table.Column<int>(nullable: true),
                    CourseId = table.Column<int>(nullable: true),
                    CreatedAt = table.Column<DateTime>(nullable: true),
                    UpdatedAt = table.Column<DateTime>(nullable: true),
                    DeletedAt = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TrainingCertifications", x => x.GUID);
                    table.ForeignKey(
                        name: "FK_TrainingCertifications_Courses_CourseId",
                        column: x => x.CourseId,
                        principalTable: "Courses",
                        principalColumn: "CourseId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TrainingCertifications_Trainings_TrainingId",
                        column: x => x.TrainingId,
                        principalTable: "Trainings",
                        principalColumn: "TrainingId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TaskAnswerDetails_ScorerGUID",
                table: "TaskAnswerDetails",
                column: "ScorerGUID");

            migrationBuilder.CreateIndex(
                name: "IX_TrainingCertifications_CourseId",
                table: "TrainingCertifications",
                column: "CourseId");

            migrationBuilder.CreateIndex(
                name: "IX_TrainingCertifications_TrainingId",
                table: "TrainingCertifications",
                column: "TrainingId");

            migrationBuilder.AddForeignKey(
                name: "FK_TaskAnswerDetails_Employees_ScorerGUID",
                table: "TaskAnswerDetails",
                column: "ScorerGUID",
                principalTable: "Employees",
                principalColumn: "EmployeeId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TaskAnswerDetails_Employees_ScorerGUID",
                table: "TaskAnswerDetails");

            migrationBuilder.DropTable(
                name: "TrainingCertifications");

            migrationBuilder.DropIndex(
                name: "IX_TaskAnswerDetails_ScorerGUID",
                table: "TaskAnswerDetails");

            migrationBuilder.DropColumn(
                name: "Feedback",
                table: "TaskAnswerDetails");

            migrationBuilder.DropColumn(
                name: "IsFinished",
                table: "TaskAnswerDetails");

            migrationBuilder.DropColumn(
                name: "ScorerGUID",
                table: "TaskAnswerDetails");
        }
    }
}
