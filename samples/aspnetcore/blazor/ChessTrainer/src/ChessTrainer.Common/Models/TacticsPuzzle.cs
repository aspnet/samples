using System;
using System.Collections.Generic;
using MjrChess.Engine;
using MjrChess.Engine.Models;
using MjrChess.Engine.Utilities;

namespace MjrChess.Trainer.Models
{
    /// <summary>
    /// A one-move chess tactics puzzle.
    /// </summary>
    public class TacticsPuzzle : IEntity
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
                throw new ArgumentException("message", nameof(position));
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

            // Load the game in an engine to resolve the setup, correct, and incorrect moves based on input starting and ending squares.
            var engine = new ChessEngine();
            engine.LoadFEN(Position);

            // Square moved from and to is sufficient to construct a long algebraic notation representation of the move that the engine is able to resolve.
            SetupMove = engine.MoveFromAlgebraicNotation($"{SetupMovedFrom}{SetupMovedTo}{(SetupPiecePromotedTo.HasValue ? $"={ChessFormatter.PieceToString(SetupPiecePromotedTo.Value, false)}" : string.Empty)}");
            engine.Game.Move(SetupMove);
            Solution = engine.MoveFromAlgebraicNotation($"{MovedFrom}{MovedTo}{(PiecePromotedTo.HasValue ? $"={ChessFormatter.PieceToString(PiecePromotedTo.Value, false)}" : string.Empty)}");
            if (IncorrectMovedFrom != null && IncorrectMovedTo != null)
            {
                IncorrectMove = engine.MoveFromAlgebraicNotation($"{IncorrectMovedFrom}{IncorrectMovedTo}{(IncorrectPiecePromotedTo.HasValue ? $"={ChessFormatter.PieceToString(IncorrectPiecePromotedTo.Value, false)}" : string.Empty)}");
            }
        }

        /// <summary>
        /// Gets the initial position of the board in FEN notation.
        /// </summary>
        public string Position { get; }

        /// <summary>
        /// Gets the square a piece was moved from to set up the tactic.
        /// </summary>
        public string SetupMovedFrom { get; }

        /// <summary>
        /// Gets the square a piece was moved to to set up the tactic.
        /// </summary>
        public string SetupMovedTo { get; }

        /// <summary>
        /// Gets type of piece promoted to if a piece was promoted to setup the tactic, otherwise null.
        /// </summary>
        public ChessPieces? SetupPiecePromotedTo { get; }

        /// <summary>
        /// Gets the correct square to move from to solve the tactic.
        /// </summary>
        public string MovedFrom { get; }

        /// <summary>
        /// Gets the correct square to move to to solve the tactic.
        /// </summary>
        public string MovedTo { get; }

        /// <summary>
        /// Gets the correct type of piece to promote to to solve the tactic, or null if no promotion is necessary.
        /// </summary>
        public ChessPieces? PiecePromotedTo { get; }

        /// <summary>
        /// Gets the incorrect square moved from if the player missed this tactic during game the tactic puzzle is from.
        /// </summary>
        public string? IncorrectMovedFrom { get; }

        /// <summary>
        /// Gets the incorrect square moved to if the player missed this tactic during game the tactic puzzle is from.
        /// </summary>
        public string? IncorrectMovedTo { get; }

        /// <summary>
        /// Gets the incorrect type of piece promoted to if the player missed this tactic during game the tactic puzzle is from, or null if no promotion occurred.
        /// </summary>
        public ChessPieces? IncorrectPiecePromotedTo { get; }

        /// <summary>
        /// Gets a value indicating whether it is white's move in this tactic.
        /// </summary>
        public bool WhiteToMove
        {
            get
            {
                // The value is the *opposite* of what's indicated in the FEN since the FEN represents the board state prior to the setup move.
                return !Position.Split()[1].Equals("w", StringComparison.OrdinalIgnoreCase);
            }
        }

        /// <summary>
        /// Gets the correct solution to the tactics puzzle.
        /// </summary>
        public Move Solution { get; }

        /// <summary>
        /// Gets the incorrect move made in the game if the tactic was missed, or null if the player made the correct move.
        /// </summary>
        public Move? IncorrectMove { get; }

        /// <summary>
        /// Gets the setup move made from the initial board position to achieve the board state of the tactic.
        /// This is tracked so that users solving the tactic can see the immediate previous move.
        /// </summary>
        public Move SetupMove { get; }

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
