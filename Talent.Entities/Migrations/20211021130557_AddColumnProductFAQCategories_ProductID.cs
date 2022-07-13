using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Talent.Entities.Migrations
{
    public partial class AddColumnProductFAQCategories_ProductID : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "ProductId",
                table: "ProductFAQCategories",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_ProductFAQCategories_ProductId",
                table: "ProductFAQCategories",
                column: "ProductId");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductFAQCategories_Products",
                table: "ProductFAQCategories",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "ProductId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductFAQCategories_Products",
                table: "ProductFAQCategories");

            migrationBuilder.DropIndex(
                name: "IX_ProductFAQCategories_ProductId",
                table: "ProductFAQCategories");

            migrationBuilder.DropColumn(
                name: "ProductId",
                table: "ProductFAQCategories");
        }
    }
}
