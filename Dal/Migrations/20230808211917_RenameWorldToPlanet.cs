using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Dal.Migrations
{
    /// <inheritdoc />
    public partial class RenameWorldToPlanet : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Continents_Worlds_WorldId",
                table: "Continents");

            migrationBuilder.DropTable(
                name: "WorldCoordinate");

            migrationBuilder.DropTable(
                name: "Worlds");

            migrationBuilder.RenameColumn(
                name: "WorldId",
                table: "Continents",
                newName: "PlanetId");

            migrationBuilder.RenameIndex(
                name: "IX_Continents_WorldId",
                table: "Continents",
                newName: "IX_Continents_PlanetId");

            migrationBuilder.CreateTable(
                name: "PlanetCoordinate",
                columns: table => new
                {
                    ContinentId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Radius = table.Column<float>(type: "real", nullable: false),
                    Phi = table.Column<float>(type: "real", nullable: false),
                    Theta = table.Column<float>(type: "real", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlanetCoordinate", x => new { x.ContinentId, x.Id });
                    table.ForeignKey(
                        name: "FK_PlanetCoordinate_Continents_ContinentId",
                        column: x => x.ContinentId,
                        principalTable: "Continents",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Planets",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatorUsername = table.Column<string>(type: "nvarchar(256)", nullable: false),
                    Radius = table.Column<float>(type: "real", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Planets", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Planets_AspNetUsers_CreatorUsername",
                        column: x => x.CreatorUsername,
                        principalTable: "AspNetUsers",
                        principalColumn: "UserName",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Planets_CreatorUsername_Name",
                table: "Planets",
                columns: new[] { "CreatorUsername", "Name" },
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Continents_Planets_PlanetId",
                table: "Continents",
                column: "PlanetId",
                principalTable: "Planets",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Continents_Planets_PlanetId",
                table: "Continents");

            migrationBuilder.DropTable(
                name: "PlanetCoordinate");

            migrationBuilder.DropTable(
                name: "Planets");

            migrationBuilder.RenameColumn(
                name: "PlanetId",
                table: "Continents",
                newName: "WorldId");

            migrationBuilder.RenameIndex(
                name: "IX_Continents_PlanetId",
                table: "Continents",
                newName: "IX_Continents_WorldId");

            migrationBuilder.CreateTable(
                name: "WorldCoordinate",
                columns: table => new
                {
                    ContinentId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Phi = table.Column<float>(type: "real", nullable: false),
                    Radius = table.Column<float>(type: "real", nullable: false),
                    Theta = table.Column<float>(type: "real", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WorldCoordinate", x => new { x.ContinentId, x.Id });
                    table.ForeignKey(
                        name: "FK_WorldCoordinate_Continents_ContinentId",
                        column: x => x.ContinentId,
                        principalTable: "Continents",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Worlds",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatorUsername = table.Column<string>(type: "nvarchar(256)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Radius = table.Column<float>(type: "real", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Worlds", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Worlds_AspNetUsers_CreatorUsername",
                        column: x => x.CreatorUsername,
                        principalTable: "AspNetUsers",
                        principalColumn: "UserName",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Worlds_CreatorUsername_Name",
                table: "Worlds",
                columns: new[] { "CreatorUsername", "Name" },
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Continents_Worlds_WorldId",
                table: "Continents",
                column: "WorldId",
                principalTable: "Worlds",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
