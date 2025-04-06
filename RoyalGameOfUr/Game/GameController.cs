using RoyalGameOfUr.Models;
using RoyalGameOfUr.Rendering.Interfaces;
using RoyalGameOfUr.UI.Interfaces;
using System.Diagnostics.CodeAnalysis;

namespace RoyalGameOfUr.Game;

/// <summary>
///     Controls the game flow and logic
/// </summary>
[SuppressMessage("ReSharper", "SwitchStatementMissingSomeEnumCasesNoDefault")]
[SuppressMessage("ReSharper", "SwitchStatementHandlesSomeKnownEnumValuesWithDefault")]
public class GameController
{
    private readonly IConsoleWrapper _console;
    private readonly GameState _gameState;
    private readonly Random _random = new();
    private readonly IBoardRendererFactory _rendererFactory;
    private readonly BoardOrientation[] _rotationOrder = GameConfig.RotationOrder;
    private BoardOrientation _currentOrientation = BoardOrientation.HorizontalLtr;

    /// <summary>
    ///     Creates a new game controller
    /// </summary>
    public GameController(IConsoleWrapper console, IBoardRendererFactory rendererFactory)
    {
        _console = console ?? throw new ArgumentNullException(nameof(console));
        _rendererFactory = rendererFactory ?? throw new ArgumentNullException(nameof(rendererFactory));
        _gameState = new GameState(GameBoard.CreateDefault());
    }

    public void RunGame()
    {
        var gameRunning = true;

        while (gameRunning)
        {
            DisplayGameState();
            ShowCurrentPlayer();

            _console.WriteLine(GameConfig.Prompts.RollPrompt);
            var keyInfo = _console.ReadKey(true);

            if (HandleInput(keyInfo)) continue;

            var roll = RollDice();
            _console.WriteLine(string.Format(GameConfig.Prompts.RollResult, roll));

            if (roll == 0)
            {
                _console.WriteLine(GameConfig.Prompts.NoMovesZeroRoll);
                _console.ReadKey(true);
                _gameState.SwitchPlayer();
                continue;
            }

            var possibleMoves = GetPossibleMoves(roll);
            if (possibleMoves.Count == 0)
            {
                _console.WriteLine(GameConfig.Prompts.NoValidMoves);
                _console.ReadKey(true);
                _gameState.SwitchPlayer();
                continue;
            }

            var selectedMove = ShowMoveSelectionMenu(possibleMoves);
            if (selectedMove == null) return;

            var anotherTurn = ExecuteMove(selectedMove, roll);
            var winner = _gameState.GetWinner();

            if (winner != null)
            {
                DisplayGameState();
                _console.WriteLine(string.Format(
                    GameConfig.Prompts.WinnerMessage,
                    winner.Number,
                    winner.Color.ToString()));
                _console.WriteLine(GameConfig.Prompts.WinnerPrompt);
                _console.ReadKey(true);
                gameRunning = false;
            }

            if (!anotherTurn) _gameState.SwitchPlayer();
        }
    }

    private void ShowCurrentPlayer()
    {
        var player = _gameState.CurrentPlayer;
        _console.WriteLine(string.Format(GameConfig.Prompts.CurrentPlayer, player.Number, player.Color.ToString()));
    }


    private void DisplayGameState()
    {
        _console.Clear();
        _rendererFactory.CreateRenderer(_currentOrientation).RenderGameState(_gameState);
    }

    private bool HandleInput(ConsoleKeyInfo keyInfo)
    {
        switch (keyInfo.Key)
        {
            case ConsoleKey.Escape:
                Environment.Exit(0);
                return false;
            case ConsoleKey.R:
                HandleBoardRotation();
                return true;
            case ConsoleKey.B:
                ToggleHorizontalOrientation();
                return true;
            case ConsoleKey.V:
                ToggleVerticalOrientation();
                return true;
            default:
                return false;
        }
    }


    /// <summary>
    ///     Rolls 4 dice and returns the result
    /// </summary>
    private int RollDice()
    {
        var total = 0;
        for (var i = 0; i < GameConfig.DiceCount; i++)
            total += _random.Next(GameConfig.DiceSides);
        return total;
    }


    /// <summary>
    ///     Handles the board rotation interactive mode
    /// </summary>
    private void HandleBoardRotation()
    {
        var exitRotationMode = false;

        while (!exitRotationMode)
        {
            _console.Clear();

            var renderer = _rendererFactory.CreateRenderer(_currentOrientation);
            renderer.RenderGameState(_gameState);

            var orientationName = GetOrientationName(_currentOrientation);
            _console.WriteLine($"\nCurrent view: {orientationName}");
            _console.WriteLine("\nControls:");
            _console.WriteLine("  Space: Rotate view 90° clockwise");
            _console.WriteLine("  Enter: Return to game");
            _console.WriteLine("  Escape: Exit to menu");

            var keyInfo = _console.ReadKey(true);

            switch (keyInfo.Key)
            {
                case ConsoleKey.Escape:
                    return;
                case ConsoleKey.Enter:
                    exitRotationMode = true;
                    break;
                case ConsoleKey.Spacebar:
                    _currentOrientation = GetNextOrientation(_currentOrientation);
                    break;
            }
        }
    }

    /// <summary>
    ///     Gets the next orientation in rotation
    /// </summary>
    private BoardOrientation GetNextOrientation(BoardOrientation current)
    {
        var index = Array.IndexOf(_rotationOrder, current);
        if (index == -1) return BoardOrientation.HorizontalLtr;

        var nextIndex = (index + 1) % _rotationOrder.Length;
        return _rotationOrder[nextIndex];
    }

    /// <summary>
    ///     Toggles the board orientation between Left-to-Right and Right-to-Left
    /// </summary>
    private void ToggleHorizontalOrientation()
    {
        if (_currentOrientation is BoardOrientation.HorizontalLtr or BoardOrientation.HorizontalRtl)
            _currentOrientation = _currentOrientation == BoardOrientation.HorizontalLtr
                ? BoardOrientation.HorizontalRtl
                : BoardOrientation.HorizontalLtr;
        else
            _currentOrientation = BoardOrientation.HorizontalLtr;
    }

    /// <summary>
    ///     Toggles the board orientation between Top-to-Bottom and Bottom-to-Top
    /// </summary>
    private void ToggleVerticalOrientation()
    {
        if (_currentOrientation is BoardOrientation.VerticalTtb or BoardOrientation.VerticalBtt)
            _currentOrientation = _currentOrientation == BoardOrientation.VerticalTtb
                ? BoardOrientation.VerticalBtt
                : BoardOrientation.VerticalTtb;
        else
            _currentOrientation = BoardOrientation.VerticalTtb;
    }

    /// <summary>
    ///     Gets the orientation name to display
    /// </summary>
    private static string GetOrientationName(BoardOrientation orientation)
    {
        return orientation switch
        {
            BoardOrientation.HorizontalLtr => "Wide (Left to Right)",
            BoardOrientation.HorizontalRtl => "Wide (Right to Left)",
            BoardOrientation.VerticalTtb => "Tall (Top to Bottom)",
            BoardOrientation.VerticalBtt => "Tall (Bottom to Top)",
            _ => "Unknown"
        };
    }


    /// <summary>
    ///     Gets the list of possible moves for the current roll
    /// </summary>
    private List<PossibleMove> GetPossibleMoves(int roll)
    {
        var moves = new List<PossibleMove>();
        var currentPlayer = _gameState.CurrentPlayer;

        var canAddNew = currentPlayer.GetOffBoardPiecesCount() > 0;
        if (canAddNew && roll is > 0 and <= 16)
        {
            var (row, col) = currentPlayer.GetBoardCoordinates(roll);
            var target = _gameState.GetPieceAt(row, col);

            if (IsEmptyOrOpponent(currentPlayer, target))
                moves.Add(new PossibleMove(
                    PossibleMove.MoveType.AddNew,
                    null,
                    roll,
                    $"Add a new piece to position {roll}"));
        }

        // Check which pieces can move
        foreach (var piece in currentPlayer.Pieces.Where(p => p.IsOnBoard))
        {
            var newPosition = piece.Position + roll;

            switch (newPosition)
            {
                // Check if position is valid and not occupied by own piece
                case <= 16:
                    {
                        var (row, col) = currentPlayer.GetBoardCoordinates(newPosition);
                        var target = _gameState.GetPieceAt(row, col);

                        if (IsEmptyOrOpponent(currentPlayer, target))
                            moves.Add(new PossibleMove(
                                PossibleMove.MoveType.MovePiece,
                                piece,
                                newPosition,
                                $"Move piece at position {piece.Position} to position {newPosition}"));

                        break;
                    }
                // Check if exact exit
                case 17:
                    moves.Add(new PossibleMove(
                        PossibleMove.MoveType.ExitPiece,
                        piece,
                        17,
                        $"Move piece at position {piece.Position} off the board (completes path)"));
                    break;
            }
        }

        return moves;

    }

    private bool IsEmptyOrOpponent(Player currentPlayer, GamePiece? piece)
    {
        return piece == null ||
               (piece.Owner != currentPlayer &&
                !GameState.IsInSafeZone(piece.Position) &&
                !_gameState.IsRosette(piece.Position));
    }

    /// <summary>
    ///     Shows a menu for move selection and returns the selected move
    /// </summary>
    private PossibleMove? ShowMoveSelectionMenu(List<PossibleMove> possibleMoves)
    {
        var selectedIndex = 0;
        var selectionMade = false;

        while (!selectionMade)
        {
            _console.Clear();
            var renderer = _rendererFactory.CreateRenderer(_currentOrientation);
            renderer.RenderGameState(_gameState);

            var currentPlayer = _gameState.CurrentPlayer;
            _console.WriteLine(
                $"\nCurrent Player: Player {currentPlayer.Number} ({(currentPlayer.Number == 1 ? "Red" : "Blue")})");
            _console.WriteLine("\nSelect your move:");

            for (var i = 0; i < possibleMoves.Count; i++)
                if (i == selectedIndex)
                {
                    _console.Write("> ");
                    var originalFg = _console.ForegroundColor;
                    var originalBg = _console.BackgroundColor;
                    _console.SetForegroundColor(ConsoleColor.Black);
                    _console.SetBackgroundColor(ConsoleColor.White);
                    _console.WriteLine(possibleMoves[i].Description);
                    _console.SetForegroundColor(originalFg);
                    _console.SetBackgroundColor(originalBg);
                }
                else
                {
                    _console.WriteLine($"  {possibleMoves[i].Description}");
                }

            _console.WriteLine("\nUse Up/Down arrows to select, Enter to confirm, or Esc to exit");

            var key = _console.ReadKey(true).Key;

            switch (key)
            {
                case ConsoleKey.UpArrow:
                    selectedIndex = (selectedIndex - 1 + possibleMoves.Count) % possibleMoves.Count;
                    break;
                case ConsoleKey.DownArrow:
                    selectedIndex = (selectedIndex + 1) % possibleMoves.Count;
                    break;
                case ConsoleKey.Enter:
                    selectionMade = true;
                    break;
                case ConsoleKey.Escape:
                    return null;
            }
        }

        return possibleMoves[selectedIndex];
    }

    /// <summary>
    ///     Executes the selected move
    /// </summary>
    /// <returns>True if landed on a rosette (gets another turn)</returns>
    private bool ExecuteMove(PossibleMove move, int roll)
    {
        var landedOnRosette = false;

        switch (move.Type)
        {
            case PossibleMove.MoveType.AddNew:
                _gameState.AddPiece(move.NewPosition);

                var newPiece = _gameState.CurrentPlayer.Pieces.First(p => p.Position == move.NewPosition);
                if (_gameState.CheckCapture(newPiece))
                {
                    _console.WriteLine(GameConfig.Prompts.CaptureMessage);
                    _console.ReadKey(true);
                }

                if (_gameState.IsRosette(move.NewPosition))
                {
                    _console.WriteLine(GameConfig.Prompts.RosetteMessage);
                    _console.ReadKey(true);
                    landedOnRosette = true;
                }

                break;

            case PossibleMove.MoveType.MovePiece:
                if (move.Piece != null)
                {
                    GameState.MovePiece(move.Piece, roll);

                    if (_gameState.CheckCapture(move.Piece))
                    {
                        _console.WriteLine(GameConfig.Prompts.CaptureMessage);
                        _console.ReadKey(true);
                    }

                    if (_gameState.IsRosette(move.NewPosition))
                    {
                        _console.WriteLine(GameConfig.Prompts.RosetteMessage);
                        _console.ReadKey(true);
                        landedOnRosette = true;
                    }
                }

                break;

            case PossibleMove.MoveType.ExitPiece:
                if (move.Piece != null)
                {
                    GameState.MovePiece(move.Piece, roll);
                    _console.WriteLine(GameConfig.Prompts.ExitMessage);
                    _console.ReadKey(true);
                }

                break;
            default:
                throw new NotImplementedException();
        }

        return landedOnRosette;
    }

    /// <summary>
    ///     Represents a possible move in the game
    /// </summary>
    private class PossibleMove(PossibleMove.MoveType type, GamePiece? piece, int newPosition, string description)
    {
        public enum MoveType
        {
            AddNew,
            MovePiece,
            ExitPiece
        }

        public MoveType Type { get; } = type;
        public GamePiece? Piece { get; } = piece;
        public int NewPosition { get; } = newPosition;
        public string Description { get; } = description;
    }
}