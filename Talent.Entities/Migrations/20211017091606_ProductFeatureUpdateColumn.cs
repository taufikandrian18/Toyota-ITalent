using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Talent.Entities.Migrations
{
    public partial class ProductFeatureUpdateColumn : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "ApprovedAt",
                table: "ProductFeatures",
                nullable: true,
                defaultValueSql: "(getdate())");

            migrationBuilder.AddColumn<string>(
                name: "IsApproved",
                table: "ProductFeatures",
                unicode: false,
                maxLength: 50,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ApprovedAt",
                table: "ProductFeatures");

            migrationBuilder.DropColumn(
                name: "IsApproved",
                table: "ProductFeatures");
        }
    }
}
