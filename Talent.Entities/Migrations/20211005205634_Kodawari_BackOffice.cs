using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Talent.Entities.Migrations
{
    public partial class Kodawari_BackOffice : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "KodawariBanners",
                columns: table => new
                {
                    KodawariBannerId = table.Column<Guid>(nullable: false),
                    BlobId = table.Column<Guid>(nullable: false),
                    KodawariBannerTitle = table.Column<string>(unicode: false, maxLength: 50, nullable: false),
                    CreatedAt = table.Column<DateTime>(nullable: false, defaultValueSql: "(getdate())"),
                    CreatedBy = table.Column<string>(unicode: false, maxLength: 64, nullable: false, defaultValueSql: "('SYSTEM')"),
                    UpdatedAt = table.Column<DateTime>(nullable: false, defaultValueSql: "(getdate())"),
                    UpdatedBy = table.Column<string>(unicode: false, maxLength: 64, nullable: false, defaultValueSql: "('SYSTEM')"),
                    IsDeleted = table.Column<bool>(nullable: false, defaultValueSql: "((0))")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_KodawariBanners", x => x.KodawariBannerId);
                    table.ForeignKey(
                        name: "FK_KodawariBanners_Blobs",
                        column: x => x.BlobId,
                        principalTable: "Blobs",
                        principalColumn: "BlobId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "KodawariDownloads",
                columns: table => new
                {
                    KodawariDownloadId = table.Column<Guid>(nullable: false),
                    Cms_MenuId = table.Column<Guid>(nullable: false),
                    BlobId = table.Column<Guid>(nullable: false),
                    KodawariDownloadTitle = table.Column<string>(unicode: false, maxLength: 50, nullable: false),
                    IsDownloadable = table.Column<bool>(nullable: false),
                    CreatedAt = table.Column<DateTime>(nullable: false, defaultValueSql: "(getdate())"),
                    CreatedBy = table.Column<string>(unicode: false, maxLength: 64, nullable: false, defaultValueSql: "('SYSTEM')"),
                    UpdatedAt = table.Column<DateTime>(nullable: false, defaultValueSql: "(getdate())"),
                    UpdatedBy = table.Column<string>(unicode: false, maxLength: 64, nullable: false, defaultValueSql: "('SYSTEM')"),
                    IsDeleted = table.Column<bool>(nullable: false, defaultValueSql: "((0))")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_KodawariDownloads", x => x.KodawariDownloadId);
                    table.ForeignKey(
                        name: "FK_KodawariDownloads_Blobs",
                        column: x => x.BlobId,
                        principalTable: "Blobs",
                        principalColumn: "BlobId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_KodawariDownloads_CmsMenus",
                        column: x => x.Cms_MenuId,
                        principalTable: "Cms_Menus",
                        principalColumn: "Cms_MenuId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "KodawariMenus",
                columns: table => new
                {
                    KodawariMenuId = table.Column<Guid>(nullable: false),
                    KodawariMenuName = table.Column<string>(unicode: false, nullable: true),
                    KodawariMenuSequence = table.Column<int>(nullable: false),
                    CreatedAt = table.Column<DateTime>(nullable: false, defaultValueSql: "(getdate())"),
                    CreatedBy = table.Column<string>(unicode: false, maxLength: 64, nullable: false, defaultValueSql: "('SYSTEM')"),
                    UpdatedAt = table.Column<DateTime>(nullable: false, defaultValueSql: "(getdate())"),
                    UpdatedBy = table.Column<string>(unicode: false, maxLength: 64, nullable: false, defaultValueSql: "('SYSTEM')"),
                    IsDeleted = table.Column<bool>(nullable: false, defaultValueSql: "((0))")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_KodawariMenus", x => x.KodawariMenuId);
                });

            migrationBuilder.CreateTable(
                name: "KodawariTypes",
                columns: table => new
                {
                    KodawariTypeId = table.Column<Guid>(nullable: false),
                    BlobId = table.Column<Guid>(nullable: true),
                    KodawariTypeName = table.Column<string>(unicode: false, maxLength: 50, nullable: false),
                    KodawariTypeDescription = table.Column<string>(unicode: false, maxLength: 250, nullable: true),
                    CreatedAt = table.Column<DateTime>(nullable: false, defaultValueSql: "(getdate())"),
                    CreatedBy = table.Column<string>(unicode: false, maxLength: 64, nullable: false, defaultValueSql: "('SYSTEM')"),
                    UpdatedAt = table.Column<DateTime>(nullable: false, defaultValueSql: "(getdate())"),
                    UpdatedBy = table.Column<string>(unicode: false, maxLength: 64, nullable: false, defaultValueSql: "('SYSTEM')"),
                    IsDeleted = table.Column<bool>(nullable: false, defaultValueSql: "((0))")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_KodawariTypes", x => x.KodawariTypeId);
                    table.ForeignKey(
                        name: "FK_KodawariTypes_Blobs",
                        column: x => x.BlobId,
                        principalTable: "Blobs",
                        principalColumn: "BlobId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Kodawaris",
                columns: table => new
                {
                    KodawariId = table.Column<Guid>(nullable: false),
                    KodawariMenuId = table.Column<Guid>(nullable: false),
                    Cms_MenuId = table.Column<Guid>(nullable: false),
                    KodawariTypeId = table.Column<Guid>(nullable: false),
                    Cms_ContentId = table.Column<Guid>(nullable: false),
                    Cms_SubContentId = table.Column<Guid>(nullable: true),
                    KodawariSequence = table.Column<int>(nullable: false),
                    CreatedAt = table.Column<DateTime>(nullable: false, defaultValueSql: "(getdate())"),
                    CreatedBy = table.Column<string>(unicode: false, maxLength: 64, nullable: false, defaultValueSql: "('SYSTEM')"),
                    UpdatedAt = table.Column<DateTime>(nullable: false, defaultValueSql: "(getdate())"),
                    UpdatedBy = table.Column<string>(unicode: false, maxLength: 64, nullable: false, defaultValueSql: "('SYSTEM')"),
                    IsDeleted = table.Column<bool>(nullable: false, defaultValueSql: "((0))")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Kodawaris", x => x.KodawariId);
                    table.ForeignKey(
                        name: "FK_Kodawaris_CmsContents",
                        column: x => x.Cms_ContentId,
                        principalTable: "Cms_Contents",
                        principalColumn: "Cms_ContentId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Kodawaris_CmsMenus",
                        column: x => x.Cms_MenuId,
                        principalTable: "Cms_Menus",
                        principalColumn: "Cms_MenuId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Kodawaris_Cms_SubContents",
                        column: x => x.Cms_SubContentId,
                        principalTable: "Cms_SubContents",
                        principalColumn: "Cms_SubContentId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Kodawaris_KodawariMenus",
                        column: x => x.KodawariMenuId,
                        principalTable: "KodawariMenus",
                        principalColumn: "KodawariMenuId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Kodawaris_KodawariTypes",
                        column: x => x.KodawariTypeId,
                        principalTable: "KodawariTypes",
                        principalColumn: "KodawariTypeId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_KodawariBanners_BlobId",
                table: "KodawariBanners",
                column: "BlobId");

            migrationBuilder.CreateIndex(
                name: "IX_KodawariDownloads_BlobId",
                table: "KodawariDownloads",
                column: "BlobId");

            migrationBuilder.CreateIndex(
                name: "IX_KodawariDownloads_Cms_MenuId",
                table: "KodawariDownloads",
                column: "Cms_MenuId");

            migrationBuilder.CreateIndex(
                name: "IX_Kodawaris_Cms_ContentId",
                table: "Kodawaris",
                column: "Cms_ContentId");

            migrationBuilder.CreateIndex(
                name: "IX_Kodawaris_Cms_MenuId",
                table: "Kodawaris",
                column: "Cms_MenuId");

            migrationBuilder.CreateIndex(
                name: "IX_Kodawaris_Cms_SubContentId",
                table: "Kodawaris",
                column: "Cms_SubContentId");

            migrationBuilder.CreateIndex(
                name: "IX_Kodawaris_KodawariMenuId",
                table: "Kodawaris",
                column: "KodawariMenuId");

            migrationBuilder.CreateIndex(
                name: "IX_Kodawaris_KodawariTypeId",
                table: "Kodawaris",
                column: "KodawariTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_KodawariTypes_BlobId",
                table: "KodawariTypes",
                column: "BlobId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "KodawariBanners");

            migrationBuilder.DropTable(
                name: "KodawariDownloads");

            migrationBuilder.DropTable(
                name: "Kodawaris");

            migrationBuilder.DropTable(
                name: "KodawariMenus");

            migrationBuilder.DropTable(
                name: "KodawariTypes");
        }
    }
}
