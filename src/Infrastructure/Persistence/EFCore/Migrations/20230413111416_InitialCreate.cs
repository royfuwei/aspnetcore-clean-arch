using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CleanArch.Infrastructure.Persistence.EFCore.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "weatherforecast");

            migrationBuilder.CreateSequence(
                name: "weatherforecastseq",
                incrementBy: 10);

            migrationBuilder.CreateTable(
                name: "weatherForecasts",
                schema: "weatherforecast",
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
                schema: "weatherforecast");

            migrationBuilder.DropSequence(
                name: "weatherforecastseq");
        }
    }
}
