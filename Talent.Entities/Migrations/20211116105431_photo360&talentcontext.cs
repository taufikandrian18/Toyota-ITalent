using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Talent.Entities.Migrations
{
    public partial class photo360talentcontext : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "ApprovedAt",
                table: "ServiceTips",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldNullable: true,
                oldDefaultValueSql: "(getdate())");

            migrationBuilder.AlterColumn<DateTime>(
                name: "ApprovedAt",
                table: "ProductTypes",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldNullable: true,
                oldDefaultValueSql: "(getdate())");

            migrationBuilder.AlterColumn<DateTime>(
                name: "ApprovedAt",
                table: "ProductTips",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldNullable: true,
                oldDefaultValueSql: "(getdate())");

            migrationBuilder.AlterColumn<DateTime>(
                name: "ApprovedAt",
                table: "ProductSPWAMappings",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldNullable: true,
                oldDefaultValueSql: "(getdate())");

            migrationBuilder.AlterColumn<bool>(
                name: "IsCompetitor",
                table: "Products",
                nullable: false,
                defaultValueSql: "((0))",
                oldClrType: typeof(bool));

            migrationBuilder.AlterColumn<DateTime>(
                name: "ApprovedAt",
                table: "Products",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldNullable: true,
                oldDefaultValueSql: "(getdate())");

            migrationBuilder.AlterColumn<DateTime>(
                name: "ApprovedAt",
                table: "ProductProgramMappings",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldNullable: true,
                oldDefaultValueSql: "(getdate())");

            migrationBuilder.AlterColumn<DateTime>(
                name: "ApprovedAt",
                table: "ProductPhotos",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldNullable: true,
                oldDefaultValueSql: "(getdate())");

            migrationBuilder.AddColumn<bool>(
                name: "Is360Photo",
                table: "ProductPhotos",
                nullable: false,
                defaultValueSql: "((0))");

            migrationBuilder.AlterColumn<bool>(
                name: "IsColor",
                table: "ProductGalleries",
                nullable: false,
                defaultValueSql: "((0))",
                oldClrType: typeof(bool));

            migrationBuilder.AlterColumn<DateTime>(
                name: "ApprovedAt",
                table: "ProductGalleries",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldNullable: true,
                oldDefaultValueSql: "(getdate())");

            migrationBuilder.AlterColumn<DateTime>(
                name: "ApprovedAt",
                table: "ProductFeatures",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldNullable: true,
                oldDefaultValueSql: "(getdate())");

            migrationBuilder.AlterColumn<DateTime>(
                name: "ApprovedAt",
                table: "ProductFeatureMappings",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldNullable: true,
                oldDefaultValueSql: "(getdate())");

            migrationBuilder.AlterColumn<DateTime>(
                name: "ApprovedAt",
                table: "ProductFAQs",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldNullable: true,
                oldDefaultValueSql: "(getdate())");

            migrationBuilder.AlterColumn<DateTime>(
                name: "ApprovedAt",
                table: "ProductCustomers",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldNullable: true,
                oldDefaultValueSql: "(getdate())");

            migrationBuilder.AlterColumn<DateTime>(
                name: "ApprovedAt",
                table: "ProductCompetitorMappings",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldNullable: true,
                oldDefaultValueSql: "(getdate())");

            migrationBuilder.AlterColumn<DateTime>(
                name: "ApprovedAt",
                table: "Kodawaris",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldNullable: true,
                oldDefaultValueSql: "(getdate())");

            migrationBuilder.AlterColumn<bool>(
                name: "IsDownloadable",
                table: "KodawariDownloads",
                nullable: false,
                defaultValueSql: "((0))",
                oldClrType: typeof(bool));

            migrationBuilder.AlterColumn<DateTime>(
                name: "ApprovedAt",
                table: "KodawariDownloads",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldNullable: true,
                oldDefaultValueSql: "(getdate())");

            migrationBuilder.AlterColumn<DateTime>(
                name: "ApprovedAt",
                table: "KodawariBanners",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldNullable: true,
                oldDefaultValueSql: "(getdate())");

            migrationBuilder.AlterColumn<DateTime>(
                name: "ApprovedAt",
                table: "KeyPieceOfMinds",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldNullable: true,
                oldDefaultValueSql: "(getdate())");

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "HOGuidelines",
                nullable: false,
                defaultValueSql: "((0))",
                oldClrType: typeof(bool));

            migrationBuilder.AlterColumn<bool>(
                name: "IsApproved",
                table: "HOGuidelines",
                nullable: false,
                defaultValueSql: "((0))",
                oldClrType: typeof(bool));

            migrationBuilder.AlterColumn<DateTime>(
                name: "ApprovedAt",
                table: "HOGuidelines",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldNullable: true,
                oldDefaultValueSql: "(getdate())");

            migrationBuilder.AlterColumn<bool>(
                name: "DictionaryStatus",
                table: "Dictionaries",
                nullable: false,
                defaultValueSql: "((0))",
                oldClrType: typeof(bool));

            migrationBuilder.AlterColumn<DateTime>(
                name: "ApprovedAt",
                table: "Dictionaries",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldNullable: true,
                oldDefaultValueSql: "(getdate())");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Is360Photo",
                table: "ProductPhotos");

            migrationBuilder.AlterColumn<DateTime>(
                name: "ApprovedAt",
                table: "ServiceTips",
                nullable: true,
                defaultValueSql: "(getdate())",
                oldClrType: typeof(DateTime),
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "ApprovedAt",
                table: "ProductTypes",
                nullable: true,
                defaultValueSql: "(getdate())",
                oldClrType: typeof(DateTime),
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "ApprovedAt",
                table: "ProductTips",
                nullable: true,
                defaultValueSql: "(getdate())",
                oldClrType: typeof(DateTime),
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "ApprovedAt",
                table: "ProductSPWAMappings",
                nullable: true,
                defaultValueSql: "(getdate())",
                oldClrType: typeof(DateTime),
                oldNullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "IsCompetitor",
                table: "Products",
                nullable: false,
                oldClrType: typeof(bool),
                oldDefaultValueSql: "((0))");

            migrationBuilder.AlterColumn<DateTime>(
                name: "ApprovedAt",
                table: "Products",
                nullable: true,
                defaultValueSql: "(getdate())",
                oldClrType: typeof(DateTime),
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "ApprovedAt",
                table: "ProductProgramMappings",
                nullable: true,
                defaultValueSql: "(getdate())",
                oldClrType: typeof(DateTime),
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "ApprovedAt",
                table: "ProductPhotos",
                nullable: true,
                defaultValueSql: "(getdate())",
                oldClrType: typeof(DateTime),
                oldNullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "IsColor",
                table: "ProductGalleries",
                nullable: false,
                oldClrType: typeof(bool),
                oldDefaultValueSql: "((0))");

            migrationBuilder.AlterColumn<DateTime>(
                name: "ApprovedAt",
                table: "ProductGalleries",
                nullable: true,
                defaultValueSql: "(getdate())",
                oldClrType: typeof(DateTime),
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "ApprovedAt",
                table: "ProductFeatures",
                nullable: true,
                defaultValueSql: "(getdate())",
                oldClrType: typeof(DateTime),
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "ApprovedAt",
                table: "ProductFeatureMappings",
                nullable: true,
                defaultValueSql: "(getdate())",
                oldClrType: typeof(DateTime),
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "ApprovedAt",
                table: "ProductFAQs",
                nullable: true,
                defaultValueSql: "(getdate())",
                oldClrType: typeof(DateTime),
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "ApprovedAt",
                table: "ProductCustomers",
                nullable: true,
                defaultValueSql: "(getdate())",
                oldClrType: typeof(DateTime),
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "ApprovedAt",
                table: "ProductCompetitorMappings",
                nullable: true,
                defaultValueSql: "(getdate())",
                oldClrType: typeof(DateTime),
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "ApprovedAt",
                table: "Kodawaris",
                nullable: true,
                defaultValueSql: "(getdate())",
                oldClrType: typeof(DateTime),
                oldNullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "IsDownloadable",
                table: "KodawariDownloads",
                nullable: false,
                oldClrType: typeof(bool),
                oldDefaultValueSql: "((0))");

            migrationBuilder.AlterColumn<DateTime>(
                name: "ApprovedAt",
                table: "KodawariDownloads",
                nullable: true,
                defaultValueSql: "(getdate())",
                oldClrType: typeof(DateTime),
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "ApprovedAt",
                table: "KodawariBanners",
                nullable: true,
                defaultValueSql: "(getdate())",
                oldClrType: typeof(DateTime),
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "ApprovedAt",
                table: "KeyPieceOfMinds",
                nullable: true,
                defaultValueSql: "(getdate())",
                oldClrType: typeof(DateTime),
                oldNullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "HOGuidelines",
                nullable: false,
                oldClrType: typeof(bool),
                oldDefaultValueSql: "((0))");

            migrationBuilder.AlterColumn<bool>(
                name: "IsApproved",
                table: "HOGuidelines",
                nullable: false,
                oldClrType: typeof(bool),
                oldDefaultValueSql: "((0))");

            migrationBuilder.AlterColumn<DateTime>(
                name: "ApprovedAt",
                table: "HOGuidelines",
                nullable: true,
                defaultValueSql: "(getdate())",
                oldClrType: typeof(DateTime),
                oldNullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "DictionaryStatus",
                table: "Dictionaries",
                nullable: false,
                oldClrType: typeof(bool),
                oldDefaultValueSql: "((0))");

            migrationBuilder.AlterColumn<DateTime>(
                name: "ApprovedAt",
                table: "Dictionaries",
                nullable: true,
                defaultValueSql: "(getdate())",
                oldClrType: typeof(DateTime),
                oldNullable: true);
        }
    }
}
