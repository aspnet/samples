using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using MjrChess.Engine.Utilities;

namespace MjrChess.Engine.Models
{
    /// <summary>
    /// Represents a chess game, including initial starting position, moves, and metadata.
    /// </summary>
    public class ChessGame
    {
        private const int DefaultBoardSize = 8;
        private const string InitialGameFEN = "rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq - 0 1";
        private const char FenRankDelimiter = '/';

        // Stores which piece (if any) is on each board space
        private ChessPiece?[][] boardState = new ChessPiece[DefaultBoardSize][];

        /// <summary>
        /// Gets or sets the number of squares on one side of the chess board.
        /// </summary>
        public int BoardSize { get; set; } = DefaultBoardSize;

        /// <summary>
        /// Gets or sets the position the game began from in FEN notation.
        /// </summary>
        public string StartingFEN { get; set; } = InitialGameFEN;

        /// <summary>
        /// Gets or sets moves since initial position.
        /// </summary>
        public IList<Move> Moves { get; set; } = new List<Move>();

        /// <summary>
        /// Gets or sets the name of the event this game was a part of.
        /// </summary>
        public string? Event { get; set; }

        /// <summary>
        /// Gets or sets the location of the event.
        /// </summary>
        public string? Site { get; set; }

        /// <summary>
        /// Gets or sets the playing round of the event this game was played in.
        /// </summary>
        public string? Round { get; set; }

        /// <summary>
        /// Gets or sets the day the game started.
        /// </summary>
        public DateTimeOffset StartDate { get; set; }

        /// <summary>
        /// Gets pieces currently on the board.
        /// </summary>
        public IEnumerable<ChessPiece> Pieces
        {
            get
            {
                for (var file = 0; file < BoardSize; file++)
                {
                    for (var rank = 0; rank < BoardSize; rank++)
                    {
                        var squareState = boardState[file][rank];
                        if (squareState != null)
                        {
                            yield return squareState;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Callback delegate for handling OnMove events.
        /// </summary>
        /// <param name="sender">The chess game in which the move was made.</param>
        /// <param name="move">The move that was made.</param>
        public delegate void MoveHandler(ChessGame sender, Move move);

        /// <summary>
        /// Event that is raised when a move is made.
        /// </summary>
        public event MoveHandler? OnMove;

        /// <summary>
        /// Gets or sets name of the player with the white pieces (if known).
        /// </summary>
        public string? WhitePlayer { get; set; }

        /// <summary>
        /// Gets or sets name of the player with the black pieces (if known).
        /// </summary>
        public string? BlackPlayer { get; set; }

        /// <summary>
        /// Gets an integer representing white's advantage (or disadvantage).
        /// </summary>
        public int WhiteAdvantage => Pieces.Sum(p => ChessFormatter.GetPieceValue(p.PieceType));

        /// <summary>
        /// Gets or sets the game's result.
        /// </summary>
        public GameResult Result { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether it's the white player's turn to move.
        /// </summary>
        public bool WhiteToMove { get; set; }

        /// <summary>
        /// Gets or sets castling options available to the black pieces.
        /// </summary>
        public CastlingOptions BlackCastlingOptions { get; set; }

        /// <summary>
        /// Gets or sets castling options available to the white pieces.
        /// </summary>
        public CastlingOptions WhiteCastlingOptions { get; set; }

        /// <summary>
        /// Gets or sets the square an en passant capture can be made to (if any).
        /// </summary>
        public BoardPosition? EnPassantTarget { get; set; }

        /// <summary>
        /// Gets or sets the number of half moves since a pawn was moved or a piece was captured.
        /// </summary>
        public int HalfMoveClock { get; set; }

        /// <summary>
        /// Gets or sets the number of full moves made (beginning with 1 are the start of the game).
        /// </summary>
        public int MoveCount { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether a player's king is in check.
        /// </summary>
        public bool Check { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ChessGame"/> class, setup for a new standard chess game.
        /// </summary>
        public ChessGame()
        {
            LoadFEN(InitialGameFEN);
        }

        /// <summary>
        /// Clears the board and all game state.
        /// </summary>
        public void ClearGameState()
        {
            Moves = new List<Move>();
            WhitePlayer = "White Player";
            BlackPlayer = "Black Player";
            Round = "-";
            StartDate = DateTimeOffset.Now;
            WhiteToMove = true;
            WhiteCastlingOptions =
                BlackCastlingOptions = CastlingOptions.KingSide | CastlingOptions.QueenSide;
            EnPassantTarget = null;
            Result = GameResult.Ongoing;
            HalfMoveClock = 0;
            MoveCount = 1;
            Check = false;
            boardState = new ChessPiece[BoardSize][];
            for (var i = 0; i < boardState.GetLength(0); i++)
            {
                boardState[i] = new ChessPiece?[BoardSize];
            }
        }

        /// <summary>
        /// Initialize the board to a position given in FEN format. https://en.wikipedia.org/wiki/Forsyth%E2%80%93Edwards_Notation.
        /// </summary>
        /// <param name="fen">The FEN-formatted game state to load.</param>
        public void LoadFEN(string fen)
        {
            ClearGameState();
            StartingFEN = fen;
            var fenComponents = fen?.Split(new char[] { ' ' }, 6, StringSplitOptions.RemoveEmptyEntries) ?? new string[0];

            // Parse piece positions
            if (fenComponents.Length > 0)
            {
                var rank = 7;
                var file = 0;
                foreach (var piece in fenComponents[0])
                {
                    switch (piece)
                    {
                        case FenRankDelimiter:
                            rank--;
                            file = 0;
                            break;
                        case 'K':
                        case 'k':
                        case 'Q':
                        case 'q':
                        case 'R':
                        case 'r':
                        case 'B':
                        case 'b':
                        case 'N':
                        case 'n':
                        case 'P':
                        case 'p':
                            boardState[file][rank] = new ChessPiece(
                                ChessFormatter.PieceFromString($"{piece}"),
                                new BoardPosition(file, rank));
                            file++;
                            break;
                        default:
                            if (int.TryParse($"{piece}", out var spaceCount))
                            {
                                file += spaceCount;
                            }

                            break;
                    }
                }
            }

            // Parse active color
            if (fenComponents.Length > 1)
            {
                WhiteToMove = fenComponents[1].Equals("w", StringComparison.InvariantCultureIgnoreCase);
            }

            // Parse castling options
            if (fenComponents.Length > 2)
            {
                WhiteCastlingOptions = CastlingOptions.None;
                BlackCastlingOptions = CastlingOptions.None;

                var castlingOptions = fenComponents[2];
                if (castlingOptions.Contains("K"))
                {
                    WhiteCastlingOptions |= CastlingOptions.KingSide;
                }

                if (castlingOptions.Contains("Q"))
                {
                    WhiteCastlingOptions |= CastlingOptions.QueenSide;
                }

                if (castlingOptions.Contains("k"))
                {
                    BlackCastlingOptions |= CastlingOptions.KingSide;
                }

                if (castlingOptions.Contains("q"))
                {
                    BlackCastlingOptions |= CastlingOptions.QueenSide;
                }
            }

            // Parse en passant target
            if (fenComponents.Length > 3 && fenComponents[3].Length == 2)
            {
                EnPassantTarget = new BoardPosition(
                    ChessFormatter.FileFromString($"{fenComponents[3][0]}"),
                    ChessFormatter.RankFromString($"{fenComponents[3][1]}"));
            }

            // Parse half move clock
            if (fenComponents.Length > 4)
            {
                HalfMoveClock = int.Parse(fenComponents[4], CultureInfo.InvariantCulture);
            }

            // Parse move count
            if (fenComponents.Length > 5)
            {
                MoveCount = int.Parse(fenComponents[5], CultureInfo.InvariantCulture);
            }

            // TODO : Look for check
            // TODO : Set Result
        }

        /// <summary>
        /// Gets the chess piece at a particular board position.
        /// </summary>
        /// <param name="position">The position to retrieve a piece from.</param>
        /// <returns>The piece on the indicated square or null if no piece is there.</returns>
        public ChessPiece? GetPiece(BoardPosition position) => GetPiece(position.File, position.Rank);

        /// <summary>
        /// Gets the chess piece at a particular board position.
        /// </summary>
        /// <param name="file">The file the piece is on.</param>
        /// <param name="rank">The rank the piece is on.</param>
        /// <returns>The piece on the indicated square or null if no piece is there.</returns>
        public ChessPiece? GetPiece(int file, int rank)
        {
            if (file >= BoardSize || rank >= BoardSize || file < 0 || rank < 0)
            {
                return null;
            }

            return boardState[file][rank];
        }

        /// <summary>
        /// Make a move.
        /// </summary>
        /// <param name="move">The move to apply to the game state.</param>
        public void Move(Move move)
        {
            // Add to moves list
            Moves.Add(move);

            // Adjust piece positions (and apply promotion, if necessary)
            boardState[move.OriginalPosition.File][move.OriginalPosition.Rank] = null;
            boardState[move.FinalPosition.File][move.FinalPosition.Rank] = new ChessPiece(move.PiecePromotedTo ?? move.PieceMoved, move.FinalPosition);

            // Make additional board adjustments in cases of castling
            if (move.ShortCastle)
            {
                if (WhiteToMove)
                {
                    boardState[7][0] = null;
                    boardState[5][0] = new ChessPiece(ChessPieces.WhiteRook, new BoardPosition(5, 0));
                }
                else
                {
                    boardState[7][7] = null;
                    boardState[5][7] = new ChessPiece(ChessPieces.BlackRook, new BoardPosition(5, 7));
                }
            }
            else if (move.LongCastle)
            {
                if (WhiteToMove)
                {
                    boardState[0][0] = null;
                    boardState[3][0] = new ChessPiece(ChessPieces.WhiteRook, new BoardPosition(3, 0));
                }
                else
                {
                    boardState[0][7] = null;
                    boardState[3][7] = new ChessPiece(ChessPieces.BlackRook, new BoardPosition(3, 7));
                }
            }

            // Remove captured en passant piece
            if ((move.PieceMoved == ChessPieces.WhitePawn || move.PieceMoved == ChessPieces.BlackPawn) &&
                EnPassantTarget.HasValue &&
                move.FinalPosition == EnPassantTarget.Value)
            {
                boardState[EnPassantTarget.Value.File][EnPassantTarget.Value.Rank + (WhiteToMove ? -1 : 1)] = null;
            }

            // Increment or reset half move clock
            if (move.Capture || move.PieceMoved == ChessPieces.WhitePawn || move.PieceMoved == ChessPieces.BlackPawn)
            {
                HalfMoveClock = 0;
            }
            else
            {
                HalfMoveClock++;
            }

            // Update en passant target
            if (move.PieceMoved == ChessPieces.WhitePawn && move.OriginalPosition.Rank == 1 && move.FinalPosition.Rank == 3)
            {
                EnPassantTarget = new BoardPosition(move.OriginalPosition.File, 2);
            }
            else if (move.PieceMoved == ChessPieces.BlackPawn && move.OriginalPosition.Rank == 6 && move.FinalPosition.Rank == 4)
            {
                EnPassantTarget = new BoardPosition(move.OriginalPosition.File, 5);
            }
            else
            {
                EnPassantTarget = null;
            }

            // Update castling options, if necessary
            (WhiteCastlingOptions, BlackCastlingOptions) = move.OriginalPosition switch
            {
                { File: 4, Rank: 0 } => (CastlingOptions.None, BlackCastlingOptions),
                { File: 4, Rank: 7 } => (WhiteCastlingOptions, CastlingOptions.None),
                { File: 0, Rank: 0 } => (WhiteCastlingOptions & ~CastlingOptions.QueenSide, BlackCastlingOptions),
                { File: 0, Rank: 7 } => (WhiteCastlingOptions, BlackCastlingOptions & ~CastlingOptions.QueenSide),
                { File: 7, Rank: 0 } => (WhiteCastlingOptions & ~CastlingOptions.KingSide, BlackCastlingOptions),
                { File: 7, Rank: 7 } => (WhiteCastlingOptions, BlackCastlingOptions & ~CastlingOptions.KingSide),
                _ => (WhiteCastlingOptions, BlackCastlingOptions)
            };

            if (move.Checks || move.Checkmates)
            {
                Check = true;
            }
            else
            {
                Check = false;
            }

            if (move.Checkmates)
            {
                Result = WhiteToMove ? GameResult.WhiteWins : GameResult.BlackWins;
            }

            if (move.Stalemates)
            {
                Result = GameResult.Draw;
            }

            // Update move count
            if (!WhiteToMove)
            {
                MoveCount++;
            }

            // Update active color
            WhiteToMove = !WhiteToMove;

            OnMove?.Invoke(this, move);
        }

        /// <summary>
        /// Get the game state in FEN notation. https://en.wikipedia.org/wiki/Forsyth%E2%80%93Edwards_Notation.
        /// </summary>
        /// <returns>FEN notation description of the game state.</returns>
        public string GetFEN()
        {
            var fen = new StringBuilder();

            // Write moves
            var emptySquares = 0;
            for (var rank = BoardSize - 1; rank >= 0; rank--)
            {
                for (var file = 0; file < BoardSize; file++)
                {
                    var squareState = boardState[file][rank];
                    if (squareState is null)
                    {
                        emptySquares++;
                    }
                    else
                    {
                        if (emptySquares > 0)
                        {
                            fen.Append(emptySquares);
                            emptySquares = 0;
                        }

                        fen.Append(ChessFormatter.PieceToString(squareState.PieceType, true, true));
                    }
                }

                if (emptySquares > 0)
                {
                    fen.Append(emptySquares);
                    emptySquares = 0;
                }

                if (rank > 0)
                {
                    fen.Append(FenRankDelimiter);
                }
            }

            // Write active color
            fen.Append($" {(WhiteToMove ? "w" : "b")} ");

            // Write castling options
            if (BlackCastlingOptions == CastlingOptions.None && WhiteCastlingOptions == CastlingOptions.None)
            {
                fen.Append("- ");
            }
            else
            {
                if ((WhiteCastlingOptions & CastlingOptions.KingSide) == CastlingOptions.KingSide)
                {
                    fen.Append("K");
                }

                if ((WhiteCastlingOptions & CastlingOptions.QueenSide) == CastlingOptions.QueenSide)
                {
                    fen.Append("Q");
                }

                if ((BlackCastlingOptions & CastlingOptions.KingSide) == CastlingOptions.KingSide)
                {
                    fen.Append("k");
                }

                if ((BlackCastlingOptions & CastlingOptions.QueenSide) == CastlingOptions.QueenSide)
                {
                    fen.Append("q");
                }

                fen.Append(' ');
            }

            // Write en passant state, halfmove clock, and move count
            fen.Append($"{EnPassantTarget?.ToString() ?? "-"} {HalfMoveClock} {MoveCount}");

            return fen.ToString();
        }

        /// <summary>
        /// Get the game state in PGN notation. https://en.wikipedia.org/wiki/Portable_Game_Notation, https://www.chessclub.com/help/PGN-spec.
        /// </summary>
        /// <returns>PGN notation description of the game state.</returns>
        public string GetPGN()
        {
            var pgn = new StringBuilder();
            pgn.AppendLine($"[Event \"{Event ?? "-"}\"]");
            pgn.AppendLine($"[Site \"{Site ?? "-"}\"]");
            pgn.AppendLine($"[Round \"{Round ?? "-"}\"]");
            pgn.AppendLine($"[Date \"{StartDate.ToString("yyyy.MM.dd", CultureInfo.InvariantCulture) ?? "??"}\"]");
            pgn.AppendLine($"[White \"{WhitePlayer}\"]");
            pgn.AppendLine($"[Black \"{BlackPlayer}\"]");
            pgn.AppendLine($"[Result \"{ChessFormatter.ResultToString(Result)}\"]");
            if (!string.Equals(StartingFEN, InitialGameFEN, StringComparison.Ordinal))
            {
                pgn.AppendLine($"[FEN \"{StartingFEN}\"]");
            }

            pgn.AppendLine();
            pgn.Append(GetMoveList());
            pgn.AppendLine($" {ChessFormatter.ResultToString(Result)}");
            pgn.AppendLine();

            return pgn.ToString();
        }

        public string GetMoveList()
        {
            var halfMovesSinceStart = Moves.Count;

            // Add one half move if black started
            var firstPieceMoved = Moves.FirstOrDefault();
            if (firstPieceMoved != null && !ChessFormatter.IsPieceWhite(firstPieceMoved.PieceMoved))
            {
                halfMovesSinceStart++;
            }

            var movesSinceStart = halfMovesSinceStart / 2;
            return ChessFormatter.MovesToString(Moves, MoveCount - movesSinceStart);
        }

        /// <summary>
        /// Returns a string representation of the game (in FEN notation).
        /// </summary>
        /// <returns>FEN notation description of the game state.</returns>
        public override string ToString()
        {
            return GetFEN();
        }
    }
}
