using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Talent.Entities.Migrations
{
    public partial class AddColumnProductTipCategories_ProductID : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "ProductId",
                table: "ProductTipCategories",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AlterColumn<string>(
                name: "ProductCustomerUmur",
                table: "ProductCustomers",
                type: "varchar(MAX)",
                unicode: false,
                nullable: false,
                oldClrType: typeof(string),
                oldUnicode: false,
                oldMaxLength: 50);

            migrationBuilder.AlterColumn<string>(
                name: "ProductCustomerStatus",
                table: "ProductCustomers",
                type: "varchar(MAX)",
                unicode: false,
                nullable: false,
                oldClrType: typeof(string),
                oldUnicode: false,
                oldMaxLength: 50);

            migrationBuilder.AlterColumn<string>(
                name: "ProductCustomerProspectSource",
                table: "ProductCustomers",
                type: "varchar(MAX)",
                unicode: false,
                nullable: true,
                oldClrType: typeof(string),
                oldUnicode: false,
                oldMaxLength: 250,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "ProductCustomerPrevVehicle",
                table: "ProductCustomers",
                type: "varchar(MAX)",
                unicode: false,
                nullable: false,
                oldClrType: typeof(string),
                oldUnicode: false,
                oldMaxLength: 50);

            migrationBuilder.AlterColumn<string>(
                name: "ProductCustomerPenghasilan",
                table: "ProductCustomers",
                type: "varchar(MAX)",
                unicode: false,
                nullable: true,
                oldClrType: typeof(string),
                oldUnicode: false,
                oldMaxLength: 50,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "ProductCustomerPekerjaan",
                table: "ProductCustomers",
                type: "varchar(MAX)",
                unicode: false,
                nullable: false,
                oldClrType: typeof(string),
                oldUnicode: false,
                oldMaxLength: 50);

            migrationBuilder.AlterColumn<string>(
                name: "ProductCustomerKebutuhan",
                table: "ProductCustomers",
                type: "varchar(MAX)",
                unicode: false,
                nullable: false,
                oldClrType: typeof(string),
                oldUnicode: false,
                oldMaxLength: 50);

            migrationBuilder.CreateIndex(
                name: "IX_ProductTipCategories_ProductId",
                table: "ProductTipCategories",
                column: "ProductId");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductTipCategories_Products",
                table: "ProductTipCategories",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "ProductId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductTipCategories_Products",
                table: "ProductTipCategories");

            migrationBuilder.DropIndex(
                name: "IX_ProductTipCategories_ProductId",
                table: "ProductTipCategories");

            migrationBuilder.DropColumn(
                name: "ProductId",
                table: "ProductTipCategories");

            migrationBuilder.AlterColumn<string>(
                name: "ProductCustomerUmur",
                table: "ProductCustomers",
                unicode: false,
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(MAX)",
                oldUnicode: false);

            migrationBuilder.AlterColumn<string>(
                name: "ProductCustomerStatus",
                table: "ProductCustomers",
                unicode: false,
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(MAX)",
                oldUnicode: false);

            migrationBuilder.AlterColumn<string>(
                name: "ProductCustomerProspectSource",
                table: "ProductCustomers",
                unicode: false,
                maxLength: 250,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(MAX)",
                oldUnicode: false,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "ProductCustomerPrevVehicle",
                table: "ProductCustomers",
                unicode: false,
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(MAX)",
                oldUnicode: false);

            migrationBuilder.AlterColumn<string>(
                name: "ProductCustomerPenghasilan",
                table: "ProductCustomers",
                unicode: false,
                maxLength: 50,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(MAX)",
                oldUnicode: false,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "ProductCustomerPekerjaan",
                table: "ProductCustomers",
                unicode: false,
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(MAX)",
                oldUnicode: false);

            migrationBuilder.AlterColumn<string>(
                name: "ProductCustomerKebutuhan",
                table: "ProductCustomers",
                unicode: false,
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(MAX)",
                oldUnicode: false);
        }
    }
}
