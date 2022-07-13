using Microsoft.EntityFrameworkCore.Migrations;

namespace Talent.Entities.Migrations
{
    public partial class updateentitymodulemapping : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AssesmentId",
                table: "SetupModules",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_SetupModules_AssesmentId",
                table: "SetupModules",
                column: "AssesmentId");

            migrationBuilder.AddForeignKey(
                name: "FK_SetupModules_Assesments_AssesmentId",
                table: "SetupModules",
                column: "AssesmentId",
                principalTable: "Assesments",
                principalColumn: "GUID",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SetupModules_Assesments_AssesmentId",
                table: "SetupModules");

            migrationBuilder.DropIndex(
                name: "IX_SetupModules_AssesmentId",
                table: "SetupModules");

            migrationBuilder.DropColumn(
                name: "AssesmentId",
                table: "SetupModules");
        }
    }
}
