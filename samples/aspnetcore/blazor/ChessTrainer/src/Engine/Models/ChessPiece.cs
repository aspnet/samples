using System;
using MjrChess.Engine.Utilities;

namespace MjrChess.Engine.Models
{
    /// <summary>
    /// Represents a chess piece in play in a chess game.
    /// </summary>
    public class ChessPiece : IEquatable<ChessPiece>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ChessPiece"/> class.
        /// </summary>
        /// <param name="pieceType">The type of chess piece.</param>
        /// <param name="position">The piece's position on the board.</param>
        public ChessPiece(ChessPieces pieceType, BoardPosition position)
        {
            PieceType = pieceType;
            Position = position;
        }

        /// <summary>
        /// Gets the type of chess piece (including its color).
        /// </summary>
        public ChessPieces PieceType { get; }

        /// <summary>
        /// Gets the piece's position on the board.
        /// </summary>
        public BoardPosition Position { get; }

        public bool Equals(ChessPiece? other) =>
            PieceType == other?.PieceType &&
            Position == other?.Position;

        public override bool Equals(object obj) => (obj is ChessPiece piece) && Equals(piece);

        public override int GetHashCode() => (Position.GetHashCode() * Enum.GetValues(typeof(ChessPieces)).Length) + PieceType.GetHashCode();

        public override string ToString() =>
            string.Join(" ",
                ChessFormatter.PieceToString(PieceType, pForPawn: true),
                Position.ToString());

        public static bool operator ==(ChessPiece? lhs, ChessPiece? rhs)
        {
            // Check for null on left side.
            if (lhs is null)
            {
                if (rhs is null)
                {
                    // null == null = true.
                    return true;
                }

                // Only the left side is null.
                return false;
            }

            // Equals handles case of null on right side.
            return lhs.Equals(rhs);
        }

        public static bool operator !=(ChessPiece? lhs, ChessPiece? rhs)
        {
            return !(lhs == rhs);
        }
    }
}
