using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using MjrChess.Engine.Models;
using MjrChess.Engine.Utilities;

namespace MjrChess.Engine
{
    /// <summary>
    /// Analysis engine for determining legal moves, best moves, etc. from a chess position.
    /// </summary>
    public class ChessEngine
    {
        /// <summary>
        /// Regex for parsing a PGN representation of a chess game.
        /// </summary>
        private static readonly Regex PgnRegex =

            // :       Tag pairs with the form `[Name "Value"]`          Move list with the form `#. WhiteMove BlackMove `                Result with the form `1-0|0-1|1/2-1/2`
            new Regex("(?:\\[(?'key'.+?)\\s+\\\"(?'value'.+)\\\"\\]\\s+)+(?:\\d+\\.\\s*(?'whiteMove'\\S+)\\s+(?:(?'blackMove'\\S+)\\s+)?)+(?'result'1\\-0|0\\-1|1\\/2\\-1\\/2)?", RegexOptions.Compiled);

        /// <summary>
        /// Regex for parsing a chess move from (long or short) algebraic notation.
        /// </summary>
        private static readonly Regex AlgebraicNotationRegex =

            // This doesn't capture whether the move checks, checkmates, or captures since those aren't necessary to find the move from legal moves
            // If we needed that information, the x could be captured and `(?:(?'checks'\+)|(?'checkmates'#))?` could be added to the end of the expression
            // (after wrapping it as a non-capturing group). Doing that now, though, would just make the expressions a bit slower and more complicated unnecessarilly.
            // :       Long castle `O-O-O`       Short castle `O-O`   Piece moved       File disambiguator     Rank disambiguator     Capture?  Final position              Piece promoted to
            new Regex("(?'longCastle'O\\-O\\-O)|(?'shortCastle'O\\-O)|(?'piece'[KQRBN])?(?'originalFile'[a-h])?(?'originalRank'[1-8])?(?:\\-|x)?(?'finalPosition'[a-h][1-8])(=(?'promotedTo'[QRBN]))?");

        /// <summary>
        /// Gets legal moves for a rook.
        /// </summary>
        private static IEnumerable<Func<BoardPosition, BoardPosition>> RookMovements => new Func<BoardPosition, BoardPosition>[]
        {
            position => new BoardPosition(position.File + 1, position.Rank),
            position => new BoardPosition(position.File - 1, position.Rank),
            position => new BoardPosition(position.File, position.Rank + 1),
            position => new BoardPosition(position.File, position.Rank - 1)
        };

        /// <summary>
        /// Gets legal moves for a bishop.
        /// </summary>
        private static IEnumerable<Func<BoardPosition, BoardPosition>> BishopMovements => new Func<BoardPosition, BoardPosition>[]
        {
            position => new BoardPosition(position.File + 1, position.Rank + 1),
            position => new BoardPosition(position.File - 1, position.Rank + 1),
            position => new BoardPosition(position.File + 1, position.Rank - 1),
            position => new BoardPosition(position.File - 1, position.Rank - 1)
        };

        /// <summary>
        /// Gets legal moves for a knight.
        /// </summary>
        private static IEnumerable<Func<BoardPosition, BoardPosition>> KnightMovements => new Func<BoardPosition, BoardPosition>[]
        {
            position => new BoardPosition(position.File + 2, position.Rank + 1),
            position => new BoardPosition(position.File + 2, position.Rank - 1),
            position => new BoardPosition(position.File - 2, position.Rank + 1),
            position => new BoardPosition(position.File - 2, position.Rank - 1),
            position => new BoardPosition(position.File + 1, position.Rank + 2),
            position => new BoardPosition(position.File - 1, position.Rank + 2),
            position => new BoardPosition(position.File + 1, position.Rank - 2),
            position => new BoardPosition(position.File - 1, position.Rank - 2)
        };

        /// <summary>
        /// Gets legal moves for a king or queen.
        /// </summary>
        private static IEnumerable<Func<BoardPosition, BoardPosition>> RoyalMovements => RookMovements.Union(BishopMovements);

        /// <summary>
        /// Gets the chess game being analyzed by the engine.
        /// </summary>
        public ChessGame Game { get; internal set; }

        // This is created as an instance class (even though identifying legal moves
        // could easily be accomplished by a static class) to facilitate more easily expanding
        // this class to a full UCI-compatible analysis engine in the future (which makes sense
        // to have an instance class for, for pondering, etc.).
        // UCI Spec: http://download.shredderchess.com/div/uci.zip
        public ChessEngine()
        {
            Game = new ChessGame();
        }

        /// <summary>
        /// Loads chess game into the analysis engine.
        /// </summary>
        /// <param name="fen">A FEN description of the game to load.</param>
        public void LoadFEN(string fen)
        {
            Game.LoadFEN(fen);
        }

        /// <summary>
        /// Loads chess game from a PGN string.
        /// </summary>
        /// <param name="pgn">A PGN description of the game to load.</param>
        public void LoadPGN(string pgn)
        {
            if (pgn is null)
            {
                throw new ArgumentNullException(nameof(pgn));
            }

            // Parse PGN
            var match = PgnRegex.Match(pgn);
            if (!match.Success)
            {
                throw new ArgumentException("Invalid PGN string; could not be parsed.", nameof(pgn));
            }

            // Get tag pairs, moves, and result from the parsed output.
            var tagKeys = match.Groups["key"].Captures.OfType<Capture>().Select(c => c.Value).ToArray();
            var tagValues = match.Groups["value"].Captures.OfType<Capture>().Select(c => c.Value).ToArray();
            var whiteMoves = match.Groups["whiteMove"].Captures.OfType<Capture>().Select(c => c.Value).ToArray();
            var blackMoves = match.Groups["blackMove"].Captures.OfType<Capture>().Select(c => c.Value).ToArray();
            var result = match.Groups["result"].Value;

            if (tagKeys.Length != tagValues.Length)
            {
                throw new ArgumentException("Invalid PGN string; invalid tag pairs.", nameof(pgn));
            }

            // Create new game and assign properties based on PGN tag pairs
            Game = new ChessGame();
            for (var i = 0; i < tagKeys.Length; i++)
            {
                var val = tagValues[i];
                switch (tagKeys[i].ToLowerInvariant())
                {
                    case "event":
                        Game.Event = val;
                        break;
                    case "site":
                        Game.Site = val;
                        break;
                    case "date":
                        Game.StartDate = DateTimeOffset.ParseExact(val, "yyyy.MM.dd", CultureInfo.InvariantCulture);
                        break;
                    case "round":
                        Game.Round = val;
                        break;
                    case "white":
                        Game.WhitePlayer = val;
                        break;
                    case "black":
                        Game.BlackPlayer = val;
                        break;
                    case "result":
                        Game.Result = ChessFormatter.ResultFromString(val);
                        break;
                    case "fen":
                        Game.LoadFEN(val);
                        break;
                    default:
                        break;
                }
            }

            // Game.Result defaults to Ongoing. If it has that value after evaluating
            // tag pairs, check whether the result notation at the end of the move list
            // gives the result.
            if (Game.Result == GameResult.Ongoing)
            {
                Game.Result = ChessFormatter.ResultFromString(result);
            }

            // Apply moves
            for (var i = 0; i < whiteMoves.Length; i++)
            {
                var whiteMove = MoveFromAlgebraicNotation(whiteMoves[i]);
                Game.Move(whiteMove);

                if (i < blackMoves.Length)
                {
                    var blackMove = MoveFromAlgebraicNotation(blackMoves[i]);
                    Game.Move(blackMove);
                }
            }
        }

        /// <summary>
        /// Loads a chess game into the analysis engine.
        /// </summary>
        /// <param name="game">The game to load.</param>
        public void LoadPosition(ChessGame game)
        {
            Game = game;
        }

        /// <summary>
        /// Retrieves potential legal moves for a given piece.
        /// </summary>
        /// <param name="position">The position of the piece to move.</param>
        /// <returns>All possible legal moves for the piece.</returns>
        public IEnumerable<Move> GetLegalMoves(BoardPosition position)
        {
            var pieceToMove = Game.GetPiece(position);
            if (pieceToMove != null)
            {
                var possibleMoves = GetMoveOptions(pieceToMove).Where(IsLegal);
                foreach (var move in possibleMoves)
                {
                    CheckForAmbiguousMove(move);
                    CheckForCheck(move);
                    CheckForMate(move);
                    yield return move;
                }
            }
        }

        /// <summary>
        /// Returns all possible squares a piece can move to, not considering
        /// whether the move is legal (due to check) or whether the move results
        /// in check, checkmate, or an ambiguous move.
        /// </summary>
        /// <param name="piece">The piece to move.</param>
        /// <returns>A complete list of possible ending squares.</returns>
        public IEnumerable<Move> GetMoveOptions(ChessPiece piece) =>
            piece?.PieceType switch
            {
                ChessPieces.WhitePawn => GetPawnMoves(piece),
                ChessPieces.BlackPawn => GetPawnMoves(piece),
                ChessPieces.WhiteRook => GetRookMoves(piece),
                ChessPieces.BlackRook => GetRookMoves(piece),
                ChessPieces.WhiteBishop => GetBishopMoves(piece),
                ChessPieces.BlackBishop => GetBishopMoves(piece),
                ChessPieces.WhiteQueen => GetQueenMoves(piece),
                ChessPieces.BlackQueen => GetQueenMoves(piece),
                ChessPieces.WhiteKnight => GetKnightMoves(piece),
                ChessPieces.BlackKnight => GetKnightMoves(piece),
                ChessPieces.WhiteKing => GetKingMoves(piece),
                ChessPieces.BlackKing => GetKingMoves(piece),
                _ => Enumerable.Empty<Move>()
            };

        /// <summary>
        /// Checks whether a move would be amgibuous expressed in short algebraic notation and sets ambiguous properties, if necessary.
        /// </summary>
        /// <param name="move">The move to check.</param>
        private void CheckForAmbiguousMove(Move move)
        {
            var similarPieces = Game.Pieces.Where(p => p.PieceType == move.PieceMoved);

            foreach (var piece in similarPieces)
            {
                if (piece.Position == move.OriginalPosition)
                {
                    continue;
                }

                if (GetMoveOptions(piece).Where(IsLegal).Select(m => m.FinalPosition).Contains(move.FinalPosition))
                {
                    if (piece.Position.File != move.OriginalPosition.File)
                    {
                        move.AmbiguousOriginalFile = true;
                    }
                    else
                    {
                        move.AmbiguousOriginalRank = true;
                    }
                }
            }
        }

        /// <summary>
        /// Checks whether the given move gives check.
        /// </summary>
        /// <param name="move">The move to check.</param>
        private void CheckForCheck(Move move)
        {
            var whiteMoving = ChessFormatter.IsPieceWhite(move.PieceMoved);

            // Create a hypothetical board state if the propose move was made
            var prospectiveGame = new ChessGame();
            prospectiveGame.LoadFEN(Game.GetFEN());
            prospectiveGame.Move(move);
            var prospectiveEngine = new ChessEngine { Game = prospectiveGame };

            var friendlyPieces = prospectiveGame.Pieces.Where(p => ChessFormatter.IsPieceWhite(p.PieceType) == whiteMoving);
            var opposingKing = prospectiveGame.Pieces.Single(p => p.PieceType == (whiteMoving ? ChessPieces.BlackKing : ChessPieces.WhiteKing));

            foreach (var piece in friendlyPieces)
            {
                // If any friendly piece could move to the opposing king's square, then the move gives check
                // We don't need to check for legal moves because even pinned pieces or others that can't move because
                // of a risk of check from the opponent are still able to give check.
                if (prospectiveEngine.GetMoveOptions(piece).Select(m => m.FinalPosition).Contains(opposingKing.Position))
                {
                    move.Checks = true;
                    if (move.Stalemates)
                    {
                        move.Stalemates = false;
                        move.Checkmates = true;
                    }

                    break;
                }
            }
        }

        /// <summary>
        /// Checks whether a given move gives checkmate.
        /// </summary>
        /// <param name="move">The move to check.</param>
        private void CheckForMate(Move move)
        {
            // Create a hypothetical board state if the propose move was made
            var prospectiveGame = new ChessGame();
            prospectiveGame.LoadFEN(Game.GetFEN());
            prospectiveGame.Move(move);
            var prospectiveEngine = new ChessEngine { Game = prospectiveGame };
            var opposingPieces = prospectiveGame.Pieces.Where(p => ChessFormatter.IsPieceWhite(p.PieceType) != ChessFormatter.IsPieceWhite(move.PieceMoved));

            // Calculate all possible legal moves for the opponent after the proposed move. If there are no moves available, then it is mate.
            // If I develop this into a fuller engine in the future, this method may be absorbed into more general calculating functionality.
            var possibleMoves = opposingPieces.SelectMany(p => prospectiveEngine.GetMoveOptions(p).Where(prospectiveEngine.IsLegal));

            if (!possibleMoves.Any())
            {
                if (move.Checks)
                {
                    move.Checkmates = true;
                }
                else
                {
                    move.Stalemates = true;
                }
            }
        }

        /// <summary>
        /// Check whether a given possible move is legal (based on whether it leaves the king in check).
        /// </summary>
        /// <param name="move">The move to analyze.</param>
        /// <returns>True if the move is legal, false otherwise.</returns>
        private bool IsLegal(Move move)
        {
            var whiteMoving = ChessFormatter.IsPieceWhite(move.PieceMoved);

            // Create a hypothetical board state if the propose move was made
            var prospectiveGame = new ChessGame();
            prospectiveGame.LoadFEN(Game.GetFEN());
            prospectiveGame.Move(move);
            var prospectiveEngine = new ChessEngine { Game = prospectiveGame };

            var opposingPieces = prospectiveGame.Pieces.Where(p => ChessFormatter.IsPieceWhite(p.PieceType) != whiteMoving);
            var friendlyKing = prospectiveGame.Pieces.Single(p => p.PieceType == (whiteMoving ? ChessPieces.WhiteKing : ChessPieces.BlackKing));

            // If any of the opponent's pieces could capture the king after this move, then it is illegal
            var kingVulnerable = opposingPieces.Any(piece => prospectiveEngine.GetMoveOptions(piece).Select(m => m.FinalPosition).Contains(friendlyKing.Position));

            // Special case castling as the king can't move out of or through a threatened square while castling
            if (move.LongCastle || move.ShortCastle)
            {
                var traversedFiles = move.LongCastle ? new int[] { 4, 3 } : new int[] { 4, 5 };
                var traversedSquares = traversedFiles.Select(file => new BoardPosition(file, move.OriginalPosition.Rank));
                kingVulnerable |= traversedSquares.Any(traversedSquare =>
                    opposingPieces.Any(piece => prospectiveEngine.GetMoveOptions(piece).Select(m => m.FinalPosition).Contains(traversedSquare)));
            }

            return !kingVulnerable;
        }

        /// <summary>
        /// Gets possible (unvalidated) moves for a pawn based on the current board position.
        /// </summary>
        /// <remarks>If the pawn would be promoted, indicate queen promotion with the intention that the caller will see this and adjust, if necessary.</remarks>
        /// <param name="pawn">The pawn to move.</param>
        /// <returns>Possible squares to move to (considering the position of the pawn and other pieces, but not considering check).</returns>
        private IEnumerable<Move> GetPawnMoves(ChessPiece pawn)
        {
            var whitePawn = ChessFormatter.IsPieceWhite(pawn.PieceType);
            var rankProgresion = whitePawn ? 1 : -1;

            // Check one-space advance
            var finalPosition = new BoardPosition(pawn.Position.File, pawn.Position.Rank + rankProgresion);
            if (Game.GetPiece(finalPosition) == null)
            {
                yield return CreateMoveFromPiece(pawn, finalPosition, false);

                // Check two-space advance
                finalPosition = new BoardPosition(pawn.Position.File, pawn.Position.Rank + (2 * rankProgresion));
                if (pawn.Position.Rank == (whitePawn ? 1 : 6) && Game.GetPiece(finalPosition) == null)
                {
                    yield return CreateMoveFromPiece(pawn, finalPosition, false);
                }
            }

            // Check captures
            var captureFiles = new[] { pawn.Position.File - 1, pawn.Position.File + 1 };
            foreach (var captureFile in captureFiles)
            {
                finalPosition = new BoardPosition(captureFile, pawn.Position.Rank + rankProgresion);
                var pieceAtDestination = Game.GetPiece(finalPosition);
                if (Game.EnPassantTarget == finalPosition || (pieceAtDestination != null && ChessFormatter.IsPieceWhite(pieceAtDestination.PieceType) != whitePawn))
                {
                    yield return CreateMoveFromPiece(pawn, finalPosition, true);
                }
            }
        }

        /// <summary>
        /// Gets possible (unvalidated) moves for a king based on the current board position.
        /// </summary>
        /// <param name="king">The king to move.</param>
        /// <returns>Possible squares to move to (considering the position of the king and other pieces, but not considering check).</returns>
        private IEnumerable<Move> GetKingMoves(ChessPiece king)
        {
            var whiteKing = ChessFormatter.IsPieceWhite(king.PieceType);

            // Check for castling
            if (whiteKing)
            {
                if ((Game.WhiteCastlingOptions & CastlingOptions.KingSide) == CastlingOptions.KingSide &&
                    Game.GetPiece(5, 0) == null &&
                    Game.GetPiece(6, 0) == null)
                {
                    yield return CreateMoveFromPiece(king, new BoardPosition(6, 0), false);
                }

                if ((Game.WhiteCastlingOptions & CastlingOptions.QueenSide) == CastlingOptions.QueenSide &&
                    Game.GetPiece(3, 0) == null &&
                    Game.GetPiece(2, 0) == null &&
                    Game.GetPiece(1, 0) == null)
                {
                    yield return CreateMoveFromPiece(king, new BoardPosition(2, 0), false);
                }
            }
            else
            {
                if ((Game.BlackCastlingOptions & CastlingOptions.KingSide) == CastlingOptions.KingSide &&
                    Game.GetPiece(5, 7) == null &&
                    Game.GetPiece(6, 7) == null)
                {
                    yield return CreateMoveFromPiece(king, new BoardPosition(6, 7), false);
                }

                if ((Game.BlackCastlingOptions & CastlingOptions.QueenSide) == CastlingOptions.QueenSide &&
                    Game.GetPiece(3, 7) == null &&
                    Game.GetPiece(2, 7) == null &&
                    Game.GetPiece(1, 7) == null)
                {
                    yield return CreateMoveFromPiece(king, new BoardPosition(2, 7), false);
                }
            }

            // Check for normal moves
            foreach (var applyMovement in RoyalMovements)
            {
                var finalPosition = applyMovement(king.Position);
                if (finalPosition.File >= 0 &&
                    finalPosition.File < Game.BoardSize &&
                    finalPosition.Rank >= 0 &&
                    finalPosition.Rank < Game.BoardSize)
                {
                    var pieceAtDestination = Game.GetPiece(finalPosition);
                    if (pieceAtDestination == null)
                    {
                        yield return CreateMoveFromPiece(king, finalPosition, false);
                    }
                    else if (whiteKing != ChessFormatter.IsPieceWhite(pieceAtDestination.PieceType))
                    {
                        yield return CreateMoveFromPiece(king, finalPosition, true);
                    }
                }
            }
        }

        /// <summary>
        /// Gets possible (unvalidated) moves for a knight based on the current board position.
        /// </summary>
        /// <param name="knight">The knight to move.</param>
        /// <returns>Possible squares to move to (considering the position of the knight and other pieces, but not considering check).</returns>
        private IEnumerable<Move> GetKnightMoves(ChessPiece knight)
        {
            foreach (var applyMovement in KnightMovements)
            {
                var finalPosition = applyMovement(knight.Position);
                if (finalPosition.File >= 0 &&
                    finalPosition.File < Game.BoardSize &&
                    finalPosition.Rank >= 0 &&
                    finalPosition.Rank < Game.BoardSize)
                {
                    var pieceAtDestination = Game.GetPiece(finalPosition);
                    if (pieceAtDestination == null)
                    {
                        yield return CreateMoveFromPiece(knight, finalPosition, false);
                    }
                    else if (ChessFormatter.IsPieceWhite(knight.PieceType) != ChessFormatter.IsPieceWhite(pieceAtDestination.PieceType))
                    {
                        yield return CreateMoveFromPiece(knight, finalPosition, true);
                    }
                }
            }
        }

        /// <summary>
        /// Gets possible (unvalidated) moves for a rook based on the current board position.
        /// </summary>
        /// <param name="rook">The rook to move.</param>
        /// <returns>Possible squares to move to (considering the position of the rook and other pieces, but not considering check).</returns>
        private IEnumerable<Move> GetRookMoves(ChessPiece rook) => GetStraightPieceMoves(rook, RookMovements);

        /// <summary>
        /// Gets possible (unvalidated) moves for a bishop based on the current board position.
        /// </summary>
        /// <param name="bishop">The bishop to move.</param>
        /// <returns>Possible squares to move to (considering the position of the bishop and other pieces, but not considering check).</returns>
        private IEnumerable<Move> GetBishopMoves(ChessPiece bishop) => GetStraightPieceMoves(bishop, BishopMovements);

        /// <summary>
        /// Gets possible (unvalidated) moves for a queen based on the current board position.
        /// </summary>
        /// <param name="queen">The queen to move.</param>
        /// <returns>Possible squares to move to (considering the position of the queen and other pieces, but not considering check).</returns>
        private IEnumerable<Move> GetQueenMoves(ChessPiece queen) => GetStraightPieceMoves(queen, RoyalMovements);

        private IEnumerable<Move> GetStraightPieceMoves(ChessPiece piece, IEnumerable<Func<BoardPosition, BoardPosition>> movements)
        {
            foreach (var applyMovement in movements)
            {
                var finalPosition = applyMovement(piece.Position);

                while (finalPosition.File >= 0 &&
                    finalPosition.File < Game.BoardSize &&
                    finalPosition.Rank >= 0 &&
                    finalPosition.Rank < Game.BoardSize)
                {
                    var pieceAtDestination = Game.GetPiece(finalPosition);
                    if (pieceAtDestination == null)
                    {
                        yield return CreateMoveFromPiece(piece, finalPosition, false);
                    }
                    else if (ChessFormatter.IsPieceWhite(piece.PieceType) != ChessFormatter.IsPieceWhite(pieceAtDestination.PieceType))
                    {
                        yield return CreateMoveFromPiece(piece, finalPosition, true);
                        break;
                    }
                    else
                    {
                        break;
                    }

                    finalPosition = applyMovement(finalPosition);
                }
            }
        }

        /// <summary>
        /// Creates a move based on piece to be moved, final position, and whether the move is a capture.
        /// </summary>
        /// <param name="piece">The piece to be moved.</param>
        /// <param name="finalPosition">The board square to move the piece to.</param>
        /// <param name="capture">Whether the move captures another piece.</param>
        /// <returns>A move representing the indicated piece movement.</returns>
        private Move CreateMoveFromPiece(ChessPiece piece, BoardPosition finalPosition, bool capture)
        {
            var promoted = (piece.PieceType == ChessPieces.WhitePawn || piece.PieceType == ChessPieces.BlackPawn) && finalPosition.Rank % (Game.BoardSize - 1) == 0;
            return new Move(piece.PieceType, piece.Position, finalPosition)
            {
                Capture = capture,
                PiecePromotedTo = promoted ? (ChessFormatter.IsPieceWhite(piece.PieceType) ? ChessPieces.WhiteQueen : ChessPieces.BlackQueen) : (ChessPieces?)null
            };
        }

        /// <summary>
        /// Gets a move from the algebraic representation of the move. Uses the current game state to resolve ambiguity (like which piece was moved).
        /// </summary>
        /// <param name="move">A string representing a move in algebraic notation.</param>
        /// <returns>The move indicated by the input string.</returns>
        public Move MoveFromAlgebraicNotation(string move)
        {
            if (move is null)
            {
                throw new ArgumentNullException(nameof(move));
            }

            var match = AlgebraicNotationRegex.Match(move);

            if (!match.Success)
            {
                throw new ArgumentException("Move is not valid algebraic notation", nameof(move));
            }

            // These locals will store what we know about the move from the parsed string
            // and will be used to look up the move from legal possibilities.
            ChessPieces? pieceMoved = null;
            ChessPieces? promotedTo = null;
            int? originalRank = null;
            int? originalFile = null;
            BoardPosition finalPosition = default;

            if (match.Groups["longCastle"].Success)
            {
                // Check for castling
                (pieceMoved, finalPosition) = Game.WhiteToMove
                    ? (ChessPieces.WhiteKing, new BoardPosition(2, 0))
                    : (ChessPieces.BlackKing, new BoardPosition(2, 7));
            }
            else if (match.Groups["shortCastle"].Success)
            {
                (pieceMoved, finalPosition) = Game.WhiteToMove
                    ? (ChessPieces.WhiteKing, new BoardPosition(6, 0))
                    : (ChessPieces.BlackKing, new BoardPosition(6, 7));
            }
            else
            {
                // If not castling, identify piece type, disambiguators, final position, and promotion
                finalPosition = new BoardPosition(match.Groups["finalPosition"].Value);

                // Note the original file if specified
                if (match.Groups["originalFile"].Success)
                {
                    originalFile = ChessFormatter.FileFromString(match.Groups["originalFile"].Value);
                }

                // Note the original rank if specified
                if (match.Groups["originalRank"].Success)
                {
                    originalRank = ChessFormatter.RankFromString(match.Groups["originalRank"].Value);
                }

                // Note the piece promoted to if specified
                if (match.Groups["promotedTo"].Success)
                {
                    var promotedToString = match.Groups["promotedTo"].Value;
                    if (!Game.WhiteToMove)
                    {
                        promotedToString = promotedToString.ToLowerInvariant();
                    }

                    promotedTo = ChessFormatter.PieceFromString(promotedToString);
                }

                var pieceMatch = match.Groups["piece"];
                if (pieceMatch.Success)
                {
                    // If the piece type was specified, use that to identify piece moved.
                    var pieceString = match.Groups["piece"].Value;

                    // If black is to move next, make the piece string lower case so that it
                    // is interpretted by the formatted as a black piece.
                    if (!Game.WhiteToMove)
                    {
                        pieceString = pieceString.ToLowerInvariant();
                    }

                    pieceMoved = ChessFormatter.PieceFromString(pieceString);
                }
                else if (originalRank.HasValue && originalFile.HasValue)
                {
                    // If the piece type isn't specified, look for original rank and file since some algebraic
                    // notations include these but omit piece type.
                    var originalPosition = new BoardPosition(originalFile.Value, originalRank.Value);
                    pieceMoved = Game.GetPiece(originalPosition)?.PieceType
                        ?? throw new ArgumentException($"Cannot make move {move} from {originalPosition}; there is no piece there.", nameof(move));
                }
                else
                {
                    // Default to pawn
                    pieceMoved = Game.WhiteToMove ? ChessPieces.WhitePawn : ChessPieces.BlackPawn;
                }
            }

            // Identify possible pieces moved
            var possiblePieces = Game.Pieces.Where(p => p.PieceType == pieceMoved);
            if (originalFile.HasValue)
            {
                possiblePieces = possiblePieces.Where(p => p.Position.File == originalFile);
            }

            if (originalRank.HasValue)
            {
                possiblePieces = possiblePieces.Where(p => p.Position.Rank == originalRank);
            }

            // Find move
            var possibleMoves = possiblePieces.SelectMany(p => GetLegalMoves(p.Position))
                .Where(m => m.FinalPosition == finalPosition);

            if (possibleMoves.Count() > 1)
            {
                throw new ArgumentException($"The move {move} is ambiguous", nameof(move));
            }

            if (possibleMoves.Count() == 0)
            {
                throw new ArgumentException($"The move {move} is impossible", nameof(move));
            }

            var resolvedMove = possibleMoves.Single();

            if (promotedTo.HasValue)
            {
                resolvedMove.PiecePromotedTo = promotedTo;
            }

            return resolvedMove;
        }
    }
}
