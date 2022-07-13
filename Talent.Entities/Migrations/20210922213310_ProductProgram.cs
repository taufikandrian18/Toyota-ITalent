using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Talent.Entities.Migrations
{
    public partial class ProductProgram : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ProductProgramCategories",
                columns: table => new
                {
                    ProductProgramCategoryId = table.Column<Guid>(nullable: false),
                    BlobId = table.Column<Guid>(nullable: true),
                    ProductProgramCategoryName = table.Column<string>(unicode: false, maxLength: 50, nullable: false),
                    ProductProgramCategoryDescription = table.Column<string>(unicode: false, maxLength: 250, nullable: true),
                    CreatedAt = table.Column<DateTime>(nullable: false, defaultValueSql: "(getdate())"),
                    CreatedBy = table.Column<string>(unicode: false, maxLength: 64, nullable: false, defaultValueSql: "('SYSTEM')"),
                    UpdatedAt = table.Column<DateTime>(nullable: false, defaultValueSql: "(getdate())"),
                    UpdatedBy = table.Column<string>(unicode: false, maxLength: 64, nullable: false, defaultValueSql: "('SYSTEM')"),
                    IsDeleted = table.Column<bool>(nullable: false, defaultValueSql: "((0))")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductProgramCategories", x => x.ProductProgramCategoryId);
                    table.ForeignKey(
                        name: "FK_ProductProgramCategories_Blobs",
                        column: x => x.BlobId,
                        principalTable: "Blobs",
                        principalColumn: "BlobId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ProductProgramMappings",
                columns: table => new
                {
                    ProductProgramMappingId = table.Column<Guid>(nullable: false),
                    ProductProgramCategoryId = table.Column<Guid>(nullable: false),
                    ProductId = table.Column<Guid>(nullable: false),
                    Cms_MenuId = table.Column<Guid>(nullable: false),
                    Cms_ContentId = table.Column<Guid>(nullable: false),
                    IsSequence = table.Column<int>(nullable: false),
                    CreatedAt = table.Column<DateTime>(nullable: false, defaultValueSql: "(getdate())"),
                    CreatedBy = table.Column<string>(unicode: false, maxLength: 64, nullable: false, defaultValueSql: "('SYSTEM')"),
                    UpdatedAt = table.Column<DateTime>(nullable: false, defaultValueSql: "(getdate())"),
                    UpdatedBy = table.Column<string>(unicode: false, maxLength: 64, nullable: false, defaultValueSql: "('SYSTEM')"),
                    IsDeleted = table.Column<bool>(nullable: false, defaultValueSql: "((0))")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductProgramMappings", x => x.ProductProgramMappingId);
                    table.ForeignKey(
                        name: "FK_ProductProgramMappings_CmsContents",
                        column: x => x.Cms_ContentId,
                        principalTable: "Cms_Contents",
                        principalColumn: "Cms_ContentId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ProductProgramMappings_CmsMenus",
                        column: x => x.Cms_MenuId,
                        principalTable: "Cms_Menus",
                        principalColumn: "Cms_MenuId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ProductProgramMappings_Products",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "ProductId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ProductProgramMappings_ProductProgramCategories",
                        column: x => x.ProductProgramCategoryId,
                        principalTable: "ProductProgramCategories",
                        principalColumn: "ProductProgramCategoryId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ProductProgramCategories_BlobId",
                table: "ProductProgramCategories",
                column: "BlobId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductProgramMappings_Cms_ContentId",
                table: "ProductProgramMappings",
                column: "Cms_ContentId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductProgramMappings_Cms_MenuId",
                table: "ProductProgramMappings",
                column: "Cms_MenuId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductProgramMappings_ProductId",
                table: "ProductProgramMappings",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductProgramMappings_ProductProgramCategoryId",
                table: "ProductProgramMappings",
                column: "ProductProgramCategoryId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProductProgramMappings");

            migrationBuilder.DropTable(
                name: "ProductProgramCategories");
        }
    }
}
