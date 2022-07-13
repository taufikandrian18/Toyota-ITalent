using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Talent.Entities.Migrations
{
    public partial class addTableKeyPieceOfMindMenusaddcolumnFK : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "KeyPieceOfMindMenuId",
                table: "KeyPieceOfMinds",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateTable(
                name: "KeyPieceOfMindMenus",
                columns: table => new
                {
                    KeyPieceOfMindMenuId = table.Column<Guid>(nullable: false),
                    KeyPieceOfMindMenuName = table.Column<string>(unicode: false, nullable: true),
                    KeyPieceOfMindMenuSequence = table.Column<int>(nullable: false),
                    CreatedAt = table.Column<DateTime>(nullable: false, defaultValueSql: "(getdate())"),
                    CreatedBy = table.Column<string>(unicode: false, maxLength: 64, nullable: false, defaultValueSql: "('SYSTEM')"),
                    UpdatedAt = table.Column<DateTime>(nullable: false, defaultValueSql: "(getdate())"),
                    UpdatedBy = table.Column<string>(unicode: false, maxLength: 64, nullable: false, defaultValueSql: "('SYSTEM')"),
                    IsDeleted = table.Column<bool>(nullable: false, defaultValueSql: "((0))")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_KeyPieceOfMindMenus", x => x.KeyPieceOfMindMenuId);
                });

            migrationBuilder.CreateTable(
                name: "UserLogActivities",
                columns: table => new
                {
                    Guid = table.Column<string>(maxLength: 64, nullable: false),
                    EmployeeId = table.Column<string>(maxLength: 64, nullable: true),
                    Type = table.Column<string>(nullable: true),
                    Content = table.Column<string>(nullable: true),
                    CreatedAt = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserLogActivities", x => x.Guid);
                });

            migrationBuilder.CreateIndex(
                name: "IX_KeyPieceOfMinds_KeyPieceOfMindMenuId",
                table: "KeyPieceOfMinds",
                column: "KeyPieceOfMindMenuId");

            migrationBuilder.AddForeignKey(
                name: "FK_KeyPieceOfMinds_KeyPieceOfMindMenus",
                table: "KeyPieceOfMinds",
                column: "KeyPieceOfMindMenuId",
                principalTable: "KeyPieceOfMindMenus",
                principalColumn: "KeyPieceOfMindMenuId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_KeyPieceOfMinds_KeyPieceOfMindMenus",
                table: "KeyPieceOfMinds");

            migrationBuilder.DropTable(
                name: "KeyPieceOfMindMenus");

            migrationBuilder.DropTable(
                name: "UserLogActivities");

            migrationBuilder.DropIndex(
                name: "IX_KeyPieceOfMinds_KeyPieceOfMindMenuId",
                table: "KeyPieceOfMinds");

            migrationBuilder.DropColumn(
                name: "KeyPieceOfMindMenuId",
                table: "KeyPieceOfMinds");
        }
    }
}
