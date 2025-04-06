namespace RoyalGameOfUr.Models;

/// <summary>
///     Represents a player's game piece
/// </summary>
public class GamePiece
{
    /// <summary>
    ///     Creates a new game piece
    /// </summary>
    public GamePiece(Player owner)
    {
        Owner = owner;
        Position = -1; // Start off the board
    }

    /// <summary>
    ///     Gets the player who owns this piece
    /// </summary>
    public Player Owner { get; }

    /// <summary>
    ///     Gets or sets the current position on the path (1-16, or -1 if off board)
    /// </summary>
    public int Position { get; set; }

    /// <summary>
    ///     Returns true if the piece is on the board
    /// </summary>
    public bool IsOnBoard => Position is > 0 and <= 16;

    /// <summary>
    ///     Returns true if the piece has completed the path and exited the board
    /// </summary>
    public bool HasExited => Position > 16;
}