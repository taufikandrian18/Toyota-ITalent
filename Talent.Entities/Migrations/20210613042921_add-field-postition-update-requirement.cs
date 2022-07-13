using Microsoft.EntityFrameworkCore.Migrations;

namespace Talent.Entities.Migrations
{
    public partial class addfieldpostitionupdaterequirement : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsOtherDealer",
                table: "Positions",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "PoistionNote",
                table: "Employees",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsOtherDealer",
                table: "Positions");

            migrationBuilder.DropColumn(
                name: "PoistionNote",
                table: "Employees");
        }
    }
}
