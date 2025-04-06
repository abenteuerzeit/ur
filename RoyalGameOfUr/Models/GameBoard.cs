namespace RoyalGameOfUr.Models;

/// <summary>
///     Represents the game board
/// </summary>
public class GameBoard(CellType[,] boardData)
{
    /// <summary>
    ///     Gets the board data
    /// </summary>
    public CellType[,] GetBoardData()
    {
        return boardData;
    }

    /// <summary>
    ///     Creates a default game board
    /// </summary>
    public static GameBoard CreateDefault()
    {
        var boardData = new[,]
        {
            {
                CellType.Rosette, CellType.Plain, CellType.Eye, CellType.Plain, CellType.Disabled, CellType.Disabled,
                CellType.Rosette, CellType.Dots
            },
            {
                CellType.ZigZag, CellType.Eye, CellType.Cross, CellType.Rosette, CellType.Eye, CellType.Cross,
                CellType.Plain, CellType.Eye
            },
            {
                CellType.Rosette, CellType.Plain, CellType.Eye, CellType.Plain, CellType.Disabled, CellType.Disabled,
                CellType.Rosette, CellType.Dots
            }
        };

        return new GameBoard(boardData);
    }
}