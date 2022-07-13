using Microsoft.EntityFrameworkCore.Migrations;

namespace Talent.Entities.Migrations
{
    public partial class CmsOperationFix : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductFeatureMappings_CmsOperations",
                table: "ProductFeatureMappings");

            migrationBuilder.CreateIndex(
                name: "IX_ProductFeatureMappings_Cms_OperationId",
                table: "ProductFeatureMappings",
                column: "Cms_OperationId");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductFeatureMappings_CmsOperations",
                table: "ProductFeatureMappings",
                column: "Cms_OperationId",
                principalTable: "Cms_Operations",
                principalColumn: "Cms_OperationId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductFeatureMappings_CmsOperations",
                table: "ProductFeatureMappings");

            migrationBuilder.DropIndex(
                name: "IX_ProductFeatureMappings_Cms_OperationId",
                table: "ProductFeatureMappings");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductFeatureMappings_CmsOperations",
                table: "ProductFeatureMappings",
                column: "Cms_ContentId",
                principalTable: "Cms_Operations",
                principalColumn: "Cms_OperationId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
