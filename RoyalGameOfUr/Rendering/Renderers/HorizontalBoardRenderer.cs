using RoyalGameOfUr.Models;
using RoyalGameOfUr.UI.Interfaces;

namespace RoyalGameOfUr.Rendering.Renderers
{
    /// <summary>
    /// Renders the game board in a horizontal orientation
    /// </summary>
    public class HorizontalBoardRenderer(
        IConsoleWrapper console,
        ICellStyleProvider cellStyleProvider,
        bool isRightToLeft)
        : BoardRenderer(console, cellStyleProvider)
    {
        /// <summary>
        /// Renders the board in horizontal orientation
        /// </summary>
        public override void RenderBoard(CellType[,] board)
        {
            var rows = board.GetLength(0);
            var cols = board.GetLength(1);

            for (var rowIndex = 0; rowIndex < rows; rowIndex++)
            {
                RenderRow(board, rowIndex, cols);
            }
        }

        /// <summary>
        /// Renders a single row of the board
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
        /// Draws the top/bottom borders for all cells in a row
        /// </summary>
        private void DrawRowBorders(CellType[,] board, int rowIndex, int cols)
        {
            IterateRow(board, rowIndex, cols, DrawCellBorder);
        }

        /// <summary>
        /// Draws the content of all cells in a row
        /// </summary>
        private void DrawRowContent(CellType[,] board, int rowIndex, int cols)
        {
            const int linesPerCell = 3;

            for (var i = 0; i < linesPerCell; i++)
            {
                var line = i;
                IterateRow(board, rowIndex, cols, cellType => WriteCellLine(cellType, line));
                Console.WriteLine();
            }
        }

        /// <summary>
        /// Iterates through a row, handling direction based on the renderer's configuration
        /// </summary>
        private void IterateRow(CellType[,] board, int row, int cols, Action<CellType> cellAction)
        {
            if (isRightToLeft)
            {
                for (var j = cols - 1; j >= 0; --j)
                {
                    cellAction(board[row, j]);
                }
            }
            else
            {
                for (var j = 0; j < cols; ++j)
                {
                    cellAction(board[row, j]);
                }
            }
        }
    }
}