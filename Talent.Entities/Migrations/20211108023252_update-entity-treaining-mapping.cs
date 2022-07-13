using Microsoft.EntityFrameworkCore.Migrations;

namespace Talent.Entities.Migrations
{
    public partial class updateentitytreainingmapping : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AssesmentId",
                table: "SetupLearning",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Order",
                table: "SetupLearning",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_SetupLearning_AssesmentId",
                table: "SetupLearning",
                column: "AssesmentId");

            migrationBuilder.AddForeignKey(
                name: "FK_SetupLearning_Assesments_AssesmentId",
                table: "SetupLearning",
                column: "AssesmentId",
                principalTable: "Assesments",
                principalColumn: "GUID",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SetupLearning_Assesments_AssesmentId",
                table: "SetupLearning");

            migrationBuilder.DropIndex(
                name: "IX_SetupLearning_AssesmentId",
                table: "SetupLearning");

            migrationBuilder.DropColumn(
                name: "AssesmentId",
                table: "SetupLearning");

            migrationBuilder.DropColumn(
                name: "Order",
                table: "SetupLearning");
        }
    }
}
