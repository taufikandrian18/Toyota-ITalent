using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Talent.Entities.Migrations
{
    public partial class addProductTypes : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ProductGalleryType",
                table: "ProductGalleries");

            migrationBuilder.AlterColumn<string>(
                name: "ProductGalleryColorCode",
                table: "ProductGalleries",
                unicode: false,
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldUnicode: false,
                oldNullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "ProductTypeId",
                table: "ProductGalleries",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateTable(
                name: "ProductTypes",
                columns: table => new
                {
                    ProductTypeId = table.Column<Guid>(nullable: false),
                    BlobId = table.Column<Guid>(nullable: false),
                    ProductId = table.Column<Guid>(nullable: false),
                    ProductTypeName = table.Column<string>(unicode: false, maxLength: 50, nullable: false),
                    ProductTypeSalesTalk = table.Column<string>(unicode: false, maxLength: 250, nullable: true),
                    CreatedAt = table.Column<DateTime>(nullable: false, defaultValueSql: "(getdate())"),
                    CreatedBy = table.Column<string>(unicode: false, maxLength: 64, nullable: false, defaultValueSql: "('SYSTEM')"),
                    UpdatedAt = table.Column<DateTime>(nullable: false, defaultValueSql: "(getdate())"),
                    UpdatedBy = table.Column<string>(unicode: false, maxLength: 64, nullable: false, defaultValueSql: "('SYSTEM')"),
                    IsDeleted = table.Column<bool>(nullable: false, defaultValueSql: "((0))")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductTypes", x => x.ProductTypeId);
                    table.ForeignKey(
                        name: "FK_ProductTypes_Blobs",
                        column: x => x.BlobId,
                        principalTable: "Blobs",
                        principalColumn: "BlobId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProductTypes_Products",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "ProductId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ProductGalleries_ProductTypeId",
                table: "ProductGalleries",
                column: "ProductTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductTypes_BlobId",
                table: "ProductTypes",
                column: "BlobId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductTypes_ProductId",
                table: "ProductTypes",
                column: "ProductId");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductGalleries_ProductTypes",
                table: "ProductGalleries",
                column: "ProductTypeId",
                principalTable: "ProductTypes",
                principalColumn: "ProductTypeId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductGalleries_ProductTypes",
                table: "ProductGalleries");

            migrationBuilder.DropTable(
                name: "ProductTypes");

            migrationBuilder.DropIndex(
                name: "IX_ProductGalleries_ProductTypeId",
                table: "ProductGalleries");

            migrationBuilder.DropColumn(
                name: "ProductTypeId",
                table: "ProductGalleries");

            migrationBuilder.AlterColumn<string>(
                name: "ProductGalleryColorCode",
                table: "ProductGalleries",
                unicode: false,
                nullable: true,
                oldClrType: typeof(string),
                oldUnicode: false,
                oldMaxLength: 50);

            migrationBuilder.AddColumn<string>(
                name: "ProductGalleryType",
                table: "ProductGalleries",
                unicode: false,
                maxLength: 50,
                nullable: false,
                defaultValue: "");
        }
    }
}
