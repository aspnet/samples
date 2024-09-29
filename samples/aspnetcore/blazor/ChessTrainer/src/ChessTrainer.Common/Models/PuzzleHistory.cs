namespace MjrChess.Trainer.Models
{
    /// <summary>
    /// A record of a user attempting to solve a tactics puzzle.
    /// </summary>
    public class PuzzleHistory : IEntity
    {
        /// <summary>
        /// Gets or sets the user attempting the puzzle; null if the user is not logged in.
        /// </summary>
        public string? UserId { get; set; }

        /// <summary>
        /// Gets or sets the puzzle being attempted.
        /// </summary>
        public TacticsPuzzle Puzzle { get; set; } = default!;

        /// <summary>
        /// Gets or sets a value indicating whether the user successfully solved the puzzle.
        /// </summary>
        public bool Solved { get; set; }
    }
}
