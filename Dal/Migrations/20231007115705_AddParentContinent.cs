using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Dal.Migrations
{
    /// <inheritdoc />
    public partial class AddParentContinent : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "ParentContinentId",
                table: "Continents",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Continents_ParentContinentId",
                table: "Continents",
                column: "ParentContinentId");

            migrationBuilder.AddForeignKey(
                name: "FK_Continents_Continents_ParentContinentId",
                table: "Continents",
                column: "ParentContinentId",
                principalTable: "Continents",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Continents_Continents_ParentContinentId",
                table: "Continents");

            migrationBuilder.DropIndex(
                name: "IX_Continents_ParentContinentId",
                table: "Continents");

            migrationBuilder.DropColumn(
                name: "ParentContinentId",
                table: "Continents");
        }
    }
}
