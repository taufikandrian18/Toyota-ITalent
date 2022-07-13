using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Talent.Entities.Migrations
{
    public partial class KeyPieceOfMind_BackOffice : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "KeyPieceOfMindTypes",
                columns: table => new
                {
                    KeyPieceOfMindTypeId = table.Column<Guid>(nullable: false),
                    BlobId = table.Column<Guid>(nullable: true),
                    KeyPieceOfMindTypeName = table.Column<string>(unicode: false, maxLength: 50, nullable: false),
                    KeyPieceOfMindTypeDescription = table.Column<string>(unicode: false, maxLength: 250, nullable: true),
                    CreatedAt = table.Column<DateTime>(nullable: false, defaultValueSql: "(getdate())"),
                    CreatedBy = table.Column<string>(unicode: false, maxLength: 64, nullable: false, defaultValueSql: "('SYSTEM')"),
                    UpdatedAt = table.Column<DateTime>(nullable: false, defaultValueSql: "(getdate())"),
                    UpdatedBy = table.Column<string>(unicode: false, maxLength: 64, nullable: false, defaultValueSql: "('SYSTEM')"),
                    IsDeleted = table.Column<bool>(nullable: false, defaultValueSql: "((0))")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_KeyPieceOfMindTypes", x => x.KeyPieceOfMindTypeId);
                    table.ForeignKey(
                        name: "FK_KeyPieceOfMindTypes_Blobs",
                        column: x => x.BlobId,
                        principalTable: "Blobs",
                        principalColumn: "BlobId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "KeyPieceOfMinds",
                columns: table => new
                {
                    KeyPieceOfMindId = table.Column<Guid>(nullable: false),
                    KeyPieceOfMindTypeId = table.Column<Guid>(nullable: false),
                    Cms_MenuId = table.Column<Guid>(nullable: false),
                    Cms_ContentId = table.Column<Guid>(nullable: false),
                    Cms_SubContentId = table.Column<Guid>(nullable: true),
                    IsSequence = table.Column<int>(nullable: false),
                    CreatedAt = table.Column<DateTime>(nullable: false, defaultValueSql: "(getdate())"),
                    CreatedBy = table.Column<string>(unicode: false, maxLength: 64, nullable: false, defaultValueSql: "('SYSTEM')"),
                    UpdatedAt = table.Column<DateTime>(nullable: false, defaultValueSql: "(getdate())"),
                    UpdatedBy = table.Column<string>(unicode: false, maxLength: 64, nullable: false, defaultValueSql: "('SYSTEM')"),
                    IsDeleted = table.Column<bool>(nullable: false, defaultValueSql: "((0))")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_KeyPieceOfMinds", x => x.KeyPieceOfMindId);
                    table.ForeignKey(
                        name: "FK_KeyPieceOfMinds_CmsContents",
                        column: x => x.Cms_ContentId,
                        principalTable: "Cms_Contents",
                        principalColumn: "Cms_ContentId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_KeyPieceOfMinds_CmsMenus",
                        column: x => x.Cms_MenuId,
                        principalTable: "Cms_Menus",
                        principalColumn: "Cms_MenuId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_KeyPieceOfMinds_Cms_SubContents",
                        column: x => x.Cms_SubContentId,
                        principalTable: "Cms_SubContents",
                        principalColumn: "Cms_SubContentId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_KeyPieceOfMinds_KeyPieceOfMindTypes",
                        column: x => x.KeyPieceOfMindTypeId,
                        principalTable: "KeyPieceOfMindTypes",
                        principalColumn: "KeyPieceOfMindTypeId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_KeyPieceOfMinds_Cms_ContentId",
                table: "KeyPieceOfMinds",
                column: "Cms_ContentId");

            migrationBuilder.CreateIndex(
                name: "IX_KeyPieceOfMinds_Cms_MenuId",
                table: "KeyPieceOfMinds",
                column: "Cms_MenuId");

            migrationBuilder.CreateIndex(
                name: "IX_KeyPieceOfMinds_Cms_SubContentId",
                table: "KeyPieceOfMinds",
                column: "Cms_SubContentId");

            migrationBuilder.CreateIndex(
                name: "IX_KeyPieceOfMinds_KeyPieceOfMindTypeId",
                table: "KeyPieceOfMinds",
                column: "KeyPieceOfMindTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_KeyPieceOfMindTypes_BlobId",
                table: "KeyPieceOfMindTypes",
                column: "BlobId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "KeyPieceOfMinds");

            migrationBuilder.DropTable(
                name: "KeyPieceOfMindTypes");
        }
    }
}
