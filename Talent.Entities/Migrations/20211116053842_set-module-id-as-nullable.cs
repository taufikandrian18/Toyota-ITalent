using Microsoft.EntityFrameworkCore.Migrations;

namespace Talent.Entities.Migrations
{
    public partial class setmoduleidasnullable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "ModuleId",
                table: "SetupModules",
                nullable: true,
                oldClrType: typeof(int));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "ModuleId",
                table: "SetupModules",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);
        }
    }
}
