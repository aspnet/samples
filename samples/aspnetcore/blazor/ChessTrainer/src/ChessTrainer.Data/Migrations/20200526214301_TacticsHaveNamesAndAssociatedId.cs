using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MjrChess.Trainer.Data.Migrations
{
    public partial class TacticsHaveNamesAndAssociatedId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Puzzles_Players_BlackPlayerId",
                table: "Puzzles");

            migrationBuilder.DropForeignKey(
                name: "FK_Puzzles_Players_WhitePlayerId",
                table: "Puzzles");

            migrationBuilder.DropIndex(
                name: "IX_Puzzles_BlackPlayerId",
                table: "Puzzles");

            migrationBuilder.DropIndex(
                name: "IX_Puzzles_WhitePlayerId",
                table: "Puzzles");

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
                name: "BlackPlayerId",
                table: "Puzzles");

            migrationBuilder.DropColumn(
                name: "WhitePlayerId",
                table: "Puzzles");

            migrationBuilder.AddColumn<int>(
                name: "AssociatedPlayerId",
                table: "Puzzles",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "BlackPlayerName",
                table: "Puzzles",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "WhitePlayerName",
                table: "Puzzles",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "Players",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedDate", "LastModifiedDate", "Name", "Site" },
                values: new object[] { new DateTimeOffset(new DateTime(2020, 5, 26, 17, 43, 1, 101, DateTimeKind.Unspecified).AddTicks(5281), new TimeSpan(0, -4, 0, 0, 0)), new DateTimeOffset(new DateTime(2020, 5, 26, 17, 43, 1, 101, DateTimeKind.Unspecified).AddTicks(6425), new TimeSpan(0, -4, 0, 0, 0)), "aupoil", 0 });

            migrationBuilder.UpdateData(
                table: "Players",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedDate", "LastModifiedDate", "Name", "Site" },
                values: new object[] { new DateTimeOffset(new DateTime(2020, 5, 26, 17, 43, 1, 101, DateTimeKind.Unspecified).AddTicks(7443), new TimeSpan(0, -4, 0, 0, 0)), new DateTimeOffset(new DateTime(2020, 5, 26, 17, 43, 1, 101, DateTimeKind.Unspecified).AddTicks(7489), new TimeSpan(0, -4, 0, 0, 0)), "toskekg", 0 });

            migrationBuilder.UpdateData(
                table: "Puzzles",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedDate", "LastModifiedDate", "WhitePlayerName" },
                values: new object[] { new DateTimeOffset(new DateTime(2020, 5, 26, 17, 43, 1, 103, DateTimeKind.Unspecified).AddTicks(7202), new TimeSpan(0, -4, 0, 0, 0)), new DateTimeOffset(new DateTime(2020, 5, 26, 17, 43, 1, 103, DateTimeKind.Unspecified).AddTicks(7258), new TimeSpan(0, -4, 0, 0, 0)), "Hustler" });

            migrationBuilder.UpdateData(
                table: "Puzzles",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "AssociatedPlayerId", "BlackPlayerName", "CreatedDate", "LastModifiedDate", "WhitePlayerName" },
                values: new object[] { 1, "aupoil", new DateTimeOffset(new DateTime(2020, 5, 26, 17, 43, 1, 104, DateTimeKind.Unspecified).AddTicks(295), new TimeSpan(0, -4, 0, 0, 0)), new DateTimeOffset(new DateTime(2020, 5, 26, 17, 43, 1, 104, DateTimeKind.Unspecified).AddTicks(341), new TimeSpan(0, -4, 0, 0, 0)), "Vini700" });

            migrationBuilder.UpdateData(
                table: "Puzzles",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "AssociatedPlayerId", "BlackPlayerName", "CreatedDate", "LastModifiedDate", "WhitePlayerName" },
                values: new object[] { 2, "wolfwolf", new DateTimeOffset(new DateTime(2020, 5, 26, 17, 43, 1, 104, DateTimeKind.Unspecified).AddTicks(3379), new TimeSpan(0, -4, 0, 0, 0)), new DateTimeOffset(new DateTime(2020, 5, 26, 17, 43, 1, 104, DateTimeKind.Unspecified).AddTicks(3418), new TimeSpan(0, -4, 0, 0, 0)), "toskekg" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AssociatedPlayerId",
                table: "Puzzles");

            migrationBuilder.DropColumn(
                name: "BlackPlayerName",
                table: "Puzzles");

            migrationBuilder.DropColumn(
                name: "WhitePlayerName",
                table: "Puzzles");

            migrationBuilder.AddColumn<int>(
                name: "BlackPlayerId",
                table: "Puzzles",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "WhitePlayerId",
                table: "Puzzles",
                type: "int",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "Players",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedDate", "LastModifiedDate", "Name", "Site" },
                values: new object[] { new DateTimeOffset(new DateTime(2020, 5, 26, 16, 56, 47, 677, DateTimeKind.Unspecified).AddTicks(5170), new TimeSpan(0, -4, 0, 0, 0)), new DateTimeOffset(new DateTime(2020, 5, 26, 16, 56, 47, 677, DateTimeKind.Unspecified).AddTicks(6434), new TimeSpan(0, -4, 0, 0, 0)), "Hustler", 2 });

            migrationBuilder.UpdateData(
                table: "Players",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedDate", "LastModifiedDate", "Name", "Site" },
                values: new object[] { new DateTimeOffset(new DateTime(2020, 5, 26, 16, 56, 47, 677, DateTimeKind.Unspecified).AddTicks(7561), new TimeSpan(0, -4, 0, 0, 0)), new DateTimeOffset(new DateTime(2020, 5, 26, 16, 56, 47, 677, DateTimeKind.Unspecified).AddTicks(7618), new TimeSpan(0, -4, 0, 0, 0)), "Noobie", 2 });

            migrationBuilder.InsertData(
                table: "Players",
                columns: new[] { "Id", "CreatedDate", "LastModifiedDate", "Name", "Site" },
#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional
                values: new object[,]
                {
                    { 3, new DateTimeOffset(new DateTime(2020, 5, 26, 16, 56, 47, 677, DateTimeKind.Unspecified).AddTicks(7651), new TimeSpan(0, -4, 0, 0, 0)), new DateTimeOffset(new DateTime(2020, 5, 26, 16, 56, 47, 677, DateTimeKind.Unspecified).AddTicks(7666), new TimeSpan(0, -4, 0, 0, 0)), "Vini700", 0 },
                    { 4, new DateTimeOffset(new DateTime(2020, 5, 26, 16, 56, 47, 677, DateTimeKind.Unspecified).AddTicks(7680), new TimeSpan(0, -4, 0, 0, 0)), new DateTimeOffset(new DateTime(2020, 5, 26, 16, 56, 47, 677, DateTimeKind.Unspecified).AddTicks(7694), new TimeSpan(0, -4, 0, 0, 0)), "aupoil", 0 },
                    { 5, new DateTimeOffset(new DateTime(2020, 5, 26, 16, 56, 47, 677, DateTimeKind.Unspecified).AddTicks(7708), new TimeSpan(0, -4, 0, 0, 0)), new DateTimeOffset(new DateTime(2020, 5, 26, 16, 56, 47, 677, DateTimeKind.Unspecified).AddTicks(7721), new TimeSpan(0, -4, 0, 0, 0)), "toskekg", 0 },
                    { 6, new DateTimeOffset(new DateTime(2020, 5, 26, 16, 56, 47, 677, DateTimeKind.Unspecified).AddTicks(7736), new TimeSpan(0, -4, 0, 0, 0)), new DateTimeOffset(new DateTime(2020, 5, 26, 16, 56, 47, 677, DateTimeKind.Unspecified).AddTicks(7749), new TimeSpan(0, -4, 0, 0, 0)), "wolfwolf", 0 }
                });
#pragma warning restore CA1814 // Prefer jagged arrays over multidimensional

            migrationBuilder.UpdateData(
                table: "Puzzles",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "BlackPlayerId", "CreatedDate", "LastModifiedDate", "WhitePlayerId" },
                values: new object[] { 2, new DateTimeOffset(new DateTime(2020, 5, 26, 16, 56, 47, 679, DateTimeKind.Unspecified).AddTicks(9742), new TimeSpan(0, -4, 0, 0, 0)), new DateTimeOffset(new DateTime(2020, 5, 26, 16, 56, 47, 679, DateTimeKind.Unspecified).AddTicks(9797), new TimeSpan(0, -4, 0, 0, 0)), 1 });

            migrationBuilder.UpdateData(
                table: "Puzzles",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "BlackPlayerId", "CreatedDate", "LastModifiedDate", "WhitePlayerId" },
                values: new object[] { 4, new DateTimeOffset(new DateTime(2020, 5, 26, 16, 56, 47, 680, DateTimeKind.Unspecified).AddTicks(3050), new TimeSpan(0, -4, 0, 0, 0)), new DateTimeOffset(new DateTime(2020, 5, 26, 16, 56, 47, 680, DateTimeKind.Unspecified).AddTicks(3092), new TimeSpan(0, -4, 0, 0, 0)), 3 });

            migrationBuilder.UpdateData(
                table: "Puzzles",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "BlackPlayerId", "CreatedDate", "LastModifiedDate", "WhitePlayerId" },
                values: new object[] { 6, new DateTimeOffset(new DateTime(2020, 5, 26, 16, 56, 47, 680, DateTimeKind.Unspecified).AddTicks(6384), new TimeSpan(0, -4, 0, 0, 0)), new DateTimeOffset(new DateTime(2020, 5, 26, 16, 56, 47, 680, DateTimeKind.Unspecified).AddTicks(6426), new TimeSpan(0, -4, 0, 0, 0)), 5 });

            migrationBuilder.CreateIndex(
                name: "IX_Puzzles_BlackPlayerId",
                table: "Puzzles",
                column: "BlackPlayerId");

            migrationBuilder.CreateIndex(
                name: "IX_Puzzles_WhitePlayerId",
                table: "Puzzles",
                column: "WhitePlayerId");

            migrationBuilder.AddForeignKey(
                name: "FK_Puzzles_Players_BlackPlayerId",
                table: "Puzzles",
                column: "BlackPlayerId",
                principalTable: "Players",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Puzzles_Players_WhitePlayerId",
                table: "Puzzles",
                column: "WhitePlayerId",
                principalTable: "Players",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
