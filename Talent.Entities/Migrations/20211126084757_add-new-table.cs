using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Talent.Entities.Migrations
{
    public partial class addnewtable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "SkillCheckGUID",
                table: "LiveAssesmentSkillCheckScores",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "TaskScores",
                columns: table => new
                {
                    GUID = table.Column<string>(nullable: false),
                    EmployeeGUID = table.Column<string>(nullable: true),
                    Attempts = table.Column<float>(nullable: false),
                    TaskID = table.Column<int>(nullable: true),
                    ModuleID = table.Column<int>(nullable: true),
                    AverageScore = table.Column<float>(nullable: false),
                    HighestScore = table.Column<float>(nullable: false),
                    LowestScore = table.Column<float>(nullable: false),
                    IsPass = table.Column<bool>(nullable: false),
                    CreatedAt = table.Column<DateTime>(nullable: true),
                    UpdatedAt = table.Column<DateTime>(nullable: true),
                    DeletedAt = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TaskScores", x => x.GUID);
                    table.ForeignKey(
                        name: "FK_TaskScores_Employees_EmployeeGUID",
                        column: x => x.EmployeeGUID,
                        principalTable: "Employees",
                        principalColumn: "EmployeeId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TaskScores_Modules_ModuleID",
                        column: x => x.ModuleID,
                        principalTable: "Modules",
                        principalColumn: "ModuleId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TaskScores_Tasks_TaskID",
                        column: x => x.TaskID,
                        principalTable: "Tasks",
                        principalColumn: "TaskId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_LiveAssesmentSkillCheckScores_SkillCheckGUID",
                table: "LiveAssesmentSkillCheckScores",
                column: "SkillCheckGUID");

            migrationBuilder.CreateIndex(
                name: "IX_TaskScores_EmployeeGUID",
                table: "TaskScores",
                column: "EmployeeGUID");

            migrationBuilder.CreateIndex(
                name: "IX_TaskScores_ModuleID",
                table: "TaskScores",
                column: "ModuleID");

            migrationBuilder.CreateIndex(
                name: "IX_TaskScores_TaskID",
                table: "TaskScores",
                column: "TaskID");

            migrationBuilder.AddForeignKey(
                name: "FK_LiveAssesmentSkillCheckScores_SkillChecks_SkillCheckGUID",
                table: "LiveAssesmentSkillCheckScores",
                column: "SkillCheckGUID",
                principalTable: "SkillChecks",
                principalColumn: "GUID",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LiveAssesmentSkillCheckScores_SkillChecks_SkillCheckGUID",
                table: "LiveAssesmentSkillCheckScores");

            migrationBuilder.DropTable(
                name: "TaskScores");

            migrationBuilder.DropIndex(
                name: "IX_LiveAssesmentSkillCheckScores_SkillCheckGUID",
                table: "LiveAssesmentSkillCheckScores");

            migrationBuilder.DropColumn(
                name: "SkillCheckGUID",
                table: "LiveAssesmentSkillCheckScores");
        }
    }
}
