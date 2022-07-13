using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Talent.Entities.Migrations
{
    public partial class ProductCustomerProductCustomerType : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ProductCustomerTypes",
                columns: table => new
                {
                    ProductCustomerTypeId = table.Column<Guid>(nullable: false),
                    ProductCustomerTypeName = table.Column<string>(unicode: false, maxLength: 50, nullable: false),
                    CreatedAt = table.Column<DateTime>(nullable: false, defaultValueSql: "(getdate())"),
                    CreatedBy = table.Column<string>(unicode: false, maxLength: 64, nullable: false, defaultValueSql: "('SYSTEM')"),
                    UpdatedAt = table.Column<DateTime>(nullable: false, defaultValueSql: "(getdate())"),
                    UpdatedBy = table.Column<string>(unicode: false, maxLength: 64, nullable: false, defaultValueSql: "('SYSTEM')"),
                    IsDeleted = table.Column<bool>(nullable: false, defaultValueSql: "((0))")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductCustomerTypes", x => x.ProductCustomerTypeId);
                });

            migrationBuilder.CreateTable(
                name: "ProductCustomers",
                columns: table => new
                {
                    ProductCustomerId = table.Column<Guid>(nullable: false),
                    ProductId = table.Column<Guid>(nullable: false),
                    ProductCustomerTypeId = table.Column<Guid>(nullable: false),
                    ProductCustomerStatus = table.Column<string>(unicode: false, maxLength: 50, nullable: false),
                    ProductCustomerPenghasilan = table.Column<string>(unicode: false, maxLength: 50, nullable: true),
                    ProductCustomerKebutuhan = table.Column<string>(unicode: false, maxLength: 50, nullable: false),
                    ProductCustomerUmur = table.Column<string>(unicode: false, maxLength: 50, nullable: false),
                    ProductCustomerPekerjaan = table.Column<string>(unicode: false, maxLength: 50, nullable: false),
                    ProductCustomerPrevVehicle = table.Column<string>(unicode: false, maxLength: 50, nullable: false),
                    ProductCustomerProspectSource = table.Column<string>(unicode: false, maxLength: 250, nullable: true),
                    CreatedAt = table.Column<DateTime>(nullable: false, defaultValueSql: "(getdate())"),
                    CreatedBy = table.Column<string>(unicode: false, maxLength: 64, nullable: false, defaultValueSql: "('SYSTEM')"),
                    UpdatedAt = table.Column<DateTime>(nullable: false, defaultValueSql: "(getdate())"),
                    UpdatedBy = table.Column<string>(unicode: false, maxLength: 64, nullable: false, defaultValueSql: "('SYSTEM')"),
                    IsDeleted = table.Column<bool>(nullable: false, defaultValueSql: "((0))")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductCustomers", x => x.ProductCustomerId);
                    table.ForeignKey(
                        name: "FK_ProductCustomers_ProductCustomerTypes",
                        column: x => x.ProductCustomerTypeId,
                        principalTable: "ProductCustomerTypes",
                        principalColumn: "ProductCustomerTypeId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ProductCustomers_Products",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "ProductId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ProductCustomers_ProductCustomerTypeId",
                table: "ProductCustomers",
                column: "ProductCustomerTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductCustomers_ProductId",
                table: "ProductCustomers",
                column: "ProductId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProductCustomers");

            migrationBuilder.DropTable(
                name: "ProductCustomerTypes");
        }
    }
}
