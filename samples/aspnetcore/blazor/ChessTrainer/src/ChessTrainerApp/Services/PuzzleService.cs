using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MjrChess.Trainer.Data;
using MjrChess.Trainer.Models;

namespace MjrChess.Trainer.Services
{
    /// <summary>
    /// Service for retrieving tactics puzzles.
    /// </summary>
    public class PuzzleService : IPuzzleService
    {
        private static Random NumGen { get; } = new Random();

        private IRepository<TacticsPuzzle> PuzzleRepository { get; }

        private ILogger<PuzzleService> Logger { get; }

        public PuzzleService(IRepository<TacticsPuzzle> puzzleRepository,
                             ILogger<PuzzleService> logger)
        {
            PuzzleRepository = puzzleRepository ?? throw new ArgumentNullException(nameof(puzzleRepository));
            Logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <summary>
        /// Gets a puzzle by ID.
        /// </summary>
        /// <param name="puzzleId">The puzzle ID to retrieve.</param>
        /// <returns>The puzzle with the specified ID, or null if no such puzzle exists.</returns>
        public async Task<TacticsPuzzle?> GetPuzzleAsync(int puzzleId)
        {
            var puzzle = await PuzzleRepository.GetAsync(puzzleId);
            if (puzzle == null)
            {
                Logger.LogInformation("Puzzle {PuzzleId} not found", puzzleId);
            }
            else
            {
                Logger.LogInformation("Retrieved puzzle {PuzzleId}", puzzleId);
            }

            return puzzle;
        }

        /// <summary>
        /// Gets a random puzzle.
        /// </summary>
        /// <returns>A random puzzle, or null if no puzzles exist.</returns>
        public async Task<TacticsPuzzle?> GetRandomPuzzleAsync()
        {
            var puzzlesQuery = PuzzleRepository.Query();
            var puzzleCount = await puzzlesQuery.CountAsync();
            var skipCount = NumGen.Next(puzzleCount);
            var puzzle = await puzzlesQuery.Skip(skipCount).FirstOrDefaultAsync();
            if (puzzle == null)
            {
                Logger.LogError("No puzzles found");
            }
            else
            {
                Logger.LogInformation("Retrieved puzzle {PuzzleId} (index {SkipCount} of {PuzzleCount} puzzles)",
                    puzzle.Id,
                    skipCount,
                    puzzleCount);
            }

            return puzzle;
        }
    }
}
