using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Dal.Migrations
{
    /// <inheritdoc />
    public partial class IntroduceColor : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Color_A",
                table: "Regions",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Color_B",
                table: "Regions",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Color_G",
                table: "Regions",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Color_R",
                table: "Regions",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "AntiLandColor_A",
                table: "Planets",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "AntiLandColor_B",
                table: "Planets",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "AntiLandColor_G",
                table: "Planets",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "AntiLandColor_R",
                table: "Planets",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "LandColor_A",
                table: "Planets",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "LandColor_B",
                table: "Planets",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "LandColor_G",
                table: "Planets",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "LandColor_R",
                table: "Planets",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Color_A",
                table: "Regions");

            migrationBuilder.DropColumn(
                name: "Color_B",
                table: "Regions");

            migrationBuilder.DropColumn(
                name: "Color_G",
                table: "Regions");

            migrationBuilder.DropColumn(
                name: "Color_R",
                table: "Regions");

            migrationBuilder.DropColumn(
                name: "AntiLandColor_A",
                table: "Planets");

            migrationBuilder.DropColumn(
                name: "AntiLandColor_B",
                table: "Planets");

            migrationBuilder.DropColumn(
                name: "AntiLandColor_G",
                table: "Planets");

            migrationBuilder.DropColumn(
                name: "AntiLandColor_R",
                table: "Planets");

            migrationBuilder.DropColumn(
                name: "LandColor_A",
                table: "Planets");

            migrationBuilder.DropColumn(
                name: "LandColor_B",
                table: "Planets");

            migrationBuilder.DropColumn(
                name: "LandColor_G",
                table: "Planets");

            migrationBuilder.DropColumn(
                name: "LandColor_R",
                table: "Planets");
        }
    }
}
