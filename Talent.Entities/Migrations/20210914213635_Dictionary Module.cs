using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Talent.Entities.Migrations
{
    public partial class DictionaryModule : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Dictionaries",
                columns: table => new
                {
                    DictionaryId = table.Column<Guid>(nullable: false),
                    BlobId = table.Column<Guid>(nullable: false),
                    DictionaryName = table.Column<string>(unicode: false, maxLength: 50, nullable: false),
                    DictionaryStatus = table.Column<bool>(nullable: false),
                    IsFavorite = table.Column<bool>(nullable: true),
                    DictionaryArti = table.Column<string>(unicode: false, maxLength: 250, nullable: false),
                    DictionaryManfaat = table.Column<string>(unicode: false, maxLength: 250, nullable: false),
                    DictionaryFakta = table.Column<string>(unicode: false, maxLength: 250, nullable: false),
                    DictionaryBasicOperation = table.Column<string>(unicode: false, maxLength: 250, nullable: true),
                    CreatedAt = table.Column<DateTime>(nullable: false, defaultValueSql: "(getdate())"),
                    CreatedBy = table.Column<string>(unicode: false, maxLength: 64, nullable: false, defaultValueSql: "('SYSTEM')"),
                    UpdatedAt = table.Column<DateTime>(nullable: false, defaultValueSql: "(getdate())"),
                    UpdatedBy = table.Column<string>(unicode: false, maxLength: 64, nullable: false, defaultValueSql: "('SYSTEM')"),
                    IsDeleted = table.Column<bool>(nullable: false, defaultValueSql: "((0))")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Dictionaries", x => x.DictionaryId);
                    table.ForeignKey(
                        name: "FK_Dictionaries_Blobs",
                        column: x => x.BlobId,
                        principalTable: "Blobs",
                        principalColumn: "BlobId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DictionaryMappings",
                columns: table => new
                {
                    DictionaryId = table.Column<Guid>(nullable: false),
                    EmployeeId = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DictionaryMappings", x => new { x.DictionaryId, x.EmployeeId });
                    table.ForeignKey(
                        name: "FK_DictionaryMappings_Dictionaries",
                        column: x => x.DictionaryId,
                        principalTable: "Dictionaries",
                        principalColumn: "DictionaryId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_DictionaryMappings_Employees",
                        column: x => x.EmployeeId,
                        principalTable: "Employees",
                        principalColumn: "EmployeeId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Dictionaries_BlobId",
                table: "Dictionaries",
                column: "BlobId");

            migrationBuilder.CreateIndex(
                name: "IX_DictionaryMappings_EmployeeId",
                table: "DictionaryMappings",
                column: "EmployeeId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DictionaryMappings");

            migrationBuilder.DropTable(
                name: "Dictionaries");
        }
    }
}
