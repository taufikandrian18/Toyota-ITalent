using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Talent.Entities.Migrations
{
    public partial class addfieldliveassesments : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<float>(
                name: "Attempts",
                table: "LiveAssesmentSkillChecks",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.AddColumn<bool>(
                name: "IsScored",
                table: "LiveAssesmentSkillChecks",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsByOption",
                table: "Assesments",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<float>(
                name: "MinimumScore",
                table: "Assesments",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.AddColumn<float>(
                name: "RemedialLimit",
                table: "Assesments",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.CreateTable(
                name: "LiveAssesmentSkillCheckScores",
                columns: table => new
                {
                    GUID = table.Column<string>(nullable: false),
                    EmployeeGUID = table.Column<string>(nullable: true),
                    LiveAssesmentSkillCheckGUID = table.Column<string>(nullable: true),
                    Attempts = table.Column<float>(nullable: false),
                    AssesmentGUID = table.Column<string>(nullable: true),
                    AverageScore = table.Column<float>(nullable: false),
                    CreatedAt = table.Column<DateTime>(nullable: true),
                    UpdatedAt = table.Column<DateTime>(nullable: true),
                    DeletedAt = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LiveAssesmentSkillCheckScores", x => x.GUID);
                    table.ForeignKey(
                        name: "FK_LiveAssesmentSkillCheckScores_Assesments_AssesmentGUID",
                        column: x => x.AssesmentGUID,
                        principalTable: "Assesments",
                        principalColumn: "GUID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_LiveAssesmentSkillCheckScores_Employees_EmployeeGUID",
                        column: x => x.EmployeeGUID,
                        principalTable: "Employees",
                        principalColumn: "EmployeeId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_LiveAssesmentSkillCheckScores_LiveAssesmentSkillChecks_LiveAssesmentSkillCheckGUID",
                        column: x => x.LiveAssesmentSkillCheckGUID,
                        principalTable: "LiveAssesmentSkillChecks",
                        principalColumn: "GUID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_LiveAssesmentSkillCheckScores_AssesmentGUID",
                table: "LiveAssesmentSkillCheckScores",
                column: "AssesmentGUID");

            migrationBuilder.CreateIndex(
                name: "IX_LiveAssesmentSkillCheckScores_EmployeeGUID",
                table: "LiveAssesmentSkillCheckScores",
                column: "EmployeeGUID");

            migrationBuilder.CreateIndex(
                name: "IX_LiveAssesmentSkillCheckScores_LiveAssesmentSkillCheckGUID",
                table: "LiveAssesmentSkillCheckScores",
                column: "LiveAssesmentSkillCheckGUID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "LiveAssesmentSkillCheckScores");

            migrationBuilder.DropColumn(
                name: "Attempts",
                table: "LiveAssesmentSkillChecks");

            migrationBuilder.DropColumn(
                name: "IsScored",
                table: "LiveAssesmentSkillChecks");

            migrationBuilder.DropColumn(
                name: "IsByOption",
                table: "Assesments");

            migrationBuilder.DropColumn(
                name: "MinimumScore",
                table: "Assesments");

            migrationBuilder.DropColumn(
                name: "RemedialLimit",
                table: "Assesments");
        }
    }
}
