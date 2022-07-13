using Microsoft.EntityFrameworkCore.Migrations;

namespace Talent.Entities.Migrations
{
    public partial class cmsaddandproductcompetitorfeaturesmodified : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ProductFeatureCategoryName",
                table: "ProductFeatureMappings",
                unicode: false,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ProductFeatureCategoryName",
                table: "ProductFeatureMappings");
        }
    }
}
