using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Talent.Entities.Migrations
{
    public partial class changefieldflagging : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<Guid>(
                name: "Cms_MenuId",
                table: "ProductProgramMappings",
                nullable: true,
                oldClrType: typeof(Guid));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<Guid>(
                name: "Cms_MenuId",
                table: "ProductProgramMappings",
                nullable: false,
                oldClrType: typeof(Guid),
                oldNullable: true);
        }
    }
}
