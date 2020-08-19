using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace RailWiki.Shared.Data.Migrations
{
    public partial class InitialMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Categories",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categories", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Countries",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(maxLength: 100, nullable: false),
                    CountryCode = table.Column<string>(maxLength: 5, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Countries", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "RoadTypes",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(maxLength: 50, nullable: false),
                    DisplayOrder = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RoadTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "RollingStockTypes",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(maxLength: 50, nullable: false),
                    AARDesignation = table.Column<string>(maxLength: 10, nullable: false),
                    Description = table.Column<string>(maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RollingStockTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SubjectId = table.Column<string>(nullable: true),
                    EmailAddress = table.Column<string>(maxLength: 100, nullable: false),
                    FirstName = table.Column<string>(maxLength: 50, nullable: false),
                    LastName = table.Column<string>(maxLength: 50, nullable: false),
                    RegisteredOn = table.Column<DateTime>(nullable: false),
                    ApprovedOn = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "StateProvinces",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CountryId = table.Column<int>(nullable: false),
                    Name = table.Column<string>(maxLength: 50, nullable: false),
                    Abbreviation = table.Column<string>(maxLength: 5, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StateProvinces", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StateProvinces_Countries_CountryId",
                        column: x => x.CountryId,
                        principalTable: "Countries",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Roads",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoadTypeId = table.Column<int>(nullable: false),
                    ParentId = table.Column<int>(nullable: true),
                    Name = table.Column<string>(maxLength: 50, nullable: false),
                    Slug = table.Column<string>(maxLength: 100, nullable: false),
                    ReportingMarks = table.Column<string>(maxLength: 10, nullable: false),
                    LocomotiveCount = table.Column<int>(nullable: false),
                    RollingStockCount = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Roads", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Roads_Roads_ParentId",
                        column: x => x.ParentId,
                        principalTable: "Roads",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Roads_RoadTypes_RoadTypeId",
                        column: x => x.RoadTypeId,
                        principalTable: "RoadTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RollingStockClasses",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RollingStockTypeId = table.Column<int>(nullable: false),
                    Name = table.Column<string>(maxLength: 50, nullable: false),
                    AARDesignation = table.Column<string>(maxLength: 10, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RollingStockClasses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RollingStockClasses_RollingStockTypes_RollingStockTypeId",
                        column: x => x.RollingStockTypeId,
                        principalTable: "RollingStockTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Albums",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(nullable: false),
                    Title = table.Column<string>(maxLength: 50, nullable: true),
                    Description = table.Column<string>(maxLength: 500, nullable: true),
                    PhotoCount = table.Column<int>(nullable: false),
                    CreatedOn = table.Column<DateTime>(nullable: false),
                    UpdatedOn = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Albums", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Albums_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Locations",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(maxLength: 50, nullable: false),
                    Latitude = table.Column<float>(nullable: true),
                    Longitude = table.Column<float>(nullable: true),
                    StateProvinceId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Locations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Locations_StateProvinces_StateProvinceId",
                        column: x => x.StateProvinceId,
                        principalTable: "StateProvinces",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Locomotives",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoadId = table.Column<int>(nullable: false),
                    RoadNumber = table.Column<string>(maxLength: 10, nullable: false),
                    Notes = table.Column<string>(maxLength: 255, nullable: true),
                    Slug = table.Column<string>(maxLength: 255, nullable: true),
                    ModelNumber = table.Column<string>(maxLength: 25, nullable: true),
                    SerialNumber = table.Column<string>(maxLength: 50, nullable: true),
                    FrameNumber = table.Column<string>(maxLength: 50, nullable: true),
                    BuiltAs = table.Column<string>(maxLength: 50, nullable: true),
                    BuildMonth = table.Column<int>(nullable: true),
                    BuildYear = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Locomotives", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Locomotives_Roads_RoadId",
                        column: x => x.RoadId,
                        principalTable: "Roads",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RoadAlternateNames",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoadId = table.Column<int>(nullable: false),
                    Name = table.Column<string>(maxLength: 50, nullable: false),
                    ReportingMarks = table.Column<string>(maxLength: 10, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RoadAlternateNames", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RoadAlternateNames_Roads_RoadId",
                        column: x => x.RoadId,
                        principalTable: "Roads",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RollingStockItems",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoadId = table.Column<int>(nullable: false),
                    RoadNumber = table.Column<string>(maxLength: 10, nullable: false),
                    Notes = table.Column<string>(maxLength: 255, nullable: true),
                    Slug = table.Column<string>(maxLength: 255, nullable: true),
                    RollingStockTypeId = table.Column<int>(nullable: false),
                    RollingStockClassId = table.Column<int>(nullable: false),
                    Details = table.Column<string>(maxLength: 500, nullable: true),
                    Plate = table.Column<string>(maxLength: 25, nullable: true),
                    MaxGrossWeight = table.Column<int>(nullable: false),
                    LoadLimit = table.Column<int>(nullable: false),
                    DryCapacity = table.Column<int>(nullable: false),
                    ExteriorDimensions = table.Column<string>(nullable: true),
                    InteriorDimensions = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RollingStockItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RollingStockItems_Roads_RoadId",
                        column: x => x.RoadId,
                        principalTable: "Roads",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RollingStockItems_RollingStockClasses_RollingStockClassId",
                        column: x => x.RollingStockClassId,
                        principalTable: "RollingStockClasses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RollingStockItems_RollingStockTypes_RollingStockTypeId",
                        column: x => x.RollingStockTypeId,
                        principalTable: "RollingStockTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Photos",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AlbumId = table.Column<int>(nullable: false),
                    UserId = table.Column<int>(nullable: false),
                    Author = table.Column<string>(maxLength: 50, nullable: true),
                    LocationName = table.Column<string>(maxLength: 50, nullable: true),
                    LocationId = table.Column<int>(nullable: true),
                    Title = table.Column<string>(maxLength: 50, nullable: true),
                    Description = table.Column<string>(maxLength: 509, nullable: true),
                    Filename = table.Column<string>(maxLength: 255, nullable: false),
                    PhotoDate = table.Column<DateTime>(nullable: true),
                    UploadDate = table.Column<DateTime>(nullable: false),
                    ViewCount = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Photos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Photos_Albums_AlbumId",
                        column: x => x.AlbumId,
                        principalTable: "Albums",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Photos_Locations_LocationId",
                        column: x => x.LocationId,
                        principalTable: "Locations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Photos_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PhotoCategories",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PhotoId = table.Column<int>(nullable: false),
                    CategoryId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PhotoCategories", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PhotoCategories_Categories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Categories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PhotoCategories_Photos_PhotoId",
                        column: x => x.PhotoId,
                        principalTable: "Photos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PhotoLocomotives",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PhotoId = table.Column<int>(nullable: false),
                    LocomotiveId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PhotoLocomotives", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PhotoLocomotives_Locomotives_LocomotiveId",
                        column: x => x.LocomotiveId,
                        principalTable: "Locomotives",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PhotoLocomotives_Photos_PhotoId",
                        column: x => x.PhotoId,
                        principalTable: "Photos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PhotoRollingStocks",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PhotoId = table.Column<int>(nullable: false),
                    RollingStockId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PhotoRollingStocks", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PhotoRollingStocks_Photos_PhotoId",
                        column: x => x.PhotoId,
                        principalTable: "Photos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PhotoRollingStocks_RollingStockItems_RollingStockId",
                        column: x => x.RollingStockId,
                        principalTable: "RollingStockItems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Albums_UserId",
                table: "Albums",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Locations_StateProvinceId",
                table: "Locations",
                column: "StateProvinceId");

            migrationBuilder.CreateIndex(
                name: "IX_Locomotives_RoadId",
                table: "Locomotives",
                column: "RoadId");

            migrationBuilder.CreateIndex(
                name: "IX_PhotoCategories_CategoryId",
                table: "PhotoCategories",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_PhotoCategories_PhotoId",
                table: "PhotoCategories",
                column: "PhotoId");

            migrationBuilder.CreateIndex(
                name: "IX_PhotoLocomotives_LocomotiveId",
                table: "PhotoLocomotives",
                column: "LocomotiveId");

            migrationBuilder.CreateIndex(
                name: "IX_PhotoLocomotives_PhotoId",
                table: "PhotoLocomotives",
                column: "PhotoId");

            migrationBuilder.CreateIndex(
                name: "IX_PhotoRollingStocks_PhotoId",
                table: "PhotoRollingStocks",
                column: "PhotoId");

            migrationBuilder.CreateIndex(
                name: "IX_PhotoRollingStocks_RollingStockId",
                table: "PhotoRollingStocks",
                column: "RollingStockId");

            migrationBuilder.CreateIndex(
                name: "IX_Photos_AlbumId",
                table: "Photos",
                column: "AlbumId");

            migrationBuilder.CreateIndex(
                name: "IX_Photos_LocationId",
                table: "Photos",
                column: "LocationId");

            migrationBuilder.CreateIndex(
                name: "IX_Photos_UserId",
                table: "Photos",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_RoadAlternateNames_RoadId",
                table: "RoadAlternateNames",
                column: "RoadId");

            migrationBuilder.CreateIndex(
                name: "IX_Roads_ParentId",
                table: "Roads",
                column: "ParentId");

            migrationBuilder.CreateIndex(
                name: "IX_Roads_RoadTypeId",
                table: "Roads",
                column: "RoadTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_RollingStockClasses_RollingStockTypeId",
                table: "RollingStockClasses",
                column: "RollingStockTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_RollingStockItems_RoadId",
                table: "RollingStockItems",
                column: "RoadId");

            migrationBuilder.CreateIndex(
                name: "IX_RollingStockItems_RollingStockClassId",
                table: "RollingStockItems",
                column: "RollingStockClassId");

            migrationBuilder.CreateIndex(
                name: "IX_RollingStockItems_RollingStockTypeId",
                table: "RollingStockItems",
                column: "RollingStockTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_StateProvinces_CountryId",
                table: "StateProvinces",
                column: "CountryId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PhotoCategories");

            migrationBuilder.DropTable(
                name: "PhotoLocomotives");

            migrationBuilder.DropTable(
                name: "PhotoRollingStocks");

            migrationBuilder.DropTable(
                name: "RoadAlternateNames");

            migrationBuilder.DropTable(
                name: "Categories");

            migrationBuilder.DropTable(
                name: "Locomotives");

            migrationBuilder.DropTable(
                name: "Photos");

            migrationBuilder.DropTable(
                name: "RollingStockItems");

            migrationBuilder.DropTable(
                name: "Albums");

            migrationBuilder.DropTable(
                name: "Locations");

            migrationBuilder.DropTable(
                name: "Roads");

            migrationBuilder.DropTable(
                name: "RollingStockClasses");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "StateProvinces");

            migrationBuilder.DropTable(
                name: "RoadTypes");

            migrationBuilder.DropTable(
                name: "RollingStockTypes");

            migrationBuilder.DropTable(
                name: "Countries");
        }
    }
}
