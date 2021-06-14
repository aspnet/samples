namespace MjrChess.Engine.Models
{
    /// <summary>
    /// Possible results for a chess game.
    /// </summary>
    public enum GameResult
    {
        /// <summary>
        /// 1-0
        /// </summary>
        WhiteWins,

        /// <summary>
        /// 0-1
        /// </summary>
        BlackWins,

        /// <summary>
        /// 1/2-1/2
        /// </summary>
        Draw,

        /// <summary>
        /// *
        /// </summary>
        Ongoing
    }
}
