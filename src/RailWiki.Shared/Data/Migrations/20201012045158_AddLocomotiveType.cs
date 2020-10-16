using Microsoft.EntityFrameworkCore.Migrations;

namespace RailWiki.Shared.Data.Migrations
{
    public partial class AddLocomotiveType : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "TypeId",
                table: "Locomotives",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "LocomotiveTypes",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Family = table.Column<string>(maxLength: 25, nullable: true),
                    Manufacturer = table.Column<string>(maxLength: 50, nullable: false),
                    Name = table.Column<string>(nullable: true),
                    LocomotiveCount = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LocomotiveTypes", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Locomotives_TypeId",
                table: "Locomotives",
                column: "TypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Locomotives_LocomotiveTypes_TypeId",
                table: "Locomotives",
                column: "TypeId",
                principalTable: "LocomotiveTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Locomotives_LocomotiveTypes_TypeId",
                table: "Locomotives");

            migrationBuilder.DropTable(
                name: "LocomotiveTypes");

            migrationBuilder.DropIndex(
                name: "IX_Locomotives_TypeId",
                table: "Locomotives");

            migrationBuilder.DropColumn(
                name: "TypeId",
                table: "Locomotives");
        }
    }
}
