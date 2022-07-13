using Microsoft.EntityFrameworkCore.Migrations;

namespace Talent.Entities.Migrations
{
    public partial class employeecertificate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "EnumCertificate",
                table: "Trainings",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsCertificate",
                table: "Trainings",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsFinished",
                table: "LiveAssesmentSkillChecks",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AlterColumn<string>(
                name: "Venue",
                table: "EmployeeCertificates",
                unicode: false,
                nullable: true,
                oldClrType: typeof(string),
                oldUnicode: false,
                oldMaxLength: 255);

            migrationBuilder.AddColumn<string>(
                name: "CertificationHeader",
                table: "EmployeeCertificates",
                maxLength: 255,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "TrainingId",
                table: "EmployeeCertificates",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeCertificates_TrainingId",
                table: "EmployeeCertificates",
                column: "TrainingId");

            migrationBuilder.AddForeignKey(
                name: "FK_EmployeeCertificates_Trainings_TrainingId",
                table: "EmployeeCertificates",
                column: "TrainingId",
                principalTable: "Trainings",
                principalColumn: "TrainingId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EmployeeCertificates_Trainings_TrainingId",
                table: "EmployeeCertificates");

            migrationBuilder.DropIndex(
                name: "IX_EmployeeCertificates_TrainingId",
                table: "EmployeeCertificates");

            migrationBuilder.DropColumn(
                name: "EnumCertificate",
                table: "Trainings");

            migrationBuilder.DropColumn(
                name: "IsCertificate",
                table: "Trainings");

            migrationBuilder.DropColumn(
                name: "IsFinished",
                table: "LiveAssesmentSkillChecks");

            migrationBuilder.DropColumn(
                name: "CertificationHeader",
                table: "EmployeeCertificates");

            migrationBuilder.DropColumn(
                name: "TrainingId",
                table: "EmployeeCertificates");

            migrationBuilder.AlterColumn<string>(
                name: "Venue",
                table: "EmployeeCertificates",
                unicode: false,
                maxLength: 255,
                nullable: false,
                oldClrType: typeof(string),
                oldUnicode: false,
                oldNullable: true);
        }
    }
}
