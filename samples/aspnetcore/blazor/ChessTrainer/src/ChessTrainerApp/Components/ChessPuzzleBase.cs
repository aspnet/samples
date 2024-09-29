using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using MjrChess.Engine;
using MjrChess.Engine.Models;
using MjrChess.Trainer.Models;
using MjrChess.Trainer.Services;

namespace MjrChess.Trainer.Components
{
    public class ChessPuzzleBase : OwningComponentBase
    {
        private const int MaxHistoryShown = 10;

        [Inject]
        private ILogger<ChessPuzzleBase> Logger { get; set; } = default!; // Injected service, so no initialization needed

        private IPuzzleService PuzzleService { get; set; } = default!; // Retrieved from DI in OnInitialized https://github.com/dotnet/csharplang/issues/2830

        private IHistoryService UserService { get; set; } = default!; // Retrieved from DI in OnInitialized https://github.com/dotnet/csharplang/issues/2830

        [CascadingParameter]
#pragma warning disable SA1300 // Element should begin with upper-case letter
#pragma warning disable IDE1006 // Apply .editorconfig rules
        private Task<AuthenticationState>? authenticationStateTask { get; set; }
#pragma warning restore SA1300 // Element should begin with upper-case letter
#pragma warning restore IDE1006 // Apply .editorconfig rules

        /// <summary>
        /// Helper method to get the current authenticated user's ID based on current authentication state.
        /// </summary>
        /// <returns>The current user's ID or null if no user is authenticated.</returns>
        private async Task<string?> GetUserIdAsync() => authenticationStateTask != null
            ? (await authenticationStateTask)?.User?.GetUserId()
            : null;

        private ChessEngine puzzleEngine = default!; // Injected service, so no initialization needed

        /// <summary>
        /// Gets or sets the puzzle engine (including the game state) for the current puzzle.
        /// </summary>
        [Inject]
        protected ChessEngine PuzzleEngine
        {
            get => puzzleEngine;
            set
            {
                // This setter makes sure the chess game's move handlers are
                // wired correctly when switching which game is in use for the
                // current puzzle.
                if (puzzleEngine != null)
                {
                    puzzleEngine.Game.OnMove -= HandleMove;
                }

                puzzleEngine = value;
                puzzleEngine.Game.OnMove += HandleMove;
            }
        }

        private TacticsPuzzle? currentPuzzle;

        /// <summary>
        /// Gets or sets the current tactics puzzle being shown to the user.
        /// </summary>
        protected TacticsPuzzle? CurrentPuzzle
        {
            get => currentPuzzle;
            set
            {
                // When the current puzzle changes, update the engine to display the new puzzle
                currentPuzzle = value;
                if (value != null)
                {
                    // Load the new puzzle and make the inital move to prepare it
                    PuzzleEngine.LoadFEN(value.Position);
                    MakeMove(value.SetupMove);
                    PuzzleEngine.Game.WhitePlayer = value.WhitePlayerName ?? "White Player";
                    PuzzleEngine.Game.BlackPlayer = value.BlackPlayerName ?? "Black Player";
                }
            }
        }

        /// <summary>
        /// Gets or sets the state of the current puzzle (solved, missed, ongoing, or revealed).
        /// </summary>
        protected PuzzleState CurrentPuzzleState { get; set; }

        /// <summary>
        /// Gets or sets the user's puzzle history.
        /// </summary>
        protected IList<PuzzleHistory>? PuzzleHistory { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the user has already attempted this puzzle or not.
        /// </summary>
        protected bool FirstAttempt { get; set; }

        protected override void OnInitialized()
        {
            // PuzzleService and UserService are retrieved from OwningComponentBase's ScopedServices property
            // so that services scoped specifically to this instance (rather than services shared with other components
            // are used. For more information about OwningComponentBase and component-scoped DI services,
            // see documentation at https://docs.microsoft.com/aspnet/core/blazor/fundamentals/dependency-injection#utility-base-component-classes-to-manage-a-di-scope
            PuzzleService = ScopedServices.GetRequiredService<IPuzzleService>();
            UserService = ScopedServices.GetRequiredService<IHistoryService>();

            base.OnInitialized();
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            // Load initial puzzle in OnAfterRenderAsync instead of in OnInitializedAsync
            // because the latter is invoked twice (once during pre-rendering and once
            // after the client has connected to the server). Therefore, only stable,
            // auth-agnostic initialization should happen there. This method may return different
            // puzzles when called twice resulting in the puzzle the user sees changing
            // after the client-server connection is established. Therefore, load the initial
            // puzzle here instead.
            //
            // Pre-rendering docs and OnInitialized interactions:
            // https://docs.microsoft.com/aspnet/core/blazor/hosting-models#stateful-reconnection-after-prerendering
            // Recommendation to use OnAfterRender for this scenario:
            // https://github.com/dotnet/aspnetcore/issues/13711
            if (firstRender)
            {
                await LoadNextPuzzleAsync();
                var userId = await GetUserIdAsync();

                // Don't await loading puzzle history since it can take a second or two and awaiting it
                // in OnAfterRenderAsync can block initialization of this component. Instead, let the
                // history be populated asynchronously and use InvokeAsync to notify the UI that state
                // has changed once it is finished.
                _ = Task.Run(async () =>
                {
                    PuzzleHistory = userId is null
                        ? new List<PuzzleHistory>()
                        : (await UserService.GetPuzzleHistoryAsync(userId))?.OrderByDescending(h => h.LastModifiedDate).Take(MaxHistoryShown).ToList() ?? new List<PuzzleHistory>();

                    // StateHasChanged cannot be called directly since this thread will not be on the renderer's synchronization context.
                    // Use InvokeAsync to switch execution to the renderer's sync context.
                    await InvokeAsync(StateHasChanged);
                });
            }

            await base.OnAfterRenderAsync(firstRender);
        }

        /// <summary>
        /// Loads a random puzzle from the puzzle service.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        protected async Task LoadNextPuzzleAsync() => LoadPuzzle(await PuzzleService.GetRandomPuzzleAsync());

        /// <summary>
        /// Loads a puzzle by ID.
        /// </summary>
        /// <param name="id">The ID of the puzzle to load.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        protected async Task LoadPuzzleByIdAsync(int id) => LoadPuzzle(await PuzzleService.GetPuzzleAsync(id));

        private void LoadPuzzle(TacticsPuzzle? puzzle)
        {
            if (puzzle != null)
            {
                CurrentPuzzle = puzzle;
                CurrentPuzzleState = PuzzleState.Ongoing;
                FirstAttempt = true;
                Logger.LogInformation("Loaded puzzle ID {PuzzleId}", CurrentPuzzle?.Id);
                StateHasChanged();
            }
        }

        /// <summary>
        /// Resets the current puzzle to an ongoing/unsolved state
        /// (but does not reset FirstAttempt).
        /// </summary>
        protected void ResetPuzzle()
        {
            if (CurrentPuzzle != null)
            {
                // Currently it's a no-op to reset an ongoing puzzle (since an ongoing
                // puzzle is already in the starting state), but I'm allowing it since,
                // in the future, multi-step puzzles might make sense to reset while still
                // ongoing.
                CurrentPuzzleState = PuzzleState.Ongoing;
                CurrentPuzzle = CurrentPuzzle;
                Logger.LogInformation("Reset puzzle ID {PuzzleId}", CurrentPuzzle.Id);
                StateHasChanged();
            }
        }

        /// <summary>
        /// Reveals the solution to the current puzzle without recording puzzle history.
        /// </summary>
        protected void RevealPuzzle()
        {
            // Reveal is a no-op if there's no puzzle or the puzzle is already revealed or solved
            if (CurrentPuzzle != null && CurrentPuzzleState != PuzzleState.Revealed && CurrentPuzzleState != PuzzleState.Solved)
            {
                if (CurrentPuzzleState == PuzzleState.Missed)
                {
                    ResetPuzzle();
                }

                // Users don't get credit for solving (or missing) a puzzle once they know the solution
                FirstAttempt = false;

                MakeMove(CurrentPuzzle.Solution);

                Logger.LogInformation("Revealed puzzle ID {PuzzleId} solution", CurrentPuzzle.Id);
                CurrentPuzzleState = PuzzleState.Revealed;
                StateHasChanged();
            }
        }

        private async void HandleMove(ChessGame game, Move move)
        {
            if (CurrentPuzzle != null)
            {
                var userId = await GetUserIdAsync();

                if (move == CurrentPuzzle.Solution)
                {
                    Logger.LogInformation("Puzzle {PuzzleId} solved by {UserId}", CurrentPuzzle.Id, userId ?? "Anonymous");
                    CurrentPuzzleState = PuzzleState.Solved;
                }
                else
                {
                    Logger.LogInformation("Puzzle {PuzzleId} missed by {UserId}", CurrentPuzzle.Id, userId ?? "Anonymous");
                    CurrentPuzzleState = PuzzleState.Missed;
                }

                // Record the user's first attempt in puzzle history
                if (FirstAttempt)
                {
                    var puzzleHistory = new PuzzleHistory
                    {
                        UserId = userId,
                        Puzzle = CurrentPuzzle,
                        Solved = CurrentPuzzleState == PuzzleState.Solved
                    };

                    // Show the result in the puzzle history component
                    PuzzleHistory?.Insert(0, puzzleHistory);
                    while (PuzzleHistory?.Count > MaxHistoryShown)
                    {
                        // If too many history items are shown, remove the oldest one.
                        PuzzleHistory.RemoveAt(MaxHistoryShown);
                    }

                    if (!(userId is null))
                    {
                        // If the user is authenticated, store the new puzzle history
                        // with the user service.
                        await UserService.RecordPuzzleHistoryAsync(puzzleHistory);
                    }

                    FirstAttempt = false;
                }

                StateHasChanged();
            }
        }

        private void MakeMove(Move move)
        {
            if (CurrentPuzzle is null)
            {
                return;
            }

            // Moves in puzzles only include where the piece should move.
            // They don't include information about check, checkmate, etc.
            // By finding the move with the current engine, that information
            // is added (so that it will display correctly).
            var resolvedMove = PuzzleEngine.GetLegalMoves(move.OriginalPosition).SingleOrDefault(m => m.FinalPosition == move.FinalPosition);
            if (resolvedMove == null)
            {
                Logger.LogError("Invalid puzzle move ({Move}) for {PuzzleId}", move, CurrentPuzzle.Id);
                throw new InvalidOperationException($"Invalid move ({move}) for puzzle {CurrentPuzzle.Id}");
            }

            // Temporarily detach the move handler so that setup moves aren't
            // handled as if they were moves made by the user.
            PuzzleEngine.Game.OnMove -= HandleMove;

            // Move
            PuzzleEngine.Game.Move(resolvedMove);

            // Re-attach the move handler
            PuzzleEngine.Game.OnMove += HandleMove;
        }
    }
}
