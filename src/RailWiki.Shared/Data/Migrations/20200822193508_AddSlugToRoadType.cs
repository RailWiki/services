using Microsoft.EntityFrameworkCore.Migrations;

namespace RailWiki.Shared.Data.Migrations
{
    public partial class AddSlugToRoadType : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Slug",
                table: "RoadTypes",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.Sql(@"
                UPDATE rt
                SET rt.Slug = LOWER(REPLACE(rt.Name, ' ', '-'))
                FROM RoadTypes rt
            ");

            migrationBuilder.AddUniqueConstraint(
                name: "UNQ_RoadType_Slug",
                table: "RoadTypes",
                column: "Slug");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Slug",
                table: "RoadTypes");
        }
    }
}
