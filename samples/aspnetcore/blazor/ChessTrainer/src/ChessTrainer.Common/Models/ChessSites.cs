namespace MjrChess.Trainer.Models
{
    /// <summary>
    /// Popular internet chess sites where the games the puzzles come from may have been played.
    /// </summary>
    public enum ChessSites
    {
        /// <summary>
        /// Players or games from https://lichess.org/
        /// </summary>
        LiChess,

        /// <summary>
        /// Players or games from https://chess.com/
        /// </summary>
        ChessCom,

        /// <summary>
        /// Players or games from another source
        /// </summary>
        Other
    }
}
