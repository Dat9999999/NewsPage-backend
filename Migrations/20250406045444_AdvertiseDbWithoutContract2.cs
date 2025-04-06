using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NewsPage.Migrations
{
    /// <inheritdoc />
    public partial class AdvertiseDbWithoutContract2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "TargetAutidon",
                table: "BannerCampaigns",
                newName: "TargetAutidienceId");

            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "BannerCampaigns",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Status",
                table: "BannerCampaigns");

            migrationBuilder.RenameColumn(
                name: "TargetAutidienceId",
                table: "BannerCampaigns",
                newName: "TargetAutidon");
        }
    }
}
