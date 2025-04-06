using RoyalGameOfUr.Models;

namespace RoyalGameOfUr.Rendering.Interfaces;

/// <summary>
///     Interface for board rendering strategies
/// </summary>
public interface IBoardRenderer
{
    /// <summary>
    ///     Renders the game board to the console
    /// </summary>
    /// <param name="board">The cell type board to render</param>
    void RenderBoard(CellType[,] board);

    /// <summary>
    ///     Renders the game board with pieces
    /// </summary>
    /// <param name="gameState">The current game state</param>
    void RenderGameState(GameState gameState);
}