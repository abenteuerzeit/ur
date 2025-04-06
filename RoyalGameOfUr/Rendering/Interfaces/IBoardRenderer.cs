using RoyalGameOfUr.Models;

namespace RoyalGameOfUr.Rendering.Interfaces
{
    /// <summary>
    /// Interface for board rendering strategies
    /// </summary>
    public interface IBoardRenderer
    {
        /// <summary>
        /// Renders the game board to the console
        /// </summary>
        /// <param name="board">The cell type board to render</param>
        void RenderBoard(CellType[,] board);
    }
}