using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MjrChess.Trainer.Data.Migrations
{
    public partial class RemovePuzzlePieceType : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IncorrectPieceMoved",
                table: "Puzzles");

            migrationBuilder.DropColumn(
                name: "PieceMoved",
                table: "Puzzles");

            migrationBuilder.DropColumn(
                name: "SetupPieceMoved",
                table: "Puzzles");

            migrationBuilder.UpdateData(
                table: "Players",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedDate", "LastModifiedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2020, 5, 26, 16, 56, 47, 677, DateTimeKind.Unspecified).AddTicks(5170), new TimeSpan(0, -4, 0, 0, 0)), new DateTimeOffset(new DateTime(2020, 5, 26, 16, 56, 47, 677, DateTimeKind.Unspecified).AddTicks(6434), new TimeSpan(0, -4, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                table: "Players",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedDate", "LastModifiedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2020, 5, 26, 16, 56, 47, 677, DateTimeKind.Unspecified).AddTicks(7561), new TimeSpan(0, -4, 0, 0, 0)), new DateTimeOffset(new DateTime(2020, 5, 26, 16, 56, 47, 677, DateTimeKind.Unspecified).AddTicks(7618), new TimeSpan(0, -4, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                table: "Players",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedDate", "LastModifiedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2020, 5, 26, 16, 56, 47, 677, DateTimeKind.Unspecified).AddTicks(7651), new TimeSpan(0, -4, 0, 0, 0)), new DateTimeOffset(new DateTime(2020, 5, 26, 16, 56, 47, 677, DateTimeKind.Unspecified).AddTicks(7666), new TimeSpan(0, -4, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                table: "Players",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "CreatedDate", "LastModifiedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2020, 5, 26, 16, 56, 47, 677, DateTimeKind.Unspecified).AddTicks(7680), new TimeSpan(0, -4, 0, 0, 0)), new DateTimeOffset(new DateTime(2020, 5, 26, 16, 56, 47, 677, DateTimeKind.Unspecified).AddTicks(7694), new TimeSpan(0, -4, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                table: "Players",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "CreatedDate", "LastModifiedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2020, 5, 26, 16, 56, 47, 677, DateTimeKind.Unspecified).AddTicks(7708), new TimeSpan(0, -4, 0, 0, 0)), new DateTimeOffset(new DateTime(2020, 5, 26, 16, 56, 47, 677, DateTimeKind.Unspecified).AddTicks(7721), new TimeSpan(0, -4, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                table: "Players",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "CreatedDate", "LastModifiedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2020, 5, 26, 16, 56, 47, 677, DateTimeKind.Unspecified).AddTicks(7736), new TimeSpan(0, -4, 0, 0, 0)), new DateTimeOffset(new DateTime(2020, 5, 26, 16, 56, 47, 677, DateTimeKind.Unspecified).AddTicks(7749), new TimeSpan(0, -4, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                table: "Puzzles",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedDate", "LastModifiedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2020, 5, 26, 16, 56, 47, 679, DateTimeKind.Unspecified).AddTicks(9742), new TimeSpan(0, -4, 0, 0, 0)), new DateTimeOffset(new DateTime(2020, 5, 26, 16, 56, 47, 679, DateTimeKind.Unspecified).AddTicks(9797), new TimeSpan(0, -4, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                table: "Puzzles",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedDate", "LastModifiedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2020, 5, 26, 16, 56, 47, 680, DateTimeKind.Unspecified).AddTicks(3050), new TimeSpan(0, -4, 0, 0, 0)), new DateTimeOffset(new DateTime(2020, 5, 26, 16, 56, 47, 680, DateTimeKind.Unspecified).AddTicks(3092), new TimeSpan(0, -4, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                table: "Puzzles",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedDate", "LastModifiedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2020, 5, 26, 16, 56, 47, 680, DateTimeKind.Unspecified).AddTicks(6384), new TimeSpan(0, -4, 0, 0, 0)), new DateTimeOffset(new DateTime(2020, 5, 26, 16, 56, 47, 680, DateTimeKind.Unspecified).AddTicks(6426), new TimeSpan(0, -4, 0, 0, 0)) });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "IncorrectPieceMoved",
                table: "Puzzles",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "PieceMoved",
                table: "Puzzles",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "SetupPieceMoved",
                table: "Puzzles",
                type: "int",
                nullable: false,
                defaultValue: 0);

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

            migrationBuilder.UpdateData(
                table: "Players",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedDate", "LastModifiedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2020, 2, 6, 17, 32, 16, 534, DateTimeKind.Unspecified).AddTicks(272), new TimeSpan(0, -5, 0, 0, 0)), new DateTimeOffset(new DateTime(2020, 2, 6, 17, 32, 16, 534, DateTimeKind.Unspecified).AddTicks(279), new TimeSpan(0, -5, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                table: "Players",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "CreatedDate", "LastModifiedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2020, 2, 6, 17, 32, 16, 534, DateTimeKind.Unspecified).AddTicks(287), new TimeSpan(0, -5, 0, 0, 0)), new DateTimeOffset(new DateTime(2020, 2, 6, 17, 32, 16, 534, DateTimeKind.Unspecified).AddTicks(295), new TimeSpan(0, -5, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                table: "Players",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "CreatedDate", "LastModifiedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2020, 2, 6, 17, 32, 16, 534, DateTimeKind.Unspecified).AddTicks(302), new TimeSpan(0, -5, 0, 0, 0)), new DateTimeOffset(new DateTime(2020, 2, 6, 17, 32, 16, 534, DateTimeKind.Unspecified).AddTicks(309), new TimeSpan(0, -5, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                table: "Players",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "CreatedDate", "LastModifiedDate" },
                values: new object[] { new DateTimeOffset(new DateTime(2020, 2, 6, 17, 32, 16, 534, DateTimeKind.Unspecified).AddTicks(317), new TimeSpan(0, -5, 0, 0, 0)), new DateTimeOffset(new DateTime(2020, 2, 6, 17, 32, 16, 534, DateTimeKind.Unspecified).AddTicks(323), new TimeSpan(0, -5, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                table: "Puzzles",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedDate", "LastModifiedDate", "PieceMoved", "SetupPieceMoved" },
                values: new object[] { new DateTimeOffset(new DateTime(2020, 2, 6, 17, 32, 16, 535, DateTimeKind.Unspecified).AddTicks(8352), new TimeSpan(0, -5, 0, 0, 0)), new DateTimeOffset(new DateTime(2020, 2, 6, 17, 32, 16, 535, DateTimeKind.Unspecified).AddTicks(8428), new TimeSpan(0, -5, 0, 0, 0)), 1, 10 });

            migrationBuilder.UpdateData(
                table: "Puzzles",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedDate", "LastModifiedDate", "PieceMoved", "SetupPieceMoved" },
                values: new object[] { new DateTimeOffset(new DateTime(2020, 2, 6, 17, 32, 16, 536, DateTimeKind.Unspecified).AddTicks(991), new TimeSpan(0, -5, 0, 0, 0)), new DateTimeOffset(new DateTime(2020, 2, 6, 17, 32, 16, 536, DateTimeKind.Unspecified).AddTicks(1029), new TimeSpan(0, -5, 0, 0, 0)), 10, 2 });

            migrationBuilder.UpdateData(
                table: "Puzzles",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedDate", "IncorrectPieceMoved", "LastModifiedDate", "PieceMoved", "SetupPieceMoved" },
                values: new object[] { new DateTimeOffset(new DateTime(2020, 2, 6, 17, 32, 16, 536, DateTimeKind.Unspecified).AddTicks(3265), new TimeSpan(0, -5, 0, 0, 0)), 8, new DateTimeOffset(new DateTime(2020, 2, 6, 17, 32, 16, 536, DateTimeKind.Unspecified).AddTicks(3298), new TimeSpan(0, -5, 0, 0, 0)), 9, 5 });
        }
    }
}
