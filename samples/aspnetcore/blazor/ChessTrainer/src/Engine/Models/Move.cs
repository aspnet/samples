using System;
using MjrChess.Engine.Utilities;

namespace MjrChess.Engine.Models
{
    /// <summary>
    /// A single move in a chess game.
    /// </summary>
    public class Move : IEquatable<Move>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Move"/> class.
        /// </summary>
        /// <param name="pieceMoved">The type of piece moved.</param>
        /// <param name="originalPosition">The board position the piece was moved from.</param>
        /// <param name="finalPosition">The board position the piece was moved to.</param>
        /// <param name="piecePromotedTo">The type of piece promoted to as part of the move, or null if not promotion occurred.</param>
        public Move(ChessPieces pieceMoved, BoardPosition originalPosition, BoardPosition finalPosition, ChessPieces? piecePromotedTo = null)
        {
            PieceMoved = pieceMoved;
            OriginalPosition = originalPosition;
            FinalPosition = finalPosition;
            PiecePromotedTo = piecePromotedTo;
        }

        /// <summary>
        /// Gets the board position the moved piece began in.
        /// </summary>
        public BoardPosition OriginalPosition { get; }

        /// <summary>
        /// Gets the board position the moved piece ended in.
        /// </summary>
        public BoardPosition FinalPosition { get; }

        /// <summary>
        /// Gets or sets a value indicating whether the rank the piece moved from is needed to disambiguate the move.
        /// </summary>
        public bool AmbiguousOriginalRank { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the file the piece moved from is needed to disambiguate the move.
        /// </summary>
        public bool AmbiguousOriginalFile { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the move captured another piece.
        /// </summary>
        public bool Capture { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the move resulted in check.
        /// </summary>
        public bool Checks { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the move resulted in checkmate.
        /// </summary>
        public bool Checkmates { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the move resulted in stalemate.
        /// </summary>
        public bool Stalemates { get; set; }

        /// <summary>
        /// Gets the type of piece moved.
        /// </summary>
        public ChessPieces PieceMoved { get; }

        /// <summary>
        /// Gets or sets the type of piece promoted to, or null if the move did not result in promotion.
        /// </summary>
        public ChessPieces? PiecePromotedTo { get; set; }

        /// <summary>
        /// Gets a value indicating whether the move was a short-side (king side) castle.
        /// </summary>
        public bool ShortCastle =>
            (PieceMoved == ChessPieces.WhiteKing || PieceMoved == ChessPieces.BlackKing) &&
            OriginalPosition.File == 4 &&
            FinalPosition.File == 6;

        /// <summary>
        /// Gets a value indicating whether the move was a long-side (queen side) castle.
        /// </summary>
        public bool LongCastle =>
            (PieceMoved == ChessPieces.WhiteKing || PieceMoved == ChessPieces.BlackKing) &&
            OriginalPosition.File == 4 &&
            FinalPosition.File == 2;

        public bool Equals(Move? other) =>
            !(other is null) &&
            PieceMoved == other.PieceMoved &&
            OriginalPosition == other.OriginalPosition &&
            FinalPosition == other.FinalPosition &&
            PiecePromotedTo == other.PiecePromotedTo;

        public override bool Equals(object obj) => (obj is Move move) && Equals(move);

        public override int GetHashCode() => $"{PieceMoved}{OriginalPosition}{FinalPosition}{PiecePromotedTo}".GetHashCode();

        public static bool operator ==(Move? lhs, Move? rhs) => lhs?.Equals(rhs) ?? rhs is null;

        public static bool operator !=(Move? lhs, Move? rhs) => !(lhs == rhs);

        /// <summary>
        /// Formats the chess move as a string in short algebraic notation.
        /// </summary>
        /// <returns>The move expressed in short algebraic notation.</returns>
        public override string ToString() => ChessFormatter.MoveToString(this);
    }
}
