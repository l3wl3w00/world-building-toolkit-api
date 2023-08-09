using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Dal.Migrations
{
    /// <inheritdoc />
    public partial class ConnectUserAndWorld : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "UserId",
                table: "Worlds",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: Guid.Empty);

            migrationBuilder.CreateIndex(
                name: "IX_Worlds_UserId",
                table: "Worlds",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Worlds_AspNetUsers_UserId",
                table: "Worlds",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Worlds_AspNetUsers_UserId",
                table: "Worlds");

            migrationBuilder.DropIndex(
                name: "IX_Worlds_UserId",
                table: "Worlds");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Worlds");

            migrationBuilder.InsertData(
                table: "Worlds",
                columns: new[] { "Id", "Description", "Name" },
                values: new object[,]
                {
                    { new Guid("3011d5e5-8987-4632-870c-93849d654b7e"), "World 2 Description", "World 2" },
                    { new Guid("72292e48-5482-422a-bd55-954bd4c9b694"), "World 1 Description", "World 1" },
                    { new Guid("8401de86-566d-4df8-9717-50429d4c957f"), "World 3 Description", "World 3" }
                });
        }
    }
}
