using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Talent.Entities.Migrations
{
    public partial class TBFinalScore : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "FinalScores",
                columns: table => new
                {
                    FinalScoreId = table.Column<Guid>(nullable: false),
                    EmployeeId = table.Column<string>(nullable: true),
                    SkillCheckGuid = table.Column<string>(nullable: true),
                    SetupModuleId = table.Column<int>(nullable: true),
                    AssesmentId = table.Column<string>(nullable: true),
                    ModuleId = table.Column<int>(nullable: true),
                    TrainingId = table.Column<int>(nullable: true),
                    CourseId = table.Column<int>(nullable: true),
                    FinalScore = table.Column<float>(nullable: false),
                    PassedStatus = table.Column<bool>(nullable: false),
                    CreatedAt = table.Column<DateTime>(nullable: false, defaultValueSql: "(getdate())")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FinalScores", x => x.FinalScoreId);
                    table.ForeignKey(
                        name: "FK_FinalScores_Assesments_AssesmentId",
                        column: x => x.AssesmentId,
                        principalTable: "Assesments",
                        principalColumn: "GUID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FinalScores_Courses_CourseId",
                        column: x => x.CourseId,
                        principalTable: "Courses",
                        principalColumn: "CourseId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FinalScores_Employees_EmployeeId",
                        column: x => x.EmployeeId,
                        principalTable: "Employees",
                        principalColumn: "EmployeeId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FinalScores_Modules_ModuleId",
                        column: x => x.ModuleId,
                        principalTable: "Modules",
                        principalColumn: "ModuleId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FinalScores_SetupModules_SetupModuleId",
                        column: x => x.SetupModuleId,
                        principalTable: "SetupModules",
                        principalColumn: "SetupModuleId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FinalScores_SkillChecks_SkillCheckGuid",
                        column: x => x.SkillCheckGuid,
                        principalTable: "SkillChecks",
                        principalColumn: "GUID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FinalScores_Trainings_TrainingId",
                        column: x => x.TrainingId,
                        principalTable: "Trainings",
                        principalColumn: "TrainingId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_FinalScores_AssesmentId",
                table: "FinalScores",
                column: "AssesmentId");

            migrationBuilder.CreateIndex(
                name: "IX_FinalScores_CourseId",
                table: "FinalScores",
                column: "CourseId");

            migrationBuilder.CreateIndex(
                name: "IX_FinalScores_EmployeeId",
                table: "FinalScores",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_FinalScores_ModuleId",
                table: "FinalScores",
                column: "ModuleId");

            migrationBuilder.CreateIndex(
                name: "IX_FinalScores_SetupModuleId",
                table: "FinalScores",
                column: "SetupModuleId");

            migrationBuilder.CreateIndex(
                name: "IX_FinalScores_SkillCheckGuid",
                table: "FinalScores",
                column: "SkillCheckGuid");

            migrationBuilder.CreateIndex(
                name: "IX_FinalScores_TrainingId",
                table: "FinalScores",
                column: "TrainingId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FinalScores");
        }
    }
}
