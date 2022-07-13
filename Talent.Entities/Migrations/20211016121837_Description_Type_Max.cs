using Microsoft.EntityFrameworkCore.Migrations;

namespace Talent.Entities.Migrations
{
    public partial class Description_Type_Max : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "ServiceTipTypeDescription",
                table: "ServiceTipTypes",
                type: "varchar(MAX)",
                unicode: false,
                nullable: true,
                oldClrType: typeof(string),
                oldUnicode: false,
                oldMaxLength: 250,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "ProductTipDescription",
                table: "ProductTips",
                type: "varchar(MAX)",
                unicode: false,
                nullable: false,
                oldClrType: typeof(string),
                oldUnicode: false,
                oldMaxLength: 250);

            migrationBuilder.AlterColumn<string>(
                name: "ProductSPWACategoryDescription",
                table: "ProductSPWACategories",
                type: "varchar(MAX)",
                unicode: false,
                nullable: true,
                oldClrType: typeof(string),
                oldUnicode: false,
                oldMaxLength: 250,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "ProductProgramCategoryDescription",
                table: "ProductProgramCategories",
                type: "varchar(MAX)",
                unicode: false,
                nullable: true,
                oldClrType: typeof(string),
                oldUnicode: false,
                oldMaxLength: 250,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "ProductFaqDescription",
                table: "ProductFAQs",
                type: "varchar(MAX)",
                unicode: false,
                nullable: false,
                oldClrType: typeof(string),
                oldUnicode: false,
                oldMaxLength: 250);

            migrationBuilder.AlterColumn<string>(
                name: "KodawariTypeDescription",
                table: "KodawariTypes",
                type: "varchar(MAX)",
                unicode: false,
                nullable: true,
                oldClrType: typeof(string),
                oldUnicode: false,
                oldMaxLength: 250,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "KeyPieceOfMindTypeDescription",
                table: "KeyPieceOfMindTypes",
                type: "varchar(MAX)",
                unicode: false,
                nullable: true,
                oldClrType: typeof(string),
                oldUnicode: false,
                oldMaxLength: 250,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "HOGuidelineComment",
                table: "HOGuidelines",
                type: "varchar(MAX)",
                unicode: false,
                nullable: true,
                oldClrType: typeof(string),
                oldUnicode: false,
                oldMaxLength: 250,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "DictionaryManfaat",
                table: "Dictionaries",
                type: "varchar(MAX)",
                unicode: false,
                nullable: false,
                oldClrType: typeof(string),
                oldUnicode: false,
                oldMaxLength: 250);

            migrationBuilder.AlterColumn<string>(
                name: "DictionaryFakta",
                table: "Dictionaries",
                type: "varchar(MAX)",
                unicode: false,
                nullable: false,
                oldClrType: typeof(string),
                oldUnicode: false,
                oldMaxLength: 250);

            migrationBuilder.AlterColumn<string>(
                name: "DictionaryBasicOperation",
                table: "Dictionaries",
                type: "varchar(MAX)",
                unicode: false,
                nullable: true,
                oldClrType: typeof(string),
                oldUnicode: false,
                oldMaxLength: 250,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "DictionaryArti",
                table: "Dictionaries",
                type: "varchar(MAX)",
                unicode: false,
                nullable: false,
                oldClrType: typeof(string),
                oldUnicode: false,
                oldMaxLength: 250);

            migrationBuilder.AlterColumn<string>(
                name: "Cms_WorkPrincipalDescription",
                table: "Cms_WorkPrincipals",
                type: "varchar(MAX)",
                unicode: false,
                nullable: false,
                oldClrType: typeof(string),
                oldUnicode: false,
                oldMaxLength: 255);

            migrationBuilder.AlterColumn<string>(
                name: "Cms_SubContentDescription",
                table: "Cms_SubContents",
                type: "varchar(MAX)",
                unicode: false,
                nullable: false,
                oldClrType: typeof(string),
                oldUnicode: false,
                oldMaxLength: 255);

            migrationBuilder.AlterColumn<string>(
                name: "Cms_SettingDescription",
                table: "Cms_Settings",
                type: "varchar(MAX)",
                unicode: false,
                nullable: false,
                oldClrType: typeof(string),
                oldUnicode: false,
                oldMaxLength: 255);

            migrationBuilder.AlterColumn<string>(
                name: "Cms_OperationDescription",
                table: "Cms_Operations",
                type: "varchar(MAX)",
                unicode: false,
                nullable: false,
                oldClrType: typeof(string),
                oldUnicode: false,
                oldMaxLength: 255);

            migrationBuilder.AlterColumn<string>(
                name: "Cms_FmbDescription",
                table: "Cms_Fmbs",
                type: "varchar(MAX)",
                unicode: false,
                nullable: false,
                oldClrType: typeof(string),
                oldUnicode: false,
                oldMaxLength: 255);

            migrationBuilder.AlterColumn<string>(
                name: "Cms_ContentDescription",
                table: "Cms_Contents",
                type: "varchar(MAX)",
                unicode: false,
                nullable: false,
                oldClrType: typeof(string),
                oldUnicode: false,
                oldMaxLength: 255);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "ServiceTipTypeDescription",
                table: "ServiceTipTypes",
                unicode: false,
                maxLength: 250,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(MAX)",
                oldUnicode: false,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "ProductTipDescription",
                table: "ProductTips",
                unicode: false,
                maxLength: 250,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(MAX)",
                oldUnicode: false);

            migrationBuilder.AlterColumn<string>(
                name: "ProductSPWACategoryDescription",
                table: "ProductSPWACategories",
                unicode: false,
                maxLength: 250,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(MAX)",
                oldUnicode: false,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "ProductProgramCategoryDescription",
                table: "ProductProgramCategories",
                unicode: false,
                maxLength: 250,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(MAX)",
                oldUnicode: false,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "ProductFaqDescription",
                table: "ProductFAQs",
                unicode: false,
                maxLength: 250,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(MAX)",
                oldUnicode: false);

            migrationBuilder.AlterColumn<string>(
                name: "KodawariTypeDescription",
                table: "KodawariTypes",
                unicode: false,
                maxLength: 250,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(MAX)",
                oldUnicode: false,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "KeyPieceOfMindTypeDescription",
                table: "KeyPieceOfMindTypes",
                unicode: false,
                maxLength: 250,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(MAX)",
                oldUnicode: false,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "HOGuidelineComment",
                table: "HOGuidelines",
                unicode: false,
                maxLength: 250,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(MAX)",
                oldUnicode: false,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "DictionaryManfaat",
                table: "Dictionaries",
                unicode: false,
                maxLength: 250,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(MAX)",
                oldUnicode: false);

            migrationBuilder.AlterColumn<string>(
                name: "DictionaryFakta",
                table: "Dictionaries",
                unicode: false,
                maxLength: 250,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(MAX)",
                oldUnicode: false);

            migrationBuilder.AlterColumn<string>(
                name: "DictionaryBasicOperation",
                table: "Dictionaries",
                unicode: false,
                maxLength: 250,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(MAX)",
                oldUnicode: false,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "DictionaryArti",
                table: "Dictionaries",
                unicode: false,
                maxLength: 250,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(MAX)",
                oldUnicode: false);

            migrationBuilder.AlterColumn<string>(
                name: "Cms_WorkPrincipalDescription",
                table: "Cms_WorkPrincipals",
                unicode: false,
                maxLength: 255,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(MAX)",
                oldUnicode: false);

            migrationBuilder.AlterColumn<string>(
                name: "Cms_SubContentDescription",
                table: "Cms_SubContents",
                unicode: false,
                maxLength: 255,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(MAX)",
                oldUnicode: false);

            migrationBuilder.AlterColumn<string>(
                name: "Cms_SettingDescription",
                table: "Cms_Settings",
                unicode: false,
                maxLength: 255,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(MAX)",
                oldUnicode: false);

            migrationBuilder.AlterColumn<string>(
                name: "Cms_OperationDescription",
                table: "Cms_Operations",
                unicode: false,
                maxLength: 255,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(MAX)",
                oldUnicode: false);

            migrationBuilder.AlterColumn<string>(
                name: "Cms_FmbDescription",
                table: "Cms_Fmbs",
                unicode: false,
                maxLength: 255,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(MAX)",
                oldUnicode: false);

            migrationBuilder.AlterColumn<string>(
                name: "Cms_ContentDescription",
                table: "Cms_Contents",
                unicode: false,
                maxLength: 255,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(MAX)",
                oldUnicode: false);
        }
    }
}
