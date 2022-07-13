using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Talent.Entities.Migrations
{
    public partial class ServiceTip : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ServiceTipMenus",
                columns: table => new
                {
                    ServiceTipMenuId = table.Column<Guid>(nullable: false),
                    ServiceTipMenuName = table.Column<string>(unicode: false, nullable: true),
                    ServiceTipMenuSequence = table.Column<int>(nullable: false),
                    CreatedAt = table.Column<DateTime>(nullable: false, defaultValueSql: "(getdate())"),
                    CreatedBy = table.Column<string>(unicode: false, maxLength: 64, nullable: false, defaultValueSql: "('SYSTEM')"),
                    UpdatedAt = table.Column<DateTime>(nullable: false, defaultValueSql: "(getdate())"),
                    UpdatedBy = table.Column<string>(unicode: false, maxLength: 64, nullable: false, defaultValueSql: "('SYSTEM')"),
                    IsDeleted = table.Column<bool>(nullable: false, defaultValueSql: "((0))")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ServiceTipMenus", x => x.ServiceTipMenuId);
                });

            migrationBuilder.CreateTable(
                name: "ServiceTipTypes",
                columns: table => new
                {
                    ServiceTipTypeId = table.Column<Guid>(nullable: false),
                    BlobId = table.Column<Guid>(nullable: true),
                    ServiceTipTypeName = table.Column<string>(unicode: false, maxLength: 50, nullable: false),
                    ServiceTipTypeDescription = table.Column<string>(unicode: false, maxLength: 250, nullable: true),
                    CreatedAt = table.Column<DateTime>(nullable: false, defaultValueSql: "(getdate())"),
                    CreatedBy = table.Column<string>(unicode: false, maxLength: 64, nullable: false, defaultValueSql: "('SYSTEM')"),
                    UpdatedAt = table.Column<DateTime>(nullable: false, defaultValueSql: "(getdate())"),
                    UpdatedBy = table.Column<string>(unicode: false, maxLength: 64, nullable: false, defaultValueSql: "('SYSTEM')"),
                    IsDeleted = table.Column<bool>(nullable: false, defaultValueSql: "((0))")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ServiceTipTypes", x => x.ServiceTipTypeId);
                    table.ForeignKey(
                        name: "FK_ServiceTipTypes_Blobs",
                        column: x => x.BlobId,
                        principalTable: "Blobs",
                        principalColumn: "BlobId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ServiceTips",
                columns: table => new
                {
                    ServiceTipId = table.Column<Guid>(nullable: false),
                    ServiceTipTypeId = table.Column<Guid>(nullable: false),
                    ServiceTipMenuId = table.Column<Guid>(nullable: false),
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
                    table.PrimaryKey("PK_ServiceTips", x => x.ServiceTipId);
                    table.ForeignKey(
                        name: "FK_ServiceTips_CmsContents",
                        column: x => x.Cms_ContentId,
                        principalTable: "Cms_Contents",
                        principalColumn: "Cms_ContentId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ServiceTips_CmsMenus",
                        column: x => x.Cms_MenuId,
                        principalTable: "Cms_Menus",
                        principalColumn: "Cms_MenuId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ServiceTips_Cms_SubContents",
                        column: x => x.Cms_SubContentId,
                        principalTable: "Cms_SubContents",
                        principalColumn: "Cms_SubContentId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ServiceTips_ServiceTipMenus_ServiceTipMenuId",
                        column: x => x.ServiceTipMenuId,
                        principalTable: "ServiceTipMenus",
                        principalColumn: "ServiceTipMenuId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ServiceTips_ServiceTipTypes",
                        column: x => x.ServiceTipTypeId,
                        principalTable: "ServiceTipTypes",
                        principalColumn: "ServiceTipTypeId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ServiceTips_Cms_ContentId",
                table: "ServiceTips",
                column: "Cms_ContentId");

            migrationBuilder.CreateIndex(
                name: "IX_ServiceTips_Cms_MenuId",
                table: "ServiceTips",
                column: "Cms_MenuId");

            migrationBuilder.CreateIndex(
                name: "IX_ServiceTips_Cms_SubContentId",
                table: "ServiceTips",
                column: "Cms_SubContentId");

            migrationBuilder.CreateIndex(
                name: "IX_ServiceTips_ServiceTipMenuId",
                table: "ServiceTips",
                column: "ServiceTipMenuId");

            migrationBuilder.CreateIndex(
                name: "IX_ServiceTips_ServiceTipTypeId",
                table: "ServiceTips",
                column: "ServiceTipTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_ServiceTipTypes_BlobId",
                table: "ServiceTipTypes",
                column: "BlobId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ServiceTips");

            migrationBuilder.DropTable(
                name: "ServiceTipMenus");

            migrationBuilder.DropTable(
                name: "ServiceTipTypes");

            migrationBuilder.DropColumn(
                name: "IsDataValidation",
                table: "Employees");
        }
    }
}
