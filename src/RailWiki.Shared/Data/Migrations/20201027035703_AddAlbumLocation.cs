using Microsoft.EntityFrameworkCore.Migrations;

namespace RailWiki.Shared.Data.Migrations
{
    public partial class AddAlbumLocation : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "LocationId",
                table: "Albums",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Albums_LocationId",
                table: "Albums",
                column: "LocationId");

            migrationBuilder.AddForeignKey(
                name: "FK_Albums_Locations_LocationId",
                table: "Albums",
                column: "LocationId",
                principalTable: "Locations",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Albums_Locations_LocationId",
                table: "Albums");

            migrationBuilder.DropIndex(
                name: "IX_Albums_LocationId",
                table: "Albums");

            migrationBuilder.DropColumn(
                name: "LocationId",
                table: "Albums");
        }
    }
}
