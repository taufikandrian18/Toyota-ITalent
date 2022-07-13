using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Talent.Entities.Migrations
{
    public partial class ProductGalleries : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "Products",
                nullable: false,
                defaultValueSql: "((0))",
                oldClrType: typeof(bool));

            migrationBuilder.CreateTable(
                name: "ProductGalleries",
                columns: table => new
                {
                    ProductGalleryId = table.Column<Guid>(nullable: false),
                    BlobId = table.Column<Guid>(nullable: false),
                    ProductId = table.Column<Guid>(nullable: false),
                    ProductGalleryType = table.Column<string>(unicode: false, maxLength: 50, nullable: false),
                    ProductGalleryColorCode = table.Column<string>(unicode: false, nullable: true),
                    ProductGalleryColorName = table.Column<string>(unicode: false, nullable: true),
                    ProductGalleryStatus = table.Column<bool>(nullable: false),
                    IsApproved = table.Column<string>(unicode: false, nullable: true),
                    CreatedAt = table.Column<DateTime>(nullable: false, defaultValueSql: "(getdate())"),
                    CreatedBy = table.Column<string>(unicode: false, maxLength: 64, nullable: false, defaultValueSql: "('SYSTEM')"),
                    UpdatedAt = table.Column<DateTime>(nullable: false, defaultValueSql: "(getdate())"),
                    UpdatedBy = table.Column<string>(unicode: false, maxLength: 64, nullable: false, defaultValueSql: "('SYSTEM')"),
                    IsDeleted = table.Column<bool>(nullable: false, defaultValueSql: "((0))")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductGalleries", x => x.ProductGalleryId);
                    table.ForeignKey(
                        name: "FK_ProductGalleries_Blobs",
                        column: x => x.BlobId,
                        principalTable: "Blobs",
                        principalColumn: "BlobId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProductGalleries_Products",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "ProductId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ProductGalleries_BlobId",
                table: "ProductGalleries",
                column: "BlobId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductGalleries_ProductId",
                table: "ProductGalleries",
                column: "ProductId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProductGalleries");

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "Products",
                nullable: false,
                oldClrType: typeof(bool),
                oldDefaultValueSql: "((0))");
        }
    }
}
