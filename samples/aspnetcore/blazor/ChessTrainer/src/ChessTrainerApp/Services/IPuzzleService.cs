using System.Threading.Tasks;
using MjrChess.Trainer.Models;

namespace MjrChess.Trainer.Services
{
    /// <summary>
    /// Service for retrieving tactics puzzles.
    /// </summary>
    public interface IPuzzleService
    {
        /// <summary>
        /// Gets a puzzle by ID.
        /// </summary>
        /// <param name="puzzleId">The puzzle ID to retrieve.</param>
        /// <returns>The puzzle with the specified ID, or null if no such puzzle exists.</returns>
        Task<TacticsPuzzle?> GetRandomPuzzleAsync();

        /// <summary>
        /// Gets a random puzzle.
        /// </summary>
        /// <returns>A random puzzle, or null if no puzzles exist.</returns>
        Task<TacticsPuzzle?> GetPuzzleAsync(int puzzleId);
    }
}
