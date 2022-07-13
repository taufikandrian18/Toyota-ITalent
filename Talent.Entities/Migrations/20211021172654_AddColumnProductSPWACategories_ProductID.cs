using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Talent.Entities.Migrations
{
    public partial class AddColumnProductSPWACategories_ProductID : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "ProductId",
                table: "ProductSPWACategories",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_ProductSPWACategories_ProductId",
                table: "ProductSPWACategories",
                column: "ProductId");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductSPWACategories_Products",
                table: "ProductSPWACategories",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "ProductId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductSPWACategories_Products",
                table: "ProductSPWACategories");

            migrationBuilder.DropIndex(
                name: "IX_ProductSPWACategories_ProductId",
                table: "ProductSPWACategories");

            migrationBuilder.DropColumn(
                name: "ProductId",
                table: "ProductSPWACategories");
        }
    }
}
