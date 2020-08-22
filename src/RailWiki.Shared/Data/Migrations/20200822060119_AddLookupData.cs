using Microsoft.EntityFrameworkCore.Migrations;

namespace RailWiki.Shared.Data.Migrations
{
    public partial class AddLookupData : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData("RoadTypes",
                columns: new[] { "Id", "Name", "DisplayOrder" },
                values: new object[,] {
                    { 1, "Major Carriers", 1 },
                    { 2, "Regionals", 2 },
                    { 3, "Short Lines", 3 },
                    { 4, "Leasing Companies", 4 },
                    { 5, "Terminal", 5 },
                    { 6, "Industrial", 6 },
                    { 7, "Passenger", 7 },
                    { 8, "Fallen Flags", 8 },
                    { 9, "Foreign", 9 },
                    { 10, "Museum/Tourist", 10 },
                    { 11, "Private Owner", 11 },
                    { 12, "Intermodal Equipment", 12 },
                    { 13, "Other", 13 },
                    { 14, "Unclassified", 14 },
                    { 15, "Unrecognized", 15 },
                    { 16, "Non-AAR", 16 }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DELETE FROM RoadTypes");
        }
    }
}
