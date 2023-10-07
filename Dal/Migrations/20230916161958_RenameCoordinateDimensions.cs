using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Dal.Migrations
{
    /// <inheritdoc />
    public partial class RenameCoordinateDimensions : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Theta",
                table: "PlanetCoordinate",
                newName: "Polar");

            migrationBuilder.RenameColumn(
                name: "Phi",
                table: "PlanetCoordinate",
                newName: "Azimuthal");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Polar",
                table: "PlanetCoordinate",
                newName: "Theta");

            migrationBuilder.RenameColumn(
                name: "Azimuthal",
                table: "PlanetCoordinate",
                newName: "Phi");
        }
    }
}
