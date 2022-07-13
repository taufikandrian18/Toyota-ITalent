using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Talent.Entities.Migrations
{
    public partial class newTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "UpgradeAccountApprovals",
                columns: table => new
                {
                    Guid = table.Column<string>(maxLength: 64, nullable: false),
                    EmployeeId = table.Column<string>(maxLength: 64, nullable: true),
                    ApprovalDate = table.Column<DateTime>(nullable: false, defaultValueSql: "(getdate())"),
                    ApprovedBy = table.Column<string>(nullable: true),
                    ApprovedDate = table.Column<DateTime>(nullable: false),
                    CreatedAt = table.Column<DateTime>(nullable: false, defaultValueSql: "(getdate())"),
                    CreatedBy = table.Column<string>(nullable: true),
                    UpdatedAt = table.Column<DateTime>(nullable: false, defaultValueSql: "(getdate())"),
                    UpdatedBy = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UpgradeAccountApprovals", x => x.Guid);
                    table.ForeignKey(
                        name: "FK_Upgrade_Approvals_Employees",
                        column: x => x.EmployeeId,
                        principalTable: "Employees",
                        principalColumn: "EmployeeId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UpgradeAccountApprovals_EmployeeId",
                table: "UpgradeAccountApprovals",
                column: "EmployeeId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UpgradeAccountApprovals");
        }
    }
}
