using Microsoft.EntityFrameworkCore.Migrations;

namespace Talent.Entities.Migrations
{
    public partial class ProductTipFix : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "OutletId",
                table: "ProductTips",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ProductTips_OutletId",
                table: "ProductTips",
                column: "OutletId");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductTips_Outlets",
                table: "ProductTips",
                column: "OutletId",
                principalTable: "Outlets",
                principalColumn: "OutletId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductTips_Outlets",
                table: "ProductTips");

            migrationBuilder.DropIndex(
                name: "IX_ProductTips_OutletId",
                table: "ProductTips");

            migrationBuilder.DropColumn(
                name: "OutletId",
                table: "ProductTips");
        }
    }
}
