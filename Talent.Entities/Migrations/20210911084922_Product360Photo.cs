using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Talent.Entities.Migrations
{
    public partial class Product360Photo : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ProductPhotos",
                columns: table => new
                {
                    ProductPhotoId = table.Column<Guid>(nullable: false),
                    BlobId = table.Column<Guid>(nullable: false),
                    CreatedAt = table.Column<DateTime>(nullable: false),
                    CreatedBy = table.Column<string>(unicode: false, maxLength: 64, nullable: false, defaultValueSql: "('SYSTEM')"),
                    UpdatedAt = table.Column<DateTime>(nullable: false, defaultValueSql: "(getdate())"),
                    UpdatedBy = table.Column<string>(unicode: false, maxLength: 64, nullable: false, defaultValueSql: "('SYSTEM')"),
                    IsDeleted = table.Column<bool>(nullable: false, defaultValueSql: "((0))")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductPhotos", x => x.ProductPhotoId);
                    table.ForeignKey(
                        name: "FK_ProductPhotos_Blobs",
                        column: x => x.BlobId,
                        principalTable: "Blobs",
                        principalColumn: "BlobId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ProductPhoto360Mappings",
                columns: table => new
                {
                    ProductId = table.Column<Guid>(nullable: false),
                    ProductPhotoId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductPhoto360Mappings", x => new { x.ProductId, x.ProductPhotoId });
                    table.ForeignKey(
                        name: "FK_ProductPhoto360Mappings_Products",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "ProductId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ProductPhoto360Mappings_ProductPhotos",
                        column: x => x.ProductPhotoId,
                        principalTable: "ProductPhotos",
                        principalColumn: "ProductPhotoId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ProductPhoto360Mappings_ProductPhotoId",
                table: "ProductPhoto360Mappings",
                column: "ProductPhotoId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductPhotos_BlobId",
                table: "ProductPhotos",
                column: "BlobId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProductPhoto360Mappings");

            migrationBuilder.DropTable(
                name: "ProductPhotos");
        }
    }
}
