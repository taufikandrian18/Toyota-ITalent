using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Talent.Entities.Migrations
{
    public partial class cmsaddandproductcompetitorfeatures : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                table: "ProductPhotos",
                nullable: false,
                defaultValueSql: "(getdate())",
                oldClrType: typeof(DateTime));

            migrationBuilder.CreateTable(
                name: "Cms_Contents",
                columns: table => new
                {
                    Cms_ContentId = table.Column<Guid>(nullable: false),
                    BlobId = table.Column<Guid>(nullable: true),
                    CmsContentVideoBlobId = table.Column<Guid>(nullable: true),
                    CmsContentFileBlobId = table.Column<Guid>(nullable: true),
                    Cms_ContentName = table.Column<string>(unicode: false, maxLength: 50, nullable: true),
                    Cms_ContentDescription = table.Column<string>(unicode: false, maxLength: 255, nullable: false),
                    Cms_ContentSequence = table.Column<int>(nullable: true),
                    Cms_ContentIsSequence = table.Column<bool>(nullable: true),
                    CreatedAt = table.Column<DateTime>(nullable: false, defaultValueSql: "(getdate())"),
                    CreatedBy = table.Column<string>(unicode: false, maxLength: 64, nullable: false, defaultValueSql: "('SYSTEM')"),
                    UpdatedAt = table.Column<DateTime>(nullable: false, defaultValueSql: "(getdate())"),
                    UpdatedBy = table.Column<string>(unicode: false, maxLength: 64, nullable: false, defaultValueSql: "('SYSTEM')"),
                    IsDeleted = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cms_Contents", x => x.Cms_ContentId);
                    table.ForeignKey(
                        name: "FK_CmsContents_Blobs",
                        column: x => x.BlobId,
                        principalTable: "Blobs",
                        principalColumn: "BlobId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CmsContents_CmsContentFileBlobs",
                        column: x => x.CmsContentFileBlobId,
                        principalTable: "Blobs",
                        principalColumn: "BlobId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CmsContents_CmsContentVideoBlobs",
                        column: x => x.CmsContentVideoBlobId,
                        principalTable: "Blobs",
                        principalColumn: "BlobId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Cms_Fmbs",
                columns: table => new
                {
                    Cms_FmbId = table.Column<Guid>(nullable: false),
                    BlobId = table.Column<Guid>(nullable: true),
                    CmsFmbFileBlobId = table.Column<Guid>(nullable: true),
                    Cms_FmbDescription = table.Column<string>(unicode: false, maxLength: 255, nullable: false),
                    CreatedAt = table.Column<DateTime>(nullable: false, defaultValueSql: "(getdate())"),
                    CreatedBy = table.Column<string>(unicode: false, maxLength: 64, nullable: false, defaultValueSql: "('SYSTEM')"),
                    UpdatedAt = table.Column<DateTime>(nullable: false, defaultValueSql: "(getdate())"),
                    UpdatedBy = table.Column<string>(unicode: false, maxLength: 64, nullable: false, defaultValueSql: "('SYSTEM')"),
                    IsDeleted = table.Column<bool>(nullable: false),
                    CmsFileBlobId = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cms_Fmbs", x => x.Cms_FmbId);
                    table.ForeignKey(
                        name: "FK_CmsFmbs_Blobs",
                        column: x => x.BlobId,
                        principalTable: "Blobs",
                        principalColumn: "BlobId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CmsFmbs_CmsFmbFileBlobs",
                        column: x => x.CmsFmbFileBlobId,
                        principalTable: "Blobs",
                        principalColumn: "BlobId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Cms_Menus",
                columns: table => new
                {
                    Cms_MenuId = table.Column<Guid>(nullable: false),
                    Cms_MenuName = table.Column<string>(unicode: false, maxLength: 50, nullable: false),
                    CreatedAt = table.Column<DateTime>(nullable: false, defaultValueSql: "(getdate())"),
                    CreatedBy = table.Column<string>(unicode: false, maxLength: 64, nullable: false, defaultValueSql: "('SYSTEM')"),
                    UpdatedAt = table.Column<DateTime>(nullable: false, defaultValueSql: "(getdate())"),
                    UpdatedBy = table.Column<string>(unicode: false, maxLength: 64, nullable: false, defaultValueSql: "('SYSTEM')"),
                    IsDeleted = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cms_Menus", x => x.Cms_MenuId);
                });

            migrationBuilder.CreateTable(
                name: "Cms_Operations",
                columns: table => new
                {
                    Cms_OperationId = table.Column<Guid>(nullable: false),
                    BlobId = table.Column<Guid>(nullable: true),
                    Cms_OperationFileBlobId = table.Column<Guid>(nullable: true),
                    Cms_OperationDescription = table.Column<string>(unicode: false, maxLength: 255, nullable: false),
                    CreatedAt = table.Column<DateTime>(nullable: false, defaultValueSql: "(getdate())"),
                    CreatedBy = table.Column<string>(unicode: false, maxLength: 64, nullable: false, defaultValueSql: "('SYSTEM')"),
                    UpdatedAt = table.Column<DateTime>(nullable: false, defaultValueSql: "(getdate())"),
                    UpdatedBy = table.Column<string>(unicode: false, maxLength: 64, nullable: false, defaultValueSql: "('SYSTEM')"),
                    IsDeleted = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cms_Operations", x => x.Cms_OperationId);
                    table.ForeignKey(
                        name: "FK_CmsOperations_Blobs",
                        column: x => x.BlobId,
                        principalTable: "Blobs",
                        principalColumn: "BlobId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CmsOperations_CmsOperationFileBlobs",
                        column: x => x.Cms_OperationFileBlobId,
                        principalTable: "Blobs",
                        principalColumn: "BlobId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Cms_Settings",
                columns: table => new
                {
                    Cms_SettingId = table.Column<Guid>(nullable: false),
                    BlobId = table.Column<Guid>(nullable: true),
                    Cms_SettingFileBlobId = table.Column<Guid>(nullable: true),
                    Cms_SettingDescription = table.Column<string>(unicode: false, maxLength: 255, nullable: false),
                    CreatedAt = table.Column<DateTime>(nullable: false, defaultValueSql: "(getdate())"),
                    CreatedBy = table.Column<string>(unicode: false, maxLength: 64, nullable: false, defaultValueSql: "('SYSTEM')"),
                    UpdatedAt = table.Column<DateTime>(nullable: false, defaultValueSql: "(getdate())"),
                    UpdatedBy = table.Column<string>(unicode: false, maxLength: 64, nullable: false, defaultValueSql: "('SYSTEM')"),
                    IsDeleted = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cms_Settings", x => x.Cms_SettingId);
                    table.ForeignKey(
                        name: "FK_CmsSettings_Blobs",
                        column: x => x.BlobId,
                        principalTable: "Blobs",
                        principalColumn: "BlobId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CmsSettings_CmsSettingFileBlobs",
                        column: x => x.Cms_SettingFileBlobId,
                        principalTable: "Blobs",
                        principalColumn: "BlobId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Cms_SubContents",
                columns: table => new
                {
                    Cms_SubContentId = table.Column<Guid>(nullable: false),
                    BlobId = table.Column<Guid>(nullable: true),
                    CmsSubContentVideoBlobId = table.Column<Guid>(nullable: true),
                    CmsSubContentFileBlobId = table.Column<Guid>(nullable: true),
                    Cms_SubContentName = table.Column<string>(unicode: false, maxLength: 50, nullable: true),
                    Cms_SubContentDescription = table.Column<string>(unicode: false, maxLength: 255, nullable: false),
                    Cms_SubContentSequence = table.Column<int>(nullable: true),
                    Cms_SubContentIsSequence = table.Column<bool>(nullable: true),
                    CreatedAt = table.Column<DateTime>(nullable: false, defaultValueSql: "(getdate())"),
                    CreatedBy = table.Column<string>(unicode: false, maxLength: 64, nullable: false, defaultValueSql: "('SYSTEM')"),
                    UpdatedAt = table.Column<DateTime>(nullable: false, defaultValueSql: "(getdate())"),
                    UpdatedBy = table.Column<string>(unicode: false, maxLength: 64, nullable: false, defaultValueSql: "('SYSTEM')"),
                    IsDeleted = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cms_SubContents", x => x.Cms_SubContentId);
                    table.ForeignKey(
                        name: "FK_CmsSubContents_Blobs",
                        column: x => x.BlobId,
                        principalTable: "Blobs",
                        principalColumn: "BlobId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CmsSubContents_CmsSubContentFileBlobs",
                        column: x => x.CmsSubContentFileBlobId,
                        principalTable: "Blobs",
                        principalColumn: "BlobId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CmsSubContents_CmsSubContentVideoBlobs",
                        column: x => x.CmsSubContentVideoBlobId,
                        principalTable: "Blobs",
                        principalColumn: "BlobId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Cms_WorkPrincipals",
                columns: table => new
                {
                    Cms_WorkPrincipalId = table.Column<Guid>(nullable: false),
                    BlobId = table.Column<Guid>(nullable: true),
                    Cms_WorkPrincipalFileBlobId = table.Column<Guid>(nullable: true),
                    Cms_WorkPrincipalDescription = table.Column<string>(unicode: false, maxLength: 255, nullable: false),
                    CreatedAt = table.Column<DateTime>(nullable: false, defaultValueSql: "(getdate())"),
                    CreatedBy = table.Column<string>(unicode: false, maxLength: 64, nullable: false, defaultValueSql: "('SYSTEM')"),
                    UpdatedAt = table.Column<DateTime>(nullable: false, defaultValueSql: "(getdate())"),
                    UpdatedBy = table.Column<string>(unicode: false, maxLength: 64, nullable: false, defaultValueSql: "('SYSTEM')"),
                    IsDeleted = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cms_WorkPrincipals", x => x.Cms_WorkPrincipalId);
                    table.ForeignKey(
                        name: "FK_CmsWorkPrincipals_Blobs",
                        column: x => x.BlobId,
                        principalTable: "Blobs",
                        principalColumn: "BlobId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CmsWorkPrincipals_CmsWorkPrincipalFileBlobs",
                        column: x => x.Cms_WorkPrincipalFileBlobId,
                        principalTable: "Blobs",
                        principalColumn: "BlobId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ProductCompetitorMappings",
                columns: table => new
                {
                    ProductId = table.Column<Guid>(nullable: false),
                    ProductTypeId = table.Column<Guid>(nullable: false),
                    ProductCompetitorId = table.Column<Guid>(nullable: false),
                    ProductCompetitorTypeId = table.Column<Guid>(nullable: false),
                    CreatedAt = table.Column<DateTime>(nullable: false),
                    CreatedBy = table.Column<string>(maxLength: 64, nullable: false),
                    UpdatedAt = table.Column<DateTime>(nullable: false),
                    UpdatedBy = table.Column<string>(maxLength: 64, nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductCompetitorMappings", x => new { x.ProductId, x.ProductTypeId, x.ProductCompetitorId, x.ProductCompetitorTypeId });
                    table.ForeignKey(
                        name: "FK_ProductCompetitorMappings_ProductCompetitors",
                        column: x => x.ProductCompetitorId,
                        principalTable: "Products",
                        principalColumn: "ProductId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ProductCompetitorMappings_ProductCompetitorTypes",
                        column: x => x.ProductCompetitorTypeId,
                        principalTable: "ProductTypes",
                        principalColumn: "ProductTypeId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ProductCompetitorMappings_Products",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "ProductId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ProductCompetitorMappings_ProductTypes",
                        column: x => x.ProductTypeId,
                        principalTable: "ProductTypes",
                        principalColumn: "ProductTypeId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ProductFeatures",
                columns: table => new
                {
                    ProductFeatureId = table.Column<Guid>(nullable: false),
                    BlobId = table.Column<Guid>(nullable: false),
                    ProductFeatureName = table.Column<string>(unicode: false, maxLength: 50, nullable: false),
                    CreatedAt = table.Column<DateTime>(nullable: false, defaultValueSql: "(getdate())"),
                    CreatedBy = table.Column<string>(unicode: false, maxLength: 64, nullable: false, defaultValueSql: "('SYSTEM')"),
                    UpdatedAt = table.Column<DateTime>(nullable: false, defaultValueSql: "(getdate())"),
                    UpdatedBy = table.Column<string>(unicode: false, maxLength: 64, nullable: false, defaultValueSql: "('SYSTEM')"),
                    IsDeleted = table.Column<bool>(nullable: false, defaultValueSql: "((0))")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductFeatures", x => x.ProductFeatureId);
                    table.ForeignKey(
                        name: "FK_ProductFeatures_Blobs",
                        column: x => x.BlobId,
                        principalTable: "Blobs",
                        principalColumn: "BlobId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ProductFeatureMappings",
                columns: table => new
                {
                    ProductFeatureMappingId = table.Column<Guid>(nullable: false),
                    ProductId = table.Column<Guid>(nullable: false),
                    ProductTypeId = table.Column<Guid>(nullable: false),
                    ProductFeatureId = table.Column<Guid>(nullable: false),
                    Cms_FmbId = table.Column<Guid>(nullable: true),
                    Cms_WorkPrincipalId = table.Column<Guid>(nullable: true),
                    Cms_ContentId = table.Column<Guid>(nullable: true),
                    Cms_OperationId = table.Column<Guid>(nullable: true),
                    Cms_SettingId = table.Column<Guid>(nullable: true),
                    IsSpecial = table.Column<bool>(nullable: true),
                    CreatedAt = table.Column<DateTime>(nullable: false, defaultValueSql: "(getdate())"),
                    CreatedBy = table.Column<string>(unicode: false, maxLength: 64, nullable: false, defaultValueSql: "('SYSTEM')"),
                    UpdatedAt = table.Column<DateTime>(nullable: false, defaultValueSql: "(getdate())"),
                    UpdatedBy = table.Column<string>(unicode: false, maxLength: 64, nullable: false, defaultValueSql: "('SYSTEM')"),
                    IsDeleted = table.Column<bool>(nullable: false, defaultValueSql: "((0))")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductFeatureMappings", x => x.ProductFeatureMappingId);
                    table.ForeignKey(
                        name: "FK_ProductFeatureMappings_CmsContents",
                        column: x => x.Cms_ContentId,
                        principalTable: "Cms_Contents",
                        principalColumn: "Cms_ContentId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ProductFeatureMappings_CmsOperations",
                        column: x => x.Cms_ContentId,
                        principalTable: "Cms_Operations",
                        principalColumn: "Cms_OperationId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ProductFeatureMappings_CmsFmbs",
                        column: x => x.Cms_FmbId,
                        principalTable: "Cms_Fmbs",
                        principalColumn: "Cms_FmbId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ProductFeatureMappings_CmsSettings",
                        column: x => x.Cms_SettingId,
                        principalTable: "Cms_Settings",
                        principalColumn: "Cms_SettingId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ProductFeatureMappings_CmsWorkPrincipals",
                        column: x => x.Cms_WorkPrincipalId,
                        principalTable: "Cms_WorkPrincipals",
                        principalColumn: "Cms_WorkPrincipalId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ProductFeatureMappings_ProductFeatures",
                        column: x => x.ProductFeatureId,
                        principalTable: "ProductFeatures",
                        principalColumn: "ProductFeatureId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ProductFeatureMappings_Products",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "ProductId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ProductFeatureMappings_ProductTypes",
                        column: x => x.ProductTypeId,
                        principalTable: "ProductTypes",
                        principalColumn: "ProductTypeId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Cms_Contents_BlobId",
                table: "Cms_Contents",
                column: "BlobId");

            migrationBuilder.CreateIndex(
                name: "IX_Cms_Contents_CmsContentFileBlobId",
                table: "Cms_Contents",
                column: "CmsContentFileBlobId");

            migrationBuilder.CreateIndex(
                name: "IX_Cms_Contents_CmsContentVideoBlobId",
                table: "Cms_Contents",
                column: "CmsContentVideoBlobId");

            migrationBuilder.CreateIndex(
                name: "IX_Cms_Fmbs_BlobId",
                table: "Cms_Fmbs",
                column: "BlobId");

            migrationBuilder.CreateIndex(
                name: "IX_Cms_Fmbs_CmsFmbFileBlobId",
                table: "Cms_Fmbs",
                column: "CmsFmbFileBlobId");

            migrationBuilder.CreateIndex(
                name: "IX_Cms_Operations_BlobId",
                table: "Cms_Operations",
                column: "BlobId");

            migrationBuilder.CreateIndex(
                name: "IX_Cms_Operations_Cms_OperationFileBlobId",
                table: "Cms_Operations",
                column: "Cms_OperationFileBlobId");

            migrationBuilder.CreateIndex(
                name: "IX_Cms_Settings_BlobId",
                table: "Cms_Settings",
                column: "BlobId");

            migrationBuilder.CreateIndex(
                name: "IX_Cms_Settings_Cms_SettingFileBlobId",
                table: "Cms_Settings",
                column: "Cms_SettingFileBlobId");

            migrationBuilder.CreateIndex(
                name: "IX_Cms_SubContents_BlobId",
                table: "Cms_SubContents",
                column: "BlobId");

            migrationBuilder.CreateIndex(
                name: "IX_Cms_SubContents_CmsSubContentFileBlobId",
                table: "Cms_SubContents",
                column: "CmsSubContentFileBlobId");

            migrationBuilder.CreateIndex(
                name: "IX_Cms_SubContents_CmsSubContentVideoBlobId",
                table: "Cms_SubContents",
                column: "CmsSubContentVideoBlobId");

            migrationBuilder.CreateIndex(
                name: "IX_Cms_WorkPrincipals_BlobId",
                table: "Cms_WorkPrincipals",
                column: "BlobId");

            migrationBuilder.CreateIndex(
                name: "IX_Cms_WorkPrincipals_Cms_WorkPrincipalFileBlobId",
                table: "Cms_WorkPrincipals",
                column: "Cms_WorkPrincipalFileBlobId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductCompetitorMappings_ProductCompetitorId",
                table: "ProductCompetitorMappings",
                column: "ProductCompetitorId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductCompetitorMappings_ProductCompetitorTypeId",
                table: "ProductCompetitorMappings",
                column: "ProductCompetitorTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductCompetitorMappings_ProductTypeId",
                table: "ProductCompetitorMappings",
                column: "ProductTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductFeatureMappings_Cms_ContentId",
                table: "ProductFeatureMappings",
                column: "Cms_ContentId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductFeatureMappings_Cms_FmbId",
                table: "ProductFeatureMappings",
                column: "Cms_FmbId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductFeatureMappings_Cms_SettingId",
                table: "ProductFeatureMappings",
                column: "Cms_SettingId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductFeatureMappings_Cms_WorkPrincipalId",
                table: "ProductFeatureMappings",
                column: "Cms_WorkPrincipalId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductFeatureMappings_ProductFeatureId",
                table: "ProductFeatureMappings",
                column: "ProductFeatureId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductFeatureMappings_ProductId",
                table: "ProductFeatureMappings",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductFeatureMappings_ProductTypeId",
                table: "ProductFeatureMappings",
                column: "ProductTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductFeatures_BlobId",
                table: "ProductFeatures",
                column: "BlobId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Cms_Menus");

            migrationBuilder.DropTable(
                name: "Cms_SubContents");

            migrationBuilder.DropTable(
                name: "ProductCompetitorMappings");

            migrationBuilder.DropTable(
                name: "ProductFeatureMappings");

            migrationBuilder.DropTable(
                name: "Cms_Contents");

            migrationBuilder.DropTable(
                name: "Cms_Operations");

            migrationBuilder.DropTable(
                name: "Cms_Fmbs");

            migrationBuilder.DropTable(
                name: "Cms_Settings");

            migrationBuilder.DropTable(
                name: "Cms_WorkPrincipals");

            migrationBuilder.DropTable(
                name: "ProductFeatures");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                table: "ProductPhotos",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldDefaultValueSql: "(getdate())");
        }
    }
}
