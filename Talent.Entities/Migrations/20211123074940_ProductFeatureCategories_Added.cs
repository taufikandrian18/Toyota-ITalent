using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Talent.Entities.Migrations
{
    public partial class ProductFeatureCategories_Added : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "ProductFeatureCategoryId",
                table: "ProductFeatureMappings",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateTable(
                name: "ProductFeatureCategories",
                columns: table => new
                {
                    ProductFeatureCategoryId = table.Column<Guid>(nullable: false),
                    ProductFeatureCategoryName = table.Column<string>(unicode: false, maxLength: 150, nullable: false),
                    CreatedAt = table.Column<DateTime>(nullable: false, defaultValueSql: "(getdate())"),
                    CreatedBy = table.Column<string>(unicode: false, maxLength: 64, nullable: false, defaultValueSql: "('SYSTEM')"),
                    UpdatedAt = table.Column<DateTime>(nullable: false, defaultValueSql: "(getdate())"),
                    UpdatedBy = table.Column<string>(unicode: false, maxLength: 64, nullable: false, defaultValueSql: "('SYSTEM')"),
                    IsDeleted = table.Column<bool>(nullable: false, defaultValueSql: "((0))")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductFeatureCategories", x => x.ProductFeatureCategoryId);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ProductFeatureMappings_ProductFeatureCategoryId",
                table: "ProductFeatureMappings",
                column: "ProductFeatureCategoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductFeatureMappings_ProductFeatureCategories",
                table: "ProductFeatureMappings",
                column: "ProductFeatureCategoryId",
                principalTable: "ProductFeatureCategories",
                principalColumn: "ProductFeatureCategoryId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductFeatureMappings_ProductFeatureCategories",
                table: "ProductFeatureMappings");

            migrationBuilder.DropTable(
                name: "ProductFeatureCategories");

            migrationBuilder.DropIndex(
                name: "IX_ProductFeatureMappings_ProductFeatureCategoryId",
                table: "ProductFeatureMappings");

            migrationBuilder.DropColumn(
                name: "ProductFeatureCategoryId",
                table: "ProductFeatureMappings");
        }
    }
}
