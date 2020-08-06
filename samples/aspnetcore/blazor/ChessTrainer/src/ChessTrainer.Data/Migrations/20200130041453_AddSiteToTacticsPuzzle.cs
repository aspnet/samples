using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MjrChess.Trainer.Data.Migrations
{
    public partial class AddSiteToTacticsPuzzle : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Players_UserSettings_UserSettingsId",
                table: "Players");

            migrationBuilder.DropIndex(
                name: "IX_Players_UserSettingsId",
                table: "Players");

            migrationBuilder.DropColumn(
                name: "UserSettingsId",
                table: "Players");

            migrationBuilder.AddColumn<string>(
                name: "Site",
                table: "Puzzles",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "UserSettingsXPlayers",
                columns: table => new
                {
                    UserSettingsId = table.Column<int>(nullable: false),
                    PlayerId = table.Column<int>(nullable: false)
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

            migrationBuilder.UpdateData(
                table: "Puzzles",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedDate", "LastModifiedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2020, 1, 29, 23, 14, 52, 496, DateTimeKind.Unspecified).AddTicks(3665), new TimeSpan(0, -5, 0, 0, 0)), new DateTimeOffset(new DateTime(2020, 1, 29, 23, 14, 52, 500, DateTimeKind.Unspecified).AddTicks(8775), new TimeSpan(0, -5, 0, 0, 0)) });

            migrationBuilder.CreateIndex(
                name: "IX_UserSettingsXPlayers_PlayerId",
                table: "UserSettingsXPlayers",
                column: "PlayerId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserSettingsXPlayers");

            migrationBuilder.DropColumn(
                name: "Site",
                table: "Puzzles");

            migrationBuilder.AddColumn<int>(
                name: "UserSettingsId",
                table: "Players",
                type: "int",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "Puzzles",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedDate", "LastModifiedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2020, 1, 29, 14, 46, 38, 46, DateTimeKind.Unspecified).AddTicks(9553), new TimeSpan(0, -5, 0, 0, 0)), new DateTimeOffset(new DateTime(2020, 1, 29, 14, 46, 38, 51, DateTimeKind.Unspecified).AddTicks(766), new TimeSpan(0, -5, 0, 0, 0)) });

            migrationBuilder.CreateIndex(
                name: "IX_Players_UserSettingsId",
                table: "Players",
                column: "UserSettingsId");

            migrationBuilder.AddForeignKey(
                name: "FK_Players_UserSettings_UserSettingsId",
                table: "Players",
                column: "UserSettingsId",
                principalTable: "UserSettings",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
