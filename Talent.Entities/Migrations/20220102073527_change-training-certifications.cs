using Microsoft.EntityFrameworkCore.Migrations;

namespace Talent.Entities.Migrations
{
    public partial class changetrainingcertifications : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TrainingCertifications_Courses_CourseId",
                table: "TrainingCertifications");

            migrationBuilder.RenameColumn(
                name: "CourseId",
                table: "TrainingCertifications",
                newName: "RelatedTrainingId");

            migrationBuilder.RenameIndex(
                name: "IX_TrainingCertifications_CourseId",
                table: "TrainingCertifications",
                newName: "IX_TrainingCertifications_RelatedTrainingId");


            migrationBuilder.AddForeignKey(
                name: "FK_TrainingCertifications_Trainings_RelatedTrainingId",
                table: "TrainingCertifications",
                column: "RelatedTrainingId",
                principalTable: "Trainings",
                principalColumn: "TrainingId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TrainingCertifications_Trainings_RelatedTrainingId",
                table: "TrainingCertifications");

            migrationBuilder.RenameColumn(
                name: "RelatedTrainingId",
                table: "TrainingCertifications",
                newName: "CourseId");

            migrationBuilder.RenameIndex(
                name: "IX_TrainingCertifications_RelatedTrainingId",
                table: "TrainingCertifications",
                newName: "IX_TrainingCertifications_CourseId");

            migrationBuilder.AddForeignKey(
                name: "FK_TrainingCertifications_Courses_CourseId",
                table: "TrainingCertifications",
                column: "CourseId",
                principalTable: "Courses",
                principalColumn: "CourseId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
