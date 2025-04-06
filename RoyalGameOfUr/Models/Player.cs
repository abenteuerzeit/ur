namespace RoyalGameOfUr.Models;

/// <summary>
///     Represents a player in the game
/// </summary>
public class Player
{
    /// <summary>
    ///     Creates a new player
    /// </summary>
    public Player(int number, ConsoleColor color, int[,] path)
    {
        Number = number;
        Color = color;
        Path = path;

        // Initialize 7 pieces for this player
        Pieces = new GamePiece[7];
        for (var i = 0; i < 7; i++) Pieces[i] = new GamePiece(this);
    }

    /// <summary>
    ///     Gets the player number (1 or 2)
    /// </summary>
    public int Number { get; }

    /// <summary>
    ///     Gets the player's color
    /// </summary>
    public ConsoleColor Color { get; }

    /// <summary>
    ///     Gets the player's pieces
    /// </summary>
    public GamePiece[] Pieces { get; }

    /// <summary>
    ///     Gets the player's path on the board
    /// </summary>
    public int[,] Path { get; }

    /// <summary>
    ///     Gets the row and column on the board for a given path position
    /// </summary>
    public (int Row, int Col) GetBoardCoordinates(int pathPosition)
    {
        // If off board, return invalid coordinates
        if (pathPosition is < 1 or > 16) return (-1, -1);

        // Search for the position in the path
        var rows = Path.GetLength(0);
        var cols = Path.GetLength(1);

        for (var row = 0; row < rows; row++)
            for (var col = 0; col < cols; col++)
                if (Path[row, col] == pathPosition)
                    return (row, col);

        // Should never reach here if path is correctly defined
        return (-1, -1);
    }

    /// <summary>
    ///     Returns the count of pieces that have completed the path
    /// </summary>
    public int GetCompletedPiecesCount()
    {
        return Pieces.Count(p => p.HasExited);
    }

    /// <summary>
    ///     Returns the count of pieces still off the board
    /// </summary>
    public int GetOffBoardPiecesCount()
    {
        return Pieces.Count(p => p.Position == -1);
    }
}