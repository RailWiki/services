using Microsoft.EntityFrameworkCore.Migrations;

namespace RailWiki.Shared.Data.Migrations
{
    public partial class AddCrossRefTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CrossReferences",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EntityType = table.Column<string>(maxLength: 50, nullable: false),
                    EntityId = table.Column<int>(nullable: false),
                    Source = table.Column<string>(maxLength: 50, nullable: false),
                    SourceIdentifier = table.Column<string>(maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CrossReferences", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CrossReferences");
        }
    }
}
