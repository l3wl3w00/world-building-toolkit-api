using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Dal.Migrations
{
    /// <inheritdoc />
    public partial class Init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "World",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_World", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "World",
                columns: new[] { "Id", "Description", "Name" },
                values: new object[,]
                {
                    { new Guid("835e8891-29d3-433a-ab61-bdd50926204f"), "World 3 Description", "World 3" },
                    { new Guid("ce8cdf75-2272-4449-9862-3fae32eb732e"), "World 2 Description", "World 2" },
                    { new Guid("fadfffad-7837-4893-b1a7-003aa2c88917"), "World 1 Description", "World 1" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "World");
        }
    }
}
