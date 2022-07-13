using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Talent.Entities.Migrations
{
    public partial class area_specialist_table : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AreaSpecialistId",
                table: "Outlets",
                maxLength: 32,
                nullable: true);

            migrationBuilder.CreateTable(
                name: "AreaSpecialists",
                columns: table => new
                {
                    AreaSpecialistId = table.Column<string>(unicode: false, maxLength: 32, nullable: false),
                    AreaSpecialistName = table.Column<string>(unicode: false, maxLength: 128, nullable: false),
                    CreatedAt = table.Column<DateTime>(nullable: false, defaultValueSql: "(getdate())"),
                    CreatedBy = table.Column<string>(unicode: false, maxLength: 64, nullable: false, defaultValueSql: "('SYSTEM')"),
                    UpdatedAt = table.Column<DateTime>(nullable: false, defaultValueSql: "(getdate())"),
                    UpdatedBy = table.Column<string>(unicode: false, maxLength: 64, nullable: false, defaultValueSql: "('SYSTEM')"),
                    IsDeleted = table.Column<bool>(nullable: false, defaultValueSql: "((0))")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AreaSpecialists", x => x.AreaSpecialistId);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Outlets_AreaSpecialistId",
                table: "Outlets",
                column: "AreaSpecialistId");

            migrationBuilder.AddForeignKey(
                name: "FK_Outlets_AreaSpecialists",
                table: "Outlets",
                column: "AreaSpecialistId",
                principalTable: "AreaSpecialists",
                principalColumn: "AreaSpecialistId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Outlets_AreaSpecialists",
                table: "Outlets");

            migrationBuilder.DropTable(
                name: "AreaSpecialists");

            migrationBuilder.DropIndex(
                name: "IX_Outlets_AreaSpecialistId",
                table: "Outlets");

            migrationBuilder.DropColumn(
                name: "AreaSpecialistId",
                table: "Outlets");
        }
    }
}
