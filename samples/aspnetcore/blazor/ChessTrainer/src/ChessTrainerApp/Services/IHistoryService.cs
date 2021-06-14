using System.Collections.Generic;
using System.Threading.Tasks;
using MjrChess.Trainer.Models;

namespace MjrChess.Trainer.Services
{
    /// <summary>
    /// Service for retrieving and updating users' attempts to solve tactics puzzles.
    /// </summary>
    public interface IHistoryService
    {
        /// <summary>
        /// Gets puzzles a user has attempted to solve.
        /// </summary>
        /// <param name="userId">The user to retrieve puzzle histories for.</param>
        /// <returns>An enumerable of puzzle histories for the given user, recording puzzles they have solved or failed.</returns>
        Task<IEnumerable<PuzzleHistory>> GetPuzzleHistoryAsync(string userId);

        /// <summary>
        /// Record an attempt to solve a puzzle.
        /// </summary>
        /// <param name="puzzleHistory">The puzzle attempt, including the user's ID, the puzzle, and whether it was solved successfully or not.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        Task RecordPuzzleHistoryAsync(PuzzleHistory puzzleHistory);
    }
}
