using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CleanArch.Infrastructure.Persistence.EFCore.Migrations
{
    /// <inheritdoc />
    public partial class WeatherForecastRenameTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_weatherForecasts",
                schema: "dbo",
                table: "weatherForecasts");

            migrationBuilder.RenameTable(
                name: "weatherForecasts",
                schema: "dbo",
                newName: "WeatherForecasts",
                newSchema: "dbo");

            migrationBuilder.AddPrimaryKey(
                name: "PK_WeatherForecasts",
                schema: "dbo",
                table: "WeatherForecasts",
                column: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_WeatherForecasts",
                schema: "dbo",
                table: "WeatherForecasts");

            migrationBuilder.RenameTable(
                name: "WeatherForecasts",
                schema: "dbo",
                newName: "weatherForecasts",
                newSchema: "dbo");

            migrationBuilder.AddPrimaryKey(
                name: "PK_weatherForecasts",
                schema: "dbo",
                table: "weatherForecasts",
                column: "Id");
        }
    }
}
