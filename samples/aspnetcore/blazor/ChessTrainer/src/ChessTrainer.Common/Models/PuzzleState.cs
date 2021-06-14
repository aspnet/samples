namespace MjrChess.Trainer.Models
{
    public enum PuzzleState
    {
        /// <summary>
        /// Represents a puzzle that the user is still working on.
        /// </summary>
        Ongoing,

        /// <summary>
        /// Represents a correctly solved puzzle.
        /// </summary>
        Solved,

        /// <summary>
        /// Represents an incorrectly solved puzzle.
        /// </summary>
        Missed,

        /// <summary>
        /// Represents a puzzle whose solution was shown (but was not solved).
        /// </summary>
        Revealed
    }
}
