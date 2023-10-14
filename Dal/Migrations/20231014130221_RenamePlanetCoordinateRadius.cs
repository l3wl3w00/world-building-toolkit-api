using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Dal.Migrations
{
    /// <inheritdoc />
    public partial class RenamePlanetCoordinateRadius : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Radius",
                table: "Regions_Bounds",
                newName: "Height");

            migrationBuilder.RenameColumn(
                name: "Radius",
                table: "Continents_Bounds",
                newName: "Height");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Height",
                table: "Regions_Bounds",
                newName: "Radius");

            migrationBuilder.RenameColumn(
                name: "Height",
                table: "Continents_Bounds",
                newName: "Radius");
        }
    }
}
