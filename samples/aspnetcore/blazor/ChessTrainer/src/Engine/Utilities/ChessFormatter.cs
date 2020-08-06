using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using MjrChess.Engine.Models;

namespace MjrChess.Engine.Utilities
{
    /// <summary>
    /// Helper methods for translating chess concepts to or from strings.
    /// </summary>
    public static class ChessFormatter
    {
        /// <summary>
        /// Gets a collection of chess pieces controlled by the white player.
        /// </summary>
        public static ChessPieces[] WhitePieces { get; } = new[]
        {
            ChessPieces.WhiteKing,
            ChessPieces.WhiteQueen,
            ChessPieces.WhiteRook,
            ChessPieces.WhiteBishop,
            ChessPieces.WhiteKnight,
            ChessPieces.WhitePawn
        };

        /// <summary>
        /// Determines whether a given chess piece is controlled by the white player.
        /// </summary>
        /// <param name="piece">The type of chess piece.</param>
        /// <returns>True if the piece belongs to the white player and false otherwise.</returns>
        public static bool IsPieceWhite(ChessPieces piece) => WhitePieces.Contains(piece);

        /// <summary>
        /// Gets a collection of chess pieces the white player controls at the beginning of the game.
        /// </summary>
        public static ChessPieces[] StartingWhitePieces { get; } = new[]
        {
            ChessPieces.WhitePawn,
            ChessPieces.WhitePawn,
            ChessPieces.WhitePawn,
            ChessPieces.WhitePawn,
            ChessPieces.WhitePawn,
            ChessPieces.WhitePawn,
            ChessPieces.WhitePawn,
            ChessPieces.WhitePawn,
            ChessPieces.WhiteRook,
            ChessPieces.WhiteKnight,
            ChessPieces.WhiteBishop,
            ChessPieces.WhiteQueen,
            ChessPieces.WhiteKing,
            ChessPieces.WhiteBishop,
            ChessPieces.WhiteKnight,
            ChessPieces.WhiteRook
        };

        /// <summary>
        /// Gets a collection of chess pieces the black player controls at the beginning of the game.
        /// </summary>
        public static ChessPieces[] StartingBlackPieces { get; } = new[]
        {
            ChessPieces.BlackPawn,
            ChessPieces.BlackPawn,
            ChessPieces.BlackPawn,
            ChessPieces.BlackPawn,
            ChessPieces.BlackPawn,
            ChessPieces.BlackPawn,
            ChessPieces.BlackPawn,
            ChessPieces.BlackPawn,
            ChessPieces.BlackRook,
            ChessPieces.BlackKnight,
            ChessPieces.BlackBishop,
            ChessPieces.BlackQueen,
            ChessPieces.BlackKing,
            ChessPieces.BlackBishop,
            ChessPieces.BlackKnight,
            ChessPieces.BlackRook
        };

        /// <summary>
        /// Converts a rank index to its string equivalent.
        /// </summary>
        /// <param name="rank">The 0-based index of the rank.</param>
        /// <returns>A string description of the rank.</returns>
        public static string RankToString(int rank) => $"{rank + 1}";

        /// <summary>
        /// Converts a rank to its 0-based index.
        /// </summary>
        /// <param name="rank">A string description of the rank.</param>
        /// <returns>An integer representing the 0-based index of the rank.</returns>
        public static int RankFromString(string rank) => int.Parse(rank, CultureInfo.InvariantCulture) - 1;

        /// <summary>
        /// Converts a rank to its 0-based index.
        /// </summary>
        /// <param name="rank">A one character description of the rank.</param>
        /// <returns>An integer representing the 0-based index of the rank.</returns>
        public static int RankFromChar(char rank) => RankFromString(rank.ToString(CultureInfo.InvariantCulture));

        /// <summary>
        /// Converts a file index to its string equivalent.
        /// </summary>
        /// <param name="file">The 0-based index of the file.</param>
        /// <returns>A string description of the file.</returns>
        public static string FileToString(int file) => $"{(char)(file + 0x61)}";

        /// <summary>
        /// Converts a file to its 0-based index.
        /// </summary>
        /// <param name="file">A string description of the file.</param>
        /// <returns>An integer representing the 0-based index of the file.</returns>
        public static int FileFromString(string file) => FileFromChar(file[0]);

        /// <summary>
        /// Converts a file to its 0-based index.
        /// </summary>
        /// <param name="file">A one character description of the file.</param>
        /// <returns>An integer representing the 0-based index of the file.</returns>
        public static int FileFromChar(char file) => file - 0x61;

        /// <summary>
        /// Converts a chess piece to an approximate numerical value.
        /// </summary>
        /// <param name="piece">The piece to be converted to a value.</param>
        /// <returns>The approximate number of pawns the piece is worth.</returns>
        public static int GetPieceValue(ChessPieces piece) =>
            piece switch
            {
                // TODO : This is fine for purposes of displaying advantage,
                //        but if this develops into a fuller chess engine, it
                //        will need a more sophisticated system and kings will need
                //        a value.
                ChessPieces.WhiteQueen => 9,
                ChessPieces.BlackQueen => -9,
                ChessPieces.WhiteRook => 5,
                ChessPieces.BlackRook => -5,
                ChessPieces.WhiteBishop => 3,
                ChessPieces.BlackBishop => -3,
                ChessPieces.WhiteKnight => 3,
                ChessPieces.BlackKnight => -3,
                ChessPieces.WhitePawn => 1,
                ChessPieces.BlackPawn => -1,
                _ => 0
            };

        /// <summary>
        /// Formats a game result as a string.
        /// </summary>
        /// <param name="result">The game result.</param>
        /// <returns>A common string representation of the game result.</returns>
        public static string ResultToString(GameResult result) =>
            result switch
            {
                GameResult.BlackWins => "0-1",
                GameResult.WhiteWins => "1-0",
                GameResult.Draw => "1/2-1/2",
                _ => "*"
            };

        /// <summary>
        /// Parses a game result from a string.
        /// </summary>
        /// <param name="result">String representation of the game result.</param>
        /// <returns>The game result.</returns>
        public static GameResult ResultFromString(string result) =>
            result switch
            {
                "0-1" => GameResult.BlackWins,
                "1-0" => GameResult.WhiteWins,
                "1/2-1/2" => GameResult.Draw,
                "*" => GameResult.Ongoing,
                _ => throw new ArgumentException("Invalid result string", nameof(result))
            };

        /// <summary>
        /// Converts a chess piece into a string representation.
        /// </summary>
        /// <param name="piece">The piece to convert to a string.</param>
        /// <param name="fenStyle">True to use lower-case characters for black pieces.</param>
        /// <param name="pForPawn">True to use 'P' for pawns, otherwise pawns convert to empty strings.</param>
        /// <returns>A string representation of the chess piece.</returns>
        public static string PieceToString(ChessPieces piece, bool fenStyle = false, bool pForPawn = false)
        {
            var ret = piece switch
            {
                ChessPieces.WhiteKing => "K",
                ChessPieces.BlackKing => "k",
                ChessPieces.WhiteQueen => "Q",
                ChessPieces.BlackQueen => "q",
                ChessPieces.WhiteRook => "R",
                ChessPieces.BlackRook => "r",
                ChessPieces.WhiteBishop => "B",
                ChessPieces.BlackBishop => "b",
                ChessPieces.WhiteKnight => "N",
                ChessPieces.BlackKnight => "n",
                ChessPieces.WhitePawn => pForPawn ? "P" : string.Empty,
                ChessPieces.BlackPawn => pForPawn ? "p" : string.Empty,
                _ => "\uFFFD",
            };

            if (!fenStyle)
            {
                ret = ret.ToUpperInvariant();
            }

            return ret;
        }

        /// <summary>
        /// Get a chess piece from a string descrption.
        /// </summary>
        /// <param name="piece">A string representing a chess piece.</param>
        /// <returns>A chess piece.</returns>
        /// <remarks>Capital letters are interpreted as white pieces and lower case letters as black pieces (as in FEN notation).</remarks>
        public static ChessPieces PieceFromString(string piece)
        {
            return piece switch
            {
                "K" => ChessPieces.WhiteKing,
                "k" => ChessPieces.BlackKing,
                "Q" => ChessPieces.WhiteQueen,
                "q" => ChessPieces.BlackQueen,
                "R" => ChessPieces.WhiteRook,
                "r" => ChessPieces.BlackRook,
                "B" => ChessPieces.WhiteBishop,
                "b" => ChessPieces.BlackBishop,
                "N" => ChessPieces.WhiteKnight,
                "n" => ChessPieces.BlackKnight,
                "P" => ChessPieces.WhitePawn,
                "p" => ChessPieces.BlackPawn,
                _ => throw new ArgumentException("Invalid piece identifier", nameof(piece))
            };
        }

        /// <summary>
        /// Formats a chess move in the UCI protocol format.
        /// The UCI protocol calls this format long algebraic notation, but it
        /// differs from standard long algebraic notation in several ways. It does
        /// not separate original and final positions with - or x and it does denotes
        /// castling by just returning the beginning and ending square for the king.
        /// http://wbec-ridderkerk.nl/html/UCIProtocol.html.
        /// </summary>
        /// <param name="move">The move to format for UCI use.</param>
        /// <returns>A chess move in UCI format.</returns>
        public static string MoveToUCINotation(Move move) =>
            $"{move.OriginalPosition}{move.FinalPosition}{(move.PiecePromotedTo.HasValue ? PieceToString(move.PiecePromotedTo.Value, false, false).ToLowerInvariant() : string.Empty)}";

        /// <summary>
        /// Formats a chess move in the long algebraic notation.
        /// https://en.wikipedia.org/wiki/Algebraic_notation_(chess)#Long_algebraic_notation.
        /// </summary>
        /// <param name="move">The move to format in long algebraic notation.</param>
        /// <returns>A standard long algebraic representation of the move.</returns>
        public static string MoveToLongAlgebraicNotation(Move move)
        {
            if (move.ShortCastle)
            {
                return "O-O";
            }

            if (move.LongCastle)
            {
                return "O-O-O";
            }

            return $"{PieceToString(move.PieceMoved, false, false)}" + // Piece moved
                   $"{move.OriginalPosition}" + // Original position
                   $"{(move.Capture ? "x" : "-")}" + // Capture or move
                   $"{move.FinalPosition}" + // Final position
                   $"{(move.PiecePromotedTo.HasValue ? $"={PieceToString(move.PiecePromotedTo.Value, false, false)}" : string.Empty)}"; // Promotion disabiguation
        }

        /// <summary>
        /// Format a chess move in standard algebraic notation. https://en.wikipedia.org/wiki/Algebraic_notation_(chess).
        /// </summary>
        /// <param name="move">The move to convert to a string.</param>
        /// <returns>A standard algebraic notation description of the move.</returns>
        public static string MoveToString(Move move)
        {
            var output = new StringBuilder();

            if (move.ShortCastle)
            {
                return "O-O";
            }

            if (move.LongCastle)
            {
                return "O-O-O";
            }

            // Begin with the piece type unless the piece was a pawn,
            // in which case the original file is used for captures and an
            // empty string for moves.
            var pieceMoved = PieceToString(move.PieceMoved);
            output.Append(
                string.IsNullOrEmpty(pieceMoved) ?
                (move.Capture ? FileToString(move.OriginalPosition.File) : string.Empty) :
                pieceMoved);

            if (move.AmbiguousOriginalFile)
            {
                output.Append(FileToString(move.OriginalPosition.File));
            }

            if (move.AmbiguousOriginalRank)
            {
                output.Append(RankToString(move.OriginalPosition.Rank));
            }

            if (move.Capture)
            {
                output.Append("x");
            }

            output.Append($"{move.FinalPosition}");

            if (move.PiecePromotedTo != null)
            {
                output.Append($"={PieceToString(move.PiecePromotedTo.Value)}");
            }

            if (move.Checkmates)
            {
                output.Append("#");
            }
            else if (move.Checks)
            {
                output.Append("+");
            }

            return output.ToString();
        }

        /// <summary>
        /// Formats a series of chess moves into a string using standard algebraic notation.
        /// </summary>
        /// <param name="moves">The collection of moves to convert to a string.</param>
        /// <param name="firstMoveCount">The move number for the first move in moves.</param>
        /// <returns>A string with standard algebraic notation of the moves preceeded by the move count.</returns>
        public static string MovesToString(IEnumerable<Move> moves, int firstMoveCount = 1)
        {
            var movesString = new StringBuilder();
            var lineBreak = 0;
            var moveCount = firstMoveCount;

            foreach (var move in moves)
            {
                var whiteMove = IsPieceWhite(move.PieceMoved);

                if (whiteMove)
                {
                    movesString.Append($"{moveCount}. {MoveToString(move)} ");
                }
                else
                {
                    if (movesString.Length == 0)
                    {
                        // In the rare case we start with black to move, use this notation
                        movesString.Append($"{moveCount}... ");
                    }

                    movesString.Append($"{MoveToString(move)} ");
                }

                if (movesString.Length - lineBreak > 74)
                {
                    // PGN lines should not be more than 80 characters. If line length is getting close
                    // to that, remove the trailing space and go to the next line.
                    movesString.Remove(movesString.Length - 1, 1);
                    movesString.AppendLine();
                    lineBreak = movesString.Length;
                }

                if (!whiteMove)
                {
                    moveCount++;
                }
            }

            return movesString.ToString().TrimEnd();
        }
    }
}
