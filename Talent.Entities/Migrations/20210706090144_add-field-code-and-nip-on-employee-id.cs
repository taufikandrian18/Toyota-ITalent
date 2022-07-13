using Microsoft.EntityFrameworkCore.Migrations;

namespace Talent.Entities.Migrations
{
    public partial class addfieldcodeandniponemployeeid : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "EmployeeMDMCode",
                table: "Employees",
                maxLength: 64,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "EmployeeMDMUsername",
                table: "Employees",
                maxLength: 64,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "NIP",
                table: "Employees",
                maxLength: 64,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EmployeeMDMCode",
                table: "Employees");

            migrationBuilder.DropColumn(
                name: "EmployeeMDMUsername",
                table: "Employees");

            migrationBuilder.DropColumn(
                name: "NIP",
                table: "Employees");
        }
    }
}
