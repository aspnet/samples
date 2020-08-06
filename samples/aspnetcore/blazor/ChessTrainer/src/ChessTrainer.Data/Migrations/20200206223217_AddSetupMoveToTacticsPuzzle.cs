using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MjrChess.Trainer.Data.Migrations
{
    /// <summary>
    /// Migration for adding SetupMove to tactices puzzles.
    /// </summary>
    public partial class AddSetupMoveToTacticsPuzzle : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "GameUrl",
                table: "Puzzles",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "IncorrectPiecePromotedTo",
                table: "Puzzles",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "PiecePromotedTo",
                table: "Puzzles",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SetupMovedFrom",
                table: "Puzzles",
                nullable: false,
                defaultValue: string.Empty);

            migrationBuilder.AddColumn<string>(
                name: "SetupMovedTo",
                table: "Puzzles",
                nullable: false,
                defaultValue: string.Empty);

            migrationBuilder.AddColumn<int>(
                name: "SetupPieceMoved",
                table: "Puzzles",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "SetupPiecePromotedTo",
                table: "Puzzles",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "Players",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedDate", "LastModifiedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2020, 2, 6, 17, 32, 16, 530, DateTimeKind.Unspecified).AddTicks(631), new TimeSpan(0, -5, 0, 0, 0)), new DateTimeOffset(new DateTime(2020, 2, 6, 17, 32, 16, 533, DateTimeKind.Unspecified).AddTicks(9085), new TimeSpan(0, -5, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                table: "Players",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedDate", "LastModifiedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2020, 2, 6, 17, 32, 16, 534, DateTimeKind.Unspecified).AddTicks(210), new TimeSpan(0, -5, 0, 0, 0)), new DateTimeOffset(new DateTime(2020, 2, 6, 17, 32, 16, 534, DateTimeKind.Unspecified).AddTicks(253), new TimeSpan(0, -5, 0, 0, 0)) });

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional (suppressed because MigrationBuilder.InsertData expects an object[,])
            var playersSeedData = new object[,]
                {
                    { 3, new DateTimeOffset(new DateTime(2020, 2, 6, 17, 32, 16, 534, DateTimeKind.Unspecified).AddTicks(272), new TimeSpan(0, -5, 0, 0, 0)), new DateTimeOffset(new DateTime(2020, 2, 6, 17, 32, 16, 534, DateTimeKind.Unspecified).AddTicks(279), new TimeSpan(0, -5, 0, 0, 0)), "Vini700", 0 },
                    { 4, new DateTimeOffset(new DateTime(2020, 2, 6, 17, 32, 16, 534, DateTimeKind.Unspecified).AddTicks(287), new TimeSpan(0, -5, 0, 0, 0)), new DateTimeOffset(new DateTime(2020, 2, 6, 17, 32, 16, 534, DateTimeKind.Unspecified).AddTicks(295), new TimeSpan(0, -5, 0, 0, 0)), "aupoil", 0 },
                    { 5, new DateTimeOffset(new DateTime(2020, 2, 6, 17, 32, 16, 534, DateTimeKind.Unspecified).AddTicks(302), new TimeSpan(0, -5, 0, 0, 0)), new DateTimeOffset(new DateTime(2020, 2, 6, 17, 32, 16, 534, DateTimeKind.Unspecified).AddTicks(309), new TimeSpan(0, -5, 0, 0, 0)), "toskekg", 0 },
                    { 6, new DateTimeOffset(new DateTime(2020, 2, 6, 17, 32, 16, 534, DateTimeKind.Unspecified).AddTicks(317), new TimeSpan(0, -5, 0, 0, 0)), new DateTimeOffset(new DateTime(2020, 2, 6, 17, 32, 16, 534, DateTimeKind.Unspecified).AddTicks(323), new TimeSpan(0, -5, 0, 0, 0)), "wolfwolf", 0 }
                };
#pragma warning restore CA1814 // Prefer jagged arrays over multidimensional

            migrationBuilder.InsertData(
                table: "Players",
                columns: new[] { "Id", "CreatedDate", "LastModifiedDate", "Name", "Site" },
                values: playersSeedData);

            migrationBuilder.UpdateData(
                table: "Puzzles",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedDate", "IncorrectMovedFrom", "IncorrectMovedTo", "IncorrectPieceMoved", "LastModifiedDate", "Position", "SetupMovedFrom", "SetupMovedTo", "SetupPieceMoved" },
                values: new object?[] { new DateTimeOffset(new DateTime(2020, 2, 6, 17, 32, 16, 535, DateTimeKind.Unspecified).AddTicks(8352), new TimeSpan(0, -5, 0, 0, 0)), null, null, null, new DateTimeOffset(new DateTime(2020, 2, 6, 17, 32, 16, 535, DateTimeKind.Unspecified).AddTicks(8428), new TimeSpan(0, -5, 0, 0, 0)), "rnbqk1nr/pppp1ppp/8/2b1p3/2B1P3/5Q2/PPPP1PPP/RNB1K1NR b KQkq - 3 3", "b8", "c6", 10 });

            migrationBuilder.InsertData(
                table: "Puzzles",
                columns: new[] { "Id", "BlackPlayerId", "CreatedDate", "GameDate", "GameUrl", "IncorrectMovedFrom", "IncorrectMovedTo", "IncorrectPieceMoved", "IncorrectPiecePromotedTo", "LastModifiedDate", "MovedFrom", "MovedTo", "PieceMoved", "PiecePromotedTo", "Position", "SetupMovedFrom", "SetupMovedTo", "SetupPieceMoved", "SetupPiecePromotedTo", "Site", "WhitePlayerId" },
                values: new object?[] { 2, 4, new DateTimeOffset(new DateTime(2020, 2, 6, 17, 32, 16, 536, DateTimeKind.Unspecified).AddTicks(991), new TimeSpan(0, -5, 0, 0, 0)), new DateTimeOffset(new DateTime(2016, 8, 8, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "https://lichess.org/3piQphpY", null, null, null, null, new DateTimeOffset(new DateTime(2020, 2, 6, 17, 32, 16, 536, DateTimeKind.Unspecified).AddTicks(1029), new TimeSpan(0, -5, 0, 0, 0)), "c6", "a5", 10, null, "r3r1k1/ppp2pp1/2n4p/3q4/3Pb3/B1P2N1P/P2Q1PP1/R3R1K1 w - - 4 16", "e1", "e3", 2, null, "lichess.org", 3 });

            migrationBuilder.InsertData(
                table: "Puzzles",
                columns: new[] { "Id", "BlackPlayerId", "CreatedDate", "GameDate", "GameUrl", "IncorrectMovedFrom", "IncorrectMovedTo", "IncorrectPieceMoved", "IncorrectPiecePromotedTo", "LastModifiedDate", "MovedFrom", "MovedTo", "PieceMoved", "PiecePromotedTo", "Position", "SetupMovedFrom", "SetupMovedTo", "SetupPieceMoved", "SetupPiecePromotedTo", "Site", "WhitePlayerId" },
                values: new object?[] { 3, 6, new DateTimeOffset(new DateTime(2020, 2, 6, 17, 32, 16, 536, DateTimeKind.Unspecified).AddTicks(3265), new TimeSpan(0, -5, 0, 0, 0)), new DateTimeOffset(new DateTime(2016, 10, 7, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "https://lichess.org/HjVhr1Dn", "f8", "f5", 8, null, new DateTimeOffset(new DateTime(2020, 2, 6, 17, 32, 16, 536, DateTimeKind.Unspecified).AddTicks(3298), new TimeSpan(0, -5, 0, 0, 0)), "e7", "b4", 9, null, "r2q1rk1/1pp1b1pp/p7/4pp2/2PnB1P1/3PB2P/PP1Q1P2/R3K2R w KQ - 0 15", "g4", "f5", 5, null, "lichess.org", 5 });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Puzzles",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Puzzles",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Players",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Players",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Players",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Players",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DropColumn(
                name: "GameUrl",
                table: "Puzzles");

            migrationBuilder.DropColumn(
                name: "IncorrectPiecePromotedTo",
                table: "Puzzles");

            migrationBuilder.DropColumn(
                name: "PiecePromotedTo",
                table: "Puzzles");

            migrationBuilder.DropColumn(
                name: "SetupMovedFrom",
                table: "Puzzles");

            migrationBuilder.DropColumn(
                name: "SetupMovedTo",
                table: "Puzzles");

            migrationBuilder.DropColumn(
                name: "SetupPieceMoved",
                table: "Puzzles");

            migrationBuilder.DropColumn(
                name: "SetupPiecePromotedTo",
                table: "Puzzles");

            migrationBuilder.UpdateData(
                table: "Players",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedDate", "LastModifiedDate" },
                values: new object?[] { null, null });

            migrationBuilder.UpdateData(
                table: "Players",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedDate", "LastModifiedDate" },
                values: new object?[] { null, null });

            migrationBuilder.UpdateData(
                table: "Puzzles",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedDate", "IncorrectMovedFrom", "IncorrectMovedTo", "IncorrectPieceMoved", "LastModifiedDate", "Position" },
                values: new object[] { new DateTimeOffset(new DateTime(2020, 1, 31, 15, 33, 20, 398, DateTimeKind.Unspecified).AddTicks(1989), new TimeSpan(0, -5, 0, 0, 0)), "d2", "d4", 5, new DateTimeOffset(new DateTime(2020, 1, 31, 15, 33, 20, 402, DateTimeKind.Unspecified).AddTicks(4953), new TimeSpan(0, -5, 0, 0, 0)), "r1bqk1nr/pppp1ppp/2n5/2b1p3/2B1P3/5Q2/PPPP1PPP/RNB1K1NR w KQkq - 4 4" });
        }
    }
}
