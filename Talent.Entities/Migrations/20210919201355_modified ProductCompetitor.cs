using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Talent.Entities.Migrations
{
    public partial class modifiedProductCompetitor : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_ProductCompetitorMappings",
                table: "ProductCompetitorMappings");

            migrationBuilder.AlterColumn<string>(
                name: "UpdatedBy",
                table: "ProductCompetitorMappings",
                unicode: false,
                maxLength: 64,
                nullable: false,
                defaultValueSql: "('SYSTEM')",
                oldClrType: typeof(string),
                oldMaxLength: 64);

            migrationBuilder.AlterColumn<DateTime>(
                name: "UpdatedAt",
                table: "ProductCompetitorMappings",
                nullable: false,
                defaultValueSql: "(getdate())",
                oldClrType: typeof(DateTime));

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "ProductCompetitorMappings",
                nullable: false,
                defaultValueSql: "((0))",
                oldClrType: typeof(bool));

            migrationBuilder.AlterColumn<string>(
                name: "CreatedBy",
                table: "ProductCompetitorMappings",
                unicode: false,
                maxLength: 64,
                nullable: false,
                defaultValueSql: "('SYSTEM')",
                oldClrType: typeof(string),
                oldMaxLength: 64);

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                table: "ProductCompetitorMappings",
                nullable: false,
                defaultValueSql: "(getdate())",
                oldClrType: typeof(DateTime));

            migrationBuilder.AddColumn<Guid>(
                name: "ProductCompetitorMappingId",
                table: "ProductCompetitorMappings",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddPrimaryKey(
                name: "PK_ProductCompetitorMappings",
                table: "ProductCompetitorMappings",
                column: "ProductCompetitorMappingId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductCompetitorMappings_ProductId",
                table: "ProductCompetitorMappings",
                column: "ProductId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_ProductCompetitorMappings",
                table: "ProductCompetitorMappings");

            migrationBuilder.DropIndex(
                name: "IX_ProductCompetitorMappings_ProductId",
                table: "ProductCompetitorMappings");

            migrationBuilder.DropColumn(
                name: "ProductCompetitorMappingId",
                table: "ProductCompetitorMappings");

            migrationBuilder.AlterColumn<string>(
                name: "UpdatedBy",
                table: "ProductCompetitorMappings",
                maxLength: 64,
                nullable: false,
                oldClrType: typeof(string),
                oldUnicode: false,
                oldMaxLength: 64,
                oldDefaultValueSql: "('SYSTEM')");

            migrationBuilder.AlterColumn<DateTime>(
                name: "UpdatedAt",
                table: "ProductCompetitorMappings",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldDefaultValueSql: "(getdate())");

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "ProductCompetitorMappings",
                nullable: false,
                oldClrType: typeof(bool),
                oldDefaultValueSql: "((0))");

            migrationBuilder.AlterColumn<string>(
                name: "CreatedBy",
                table: "ProductCompetitorMappings",
                maxLength: 64,
                nullable: false,
                oldClrType: typeof(string),
                oldUnicode: false,
                oldMaxLength: 64,
                oldDefaultValueSql: "('SYSTEM')");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                table: "ProductCompetitorMappings",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldDefaultValueSql: "(getdate())");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ProductCompetitorMappings",
                table: "ProductCompetitorMappings",
                columns: new[] { "ProductId", "ProductTypeId", "ProductCompetitorId", "ProductCompetitorTypeId" });
        }
    }
}
