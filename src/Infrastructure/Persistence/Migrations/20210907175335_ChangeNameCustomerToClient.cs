using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace FusionIT.TimeFusion.Infrastructure.Persistence.Migrations
{
    public partial class ChangeNameCustomerToClient : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Projects_Customers_ClientId",
                table: "Projects");

            migrationBuilder.DropTable(
                name: "Customers");

            migrationBuilder.AddColumn<bool>(
                name: "Active",
                table: "Referrers",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "ClientId",
                table: "Referrers",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Title",
                table: "Referrers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Clients",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CurrencyId = table.Column<int>(type: "int", nullable: true),
                    Active = table.Column<bool>(type: "bit", nullable: false),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastModified = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Clients", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Clients_Currencies_CurrencyId",
                        column: x => x.CurrencyId,
                        principalTable: "Currencies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Referrers_ClientId",
                table: "Referrers",
                column: "ClientId");

            migrationBuilder.CreateIndex(
                name: "IX_Clients_CurrencyId",
                table: "Clients",
                column: "CurrencyId");

            migrationBuilder.AddForeignKey(
                name: "FK_Projects_Clients_ClientId",
                table: "Projects",
                column: "ClientId",
                principalTable: "Clients",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Referrers_Clients_ClientId",
                table: "Referrers",
                column: "ClientId",
                principalTable: "Clients",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Projects_Clients_ClientId",
                table: "Projects");

            migrationBuilder.DropForeignKey(
                name: "FK_Referrers_Clients_ClientId",
                table: "Referrers");

            migrationBuilder.DropTable(
                name: "Clients");

            migrationBuilder.DropIndex(
                name: "IX_Referrers_ClientId",
                table: "Referrers");

            migrationBuilder.DropColumn(
                name: "Active",
                table: "Referrers");

            migrationBuilder.DropColumn(
                name: "ClientId",
                table: "Referrers");

            migrationBuilder.DropColumn(
                name: "Title",
                table: "Referrers");

            migrationBuilder.CreateTable(
                name: "Customers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Active = table.Column<bool>(type: "bit", nullable: false),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CurrencyId = table.Column<int>(type: "int", nullable: true),
                    LastModified = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ReferrerId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Customers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Customers_Currencies_CurrencyId",
                        column: x => x.CurrencyId,
                        principalTable: "Currencies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Customers_Referrers_ReferrerId",
                        column: x => x.ReferrerId,
                        principalTable: "Referrers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Customers_CurrencyId",
                table: "Customers",
                column: "CurrencyId");

            migrationBuilder.CreateIndex(
                name: "IX_Customers_ReferrerId",
                table: "Customers",
                column: "ReferrerId");

            migrationBuilder.AddForeignKey(
                name: "FK_Projects_Customers_ClientId",
                table: "Projects",
                column: "ClientId",
                principalTable: "Customers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
