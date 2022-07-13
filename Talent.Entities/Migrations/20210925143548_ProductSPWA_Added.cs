using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Talent.Entities.Migrations
{
    public partial class ProductSPWA_Added : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ProductSPWACategories",
                columns: table => new
                {
                    ProductSPWACategoryId = table.Column<Guid>(nullable: false),
                    BlobId = table.Column<Guid>(nullable: true),
                    ProductSPWACategoryName = table.Column<string>(unicode: false, maxLength: 50, nullable: false),
                    ProductSPWACategoryDescription = table.Column<string>(unicode: false, maxLength: 250, nullable: true),
                    CreatedAt = table.Column<DateTime>(nullable: false, defaultValueSql: "(getdate())"),
                    CreatedBy = table.Column<string>(unicode: false, maxLength: 64, nullable: false, defaultValueSql: "('SYSTEM')"),
                    UpdatedAt = table.Column<DateTime>(nullable: false, defaultValueSql: "(getdate())"),
                    UpdatedBy = table.Column<string>(unicode: false, maxLength: 64, nullable: false, defaultValueSql: "('SYSTEM')"),
                    IsDeleted = table.Column<bool>(nullable: false, defaultValueSql: "((0))")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductSPWACategories", x => x.ProductSPWACategoryId);
                    table.ForeignKey(
                        name: "FK_ProductSPWACategories_Blobs",
                        column: x => x.BlobId,
                        principalTable: "Blobs",
                        principalColumn: "BlobId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ProductSPWAMappings",
                columns: table => new
                {
                    ProductSPWAMappingId = table.Column<Guid>(nullable: false),
                    ProductSPWACategoryId = table.Column<Guid>(nullable: false),
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
                    table.PrimaryKey("PK_ProductSPWAMappings", x => x.ProductSPWAMappingId);
                    table.ForeignKey(
                        name: "FK_ProductSPWAMappings_CmsContents",
                        column: x => x.Cms_ContentId,
                        principalTable: "Cms_Contents",
                        principalColumn: "Cms_ContentId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ProductSPWAMappings_CmsMenus",
                        column: x => x.Cms_MenuId,
                        principalTable: "Cms_Menus",
                        principalColumn: "Cms_MenuId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ProductSPWAMappings_Products",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "ProductId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ProductSPWAMappings_ProductSPWACategories",
                        column: x => x.ProductSPWACategoryId,
                        principalTable: "ProductSPWACategories",
                        principalColumn: "ProductSPWACategoryId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ProductSPWACategories_BlobId",
                table: "ProductSPWACategories",
                column: "BlobId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductSPWAMappings_Cms_ContentId",
                table: "ProductSPWAMappings",
                column: "Cms_ContentId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductSPWAMappings_Cms_MenuId",
                table: "ProductSPWAMappings",
                column: "Cms_MenuId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductSPWAMappings_ProductId",
                table: "ProductSPWAMappings",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductSPWAMappings_ProductSPWACategoryId",
                table: "ProductSPWAMappings",
                column: "ProductSPWACategoryId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProductSPWAMappings");

            migrationBuilder.DropTable(
                name: "ProductSPWACategories");
        }
    }
}
