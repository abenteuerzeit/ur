using RoyalGameOfUr.Models;
using RoyalGameOfUr.UI.Interfaces;

namespace RoyalGameOfUr.Rendering.Renderers;

/// <summary>
///     Renders the game board in a vertical orientation
/// </summary>
public class VerticalBoardRenderer(
    IConsoleWrapper console,
    ICellStyleProvider cellStyleProvider,
    bool isBottomToTop)
    : BoardRenderer(console, cellStyleProvider)
{
    private const int LinesPerCell = 3;

    /// <summary>
    ///     Renders the board in vertical orientation
    /// </summary>
    public override void RenderBoard(CellType[,] board)
    {
        var rows = board.GetLength(0);
        var cols = board.GetLength(1);

        var activeColumns = GetActiveColumns(board, rows, cols);
        foreach (var column in activeColumns)
            RenderColumn(board, rows, column);
    }

    public override void RenderGameState(GameState gameState)
    {
        // TODO: Render board pieces
        //var board = gameState.Board.GetBoardData();
        //var rows = board.GetLength(0);
        //var cols = board.GetLength(1);
        // var activeColumns = GetActiveColumns(board, rows, cols);

        throw new NotImplementedException(nameof(GameState));
    }

    /// <summary>
    ///     Renders a single column of the board
    /// </summary>
    private void RenderColumn(CellType[,] board, int rows, int column)
    {
        DrawColumnBorders(board, rows, column);
        Console.WriteLine();

        DrawColumnContent(board, rows, column);

        DrawColumnBorders(board, rows, column);
        Console.WriteLine();
    }

    /// <summary>
    ///     Draws the top/bottom borders for all cells in a column
    /// </summary>
    private void DrawColumnBorders(CellType[,] board, int rows, int column)
    {
        for (var i = 0; i < rows; i++)
        {
            var rowIndex = isBottomToTop ? i : rows - 1 - i;
            DrawCellBorder(board[rowIndex, column]);
        }
    }

    /// <summary>
    ///     Draws the content of all cells in a column
    /// </summary>
    private void DrawColumnContent(CellType[,] board, int rows, int column)
    {
        for (var line = 0; line < LinesPerCell; line++)
        {
            for (var i = 0; i < rows; i++)
            {
                var rowIndex = isBottomToTop ? i : rows - 1 - i;
                WriteCellLine(board[rowIndex, column], line);
            }

            Console.WriteLine();
        }
    }

    /// <summary>
    ///     Gets all active columns that should be rendered, in the correct order
    /// </summary>
    private List<int> GetActiveColumns(CellType[,] board, int rows, int cols)
    {
        return GetColumnsToRender(cols)
            .Where(column => HasActiveCells(board, rows, column))
            .ToList();
    }

    /// <summary>
    ///     Gets the list of column indices to render in the appropriate order
    /// </summary>
    private List<int> GetColumnsToRender(int cols)
    {
        var result = new List<int>();

        if (isBottomToTop)
            for (var j = cols - 1; j >= 0; j--)
                result.Add(j);
        else
            for (var j = 0; j < cols; j++)
                result.Add(j);

        return result;
    }

    /// <summary>
    ///     Checks if a column has any active cells
    /// </summary>
    private static bool HasActiveCells(CellType[,] board, int rows, int col)
    {
        for (var i = 0; i < rows; i++)
            if (board[i, col] != CellType.Disabled)
                return true;

        return false;
    }
}