using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MjrChess.Trainer.Data.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "UserSettings",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedDate = table.Column<DateTimeOffset>(nullable: true),
                    LastModifiedDate = table.Column<DateTimeOffset>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserSettings", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Players",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedDate = table.Column<DateTimeOffset>(nullable: true),
                    LastModifiedDate = table.Column<DateTimeOffset>(nullable: true),
                    Name = table.Column<string>(nullable: false),
                    Site = table.Column<int>(nullable: false),
                    UserSettingsId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Players", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Players_UserSettings_UserSettingsId",
                        column: x => x.UserSettingsId,
                        principalTable: "UserSettings",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Puzzles",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedDate = table.Column<DateTimeOffset>(nullable: true),
                    LastModifiedDate = table.Column<DateTimeOffset>(nullable: true),
                    Position = table.Column<string>(nullable: false),
                    PieceMoved = table.Column<int>(nullable: false),
                    MovedFrom = table.Column<string>(nullable: false),
                    MovedTo = table.Column<string>(nullable: false),
                    WhitePlayerId = table.Column<int>(nullable: true),
                    BlackPlayerId = table.Column<int>(nullable: true),
                    GameDate = table.Column<DateTimeOffset>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Puzzles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Puzzles_Players_BlackPlayerId",
                        column: x => x.BlackPlayerId,
                        principalTable: "Players",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Puzzles_Players_WhitePlayerId",
                        column: x => x.WhitePlayerId,
                        principalTable: "Players",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PuzzleHistories",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedDate = table.Column<DateTimeOffset>(nullable: true),
                    LastModifiedDate = table.Column<DateTimeOffset>(nullable: true),
                    UserId = table.Column<string>(nullable: false),
                    PuzzleId = table.Column<int>(nullable: false),
                    Solved = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PuzzleHistories", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PuzzleHistories_Puzzles_PuzzleId",
                        column: x => x.PuzzleId,
                        principalTable: "Puzzles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Players",
                columns: new[] { "Id", "CreatedDate", "LastModifiedDate", "Name", "Site", "UserSettingsId" },
                values: new object?[] { 1, null, null, "Hustler", 2, null });

            migrationBuilder.InsertData(
                table: "Players",
                columns: new[] { "Id", "CreatedDate", "LastModifiedDate", "Name", "Site", "UserSettingsId" },
                values: new object?[] { 2, null, null, "Noobie", 2, null });

            migrationBuilder.InsertData(
                table: "Puzzles",
                columns: new[] { "Id", "BlackPlayerId", "CreatedDate", "GameDate", "LastModifiedDate", "MovedFrom", "MovedTo", "PieceMoved", "Position", "WhitePlayerId" },
                values: new object?[] { 1, 2, new DateTimeOffset(new DateTime(2020, 1, 28, 15, 8, 36, 513, DateTimeKind.Unspecified).AddTicks(2138), new TimeSpan(0, -5, 0, 0, 0)), new DateTimeOffset(new DateTime(2015, 2, 7, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2020, 1, 28, 15, 8, 36, 517, DateTimeKind.Unspecified).AddTicks(5938), new TimeSpan(0, -5, 0, 0, 0)), "f3", "f7", 1, "r1bqk1nr/pppp1ppp/2n5/2b1p3/2B1P3/5Q2/PPPP1PPP/RNB1K1NR w KQkq - 4 4", 1 });

            migrationBuilder.CreateIndex(
                name: "IX_Players_UserSettingsId",
                table: "Players",
                column: "UserSettingsId");

            migrationBuilder.CreateIndex(
                name: "IX_PuzzleHistories_PuzzleId",
                table: "PuzzleHistories",
                column: "PuzzleId");

            migrationBuilder.CreateIndex(
                name: "IX_Puzzles_BlackPlayerId",
                table: "Puzzles",
                column: "BlackPlayerId");

            migrationBuilder.CreateIndex(
                name: "IX_Puzzles_WhitePlayerId",
                table: "Puzzles",
                column: "WhitePlayerId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PuzzleHistories");

            migrationBuilder.DropTable(
                name: "Puzzles");

            migrationBuilder.DropTable(
                name: "Players");

            migrationBuilder.DropTable(
                name: "UserSettings");
        }
    }
}
