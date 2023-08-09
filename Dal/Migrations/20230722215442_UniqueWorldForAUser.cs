using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Dal.Migrations
{
    /// <inheritdoc />
    public partial class UniqueWorldForAUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Worlds_AspNetUsers_Username",
                table: "Worlds");

            migrationBuilder.DropIndex(
                name: "IX_Worlds_Username",
                table: "Worlds");

            migrationBuilder.RenameColumn(
                name: "Username",
                table: "Worlds",
                newName: "CreatorUsername");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Worlds",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.CreateIndex(
                name: "IX_Worlds_CreatorUsername_Name",
                table: "Worlds",
                columns: new[] { "CreatorUsername", "Name" },
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Worlds_AspNetUsers_CreatorUsername",
                table: "Worlds",
                column: "CreatorUsername",
                principalTable: "AspNetUsers",
                principalColumn: "UserName",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Worlds_AspNetUsers_CreatorUsername",
                table: "Worlds");

            migrationBuilder.DropIndex(
                name: "IX_Worlds_CreatorUsername_Name",
                table: "Worlds");

            migrationBuilder.RenameColumn(
                name: "CreatorUsername",
                table: "Worlds",
                newName: "Username");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Worlds",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.CreateIndex(
                name: "IX_Worlds_Username",
                table: "Worlds",
                column: "Username");

            migrationBuilder.AddForeignKey(
                name: "FK_Worlds_AspNetUsers_Username",
                table: "Worlds",
                column: "Username",
                principalTable: "AspNetUsers",
                principalColumn: "UserName",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
