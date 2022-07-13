using Microsoft.EntityFrameworkCore.Migrations;

namespace Talent.Entities.Migrations
{
    public partial class remove_ProductCategoryName_ProductFeatureMapping : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ProductFeatureCategoryName",
                table: "ProductFeatureMappings");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ProductFeatureCategoryName",
                table: "ProductFeatureMappings",
                unicode: false,
                nullable: true);
        }
    }
}
