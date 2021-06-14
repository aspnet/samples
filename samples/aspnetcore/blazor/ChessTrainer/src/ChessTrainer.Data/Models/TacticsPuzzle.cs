using System;
using System.Collections.Generic;
using MjrChess.Engine.Models;

namespace MjrChess.Trainer.Data.Models
{
    /// <summary>
    /// A one-move chess tactics puzzle.
    /// </summary>
    public class TacticsPuzzle : EntityBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TacticsPuzzle"/> class.
        /// </summary>
        /// <param name="position">The initial position of the board in FEN notation.</param>
        /// <param name="setupMovedFrom">The square a piece was moved from to set up the tactic.</param>
        /// <param name="setupMovedTo">The square a piece was moved to to set up the tactic.</param>
        /// <param name="setupPiecePromotedTo">If the piece moved to set up the tactic was promoted, the type of piece it was promoted to.</param>
        /// <param name="movedFrom">The square a piece should be moved from to solve the tactic.</param>
        /// <param name="movedTo">The square a piece should be moved to to solve the tactic.</param>
        /// <param name="piecePromotedTo">If a piece is promoted to solve the tactic, the type of piece it is promoted to.</param>
        /// <param name="incorrectMovedFrom">The incorrect square moved from in the actual game if the player missed the tactic.</param>
        /// <param name="incorrectMovedTo">The incorrect square moved to in the actual game if the player missed the tactic.</param>
        /// <param name="incorrectPiecePromotedTo">The incorrect piece promoted to in the actual game if the player missed the tactic.</param>
        public TacticsPuzzle(
            string position,
            string setupMovedFrom,
            string setupMovedTo,
            ChessPieces? setupPiecePromotedTo,
            string movedFrom,
            string movedTo,
            ChessPieces? piecePromotedTo,
            string? incorrectMovedFrom,
            string? incorrectMovedTo,
            ChessPieces? incorrectPiecePromotedTo)
        {
            if (string.IsNullOrWhiteSpace(position))
            {
                throw new ArgumentException("Position must be a valid FEN string", nameof(position));
            }

            Position = position;
            SetupMovedFrom = setupMovedFrom;
            SetupMovedTo = setupMovedTo;
            SetupPiecePromotedTo = setupPiecePromotedTo;
            MovedFrom = movedFrom;
            MovedTo = movedTo;
            PiecePromotedTo = piecePromotedTo;
            IncorrectMovedFrom = incorrectMovedFrom;
            IncorrectMovedTo = incorrectMovedTo;
            IncorrectPiecePromotedTo = incorrectPiecePromotedTo;
        }

        /// <summary>
        /// Gets or sets the initial position of the board in FEN notation.
        /// </summary>
        public string Position { get; set; }

        /// <summary>
        /// Gets or sets the square a piece was moved from to set up the tactic.
        /// </summary>
        public string SetupMovedFrom { get; set; }

        /// <summary>
        /// Gets or sets the square a piece was moved to to set up the tactic.
        /// </summary>
        public string SetupMovedTo { get; set; }

        /// <summary>
        /// Gets or sets type of piece promoted to if a piece was promoted to setup the tactic, otherwise null.
        /// </summary>
        public ChessPieces? SetupPiecePromotedTo { get; set; }

        /// <summary>
        /// Gets or sets the correct square to move from to solve the tactic.
        /// </summary>
        public string MovedFrom { get; set; }

        /// <summary>
        /// Gets or sets the correct square to move to to solve the tactic.
        /// </summary>
        public string MovedTo { get; set; }

        /// <summary>
        /// Gets or sets the correct type of piece to promote to to solve the tactic, or null if no promotion is necessary.
        /// </summary>
        public ChessPieces? PiecePromotedTo { get; set; }

        /// <summary>
        /// Gets or sets the incorrect square moved from if the player missed this tactic during game the tactic puzzle is from.
        /// </summary>
        public string? IncorrectMovedFrom { get; set; }

        /// <summary>
        /// Gets or sets the incorrect square moved to if the player missed this tactic during game the tactic puzzle is from.
        /// </summary>
        public string? IncorrectMovedTo { get; set; }

        /// <summary>
        /// Gets or sets the incorrect type of piece promoted to if the player missed this tactic during game the tactic puzzle is from, or null if no promotion occurred.
        /// </summary>
        public ChessPieces? IncorrectPiecePromotedTo { get; set; }

        /// <summary>
        /// Gets or sets the username of the white player from the game this tactic comes from, if available.
        /// </summary>
        public string? WhitePlayerName { get; set; }

        /// <summary>
        /// Gets or sets the username of the black player from the game this tactic comes from, if available.
        /// </summary>
        public string? BlackPlayerName { get; set; }

        /// <summary>
        /// Gets or sets the date and time the game this tactic comes from was played, if available.
        /// </summary>
        public DateTimeOffset? GameDate { get; set; }

        /// <summary>
        /// Gets or sets where the game this tactic comes from was played, if available.
        /// </summary>
        public string? Site { get; set; }

        /// <summary>
        /// Gets or sets a URL linking to the game this tactic comes from, if available.
        /// </summary>
        public string? GameUrl { get; set; }

        /// <summary>
        /// Gets or sets a collection of attempts to solve this tactic.
        /// </summary>
        public ICollection<PuzzleHistory> History { get; set; } = new HashSet<PuzzleHistory>();
    }
}
