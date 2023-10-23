using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Dal.Migrations
{
    /// <inheritdoc />
    public partial class RefactorCalendar : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Planets_Calendar_CalendarId",
                table: "Planets");

            migrationBuilder.DropIndex(
                name: "IX_Planets_CalendarId",
                table: "Planets");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Calendar",
                table: "Calendar");

            migrationBuilder.DropColumn(
                name: "End_Day",
                table: "Events");

            migrationBuilder.DropColumn(
                name: "End_Year",
                table: "Events");

            migrationBuilder.DropColumn(
                name: "Start_Day",
                table: "Events");

            migrationBuilder.DropColumn(
                name: "Start_Year",
                table: "Events");

            migrationBuilder.DropColumn(
                name: "DayLength",
                table: "Calendar");

            migrationBuilder.RenameTable(
                name: "Calendar",
                newName: "Calendars");

            migrationBuilder.RenameColumn(
                name: "NumberOfDaysInYear",
                table: "Calendars",
                newName: "FirstYear");

            migrationBuilder.AddColumn<TimeSpan>(
                name: "DayLength",
                table: "Planets",
                type: "time",
                nullable: false,
                defaultValue: new TimeSpan(0, 0, 0, 0, 0));

            migrationBuilder.AddColumn<int>(
                name: "NumberOfDaysInYear",
                table: "Planets",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<TimeSpan>(
                name: "End_Time",
                table: "Events",
                type: "time",
                nullable: false,
                defaultValue: new TimeSpan(0, 0, 0, 0, 0));

            migrationBuilder.AddColumn<TimeSpan>(
                name: "Start_Time",
                table: "Events",
                type: "time",
                nullable: false,
                defaultValue: new TimeSpan(0, 0, 0, 0, 0));

            migrationBuilder.AddPrimaryKey(
                name: "PK_Calendars",
                table: "Calendars",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "YearPhase",
                columns: table => new
                {
                    CalendarId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NumberOfDays = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_YearPhase", x => new { x.CalendarId, x.Id });
                    table.ForeignKey(
                        name: "FK_YearPhase_Calendars_CalendarId",
                        column: x => x.CalendarId,
                        principalTable: "Calendars",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Planets_CalendarId",
                table: "Planets",
                column: "CalendarId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Planets_Calendars_CalendarId",
                table: "Planets",
                column: "CalendarId",
                principalTable: "Calendars",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Planets_Calendars_CalendarId",
                table: "Planets");

            migrationBuilder.DropTable(
                name: "YearPhase");

            migrationBuilder.DropIndex(
                name: "IX_Planets_CalendarId",
                table: "Planets");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Calendars",
                table: "Calendars");

            migrationBuilder.DropColumn(
                name: "DayLength",
                table: "Planets");

            migrationBuilder.DropColumn(
                name: "NumberOfDaysInYear",
                table: "Planets");

            migrationBuilder.DropColumn(
                name: "End_Time",
                table: "Events");

            migrationBuilder.DropColumn(
                name: "Start_Time",
                table: "Events");

            migrationBuilder.RenameTable(
                name: "Calendars",
                newName: "Calendar");

            migrationBuilder.RenameColumn(
                name: "FirstYear",
                table: "Calendar",
                newName: "NumberOfDaysInYear");

            migrationBuilder.AddColumn<long>(
                name: "End_Day",
                table: "Events",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<int>(
                name: "End_Year",
                table: "Events",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<long>(
                name: "Start_Day",
                table: "Events",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<int>(
                name: "Start_Year",
                table: "Events",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "DayLength",
                table: "Calendar",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Calendar",
                table: "Calendar",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_Planets_CalendarId",
                table: "Planets",
                column: "CalendarId");

            migrationBuilder.AddForeignKey(
                name: "FK_Planets_Calendar_CalendarId",
                table: "Planets",
                column: "CalendarId",
                principalTable: "Calendar",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
