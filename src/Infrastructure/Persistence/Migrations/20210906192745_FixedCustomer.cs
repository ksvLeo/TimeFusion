using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace FusionIT.TimeFusion.Infrastructure.Persistence.Migrations
{
    public partial class FixedCustomer : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Customer_CurrencyReferences_CurrencyId",
                table: "Customer");

            migrationBuilder.DropForeignKey(
                name: "FK_Customer_Referrers_ReferrerId",
                table: "Customer");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Customer",
                table: "Customer");

            migrationBuilder.RenameTable(
                name: "Customer",
                newName: "Customers");

            migrationBuilder.RenameIndex(
                name: "IX_Customer_ReferrerId",
                table: "Customers",
                newName: "IX_Customers_ReferrerId");

            migrationBuilder.RenameIndex(
                name: "IX_Customer_CurrencyId",
                table: "Customers",
                newName: "IX_Customers_CurrencyId");

            migrationBuilder.AddColumn<DateTime>(
                name: "Created",
                table: "CurrencyReferences",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                table: "CurrencyReferences",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "LastModified",
                table: "CurrencyReferences",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LastModifiedBy",
                table: "CurrencyReferences",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "Created",
                table: "Customers",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                table: "Customers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "LastModified",
                table: "Customers",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LastModifiedBy",
                table: "Customers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Customers",
                table: "Customers",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Customers_CurrencyReferences_CurrencyId",
                table: "Customers",
                column: "CurrencyId",
                principalTable: "CurrencyReferences",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Customers_Referrers_ReferrerId",
                table: "Customers",
                column: "ReferrerId",
                principalTable: "Referrers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Customers_CurrencyReferences_CurrencyId",
                table: "Customers");

            migrationBuilder.DropForeignKey(
                name: "FK_Customers_Referrers_ReferrerId",
                table: "Customers");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Customers",
                table: "Customers");

            migrationBuilder.DropColumn(
                name: "Created",
                table: "CurrencyReferences");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "CurrencyReferences");

            migrationBuilder.DropColumn(
                name: "LastModified",
                table: "CurrencyReferences");

            migrationBuilder.DropColumn(
                name: "LastModifiedBy",
                table: "CurrencyReferences");

            migrationBuilder.DropColumn(
                name: "Created",
                table: "Customers");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "Customers");

            migrationBuilder.DropColumn(
                name: "LastModified",
                table: "Customers");

            migrationBuilder.DropColumn(
                name: "LastModifiedBy",
                table: "Customers");

            migrationBuilder.RenameTable(
                name: "Customers",
                newName: "Customer");

            migrationBuilder.RenameIndex(
                name: "IX_Customers_ReferrerId",
                table: "Customer",
                newName: "IX_Customer_ReferrerId");

            migrationBuilder.RenameIndex(
                name: "IX_Customers_CurrencyId",
                table: "Customer",
                newName: "IX_Customer_CurrencyId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Customer",
                table: "Customer",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Customer_CurrencyReferences_CurrencyId",
                table: "Customer",
                column: "CurrencyId",
                principalTable: "CurrencyReferences",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Customer_Referrers_ReferrerId",
                table: "Customer",
                column: "ReferrerId",
                principalTable: "Referrers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
