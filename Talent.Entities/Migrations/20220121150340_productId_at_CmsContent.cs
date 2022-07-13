using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Talent.Entities.Migrations
{
    public partial class productId_at_CmsContent : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<bool>(
                name: "IsCompetitor",
                table: "Products",
                nullable: true,
                defaultValueSql: "((0))",
                oldClrType: typeof(bool),
                oldDefaultValueSql: "((0))");

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

            migrationBuilder.AddColumn<string>(
                name: "Category",
                table: "Coaches",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "ProductId",
                table: "Cms_Contents",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeCertificates_TrainingId",
                table: "EmployeeCertificates",
                column: "TrainingId");

            migrationBuilder.CreateIndex(
                name: "IX_Cms_Contents_ProductId",
                table: "Cms_Contents",
                column: "ProductId");

            migrationBuilder.AddForeignKey(
                name: "FK_CmsContents_Products",
                table: "Cms_Contents",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "ProductId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CmsContents_Products",
                table: "Cms_Contents");

            migrationBuilder.DropIndex(
                name: "IX_EmployeeCertificates_TrainingId",
                table: "EmployeeCertificates");

            migrationBuilder.DropIndex(
                name: "IX_Cms_Contents_ProductId",
                table: "Cms_Contents");

            migrationBuilder.DropColumn(
                name: "CertificationHeader",
                table: "EmployeeCertificates");

            migrationBuilder.DropColumn(
                name: "Category",
                table: "Coaches");

            migrationBuilder.DropColumn(
                name: "ProductId",
                table: "Cms_Contents");

            migrationBuilder.AlterColumn<bool>(
                name: "IsCompetitor",
                table: "Products",
                nullable: false,
                defaultValueSql: "((0))",
                oldClrType: typeof(bool),
                oldNullable: true,
                oldDefaultValueSql: "((0))");

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
