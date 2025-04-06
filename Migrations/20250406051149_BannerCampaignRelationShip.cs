using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NewsPage.Migrations
{
    /// <inheritdoc />
    public partial class BannerCampaignRelationShip : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Status",
                table: "BannerCampaigns",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.CreateIndex(
                name: "IX_BannerCampaigns_AdvertisorId",
                table: "BannerCampaigns",
                column: "AdvertisorId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_BannerCampaigns_TargetAutidienceId",
                table: "BannerCampaigns",
                column: "TargetAutidienceId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_BannerCampaigns_BannerAudiences_TargetAutidienceId",
                table: "BannerCampaigns",
                column: "TargetAutidienceId",
                principalTable: "BannerAudiences",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_BannerCampaigns_UserAccounts_AdvertisorId",
                table: "BannerCampaigns",
                column: "AdvertisorId",
                principalTable: "UserAccounts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BannerCampaigns_BannerAudiences_TargetAutidienceId",
                table: "BannerCampaigns");

            migrationBuilder.DropForeignKey(
                name: "FK_BannerCampaigns_UserAccounts_AdvertisorId",
                table: "BannerCampaigns");

            migrationBuilder.DropIndex(
                name: "IX_BannerCampaigns_AdvertisorId",
                table: "BannerCampaigns");

            migrationBuilder.DropIndex(
                name: "IX_BannerCampaigns_TargetAutidienceId",
                table: "BannerCampaigns");

            migrationBuilder.AlterColumn<int>(
                name: "Status",
                table: "BannerCampaigns",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");
        }
    }
}
