using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Talent.Entities.Migrations
{
    public partial class ProductFAQs : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ProductFAQCategories",
                columns: table => new
                {
                    ProductFaqCategoryId = table.Column<Guid>(nullable: false),
                    ProductFaqCategoryName = table.Column<string>(unicode: false, maxLength: 50, nullable: false),
                    CreatedAt = table.Column<DateTime>(nullable: false, defaultValueSql: "(getdate())"),
                    CreatedBy = table.Column<string>(unicode: false, maxLength: 64, nullable: false, defaultValueSql: "('SYSTEM')"),
                    UpdatedAt = table.Column<DateTime>(nullable: false, defaultValueSql: "(getdate())"),
                    UpdatedBy = table.Column<string>(unicode: false, maxLength: 64, nullable: false, defaultValueSql: "('SYSTEM')"),
                    IsDeleted = table.Column<bool>(nullable: false, defaultValueSql: "((0))")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductFAQCategories", x => x.ProductFaqCategoryId);
                });

            migrationBuilder.CreateTable(
                name: "ProductFAQs",
                columns: table => new
                {
                    ProductFaqId = table.Column<Guid>(nullable: false),
                    ProductFaqCategoryId = table.Column<Guid>(nullable: false),
                    ProductId = table.Column<Guid>(nullable: false),
                    BlobId = table.Column<Guid>(nullable: false),
                    ProductFaqSequence = table.Column<int>(nullable: false),
                    ProductFaqTitle = table.Column<string>(unicode: false, maxLength: 50, nullable: false),
                    ProductFaqDescription = table.Column<string>(unicode: false, maxLength: 250, nullable: false),
                    CreatedAt = table.Column<DateTime>(nullable: false, defaultValueSql: "(getdate())"),
                    CreatedBy = table.Column<string>(unicode: false, maxLength: 64, nullable: false, defaultValueSql: "('SYSTEM')"),
                    UpdatedAt = table.Column<DateTime>(nullable: false, defaultValueSql: "(getdate())"),
                    UpdatedBy = table.Column<string>(unicode: false, maxLength: 64, nullable: false, defaultValueSql: "('SYSTEM')"),
                    IsDeleted = table.Column<bool>(nullable: false, defaultValueSql: "((0))")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductFAQs", x => x.ProductFaqId);
                    table.ForeignKey(
                        name: "FK_ProductFAQs_Blobs",
                        column: x => x.BlobId,
                        principalTable: "Blobs",
                        principalColumn: "BlobId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ProductFAQs_ProductFAQCategories",
                        column: x => x.ProductFaqCategoryId,
                        principalTable: "ProductFAQCategories",
                        principalColumn: "ProductFaqCategoryId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ProductFAQs_Products",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "ProductId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ProductFAQUsers",
                columns: table => new
                {
                    ProductFAQUserId = table.Column<Guid>(nullable: false),
                    ProductId = table.Column<Guid>(nullable: false),
                    ProductFaqCategoryId = table.Column<Guid>(nullable: false),
                    ProductFAQUserQuestion = table.Column<string>(unicode: false, maxLength: 250, nullable: false),
                    CreatedAt = table.Column<DateTime>(nullable: false, defaultValueSql: "(getdate())"),
                    CreatedBy = table.Column<string>(unicode: false, maxLength: 64, nullable: false, defaultValueSql: "('SYSTEM')"),
                    UpdatedAt = table.Column<DateTime>(nullable: false, defaultValueSql: "(getdate())"),
                    UpdatedBy = table.Column<string>(unicode: false, maxLength: 64, nullable: false, defaultValueSql: "('SYSTEM')"),
                    IsDeleted = table.Column<bool>(nullable: false, defaultValueSql: "((0))")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductFAQUsers", x => x.ProductFAQUserId);
                    table.ForeignKey(
                        name: "FK_ProductFAQUsers_ProductFAQCategories",
                        column: x => x.ProductFaqCategoryId,
                        principalTable: "ProductFAQCategories",
                        principalColumn: "ProductFaqCategoryId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ProductFAQUsers_Products",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "ProductId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ProductFAQs_BlobId",
                table: "ProductFAQs",
                column: "BlobId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductFAQs_ProductFaqCategoryId",
                table: "ProductFAQs",
                column: "ProductFaqCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductFAQs_ProductId",
                table: "ProductFAQs",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductFAQUsers_ProductFaqCategoryId",
                table: "ProductFAQUsers",
                column: "ProductFaqCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductFAQUsers_ProductId",
                table: "ProductFAQUsers",
                column: "ProductId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProductFAQs");

            migrationBuilder.DropTable(
                name: "ProductFAQUsers");

            migrationBuilder.DropTable(
                name: "ProductFAQCategories");
        }
    }
}
