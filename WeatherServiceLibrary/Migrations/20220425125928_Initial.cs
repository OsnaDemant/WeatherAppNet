using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WeatherServiceLibrary.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Clouds",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    All = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Clouds", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Coord",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Lon = table.Column<float>(type: "REAL", nullable: false),
                    Lat = table.Column<float>(type: "REAL", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Coord", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Main",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Temp = table.Column<float>(type: "REAL", nullable: false),
                    Feels_like = table.Column<float>(type: "REAL", nullable: false),
                    Temp_min = table.Column<float>(type: "REAL", nullable: false),
                    Temp_max = table.Column<float>(type: "REAL", nullable: false),
                    Pressure = table.Column<int>(type: "INTEGER", nullable: false),
                    Humidity = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Main", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Sys",
                columns: table => new
                {
                    DataBaseId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Type = table.Column<int>(type: "INTEGER", nullable: false),
                    Id = table.Column<int>(type: "INTEGER", nullable: false),
                    Country = table.Column<string>(type: "TEXT", nullable: true),
                    Sunrise = table.Column<int>(type: "INTEGER", nullable: false),
                    Sunset = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sys", x => x.DataBaseId);
                });

            migrationBuilder.CreateTable(
                name: "Wind",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Speed = table.Column<float>(type: "REAL", nullable: false),
                    Deg = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Wind", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "WeatherData",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    CoordId = table.Column<int>(type: "INTEGER", nullable: true),
                    Weather = table.Column<string>(type: "TEXT", nullable: true),
                    _base = table.Column<string>(type: "TEXT", nullable: true),
                    MainId = table.Column<int>(type: "INTEGER", nullable: true),
                    Visibility = table.Column<int>(type: "INTEGER", nullable: false),
                    WindId = table.Column<int>(type: "INTEGER", nullable: true),
                    CloudsId = table.Column<int>(type: "INTEGER", nullable: true),
                    Dt = table.Column<int>(type: "INTEGER", nullable: false),
                    SysDataBaseId = table.Column<int>(type: "INTEGER", nullable: true),
                    Timezone = table.Column<int>(type: "INTEGER", nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: true),
                    Cod = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WeatherData", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WeatherData_Clouds_CloudsId",
                        column: x => x.CloudsId,
                        principalTable: "Clouds",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_WeatherData_Coord_CoordId",
                        column: x => x.CoordId,
                        principalTable: "Coord",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_WeatherData_Main_MainId",
                        column: x => x.MainId,
                        principalTable: "Main",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_WeatherData_Sys_SysDataBaseId",
                        column: x => x.SysDataBaseId,
                        principalTable: "Sys",
                        principalColumn: "DataBaseId");
                    table.ForeignKey(
                        name: "FK_WeatherData_Wind_WindId",
                        column: x => x.WindId,
                        principalTable: "Wind",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "WeatherDataQuerys",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    WeatherDataId = table.Column<int>(type: "INTEGER", nullable: true),
                    CityName = table.Column<string>(type: "TEXT", nullable: true),
                    Time = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WeatherDataQuerys", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WeatherDataQuerys_WeatherData_WeatherDataId",
                        column: x => x.WeatherDataId,
                        principalTable: "WeatherData",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_WeatherData_CloudsId",
                table: "WeatherData",
                column: "CloudsId");

            migrationBuilder.CreateIndex(
                name: "IX_WeatherData_CoordId",
                table: "WeatherData",
                column: "CoordId");

            migrationBuilder.CreateIndex(
                name: "IX_WeatherData_MainId",
                table: "WeatherData",
                column: "MainId");

            migrationBuilder.CreateIndex(
                name: "IX_WeatherData_SysDataBaseId",
                table: "WeatherData",
                column: "SysDataBaseId");

            migrationBuilder.CreateIndex(
                name: "IX_WeatherData_WindId",
                table: "WeatherData",
                column: "WindId");

            migrationBuilder.CreateIndex(
                name: "IX_WeatherDataQuerys_WeatherDataId",
                table: "WeatherDataQuerys",
                column: "WeatherDataId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "WeatherDataQuerys");

            migrationBuilder.DropTable(
                name: "WeatherData");

            migrationBuilder.DropTable(
                name: "Clouds");

            migrationBuilder.DropTable(
                name: "Coord");

            migrationBuilder.DropTable(
                name: "Main");

            migrationBuilder.DropTable(
                name: "Sys");

            migrationBuilder.DropTable(
                name: "Wind");
        }
    }
}
