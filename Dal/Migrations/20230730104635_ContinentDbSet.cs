using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Dal.Migrations
{
    /// <inheritdoc />
    public partial class ContinentDbSet : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Continent_Worlds_WorldId",
                table: "Continent");

            migrationBuilder.DropForeignKey(
                name: "FK_WorldCoordinate_Continent_ContinentId",
                table: "WorldCoordinate");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Continent",
                table: "Continent");

            migrationBuilder.RenameTable(
                name: "Continent",
                newName: "Continents");

            migrationBuilder.RenameIndex(
                name: "IX_Continent_WorldId",
                table: "Continents",
                newName: "IX_Continents_WorldId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Continents",
                table: "Continents",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Continents_Worlds_WorldId",
                table: "Continents",
                column: "WorldId",
                principalTable: "Worlds",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_WorldCoordinate_Continents_ContinentId",
                table: "WorldCoordinate",
                column: "ContinentId",
                principalTable: "Continents",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Continents_Worlds_WorldId",
                table: "Continents");

            migrationBuilder.DropForeignKey(
                name: "FK_WorldCoordinate_Continents_ContinentId",
                table: "WorldCoordinate");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Continents",
                table: "Continents");

            migrationBuilder.RenameTable(
                name: "Continents",
                newName: "Continent");

            migrationBuilder.RenameIndex(
                name: "IX_Continents_WorldId",
                table: "Continent",
                newName: "IX_Continent_WorldId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Continent",
                table: "Continent",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Continent_Worlds_WorldId",
                table: "Continent",
                column: "WorldId",
                principalTable: "Worlds",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_WorldCoordinate_Continent_ContinentId",
                table: "WorldCoordinate",
                column: "ContinentId",
                principalTable: "Continent",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
