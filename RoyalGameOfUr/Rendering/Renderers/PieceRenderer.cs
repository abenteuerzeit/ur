using RoyalGameOfUr.Models;
using RoyalGameOfUr.UI.Interfaces;

namespace RoyalGameOfUr.Rendering.Renderers;

/// <summary>
///     Renders game pieces on the board
/// </summary>
public class PieceRenderer
{
    private readonly IConsoleWrapper _console;

    /// <summary>
    ///     Creates a new piece renderer
    /// </summary>
    public PieceRenderer(IConsoleWrapper console)
    {
        _console = console ?? throw new ArgumentNullException(nameof(console));
    }

    /// <summary>
    ///     Renders a piece at the specified console position
    /// </summary>
    public void RenderPiece(int cellStartX, int cellStartY, GamePiece piece)
    {
        var originalFg = _console.ForegroundColor;
        var originalBg = _console.BackgroundColor;

        try
        {
            // Cell is 7 chars wide (+-----+) and 3 lines tall
            // Save current cursor position
            var originalLeft = _console.CursorLeft;
            var originalTop = _console.CursorTop;

            // Set color for the piece
            _console.SetForegroundColor(piece.Owner.Color);
            _console.SetBackgroundColor(ConsoleColor.Black);

            // Draw diamond pattern - shifted down by 1 line
            // Top of diamond (moved from line 0 to line 1)
            _console.SetCursorPosition(cellStartX + 3, cellStartY + 1);
            _console.Write("0");

            // Middle of diamond (moved from line 1 to line 2)
            _console.SetCursorPosition(cellStartX + 2, cellStartY + 2);
            _console.Write("000");

            // Bottom of diamond (moved from line 2 to line 3)
            _console.SetCursorPosition(cellStartX + 3, cellStartY + 3);
            _console.Write("0");

            // Restore cursor position
            _console.SetCursorPosition(originalLeft, originalTop);
        }
        finally
        {
            // Restore original colors
            _console.SetForegroundColor(originalFg);
            _console.SetBackgroundColor(originalBg);
        }
    }

    /// <summary>
    ///     Renders off-board pieces for a player
    /// </summary>
    public void RenderOffBoardPieces(int startLeft, int startTop, Player player)
    {
        var offBoardCount = player.GetOffBoardPiecesCount();

        var originalFg = _console.ForegroundColor;
        try
        {
            _console.SetForegroundColor(player.Color);

            for (var i = 0; i < offBoardCount; i++)
            {
                var row = i / 3; // 3 pieces per row
                var col = i % 3;

                _console.SetCursorPosition(startLeft + col * 2, startTop + row);
                _console.Write("0");
            }
        }
        finally
        {
            _console.SetForegroundColor(originalFg);
        }
    }

    /// <summary>
    ///     Renders pieces that have completed the board
    /// </summary>
    public void RenderCompletedPieces(int startLeft, int startTop, Player player)
    {
        var completedCount = player.GetCompletedPiecesCount();

        var originalFg = _console.ForegroundColor;
        try
        {
            _console.SetForegroundColor(player.Color);

            for (var i = 0; i < completedCount; i++)
            {
                var row = i / 3; // 3 pieces per row
                var col = i % 3;

                _console.SetCursorPosition(startLeft + col * 2, startTop + row);
                _console.Write("0");
            }
        }
        finally
        {
            _console.SetForegroundColor(originalFg);
        }
    }
}