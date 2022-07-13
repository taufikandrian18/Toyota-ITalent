using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Talent.Entities.Migrations
{
    public partial class ApprovedAtFeature1_AddingRest : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "ApprovedAt",
                table: "ServiceTips",
                nullable: true,
                defaultValueSql: "(getdate())");

            migrationBuilder.AddColumn<DateTime>(
                name: "ApprovedAt",
                table: "ProductTips",
                nullable: true,
                defaultValueSql: "(getdate())");

            migrationBuilder.AddColumn<DateTime>(
                name: "ApprovedAt",
                table: "ProductSPWAMappings",
                nullable: true,
                defaultValueSql: "(getdate())");

            migrationBuilder.AddColumn<DateTime>(
                name: "ApprovedAt",
                table: "ProductProgramMappings",
                nullable: true,
                defaultValueSql: "(getdate())");

            migrationBuilder.AddColumn<DateTime>(
                name: "ApprovedAt",
                table: "ProductPhotos",
                nullable: true,
                defaultValueSql: "(getdate())");

            migrationBuilder.AddColumn<DateTime>(
                name: "ApprovedAt",
                table: "ProductGalleries",
                nullable: true,
                defaultValueSql: "(getdate())");

            migrationBuilder.AddColumn<DateTime>(
                name: "ApprovedAt",
                table: "ProductFeatureMappings",
                nullable: true,
                defaultValueSql: "(getdate())");

            migrationBuilder.AddColumn<DateTime>(
                name: "ApprovedAt",
                table: "ProductFAQs",
                nullable: true,
                defaultValueSql: "(getdate())");

            migrationBuilder.AddColumn<DateTime>(
                name: "ApprovedAt",
                table: "ProductCustomers",
                nullable: true,
                defaultValueSql: "(getdate())");

            migrationBuilder.AddColumn<DateTime>(
                name: "ApprovedAt",
                table: "ProductCompetitorMappings",
                nullable: true,
                defaultValueSql: "(getdate())");

            migrationBuilder.AddColumn<DateTime>(
                name: "ApprovedAt",
                table: "Kodawaris",
                nullable: true,
                defaultValueSql: "(getdate())");

            migrationBuilder.AddColumn<DateTime>(
                name: "ApprovedAt",
                table: "KodawariDownloads",
                nullable: true,
                defaultValueSql: "(getdate())");

            migrationBuilder.AddColumn<DateTime>(
                name: "ApprovedAt",
                table: "KodawariBanners",
                nullable: true,
                defaultValueSql: "(getdate())");

            migrationBuilder.AddColumn<DateTime>(
                name: "ApprovedAt",
                table: "KeyPieceOfMinds",
                nullable: true,
                defaultValueSql: "(getdate())");

            migrationBuilder.AddColumn<DateTime>(
                name: "ApprovedAt",
                table: "HOGuidelines",
                nullable: true,
                defaultValueSql: "(getdate())");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ApprovedAt",
                table: "ServiceTips");

            migrationBuilder.DropColumn(
                name: "ApprovedAt",
                table: "ProductTips");

            migrationBuilder.DropColumn(
                name: "ApprovedAt",
                table: "ProductSPWAMappings");

            migrationBuilder.DropColumn(
                name: "ApprovedAt",
                table: "ProductProgramMappings");

            migrationBuilder.DropColumn(
                name: "ApprovedAt",
                table: "ProductPhotos");

            migrationBuilder.DropColumn(
                name: "ApprovedAt",
                table: "ProductGalleries");

            migrationBuilder.DropColumn(
                name: "ApprovedAt",
                table: "ProductFeatureMappings");

            migrationBuilder.DropColumn(
                name: "ApprovedAt",
                table: "ProductFAQs");

            migrationBuilder.DropColumn(
                name: "ApprovedAt",
                table: "ProductCustomers");

            migrationBuilder.DropColumn(
                name: "ApprovedAt",
                table: "ProductCompetitorMappings");

            migrationBuilder.DropColumn(
                name: "ApprovedAt",
                table: "Kodawaris");

            migrationBuilder.DropColumn(
                name: "ApprovedAt",
                table: "KodawariDownloads");

            migrationBuilder.DropColumn(
                name: "ApprovedAt",
                table: "KodawariBanners");

            migrationBuilder.DropColumn(
                name: "ApprovedAt",
                table: "KeyPieceOfMinds");

            migrationBuilder.DropColumn(
                name: "ApprovedAt",
                table: "HOGuidelines");
        }
    }
}
