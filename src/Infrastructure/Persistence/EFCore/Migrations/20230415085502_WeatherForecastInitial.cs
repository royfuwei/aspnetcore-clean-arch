using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CleanArch.Infrastructure.Persistence.EFCore.Migrations
{
    /// <inheritdoc />
    public partial class WeatherForecastInitial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "dbo");

            migrationBuilder.CreateSequence(
                name: "weatherforecastseq",
                incrementBy: 10);

            migrationBuilder.CreateTable(
                name: "weatherForecasts",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    TemperatureC = table.Column<int>(type: "int", nullable: false),
                    Summary = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_weatherForecasts", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "weatherForecasts",
                schema: "dbo");

            migrationBuilder.DropSequence(
                name: "weatherforecastseq");
        }
    }
}
