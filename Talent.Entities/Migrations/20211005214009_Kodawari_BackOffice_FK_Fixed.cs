using Microsoft.EntityFrameworkCore.Migrations;

namespace Talent.Entities.Migrations
{
    public partial class Kodawari_BackOffice_FK_Fixed : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_KodawariDownloads_CmsMenus",
                table: "KodawariDownloads");

            migrationBuilder.RenameColumn(
                name: "Cms_MenuId",
                table: "KodawariDownloads",
                newName: "KodawariMenuId");

            migrationBuilder.RenameIndex(
                name: "IX_KodawariDownloads_Cms_MenuId",
                table: "KodawariDownloads",
                newName: "IX_KodawariDownloads_KodawariMenuId");

            migrationBuilder.AddForeignKey(
                name: "FK_KodawariDownloads_KodawariMenus",
                table: "KodawariDownloads",
                column: "KodawariMenuId",
                principalTable: "KodawariMenus",
                principalColumn: "KodawariMenuId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_KodawariDownloads_KodawariMenus",
                table: "KodawariDownloads");

            migrationBuilder.RenameColumn(
                name: "KodawariMenuId",
                table: "KodawariDownloads",
                newName: "Cms_MenuId");

            migrationBuilder.RenameIndex(
                name: "IX_KodawariDownloads_KodawariMenuId",
                table: "KodawariDownloads",
                newName: "IX_KodawariDownloads_Cms_MenuId");

            migrationBuilder.AddForeignKey(
                name: "FK_KodawariDownloads_CmsMenus",
                table: "KodawariDownloads",
                column: "Cms_MenuId",
                principalTable: "Cms_Menus",
                principalColumn: "Cms_MenuId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
