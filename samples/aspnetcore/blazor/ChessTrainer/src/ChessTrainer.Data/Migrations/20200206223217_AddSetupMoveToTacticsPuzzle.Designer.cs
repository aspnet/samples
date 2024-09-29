﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using MjrChess.Trainer.Data;

namespace MjrChess.Trainer.Data.Migrations
{
    [DbContext(typeof(PuzzleDbContext))]
    [Migration("20200206223217_AddSetupMoveToTacticsPuzzle")]
    partial class AddSetupMoveToTacticsPuzzle
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.1")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("MjrChess.Trainer.Models.Player", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTimeOffset?>("CreatedDate")
                        .HasColumnType("datetimeoffset");

                    b.Property<DateTimeOffset?>("LastModifiedDate")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Site")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("Players");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            CreatedDate = new DateTimeOffset(new DateTime(2020, 2, 6, 17, 32, 16, 530, DateTimeKind.Unspecified).AddTicks(631), new TimeSpan(0, -5, 0, 0, 0)),
                            LastModifiedDate = new DateTimeOffset(new DateTime(2020, 2, 6, 17, 32, 16, 533, DateTimeKind.Unspecified).AddTicks(9085), new TimeSpan(0, -5, 0, 0, 0)),
                            Name = "Hustler",
                            Site = 2
                        },
                        new
                        {
                            Id = 2,
                            CreatedDate = new DateTimeOffset(new DateTime(2020, 2, 6, 17, 32, 16, 534, DateTimeKind.Unspecified).AddTicks(210), new TimeSpan(0, -5, 0, 0, 0)),
                            LastModifiedDate = new DateTimeOffset(new DateTime(2020, 2, 6, 17, 32, 16, 534, DateTimeKind.Unspecified).AddTicks(253), new TimeSpan(0, -5, 0, 0, 0)),
                            Name = "Noobie",
                            Site = 2
                        },
                        new
                        {
                            Id = 3,
                            CreatedDate = new DateTimeOffset(new DateTime(2020, 2, 6, 17, 32, 16, 534, DateTimeKind.Unspecified).AddTicks(272), new TimeSpan(0, -5, 0, 0, 0)),
                            LastModifiedDate = new DateTimeOffset(new DateTime(2020, 2, 6, 17, 32, 16, 534, DateTimeKind.Unspecified).AddTicks(279), new TimeSpan(0, -5, 0, 0, 0)),
                            Name = "Vini700",
                            Site = 0
                        },
                        new
                        {
                            Id = 4,
                            CreatedDate = new DateTimeOffset(new DateTime(2020, 2, 6, 17, 32, 16, 534, DateTimeKind.Unspecified).AddTicks(287), new TimeSpan(0, -5, 0, 0, 0)),
                            LastModifiedDate = new DateTimeOffset(new DateTime(2020, 2, 6, 17, 32, 16, 534, DateTimeKind.Unspecified).AddTicks(295), new TimeSpan(0, -5, 0, 0, 0)),
                            Name = "aupoil",
                            Site = 0
                        },
                        new
                        {
                            Id = 5,
                            CreatedDate = new DateTimeOffset(new DateTime(2020, 2, 6, 17, 32, 16, 534, DateTimeKind.Unspecified).AddTicks(302), new TimeSpan(0, -5, 0, 0, 0)),
                            LastModifiedDate = new DateTimeOffset(new DateTime(2020, 2, 6, 17, 32, 16, 534, DateTimeKind.Unspecified).AddTicks(309), new TimeSpan(0, -5, 0, 0, 0)),
                            Name = "toskekg",
                            Site = 0
                        },
                        new
                        {
                            Id = 6,
                            CreatedDate = new DateTimeOffset(new DateTime(2020, 2, 6, 17, 32, 16, 534, DateTimeKind.Unspecified).AddTicks(317), new TimeSpan(0, -5, 0, 0, 0)),
                            LastModifiedDate = new DateTimeOffset(new DateTime(2020, 2, 6, 17, 32, 16, 534, DateTimeKind.Unspecified).AddTicks(323), new TimeSpan(0, -5, 0, 0, 0)),
                            Name = "wolfwolf",
                            Site = 0
                        });
                });

            modelBuilder.Entity("MjrChess.Trainer.Models.PuzzleHistory", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTimeOffset?>("CreatedDate")
                        .HasColumnType("datetimeoffset");

                    b.Property<DateTimeOffset?>("LastModifiedDate")
                        .HasColumnType("datetimeoffset");

                    b.Property<int>("PuzzleId")
                        .HasColumnType("int");

                    b.Property<bool>("Solved")
                        .HasColumnType("bit");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("PuzzleId");

                    b.ToTable("PuzzleHistories");
                });

            modelBuilder.Entity("MjrChess.Trainer.Models.TacticsPuzzle", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int?>("BlackPlayerId")
                        .HasColumnType("int");

                    b.Property<DateTimeOffset?>("CreatedDate")
                        .HasColumnType("datetimeoffset");

                    b.Property<DateTimeOffset?>("GameDate")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("GameUrl")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("IncorrectMovedFrom")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("IncorrectMovedTo")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("IncorrectPieceMoved")
                        .HasColumnType("int");

                    b.Property<int?>("IncorrectPiecePromotedTo")
                        .HasColumnType("int");

                    b.Property<DateTimeOffset?>("LastModifiedDate")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("MovedFrom")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("MovedTo")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("PieceMoved")
                        .HasColumnType("int");

                    b.Property<int?>("PiecePromotedTo")
                        .HasColumnType("int");

                    b.Property<string>("Position")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("SetupMovedFrom")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("SetupMovedTo")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("SetupPieceMoved")
                        .HasColumnType("int");

                    b.Property<int?>("SetupPiecePromotedTo")
                        .HasColumnType("int");

                    b.Property<string>("Site")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("WhitePlayerId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("BlackPlayerId");

                    b.HasIndex("WhitePlayerId");

                    b.ToTable("Puzzles");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            BlackPlayerId = 2,
                            CreatedDate = new DateTimeOffset(new DateTime(2020, 2, 6, 17, 32, 16, 535, DateTimeKind.Unspecified).AddTicks(8352), new TimeSpan(0, -5, 0, 0, 0)),
                            GameDate = new DateTimeOffset(new DateTime(2015, 2, 7, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)),
                            LastModifiedDate = new DateTimeOffset(new DateTime(2020, 2, 6, 17, 32, 16, 535, DateTimeKind.Unspecified).AddTicks(8428), new TimeSpan(0, -5, 0, 0, 0)),
                            MovedFrom = "f3",
                            MovedTo = "f7",
                            PieceMoved = 1,
                            Position = "rnbqk1nr/pppp1ppp/8/2b1p3/2B1P3/5Q2/PPPP1PPP/RNB1K1NR b KQkq - 3 3",
                            SetupMovedFrom = "b8",
                            SetupMovedTo = "c6",
                            SetupPieceMoved = 10,
                            WhitePlayerId = 1
                        },
                        new
                        {
                            Id = 2,
                            BlackPlayerId = 4,
                            CreatedDate = new DateTimeOffset(new DateTime(2020, 2, 6, 17, 32, 16, 536, DateTimeKind.Unspecified).AddTicks(991), new TimeSpan(0, -5, 0, 0, 0)),
                            GameDate = new DateTimeOffset(new DateTime(2016, 8, 8, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)),
                            GameUrl = "https://lichess.org/3piQphpY",
                            LastModifiedDate = new DateTimeOffset(new DateTime(2020, 2, 6, 17, 32, 16, 536, DateTimeKind.Unspecified).AddTicks(1029), new TimeSpan(0, -5, 0, 0, 0)),
                            MovedFrom = "c6",
                            MovedTo = "a5",
                            PieceMoved = 10,
                            Position = "r3r1k1/ppp2pp1/2n4p/3q4/3Pb3/B1P2N1P/P2Q1PP1/R3R1K1 w - - 4 16",
                            SetupMovedFrom = "e1",
                            SetupMovedTo = "e3",
                            SetupPieceMoved = 2,
                            Site = "lichess.org",
                            WhitePlayerId = 3
                        },
                        new
                        {
                            Id = 3,
                            BlackPlayerId = 6,
                            CreatedDate = new DateTimeOffset(new DateTime(2020, 2, 6, 17, 32, 16, 536, DateTimeKind.Unspecified).AddTicks(3265), new TimeSpan(0, -5, 0, 0, 0)),
                            GameDate = new DateTimeOffset(new DateTime(2016, 10, 7, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)),
                            GameUrl = "https://lichess.org/HjVhr1Dn",
                            IncorrectMovedFrom = "f8",
                            IncorrectMovedTo = "f5",
                            IncorrectPieceMoved = 8,
                            LastModifiedDate = new DateTimeOffset(new DateTime(2020, 2, 6, 17, 32, 16, 536, DateTimeKind.Unspecified).AddTicks(3298), new TimeSpan(0, -5, 0, 0, 0)),
                            MovedFrom = "e7",
                            MovedTo = "b4",
                            PieceMoved = 9,
                            Position = "r2q1rk1/1pp1b1pp/p7/4pp2/2PnB1P1/3PB2P/PP1Q1P2/R3K2R w KQ - 0 15",
                            SetupMovedFrom = "g4",
                            SetupMovedTo = "f5",
                            SetupPieceMoved = 5,
                            Site = "lichess.org",
                            WhitePlayerId = 5
                        });
                });

            modelBuilder.Entity("MjrChess.Trainer.Models.UserSettings", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTimeOffset?>("CreatedDate")
                        .HasColumnType("datetimeoffset");

                    b.Property<DateTimeOffset?>("LastModifiedDate")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("UserSettings");
                });

            modelBuilder.Entity("MjrChess.Trainer.Models.UserSettingsXPlayer", b =>
                {
                    b.Property<int>("UserSettingsId")
                        .HasColumnType("int");

                    b.Property<int>("PlayerId")
                        .HasColumnType("int");

                    b.HasKey("UserSettingsId", "PlayerId");

                    b.HasIndex("PlayerId");

                    b.ToTable("UserSettingsXPlayers");
                });

            modelBuilder.Entity("MjrChess.Trainer.Models.PuzzleHistory", b =>
                {
                    b.HasOne("MjrChess.Trainer.Models.TacticsPuzzle", "Puzzle")
                        .WithMany("History")
                        .HasForeignKey("PuzzleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("MjrChess.Trainer.Models.TacticsPuzzle", b =>
                {
                    b.HasOne("MjrChess.Trainer.Models.Player", "BlackPlayer")
                        .WithMany()
                        .HasForeignKey("BlackPlayerId");

                    b.HasOne("MjrChess.Trainer.Models.Player", "WhitePlayer")
                        .WithMany()
                        .HasForeignKey("WhitePlayerId");
                });

            modelBuilder.Entity("MjrChess.Trainer.Models.UserSettingsXPlayer", b =>
                {
                    b.HasOne("MjrChess.Trainer.Models.Player", "Player")
                        .WithMany()
                        .HasForeignKey("PlayerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("MjrChess.Trainer.Models.UserSettings", "UserSettings")
                        .WithMany("PreferredPlayers")
                        .HasForeignKey("UserSettingsId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
