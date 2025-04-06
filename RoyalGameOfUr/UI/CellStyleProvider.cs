using RoyalGameOfUr.Models;
using RoyalGameOfUr.UI.Interfaces;

namespace RoyalGameOfUr.UI
{
    /// <summary>
    /// Provides styling for cells
    /// </summary>
    public class CellStyleProvider(IConsoleWrapper console) : ICellStyleProvider
    {
        private readonly IConsoleWrapper _console = console ?? throw new ArgumentNullException(nameof(console));

        /// <summary>
        /// Apply the appropriate style for a cell type
        /// </summary>
        public void ApplyCellStyle(CellType cellType)
        {
            switch (cellType)
            {
                case CellType.Rosette:
                    _console.SetBackgroundColor(ConsoleColor.DarkRed);
                    _console.SetForegroundColor(ConsoleColor.White);
                    break;

                case CellType.Plain:
                    _console.SetBackgroundColor(ConsoleColor.White);
                    _console.SetForegroundColor(ConsoleColor.Black);
                    break;

                case CellType.Eye:
                case CellType.Dots:
                case CellType.Cross:
                case CellType.ZigZag:
                    _console.SetBackgroundColor(ConsoleColor.White);
                    _console.SetForegroundColor(ConsoleColor.DarkBlue);
                    break;

                case CellType.Disabled:
                default:
                    _console.ResetColors();
                    break;
            }
        }

        /// <summary>
        /// Get the line content for a cell type
        /// </summary>
        public string GetCellLine(CellType cellType, int lineIndex)
        {
            if (cellType == CellType.Disabled)
            {
                return "       "; // Empty space for disabled cells
            }

            return cellType switch
            {
                CellType.Rosette => lineIndex == 1 ? "|  *  |" : "|     |",
                CellType.Plain => "|     |",
                CellType.Eye => lineIndex == 1 ? "|  O  |" : "|     |",
                CellType.Dots => lineIndex switch
                {
                    0 => "| o o |",
                    1 => "|  o  |",
                    _ => "| o o |"
                },
                CellType.Cross => lineIndex switch
                {
                    0 => "| \\ / |",
                    1 => "|  X  |",
                    _ => "| / \\ |"
                },
                CellType.ZigZag => lineIndex == 1 ? @"| /\/\|" : "|     |",
                _ => "|     |"
            };
        }
    }
}