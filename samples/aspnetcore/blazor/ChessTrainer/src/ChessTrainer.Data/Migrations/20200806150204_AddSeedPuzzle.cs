using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MjrChess.Trainer.Data.Migrations
{
    /// <summary>
    /// Migration for cleaning up unused tables and adding additional seed puzzle.
    /// </summary>
    public partial class AddSeedPuzzle : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserSettingsXPlayers");

            migrationBuilder.DropTable(
                name: "Players");

            migrationBuilder.DropTable(
                name: "UserSettings");

            migrationBuilder.DropColumn(
                name: "AssociatedPlayerId",
                table: "Puzzles");

            migrationBuilder.UpdateData(
                table: "Puzzles",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "BlackPlayerName", "CreatedDate", "LastModifiedDate" },
                values: new object[] { "Noobie", new DateTimeOffset(new DateTime(2020, 8, 6, 11, 2, 3, 198, DateTimeKind.Unspecified).AddTicks(7643), new TimeSpan(0, -4, 0, 0, 0)), new DateTimeOffset(new DateTime(2020, 8, 6, 11, 2, 3, 198, DateTimeKind.Unspecified).AddTicks(7800), new TimeSpan(0, -4, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                table: "Puzzles",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedDate", "LastModifiedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2020, 8, 6, 11, 2, 3, 199, DateTimeKind.Unspecified).AddTicks(1347), new TimeSpan(0, -4, 0, 0, 0)), new DateTimeOffset(new DateTime(2020, 8, 6, 11, 2, 3, 199, DateTimeKind.Unspecified).AddTicks(1398), new TimeSpan(0, -4, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                table: "Puzzles",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedDate", "LastModifiedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2020, 8, 6, 11, 2, 3, 199, DateTimeKind.Unspecified).AddTicks(4139), new TimeSpan(0, -4, 0, 0, 0)), new DateTimeOffset(new DateTime(2020, 8, 6, 11, 2, 3, 199, DateTimeKind.Unspecified).AddTicks(4177), new TimeSpan(0, -4, 0, 0, 0)) });

            migrationBuilder.InsertData(
                table: "Puzzles",
                columns: new[] { "Id", "BlackPlayerName", "CreatedDate", "GameDate", "GameUrl", "IncorrectMovedFrom", "IncorrectMovedTo", "IncorrectPiecePromotedTo", "LastModifiedDate", "MovedFrom", "MovedTo", "PiecePromotedTo", "Position", "SetupMovedFrom", "SetupMovedTo", "SetupPiecePromotedTo", "Site", "WhitePlayerName" },
                values: new object?[] { 4, "mjrousos", new DateTimeOffset(new DateTime(2020, 8, 6, 11, 2, 3, 199, DateTimeKind.Unspecified).AddTicks(7176), new TimeSpan(0, -4, 0, 0, 0)), new DateTimeOffset(new DateTime(2019, 10, 26, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "https://lichess.org/ABBg3RuE", "g4", "a4", null, new DateTimeOffset(new DateTime(2020, 8, 6, 11, 2, 3, 199, DateTimeKind.Unspecified).AddTicks(7220), new TimeSpan(0, -4, 0, 0, 0)), "b8", "b1", null, "krr5/p6p/2pQ4/3pp3/N5q1/6P1/P1P2PBP/3R2K1 w - - 14 31", "d1", "d5", null, "lichess.org", "fucilaco" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Puzzles",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.AddColumn<int>(
                name: "AssociatedPlayerId",
                table: "Puzzles",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "Players",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    LastModifiedDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Site = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Players", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UserSettings",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    LastModifiedDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    UserId = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserSettings", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UserSettingsXPlayers",
                columns: table => new
                {
                    UserSettingsId = table.Column<int>(type: "int", nullable: false),
                    PlayerId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserSettingsXPlayers", x => new { x.UserSettingsId, x.PlayerId });
                    table.ForeignKey(
                        name: "FK_UserSettingsXPlayers_Players_PlayerId",
                        column: x => x.PlayerId,
                        principalTable: "Players",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserSettingsXPlayers_UserSettings_UserSettingsId",
                        column: x => x.UserSettingsId,
                        principalTable: "UserSettings",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional (suppressed because MigrationBuilder.InsertData expects an object[,])
            migrationBuilder.InsertData(
                table: "Players",
                columns: new[] { "Id", "CreatedDate", "LastModifiedDate", "Name", "Site" },
                values: new object[,]
                {
                    { 1, new DateTimeOffset(new DateTime(2020, 5, 26, 17, 43, 1, 101, DateTimeKind.Unspecified).AddTicks(5281), new TimeSpan(0, -4, 0, 0, 0)), new DateTimeOffset(new DateTime(2020, 5, 26, 17, 43, 1, 101, DateTimeKind.Unspecified).AddTicks(6425), new TimeSpan(0, -4, 0, 0, 0)), "aupoil", 0 },
                    { 2, new DateTimeOffset(new DateTime(2020, 5, 26, 17, 43, 1, 101, DateTimeKind.Unspecified).AddTicks(7443), new TimeSpan(0, -4, 0, 0, 0)), new DateTimeOffset(new DateTime(2020, 5, 26, 17, 43, 1, 101, DateTimeKind.Unspecified).AddTicks(7489), new TimeSpan(0, -4, 0, 0, 0)), "toskekg", 0 }
                });
#pragma warning restore CA1814 // Prefer jagged arrays over multidimensional

            migrationBuilder.UpdateData(
                table: "Puzzles",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "BlackPlayerName", "CreatedDate", "LastModifiedDate" },
                values: new object?[] { null, new DateTimeOffset(new DateTime(2020, 5, 26, 17, 43, 1, 103, DateTimeKind.Unspecified).AddTicks(7202), new TimeSpan(0, -4, 0, 0, 0)), new DateTimeOffset(new DateTime(2020, 5, 26, 17, 43, 1, 103, DateTimeKind.Unspecified).AddTicks(7258), new TimeSpan(0, -4, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                table: "Puzzles",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "AssociatedPlayerId", "CreatedDate", "LastModifiedDate" },
                values: new object[] { 1, new DateTimeOffset(new DateTime(2020, 5, 26, 17, 43, 1, 104, DateTimeKind.Unspecified).AddTicks(295), new TimeSpan(0, -4, 0, 0, 0)), new DateTimeOffset(new DateTime(2020, 5, 26, 17, 43, 1, 104, DateTimeKind.Unspecified).AddTicks(341), new TimeSpan(0, -4, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                table: "Puzzles",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "AssociatedPlayerId", "CreatedDate", "LastModifiedDate" },
                values: new object[] { 2, new DateTimeOffset(new DateTime(2020, 5, 26, 17, 43, 1, 104, DateTimeKind.Unspecified).AddTicks(3379), new TimeSpan(0, -4, 0, 0, 0)), new DateTimeOffset(new DateTime(2020, 5, 26, 17, 43, 1, 104, DateTimeKind.Unspecified).AddTicks(3418), new TimeSpan(0, -4, 0, 0, 0)) });

            migrationBuilder.CreateIndex(
                name: "IX_UserSettingsXPlayers_PlayerId",
                table: "UserSettingsXPlayers",
                column: "PlayerId");
        }
    }
}
