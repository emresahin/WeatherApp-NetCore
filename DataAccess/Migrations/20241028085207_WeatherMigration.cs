using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class WeatherMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TBL_CITY",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TBL_CITY", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TBL_USER",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TBL_USER", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TBL_WEATHER_CITY",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CityId = table.Column<int>(type: "int", nullable: false),
                    Degree = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TBL_WEATHER_CITY", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TBL_WEATHER_CITY_TBL_CITY_CityId",
                        column: x => x.CityId,
                        principalTable: "TBL_CITY",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TBL_USER_CITY",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    CityId = table.Column<int>(type: "int", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TBL_USER_CITY", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TBL_USER_CITY_TBL_CITY_CityId",
                        column: x => x.CityId,
                        principalTable: "TBL_CITY",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TBL_USER_CITY_TBL_USER_UserId",
                        column: x => x.UserId,
                        principalTable: "TBL_USER",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TBL_USER_CITY_CityId",
                table: "TBL_USER_CITY",
                column: "CityId");

            migrationBuilder.CreateIndex(
                name: "IX_TBL_USER_CITY_UserId",
                table: "TBL_USER_CITY",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_TBL_WEATHER_CITY_CityId",
                table: "TBL_WEATHER_CITY",
                column: "CityId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TBL_USER_CITY");

            migrationBuilder.DropTable(
                name: "TBL_WEATHER_CITY");

            migrationBuilder.DropTable(
                name: "TBL_USER");

            migrationBuilder.DropTable(
                name: "TBL_CITY");
        }
    }
}
