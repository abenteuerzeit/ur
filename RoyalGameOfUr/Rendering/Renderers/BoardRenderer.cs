using RoyalGameOfUr.Models;
using RoyalGameOfUr.Rendering.Interfaces;
using RoyalGameOfUr.UI.Interfaces;

namespace RoyalGameOfUr.Rendering.Renderers
{
    /// <summary>
    /// Abstract base class for board renderers
    /// </summary>
    public abstract class BoardRenderer : IBoardRenderer
    {
        /// <summary>
        /// Console wrapper for output
        /// </summary>
        protected readonly IConsoleWrapper Console;

        /// <summary>
        /// Cell style provider for appearance
        /// </summary>
        private readonly ICellStyleProvider _cellStyleProvider;

        /// <summary>
        /// Initializes a new instance of the board renderer
        /// </summary>
        protected BoardRenderer(IConsoleWrapper console, ICellStyleProvider cellStyleProvider)
        {
            Console = console ?? throw new ArgumentNullException(nameof(console));
            _cellStyleProvider = cellStyleProvider ?? throw new ArgumentNullException(nameof(cellStyleProvider));
        }

        /// <summary>
        /// Renders the game board
        /// </summary>
        public abstract void RenderBoard(CellType[,] board);

        /// <summary>
        /// Draws a border for a cell of the specified type
        /// </summary>
        protected void DrawCellBorder(CellType cellType)
        {
            if (cellType == CellType.Disabled)
            {
                Console.Write("       ");
                return;
            }

            _cellStyleProvider.ApplyCellStyle(cellType);
            Console.Write("+-----+");
            Console.ResetColors();
        }

        /// <summary>
        /// Writes a cell line with appropriate styling
        /// </summary>
        protected void WriteCellLine(CellType cellType, int line)
        {
            _cellStyleProvider.ApplyCellStyle(cellType);
            Console.Write(_cellStyleProvider.GetCellLine(cellType, line));
            Console.ResetColors();
        }
    }
}