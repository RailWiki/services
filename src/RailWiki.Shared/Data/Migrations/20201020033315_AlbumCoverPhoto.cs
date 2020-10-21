using Microsoft.EntityFrameworkCore.Migrations;

namespace RailWiki.Shared.Data.Migrations
{
    public partial class AlbumCoverPhoto : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CoverPhotoFileName",
                table: "Albums",
                maxLength: 255,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CoverPhotoId",
                table: "Albums",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CoverPhotoFileName",
                table: "Albums");

            migrationBuilder.DropColumn(
                name: "CoverPhotoId",
                table: "Albums");
        }
    }
}
