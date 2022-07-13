using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Talent.Entities.Migrations
{
    public partial class ProductTipProductTipCategory : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ProductTipCategories",
                columns: table => new
                {
                    ProductTipCategoryId = table.Column<Guid>(nullable: false),
                    ProductTipCategoryName = table.Column<string>(unicode: false, maxLength: 50, nullable: false),
                    CreatedAt = table.Column<DateTime>(nullable: false, defaultValueSql: "(getdate())"),
                    CreatedBy = table.Column<string>(unicode: false, maxLength: 64, nullable: false, defaultValueSql: "('SYSTEM')"),
                    UpdatedAt = table.Column<DateTime>(nullable: false, defaultValueSql: "(getdate())"),
                    UpdatedBy = table.Column<string>(unicode: false, maxLength: 64, nullable: false, defaultValueSql: "('SYSTEM')"),
                    IsDeleted = table.Column<bool>(nullable: false, defaultValueSql: "((0))")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductTipCategories", x => x.ProductTipCategoryId);
                });

            migrationBuilder.CreateTable(
                name: "ProductTips",
                columns: table => new
                {
                    ProductTipId = table.Column<Guid>(nullable: false),
                    ProductId = table.Column<Guid>(nullable: false),
                    BlobId = table.Column<Guid>(nullable: false),
                    ProductTipCategoryId = table.Column<Guid>(nullable: false),
                    ProductTipTitle = table.Column<string>(unicode: false, maxLength: 50, nullable: false),
                    ProductTipDescription = table.Column<string>(unicode: false, maxLength: 250, nullable: false),
                    IsSequence = table.Column<int>(nullable: false),
                    CreatedAt = table.Column<DateTime>(nullable: false, defaultValueSql: "(getdate())"),
                    CreatedBy = table.Column<string>(unicode: false, maxLength: 64, nullable: false, defaultValueSql: "('SYSTEM')"),
                    UpdatedAt = table.Column<DateTime>(nullable: false, defaultValueSql: "(getdate())"),
                    UpdatedBy = table.Column<string>(unicode: false, maxLength: 64, nullable: false, defaultValueSql: "('SYSTEM')"),
                    IsDeleted = table.Column<bool>(nullable: false, defaultValueSql: "((0))")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductTips", x => x.ProductTipId);
                    table.ForeignKey(
                        name: "FK_ProductTips_Blobs",
                        column: x => x.BlobId,
                        principalTable: "Blobs",
                        principalColumn: "BlobId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ProductTips_Products",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "ProductId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ProductTips_ProductTipCategories",
                        column: x => x.ProductTipCategoryId,
                        principalTable: "ProductTipCategories",
                        principalColumn: "ProductTipCategoryId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ProductTips_BlobId",
                table: "ProductTips",
                column: "BlobId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductTips_ProductId",
                table: "ProductTips",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductTips_ProductTipCategoryId",
                table: "ProductTips",
                column: "ProductTipCategoryId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProductTips");

            migrationBuilder.DropTable(
                name: "ProductTipCategories");
        }
    }
}
