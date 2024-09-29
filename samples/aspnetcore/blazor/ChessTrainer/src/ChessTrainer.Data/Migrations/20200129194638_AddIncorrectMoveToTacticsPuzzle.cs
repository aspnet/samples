using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MjrChess.Trainer.Data.Migrations
{
    public partial class AddIncorrectMoveToTacticsPuzzle : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "UserSettings",
                nullable: false,
                defaultValue: string.Empty);

            migrationBuilder.AddColumn<string>(
                name: "IncorrectMovedFrom",
                table: "Puzzles",
                nullable: false,
                defaultValue: string.Empty);

            migrationBuilder.AddColumn<string>(
                name: "IncorrectMovedTo",
                table: "Puzzles",
                nullable: false,
                defaultValue: string.Empty);

            migrationBuilder.AddColumn<int>(
                name: "IncorrectPieceMoved",
                table: "Puzzles",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.UpdateData(
                table: "Puzzles",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedDate", "IncorrectMovedFrom", "IncorrectMovedTo", "IncorrectPieceMoved", "LastModifiedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2020, 1, 29, 14, 46, 38, 46, DateTimeKind.Unspecified).AddTicks(9553), new TimeSpan(0, -5, 0, 0, 0)), "d2", "d4", 5, new DateTimeOffset(new DateTime(2020, 1, 29, 14, 46, 38, 51, DateTimeKind.Unspecified).AddTicks(766), new TimeSpan(0, -5, 0, 0, 0)) });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UserId",
                table: "UserSettings");

            migrationBuilder.DropColumn(
                name: "IncorrectMovedFrom",
                table: "Puzzles");

            migrationBuilder.DropColumn(
                name: "IncorrectMovedTo",
                table: "Puzzles");

            migrationBuilder.DropColumn(
                name: "IncorrectPieceMoved",
                table: "Puzzles");

            migrationBuilder.UpdateData(
                table: "Puzzles",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedDate", "LastModifiedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2020, 1, 28, 15, 8, 36, 513, DateTimeKind.Unspecified).AddTicks(2138), new TimeSpan(0, -5, 0, 0, 0)), new DateTimeOffset(new DateTime(2020, 1, 28, 15, 8, 36, 517, DateTimeKind.Unspecified).AddTicks(5938), new TimeSpan(0, -5, 0, 0, 0)) });
        }
    }
}
