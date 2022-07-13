using Microsoft.EntityFrameworkCore.Migrations;

namespace Talent.Entities.Migrations
{
    public partial class addremedialfieldssetuolearning : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "EnumRemedialOption",
                table: "SetupModules",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "EnumScoringMethod",
                table: "SetupModules",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsByOption",
                table: "SetupModules",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<float>(
                name: "RemedialLimit",
                table: "SetupModules",
                nullable: false,
                defaultValue: 0f);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EnumRemedialOption",
                table: "SetupModules");

            migrationBuilder.DropColumn(
                name: "EnumScoringMethod",
                table: "SetupModules");

            migrationBuilder.DropColumn(
                name: "IsByOption",
                table: "SetupModules");

            migrationBuilder.DropColumn(
                name: "RemedialLimit",
                table: "SetupModules");
        }
    }
}
