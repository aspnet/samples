using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MjrChess.Trainer.Data.Models;

namespace MjrChess.Trainer.Data
{
    public class PuzzleDbContext : DbContext
    {
        public DbSet<PuzzleHistory> PuzzleHistories { get; set; } = default!; // https://github.com/dotnet/efcore/issues/10347#issuecomment-524877325

        public DbSet<TacticsPuzzle> Puzzles { get; set; } = default!; // https://github.com/dotnet/efcore/issues/10347#issuecomment-524877325

        public PuzzleDbContext(DbContextOptions<PuzzleDbContext> options)
            : base(options)
        { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Puzzle history configuration
            modelBuilder.Entity<PuzzleHistory>()
                .Property(h => h.UserId)
                .IsRequired();

            modelBuilder.Entity<PuzzleHistory>()
                .HasOne(h => h.Puzzle)
                .WithMany(p => p.History)
                .IsRequired();

            // TacticsPuzzle configuration
            modelBuilder.Entity<TacticsPuzzle>()
                .Property(p => p.Position)
                .IsRequired();

            modelBuilder.Entity<TacticsPuzzle>()
                .Property(p => p.MovedFrom)
                .IsRequired();

            modelBuilder.Entity<TacticsPuzzle>()
                .Property(p => p.MovedTo)
                .IsRequired();

            modelBuilder.Entity<TacticsPuzzle>()
                .Property(p => p.SetupMovedFrom)
                .IsRequired();

            modelBuilder.Entity<TacticsPuzzle>()
                .Property(p => p.SetupMovedTo)
                .IsRequired();

            modelBuilder.Entity<TacticsPuzzle>()
                .HasMany(p => p.History)
                .WithOne(h => h.Puzzle);

            SeedData(modelBuilder);
        }

        /// <summary>
        /// Populates the DB context with seed data (some initial puzzles).
        /// </summary>
        /// <param name="modelBuilder">The model builder to seed.</param>
        private void SeedData(ModelBuilder modelBuilder)
        {
            // Some initial puzzles
            modelBuilder.Entity<TacticsPuzzle>()
                .HasData(
                    new
                    {
                        Id = 1,
                        Position = "rnbqk1nr/pppp1ppp/8/2b1p3/2B1P3/5Q2/PPPP1PPP/RNB1K1NR b KQkq - 3 3",
                        CreatedDate = DateTimeOffset.Now,
                        LastModifiedDate = DateTimeOffset.Now,
                        PieceMoved = Engine.Models.ChessPieces.WhiteQueen,
                        MovedFrom = "f3",
                        MovedTo = "f7",
                        SetupPieceMoved = Engine.Models.ChessPieces.BlackKnight,
                        SetupMovedFrom = "b8",
                        SetupMovedTo = "c6",
                        WhitePlayerName = "Hustler",
                        BlackPlayerName = "Noobie",
                        GameDate = new DateTimeOffset(2015, 2, 7, 0, 0, 0, TimeSpan.Zero)
                    },
                    new
                    {
                        Id = 2,
                        Position = "r3r1k1/ppp2pp1/2n4p/3q4/3Pb3/B1P2N1P/P2Q1PP1/R3R1K1 w - - 4 16",
                        CreatedDate = DateTimeOffset.Now,
                        LastModifiedDate = DateTimeOffset.Now,
                        PieceMoved = Engine.Models.ChessPieces.BlackKnight,
                        MovedFrom = "c6",
                        MovedTo = "a5",
                        SetupPieceMoved = Engine.Models.ChessPieces.WhiteRook,
                        SetupMovedFrom = "e1",
                        SetupMovedTo = "e3",
                        WhitePlayerName = "Vini700",
                        BlackPlayerName = "aupoil",
                        GameDate = new DateTimeOffset(2016, 8, 8, 0, 0, 0, TimeSpan.Zero),
                        Site = "lichess.org",
                        GameUrl = "https://lichess.org/3piQphpY"
                    },
                    new
                    {
                        Id = 3,
                        Position = "r2q1rk1/1pp1b1pp/p7/4pp2/2PnB1P1/3PB2P/PP1Q1P2/R3K2R w KQ - 0 15",
                        CreatedDate = DateTimeOffset.Now,
                        LastModifiedDate = DateTimeOffset.Now,
                        PieceMoved = Engine.Models.ChessPieces.BlackBishop,
                        MovedFrom = "e7",
                        MovedTo = "b4",
                        SetupPieceMoved = Engine.Models.ChessPieces.WhitePawn,
                        SetupMovedFrom = "g4",
                        SetupMovedTo = "f5",
                        IncorrectPieceMoved = Engine.Models.ChessPieces.BlackRook,
                        IncorrectMovedFrom = "f8",
                        IncorrectMovedTo = "f5",
                        WhitePlayerName = "toskekg",
                        BlackPlayerName = "wolfwolf",
                        GameDate = new DateTimeOffset(2016, 10, 7, 0, 0, 0, TimeSpan.Zero),
                        Site = "lichess.org",
                        GameUrl = "https://lichess.org/HjVhr1Dn"
                    },
                    new
                    {
                        Id = 4,
                        Position = "krr5/p6p/2pQ4/3pp3/N5q1/6P1/P1P2PBP/3R2K1 w - - 14 31",
                        CreatedDate = DateTimeOffset.Now,
                        LastModifiedDate = DateTimeOffset.Now,
                        MovedFrom = "b8",
                        MovedTo = "b1",
                        SetupMovedFrom = "d1",
                        SetupMovedTo = "d5",
                        IncorrectMovedFrom = "g4",
                        IncorrectMovedTo = "a4",
                        WhitePlayerName = "fucilaco",
                        BlackPlayerName = "mjrousos",
                        GameDate = new DateTimeOffset(2019, 10, 26, 0, 0, 0, TimeSpan.Zero),
                        Site = "lichess.org",
                        GameUrl = "https://lichess.org/ABBg3RuE"
                    });
        }

        /// <summary>
        /// Update timestamps and save changes to the database.
        /// </summary>
        /// <returns>The number of state entries written to the database.</returns>
        public override int SaveChanges()
        {
            UpdateTimestamps();
            return base.SaveChanges();
        }

        /// <summary>
        /// Update timestamps and save changes to the database.
        /// </summary>
        /// <param name="cancellationToken">A System.Threading.CancellationToken to observe while waiting for the task to complete.</param>
        /// <returns>The number of state entries written to the database.</returns>
        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            UpdateTimestamps();
            return base.SaveChangesAsync(cancellationToken);
        }

        /// <summary>
        /// Update created date and modified timestamps, as appropriate with the current time.
        /// </summary>
        private void UpdateTimestamps()
        {
            var updateTime = DateTimeOffset.Now;
            foreach (var change in ChangeTracker.Entries<EntityBase>())
            {
                switch (change.State)
                {
                    case EntityState.Added:
                        change.Entity.CreatedDate = change.Entity.LastModifiedDate = updateTime;
                        break;
                    case EntityState.Modified:
                        change.Entity.LastModifiedDate = updateTime;
                        break;
                }
            }
        }
    }
}
