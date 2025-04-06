using RoyalGameOfUr.Models;
using RoyalGameOfUr.UI.Interfaces;

namespace RoyalGameOfUr.Rendering.Renderers;

/// <summary>
///     Renders the game board in a horizontal orientation
/// </summary>
public class HorizontalBoardRenderer(
    IConsoleWrapper console,
    ICellStyleProvider cellStyleProvider,
    bool isRightToLeft)
    : BoardRenderer(console, cellStyleProvider)
{
    private const int CellWidth = 7;
    private const int OffsetX = 2; // Left margin
    private const int OffsetY = 1; // Top margin
    private const int LinesPerCell = 3;

    /// <summary>
    ///     Renders the board in horizontal orientation
    /// </summary>
    public override void RenderBoard(CellType[,] board)
    {
        var rows = board.GetLength(0);
        var cols = board.GetLength(1);

        for (var rowIndex = 0; rowIndex < rows; rowIndex++)
            RenderRow(board, rowIndex, cols);
    }

    public override void RenderGameState(GameState gameState)
    {
        var board = gameState.Board.GetBoardData();
        var rows = board.GetLength(0);
        var cols = board.GetLength(1);

        var piecesToRender = new List<(int X, int Y, GamePiece Piece)>();
        var currentY = RenderBoardWithPiecePositions(gameState, board, rows, cols, piecesToRender);

        RenderPieces(piecesToRender);
        RenderPlayerInformation(gameState, currentY);
    }

    /// <summary>
    ///     Renders the board and collects piece positions for later rendering
    /// </summary>
    private int RenderBoardWithPiecePositions(
        GameState gameState,
        CellType[,] board,
        int rows,
        int cols,
        List<(int X, int Y, GamePiece Piece)> piecesToRender)
    {
        var currentY = OffsetY;

        for (var rowIndex = 0; rowIndex < rows; rowIndex++)
        {
            var rowStartY = currentY;

            DrawBorders(rowIndex);

            currentY = RenderRowContentWithPieces(
                gameState,
                board,
                rowIndex,
                cols,
                currentY,
                rowStartY,
                piecesToRender);

            DrawBorders(rowIndex);
        }

        return currentY;

        void DrawBorders(int rowIndex)
        {
            Console.SetCursorPosition(OffsetX, currentY);
            DrawRowBorders(board, rowIndex, cols);
            Console.WriteLine();
            currentY++;
        }
    }

    /// <summary>
    ///     Renders a single row's content and identifies piece positions
    /// </summary>
    private int RenderRowContentWithPieces(
        GameState gameState,
        CellType[,] board,
        int rowIndex,
        int cols,
        int currentY,
        int rowStartY,
        List<(int X, int Y, GamePiece Piece)> piecesToRender)
    {
        for (var lineIndex = 0; lineIndex < LinesPerCell; lineIndex++)
        {
            Console.SetCursorPosition(OffsetX, currentY);

            CollectPiecesAndRenderLine(
                gameState,
                board,
                rowIndex,
                cols,
                lineIndex,
                rowStartY,
                piecesToRender);

            Console.WriteLine();
            currentY++;
        }

        return currentY;
    }

    /// <summary>
    ///     Collects pieces positions while rendering a single line
    /// </summary>
    private void CollectPiecesAndRenderLine(
        GameState gameState,
        CellType[,] board,
        int rowIndex,
        int cols,
        int lineIndex,
        int rowStartY,
        List<(int X, int Y, GamePiece Piece)> piecesToRender)
    {
        IterateRow(board, rowIndex, cols, (cellType, colIndex) =>
        {
            var cellStartX = OffsetX + colIndex * CellWidth;
            if (isRightToLeft) cellStartX = OffsetX + (cols - 1 - colIndex) * CellWidth;

            WriteCellLine(cellType, lineIndex);

            if (cellType == CellType.Disabled) return;

            var piece = gameState.GetPieceAt(rowIndex, colIndex);
            if (piece != null)
                piecesToRender.Add((cellStartX, rowStartY, piece));
        });
    }

    /// <summary>
    ///     Renders all pieces that were collected during board rendering
    /// </summary>
    private void RenderPieces(List<(int X, int Y, GamePiece Piece)> piecesToRender)
    {
        foreach (var (x, y, piece) in piecesToRender) PieceRenderer.RenderPiece(x, y, piece);
    }

    /// <summary>
    ///     Renders player information (pieces off board and completed)
    /// </summary>
    private void RenderPlayerInformation(GameState gameState, int currentY)
    {
        Console.WriteLine();
        const int padding = 2;
        RenderInGamePiecesInfo();
        RenderExitPiecesInfo();
        return;

        void AddSectionPadding(int value)
        {
            currentY += value;
        }

        void RenderInGamePiecesInfo()
        {
            AddSectionPadding(padding);

            currentY = RenderPlayerSection(
                currentY,
                "Player 1 (Red) - Pieces off board:",
                gameState.Player1,
                PieceRenderer.RenderOffBoardPieces);

            AddSectionPadding(padding);

            currentY = RenderPlayerSection(
                currentY,
                "Player 2 (Blue) - Pieces off board:",
                gameState.Player2,
                PieceRenderer.RenderOffBoardPieces);
        }

        void RenderExitPiecesInfo()
        {
            AddSectionPadding(padding);
            RenderPlayerSection(
                currentY,
                "Player 1 (Red) - Pieces completed:",
                gameState.Player1,
                PieceRenderer.RenderCompletedPieces);

            AddSectionPadding(padding);
            RenderPlayerSection(
                currentY,
                "Player 2 (Blue) - Pieces completed:",
                gameState.Player2,
                PieceRenderer.RenderCompletedPieces);
        }
    }


    /// <summary>
    ///     Renders a section for a player's pieces (either off-board or completed)
    /// </summary>
    private int RenderPlayerSection(
        int currentY,
        string title,
        Player player,
        Action<int, int, Player> renderAction)
    {
        Console.SetCursorPosition(OffsetX, currentY);
        Console.WriteLine(title);

        currentY += 1;
        renderAction(OffsetX + 4, currentY, player);

        return currentY + 2;
    }

    /// <summary>
    ///     Renders a single row of the board
    /// </summary>
    private void RenderRow(CellType[,] board, int rowIndex, int cols)
    {
        DrawRowBorders(board, rowIndex, cols);
        Console.WriteLine();

        DrawRowContent(board, rowIndex, cols);

        DrawRowBorders(board, rowIndex, cols);
        Console.WriteLine();
    }

    /// <summary>
    ///     Draws the top/bottom borders for all cells in a row
    /// </summary>
    private void DrawRowBorders(CellType[,] board, int rowIndex, int cols)
    {
        IterateRow(board, rowIndex, cols, DrawCellBorder);
    }

    /// <summary>
    ///     Draws the content of all cells in a row
    /// </summary>
    private void DrawRowContent(CellType[,] board, int rowIndex, int cols)
    {
        for (var i = 0; i < LinesPerCell; i++)
        {
            var line = i;
            IterateRow(board, rowIndex, cols, cellType => WriteCellLine(cellType, line));
            Console.WriteLine();
        }
    }

    /// <summary>
    ///     Iterates through a row, handling direction based on the renderer's configuration
    /// </summary>
    private void IterateRow(CellType[,] board, int row, int cols, Action<CellType> cellAction)
    {
        if (isRightToLeft)
            for (var j = cols - 1; j >= 0; --j)
                cellAction(board[row, j]);
        else
            for (var j = 0; j < cols; ++j)
                cellAction(board[row, j]);
    }

    /// <summary>
    ///     Iterates through a row with column index
    /// </summary>
    private void IterateRow(CellType[,] board, int row, int cols, Action<CellType, int> cellAction)
    {
        if (isRightToLeft)
            for (var j = cols - 1; j >= 0; --j)
                cellAction(board[row, j], j);
        else
            for (var j = 0; j < cols; ++j)
                cellAction(board[row, j], j);
    }
}