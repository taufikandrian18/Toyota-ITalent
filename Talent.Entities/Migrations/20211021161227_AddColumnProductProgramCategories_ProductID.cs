﻿using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Talent.Entities.Migrations
{
    public partial class AddColumnProductProgramCategories_ProductID : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "ProductId",
                table: "ProductProgramCategories",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_ProductProgramCategories_ProductId",
                table: "ProductProgramCategories",
                column: "ProductId");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductProgramCategories_Products",
                table: "ProductProgramCategories",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "ProductId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductProgramCategories_Products",
                table: "ProductProgramCategories");

            migrationBuilder.DropIndex(
                name: "IX_ProductProgramCategories_ProductId",
                table: "ProductProgramCategories");

            migrationBuilder.DropColumn(
                name: "ProductId",
                table: "ProductProgramCategories");
        }
    }
}
