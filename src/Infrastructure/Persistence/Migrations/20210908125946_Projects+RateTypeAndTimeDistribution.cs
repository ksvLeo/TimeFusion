using Microsoft.EntityFrameworkCore.Migrations;

namespace FusionIT.TimeFusion.Infrastructure.Persistence.Migrations
{
    public partial class ProjectsRateTypeAndTimeDistribution : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ProjectStatusId",
                table: "Projects",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "RateTypeId",
                table: "Projects",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "TimeDistributionId",
                table: "Projects",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Projects_ProjectStatusId",
                table: "Projects",
                column: "ProjectStatusId");

            migrationBuilder.CreateIndex(
                name: "IX_Projects_RateTypeId",
                table: "Projects",
                column: "RateTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Projects_TimeDistributionId",
                table: "Projects",
                column: "TimeDistributionId");

            migrationBuilder.AddForeignKey(
                name: "FK_Projects_ProjectStatuses_ProjectStatusId",
                table: "Projects",
                column: "ProjectStatusId",
                principalTable: "ProjectStatuses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Projects_RateTypes_RateTypeId",
                table: "Projects",
                column: "RateTypeId",
                principalTable: "RateTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Projects_TimeDistributions_TimeDistributionId",
                table: "Projects",
                column: "TimeDistributionId",
                principalTable: "TimeDistributions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Projects_ProjectStatuses_ProjectStatusId",
                table: "Projects");

            migrationBuilder.DropForeignKey(
                name: "FK_Projects_RateTypes_RateTypeId",
                table: "Projects");

            migrationBuilder.DropForeignKey(
                name: "FK_Projects_TimeDistributions_TimeDistributionId",
                table: "Projects");

            migrationBuilder.DropIndex(
                name: "IX_Projects_ProjectStatusId",
                table: "Projects");

            migrationBuilder.DropIndex(
                name: "IX_Projects_RateTypeId",
                table: "Projects");

            migrationBuilder.DropIndex(
                name: "IX_Projects_TimeDistributionId",
                table: "Projects");

            migrationBuilder.DropColumn(
                name: "ProjectStatusId",
                table: "Projects");

            migrationBuilder.DropColumn(
                name: "RateTypeId",
                table: "Projects");

            migrationBuilder.DropColumn(
                name: "TimeDistributionId",
                table: "Projects");
        }
    }
}
