using Microsoft.EntityFrameworkCore.Migrations;

namespace Talent.Entities.Migrations
{
    public partial class ApprovalStatus_ProductFeatureMapping : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ProductFeatureMappingApprovalStatus",
                table: "ProductFeatureMappings",
                unicode: false,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ProductFeatureMappingApprovalStatus",
                table: "ProductFeatureMappings");
        }
    }
}
