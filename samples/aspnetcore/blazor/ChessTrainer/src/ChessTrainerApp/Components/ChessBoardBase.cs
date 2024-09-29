using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.JSInterop;
using MjrChess.Engine;
using MjrChess.Engine.Models;
using MjrChess.Engine.Utilities;

namespace MjrChess.Trainer.Components
{
    /// <summary>
    /// Representation of a chess game.
    /// </summary>
    public class ChessBoardBase : ComponentBase
    {
        [Inject]
        private IJSRuntime JSRuntime { get; set; } = default!; // Injected service, so no initialization needed

        /// <summary>
        /// Gets or sets the chess engine to use to calculate legal moves.
        /// </summary>
        [Parameter]
        public ChessEngine Engine { get; set; } = default!; // This parameter is required. https://github.com/dotnet/aspnetcore/issues/11815

        /// <summary>
        /// Gets or sets a value indicating whether the chess board should allow the user to move the white pieces.
        /// </summary>
        [Parameter]
        public bool UserMovableWhitePieces { get; set; } = true;

        /// <summary>
        /// Gets or sets a value indicating whether the chess board should allow the user to move the black pieces.
        /// </summary>
        [Parameter]
        public bool UserMovableBlackPieces { get; set; } = true;

        /// <summary>
        /// Gets the chess game represented on the board.
        /// </summary>
        protected ChessGame Game => Engine.Game;

        /// <summary>
        /// Gets or sets a list of all legal moves for the selected piece.
        /// </summary>
        /// <remarks>This is stored as an array instead of an enumerable because storing
        /// an enumerable caused the enumerable to be iterated multiple times.</remarks>
        protected Move[] LegalMovesForSelectedPiece { get; set; } = new Move[0];

        /// <summary>
        /// Gets the last move made.
        /// </summary>
        protected Move? LastMove => Game.Moves.LastOrDefault();

        private ChessPiece? selectedPiece;

        /// <summary>
        /// Gets or sets the piece the user currently has selected. Setting the selected piece will also update legal moves.
        /// </summary>
        public ChessPiece? SelectedPiece
        {
            get
            {
                return selectedPiece;
            }

            set
            {
                selectedPiece = value;

                // When the selected piece changes, update legal moves for this board.
                // Storing an enumerable in state used by Blazor was causing the enumerable
                // to be evaluated multiple times. Therefore, store as an array to make sure
                // that the evaluation is only done once.
                LegalMovesForSelectedPiece = selectedPiece == null ? new Move[0] : Engine.GetLegalMoves(selectedPiece.Position).ToArray();
            }
        }

        protected const string ElementName = "ChessBoard";

        /// <summary>
        /// Handle mouse button down events on the chess board.
        /// </summary>
        /// <remarks>
        /// MouseDown and MouseUp events are handled separately to allow
        /// moving pieces either by dragging (which will be a single down/up)
        /// or by clicking on the piece and destination squares (which will be
        /// two separate down/up events).</remarks>
        /// <param name="args">Details of the mouse down event.</param>
        public async void HandleMouseDown(MouseEventArgs args)
        {
            if (args.Button != 0)
            {
                // Only handle left clicks
                return;
            }

            // If no piece is selected, select the piece at the position clicked
            if (SelectedPiece == null)
            {
                (var file, var rank) = await GetMousePositionAsync(args);
                SelectPiece(file, rank);

                // Tell Blazor to re-render the board
                StateHasChanged();
            }
        }

        /// <summary>
        /// Handle mouse button up events on the chess board.
        /// </summary>
        /// <remarks>
        /// MouseDown and MouseUp events are handled separately to allow
        /// moving pieces either by dragging (which will be a single down/up)
        /// or by clicking on the piece and destination squares (which will be
        /// two separate down/up events).</remarks>
        /// <param name="args">Details of the mouse up event.</param>
        public async void HandleMouseUp(MouseEventArgs args)
        {
            if (args.Button != 0)
            {
                // Only handle left clicks
                return;
            }

            // If a piece is selected, place the piece on the square where the mouse
            // button was released if it's different from the piece's inital square
            if (SelectedPiece != null)
            {
                (var file, var rank) = await GetMousePositionAsync(args);
                if (SelectedPiece.Position.File == file && SelectedPiece.Position.Rank == rank)
                {
                    // If the mouse button is released on the same square the
                    // piece was selected from, do nothing. Keep the piece selected since
                    // this could be the initial click to select it.
                    return;
                }
                else
                {
                    PlacePiece(file, rank);

                    // Tell Blazor to re-render the board
                    StateHasChanged();
                }
            }
        }

        /// <summary>
        /// Attempts to select a game piece.
        /// </summary>
        /// <param name="file">The file of the piece to be selected.</param>
        /// <param name="rank">The rank of the piece to be selected.</param>
        /// <returns>True if a piece was successfully selected, false otherwise. Note that this does not guarantee the selected piece has any legal moves.</returns>
        public bool SelectPiece(int file, int rank)
        {
            // Don't select pieces if the game is finished
            if (Game?.Result != GameResult.Ongoing)
            {
                return false;
            }

            // Don't select pieces if the user isn't allowed to move the active color's pieces
            if ((Game.WhiteToMove && !UserMovableWhitePieces) ||
                (!Game.WhiteToMove && !UserMovableBlackPieces))
            {
                return false;
            }

            var piece = Game.GetPiece(file, rank);

            // Don't select pieces if the clicked square doesn't contain a piece or contains a piece for the wrong player
            if (piece == null || ChessFormatter.IsPieceWhite(piece.PieceType) != Game.WhiteToMove)
            {
                return false;
            }

            SelectedPiece = piece;
            return true;
        }

        /// <summary>
        /// Attempts to place a selected piece. This unselects any selected piece.
        /// </summary>
        /// <param name="file">The file to place the selected piece on.</param>
        /// <param name="rank">The rank to place the selected piece on.</param>
        /// <returns>True if the selected piece was successully and legally placed on the indicated rank and file. False if the move is illegal or if no piece is selected.</returns>
        private bool PlacePiece(int file, int rank)
        {
            var move = LegalMovesForSelectedPiece.SingleOrDefault(m => m.FinalPosition.File == file && m.FinalPosition.Rank == rank);
            SelectedPiece = null;
            if (move != null)
            {
                // If the piece is placed in a legal move location,
                // move the piece.
                Game.Move(move);

                return true;
            }
            else
            {
                return false;
            }
        }

        private async Task<(int file, int rank)> GetMousePositionAsync(MouseEventArgs args)
        {
            // Use JSInterop to get the bounding rectangle of the chess board component
            var boardDimensions = await JSRuntime.InvokeAsync<Rectangle>("getBoundingRectangle", new object[] { ElementName });

            // Account for the rare case where the user clicks on the final pixel of the board
            if (args.ClientX >= boardDimensions.Right)
            {
                args.ClientX--;
            }

            if (args.ClientY >= boardDimensions.Bottom)
            {
                args.ClientY--;
            }

            // Divide by board size (8 squares) to determine which rank and file corresponds to the input position.
            var file = ((int)args.ClientX - boardDimensions.X) * Game.BoardSize / boardDimensions.Width;
            var rank = (Game.BoardSize - 1) - (((int)args.ClientY - boardDimensions.Y) * Game.BoardSize / boardDimensions.Height);

            return (file, rank);
        }
    }
}
